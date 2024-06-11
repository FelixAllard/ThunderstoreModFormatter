using Microsoft.Data.Sqlite;
using ThunderstoreFormatter.DataClass;
using ThunderstoreFormatter.DataClass.External_Call;
using ThunderstoreFormatter.SQLite.DatabaseContext;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.Utils;

public static class ModDBMS
{
    public static List<Category> categoryList;

    public static void InitializeDatabase()
    {
        using (var context = new ModDbContext())
        {
            // Create the database if it doesn't exist
            context.Database.EnsureCreated();
        }
    }
    public static Mod? GetModByFullName(string fullName)
    {
        using (var context = new ModDbContext())
        {
            return context.Mods.FirstOrDefault(mod => mod.FullName == fullName);
        }
    }

    public static async Task AddModToDatabase(ExternalServiceResponse response)
    {
        Console.WriteLine("Starting AddModToDatabase method.");

        using (var context = new ModDbContext())
        {
            Console.WriteLine("Created ModDbContext.");

            // Ensure the response and necessary fields are not null
            if (response == null)
            {
                Console.WriteLine("Response is null.");
                return;
            }

            if (response.Namespace == null || response.Name == null)
            {
                Console.WriteLine("Namespace or Name in the response is null.");
                return;
            }

            Console.WriteLine($"Looking for existing mod with Namespace: {response.Namespace}, Name: {response.Name}.");
            
            var existingMod = context.Mods
                .FirstOrDefault(m => m.NameSpace == response.Namespace && m.ModName == response.Name);

            Console.WriteLine("Checked for existing mod.");

            if (existingMod == null)
            {
                Console.WriteLine("No existing mod found.");
                if (response.CommunityListings == null)
                {
                    Console.WriteLine("CommunityListings is null.");
                    return;
                }

                var communityListing = response.CommunityListings.FirstOrDefault();
                if (communityListing != null)
                {
                    var newMod = new Mod
                    {
                        ModName = response.Name,
                        NameSpace = response.Namespace,
                        FullName = response.FullName,
                        Description = response.Latest.Description,
                        Categories = communityListing.Categories,
                        Community = communityListing.Community
                    };

                    context.Mods.Add(newMod);
                    await context.SaveChangesAsync();
                    Console.WriteLine("New mod added to the database.");
                }
                else
                {
                    Console.WriteLine("No community listings found in the response.");
                }
            }
            else
            {
                Console.WriteLine("Mod already exists in the database.");
            }
        }
        Console.WriteLine("Finished AddModToDatabase method.");
    }
    /// <summary>
    /// Check if a mod is in the database or not
    /// </summary>
    /// <param name="modFullname">The full name of the mod</param>
    /// <returns>True if it is in the databsae, false if it is not</returns>
    public static bool CheckIfModIsInDatabase(String modFullname)
    {
        Mod? foundDatabseMod = GetModByFullName(modFullname);
        if (foundDatabseMod != null)
        {
            return true;

        }
        return false;
    }

    public static List<Mod> GetAllModFromDatabaseByFullName(List<String> modFullname)
    {
        List<Mod> modList = new List<Mod>();
        foreach (var fullName in modFullname)
        {
            Mod? foundDatabseMod = GetModByFullName(fullName);
            if (foundDatabseMod != null)
            {
                Console.WriteLine("WE FOUND : " + foundDatabseMod.FullName);
                modList.Add(foundDatabseMod);
            }
        }

        return modList;
    }
    public static void DropAllTables()
    {
        using (var connection = new SqliteConnection("Data Source = Sample.db"))
        {
            connection.Open();

            // Retrieve the connection string from the opened connection
            var connectionString = connection.DataSource;

            var command = connection.CreateCommand();
            command.CommandText = @"
                    SELECT 'DROP TABLE IF EXISTS ' || name || ';' 
                    FROM sqlite_master 
                    WHERE type = 'table';";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var dropTableCommand = reader.GetString(0);
                    using (var dropTableCommandExecutor = connection.CreateCommand())
                    {
                        dropTableCommandExecutor.CommandText = dropTableCommand;
                        dropTableCommandExecutor.ExecuteNonQuery();
                    }
                }
            }
        }
    }
    
}