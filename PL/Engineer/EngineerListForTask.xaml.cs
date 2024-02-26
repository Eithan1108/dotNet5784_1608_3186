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
    /// Interaction logic for EngineerListForTask.xaml
    /// </summary>
    public partial class EngineerListForTask : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance
        public EngineerListForTask(BO.EngineerExperience? experience)
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.GetEngineersList(engineer => engineer.Level >= experience)!;
        }

        public IEnumerable<BO.Engineer> EngineerList // get list of all engineers
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListForTask), new PropertyMetadata(null));

        private void SelectedEngineer(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
            int id = engineerInList!.Id!.Value; // convert from int? to int

        }
    }
}
