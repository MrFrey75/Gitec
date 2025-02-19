using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using TecCore.DataStructs;

namespace TecCore.Services;

public class MsGraphService
{
    private readonly GraphServiceClient _graphClient;
    
    private static readonly string[] UserProperties =
    [
        "displayName", "id", "givenName", "surname", "jobTitle", "department", "employeeType", "userType", "userPrincipalName", "mailNickname", "mail", "legalAgeGroupClassification", "jobTitle", "employeeType", "department", "createdDateTime", "consentProvidedForMinor", "companyName", "city", "ageGroup", "accountEnabled"
    ];
    
    private static readonly string[] DeviceProperties = new[]
    {
        "displayName", "id", "operatingSystem", "operatingSystemVersion", "isManaged", "enrolledDateTime", "lastSyncDateTime", "deviceCategory", "Manufacturer", "Model"
    };
    
    private static readonly string[] GroupProperties = new[]
    {
        "displayName", "id", "CreatedDateTime", "Description", "MailNickname", "Members", "MembershipRule", "Settings"
    };

    public MsGraphService()
    {
        // var clientId = Environment.GetEnvironmentVariable("GRAPH_CLIENT_ID") 
        //                ?? throw new InvalidOperationException("Missing GRAPH_CLIENT_ID");
        // var tenantId = Environment.GetEnvironmentVariable("GRAPH_TENANT_ID") 
        //                ?? throw new InvalidOperationException("Missing GRAPH_TENANT_ID");
        // var clientSecret = Environment.GetEnvironmentVariable("GRAPH_CLIENT_SECRET") 
        //                    ?? throw new InvalidOperationException("Missing GRAPH_CLIENT_SECRET");

        var clientId = "b811105c-5db5-4b19-9db8-7e01ac914ac9";
        var objectId = "4ebab864-111e-4107-bf63-454854402bee";
        var tenantId = "7eaaa2e0-ab22-47ef-a037-5e216f661c16";
        var clientSecret = "cag8Q~AbVLyZ8wW5w7M2qkKTkmz7CVANb0bxtayP";


        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
    }

    // ===========================
    //       USER MANAGEMENT
    // ===========================

    /// <summary>
    /// Updates user information.
    /// </summary>
    public async Task<bool> UpdateUserAsync(User updatedUser)
    {
        try
        {
            await _graphClient.Users[updatedUser.Id].PatchAsync(updatedUser);
            return true;
        }
        catch (ODataError ex)
        {
            Console.WriteLine($"Error updating user {updatedUser.Id}: {ex.Error?.Message}");
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
        await FetchUsersAsync($"surname eq '{lastName}'");

    /// <summary>
    /// Retrieves all staff members.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllStaffAsync() =>
        await FetchUsersAsync("department eq 'Staff'");

    /// <summary>
    /// Retrieves all students.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllStudentsAsync() =>
        await FetchUsersAsync("jobTitle eq 'Student'");

    /// <summary>
    /// Retrieves all current students based on department year (current year + 5).
    /// </summary>
    public async Task<IEnumerable<User>> GetAllCurrentStudentsAsync()
    {
        var yearFilters = Enumerable.Range(DateTime.Now.Year, 6)
                                    .Select(y => $"department eq '{y}'");
        var filterQuery = $"jobTitle eq 'Student' and ({string.Join(" or ", yearFilters)})";

        return await FetchUsersAsync(filterQuery);
    }

    /// <summary>
    /// Retrieves a specific user by ID.
    /// </summary>
    public async Task<User?> GetUserAsync(string userId) =>
        await _graphClient.Users[userId].GetAsync();

    // ===========================
    //       GROUP MANAGEMENT
    // ===========================
    
    public async Task<IEnumerable<Group>> GetAllGroupsAsync() =>
        await FetchGroupsAsync("startswith(displayName, 'GITEC')");
    
    public async Task<Group> GetGroupByGroupNameAsync(string groupName)
    {
        var groups = await FetchGroupsAsync($"displayName eq '{groupName}'");
        return groups.ToArray()[0];
    }
    
    public async Task UpdateGroup(Group group)
    {
        await _graphClient.Groups[group.Id].PatchAsync(group);
    }

    // ===========================
    //       DEVICE MANAGEMENT
    // ===========================

    /// <summary>
    /// Retrieves all devices where displayName starts with "CTE".
    /// </summary>
    public async Task<IEnumerable<Device>> GetAllDevicesAsync() =>
        await FetchDevicesAsync();
    
    public async Task<IEnumerable<Device>> GetDevicesByAssetTagAsync(string assetTag) =>
        await FetchDevicesAsync($"endswith(displayName, '{assetTag}')");

    public async Task<IEnumerable<Device>> GetDevicesByClassroomAsync(string classroom)
    {
        var devices = await FetchDevicesAsync();
        var enumerable = devices as Device[] ?? devices.ToArray();
        return
            (enumerable.Length == 0)
                ? new List<Device>()
                : enumerable.Where(i => i.DisplayName!.Substring(4,3) == classroom);
    }

    public async Task<Device> GetDeviceByDeviceNameAsync(string deviceName)
    {
        var devices = await FetchDevicesAsync($"displayName eq '{deviceName}'");
        return devices.ToArray()[0];
    }
    
    public async Task UpdateDevice(Device device)
    {
        await _graphClient.Devices[device.Id].PatchAsync(device);
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
    /// Common method for fetching devices with Graph API.
    /// </summary>
    private async Task<IEnumerable<Device>> FetchDevicesAsync(string? filter = null)
    {
        var devices = new List<Device>();
        string _filter;
        try
        {
            _filter = string.IsNullOrEmpty(filter) 
                ? "startswith(displayName, 'CTE')" 
                : $"startswith(displayName, 'CTE') and ({filter})";
            var results = await _graphClient.Devices.GetAsync(request =>
            {
                request.QueryParameters.Filter = _filter;
                request.QueryParameters.Orderby = ["displayName"];
                request.QueryParameters.Count = true;
                request.QueryParameters.Select = DeviceProperties;
                request.Headers.Add("ConsistencyLevel", "eventual");
            });

            if (results?.Value != null)
                devices.AddRange(results.Value);

            if (results != null)
            {
                var pageIterator = PageIterator<Device, DeviceCollectionResponse>.CreatePageIterator(
                    _graphClient, results, device =>
                    {
                        devices.Add(device);
                        return true;
                    });

                await pageIterator.IterateAsync();
            }
        }
        catch (ODataError ex)
        {
            Console.WriteLine($"Error fetching devices: {ex.Error?.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error fetching devices: {ex.Message}");
        }

        return devices;
    }


    /// <summary>
    /// Common method for fetching groups with Graph API.
    /// </summary>
    private async Task<IEnumerable<Group>> FetchGroupsAsync(string? filter = null)
    {
        var groups = new List<Group>();
        string _filter;
        try
        {
            _filter = string.IsNullOrEmpty(filter) 
                ? "startswith(displayName, 'GITEC')" 
                : $"startswith(displayName, 'GITEC') and ({filter})";
            var results = await _graphClient.Groups.GetAsync(request =>
            {
                request.QueryParameters.Filter = _filter;
                request.QueryParameters.Orderby = ["displayName"];
                request.QueryParameters.Count = true;
                request.QueryParameters.Select = GroupProperties;
                request.Headers.Add("ConsistencyLevel", "eventual");
            });

            if (results?.Value != null)
                groups.AddRange(results.Value);

            if (results != null)
            {
                var pageIterator = PageIterator<Group, GroupCollectionResponse>.CreatePageIterator(
                    _graphClient, results, group =>
                    {
                        groups.Add(group);
                        return true;
                    });

                await pageIterator.IterateAsync();
            }
        }
        catch (ODataError ex)
        {
            Console.WriteLine($"Error fetching groups: {ex.Error?.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error fetching groups: {ex.Message}");
        }

        return groups;
    }
    
    


    /// <summary>
    /// Common method for fetching users with Graph API.
    /// </summary>
private async Task<IEnumerable<User>> FetchUsersAsync(string? filter = null)
{
    var users = new List<User>();
    const int retryCount = 3;
    var delayMilliseconds = 1000; // 1 second delay before retry

    for (var attempt = 1; attempt <= retryCount; attempt++)
    {
        string _filter;
        try
        {
            _filter = string.IsNullOrEmpty(filter) 
                ? "endswith(mail, 'gi-tec.net')" 
                : $"endswith(mail, 'gi-tec.net') and ({filter})";
            // Attempt to get users
            var results = await _graphClient.Users.GetAsync(request =>
            {
                request.QueryParameters.Filter = _filter;
                request.QueryParameters.Orderby = ["userPrincipalName"];
                request.QueryParameters.Count = true;
                request.QueryParameters.Select = UserProperties;
                request.Headers.Add("ConsistencyLevel", "eventual");
            });

            // Check if results contain any users
            if (results?.Value != null) users.AddRange(results.Value);

            // Iterate through pages if there are more results
            if (results != null)
            {
                var pageIterator = PageIterator<User, UserCollectionResponse>.CreatePageIterator(
                    _graphClient, results, user =>
                    {
                        users.Add(user);
                        return true;
                    });

                await pageIterator.IterateAsync();
            }

            // If successful, break out of the retry loop
            break;
        }
        catch (ODataError oDataError)
        {
            Console.WriteLine($"[Error] ODataError encountered: {oDataError.Error?.Message}");
            Console.WriteLine($"[Details] Code: {oDataError.Error?.Code}, Target: {oDataError.Error?.Target}");
            Console.WriteLine($"[Attempt {attempt} of {retryCount}] Retrying in {delayMilliseconds}ms...");
        }
        catch (TaskCanceledException taskCanceledEx)
        {
            Console.WriteLine($"[Error] Request timeout: {taskCanceledEx.Message}");
            Console.WriteLine($"[Attempt {attempt} of {retryCount}] Retrying in {delayMilliseconds}ms...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Unexpected error: {ex.Message}");
            Console.WriteLine($"[StackTrace] {ex.StackTrace}");
            Console.WriteLine($"[Attempt {attempt} of {retryCount}] Retrying in {delayMilliseconds}ms...");
        }

        // Wait before retrying
        await Task.Delay(delayMilliseconds);

        // Increase delay to provide exponential backoff
        delayMilliseconds *= 2;
    }

    if (users.Count == 0)
    {
        Console.WriteLine("[Warning] No users were fetched. Returning an empty list.");
    }

    return users;
}

}
