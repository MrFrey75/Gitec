using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItemImage : InfoBoardItem
{
    public InfoBoardItemImage()
    {
        ItemType = InfoBoardItemType.Image.ToString();
    }
    
    
    [MaxLength(255)]
    public string ImageUrl { get; set; } = "https://via.placeholder.com/150";
}