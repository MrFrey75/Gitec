using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using TecCore.DataStructs;

namespace TecCore.Services;

public class MsGraphService
{
    private readonly GraphServiceClient _graphClient;

    public MsGraphService()
    {
        
        // get credentials from environment variable
        var clientId = Environment.GetEnvironmentVariable("GRAPH_CLIENT_ID");
        var tenantId = Environment.GetEnvironmentVariable("GRAPH_TENANT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("GRAPH_CLIENT_SECRET");

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
    }

    // ===========================
    //       USER MANAGEMENT
    // ===========================

    /// <summary>
    /// Updates user information.
    /// </summary>
    public async Task<bool> UpdateUserAsync(string userId, User updatedUser)
    {
        try
        {
            await _graphClient.Users[userId].PatchAsync(updatedUser);
            return true;
        }
        catch (ODataError ex)
        {
            Console.WriteLine($"Error updating user {userId}: {ex.Error?.Message}");
            return false;
        }
    }

    /// <summary>
    /// Changes the password of a user.
    /// </summary>
    public async Task<bool> ChangeUserPasswordAsync(string userId, string newPassword)
    {
        try
        {
            var passwordProfile = new PasswordProfile
            {
                Password = newPassword,
                ForceChangePasswordNextSignIn = true // Ensures user resets on login
            };

            var updatedUser = new User { PasswordProfile = passwordProfile };

            await _graphClient.Users[userId].PatchAsync(updatedUser);
            return true;
        }
        catch (ODataError ex)
        {
            Console.WriteLine($"Error changing password for user {userId}: {ex.Error?.Message}");
            return false;
        }
    }

    /// <summary>
    /// Retrieves users with a specified last name.
    /// </summary>
    public async Task<IEnumerable<User>> GetUsersByLastNameAsync(string lastName) =>
        await FetchUsersAsync($"endswith(mail, 'gi-tec.net') and surname eq '{lastName}'");

    /// <summary>
    /// Retrieves all staff members.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllStaffAsync() =>
        await FetchUsersAsync("endswith(mail, 'gi-tec.net') and department eq 'Staff'");

    /// <summary>
    /// Retrieves all students.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllStudentsAsync() =>
        await FetchUsersAsync("endswith(mail, 'gi-tec.net') and jobTitle eq 'Student'");

    /// <summary>
    /// Retrieves all current students based on department year (current year + 5).
    /// </summary>
    public async Task<IEnumerable<User>> GetAllCurrentStudentsAsync()
    {
        var yearFilters = Enumerable.Range(DateTime.Now.Year, 6)
                                    .Select(y => $"department eq '{y}'");
        var filterQuery = $"endswith(mail, 'gi-tec.net') and jobTitle eq 'Student' and ({string.Join(" or ", yearFilters)})";

        return await FetchUsersAsync(filterQuery);
    }

    /// <summary>
    /// Retrieves a specific user by ID.
    /// </summary>
    public async Task<User?> GetUserAsync(string userId) =>
        await _graphClient.Users[userId].GetAsync();

    // ===========================
    //       DEVICE MANAGEMENT
    // ===========================

    /// <summary>
    /// Retrieves all devices where displayName starts with "CTE".
    /// </summary>
    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        var result = await _graphClient.Devices.GetAsync(request =>
        {
            request.QueryParameters.Filter = "startswith(displayName, 'CTE')";
            request.QueryParameters.Count = true;
            request.QueryParameters.Top = 1;
            request.QueryParameters.Orderby = new[] { "displayName" };
            request.Headers.Add("ConsistencyLevel", "eventual");
        });

        return result?.Value ?? Enumerable.Empty<Device>();
    }

    /// <summary>
    /// Retrieves a specific device by ID.
    /// </summary>
    public async Task<Device?> GetDeviceAsync(string deviceId) =>
        await _graphClient.Devices[deviceId].GetAsync();

    // ===========================
    //       INTERNAL HELPERS
    // ===========================

    /// <summary>
    /// Common method for fetching users with Graph API.
    /// </summary>
    private async Task<IEnumerable<User>> FetchUsersAsync(string filter)
    {
        var users = new List<User>();
        var results = await _graphClient.Users.GetAsync(request =>
        {
            request.QueryParameters.Filter = filter;
            request.QueryParameters.Orderby = new[] { "userPrincipalName" };
            request.QueryParameters.Count = true;
            request.QueryParameters.Select = new[]
            {
                "displayName", "givenName", "surname", "jobTitle", "department", "employeeType"
            };
            request.Headers.Add("ConsistencyLevel", "eventual");
        });

        if (results?.Value != null) users.AddRange(results.Value);

        var pageIterator = PageIterator<User, UserCollectionResponse>.CreatePageIterator(
            _graphClient, results, user =>
            {
                users.Add(user);
                return true;
            });

        await pageIterator.IterateAsync();

        return users;
    }
}
