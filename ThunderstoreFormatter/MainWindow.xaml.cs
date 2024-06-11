using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using ThunderstoreFormatter.DataClass;
using ThunderstoreFormatter.DataClass.Enums;
using ThunderstoreFormatter.SQLite.DatabaseContext;
using ThunderstoreFormatter.SQLite.Model;
using ThunderstoreFormatter.Utils;

namespace ThunderstoreFormatter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public List<Mod> allRegisteredMods { get; set; } = new List<Mod>();
    public List<CheckBoxItem> checkBoxItems { get; set; }
    
    public MainWindow()
    {
        //Initialize Database
        var connection = new SqliteConnection("Data Source = Sample.db");
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        connection.Open();
        
        
        //Initialise Page
        InitializeComponent();
        
        //TODO fix this thing here, this is the tag checkboxes
         checkBoxItems = new List<CheckBoxItem>
        {
            new CheckBoxItem { CheckboxText = "NOT YET IMPLEMENTED", Number = "1" },
            new CheckBoxItem { CheckboxText = "Please do give me feedbacks on my github or on my discord!", Number = "2" },
            // Add more items as needed
        };
         
         
        // Set the data context of the ListBox to the collection of items
        CheckBoxListBox.ItemsSource = checkBoxItems;
        
        
        InitialiseDatabaseIfNotExist();
        RetrieveInformationDatabase();
    }
    public void RetrieveInformationDatabase()
    {
        ViewProfile.ItemsSource = ProfileDBMS.RetrieveAllFromDatabase();
        ViewProfile.Items.Refresh();
    }
    public void RetrieveModsDatabase()
    {
        using (var context = new ModDbContext())
        {
            allRegisteredMods.Clear();  // Clear the list to avoid duplicate entries
            allRegisteredMods.AddRange(context.Mods.ToList());  // Add the new data from the database
        }
        Debug.WriteLine("We Registered all the mods");
    }
    /// <summary>
    /// Handles the add profile button from the FrontEnd
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddProfileButton_OnClick(object sender, RoutedEventArgs e)
    {
        //Giving Retrieve Information Database so that it is called after the information is saved
        AddProfile popup = new AddProfile(RetrieveInformationDatabase);
        popup.Owner = this; // Set the owner window to enable proper modal behavior
        popup.ShowDialog(); // Show the popup window
    }

    private void Delete_OnClick(object sender, RoutedEventArgs e)
    {
        // Check if an item is selected
        if (ViewProfile.SelectedItem != null)
        {
            // Get the selected item
            Profile selectedProfile = ViewProfile.SelectedItem as Profile;
            // Remove the selected item from the database
            ProfileDBMS.DeleteProfileByProfile(selectedProfile);
            // Refresh the ListView
            ViewProfile.Items.Refresh();
            RetrieveInformationDatabase();
        }
    }

    /// <summary>
    /// The button to show all the mods!
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonShow_OnClick(object sender, RoutedEventArgs e)
    {
        if (ViewProfile.SelectedItem != null)
        {
            // Get the selected item
            Profile selectedProfile = ViewProfile.SelectedItem as Profile;

            // Extract the ID of the selected item
            String selectedProfilePath = selectedProfile.Path;
            //TODO remake this
            List<String> allTheModsFullName = Extractor.GetManifestFullNames(selectedProfilePath); //OK
            Http.AddAllToDatabase(allTheModsFullName).Wait();
            List<Mod> allTheMods = ModDBMS.GetAllModFromDatabaseByFullName(allTheModsFullName); //NOT OK
            List<Category> categories = CategoriesHandles.GetAllCategory(allTheMods);
            
            
            //Put the text to what it is
            FinalResult.Text = Formaters.FormatForDiscord(categories);
        }
    }

    private void CopyButton_OnClick(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(FinalResult.Text);
    }
    /*private void MyWindow_Activated(object sender, EventArgs e)
    {
        // Code to execute when the window becomes the active window
        Console.WriteLine("Window activated!");
        RetrieveInformationDatabase();
    }*/


    private void Credits_OnClick(object sender, RoutedEventArgs e)
    {
        //Giving Retrieve Information Database so that it is called after the information is saved
        Credits popup = new Credits();
        popup.Owner = this; // Set the owner window to enable proper modal behavior
        popup.ShowDialog(); // Show the popup window
        
    }
    /// <summary>
    /// Will create the database if they do not exist
    /// </summary>
    private void InitialiseDatabaseIfNotExist()
    {
        ProfileDBMS.InitializeDatabase();
        ModDBMS.InitializeDatabase();
    }
    
}