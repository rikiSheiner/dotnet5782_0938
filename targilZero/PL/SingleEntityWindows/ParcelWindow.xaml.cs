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
        /*int senderID, int targetID, int weight, int priority*/

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

            enterPriority.Visibility = Visibility.Collapsed;
            enterSenderID.Visibility = Visibility.Collapsed;
            enterTargetID.Visibility = Visibility.Collapsed;
            enterWeight.Visibility = Visibility.Collapsed;

            addTheParcel.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;

            if(parcelCurrent .parcelStatus !=  Enums.ParcelStatuses.defined )
            {
                droneProvideParcel.Visibility = Visibility.Visible;
            }
            senderOfParcel.Visibility = Visibility.Visible;
            targetOfParcel.Visibility = Visibility.Visible;

            ParcelDetails.Text = parcelCurrent.ToString();
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
            ParcelDetails.Visibility = Visibility.Collapsed;
            deleteParcel.Visibility = Visibility.Collapsed;

            //insert data to ComboBoxes of parcel adding
            for (int i = 0; i < 3; i++)
            {
                newWeight.Items.Add((Enums.WeightCategories)i);
            }
            for (int i = 0; i < 3; i++)
            {
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
            ParcelDetails.Visibility = Visibility.Collapsed;
            deleteParcel.Visibility = Visibility.Collapsed;
            enterSenderID.Visibility = Visibility.Collapsed;
            newSenderID.Visibility = Visibility.Collapsed;

            //insert data to ComboBoxes of parcel adding
            for (int i = 0; i < 3; i++)
            {
                newWeight.Items.Add((Enums.WeightCategories)i);
            }
            for (int i = 0; i < 3; i++)
            {
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

        private void addTheParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            
            try
            {
                int sID = -1,tID=-1,p=-1,w=-1;
                if (int.TryParse(newTargetID.SelectedItem.ToString(), out tID))
                    tID = int.Parse(newTargetID.SelectedItem.ToString());
                if (int.TryParse(newWeight.SelectedItem.ToString(), out w))
                    w = int.Parse(newWeight.SelectedItem.ToString());
                if (int.TryParse(newPriority.SelectedItem.ToString(), out p))
                    p = int.Parse(newPriority.SelectedItem.ToString());
                if (newSenderID .Visibility == Visibility.Collapsed )
                {
                    mainData.AddParcel(currentCustomer .ID, tID, w, p);
                }
                else
                {
                    if (int.TryParse(newSenderID.SelectedItem.ToString(), out sID))
                        sID = int.Parse(newSenderID.SelectedItem.ToString());
                    mainData.AddParcel(sID, tID, w, p);
                }
                
                


                
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

        private void deleteParcel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.DeleteParcel(parcelCurrent.ID);
                MessageBox.Show("The parcel has been deleted successfuly");
                new ParcelsListWindow(mainData).Show();
                Close();
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
