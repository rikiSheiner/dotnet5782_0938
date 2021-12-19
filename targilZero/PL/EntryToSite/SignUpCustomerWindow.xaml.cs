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
using BL.BO;
using DAL.DalApi.DO;

namespace PL
{
    /// <summary>
    /// Interaction logic for SignUpCustomerWindow.xaml
    /// </summary>
    public partial class SignUpCustomerWindow : Window
    {
        private IBL mainData;
        public SignUpCustomerWindow(IBL data)
        {
            InitializeComponent();

            mainData = data;
            addTheCustomer.MouseDoubleClick += addTheCustomer_MouseDoubleClick;
        }

        private void addTheCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.AddCustomer(int.Parse(newID.Text.ToString()), newName.Text, newPhoneNum.Text, double.Parse(newLongitude.Text.ToString()), double.Parse(newLatitude.Text.ToString()));
                MessageBox.Show("Congratulation! welcome to our site " + newName .Text);
                var customer = new DAL.DalApi.DO.Customer(int.Parse(newID.Text.ToString()), newName.Text, newPhoneNum.Text, double.Parse(newLongitude.Text.ToString()), double.Parse(newLatitude.Text.ToString()));
                new CustomerActionsWindow(mainData ,mainData .ConvertCustomerToCustomerInList (customer) ).Show ();
                Close();
            }
            catch (AddingProblemException addingProblem)
            {
                MessageBox.Show(addingProblem.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }
    }
}
