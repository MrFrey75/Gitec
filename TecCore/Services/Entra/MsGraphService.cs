using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace TecCore.Services.Entra
{
    public class MsGraphService
    {
        protected readonly GraphServiceClient _graphClient;

        protected static readonly string[] UserProperties =
        {
            "displayName", "id", "givenName", "surname", "jobTitle", "department",
            "employeeType", "userType", "userPrincipalName", "mailNickname", "mail",
            "legalAgeGroupClassification", "jobTitle", "employeeType", "department",
            "createdDateTime", "consentProvidedForMinor", "companyName", "city", "ageGroup",
            "accountEnabled"
        };

        protected static readonly string[] DeviceProperties = new[]
        {
            "displayName", "id", "operatingSystem", "operatingSystemVersion", "isManaged",
            "enrolledDateTime", "lastSyncDateTime", "deviceCategory", "Manufacturer", "Model"
        };

        protected static readonly string[] GroupProperties = new[]
        {
            "displayName", "id", "CreatedDateTime", "Description", "MailNickname", "Members",
            "MembershipRule", "Settings"
        };

        public MsGraphService()
        {
            // Normally you might get these from environment variables.
            var clientId = "b811105c-5db5-4b19-9db8-7e01ac914ac9";
            var tenantId = "7eaaa2e0-ab22-47ef-a037-5e216f661c16";
            var clientSecret = "cag8Q~AbVLyZ8wW5w7M2qkKTkmz7CVANb0bxtayP";

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            _graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
        }

    }
}
