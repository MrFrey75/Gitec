using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models;

public class UserProfile : BaseEntity
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty; 
    public string EmployeeType { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

}

public class Student : UserProfile
{
    [MaxLength(4)]
    public string GraduationYear { get; set; } = string.Empty;
    [MaxLength(100)]
    public string SendingSchool { get; set; } = string.Empty;
    [MaxLength(255)]
    public string HomeEmail { get; set; } = string.Empty;
    public bool IsMinor { get; set; } = false;
    public bool ParentConsent { get; set; } = false;
    
    
    public Student()
    {
        EmployeeType = PersonType.Student.ToString(); 
        JobTitle = PersonType.Student.ToString(); 
    }
}

public class gStudent : Student
{
    [MaxLength(100)]
    public string OrgUnitPath { get; set; } = string.Empty;
}

public class eStudent : Student
{
    public eStudent() { }
}

public class Faculty : UserProfile
{
    [MaxLength(50)]
    public string ClassroomLocation { get; set; } = string.Empty;
    
    public Faculty()
    {
        EmployeeType = PersonType.Faculty.ToString(); 
    }
}

public class gFaculty : Faculty
{
    [MaxLength(50)]
    public string OrgUnitPath { get; set; } = string.Empty;
}

public class eFaculty : Faculty { }

public class Staff : UserProfile
{
    public string OfficeLocation { get; set; } = string.Empty;
    public Staff()
    {
        EmployeeType = PersonType.Staff.ToString(); 
    }
}

public class gStaff : Staff
{
    [MaxLength(50)]
    public string OrgUnitPath { get; set; } = string.Empty;
    public gStaff() { }
}

public class eStaff : Staff
{
    public eStaff() { }
}
