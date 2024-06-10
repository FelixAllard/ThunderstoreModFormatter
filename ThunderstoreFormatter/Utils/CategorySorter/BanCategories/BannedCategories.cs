namespace ThunderstoreFormatter.Utils;

public static class BannedCategories
{
    // Create a HashSet to store banned values
    private static HashSet<string> bannedValues = new HashSet<string>(Enum.GetNames(typeof(BannedCategory)), StringComparer.OrdinalIgnoreCase);

    // Method to check if a string is in the banlist
    /// <summary>
    /// Returns true if the category name is not one that can be displayed
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsBanned(string input)
    {
        return bannedValues.Contains(input);
    }
}