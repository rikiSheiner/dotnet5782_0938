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
using BL.BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for LogInCustomerWindow.xaml
    /// </summary>
    public partial class LogInCustomerWindow : Window
    {
        private IBL mainData;
        public LogInCustomerWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            closeWindow.Click += closeWindow_Click;
            ButtonLogIn.MouseDoubleClick += ButtonLogIn_MouseDoubleClick;
        }
        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonLogIn_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                int id = int.Parse(currentID.Text);
                if (mainData.FindCustomer(id )>=0)
                {
                    new CustomerActionsWindow(mainData, mainData.ConvertCustomerToCustomerInList(mainData.FindAndGetCustomer(id))).Show ();
                    Close();
                }
                else
                {
                    MessageBox.Show("wrong ID number of customer. Please try again");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }
    }
}
