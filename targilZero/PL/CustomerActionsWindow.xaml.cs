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
using PL.SingleEntityWindows;

namespace PL
{

    /// <summary>
    /// Interaction logic for CustomerActionsWindow.xaml
    /// </summary>
    public partial class CustomerActionsWindow : Window
    {
        private IBL mainData;
        private CustomerToList currentCustomer;
        public CustomerActionsWindow(IBL data,CustomerToList customer)
        {
            InitializeComponent();
            mainData = data;
            currentCustomer =customer;

            ListParcelsOfCustomer.Visibility = Visibility.Collapsed;
            listOfParcelsToSend.Visibility = Visibility.Collapsed;
            chooseParcel.Visibility = Visibility.Collapsed;
            ParcelsSentOrRecieved.Visibility = Visibility.Collapsed;
            chooseParcelToConfirm.Visibility = Visibility.Collapsed ;


            DateTime now = DateTime.Now;
            if (now.Hour >= 4 && now.Hour <= 12)
                HelloCustomer.Text = "Good morning " + currentCustomer.name + "!";
            else if (now.Hour > 12 && now.Hour <= 17)
                HelloCustomer.Text = "Good afternoon " + currentCustomer.name + "!";
            else if (now.Hour > 17 && now.Hour <= 21)
                HelloCustomer.Text = "Good evening " + currentCustomer.name + "!";
            else
                HelloCustomer.Text = "Good night " + currentCustomer.name + "!";

            parcelsOfCustomer.MouseDoubleClick += parcelsOfCustomer_MouseDoubleClick;
            sendParcelButton.MouseDoubleClick += sendParcelButton_MouseDoubleClick;
            signOutCustomer.MouseDoubleClick += signOutCustomer_MouseDoubleClick;
            confirmParcelRecievingButton.MouseDoubleClick += confirmParcelRecievingButton_MouseDoubleClick;
            confirmParcelSendingButton.MouseDoubleClick += confirmParcelSendingButton_MouseDoubleClick;
            AddParcelButton.MouseDoubleClick += AddParcelButton_MouseDoubleClick;
        }

        private void parcelsOfCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListParcelsOfCustomer.Visibility = Visibility.Visible;
            listOfParcelsToSend .Visibility = Visibility.Collapsed;
            chooseParcel.Visibility = Visibility.Collapsed;
            ParcelsSentOrRecieved.Visibility = Visibility.Collapsed;
            chooseParcelToConfirm.Visibility = Visibility.Collapsed;
            confirmSendingOrRecievingCheckBox.Visibility = Visibility.Collapsed;

            ListParcelsOfCustomer.ItemsSource = mainData.GetListParcelsWithCondition(parcel=>
            parcel.nameOfSender == currentCustomer .name || parcel .nameOfTarget == currentCustomer .name );

        }

        private void sendParcelButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListParcelsOfCustomer.Visibility = Visibility.Collapsed;
            listOfParcelsToSend.Visibility = Visibility.Visible;
            chooseParcel.Visibility = Visibility.Visible;
            ParcelsSentOrRecieved.Visibility = Visibility.Collapsed;
            chooseParcelToConfirm.Visibility = Visibility.Collapsed ;
            confirmSendingOrRecievingCheckBox.Visibility = Visibility.Collapsed;

            listOfParcelsToSend.ItemsSource = mainData.GetListParcelsWithCondition(parcel =>
            parcel.parcelStatus == Enums.ParcelStatuses.assigned && parcel.nameOfSender == currentCustomer .name);
        }

        private void listOfParcelsToSend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ParcelToList selectedParcel = (ParcelToList)listOfParcelsToSend.SelectedItem;

                mainData.CollectParcel(selectedParcel.droneSender.ID);
                mainData.ParcelDelivery(selectedParcel.droneSender.ID);
                listOfParcelsToSend.Visibility = Visibility.Collapsed;

                MessageBox.Show("The parcel has been sent successfully");
            }
            catch (BL.BO.UpdateProblemException upe)
            {
                MessageBox.Show(upe.Message);
            }
            catch (Exception )
            {
                MessageBox.Show("ERROR");
            }

        }

        private void signOutCustomer_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void confirmParcelSendingButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListParcelsOfCustomer.Visibility = Visibility.Collapsed;
            listOfParcelsToSend.Visibility = Visibility.Collapsed;
            chooseParcel.Visibility = Visibility.Collapsed;
            confirmSendingOrRecievingCheckBox.Visibility = Visibility.Collapsed;
            ParcelsSentOrRecieved.Visibility = Visibility.Visible;
            chooseParcelToConfirm.Visibility = Visibility.Visible;

            ParcelsSentOrRecieved .ItemsSource = mainData.GetListParcelsWithCondition(parcel => parcel.nameOfSender == currentCustomer.name
            && (parcel.parcelStatus == Enums.ParcelStatuses.collected || parcel.parcelStatus == Enums.ParcelStatuses.supplied));
           
        }

        private void confirmParcelRecievingButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListParcelsOfCustomer.Visibility = Visibility.Collapsed;
            listOfParcelsToSend.Visibility = Visibility.Collapsed;
            chooseParcel.Visibility = Visibility.Collapsed;
            confirmSendingOrRecievingCheckBox.Visibility = Visibility.Collapsed;
            ParcelsSentOrRecieved.Visibility = Visibility.Visible;
            chooseParcelToConfirm.Visibility = Visibility.Visible;

            ParcelsSentOrRecieved.ItemsSource = mainData.GetListParcelsWithCondition(parcel => 
            parcel.nameOfTarget == currentCustomer.name && parcel.parcelStatus == Enums.ParcelStatuses.supplied);
        }

        private void ParcelsSentOrRecieved_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedParcel = (ParcelToList)ParcelsSentOrRecieved.SelectedItem;
                if(selectedParcel != null )
                {
                    if (selectedParcel.parcelStatus == Enums.ParcelStatuses.supplied && selectedParcel.nameOfTarget == currentCustomer .name)
                    {
                        confirmSendingOrRecievingCheckBox.Visibility = Visibility.Visible;
                        confirmSendingOrRecievingCheckBox.Content = "Did the parcel\n has been collected? ";
                        if (confirmSendingOrRecievingCheckBox.IsChecked == true)
                            mainData.UpdateRecievingOfParcel(selectedParcel.ID);
                    }
                    else
                    {
                        confirmSendingOrRecievingCheckBox.Visibility = Visibility.Visible;
                        confirmSendingOrRecievingCheckBox.Content = "Did the parcel\n has been supllied? ";
                        if (confirmSendingOrRecievingCheckBox.IsChecked == true)
                            mainData.UpdateSendingOfParcel(selectedParcel.ID);
                    }
                }
                
            }
            catch (UpdateProblemException upe)
            {
                MessageBox.Show(upe.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }


        }

        private void AddParcelButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            confirmSendingOrRecievingCheckBox.Visibility = Visibility.Collapsed;
            new ParcelWindow(mainData, currentCustomer).Show ();

        }
    }
}
