using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using BlApi;
namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>

    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All; // get the experience of the engineer
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl.Engineer.GetEngineersList(null!); // get list of all engineers //check if null is ok
        }

        private void AddEngineerWindow(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(0).ShowDialog(); // open the engineer window
            RefreshList();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //check what is this button
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //check what is this button
        {

        }

        public IEnumerable<BO.Engineer> EngineerList // get list of all engineers
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));



        private void OnSelectExperience(object sender, SelectionChangedEventArgs e) // select experience of the engineer
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ? s_bl?.Engineer.GetEngineersList(null!)!
                    : s_bl?.Engineer.GetEngineersList(engineer => engineer.Level == Experience)!;
        }

        private void SelectEngineerToUpdate(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
            int id = engineerInList!.Id!.Value; // convert from int? to int
            new EngineerWindow(id).ShowDialog(); // open the engineer window
            RefreshList();
        }

        private void RefreshList()
        {
            EngineerList = s_bl.Engineer.GetEngineersList(engineer => engineer.Level == Experience); // get list of all engineers
        }
    }
}


