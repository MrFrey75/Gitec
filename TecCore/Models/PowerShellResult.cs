namespace TecCore.Models;

/// <summary>
/// Contains the output, error messages, and exit code from a PowerShell invocation.
/// </summary>
public class PowerShellResult
{
    public string Output { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public int ExitCode { get; set; }
}