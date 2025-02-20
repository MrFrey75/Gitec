using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace TecCore.Utilities
{
    public static class NetworkHelper
    {
        /// <summary>
        /// Pings the specified host and returns the result as a string.
        /// </summary>
        /// <param name="host">The hostname or IP address to ping.</param>
        /// <param name="timeout">Timeout in milliseconds. Default is 4000ms.</param>
        /// <param name="bufferSize">Size of the buffer in bytes. Default is 32 bytes.</param>
        /// <param name="ttl">Time-to-live for the ping packet. Default is 128 hops.</param>
        /// <returns>A string containing the ping result.</returns>
        public static string PingHost(string host, int timeout = 4000, int bufferSize = 32, int ttl = 128)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var buffer = new byte[bufferSize];
                    var options = new PingOptions(ttl, true);
                    var reply = ping.Send(host, timeout, buffer, options);
                    
                    if (reply.Status == IPStatus.Success)
                    {
                        return $"Ping to {host} successful: {reply.RoundtripTime}ms";
                    }
                    else
                    {
                        return $"Ping to {host} failed: {reply.Status}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Ping failed with error: {ex.Message}";
            }
        }

        /// <summary>
        /// Runs the 'ipconfig' command with the specified arguments and returns its output.
        /// </summary>
        /// <param name="arguments">Command-line arguments for ipconfig (default: "/all").</param>
        /// <returns>A string containing the ipconfig output.</returns>
        public static string RunIpConfig(string arguments = "/all")
        {
            return RunCommand("ipconfig", arguments);
        }

        /// <summary>
        /// Runs a specified command with arguments and returns its output.
        /// </summary>
        /// <param name="command">The command to run (e.g., "ipconfig", "tracert").</param>
        /// <param name="arguments">Command-line arguments.</param>
        /// <returns>A string containing the output (or error message if the command fails).</returns>
        public static string RunCommand(string command, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process? process = Process.Start(psi);
                // Wait for the process to exit.
                process!.WaitForExit();
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(error))
                {
                    output += Environment.NewLine + error;
                }
                return output;
            }
            catch (Exception ex)
            {
                return $"Error executing command '{command}': {ex.Message}";
            }
        }
    }
}
