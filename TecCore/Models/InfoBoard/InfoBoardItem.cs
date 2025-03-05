using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItem : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = "New Item";
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime StartAt { get; set; } = DateTime.UtcNow + TimeSpan.FromDays(1);
    public DateTime EndAt { get; set; } = DateTime.UtcNow + TimeSpan.FromDays(22);
    public string ItemType { get; set; } = InfoBoardItemType.Text.ToString();
}