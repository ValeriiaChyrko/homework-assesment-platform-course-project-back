// Стандартні бібліотеки
using HomeworkAssignment;
using HomeworkAssignment.Application;
using HomeworkAssignment.Extensions;
using HomeAssignment.Database;
using HomeAssignment.Database.Contexts.Implementations;
using HomeAssignment.DTOs;
using HomeAssignment.Persistence;

// Зовнішні бібліотеки
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDtosServices();
builder.Services.AddDatabaseServices();
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddGrpcServices(builder.Configuration);
builder.Services.AddOpenIddictServices();

// Configure CORS policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", p => { p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
    options.AddDefaultPolicy(policy => 
    {
        policy.WithOrigins("https://localhost:7002")
            .AllowAnyHeader();

        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader();
    });
});

// Configure Swagger for API documentation.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeworkAssignment API v1", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configure logging with Serilog.
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ClientSeeder>();
    seeder.AddClientAsync().GetAwaiter().GetResult();
    seeder.AddScopesAsync().GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeworkAssignment API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting(); 

app.UseCors("AllowMyOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();
app.UseErrorHandler();

app.MapControllers();

app.Run();
