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
            this.DataContext = customerCurrent;

            //הסתרת כל הכפתורים הקשורים להוספת לקוח
            newLatitude.Visibility = Visibility.Collapsed;
            newLongitude.Visibility = Visibility.Collapsed;
            addTheCustomer.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;
            enterLatitude.Visibility = Visibility.Collapsed;
            enterLongitude.Visibility = Visibility.Collapsed;

            chooseParcel.Visibility = Visibility.Visible;
            ListOfParcels.Visibility = Visibility.Visible;

            updateCustomer.MouseDoubleClick += updateCustomer_MouseDoubleClick;
            deleteCustomer.MouseDoubleClick += deleteCustomer_MouseDoubleClick;

            //הוספת  החבילות שהלקוח שולח או מקבל אל רשימת החבילות
            var helpCustomerBL = mainData.ConvertCustomerDalToCustomerBL(mainData.FindAndGetCustomer(customerCurrent.ID));

            foreach (ParcelToList parcel in helpCustomerBL.parcelsFromCustomer )
            {
                ListOfParcels.Items.Add(parcel);
            }
            foreach (ParcelToList parcel in helpCustomerBL.parcelsToCustomer)
            {
                ListOfParcels.Items.Add(parcel);
            }
            ListOfParcels.SelectionChanged += ListOfParcels_SelectionChanged;

            newID.IsReadOnly =true;
        }

        public CustomerWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();

            //הסתרת כל הכפתורים שקשורים חלון לקוח במצב פעולות
            updateCustomer.Visibility = Visibility.Collapsed;
            deleteCustomer.Visibility = Visibility.Collapsed;
            
            closeWindow.Click += closeWindow_Click;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            addTheCustomer.MouseDoubleClick += addTheCustomer_MouseDoubleClick;
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void updateDisplayOfOpenedWindows()
        {
            if (Application.Current.Windows.OfType<CustomersListWindow>().Any(w => w.GetType().Name.Equals("CustomersListWindow")))
            {
                Application.Current.Windows.OfType<CustomersListWindow>().FirstOrDefault().Close();
                CustomersListWindow cl = new CustomersListWindow(mainData);
                cl.Show();
            }
        }
        private void addTheCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.AddCustomer(int.Parse(newID.Text.ToString()), newName.Text, newPhoneNum.Text, double.Parse(newLongitude.Text.ToString()), double.Parse(newLatitude.Text.ToString())) ;
                MessageBox.Show("Added successfully");
                Close();
                updateDisplayOfOpenedWindows();
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
            mainData.UpdateCustomer(customerCurrent.ID, newName.Text.ToString(),newPhoneNum.Text.ToString());
            MessageBox.Show("Updated successfully");
            updateDisplayOfOpenedWindows();
        }


        private void deleteCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.DeleteCustomer(customerCurrent.ID);
                MessageBox.Show("The customer has been deleted successfuly");
                new CustomersListWindow(mainData).Show();
                Close();
                updateDisplayOfOpenedWindows();
            }
            catch(DeletedProblemException dpe)
            {
                MessageBox.Show(dpe.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't delete customer");
            }
        }
        private void ListOfParcels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ParcelWindow parcelActions = new ParcelWindow(mainData, (ParcelToList)ListOfParcels.SelectedItem);
                parcelActions.Show();
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }

        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;

        }
        
    }
}
