namespace ThunderstoreFormatter.Utils;

public static class Formaters
{
    public static string FormatForDiscord(List<String> x)
    {
        String finalString = "```\n";
        foreach (var mod in x)
        {
            finalString += mod + "\n";
        }

        finalString += "```";
        return finalString;
    }
    
}