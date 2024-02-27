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
    /// Interaction logic for EngineerLogInWindow.xaml
    /// </summary>
    public partial class EngineerLogInWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance

        public EngineerLogInWindow()
        {
            InitializeComponent();
        }

        public int EngineerID // get list of all engineers
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("EngineerID", typeof(int), typeof(EngineerLogInWindow), new PropertyMetadata(null));

        private void Enter(object sender, RoutedEventArgs e)
        {
            try
            {
                new IndividualEngineerScreen(s_bl.Engineer.GetEngineer(EngineerID)).Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Engineer not exist", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
