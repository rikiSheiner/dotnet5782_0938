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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private IBL mainData;
        private ParcelToList parcelCurrent;
        private CustomerToList currentCustomer; 

        /// <summary>
        /// constructor for actions on specific parcel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parcel"></param>
        public ParcelWindow(IBL data,ParcelToList parcel)
        {
            InitializeComponent();
            mainData = data;
            parcelCurrent = parcel;
            closeWindow.Click += closeWindow_Click;
            deleteParcel.MouseDoubleClick += deleteParcel_MouseDoubleClick;
            droneProvideParcel.MouseDoubleClick += droneProvideParcel_MouseDoubleClick;
            senderOfParcel.MouseDoubleClick += senderOfParcel_MouseDoubleClick;
            targetOfParcel.MouseDoubleClick += targetOfParcel_MouseDoubleClick;

            //הסתרת כל הכפתורים הקשורים לחלון חבילה במצב הוספה
            newPriority.Visibility = Visibility.Collapsed;
            newSenderID.Visibility = Visibility.Collapsed;
            newTargetID.Visibility = Visibility.Collapsed;
            newWeight.Visibility = Visibility.Collapsed;
            addTheParcel.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;

            enterStatus.Visibility = Visibility.Visible;
            senderName.Visibility = Visibility.Visible;
            targetName.Visibility = Visibility.Visible;
            weight.Visibility = Visibility.Visible;
            priority.Visibility = Visibility.Visible;
            status.Visibility = Visibility.Visible;

            this.DataContext = parcelCurrent;
            if(parcelCurrent != null)
                if(parcelCurrent .parcelStatus !=  Enums.ParcelStatuses.defined && parcelCurrent.parcelStatus != Enums.ParcelStatuses.supplied)
                    droneProvideParcel.Visibility = Visibility.Visible;
            
            senderOfParcel.Visibility = Visibility.Visible;
            targetOfParcel.Visibility = Visibility.Visible;

        }

        /// <summary>
        ///  parameters constructor for adding parcel by user
        /// </summary>
        /// <param name="data"></param>
        public ParcelWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            closeWindow.Click += closeWindow_Click;
            addTheParcel.MouseDoubleClick += addTheParcel_MouseDoubleClick;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            deleteParcel.Visibility = Visibility.Collapsed;

            //insert data to ComboBoxes of parcel adding
            for (int i = 0; i < 3; i++)
            {
                newWeight.Items.Add((Enums.WeightCategories)i);
                newPriority.Items.Add((Enums.Priorities)i);
            }
            foreach (CustomerToList customer in mainData.GetListCustomers())
            {
                newSenderID.Items.Add(customer.ID);
                newTargetID.Items.Add(customer.ID);
            }

        }

        /// <summary>
        /// parameters constructor for adding parcel by customer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="newCustomer"></param>
        public ParcelWindow (IBL data, CustomerToList newCustomer)
        {
            mainData = data;
            currentCustomer = newCustomer;

            InitializeComponent();
            closeWindow.Click += closeWindow_Click;
            addTheParcel.MouseDoubleClick += addTheParcel_MouseDoubleClick;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            deleteParcel.Visibility = Visibility.Collapsed;
            enterSenderID.Visibility = Visibility.Collapsed;
            newSenderID.Visibility = Visibility.Collapsed;

            //insert data to ComboBoxes of parcel adding
            for (int i = 0; i < 3; i++)
            {
                newWeight.Items.Add((Enums.WeightCategories)i);
                newPriority.Items.Add((Enums.Priorities)i);
            }
            foreach (CustomerToList customer in mainData.GetListCustomers())
            {
                newTargetID.Items.Add(customer.ID);
            }
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void updateDisplayOfOpenedWindows()
        {
            if (Application.Current.Windows.OfType<ParcelsListWindow>().Any(w => w.GetType().Name.Equals("ParcelsListWindow")))
            {
                Application.Current.Windows.OfType<ParcelsListWindow>().FirstOrDefault().Close();
                ParcelsListWindow pl = new ParcelsListWindow(mainData);
                pl.Show();
            }
        }

        private void addTheParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (newSenderID .Visibility == Visibility.Collapsed )
                {
                    mainData.AddParcel(currentCustomer.ID, int.Parse(newTargetID.SelectedItem.ToString()), newWeight.SelectedIndex, newPriority.SelectedIndex);
                }
                else
                {
                    mainData.AddParcel(int.Parse(newSenderID.SelectedItem.ToString()), int.Parse (newTargetID.SelectedItem.ToString ()), newWeight.SelectedIndex, newPriority .SelectedIndex );
                }
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

        private void cancelAdding_MouseDoubleClick(object sender, RoutedEventArgs e) { Close(); }

        private void deleteParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.DeleteParcel(parcelCurrent.ID);
                MessageBox.Show("The parcel has been deleted successfuly");
                new ParcelsListWindow(mainData).Show();
                Close();
                updateDisplayOfOpenedWindows();
            }
            catch (DeletedProblemException dpe)
            {
                MessageBox.Show(dpe.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't delete parcel");
            }
        }

        private void senderOfParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var customerOrigion= mainData.GetListCustomers().Where(customer => customer.name == parcelCurrent.nameOfSender);
            new CustomerWindow(mainData, customerOrigion .First()).Show ();
            Close();
        }

        private void droneProvideParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new DroneWindow1(mainData, mainData .ConvertDroneToDroneInList (mainData .ConvertDroneDalToDroneBL (parcelCurrent.droneSender))).Show ();
            Close();
        }

        private void targetOfParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var customerDest = mainData.GetListCustomers().Where(customer => customer.name == parcelCurrent.nameOfTarget);
            new CustomerWindow(mainData, customerDest.First()).Show();
            Close();
        }
    }

}
