using TecCore.Models.Location;
using TecCore.Models.People;

namespace TecCore.Models.Course;

public class EduProgram : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Classroom Classroom { get; set; } = new();
    public List<Lab> Labs { get; set; } = [];
    public Instructor Instructor { get; set; } = new();
    public List<ParaPro> ParaPros { get; set; } = [];
}