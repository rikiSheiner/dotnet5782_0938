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
using PL.SingleEntityWindows;
using BL.BlApi;
using BL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        private IBL mainData;
        public CustomersListWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            this.DataContext = mainData.GetListCustomers();

            closeWindow.Click += closeWindow_Click;
            AddCustomerButton.MouseDoubleClick += AddCustomerButton_MouseDoubleClick;
            ListOfCustomers.SelectionChanged += ListOfCustomers_SelectionChanged;
            FilterCustomersList.SelectionChanged += FilterCustomersList_SelectionChanged;
            refreshWindow.MouseDoubleClick += refreshWindow_MouseDoubleClick;
            
        }
        private void closeWindow_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void AddCustomerButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow(mainData);
            customerWindow.Show();
        }

        private void FilterCustomersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FilterCustomersList.SelectedIndex)
            {
                case 0:
                    this.DataContext = mainData.GetListCustomers();
                    break;
                case 1:
                    this.DataContext = mainData.GetListCustomersWithCondition(x => x.numParcelsRecieved > 0); 
                    break;
                case 2:
                    this.DataContext = mainData.GetListCustomersWithCondition(x => x.numParcelsRecieved < 1);
                    break;
                case 3:
                    ListOfCustomers .ItemsSource = mainData.GetListCustomersWithCondition(x => x.numParcelsSentAndDelivered + x.numParcelsSentNotDelivered > 0);
                    break;
                case 4:
                    this.DataContext = mainData.GetListCustomersWithCondition(x => x.numParcelsSentAndDelivered + x.numParcelsSentNotDelivered < 1);
                    break;
                default:
                    break;

            }

        }
        private void ListOfCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerWindow customerActions = new CustomerWindow(mainData, (CustomerToList)ListOfCustomers.SelectedItem);
            customerActions.Show();
        }

        private void refreshWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            CustomersListWindow newWindow = new CustomersListWindow(mainData);
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            Close();
        }
    }
}
