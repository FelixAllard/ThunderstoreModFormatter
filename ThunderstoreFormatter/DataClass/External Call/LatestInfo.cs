using System.Text.Json.Serialization;

namespace ThunderstoreFormatter.DataClass.External_Call;

public class LatestInfo
{
    [JsonPropertyName("namespace")]
    public string Namespace { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("version_number")]
    public string VersionNumber { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("dependencies")]
    public string[] Dependencies { get; set; }

    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("downloads")]
    public int Downloads { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("website_url")]
    public string WebsiteUrl { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}