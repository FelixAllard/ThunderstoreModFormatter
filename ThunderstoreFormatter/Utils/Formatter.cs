using NetTopologySuite.Noding;
using ThunderstoreFormatter.DataClass;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.Utils;

public class Formatter
{
    private List<Mod> ModList;
    private List<Category> categoryList;
    public Formatter(List<Mod> modList)
    {
        ModList = modList;
    }

    public List<Category> GetAllCategory()
    {
        categoryList = new List<Category>();

        //FOR ALL THE MODS
        foreach (var mod in ModList)
        {
            //FOR ALL THE MODS CATEGORIES
            foreach (var category in mod.Categories)
            {
                if(BannedCategories.IsBanned(category))continue;
                //FOR ALL THE CURRENTLY EXISTNG CATEGORIES
                bool categoryExist = false;
                foreach (var categoryFromList in categoryList)
                {
                    //IF THE CATEGORY ALREADY EXITS
                    if (category == categoryFromList.CategoryName)
                    {
                        categoryFromList.AddMod(mod);
                        categoryExist = true;
                    }
                }
                if (!categoryExist)
                {
                    //If the category doesn't exist, we create it and use the mod constructor
                    categoryList.Add(new Category(category, mod));
                }
                //WE DO IT FOR EACH MOD FOR EACH CATEGORIES
            }
        }
        return categoryList;
    }
}