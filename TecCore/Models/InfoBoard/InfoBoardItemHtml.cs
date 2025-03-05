using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItemHtml : InfoBoardItem
{
    public InfoBoardItemHtml()
    {
        ItemType = InfoBoardItemType.Html.ToString();
    }
    
    
    public string Content { get; set; } = "<p>New HTML</p>";
}