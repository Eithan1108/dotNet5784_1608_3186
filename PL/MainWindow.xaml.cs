﻿using Dal;
using DO;
using PL.Engineer;
using PL.Manager;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using BO;
using BlApi;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

        public MainWindow()
        {
            InitializeComponent();
            ScreenDate = s_bl.Clock;


        }


        public DateTime ScreenDate // get list of all engineers
        {
            get { return (DateTime)GetValue(ScreenDateProperty); }
            set { SetValue(ScreenDateProperty, value); }
        }

        public static readonly DependencyProperty ScreenDateProperty =
            DependencyProperty.Register("ScreenDate", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));




        private void btnHandleEngineers(object sender, RoutedEventArgs e)
        {
            new EngineerLogInWindow().Show();   
        }

        private void btnInitialization(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().Show();
        }

        private void AddAnHour(object sender, RoutedEventArgs e)
        {
            s_bl.AddHourInPl(1);
            ScreenDate = s_bl.Clock;
        }
        private void AddADay(object sender, RoutedEventArgs e)
        {
            s_bl.AddDaysInPl(1);
            ScreenDate = s_bl.Clock;
        }

    }
}