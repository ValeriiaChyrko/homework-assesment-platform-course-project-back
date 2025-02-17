using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace HomeAssignment.Database.Contexts.Implementations;

public class ClientSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public ClientSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task AddScopesAsync()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        
        var apiScope = await manager.FindByNameAsync("api");

        if (apiScope != null)
        {
            await manager.DeleteAsync(apiScope);
        }
        
        await manager.CreateAsync(new OpenIddictScopeDescriptor
        {
            DisplayName = "Api scope",
            Name = "api",
            Resources =
            {
                "resource_server_api"
            }
        });
    }

    public async Task AddClientAsync()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        
        var context = scope.ServiceProvider.GetRequiredService<HomeworkAssignmentDbContext>();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var client = await manager.FindByClientIdAsync("web-client");

        if (client != null)
        {
            await manager.DeleteAsync(client);
        }

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "web-client",
            DisplayName = "Web Application Client",
            ClientSecret = "906879F1-A422-466F-8946-3D514066F6B1",
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            RedirectUris =
            {
                new Uri("https://localhost:7002/swagger/oauth2-redirect.html"),
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://localhost:7002/callback/logout/local"),
            },
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization,
                OpenIddictConstants.Permissions.Endpoints.EndSession,
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.ResponseTypes.Code,
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.Roles,
                $"{OpenIddictConstants.Permissions.Prefixes.Scope}api"
            }
        });
        
        var reactClient = await manager.FindByClientIdAsync("react-client");
        if (reactClient != null)
        {
            await manager.DeleteAsync(reactClient);
        }

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "react-client",
            ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = "React client application",
            RedirectUris =
            {
                new Uri("https://localhost:5173/oauth/callback")
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://localhost:5173/")
            },
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Authorization,
                OpenIddictConstants.Permissions.Endpoints.EndSession,
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.ResponseTypes.Code,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles,
                $"{OpenIddictConstants.Permissions.Prefixes.Scope}api"
            }
        });
    }
}