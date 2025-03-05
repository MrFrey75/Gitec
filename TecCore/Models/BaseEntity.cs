using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TecCore.Models;

public class BaseEntity
{
    [Key]
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}