using System.Security.Claims;
using HomeAssignment.Database;
using HomeAssignment.DTOs;
using HomeAssignment.Persistence;
using HomeworkAssignment;
using HomeworkAssignment.Application;
using HomeworkAssignment.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDtosServices();
builder.Services.AddDatabaseServices();
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddGrpcServices(builder.Configuration);

// Configure CORS policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", p => { p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Configure Swagger for API documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

// Configure logging with Serilog.
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = builder.Configuration["Authorization:Audience"];
        o.MetadataAddress = builder.Configuration["Authorization:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authorization:ValidIssuer"],
            ValidAudience = builder.Configuration["Authorization:Audience"],
            RoleClaimType = "role"
        };
    });

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("Keycloak.Auth.Api"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
        tracing.AddOtlpExporter();
    });

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

app.MapControllers();
app.MapGet("users/me",
    (ClaimsPrincipal claimsPrincipal) =>
    {
        return claimsPrincipal.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
    }).RequireAuthorization();

app.Run();