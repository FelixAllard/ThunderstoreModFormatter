using System.ComponentModel;

namespace ThunderstoreFormatter.Utils;

public enum BannedCategory
{
    hello,
    //TODO make this server-side work!
    [Description("Server-side")]
    ServerSide,
    
    
}
