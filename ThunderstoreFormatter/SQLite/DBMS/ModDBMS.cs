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
        using (var context = new ModDbContext())
        {
            var existingMod = context.Mods
                .FirstOrDefault(m => m.NameSpace == response.Namespace && m.ModName == response.Name);
            if (existingMod == null)
            {
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
                }
            }
            else
            {
                Console.WriteLine("Mod already exists in the database.");
            }
        }
    }

    public static List<Mod> GetAllModFromDatabaseByFullName(List<String> modFullname)
    {
        List<Mod> modList = new List<Mod>();
        foreach (var fullName in modFullname)
        {
            Mod? foundDatabseMod = GetModByFullName(fullName);
            if (foundDatabseMod != null)
            {
                Console.WriteLine("WE RECEIVED + " + foundDatabseMod.FullName);
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