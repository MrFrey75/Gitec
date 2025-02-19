using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;

namespace TecCore.Utilities;

public static class GoogleHelper
{
    public static GoogleCredential GetCredential()
    {
        var scopes = new[] {
            DirectoryService.Scope.AdminDirectoryOrgunit,
            DirectoryService.Scope.AdminDirectoryUser
            // or if you only need read access:
            // DirectoryService.Scope.AdminDirectoryOrgunitReadonly
        };

        var credential = GoogleCredential.FromFile("gitec-auth.json")
            .CreateScoped(scopes)
            .CreateWithUser("af@gi-tec.net");




        return credential;
    }
}