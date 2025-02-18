using System.Text;
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
        const string clientId = "b811105c-5db5-4b19-9db8-7e01ac914ac9";
        const string tenantId = "7eaaa2e0-ab22-47ef-a037-5e216f661c16";
        const string clientSecret = "9Gt8Q~vDVttzhKKvbAPqSIgzAYDsaq4bF8cG-a_a"; // Store this securely!
        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _graphClient = new GraphServiceClient(clientSecretCredential, new[] { "https://graph.microsoft.com/.default" });
    }
    
    //get user by last name
    public async Task<IEnumerable<User>?> GetUsersByLastNameAsync(string lastName)
    {
        var results = await _graphClient.Users.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = $"endswith(mail, 'gi-tec.net') and surname eq '{lastName}'";
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Select = new string []{ "displayName", "givenName", "surname", "jobTitle", "department", "employeeType" };
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });

        return results?.Value;
    }
    
    public async Task<IEnumerable<User>?> GetAllStaffAsync()
    {
        List<User> Users = [];
        var results = await _graphClient.Users.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = "endswith(mail, 'gi-tec.net') and department eq 'Staff'";
            requestConfiguration.QueryParameters.Orderby = new string []{ "userPrincipalName" };
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Select = new string []{ "displayName", "givenName", "surname", "jobTitle", "department", "employeeType" };
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });

        if (results == null)
        {
            return Users;
        }

        if (results.Value != null) Users.AddRange(results.Value);

        var pageIterator = PageIterator<User, UserCollectionResponse>.CreatePageIterator(
            _graphClient,
            results,
            (user) =>
            {
                Users.Add(user);
                return true;
            });

        await pageIterator.IterateAsync();

        return Users;


    }
    
    public async Task<IEnumerable<User>?> GetAllStudentsAsync()
    {
        List<User> Users = [];
        var results = await _graphClient.Users.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = "endswith(mail, 'gi-tec.net') and jobTitle eq 'Student'";
            requestConfiguration.QueryParameters.Orderby = new string []{ "userPrincipalName" };
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Select = new string []{ "displayName", "givenName", "surname", "jobTitle", "department", "employeeType" };
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });

        if (results == null)
        {
            return Users;
        }

        if (results.Value != null) Users.AddRange(results.Value);

        var pageIterator = PageIterator<User, UserCollectionResponse>.CreatePageIterator(
            _graphClient,
            results,
            (user) =>
            {
                Users.Add(user);
                return true;
            });

        await pageIterator.IterateAsync();

        return Users;


    }
    
    public async Task<IEnumerable<User>?> GetAllCurrentStudentsAsync()
    {
        
        //for loop from this current year for 5 more
        List<string> sb = new List<string>();
        for (var year = DateTime.Now.Year; year <= DateTime.Now.Year + 5; year++)
        {
            sb.Add($"department eq {year.ToString()}");
        }

        var years = $"( {string.Join(" or ", sb)} )";
        
        
        
        
        List<User> Users = [];
        var results = await _graphClient.Users.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Filter = "endswith(mail, 'gi-tec.net') and jobTitle eq 'Student' and " + years;
            requestConfiguration.QueryParameters.Orderby = new string []{ "userPrincipalName" };
            requestConfiguration.QueryParameters.Count = true;
            requestConfiguration.QueryParameters.Select = new string []{ "displayName", "givenName", "surname", "jobTitle", "department", "employeeType" };
            requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
        });

        if (results == null)
        {
            return Users;
        }

        if (results.Value != null) Users.AddRange(results.Value);

        var pageIterator = PageIterator<User, UserCollectionResponse>.CreatePageIterator(
            _graphClient,
            results,
            (user) =>
            {
                Users.Add(user);
                return true;
            });

        await pageIterator.IterateAsync();

        return Users;


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
}