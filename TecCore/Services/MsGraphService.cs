using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace TecCore.Services;

public interface IMsGraphService
{
    Task<IEnumerable<User>?> GetUsersAsync();
    Task<IEnumerable<Group>?> GetGroupsAsync();
    Task<IEnumerable<Device>?> GetDevicesAsync();
    Task<Device?> GetDeviceAsync(string deviceId);
    Task<User?> GetUserAsync(string userId);
    Task<Group?> GetGroupAsync(string groupId);
}

public class MsGraphService : IMsGraphService
{
    private readonly GraphServiceClient _graphClient;

    public MsGraphService()
    {
        const string clientId = "b811105c-5db5-4b19-9db8-7e01ac914ac9";
        const string tenantId = "7eaaa2e0-ab22-47ef-a037-5e216f661c16";
        const string clientSecret = "9Gt8Q~vDVttzhKKvbAPqSIgzAYDsaq4bF8cG-a_a"; // Store this securely!

        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        _graphClient = new GraphServiceClient(clientSecretCredential, new[] { "https://graph.microsoft.com/.default" });
    }

    public async Task<IEnumerable<User>?> GetUsersAsync()
    {
        var result = await _graphClient.Users.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = "endswith(mail,'@gi-tec.net')";
            requestConfiguration.QueryParameters.Orderby = new string []{ "userPrincipalName" };
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });
        
        return result?.Value;
    }
    

    

    public async Task<IEnumerable<Group>?> GetGroupsAsync()
    {
        var result = await _graphClient.Groups.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = "startswith(displayName, 'GITEC-')";
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Top = 1;
            requestConfiguration.QueryParameters.Orderby = new string []{ "displayName" };
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });

        
        return result?.Value;
    }

    public async Task<IEnumerable<Device>?> GetDevicesAsync()
    {
        var result = await _graphClient.Devices.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = "startswith(displayName, 'CTE')";
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Top = 1;
            requestConfiguration.QueryParameters.Orderby = new string []{ "displayName" };
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });

        
        return result?.Value;
    }

    public async Task<Device?> GetDeviceAsync(string deviceId)
    {
        return await _graphClient.Devices[deviceId].GetAsync();
    }

    public async Task<User?> GetUserAsync(string userId)
    {
        return await _graphClient.Users[userId].GetAsync();
    }

    public async Task<Group?> GetGroupAsync(string groupId)
    {
        return await _graphClient.Groups[groupId].GetAsync();
    }
}