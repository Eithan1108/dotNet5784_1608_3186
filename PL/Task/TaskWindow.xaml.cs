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
        public TaskWindow(int id=0)
        {
            InitializeComponent();
            if(id==0)
            {
                Task = new BO.Task { Description = " ", Alias = " ", Dependencies = null, RequiredEffortTime=null, StartDate=null, ScheduledDate=null, CompleteDate=null, Deliverables=" ", Remarks=" ", Engineer = null, Complexity = BO.EngineerExperience.Beginner };
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

        public IEnumerable<BO.Task> TaskList // get list of all tasks
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TaskWindow), new PropertyMetadata(null));

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
