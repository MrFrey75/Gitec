using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Services;
using TecCore.Utilities;

namespace TecCore.Services.Google;

public interface IDirectoryServiceFactory
{
    DirectoryService Create();
}

public class DirectoryServiceFactory : IDirectoryServiceFactory
{
    public DirectoryService Create()
    {
        var credential = GoogleHelper.GetCredential().CreateWithUser("af@gi-tec.net");
        return new DirectoryService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "GITEC Admin API"
        });
    }
}