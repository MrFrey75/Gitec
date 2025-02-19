namespace TecCore.Utilities;

public static class StringHelpers
{
    public static string RemoveNonIntegers(string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }
}