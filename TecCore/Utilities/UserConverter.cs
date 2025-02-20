namespace TecCore.Utilities;

public static class UserConverter
{
    //create and extensions to Microsoft.Graph.Models.User to return TecCore.Models.UserMdl
    public static TecCore.Models.GitecUser ToUserMdl(this Microsoft.Graph.Models.User user)
    {
        return new TecCore.Models.GitecUser
        {
            DisplayName = user.DisplayName ?? string.Empty,
            Mail = user.Mail ?? string.Empty,
            UserPrincipalName = user.UserPrincipalName ?? string.Empty,
            Id = user.Id ?? string.Empty,
            GivenName = user.GivenName ?? string.Empty,
            Surname = user.Surname ?? string.Empty,
            JobTitle = user.JobTitle ?? string.Empty,
            Department = user.Department ?? string.Empty,
            EmployeeType = user.EmployeeType ?? string.Empty,
            City = user.City ?? string.Empty,
            CompanyName = user.CompanyName ?? string.Empty,
            MailNickname = user.MailNickname ?? string.Empty,
            AccountEnabled = user.AccountEnabled ?? true,
            LegalAgeGroupClassification = user.LegalAgeGroupClassification ?? string.Empty,
            ConsentProvidedForMinor = bool.TryParse(user.ConsentProvidedForMinor, out var consent) && consent,
        };
    }
    
    public static IEnumerable<TecCore.Models.GitecUser> ToUserMdl(this IEnumerable<Microsoft.Graph.Models.User> users)
    {
        return users.Select(user => user.ToUserMdl());
    }
    
    public static Microsoft.Graph.Models.User ToGraphUser(this TecCore.Models.GitecUser gitecUser)
    {
        return new Microsoft.Graph.Models.User
        {
            DisplayName = gitecUser.DisplayName,
            Mail = gitecUser.Mail,
            UserPrincipalName = gitecUser.UserPrincipalName,
            Id = gitecUser.Id,
            GivenName = gitecUser.GivenName,
            Surname = gitecUser.Surname,
            JobTitle = gitecUser.JobTitle,
            Department = gitecUser.Department,
            EmployeeType = gitecUser.EmployeeType,
            City = gitecUser.City,
            CompanyName = gitecUser.CompanyName,
            MailNickname = gitecUser.MailNickname,
            AccountEnabled = gitecUser.AccountEnabled,
            LegalAgeGroupClassification = gitecUser.LegalAgeGroupClassification,
            ConsentProvidedForMinor = gitecUser.ConsentProvidedForMinor.ToString()
        };
    }
    
    public static IEnumerable<Microsoft.Graph.Models.User> ToGraphUser(this IEnumerable<TecCore.Models.GitecUser> users)
    {
        return users.Select(user => user.ToGraphUser());
    }
    
}