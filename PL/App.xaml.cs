using System.Configuration;
using System.Data;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    public class EngineerSelectedEventArgs : EventArgs
    {
        public int SelectedEngineerId { get; set; }
    }

}
