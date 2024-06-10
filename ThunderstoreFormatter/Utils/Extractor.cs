using System.IO;
using ThunderstoreFormatter.DataClass;
using Newtonsoft.Json.Linq;
namespace ThunderstoreFormatter.Utils;

public static class Extractor
{
    public static List<string> GetManifestNames(string rootFolderPath)
    {
        List<string> manifestNames = new List<string>();

        // Check if the root folder exists
        if (!Directory.Exists(rootFolderPath))
        {
            Console.WriteLine("Root folder does not exist.");
            return manifestNames;
        }

        // Iterate through each folder inside the root folder
        foreach (string folderPath in Directory.GetDirectories(rootFolderPath))
        {
            // Check if manifest.json exists in the current folder
            string manifestPath = Path.Combine(folderPath, "manifest.json");
            if (File.Exists(manifestPath))
            {
                try
                {
                    // Read and parse the manifest.json file
                    string manifestContent = File.ReadAllText(manifestPath);
                    JObject manifestObject = JObject.Parse(manifestContent);

                    // Retrieve the "name" field from the manifest
                    
                    string name = manifestObject["name"]?.ToString();
                    string _namespace = manifestObject["namespace"]?.ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        manifestNames.Add(name);
                        Database.GetModInfo(name, _namespace);
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading manifest.json in folder '{folderPath}': {ex.Message}");
                }
            }
        }
        return manifestNames;
    }
}