using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoard : BaseEntity
{
    public List<InfoBoardItem> InfoBoardItems { get; set; } = new List<InfoBoardItem>();
    public int SortOrder { get; set; } = 0;
    public bool IsArchived { get; set; } = false;
    
    public DateTime StartAt { get; set; } = DateTime.UtcNow + TimeSpan.FromDays(1);
    public DateTime EndAt { get; set; } = DateTime.UtcNow + TimeSpan.FromDays(22);

    
}
