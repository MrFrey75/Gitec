using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace TecCore.Services.Entra
{
    public class GroupService : MsGraphService
    {
        public GroupService() : base() { }
        
        protected async Task<IEnumerable<Group>> FetchGroupsAsync(string? filter = null)
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
                    request.QueryParameters.Orderby = new[] { "displayName" };
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
        /// Retrieves all groups where displayName starts with "GITEC".
        /// </summary>
        public async Task<IEnumerable<Group>> GetAllGroupsAsync() =>
            await FetchGroupsAsync("startswith(displayName, 'GITEC')");

        /// <summary>
        /// Retrieves a specific group by its name.
        /// </summary>
        public async Task<Group?> GetGroupByGroupNameAsync(string groupName)
        {
            var groups = await FetchGroupsAsync($"displayName eq '{groupName}'");
            return groups.FirstOrDefault();
        }

        /// <summary>
        /// Updates group information.
        /// </summary>
        public async Task UpdateGroup(Group group)
        {
            await _graphClient.Groups[group.Id].PatchAsync(group);
        }
    }
}