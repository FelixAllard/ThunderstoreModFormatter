namespace ThunderstoreFormatter.DataClass.External_Call;

public class LatestInfo
{
    public string Namespace { get; set; }
    public string Name { get; set; }
    public string VersionNumber { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string[] Dependencies { get; set; }
    public string DownloadUrl { get; set; }
    public int Downloads { get; set; }
    public DateTime DateCreated { get; set; }
    public string WebsiteUrl { get; set; }
    public bool IsActive { get; set; }
}