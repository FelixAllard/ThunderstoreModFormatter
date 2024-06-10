using System.ComponentModel.DataAnnotations;

namespace ThunderstoreFormatter.SQLite.Model;

public class Profile
{
    [Key]
    public int ID { get; set; }
    public string ProfileName { get; set; }
    public int NumberMods { get; set; }
    public string Path { get; set; }
}