using HomeAssignment.Database;
using HomeAssignment.DTOs;
using HomeAssignment.Persistence;
using HomeworkAssignment;
using HomeworkAssignment.Application;
using HomeworkAssignment.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddGrpcServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddDatabaseServices();
builder.Services.AddDtosServices();
builder.Services.AddPersistenceServices();

// Configure CORS policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", p => { p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
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

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeworkAssignment API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowMyOrigin");
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseErrorHandler();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();