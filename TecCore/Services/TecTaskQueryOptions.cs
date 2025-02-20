using Microsoft.Graph.Models;
using TecCore.DataStructs;
using TecCore.Models;

namespace TecCore.Services;

public class TecTaskQueryOptions
{
    public RoomLocation? Location { get; set; }
    public Severity? Severity { get; set; }
    public TaskCategory? TaskCategory { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public TaskState? TaskState { get; set; }
    public string? SearchTerm { get; set; }
    public User AffectedUser { get; set; }
    public Device AffectedDevice { get; set; }
}