using System.IO;

namespace ThunderstoreFormatter.Utils;

public static class Folder
{
    public static int CountFoldersInPath(string path)
    {
        if (Directory.Exists(path))
        {
            string[] directories = Directory.GetDirectories(path);
            return directories.Length;
        }
        else
        {
            Console.WriteLine("The specified path does not exist.");
            return 0;
        }
    }
    public static bool CheckIfPathStillExist(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        else
        {
            Console.WriteLine("The specified path does not exist.");
            return false;
        }
    }
}