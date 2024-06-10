using System.ComponentModel.DataAnnotations;

namespace ThunderstoreFormatter.SQLite.Model;

public class Mod
{
    [Key]
    public int ID { get; set; }
    public string ModName { get; set; }
    public string NameSpace { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public string[] Categories { get; set; }
    public string Community { get; set; }
}