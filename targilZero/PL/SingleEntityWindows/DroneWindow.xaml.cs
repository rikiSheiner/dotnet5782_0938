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
using System.ComponentModel;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL mainData;
        private DroneToList droneCurrent;
        
        
        /// <summary>
        /// constructor for actions on specific drone
        /// </summary>
        /// <param name="data"></param>
        /// <param name="drone"></param>
        public DroneWindow(IBL data, DroneToList drone)
        {
            InitializeComponent();
            mainData = data;
            droneCurrent = drone;
           
            closeWindow.Click += closeWindow_Click;
            
            

            //הסתרת הכפתורים הקשורים להוספת רחפן
            enterID.Visibility = Visibility.Collapsed;
            enterModel.Visibility = Visibility.Collapsed;
            enterWeight.Visibility = Visibility.Collapsed;
            enterStationNum.Visibility = Visibility.Collapsed;
            newID.Visibility=Visibility.Collapsed;
            newModel.Visibility=Visibility.Collapsed;
            addTheDrone.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;
            newWeightCategory.Visibility = Visibility.Collapsed;
            newStationID.Visibility = Visibility.Collapsed;

            DroneDetails.Text = droneCurrent.ToString();


            if (drone.droneStatus == Enums.DroneStatuses.delivery) //במצב משלוח ניתן לאסוף או לספק חבילה 
            {
                chargeOrEndCahrge.Visibility = Visibility.Hidden;
                chargeOrEndCahrge.IsEnabled = false;
                parcelDelivery.Visibility = Visibility.Visible;
                parcelDelivery.IsEnabled = true;
                ParcelToList parcelInDrone = mainData.GetParcel(droneCurrent.parcelInDroneID);
                if (parcelInDrone.parcelStatus == Enums.ParcelStatuses.assigned) //זה אומר שהחבילה מחכה לאיסוף
                {
                    parcelDelivery.Content = "collect parcel";
                }
                else if(parcelInDrone.parcelStatus == Enums.ParcelStatuses.collected)//זה אומר שהחבילה מחכה לאספקה
                {
                    parcelDelivery.Content = "delivery parcel";
                }
            }
            else
            {
                chargeOrEndCahrge.Visibility = Visibility.Visible;
                chargeOrEndCahrge.IsEnabled = true;
                if (drone.droneStatus == Enums.DroneStatuses.available) //במצב פנוי ניתן לשייך חבילה או להטעין
                {
                    chargeOrEndCahrge.Content = "Charge Drone";
                    parcelDelivery.Content = "assign parcel to drone";

                }
                else if(drone.droneStatus == Enums.DroneStatuses.maintenance) //במצב תחזוקה ניתן רק לסיים טעינה
                {
                    chargeOrEndCahrge.Content = "End Charge Drone";
                    parcelDelivery.Visibility = Visibility.Hidden;
                    parcelDelivery.IsEnabled = false;
                }
            }
            chargeOrEndCahrge.MouseDoubleClick += chargeOrEndCahrge_MouseDoubleClick;
            parcelDelivery.MouseDoubleClick += parcelDelivery_MouseDoubleClick;
            updateDrone.MouseDoubleClick += updateDrone_MouseDoubleClick;
            submitModel.MouseDoubleClick += submitModel_MouseDoubleClick;
            submitHours.MouseDoubleClick += submitHours_MouseDoubleClick;
        }
        /// <summary>
        /// constructor for adding new drone
        /// </summary>
        /// <param name="data"></param>
        public DroneWindow(IBL data)
        {
            InitializeComponent();
            mainData = data;
            closeWindow.Click += closeWindow_Click;

            //הסתרת הכפתורים שלא רלוונטיים להוספת רחפן
            updateDrone.Visibility = Visibility.Hidden;
            updateDrone.IsEnabled = false;
            parcelDelivery.Visibility = Visibility.Hidden;
            parcelDelivery.IsEnabled = false;
            chargeOrEndCahrge.Visibility = Visibility.Hidden;
            chargeOrEndCahrge.IsEnabled = false;
            DroneDetails.Visibility = Visibility.Collapsed;

            newID.TextChanged += newID_TextChanged;
            newModel.TextChanged += newModel_TextChanged;
            addTheDrone.MouseDoubleClick += addTheDrone_MouseDoubleClick;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;

            for (int i = 0; i < 3; i++)
            {
                newWeightCategory.Items.Add((Enums.WeightCategories)i);
            }

            foreach (StationToList station in mainData .GetListStationsWithCondition (x=>x.availableChargeSlots >0))
            {
                newStationID.Items.Add(station.ID);
            }


        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chargeOrEndCahrge_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)chargeOrEndCahrge.Content == "Charge Drone")
                {
                    mainData.CreateDroneCharge(droneCurrent.ID);
                    MessageBox.Show("charged drone successfully");
                    chargeOrEndCahrge.Content = "End Charge Drone";

                    droneCurrent = mainData.GetListDrones().ElementAt(mainData.FindDrone(droneCurrent.ID));
                    DroneDetails.Text = droneCurrent.ToString();
                    parcelDelivery.Visibility = Visibility.Collapsed;
                }
                else if ((string)chargeOrEndCahrge.Content == "End Charge Drone")
                {
                    enterHoursCharging.Visibility = Visibility.Visible;
                    hoursOfCharging.Visibility = Visibility.Visible;
                    submitHours.Visibility = Visibility.Visible;

                }
            }
            catch(UpdateProblemException upe)
            {
                MessageBox.Show(upe.Message);
                enterHoursCharging.Visibility = Visibility.Collapsed;
                hoursOfCharging.Visibility = Visibility.Collapsed;
            }

            catch (Exception)
            {
                MessageBox.Show("ERROR");
                enterHoursCharging.Visibility = Visibility.Collapsed;
                hoursOfCharging.Visibility = Visibility.Collapsed;
            }

        }
        private void submitHours_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            int h = 1;
            if (int.TryParse(hoursOfCharging.Text.ToString(), out h))
                h = int.Parse(hoursOfCharging.Text);

            mainData.EndDroneCharge(droneCurrent.ID, h);
            MessageBox.Show("ended charging drone successfully");

            enterHoursCharging.Visibility = Visibility.Collapsed;
            hoursOfCharging.Visibility = Visibility.Collapsed ;
            submitHours.Visibility = Visibility.Collapsed;
            chargeOrEndCahrge.Content = "Charge Drone";

            droneCurrent = mainData.GetListDrones().ElementAt(mainData.FindDrone(droneCurrent.ID));
            DroneDetails.Text = droneCurrent.ToString();

            parcelDelivery.Visibility = Visibility.Visible;
            parcelDelivery.IsEnabled = true;
        }

        private void parcelDelivery_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)parcelDelivery.Content == "assign parcel to drone")
                {
                    mainData.AssignParcelToDrone(droneCurrent.ID);
                    MessageBox.Show("assigned successfully");
                    parcelDelivery.Content = "collect parcel";
                }
                else if ((string)parcelDelivery.Content == "collect parcel")
                {
                    mainData.CollectParcel(droneCurrent.ID);
                    MessageBox.Show("collected successfully");
                    parcelDelivery.Content = "delivery parcel";
                   
                }
                else if ((string)parcelDelivery.Content == "delivery parcel")
                {
                    mainData.ParcelDelivery(droneCurrent.ID);
                    MessageBox.Show("delivered successfully");
                    parcelDelivery.Content = "assign parcel to drone";
                    
                }
                droneCurrent = mainData.GetListDrones().ElementAt(mainData.FindDrone(droneCurrent.ID));
                DroneDetails.Text = droneCurrent.ToString();
            }
            catch(UpdateProblemException upe)
            {
                MessageBox.Show(upe.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }

        private void updateDrone_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            updatedDroneModel.Visibility = Visibility.Visible;
            updatedDroneModel.IsEnabled = true;
            parcelDelivery.Visibility = Visibility.Collapsed;
            chargeOrEndCahrge.Visibility = Visibility.Collapsed;
            submitModel.Visibility = Visibility.Visible;
            submitModel.IsEnabled = true;
        }

        private void submitModel_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            droneCurrent.model = updatedDroneModel.Text;
            mainData.UpdateDrone(droneCurrent.ID, droneCurrent.model);
            
            submitModel.Visibility = Visibility.Collapsed;
            submitModel.IsEnabled = false;
            updatedDroneModel.Visibility = Visibility.Collapsed;
            updatedDroneModel.IsEnabled = false;

            MessageBox.Show("Updated successfully");

            DroneDetails.Text = droneCurrent.ToString();
            
        }

        private void newModel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void newID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addTheDrone_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            string weight = newWeightCategory.SelectedItem.ToString();
            string stationNum = newStationID.SelectedItem.ToString();

            int w = 0;
            if (weight == "heavy")
                w = 2;
            else if (weight == "intermidiate")
                w = 1;

            try
            {
                mainData.AddDrone(int.Parse(newID.Text), newModel.Text, w, int.Parse(stationNum));
                MessageBox.Show("Added successfully");
                Close();
            }
            catch(AddingProblemException addingProblem)
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
    }
}
