using PL.Engineer;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHandleEngineers(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();    
        }

        private void btnInitialization(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show( GetWindow(this), "Are you sure you want to initialize the system?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.Do();
                MessageBox.Show("System has been initialized", "Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}