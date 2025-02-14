using HomeAssignment.Database.Contexts.Implementations; 
using HomeworkAssignment.Services; 
using HomeworkAssignment.Services.Abstractions; 
using OpenIddict.Validation.AspNetCore;
using RepoAnalisys.Grpc; 

namespace HomeworkAssignment;

public static class DependencyInjection
{
    public static void AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register gRPC service interfaces and their implementations
        services.AddScoped<IAccountGrpcService, AccountGrpcService>();
        services.AddScoped<ICompilationGrpcService, CompilationGrpcService>();
        services.AddScoped<IQualityGrpcService, QualityGrpcService>();
        services.AddScoped<ITestsGrpcService, TestsGrpcService>();

        var grpcBaseUrl = configuration
            .GetRequiredSection("GrpcSettings")
            .GetValue<string>("BaseUrl");

        // Register gRPC clients with the specified base URL
        services.AddGrpcClient<AccountsOperator.AccountsOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
        services.AddGrpcClient<CompilationOperator.CompilationOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
        services.AddGrpcClient<QualityOperator.QualityOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
        services.AddGrpcClient<TestsOperator.TestsOperatorClient>(o => { o.Address = new Uri(grpcBaseUrl!); });
        
        var issuer = configuration
            .GetRequiredSection("OpenIddictSettings")
            .GetValue<string>("Issuer");

        // Register OpenIddict services
        services.AddOpenIddict()
            .AddCore(options => 
            {
                options.UseEntityFrameworkCore().UseDbContext<HomeworkAssignmentDbContext>();
            })
            .AddServer(options =>
            {
                // Set up the authorization and token endpoints
                options.SetAuthorizationEndpointUris("/connect/authorize")
                       .SetTokenEndpointUris("/connect/token")
                       .SetIntrospectionEndpointUris("/connect/introspect");
                
                options.AllowClientCredentialsFlow();
                options.AllowPasswordFlow();
                options.AllowAuthorizationCodeFlow()
                    .RequireProofKeyForCodeExchange(); 
                options.AllowRefreshTokenFlow();
                
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();
                options.SetIssuer(new Uri(issuer!));
                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.SetIssuer(new Uri(issuer!)); 
            });

        // Configure authentication to use OpenIddict validation
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        // Configure authorization policies
        services.AddAuthorizationBuilder()
            .AddPolicy("home_assignment_policy", policy =>
            {
                policy.RequireClaim("scope", "openid_profile_email");
            });
        
        services.AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
        {
            client.BaseAddress = new Uri(issuer!);
        });
        
        var clientId = configuration
            .GetRequiredSection("OpenIddictSettings")
            .GetValue<string>("ClientId");
        var clientSecret = configuration
            .GetRequiredSection("OpenIddictSettings")
            .GetValue<string>("ClientSecret");

        services.AddSingleton<IAuthenticationService>(provider =>
        {
            var httpClient = provider.GetRequiredService<HttpClient>();
            return new AuthenticationService(httpClient, "connect/token", clientId!, clientSecret!);
        });
    }
}