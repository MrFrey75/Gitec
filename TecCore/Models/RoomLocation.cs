using System.ComponentModel.DataAnnotations;

namespace TecCore.Models;

public class RoomLocation : BaseEntity
{ 
    [MaxLength(5)]
    public string RoomNumber { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public List<RoomLocation> Labs { get; set; } = new();

    public override string ToString()
    {
        return $"{Name} ({RoomNumber})";
    }
}