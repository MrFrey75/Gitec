using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace TecCore.Services.Entra
{
    public class UserService : MsGraphService
    {
        public UserService() : base() { }

                protected async Task<IEnumerable<User>> FetchUsersAsync(string? filter = null)
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

                    var results = await _graphClient.Users.GetAsync(request =>
                    {
                        request.QueryParameters.Filter = _filter;
                        request.QueryParameters.Orderby = new[] { "userPrincipalName" };
                        request.QueryParameters.Count = true;
                        request.QueryParameters.Select = UserProperties;
                        request.Headers.Add("ConsistencyLevel", "eventual");
                    });

                    if (results?.Value != null)
                        users.AddRange(results.Value);

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

                    break; // Success; exit retry loop
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

                await Task.Delay(delayMilliseconds);
                delayMilliseconds *= 2; // Exponential backoff
            }

            if (users.Count == 0)
                Console.WriteLine("[Warning] No users were fetched. Returning an empty list.");

            return users;
        }
        
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
    }
}
