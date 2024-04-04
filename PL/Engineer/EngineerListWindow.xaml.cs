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
using PL.Task;
namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>

    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        public BO.EngineerExperienceWithAll Experience { get; set; } = BO.EngineerExperienceWithAll.All; // get the experience of the engineer
        public EngineerListWindow()
        {
            try
            {
                ProjectStarted = s_bl.projectStarted();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            InitializeComponent();
            EngineerList = s_bl.Engineer.GetEngineersList(null!).OrderBy(engineer => engineer.Id); // get list of all engineers //check if null is ok
        }

        private void AddEngineerWindow(object sender, RoutedEventArgs e)
        {
            if (ProjectStarted)
                MessageBox.Show("Project already started", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new EngineerWindow(0).ShowDialog(); // open the engineer window
                RefreshList();
            }
        }


        public bool ProjectStarted // get list of all engineers
        {
            get { return (bool)GetValue(ProjectStartedProperty); } 
            set { SetValue(ProjectStartedProperty, value); } 
        }

        public static readonly DependencyProperty ProjectStartedProperty =
            DependencyProperty.Register("ProjectStarted", typeof(bool), typeof(EngineerListWindow), new PropertyMetadata(null)); 

        public IEnumerable<BO.Engineer> EngineerList // get list of all engineers
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));



        private void OnSelectExperience(object sender, SelectionChangedEventArgs e) // select experience of the engineer
        {
            EngineerList = (Experience == BO.EngineerExperienceWithAll.All) ? s_bl?.Engineer.GetEngineersList(null!)!
                    : s_bl?.Engineer.GetEngineersList(engineer => (int)engineer.Level == (int)Experience).OrderBy(engineer => engineer.Id)!;
        }

        private void SelectEngineerToUpdate(object sender, MouseButtonEventArgs e)
        {
            if (ProjectStarted)
                MessageBox.Show("Project already started", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                BO.Engineer? engineerInList = (sender as ListView)?.SelectedItem as BO.Engineer; 
                int id = engineerInList!.Id!.Value; // convert from int? to int
                new EngineerWindow(id).ShowDialog(); // open the engineer window
                RefreshList();
            }

        }
        private void RefreshList()
        {
            EngineerList = (Experience == BO.EngineerExperienceWithAll.All) ? s_bl?.Engineer.GetEngineersList(null!).OrderBy(engineer => engineer.Id)!
                       : s_bl?.Engineer.GetEngineersList(engineer => (int)engineer.Level == (int)Experience).OrderBy(engineer => engineer.Id)!;
        }

        private void SearchForContext(object sender, TextChangedEventArgs e) //search bar
        {
            
            EngineerList = s_bl.Engineer.GetEngineersList(engineer => engineer.Name.StartsWith(TextChanged.Text) || engineer.Name.ToLower().StartsWith(TextChanged.Text) || engineer.Email.StartsWith(TextChanged.Text) || engineer.Id.ToString()!.StartsWith(TextChanged.Text)).OrderBy(engineer => engineer.Id); // get list of all engineers that match the search text 
        }
    }

}


