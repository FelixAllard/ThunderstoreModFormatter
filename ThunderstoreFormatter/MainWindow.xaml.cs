﻿using System.Collections.ObjectModel;
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
using ThunderstoreFormatter.SQLite.DatabaseContext;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public List<Profile> currentPaths { get; set; } = new List<Profile>();
    public List<CheckBoxItem> checkBoxItems { get; set; } = new List<CheckBoxItem>();
    
    public MainWindow()
    {
        var connection = new SqliteConnection("Data Source = Sample.db");

        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        connection.Open();
        
        
        InitializeComponent();
        
            
            
        //TODO fix this thing here, this is the tag checkboxes
         checkBoxItems = new List<CheckBoxItem>
        {
            new CheckBoxItem { CheckboxText = "Checkbox 1", Number = "25" },
            new CheckBoxItem { CheckboxText = "Checkbox 2", Number = "13" },
            // Add more items as needed
        };
         
         
         
         
        // Set the data context of the ListBox to the collection of items
        CheckBoxListBox.ItemsSource = checkBoxItems;
        
        
        
        using (var context = new ProfileDbContext())
        {
            // Create the database if it doesn't exist
            context.Database.EnsureCreated();
        }
        RetrieveInformationDatabase();
    }

    public void RetrieveInformationDatabase()
    {
        using (var context = new ProfileDbContext())
        {
            currentPaths.Clear();  // Clear the list to avoid duplicate entries
            currentPaths.AddRange(context.Profiles.ToList());  // Add the new data from the database
            foreach (var profile in currentPaths)
            {
                Console.WriteLine($"ID: {profile.ID}, Name: {profile.ProfileName}, Mods: {profile.NumberMods}, Path: {profile.Path}");
            }
        }
        ViewProfile.ItemsSource = currentPaths;
    }
    private void StartQuerrying_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

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

            // Extract the ID of the selected item
            int selectedProfileId = selectedProfile.ID;

            // Remove the selected item from the database
            using (var context = new ProfileDbContext())
            {
                var profileToRemove = context.Profiles.FirstOrDefault(p => p.ID == selectedProfileId);
                if (profileToRemove != null)
                {
                    context.Profiles.Remove(profileToRemove);
                    context.SaveChanges();
                }

                // Reset the auto-increment IDs (for SQLite)
            }

            // Remove the selected item from the collection
            currentPaths.Remove(selectedProfile);

            // Refresh the ListView
            ViewProfile.Items.Refresh();
            RetrieveInformationDatabase();
        }
    }
}