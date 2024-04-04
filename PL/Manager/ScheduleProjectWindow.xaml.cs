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

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for ScheculeProjectWindow.xaml
    /// </summary>
    public partial class ScheduleProjectWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

        public ScheduleProjectWindow()
        {
            try
            {
                Date = s_bl.Clock;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();

        }

        public DateTime Date // get list of all engineers
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(ScheduleProjectWindow), new PropertyMetadata(null));

        private void SetScheduleProjectBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.AutoScheduleSystem(Date); // call the bl function to schedule the project
                MessageBox.Show("The project has been scheduled successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();

        }
    }
}
