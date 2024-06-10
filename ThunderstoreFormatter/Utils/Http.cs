using System.Net.Http;
using System.Net.Http.Json;
using ThunderstoreFormatter.DataClass.External_Call;

namespace ThunderstoreFormatter.Utils;

public class Http
{
    /// <summary>
    /// This calls Thunderstore Api to get the information on the mod
    /// </summary>
    /// <param name="nameSpace"></param>
    /// <param name="packageName"></param>
    public static async Task GetModInfo(string nameSpace, string packageName)
    {
        var httpClient = new HttpClient();
        var response =
            await httpClient.GetFromJsonAsync<ExternalServiceResponse>(
                $"https://thunderstore.io/api/experimental/package/{nameSpace}/{packageName}/");

        if (response != null)
        {
            await ModDBMS.AddModToDatabase(response);
        }
        else
        {
            Console.WriteLine("Failed to retrieve data from the external service.");
        }
    }
}