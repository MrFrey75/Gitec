using TecCore.Models;

namespace TecCore.DataStructs;

public class EdCourse
{
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;

    public RoomLocation? Classroom { get; set; }
    public List<FacultyMember> Faculty { get; set; } = [];
    public List<Student> Students { get; set; } = [];
    
    public void AddFaculty(IEnumerable<FacultyMember> faculty)
    {
        this.Faculty.AddRange(faculty);
    }
    public void AddFaculty(FacultyMember faculty)
    {
        this.Faculty.Add(faculty);
    }
    public void AddStudents(IEnumerable<Student> students)
    {
        this.Students.AddRange(students);
    }
    public void AddStudents(Student student)
    {
        this.Students.Add(student);
    }
    public void SetClassroom(RoomLocation room)
    {
        this.Classroom = room;
    }
    
    
    
    
    
}