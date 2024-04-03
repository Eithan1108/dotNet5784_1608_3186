using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for ResetPassWordWindow.xaml
    /// </summary>
    public partial class ResetPassWordWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        int randomConfirmNumber;
        string managerEmail = "";
        public ResetPassWordWindow()
        {
            NotConfirmed = true;
            randomConfirmNumber = new Random().Next(1000, 9999);
            try
            {
                managerEmail = s_bl.Manager.GetManagerEmail();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ManagerEmail = managerEmail;

            s_bl.Manager.SendEmail(randomConfirmNumber);


            InitializeComponent();
        }

        
        public string ManagerEmail // get list of all engineers
        {
            get { return (string)GetValue(ManagerEmailProperty); }
            set { SetValue(ManagerEmailProperty, value); }
        }

        public static readonly DependencyProperty ManagerEmailProperty =
            DependencyProperty.Register("ManagerEmail", typeof(string), typeof(ResetPassWordWindow), new PropertyMetadata(null));

        public int VerificationCode // get list of all engineers
        {
            get { return (int)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("VerificationCode", typeof(int), typeof(ResetPassWordWindow), new PropertyMetadata(null));

        public string NewPassWord // get list of all engineers
        {
            get { return (string)GetValue(NewPassWordProperty); }
            set { SetValue(NewPassWordProperty, value); }
        }

        public static readonly DependencyProperty NewPassWordProperty =
            DependencyProperty.Register("NewPassWord", typeof(string), typeof(ResetPassWordWindow), new PropertyMetadata(null));

        public string ConfirmNewPassWord // get list of all engineers
        {
            get { return (string)GetValue(ConfirmNewPassWordProperty); }
            set { SetValue(ConfirmNewPassWordProperty, value); }
        }

        public static readonly DependencyProperty ConfirmNewPassWordProperty =
            DependencyProperty.Register("ConfirmNewPassWord", typeof(string), typeof(ResetPassWordWindow), new PropertyMetadata(null));

        public bool NotConfirmed // get list of all engineers
        {
            get { return (bool)GetValue(NotConfirmedProperty); }
            set { SetValue(NotConfirmedProperty, value); }
        }

        public static readonly DependencyProperty NotConfirmedProperty =
            DependencyProperty.Register("NotConfirmed", typeof(bool), typeof(ResetPassWordWindow), new PropertyMetadata(null));





        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void VerifyBtn(object sender, RoutedEventArgs e)
        {
            if(VerificationCode != randomConfirmNumber)
            {
                MessageBox.Show("Verification code is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Verification code is correct", "Correct", MessageBoxButton.OK, MessageBoxImage.Information);
                NotConfirmed = false; // if the verification code is correct the user can change the password
            } 
        }

        private void ResetPassword(object sender, RoutedEventArgs e)
        {
            if (NewPassWord != ConfirmNewPassWord)
            {
                MessageBox.Show("Password confirm is incorrect, try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    s_bl.Manager.SetManagerPassWord(NewPassWord);
                    MessageBox.Show("Password successfully changed", "Successfully Change Password", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
