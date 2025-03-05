using System.ComponentModel.DataAnnotations;
using TecCore.DataStructs;

namespace TecCore.Models.InfoBoard;

public class InfoBoardItemCarousel : InfoBoardItem
{
    public InfoBoardItemCarousel()
    {
        ItemType = InfoBoardItemType.Carousel.ToString();
    }
    
    
    public List<CarouselItem> CarouselItems { get; set; } = new List<CarouselItem>();
    
    
    public void Append(CarouselItem item)
    {
        CarouselItems.Add(item);
    }
    
    public void Remove(int id)
    {
        CarouselItems.RemoveAll(i => i.Id == id);
    }
    
    public void Update(CarouselItem item)
    {
        var index = CarouselItems.FindIndex(i => i.Id == item.Id);
        if (index != -1)
        {
            CarouselItems[index] = item;
        }
    }
    
    public CarouselItem Find(int id)
    {
        return CarouselItems.Find(i => i.Id == id)!;
    }
    
}



public class CarouselItem
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string ImageUrl { get; set; } = "https://via.placeholder.com/150";
    [MaxLength(75)]
    public string Title { get; set; } = "New Carousel Item";
    public string Content { get; set; } = "New Carousel Item Content";
}