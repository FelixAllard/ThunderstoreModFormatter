using ThunderstoreFormatter.DataClass;
using ThunderstoreFormatter.DataClass.Enums;

namespace ThunderstoreFormatter.Utils;

public static class Formaters
{

    public static string FormatForDiscord(List<Category> categories)
    {
        string finalString = String.Empty;

        foreach (var category in categories)
        {
            finalString += FormatForDiscord(category);
        }

        return finalString;
    }
    
    public static string FormatForDiscord(Category category)
    {
        String finalString = "";
        finalString += "### " +category.CategoryName + "\n";
        finalString += "```\n";
        foreach (var mod in category.Mods)
        {
            finalString += mod.ModName + "\n";
        }

        finalString += "```\n";
        return finalString;
    }
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