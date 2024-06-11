using System.Text.Json.Serialization;

namespace ThunderstoreFormatter.DataClass.External_Call;

public class CommunityListing
{
    [JsonPropertyName("has_nsfw_content")]
    public bool HasNsfwContent { get; set; }

    [JsonPropertyName("categories")]
    public string[] Categories { get; set; }

    [JsonPropertyName("community")]
    public string Community { get; set; }

    [JsonPropertyName("review_status")]
    public string ReviewStatus { get; set; }
}