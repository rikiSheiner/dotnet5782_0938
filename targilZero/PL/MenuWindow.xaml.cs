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
using BL.IBL;
using BL.IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        IBL mainData;
        public MenuWindow(IBL data)
        {
            InitializeComponent();
            mainData = data;
            this.GoToListDronesWindow.MouseDoubleClick += GoToListDronesWindow_MouseDoubleClick;
            this.GoToListParcelsWindow.MouseDoubleClick += GoToListParcelsWindow_MouseDoubleClick;
            this.GoToListStationsWindow.MouseDoubleClick += GoToListStationsWindow_MouseDoubleClick;
            this.GoToListCustomersWindow.MouseDoubleClick  += GoToListCustomersWindow_MouseDoubleClick;
            this.closeWindow.Click += closeWindow_Click;
        }

        private void GoToListDronesWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListDronesWindow listDronesWindow = new ListDronesWindow(mainData);
            listDronesWindow.Show();
            this.Close();
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GoToListCustomersWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            CustomersListWindow customersListWindow = new CustomersListWindow(mainData);
            customersListWindow.Show();
            Close();
        }

        private void GoToListParcelsWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new ParcelsListWindow(mainData);
            parcelsListWindow.Show();
            Close();
        }

        private void GoToListStationsWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            StationsListWindow stationsListWindow = new StationsListWindow(mainData);
            stationsListWindow.Show();
            Close();
        }
    }
}
