using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItemRss : InfoBoardItem
{
    
    public InfoBoardItemRss()
    {
        ItemType = InfoBoardItemType.RssFeed.ToString();
    }
    
    [MaxLength(255)]
    public string RssUrl { get; set; } = "https://www.example.com/rss";
}