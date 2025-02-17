using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Contexts.Implementations;
using HomeworkAssignment.Services;
using HomeworkAssignment.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
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

        services.AddTransient<AuthorizationService>();

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

    public static void AddOpenIddictServices(this IServiceCollection services)
    {
        services.AddScoped<HomeworkAssignmentDbContext>(provider =>
        {
            var contextFactory = provider.GetService<IHomeworkAssignmentDbContextFactory>();
            return contextFactory!.CreateDbContext();
        });
        
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<HomeworkAssignmentDbContext>();
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("/connect/authorize");
                options.SetEndSessionEndpointUris("/connect/logout");
                options.SetTokenEndpointUris("/connect/token");
                
                options.RegisterScopes(
                    OpenIddictConstants.Permissions.Scopes.Email, 
                    OpenIddictConstants.Permissions.Scopes.Profile, 
                    OpenIddictConstants.Permissions.Scopes.Roles);
                
                options.AllowAuthorizationCodeFlow();
                
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
                
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();
                
                options.UseAspNetCore()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(c =>
            {
                c.LoginPath = "/Authenticate";
            });
    }
}