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
}