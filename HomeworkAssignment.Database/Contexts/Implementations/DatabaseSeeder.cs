using OpenIddict.Abstractions;

namespace HomeAssignment.Database.Contexts.Implementations;

public class DatabaseSeeder
{
    public static async Task SeedData(IOpenIddictApplicationManager applicationManager)
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
            }
        };

        await applicationManager.CreateAsync(client);
    }
}