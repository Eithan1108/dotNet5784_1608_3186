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
using System.Text.RegularExpressions;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        private bool idIndicator;
        public EngineerWindow(int id = 0)
        {
            InitializeComponent();
            if (id == 0)
            {
                Engineer = new BO.Engineer { Id = 0, Name = null, Email = null, Level = BO.EngineerExperience.Beginner, Cost = 0 };
                idIndicator = false;
            }
            else
            {
                try
                {
                    Engineer = s_bl!.Engineer.GetEngineer(id);
                    idIndicator = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
        }

        public BO.Engineer Engineer // get list of all engineers
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnSaveOrUpdateEngineer(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!idIndicator)
                {
                    s_bl.Engineer.AddEngineer(Engineer);
                    MessageBox.Show("Engineer with id " + Engineer.Id +  " successfuly added to the system", "Successfuly Add Engineer ", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Engineer.UpdateEngineer(Engineer);
                    MessageBox.Show("Engineer with id " + Engineer.Id + " successfuly updated in the system", "Successfuly Update Engineer ", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
