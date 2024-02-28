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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for IndividualEngineerScreen.xaml
    /// </summary>
    public partial class IndividualEngineerScreen : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance

        public IndividualEngineerScreen(BO.Engineer individualEngineer)
        {
            InitializeComponent();
            Engineer = individualEngineer;
            TaskInEngineerList = s_bl.Task.GetTasksList(task => task.Engineer!= null && task.Engineer.Id == Engineer.Id);
            if(Engineer.Task != null)
                WorkingTask = s_bl.Task.GetTask(Engineer.Task.Id!.Value);
        }

        public BO.Engineer Engineer // get list of all engineers
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(IndividualEngineerScreen), new PropertyMetadata(null));


        public IEnumerable<BO.Task> TaskInEngineerList // get list of all engineers
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskInEngineerListProperty); }
            set { SetValue(TaskInEngineerListProperty, value); }
        }

        public static readonly DependencyProperty TaskInEngineerListProperty =
            DependencyProperty.Register("TaskInEngineerList", typeof(IEnumerable<BO.Task>), typeof(IndividualEngineerScreen), new PropertyMetadata(null));



        public BO.Task WorkingTask // get list of all engineers
        {
            get { return (BO.Task)GetValue(WorkingTaskProperty); }
            set { SetValue(WorkingTaskProperty, value); }
        }

        public static readonly DependencyProperty WorkingTaskProperty =
            DependencyProperty.Register("WorkingTask", typeof(BO.Task), typeof(IndividualEngineerScreen), new PropertyMetadata(null));


        //private void StartEngineer(object sender, MouseButtonEventArgs e)
        //{
        //    BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
        //    if (task != null)
        //    {
        //        try
        //        {
        //            s_bl.Task.StartTask(task.Id); 
        //            // TODO: update the task with StartTime Date.Now



        //            // Refresh the engineer
        //            Engineer = s_bl.Engineer.GetEngineer(Engineer.Id!.Value);


        //            MessageBox.Show("Task started successfully");
        //        }
        //        catch (BO.BlNotExistsException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}


        private void CompleteTaskBtn(object sender, RoutedEventArgs e)
        {
            // TODO: update the task with CompleteDate Date.Now

            if (Engineer.Task == null) return;

                s_bl.Task.StopTask(Engineer.Task.Id!.Value); // stop the task
            
               
            // Refresh the engineer
            Engineer = s_bl.Engineer.GetEngineer(Engineer.Id!.Value);
        }

        private void StartEngineerBtn(object sender, MouseButtonEventArgs e)
        {
            //EngineerListForTask engineerListForTask = new EngineerListForTask(Engineer.Level);
            //engineerListForTask.EngineerSelected += (sender, args) =>
            //{
            //    Engineer = s_bl.Engineer.GetEngineer(args.SelectedEngineerId);
            //    TaskInEngineerList = s_bl.Task.GetTasksList(task => task.Engineer != null && task.Engineer.Id == Engineer.Id);
            //    if (Engineer.Task != null)
            //        WorkingTask = s_bl.Task.GetTask(Engineer.Task.Id!.Value);
            //};

            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;

            if (task == null)
            {
                return;
            }




            try
            {
                s_bl.Task.StartTask(task.Id, Engineer.Id!.Value); // start the task
                MessageBox.Show("Task started successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
