using System.IO;
using System.Windows;
using System.Windows.Forms;
using ThunderstoreFormatter.SQLite.DatabaseContext;
using ThunderstoreFormatter.SQLite.Model;
using ThunderstoreFormatter.Utils;

namespace ThunderstoreFormatter;

public partial class AddProfile : Window
{
    private Action _onProfileAdded;

    private String name;
    private String path;
    public AddProfile(Action onProfileAdded)
    {
        InitializeComponent();
        _onProfileAdded = onProfileAdded;
    }
    
    private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
    {
        string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string folderPath = Path.Combine(defaultPath, "Thunderstore Mod Manager", "DataFolder", "LethalCompany", "profiles");

        if (!Directory.Exists(folderPath))
        {
            folderPath = defaultPath; // Fallback to the root folder if the specific folder doesn't exist
        }

        using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
        {
            folderBrowserDialog.Description = "Select the Profile, BepInEx or Plugin Folder";
            folderBrowserDialog.SelectedPath = folderPath;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                path = ProcessFolderPath(folderBrowserDialog.SelectedPath);
                System.Windows.MessageBox.Show("Selected Folder: " + path);
                if (name != null)
                {
                    NameTextBox.Text = name;
                }
                if (path!=null)
                {
                    PathTextBox.Text = path;
                }
                // Do something with the selectedPath, like save it or process it further
            }
        }
    }
    /// <summary>
    /// Parent FINDER! Accessive elsewhere
    /// </summary>
    /// <param name="basePath"></param>
    /// <returns></returns>
    public string ProcessFolderPath(string path)
    {
        // Remove "Plugins" from the end of the path if present
        if (path.EndsWith("Plugins", StringComparison.OrdinalIgnoreCase))
        {
            path = path.Substring(0, path.Length - "Plugins".Length).TrimEnd('\\');
        }

        // Remove "BepInEx" from the end of the path if present
        if (path.EndsWith("BepInEx", StringComparison.OrdinalIgnoreCase))
        {
            path = path.Substring(0, path.Length - "BepInEx".Length).TrimEnd('\\');
        }

        // Get the last folder name
        string[] directories = path.Split(Path.DirectorySeparatorChar);
        name = directories[directories.Length - 1];

        // Construct the new path
        string newPath = Path.Combine(path, "BepInEx", "Plugins");

        return newPath;
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
    {
        using (var context = new ProfileDbContext())
        {
            var profile = new Profile
            {
                ProfileName = NameTextBox.Text,
                NumberMods = Utils.Folder.CountFoldersInPath(path),
                Path = PathTextBox.Text
            };
            context.Profiles.Add(profile);
            context.SaveChanges();
            //This wil polute the database with mods
            Extractor.PoluteDatabaseWithMods(profile.Path);
        }
        _onProfileAdded?.Invoke(); // Call the delegate
        this.Close();
        
    }
}