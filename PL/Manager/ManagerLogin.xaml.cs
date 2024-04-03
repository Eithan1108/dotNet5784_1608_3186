using BO;
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
    /// Interaction logic for ManagerLogin.xaml
    /// </summary>
    public partial class ManagerLogin : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

        public ManagerLogin()
        {
            IsOk = true;
            InitializeComponent();
        }

        public string PassWord // get list of all engineers
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        } 

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(ManagerLogin), new PropertyMetadata(null));

        public bool IsOk // get list of all engineers
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsOk", typeof(bool), typeof(ManagerLogin), new PropertyMetadata(null));

        private void LoginBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                if(s_bl.Manager.ManagerLogIn(PassWord))
                {
                    new ManagerWindow().Show();
                    this.Close();
                }
                else
                {
                    IsOk = false;
                    MessageBox.Show("Password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IsValidOk(object sender, TextChangedEventArgs e)
        {
            IsOk = true;
        }

        private void ForgetPasswordBtn(object sender, RoutedEventArgs e)
        {
            new ResetPassWordWindow().Show();
        }
    }
}
