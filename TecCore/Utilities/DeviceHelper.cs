using System.Text.RegularExpressions;

namespace TecCore.Utilities;

public static class DeviceHelper
{
    public static bool IsValidCurrent(string input)
    {
        // Regex pattern breakdown:
        // ^CTE - Start with 'CTE'
        // [0-9]{3} - Room number: Exactly 3 digits
        // [A-Z] - Device type: Exactly one uppercase letter
        // [0-9]{4,5}$ - Asset tag: Exactly 4 or 5 digits
        string pattern = @"^CTE[0-9]{3}[A-Z][0-9]{4,5}$";
        
        // Check if the input matches the pattern
        return Regex.IsMatch(input, pattern);
    }
    
    public static bool IsValidOld(string input)
    {
        // Regex pattern breakdown:
        // ^CTE- - Start with 'CTE-'
        // (T-)? - Optional 'T-' for teaching station
        // [0-9]{3} - Room number: Exactly 3 digits
        // - - Separator
        // (V?[0-9]{4,5})$ - Optional 'V' followed by 4 or 5 digits for asset tag
        string pattern = @"^CTE-(T-)?[0-9]{3}-(V?[0-9]{4,5})$";
        
        // Check if the input matches the pattern
        return Regex.IsMatch(input, pattern);
    }
    
    public static string CreateDeviceName(string room, string type, string asset)
    {
        return $"CTE{room}{type}{asset}".ToUpper();
    }

    public static string UpdateDeviceName(string oldDeviceName)
    {
        if (IsValidCurrent(oldDeviceName))
        {
            return oldDeviceName;
        }
        if(!IsValidOld(oldDeviceName))
        {
            throw new Exception("Invalid Device Name");
        }

        var parts = GetDeviceParts(oldDeviceName);
        return CreateDeviceName(parts.Item1, parts.Item2, parts.Item3);
    }
    
    public static (string, string, string) GetDeviceParts(string input)
    {
        var room = "000";
        var type = "X";
        var asset = "0000";
        if (IsValidCurrent(input))
        {
            room = input.Substring(3, 3);
            type = input.Substring(6, 1);
            asset = input.Substring(7);
        } else if (IsValidOld(input))
        {
            var parts = input.Split("-");
            room = parts[1];
            var descriptor = parts[2];
            if(room[..1].Equals("T"))
            {
                type = "T";
                room = room.Substring(2);
            } else
            {
                type = "S";
            }
            asset = StringHelpers.RemoveNonIntegers(parts[2]);
        }
        
        return (room, type, asset);
        
        
    }    
    public static void ValidateAndParse(string input)
    {
        // Reset color at the start to ensure a clean state
        Console.ResetColor();

        // Validate and Parse the Input
        if (DeviceHelper.IsValidCurrent(input))
        {
            ConsoleHelper.DisplayMessage( $"✅ {input} is a VALID **Current Format** device name.", ConsoleColor.Green);
        }
        else if (DeviceHelper.IsValidOld(input))
        {
            ConsoleHelper.DisplayMessage( $"✅ {input} is a VALID **Old Format** device name. New: {DeviceHelper.UpdateDeviceName(input)}", ConsoleColor.Yellow);
        }
        else
        {
            ConsoleHelper.DisplayMessage( $"❌ {input} is **INVALID**.", ConsoleColor.Red);
            return;
        }

        // Extract and Display Device Parts
        var (room, type, asset) = DeviceHelper.GetDeviceParts(input);
        ConsoleHelper.DisplayMessage($"🔹 Extracted Parts -> Room: {room}, Type: {type}, Asset: {asset}", ConsoleColor.Cyan);
    }
    
}