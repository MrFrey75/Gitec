using System.ComponentModel.DataAnnotations.Schema;
using TecCore.DataStructs;

namespace TecCore.Models;

public class GitecUser : BaseEntity
{
    public PersonType PersonType { get; set; }
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string EmployeeType { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
    public string MailNickname { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string LegalAgeGroupClassification { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public bool AccountEnabled { get; set; } = true;
    public bool ConsentProvidedForMinor { get; set; } = false;

    
    
    
    [NotMapped]
    public string FirstName => GivenName;
    [NotMapped]
    public string LastName => Surname;
    [NotMapped]
    public string FullName => DisplayName;
    [NotMapped]
    public string SearchKey => $"{FirstName} {LastName} {JobTitle} {Department} {CompanyName}".ToLower();
    
    
    
    
    

}

public class Student : GitecUser
{
 
    public Student()
    {
        PersonType = PersonType.Student;
    }
}

public class FacultyMember : GitecUser
{
    public FacultyMember()
    {
        PersonType = PersonType.Faculty;
    }
}

public class StaffMember : GitecUser
{
    public StaffMember()
    {
        PersonType = PersonType.Staff;
    }
}

public class Paraprofessional  : GitecUser
{
    public Paraprofessional()
    {
        PersonType = PersonType.Paraprofessional;
    }
}