using Dal;
using DO;
using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        static readonly string s_tasks_xml = "tasks";
        static readonly string s_engineers_xml = "engineers";
        static readonly string s_dependence_xml = "dependences";
        static readonly string s_config_xml = "data-config";
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void btnHandleEngineers(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void btnInitialization(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(GetWindow(this), "Are you sure you want to initialize the system?", "Data Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Initialization", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void btnReset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(GetWindow(this), "Are you sure you want to reset the system?", "Data reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            try
            {
                // reset data conigfile
                List<DO.Engineer> engineersClear = new List<DO.Engineer>();
                List<Dependence> dependenceClear = new List<Dependence>();
                List<DO.Task> tasksClear = new List<DO.Task>();
                XMLTools.SaveListToXMLSerializer<DO.Engineer>(engineersClear, s_engineers_xml);
                XMLTools.SaveListToXMLSerializer<DO.Task>(tasksClear, s_tasks_xml);
                XMLTools.SaveListToXMLSerializer<Dependence>(dependenceClear, s_dependence_xml);

                XElement configRestart = XMLTools.LoadListFromXMLElement(s_config_xml);
                configRestart.Element("NextTaskId")!.Value = "1";
                configRestart.Element("NextDependenceId")!.Value = "1";
                configRestart.Element("ProjectStartDate")!.Value = "";
                XMLTools.SaveListToXMLElement(configRestart, s_config_xml);

                MessageBox.Show("System has been restarted", "Restart", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Reset", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHandleTasks(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void SchdualeProjetcBtn(object sender, RoutedEventArgs e)
        {

        }
    }
}
