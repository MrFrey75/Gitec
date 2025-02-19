using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using TecCore.Utilities;

namespace TecCore.Services.Google;

public class UserService
{
    private readonly DirectoryService _directoryService;
    
    public UserService(IDirectoryServiceFactory factory)
    {
        _directoryService = factory.Create();
    }
    
    // ===========================
    //       USER MANAGEMENT
    // ===========================
    
    public async Task<User?> GetUserAsync(string userEmail)
    {
        try
        {
            return await _directoryService.Users.Get(userEmail).ExecuteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user {userEmail}: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> UpdateUserAsync(User user)
    {
        try
        {
            await _directoryService.Users.Update(user, user.PrimaryEmail).ExecuteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user {user.PrimaryEmail}: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> ChangeUserPasswordAsync(string userEmail, string newPassword)
    {
        try
        {
            var user = new User { Password = newPassword };
            await _directoryService.Users.Update(user, userEmail).ExecuteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing password for {userEmail}: {ex.Message}");
            return false;
        }
    }
            // GI-TEC
            // /GI-TEC/Staff
            // /GI-TEC/Staff/Admin
            // /GI-TEC/Staff/Faculty
            // /GI-TEC/Students
            // /GI-TEC/Students/2024
            // /GI-TEC/Students/2025
            // /GI-TEC/Students/2026
            // /GI-TEC/Students/2027
            // /GI-TEC/Students/2028
    
            public async Task<IEnumerable<User>> GetUsersByOrgUnitAsync(string orgUnitPath)
            {
                var allUsers = new List<User>();
                try
                {
                    var request = _directoryService.Users.List();

                    // Instead of setting request.OrgUnitPath, you use 'query' for older library versions
                    // For example: orgUnitPath='/GI-TEC/Staff/Admin'
                    request.Query = $"orgUnitPath='{orgUnitPath}'";

                    // Typically, you should set either request.Customer or request.Domain
                    // request.Customer = "my_customer";
                    request.Domain = "gi-tec.net"; 

                    string pageToken = null;
                    do
                    {
                        request.PageToken = pageToken;
                        var response = await request.ExecuteAsync();
                        if (response.UsersValue != null)
                        {
                            allUsers.AddRange(response.UsersValue);
                        }
                        pageToken = response.NextPageToken;
                    }
                    while (!string.IsNullOrEmpty(pageToken));

                    return allUsers;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching users from {orgUnitPath}: {ex.Message}");
                    return new List<User>();
                }
            }


    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var allUsers = new List<User>();
        try
        {
            var request = _directoryService.Users.List();
            request.Domain = "gi-tec.net";
            string pageToken = null;

            do
            {
                request.PageToken = pageToken;
                var response = await request.ExecuteAsync();
                if (response.UsersValue != null)
                {
                    allUsers.AddRange(response.UsersValue);
                }
                pageToken = response.NextPageToken;
            }
            while (!string.IsNullOrEmpty(pageToken));

            return allUsers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
            return new List<User>();
        }
    }


}
