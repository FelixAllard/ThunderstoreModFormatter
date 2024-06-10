using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace ThunderstoreFormatter;

public partial class Credits : Window
{
    public Credits()
    {
        InitializeComponent();
    }
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Hyperlink hyperlink = (Hyperlink)sender;
        Process.Start(new ProcessStartInfo
        {
            FileName = hyperlink.NavigateUri.AbsoluteUri,
            UseShellExecute = true
        });
        e.Handled = true;
    }
}