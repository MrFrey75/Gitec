using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace TecCore.Services.Google;

public class OrgUnitService
{
    private readonly DirectoryService _directoryService;

    public OrgUnitService(IDirectoryServiceFactory factory)
    {
        _directoryService = factory.Create();
    }

    public async Task<IEnumerable<OrgUnit>> GetAllOrgUnitsAsync()
    {
        try
        {
            // "my_customer" is a special alias for your account's customer ID
            var request = _directoryService.Orgunits.List("my_customer");

            // Use the enum value instead of a string
            request.Type = OrgunitsResource.ListRequest.TypeEnum.All;

            // The root OU path is "/" so we start from there
            request.OrgUnitPath = "/GI-TEC";

            // Execute the request
            var response = await request.ExecuteAsync();

            // Return the list of OUs, or an empty list if null
            return response.OrganizationUnits ?? new List<OrgUnit>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching OUs: {ex.Message}");
            return new List<OrgUnit>();
        }
    }
}