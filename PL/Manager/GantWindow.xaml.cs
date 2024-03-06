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

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GantWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance

        public GantWindow()
        {

            InitializeComponent();
            TaskList = s_bl.Task.GetTasksList(null)
                    .OrderBy(task => task.ScheduledDate)
                    .ThenBy(task => task.Id)
                    .ToList();

            try
            {
                DatesBetween = GetDateRange(s_bl.Looz.GetStartDate()!.Value, s_bl.Looz.GetEndDate()!.Value);
            }
            catch
            {
                MessageBox.Show("Failed to open", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        public IEnumerable<BO.Task> TaskList // get list of all tasks
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(GantWindow), new PropertyMetadata(null));

        public IEnumerable<DateTime> DatesBetween // get list of all dates
        {
            get { return (IEnumerable<DateTime>)GetValue(DatesBetweenProperty); }
            set { SetValue(DatesBetweenProperty, value); }
        }

        public static readonly DependencyProperty DatesBetweenProperty =
            DependencyProperty.Register("DatesBetween", typeof(IEnumerable<DateTime>), typeof(GantWindow), new PropertyMetadata(null));

        
        
        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            List<DateTime> dateRange = new List<DateTime>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dateRange.Add(date);
            }

            return dateRange;
        }


    }



}
