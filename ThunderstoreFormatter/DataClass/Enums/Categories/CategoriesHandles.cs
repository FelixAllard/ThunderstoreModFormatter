

using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.DataClass.Enums{

   public static class CategoriesHandles
    {
        public static Dictionary<Categories, string> CurrentlyDisplayedCategories;
        public static List<Category> categoryList;

        // Dictionary to map enum values to their desired display names (custom ones only)
        private static readonly Dictionary<Categories, string> DisplayNames = new Dictionary<Categories, string>
        {
            { Categories.ServerSide, "Server-side" },
            { Categories.AssetReplacements, "Asset Replacements" },
            { Categories.BoomboxMusic, "Boombox Music" },
            { Categories.TVVideos, "TV Videos" },
            { Categories.ClientSide, "Client-side" }
        };

        // Create a HashSet to store all category display names for quick lookup (case-insensitive)
        private static readonly HashSet<string> AllCategories = new HashSet<string>(GetAllDisplayNames(), StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Must be called as early as possible
        /// </summary>
        public static void Init()
        {
            CurrentlyDisplayedCategories = new Dictionary<Categories, string>();

            foreach (CategoriesKeep keepCategory in Enum.GetValues(typeof(CategoriesKeep)))
            {
                if (Enum.TryParse(keepCategory.ToString(), out Categories category))
                {
                    string displayName = DisplayNames.ContainsKey(category) ? DisplayNames[category] : category.ToString();
                    CurrentlyDisplayedCategories[category] = displayName;
                }
            }
        }

        // Method to get display names for all enum values
        private static IEnumerable<string> GetAllDisplayNames()
        {
            foreach (Categories category in Enum.GetValues(typeof(Categories)))
            {
                yield return DisplayNames.ContainsKey(category) ? DisplayNames[category] : category.ToString();
            }
        }

        /// <summary>
        /// Returns true if the category name is not one that can be displayed.
        /// </summary>
        /// <param name="input">The category name to check.</param>
        /// <returns>True if the category name is in the ban list, otherwise false.</returns>
        public static bool IsBanned(string input)
        {
            return AllCategories.Contains(input) && !CurrentlyDisplayedCategories.ContainsValue(input);
        }

        /// <summary>
        /// Gets the Categories enum value corresponding to a display name.
        /// </summary>
        /// <param name="input">The display name to look up.</param>
        /// <returns>The corresponding Categories enum value, or null if not found.</returns>
        public static Categories? GetCategoryByString(string input)
        {
            foreach (var category in DisplayNames)
            {
                if (category.Value.Equals(input, StringComparison.OrdinalIgnoreCase))
                {
                    return category.Key;
                }
            }

            foreach (Categories category in Enum.GetValues(typeof(Categories)))
            {
                if (category.ToString().Equals(input, StringComparison.OrdinalIgnoreCase))
                {
                    return category;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the display name corresponding to a Categories enum value.
        /// </summary>
        /// <param name="category">The Categories enum value.</param>
        /// <returns>The corresponding display name, or "NULLREFERROR" if not found.</returns>
        public static string GetStringByCategory(Categories category)
        {
            return DisplayNames.TryGetValue(category, out var displayName) ? displayName : category.ToString();
        }

        /// <summary>
        /// Removes a category from the currently displayed categories.
        /// </summary>
        /// <param name="categoryToRemove">The category to remove.</param>
        public static void RemoveCategory(Categories categoryToRemove)
        {
            CurrentlyDisplayedCategories.Remove(categoryToRemove);
        }
        
        
        
        public static List<Category> GetAllCategory(List<Mod> ModList)
        {
            categoryList = new List<Category>();

            //FOR ALL THE MODS
            foreach (var mod in ModList)
            {
                //FOR ALL THE MODS CATEGORIES
                foreach (var category in mod.Categories)
                {
                    if(CategoriesHandles.IsBanned(category))continue;
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
}