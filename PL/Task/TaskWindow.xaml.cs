using DO;
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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        private bool idIndicator;
        private IEnumerable<BO.Task> dependenceList;


        public TaskWindow(int id = 0)
        {
            Idx = id;
            InitializeComponent();
            if (id == 0)
            {
                Task = new BO.Task { Id = 0, Description = "", Alias = "", Dependencies = s_bl.Task.TaskToTaskInListConverter(dependenceList!), RequiredEffortTime = null, Deliverables = "", Remarks = "", Complexity = BO.EngineerExperience.Beginner};
                idIndicator = false;
                try
                {
                    TaskList = s_bl.Task.GetTasksList(null!);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            else
            {
               
                try
                {
                    Task = s_bl!.Task.GetTask(id);
                    idIndicator = true;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }

        }

        public BO.Task Task // get list of all tasks
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public int Idx // get list of all tasks
        {
            get { return (int)GetValue(IdxProperty); }
            set { SetValue(IdxProperty, value); }
        }
        public IEnumerable<BO.Task> TaskList // get list of all tasks
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty IdxProperty =
           DependencyProperty.Register("Idx", typeof(int), typeof(TaskWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TaskWindow), new PropertyMetadata(null));

       

        public DateTime SelectedDate
        {
            get; set;
        }


        private void OnSaveOrUpdateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!idIndicator)
                {
                    s_bl.Task.AddTask(Task);
                    MessageBox.Show("Task with id " + Task.Id + " successfuly added to the system", "Successfuly Add Task ", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    s_bl.Task.UpdateTask(Task);
                    MessageBox.Show("Task with id " + Task.Id + " successfuly updated in the system", "Successfuly Update Task ", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddToDependency(object sender, RoutedEventArgs e)
        {
            // dependenceList.Append(s_bl.Task.GetTask(Tag));
            
            
        }
        
        private void RemoveFromDependencyList(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
