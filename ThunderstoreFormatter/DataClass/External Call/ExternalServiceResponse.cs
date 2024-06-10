namespace ThunderstoreFormatter.DataClass.External_Call;

public class ExternalServiceResponse
{
    public string Namespace { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Owner { get; set; }
    public string PackageUrl { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public int RatingScore { get; set; }
    public bool IsPinned { get; set; }
    public bool IsDeprecated { get; set; }
    public int TotalDownloads { get; set; }
    public LatestInfo Latest { get; set; }
    public CommunityListing[] CommunityListings { get; set; }
}