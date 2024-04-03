using BO;
using DO;
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
using System.Windows.Shell;


namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        private bool idIndicator;
        private IEnumerable<BO.TaskInList> dependenceList;
        private int engineerId = 0;
        
         
        public TaskWindow(int id = 0)
        {
            Idx = id;
            dependenceList = new List<BO.TaskInList>();
            InitializeComponent();
            try
            {
                TaskList = s_bl.Task.GetTasksList(null!).Select(task =>
                new CheckBoxTask
                {
                    Id = task.Id,
                    Description = task.Description,
                    Alias = task.Alias,
                    Status = task.Status,
                    IsChecked = false,

                }).OrderBy(task => task.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            if (id == 0)
            {
                Task = new BO.Task { Id = 0, Description = "", Alias = "", Dependencies = dependenceList, RequiredEffortTime = null, Deliverables = "", Remarks = "", Complexity = BO.EngineerExperience.Beginner, CompleteDate = null, StartDate = null };
                idIndicator = false;
            }
            else
            {

                try
                {
                    Task = s_bl!.Task.GetTask(id);
                    // engineerId = Task.Engineer!.Id.Value;
                    dependenceList = Task.Dependencies!;
                    if (Task.Engineer != null)
                    {
                            EngineerWorking = s_bl.Engineer.GetEngineer(Task.Engineer.Id!.Value);
                    }

                    TaskList = TaskList.Select(task =>
                     new CheckBoxTask
                     {
                         Id = task.Id,
                         Description = task.Description,
                         Alias = task.Alias,
                         Status = task.Status,
                         IsChecked = dependenceList?.Any(dependency => dependency.Id == task.Id) ??false,
                     }).OrderBy(task => task.Id);

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

        public class CheckBoxTask : BO.TaskInList
        {
            public bool IsChecked { get; set; }
        }

        public IEnumerable<CheckBoxTask> TaskList // get list of all tasks
        {
            get { return (IEnumerable<CheckBoxTask>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public BO.Engineer EngineerWorking // get list of all tasks
        {
            get { return (BO.Engineer)GetValue(EngineerWorkingProperty); }
            set { SetValue(EngineerWorkingProperty, value); }
        }

        public static readonly DependencyProperty EngineerWorkingProperty =
            DependencyProperty.Register("EngineerWorking", typeof(BO.Engineer), typeof(TaskWindow), new PropertyMetadata(null));



        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty IdxProperty =
           DependencyProperty.Register("Idx", typeof(int), typeof(TaskWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<CheckBoxTask>), typeof(TaskWindow), new PropertyMetadata(null));


        private void OnSaveOrUpdateTask(object sender, RoutedEventArgs e)
        {
            Task.Dependencies = dependenceList;

            if (engineerId != 0)
            {
                BO.Engineer engineerToUpdae = s_bl.Engineer.GetEngineer(engineerId);
                Task.Engineer = new EngineerInTask { Id = engineerId, Name = engineerToUpdae.Name };
            }

            try
            {
                if (!idIndicator)
                {
                    s_bl.Task.AddTask(Task);
                    MessageBox.Show("Task successfully added to the system.", "Successfully Add Task ", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (Task.Engineer != null)
                    {
                        engineerId = Task.Engineer!.Id!.Value;
                    }
                    s_bl.Task.UpdateTask(Task);
                    MessageBox.Show("Task with id " + Task.Id + " successfully updated in the system", "Successfully Update Task ", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Close();
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
            var Tag = (sender as CheckBox)!.Tag;
            int id = (int)Tag;
            try
            {
                BO.Task tempTask = s_bl.Task.GetTask(id);
                dependenceList = dependenceList.Append(new BO.TaskInList { Id = tempTask.Id, Description = tempTask.Description, Alias = tempTask.Alias, Status = tempTask.Status });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void RemoveFromDependencyList(object sender, RoutedEventArgs e)
        {
            var Tag = (sender as CheckBox)!.Tag;
            int id = (int)Tag;
            try
            {
                dependenceList = dependenceList.Where(task => task.Id != id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get the dependency


        }

        private void SetEngineer(object sender, RoutedEventArgs e) 
        {
            var window = new EngineerListForTask(Task.Complexity);
            window.EngineerSelected += EngineerSelectedHandler; 
            window.ShowDialog();
        }

        private void EngineerSelectedHandler(object sender, EngineerSelectedEventArgs args)
        {
            engineerId = args.SelectedEngineerId;
            try
            {
                EngineerWorking = s_bl.Engineer.GetEngineer(engineerId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

        }
    }
}
