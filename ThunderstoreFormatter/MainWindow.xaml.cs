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
using System.Windows.Threading;
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

        public MainWindow()
        {
            // Initialize Database
            var connection = new SqliteConnection("Data Source=Sample.db");
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            connection.Open();

            // Must be called 
            CategoriesHandles.Init();

            // Initialize Page
            InitializeComponent();

            // Attach progress bar :
            Http.ProgressUpdate = UpdateProgressBar;

            InitialiseDatabaseIfNotExist();
            ProfileDBMS.CheckAllProfile();
            RetrieveInformationDatabase();
            PopulateCategoriesListBox();
            CheckCategories();
        }

        private void UpdateProgressBar(int progressValue)
        {
            Dispatcher.Invoke(() => progressBar.Value = progressValue);
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

        private void AddProfileButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Giving Retrieve Information Database so that it is called after the information is saved
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

        private async void ButtonShow_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewProfile.SelectedItem != null)
            {
                // Get the selected item
                Profile selectedProfile = ViewProfile.SelectedItem as Profile;
                // Extract the ID of the selected item
                string selectedProfilePath = selectedProfile.Path;
                List<string> allTheModsFullName = Extractor.GetManifestFullNames(selectedProfilePath); // OK
                
                // Run AddAllToDatabase on a background thread
                await Task.Run(() => Http.AddAllToDatabase(allTheModsFullName));

                List<Mod> allTheMods = ModDBMS.GetAllModFromDatabaseByFullName(allTheModsFullName); // OK
                List<Category> categories = CategoriesHandles.GetAllCategory(allTheMods); // OK
                // Put the text to what it is
                FinalResult.Text = Formaters.FormatForDiscord(categories);
            }
        }

        private void CopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(FinalResult.Text);
        }

        private void Credits_OnClick(object sender, RoutedEventArgs e)
        {
            // Giving Retrieve Information Database so that it is called after the information is saved
            Credits popup = new Credits();
            popup.Owner = this; // Set the owner window to enable proper modal behavior
            popup.ShowDialog(); // Show the popup window
        }

        private void InitialiseDatabaseIfNotExist()
        {
            ProfileDBMS.InitializeDatabase();
            ModDBMS.InitializeDatabase();
        }
        
        
        
        
        
        private void PopulateCategoriesListBox()
        {
            var enumValues = Enum.GetValues(typeof(Categories)).Cast<Categories>();
            var checkBoxItems = enumValues.Select(category => new CheckBoxItem { Content = category.ToString() }).ToList();
            CheckBoxListBox.ItemsSource = checkBoxItems;
        }

        private void CheckCategories()
        {
            foreach (var item in CheckBoxListBox.Items)
            {
                if (item is CheckBoxItem checkBoxItem && Enum.TryParse(checkBoxItem.Content.ToString(), out Categories category))
                {
                    string stringValue;
                    bool isChecked = CategoriesHandles.CurrentlyDisplayedCategories.TryGetValue(category, out stringValue) &&
                                     bool.TryParse(stringValue, out bool boolValue) &&
                                     boolValue;
                    checkBoxItem.IsChecked = isChecked;
                }
            }
        }
        


        public class CheckBoxItem : CheckBox
        {
            public bool IsChecked { get; set; }
        }
    }