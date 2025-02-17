using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Client.AspNetCore;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Swagger for API documentation.
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:7285/connect/authorize"),
                TokenUrl = new Uri("https://localhost:7285/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "api", "resource server scope" }
                }
            }
        }
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer("https://localhost:7285");
        options.AddAudiences("resource_server_api");

        options.AddEncryptionKey(
            new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["SymmetricSecurityKey"]!)));

        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.OAuthClientId("web-client");
    c.OAuthClientSecret("906879F1-A422-466F-8946-3D514066F6B1");
    c.OAuthScopes("api");
});

app.UseCors("AllowMyOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();