using BO;
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
    /// Interaction logic for CreateAManager.xaml
    /// </summary>
    public partial class CreateAManager : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

        public CreateAManager()
        {
            IsValid = true; // for the red border
            InitializeComponent();
            Email = "";
            PassWord = "";
        }

        public string Email // get list of all engineers
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(CreateAManager), new PropertyMetadata(null));

        public string PassWord // get password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(CreateAManager), new PropertyMetadata(null));

        public bool IsValid // validiation informer
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(CreateAManager), new PropertyMetadata(null));


        private void LoginBtn(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PassWord)) // if the fields are not empty
            {
                try
                {
                    s_bl.Manager.CreateManager(Email, PassWord);
                    MessageBox.Show("Manager Created");
                    this.Close();
                    new ManagerWindow().Show();
                }
                catch (BlBadEmailException ex)
                {
                    IsValid = false;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void IsValidOk(object sender, TextChangedEventArgs e)
        {
            IsValid = true;

        }
    }
}
