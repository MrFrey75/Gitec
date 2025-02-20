using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Graph.Models;
using TecCore.DataStructs;
using TecCore.Services;

namespace TecCore.Models;

public class TecTask : BaseEntity
{
    
    [MaxLength(100)]
    public string TaskName { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string TaskDescription { get; set; } = string.Empty;
    public DateTimeOffset DueDate { get; set; } = DateTimeOffset.Now;
    public RoomLocation Location { get; set; } = InfraService.GetRoomLocations().FirstOrDefault(x => x.RoomNumber == "000")!;
    public TaskCategory TaskCategory { get; set; } = TaskCategory.Unknown;
    public Severity Severity { get; set; } = Severity.Unknown;
    [MaxLength(100)]
    public string ContactInfo { get; set; } = string.Empty;
    public bool IsStudentTask { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    public List<TecTaskUpdate> Updates { get; set; } = [];
    public List<GitecUser> AffectedUsers { get; set; } = [];
    public List<GitecDevice> AffectedDevices { get; set; } = [];
    
    [NotMapped]
    public TaskState TaskState =>  Updates.Count == 0 ? TaskState.Submitted : Updates.Last().TaskState;
    
    [NotMapped]
    public DateTime LastUpdate => Updates.Count == 0 ? CreatedAt : Updates.Last().CreatedAt;
    
    [NotMapped]
    public string LastUpdateSummary => Updates.Count == 0 ? "Task submitted." : Updates.Last().Notes;
    
}