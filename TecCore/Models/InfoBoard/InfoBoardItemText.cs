using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItemText : InfoBoardItem
{
    public InfoBoardItemText()
    {
        ItemType = InfoBoardItemType.Text.ToString();
    }
    
    public string Content { get; set; } = "New Text";
}