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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL.BlApi;
using BL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL mainData = BlFactory.GetBl ();
        public MainWindow()
        {
            InitializeComponent();
            SignUp.MouseDoubleClick += SignUp_MouseDoubleClick;
            LogIn.MouseDoubleClick += LogIn_MouseDoubleClick;
            LogInCustomer.MouseDoubleClick += LogInCustomer_MouseDoubleClick;
            SignUpCustomer.MouseDoubleClick += SignUpCustomer_MouseDoubleClick;
        }


        private void SignUp_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(mainData ).Show();
            Close();
        }

        private void LogIn_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new LogInWindow(mainData ).Show();
            Close();
        }

        private void LogInCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new LogInCustomerWindow(mainData).Show();
            Close();
        }

        private void SignUpCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new SignUpCustomerWindow(mainData).Show();
            Close();
        }
    }
}
