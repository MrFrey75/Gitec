using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
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
    public string OfficeLocation { get; set; } = string.Empty;
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
    public int GraduationYear => int.TryParse(Department, out int year) ? year : 0;
    public string SendingSchool => string.IsNullOrEmpty(City) ? "Unknown" : City; 
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