using Microsoft.Data.Sqlite;
using ThunderstoreFormatter.SQLite.DatabaseContext;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.Utils;

public static class ProfileDBMS
{
    public static List<Profile> currentPaths { get; set; } = new List<Profile>();
    public static void InitializeDatabase()
    {
        using (var context = new ProfileDbContext())
        {
            // Create the database if it doesn't exist
            context.Database.EnsureCreated();
        }
    }

    public static List<Profile> RetrieveAllFromDatabase()
    {
        using (var context = new ProfileDbContext())
        {
            currentPaths.Clear();  // Clear the list to avoid duplicate entries
            currentPaths.AddRange(context. Profiles.ToList());  // Add the new data from the database
            foreach (var profile in currentPaths)
            {
                Console.WriteLine($"ID: {profile.ID}, Name: {profile.ProfileName}, Mods: {profile.NumberMods}, Path: {profile.Path}");
            }
        }

        return currentPaths;
    }

    public static void DeleteProfileByProfile(Profile selectedProfile)
    {
        
        int selectedProfileId = selectedProfile.ID;
        using (var context = new ProfileDbContext())
        {
            var profileToRemove = context
                .Profiles
                .FirstOrDefault(p => p.ID == selectedProfileId);
            if (profileToRemove != null)
            {
                context.Profiles.Remove(profileToRemove);
                context.SaveChanges();
            }
        }
        // Remove the selected item from the collection
        currentPaths.Remove(selectedProfile);
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