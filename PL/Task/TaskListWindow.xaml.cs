using PL.Engineer;
using PL.Manager;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All; // get the experience of the engineer
        public BO.Status Status { get; set; } = BO.Status.All; // get the status of the task


        public TaskListWindow()
        {
            ProjectStarted = s_bl.projectStarted();
            InitializeComponent();
            TaskList = s_bl.Task.TaskToTaskInListConverter(s_bl.Task.GetTasksList(null)).OrderBy(task => task.Id); // get list of all tasks //check if null is ok
        }

        public IEnumerable<BO.TaskInList> TaskList // get list of all tasks
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));



        public bool ProjectStarted // get list of all engineers
        {
            get { return (bool)GetValue(ProjectStartedProperty); }
            set { SetValue(ProjectStartedProperty, value); }
        }

        public static readonly DependencyProperty ProjectStartedProperty =
            DependencyProperty.Register("ProjectStarted", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));





        private void btnAddUpdate(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
            RefreshList();
        }
        

        private void RefreshList()
        {
            TaskList = s_bl.Task.TaskToTaskInListConverter(s_bl.Task.GetTasksList(null)).OrderBy(task => task.Id);
            //EngineerList = (Experience == BO.EngineerExperience.All) ? s_bl?.Engineer.GetEngineersList(null!)!
            //           : s_bl?.Engineer.GetEngineersList(engineer => engineer.Level == Experience)!;
        }

        private void SelectToUpdate(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                //int id = task!.Id; // convert from int? to int
                new TaskWindow(task!.Id).ShowDialog(); // open the engineer window
                RefreshList();
            }
        }

        private void OnSelectExperience(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.All) ? s_bl.Task.TaskToTaskInListConverter(s_bl?.Task.GetTasksList(null!).OrderBy(task => task.Id)!)
        :               s_bl.Task.TaskToTaskInListConverter(s_bl?.Task.GetTasksList(task => task.Complexity == Experience)!)!;
        }
    }
    
    
}
