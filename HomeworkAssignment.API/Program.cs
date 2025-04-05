using System.Security.Claims;
using HomeAssignment.Database;
using HomeAssignment.DTOs;
using HomeAssignment.Persistence;
using HomeworkAssignment;
using HomeworkAssignment.Application;
using HomeworkAssignment.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtosServices();
builder.Services.AddDatabaseServices();
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddGrpcServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

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
            ClockSkew = TimeSpan.Zero, 
            RoleClaimType = "role"
        };
    });

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("HomeworkAssignment.Api"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddGrpcClientInstrumentation();
        tracing.AddOtlpExporter();
    });

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(1),
        Expiration = TimeSpan.FromMinutes(5)
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeworkAssignment API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowFrontend");
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