using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance


        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl.Task.TaskToTaskInListConverter(s_bl.Task.GetTasksList(null)); // get list of all tasks //check if null is ok
        }

        public IEnumerable<BO.TaskInList> TaskList // get list of all tasks
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        private void btnAddUpdate(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
            RefreshList();
        }
        

        private void RefreshList()
        {
            TaskList = s_bl.Task.TaskToTaskInListConverter(s_bl.Task.GetTasksList(null));
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
    }
    
    
}
