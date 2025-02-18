using TecCore.Models.People;

namespace TecCore.Models.Location;

public class Office : RoomLocation
{
    public string OfficeArea { get; set; } = string.Empty;
    public List<Staff> Staffs { get; set; } = [];
    
}
