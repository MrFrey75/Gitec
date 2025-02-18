namespace TecCore.Models.Location;

public class RoomLocation : BaseEntity
{
    public int RoomNumber { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}