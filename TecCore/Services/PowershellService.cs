using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TecCore.Models;

namespace TecCore.Services
{


    public static class PowerShellService
    {
        /// <summary>
        /// Executes an inline PowerShell command with additional arguments.
        /// </summary>
        /// <param name="command">The PowerShell command to execute.</param>
        /// <param name="args">Optional arguments to pass to the command.</param>
        /// <returns>A PowerShellResult containing the output, errors, and exit code.</returns>
        public static PowerShellResult RunCommand(string command, params string[]? args)
        {
            // Build a single arguments string, quoting each argument.
            var argsString = args is { Length: > 0 } 
                ? string.Join(" ", args.Select(arg => $"\"{arg}\"")) 
                : string.Empty;
            
            // Combine the command with its arguments.
            var fullCommand = string.IsNullOrWhiteSpace(argsString) 
                ? command 
                : $"{command} {argsString}";

            return ExecutePowerShellCommand(fullCommand);
        }

        /// <summary>
        /// Executes a PowerShell script file with named parameters.
        /// </summary>
        /// <param name="scriptFilePath">Full path to the PowerShell script file.</param>
        /// <param name="parameters">Optional named parameters to pass (e.g., {"ParamName", "Value"}).</param>
        /// <returns>A PowerShellResult containing the script output, errors, and exit code.</returns>
        public static PowerShellResult RunScript(string scriptFilePath, IDictionary<string, string>? parameters = null)
        {
            // Build the parameters string in the form: -ParamName "Value"
            string paramString = parameters != null
                ? string.Join(" ", parameters.Select(kv => $"-{kv.Key} \"{kv.Value}\""))
                : string.Empty;
            
            // Build the command to execute the script file with its parameters.
            string command = $"-File \"{scriptFilePath}\" {paramString}";
            return ExecutePowerShellCommand(command);
        }

        /// <summary>
        /// Executes the specified PowerShell command or script command and returns its result.
        /// </summary>
        /// <param name="arguments">The arguments to pass to powershell.exe.</param>
        /// <returns>A PowerShellResult with output, error messages, and exit code.</returns>
        private static PowerShellResult ExecutePowerShellCommand(string arguments)
        {
            var result = new PowerShellResult();

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    // Using -NoProfile and -ExecutionPolicy Bypass to ensure smooth execution.
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{arguments}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi)!;
                result.Output = process.StandardOutput.ReadToEnd();
                result.Error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                result.ExitCode = process.ExitCode;
            }
            catch (Exception ex)
            {
                result.Error = $"Error executing PowerShell command: {ex.Message}";
                result.ExitCode = -1;
            }

            return result;
        }
    }
}
