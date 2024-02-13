using Dal;
using DO;
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
using System.Xml.Linq;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        static readonly string s_tasks_xml = "tasks";
        static readonly string s_engineers_xml = "engineers";
        static readonly string s_dependence_xml = "dependences";
        static readonly string s_config_xml = "data-config";
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
                try
                {
                    List<DO.Engineer> engineersClear = new List<DO.Engineer>();
                    List<Dependence> dependenceClear = new List<Dependence>();
                    List<DO.Task> tasksClear = new List<DO.Task>();
                    XMLTools.SaveListToXMLSerializer<DO.Engineer>(engineersClear, s_engineers_xml);
                    XMLTools.SaveListToXMLSerializer<DO.Task>(tasksClear, s_tasks_xml);
                    XMLTools.SaveListToXMLSerializer<Dependence>(dependenceClear, s_dependence_xml);
                    // initialize data conigfile
                    XElement configRestart = XMLTools.LoadListFromXMLElement(s_config_xml);
                    configRestart.Element("NextTaskId")!.Value = "1";
                    configRestart.Element("NextDependenceId")!.Value = "1";
                    configRestart.Element("ProjectStartDate")!.Value = "";
                    XMLTools.SaveListToXMLElement(configRestart, s_config_xml);
                    DalTest.Initialization.Do();
                    MessageBox.Show("System has been initialized", "Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Initialization", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}