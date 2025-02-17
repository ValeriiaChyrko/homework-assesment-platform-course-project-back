using HomeworkAssignment.Services;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.OpenApi.Models;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment;

public static class DependencyInjection
{
    public static void AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountGrpcService, AccountGrpcService>();
        services.AddScoped<ICompilationGrpcService, CompilationGrpcService>();
        services.AddScoped<IQualityGrpcService, QualityGrpcService>();
        services.AddScoped<ITestsGrpcService, TestsGrpcService>();
        services.AddScoped<IKeycloakTokenService, KeycloakTokenService>();

        var grpcBaseUrl = configuration
            .GetRequiredSection("GrpcSettings")
            .GetValue<string>("BaseUrl");

        services.AddGrpcClient<AccountsOperator.AccountsOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
        services.AddGrpcClient<CompilationOperator.CompilationOperatorClient>(o =>
        {
            o.Address = new Uri(grpcBaseUrl!);
        });
        services.AddGrpcClient<QualityOperator.QualityOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
        services.AddGrpcClient<TestsOperator.TestsOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
    }

    public static void AddSwaggerGenWithAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(type => type.FullName!.Replace('+', '.'));
            o.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]!),
                        Scopes = new Dictionary<string, string> { { "openid", "openid" }, { "profile", "profile" } }
                    }
                }
            });

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Keycloak",
                            Type = ReferenceType.SecurityScheme
                        },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer"
                    },
                    []
                }
            };
            o.AddSecurityRequirement(securityRequirement);
        });
    }
}