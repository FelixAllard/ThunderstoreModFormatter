using System.Text.Json.Serialization;

namespace ThunderstoreFormatter.DataClass.External_Call;

public class ExternalServiceResponse
{
    [JsonPropertyName("namespace")]
    public string Namespace { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("package_url")]
    public string PackageUrl { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("date_updated")]
    public DateTime DateUpdated { get; set; }

    [JsonPropertyName("rating_score")]
    public int RatingScore { get; set; }

    [JsonPropertyName("is_pinned")]
    public bool IsPinned { get; set; }

    [JsonPropertyName("is_deprecated")]
    public bool IsDeprecated { get; set; }

    [JsonPropertyName("total_downloads")]
    public int TotalDownloads { get; set; }

    [JsonPropertyName("latest")]
    public LatestInfo Latest { get; set; }

    [JsonPropertyName("community_listings")]
    public CommunityListing[] CommunityListings { get; set; }
}