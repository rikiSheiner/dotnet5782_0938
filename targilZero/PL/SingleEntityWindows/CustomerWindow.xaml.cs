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

namespace PL.SingleEntityWindows
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private IBL mainData;
        private CustomerToList customerCurrent;
        /*int id, string name, string phoneNumber, double longitude, double latitude*/

        /// <summary>
        /// constructor for actions on specific customer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="customer"></param>
        public CustomerWindow(IBL data, CustomerToList customer)
        {
            InitializeComponent();
            mainData = data;
            customerCurrent = customer;
            closeWindow.Click += closeWindow_Click;

            //הסתרת כל הכפתורים הקשרוים להוספת לקוח
            newID.Visibility = Visibility.Collapsed;
            newLatitude.Visibility = Visibility.Collapsed;
            newLongitude.Visibility = Visibility.Collapsed;
            newName.Visibility = Visibility.Collapsed;
            newPhoneNum.Visibility = Visibility.Collapsed;

            addTheCustomer.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;
            
            enterID.Visibility = Visibility.Collapsed;
            enterLatitude.Visibility = Visibility.Collapsed;
            enterLongitude.Visibility = Visibility.Collapsed;
            enterName.Visibility = Visibility.Collapsed; 
            enterPhone.Visibility = Visibility.Collapsed;

            CustomerDetails.Text = customerCurrent.ToString();
            updateCustomer.MouseDoubleClick += updateCustomer_MouseDoubleClick;
            submitButton.MouseDoubleClick += submitButton_MouseDoubleClick;
        }

        public CustomerWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();

            //הסתרת כל הכפתורים שקשורים חלון לקוח במצב פעולות
            CustomerDetails.Visibility = Visibility.Collapsed;
            updateCustomer.Visibility = Visibility.Collapsed;
            
            closeWindow.Click += closeWindow_Click;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            addTheCustomer.MouseDoubleClick += addTheCustomer_MouseDoubleClick;
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addTheCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.AddCustomer(int.Parse(newID.Text.ToString()), newName.Text, newPhoneNum.Text, double.Parse(newLongitude.Text.ToString()), double.Parse(newLatitude.Text.ToString())) ;
                MessageBox.Show("Added successfully");
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

        private void cancelAdding_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void updateCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            enterUpdatedName.Visibility = Visibility.Visible ;
            enterUpdatedPhoneNum.Visibility = Visibility.Visible;
            updatedName.Visibility = Visibility.Visible;
            updatedPhoneNum.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Visible;


        }

        private void submitButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            mainData.UpdateCustomer(customerCurrent.ID, updatedName.Text, updatedPhoneNum.Text);
            customerCurrent = mainData.GetCustomer(customerCurrent.ID);

            submitButton.Visibility = Visibility.Collapsed;
            enterUpdatedName.Visibility = Visibility.Collapsed;
            enterUpdatedPhoneNum.Visibility = Visibility.Collapsed;
            updatedName.Visibility = Visibility.Collapsed;
            updatedPhoneNum.Visibility = Visibility.Collapsed;

            MessageBox.Show("Updated successfully");

            CustomerDetails.Text = customerCurrent.ToString();

        }

    }
}
