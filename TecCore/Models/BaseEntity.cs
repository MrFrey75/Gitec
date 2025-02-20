using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TecCore.Models;

public class BaseEntity
{
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}