using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItemMarkdown : InfoBoardItem
{
    public InfoBoardItemMarkdown()
    {
        ItemType = InfoBoardItemType.Markdown.ToString();
    }
    
    public string Content { get; set; } = "New Markdown";
}