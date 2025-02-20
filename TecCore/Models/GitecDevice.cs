namespace TecCore.Models;

public class GitecDevice : BaseEntity
{
    public string DisplayName { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string OperatingSystem { get; set; } = string.Empty;
    public string OperatingSystemVersion { get; set; } = string.Empty;
    public bool IsManaged { get; set; } = false;
    public DateTimeOffset EnrolledDateTime { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset LastSyncDateTime { get; set; } = DateTimeOffset.MinValue;
    public string DeviceCategory { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    
}