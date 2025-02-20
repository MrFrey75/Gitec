using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace TecCore.Utilities;

public static class ConsoleHelper
{
    // Utility Method for Displaying Colored Console Messages
    public static void DisplayMessage(string message, ConsoleColor? color = ConsoleColor.White)
    {
        Console.ForegroundColor = color ?? ConsoleColor.White;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    // Utility Method for Displaying Colored Console Messages with a Line Break
    public static void LineBreak(ConsoleColor? color = ConsoleColor.White)
    {
        Console.ForegroundColor = color ?? ConsoleColor.White;
        Console.WriteLine();
        Console.ResetColor();
    }
    
    public static void Divider(ConsoleColor? color = ConsoleColor.White)
    {
        Console.ForegroundColor = color ?? ConsoleColor.White;
        Console.WriteLine("===================================================================");
        Console.ResetColor();
    }
    
    public static void Clear()
    {
        Console.Clear();
    }
    
    public static void Write(string message, ConsoleColor? color = ConsoleColor.White)
    {
        DisplayMessage(message, color);
    }
    
    public static void WriteTitle(string message, ConsoleColor? color = ConsoleColor.Cyan)
    {
        Console.ForegroundColor = color ?? ConsoleColor.Cyan;
        Divider();
        DisplayMessage(message, color);
        Divider();
        LineBreak();
    }
    
    public static void WriteSubTitle(string message, ConsoleColor? color = ConsoleColor.DarkCyan)
    {
        Console.ForegroundColor = color ?? ConsoleColor.DarkCyan;
        DisplayMessage(message, color);
        LineBreak();
    }
    
}