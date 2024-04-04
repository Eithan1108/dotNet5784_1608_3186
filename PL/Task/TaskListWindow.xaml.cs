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
        public BO.EngineerExperienceWithAll Experience { get; set; } = BO.EngineerExperienceWithAll.All; // get the experience of the engineer
        public BO.Status Status { get; set; } = BO.Status.All; // get the status of the task


        public TaskListWindow()
        {
            try
            {
                ProjectStarted = s_bl.projectStarted();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
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


        public string SearchFor // get list of all engineers
        {
            get { return (string)GetValue(SearchForProperty); }
            set { SetValue(SearchForProperty, value); }
        }

        public static readonly DependencyProperty SearchForProperty =
            DependencyProperty.Register("SearchFor", typeof(string), typeof(TaskListWindow), new PropertyMetadata(null));


        private void btnAddUpdate(object sender, RoutedEventArgs e)
        {
            if (ProjectStarted)
                MessageBox.Show("Project already started", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new TaskWindow().ShowDialog();
                RefreshList();
            }
        }
        

        private void RefreshList()
        {
            TaskList = s_bl.Task.TaskToTaskInListConverter(s_bl.Task.GetTasksList(null)).OrderBy(task => task.Id);
            //EngineerList = (Experience == BO.EngineerExperience.All) ? s_bl?.Engineer.GetEngineersList(null!)!
            //           : s_bl?.Engineer.GetEngineersList(engineer => engineer.Level == Experience)!;
        }

        private void SelectToUpdate(object sender, MouseButtonEventArgs e)
        {
            if (ProjectStarted)
                MessageBox.Show("Project already started", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
                if (task != null)
                {
                    //int id = task!.Id; // convert from int? to int
                    new TaskWindow(task!.Id).ShowDialog(); // open the engineer window
                    RefreshList();
                }
            }

        }

        private void OnSelectExperience(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperienceWithAll.All) ? s_bl.Task.TaskToTaskInListConverter(s_bl?.Task.GetTasksList(null!).OrderBy(task => task.Id)!)
        :               s_bl.Task.TaskToTaskInListConverter(s_bl?.Task.GetTasksList(task => (int)task.Complexity == (int)Experience)!)!;
        }

        private void SearchForContext(object sender, TextChangedEventArgs e)
        {
            // SearchFor = sender!.ToString()!;
            TaskList = s_bl.Task.TaskToTaskInListConverter(s_bl.Task.GetTasksList(task => task.Description.StartsWith(TextChanged.Text) || task.Description.ToLower().StartsWith(TextChanged.Text) || task.Alias.StartsWith(TextChanged.Text) || task.Alias.ToLower().StartsWith(TextChanged.Text) || task.Id.ToString().StartsWith(TextChanged.Text))).OrderBy(task => task.Id);
        }



    }
    
    
}
