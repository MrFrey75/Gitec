namespace TecCore.Models;

public class TecConfig 
{
    public string? AppCoreName { get; set; } = "Gitec";
    public string? AppCoreVersion { get; set; } = "1.0.0";
    public string? AppCoreDescription { get; set; } = "Gitec is a Git client for Windows.";
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    
}