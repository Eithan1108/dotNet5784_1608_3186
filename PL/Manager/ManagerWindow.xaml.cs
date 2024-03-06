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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

        public ManagerWindow()
        {
            ProjectStarted = s_bl.projectStarted();
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
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.Reset();
                        DalTest.Initialization.Do();
                        ProjectStarted = s_bl.projectStarted();
                        MessageBox.Show("System has been initialized", "Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Initialization", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        private void btnReset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(GetWindow(this), "Are you sure you want to reset the system?", "Data reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Reset();
                    ProjectStarted = s_bl.projectStarted();
                    MessageBox.Show("System has been restarted", "Restart", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Reset", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        public bool ProjectStarted // get list of all engineers
        {
            get { return (bool)GetValue(ProjectStartedProperty); }
            set { SetValue(ProjectStartedProperty, value); }
        }

        public static readonly DependencyProperty ProjectStartedProperty =
            DependencyProperty.Register("ProjectStarted", typeof(bool), typeof(ManagerWindow), new PropertyMetadata(null));

        private void btnHandleTasks(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void SchdualeProjetcBtn(object sender, RoutedEventArgs e)
        {

            ScheduleProjectWindow scheduleProjectWindow = new ScheduleProjectWindow();

            // Subscribe to the Closed event of the window
            scheduleProjectWindow.Closed += (windowSender, windowArgs) =>
            {
                // Update ProjectStarted after the window is closed
                ProjectStarted = s_bl.projectStarted();
            };

            // Show the window
            scheduleProjectWindow.Show();

        }

        private void GantWindowBtn(object sender, RoutedEventArgs e)
        {
            new GantWindow().Show();
        }
    }
}
