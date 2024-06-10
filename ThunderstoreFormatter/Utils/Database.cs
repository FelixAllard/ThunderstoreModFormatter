using System.Net.Http;
using System.Net.Http.Json;
using ThunderstoreFormatter.DataClass.External_Call;
using ThunderstoreFormatter.SQLite.DatabaseContext;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.Utils;

public class Database
{
    public static async Task GetModInfo(string nameSpace, string packageName)
    {
        var httpClient = new HttpClient();
        var response =
            await httpClient.GetFromJsonAsync<ExternalServiceResponse>(
                $"https://thunderstore.io/api/experimental/package/{nameSpace}/{packageName}/");

        if (response != null)
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
        else
        {
            Console.WriteLine("Failed to retrieve data from the external service.");
        }
    }
}
