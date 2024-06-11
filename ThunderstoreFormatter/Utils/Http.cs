using System.Net.Http;
using System.Net.Http.Json;
using ThunderstoreFormatter.DataClass.External_Call;

namespace ThunderstoreFormatter.Utils;

public class Http
{

    public static async Task AddAllToDatabase(List<string> listOfMods)
    {
        foreach (var mod in listOfMods)
        {
            string[] parts = mod.Split('-');

            if (parts.Length == 2)
            {
                string firstPart = parts[0];
                string secondPart = parts[1];

                //Check if the mod is in the Database, if it is not, then get the thing online!
                if (!ModDBMS.CheckIfModIsInDatabase(mod))
                {
                    GetModInfo(firstPart, secondPart);
                }
                Console.WriteLine("First part: " + firstPart);
                Console.WriteLine("Second part: " + secondPart);
            }
            else
            {
                Console.WriteLine("Invalid input format.");
            }
            
        }
    }
    //TODO make progress bar for the loading!
    /// <summary>
    /// This calls Thunderstore Api to get the information on the mod
    /// </summary>
    /// <param name="nameSpace"></param>
    /// <param name="packageName"></param>
    public static void GetModInfo(string nameSpace, string packageName)
    {
        Console.WriteLine("HelloWorld");
    
        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30); // Set timeout to 30 seconds
        
            HttpResponseMessage httpResponse = httpClient.GetAsync($"https://thunderstore.io/api/experimental/package/{nameSpace}/{packageName}/").GetAwaiter().GetResult();
            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {httpResponse.StatusCode} ");
                // Handle the error accordingly
            }
            else
            {
                //We get 200 OK
                ExternalServiceResponse response = httpResponse.Content.ReadFromJsonAsync<ExternalServiceResponse>().GetAwaiter().GetResult();
                
                Console.WriteLine(response.Owner);
                if (response != null)
                {
                    ModDBMS.AddModToDatabase(response).GetAwaiter().GetResult();
                    
                }
                else
                {
                    Console.WriteLine("Failed to retrieve data from the external service.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            // Handle the exception accordingly
        }
    }

}