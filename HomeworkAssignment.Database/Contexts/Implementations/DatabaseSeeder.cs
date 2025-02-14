using OpenIddict.Abstractions;

namespace HomeAssignment.Database.Contexts.Implementations;

public class DatabaseSeeder
{
    public static async Task SeedDataRepoAnalysis(IOpenIddictApplicationManager applicationManager)
    {
        var existingClient = await applicationManager.FindByClientIdAsync("repo_analysis_api");
        if (existingClient != null) return;

        var client = new OpenIddictApplicationDescriptor
        {
            ClientId = "repo_analysis_api",
            ClientSecret = "22&EtqkHqpE0WttPw4tgKcnMG",
            DisplayName = "Repo Analysis API",
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles
            },
            RedirectUris = { new Uri("http://localhost:7285/callback") },
            PostLogoutRedirectUris = { new Uri("http://localhost:7285") }
        };

        await applicationManager.CreateAsync(client);
    }
    
    public static async Task SeedDataHomeAssignment(IOpenIddictApplicationManager applicationManager)
    {
        var existingClient = await applicationManager.FindByClientIdAsync("repo_analysis_api");
        if (existingClient != null) return;

        var client = new OpenIddictApplicationDescriptor
        {
            ClientId = "home_assignment_front",
            ClientSecret = "!5FrEj30gB2$]Ip61z.77h7dH",
            DisplayName = "Home Assignment Frontend API",
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles
            },
            RedirectUris = { new Uri("http://localhost:7285/callback") },
            PostLogoutRedirectUris = { new Uri("http://localhost:7285") }
        };

        await applicationManager.CreateAsync(client);
    }
}