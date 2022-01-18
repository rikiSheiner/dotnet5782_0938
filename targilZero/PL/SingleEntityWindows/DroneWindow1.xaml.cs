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


namespace PL.SingleEntityWindows
{
    /// <summary>
    /// Interaction logic for DroneWindow1.xaml
    /// </summary>
    public partial class DroneWindow1 : Window
    {
        private IBL mainData;
        private DroneToList droneCurrent;
        private BackgroundWorker workerDrone; 

        /// <summary>
        /// constructor for actions on specific drone
        /// </summary>
        /// <param name="data"></param>
        /// <param name="drone"></param>
        public DroneWindow1(IBL data, DroneToList drone)
        {
            InitializeComponent();
            mainData = data;
            droneCurrent = drone;
            workerDrone = new BackgroundWorker();
            workerDrone.WorkerReportsProgress = true;
            workerDrone.WorkerSupportsCancellation = true;
            workerDrone.DoWork += Worker_DoWork;
            workerDrone.ProgressChanged += Worker_ProgressChanged;
            workerDrone.RunWorkerCompleted += Worker_RunWorkerCompleted;
            this.DataContext = droneCurrent;

            #region 
            closeWindow.Click += closeWindow_Click;
            chargeOrEndCahrge.MouseDoubleClick += chargeOrEndCahrge_MouseDoubleClick;
            parcelDelivery.MouseDoubleClick += parcelDelivery_MouseDoubleClick;
            updateDrone.MouseDoubleClick += updateDrone_MouseDoubleClick;
            submitHours.MouseDoubleClick += submitHours_MouseDoubleClick;
            parcelInDrone.MouseDoubleClick += parcelInDrone_MouseDoubleClick;
            deleteDrone.MouseDoubleClick += deleteDrone_MouseDoubleClick;
            AutomaticButton.MouseDoubleClick += AutomaticButton_MouseDoubleClick;
            ManualButton.MouseDoubleClick += ManualButton_MouseDoubleClick;
            #endregion

            #region 
            enterStationNum.Visibility = Visibility.Collapsed;
            addTheDrone.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;
            newWeightCategory.Visibility = Visibility.Collapsed;
            newStationID.Visibility = Visibility.Collapsed;
            
            newWeightTB.Visibility = Visibility.Visible;
            newID.IsReadOnly = true;
            newStatus.Visibility = Visibility.Visible;
            newWeightTB.Visibility = Visibility.Visible;
            enterBattrey.Visibility = Visibility.Visible;
            enterStatus.Visibility = Visibility.Visible;
            enterLocation.Visibility = Visibility.Visible;
            enterPID.Visibility = Visibility.Visible;
            pbBattery.Visibility = Visibility.Visible;
            tbBattery.Visibility = Visibility.Visible;
            newLocation.Visibility = Visibility.Visible;
            newStatus.Visibility = Visibility.Visible;
            newPID.Visibility = Visibility.Visible;

            #endregion 

            if(droneCurrent != null)
                newLocation.Text = droneCurrent.location.ToString ();

            if (drone.droneStatus == Enums.DroneStatuses.delivery) //במצב משלוח ניתן לאסוף או לספק חבילה 
            {
                chargeOrEndCahrge.Visibility = Visibility.Collapsed;
                parcelDelivery.Visibility = Visibility.Visible;
                parcelDelivery.IsEnabled = true;

                ParcelToList parcelInDrone = mainData.GetParcel(droneCurrent.parcelInDroneID);
                if (parcelInDrone.parcelStatus == Enums.ParcelStatuses.assigned) //זה אומר שהחבילה מחכה לאיסוף
                {
                    parcelDelivery.Content = "collect parcel";
                }
                else if (parcelInDrone.parcelStatus == Enums.ParcelStatuses.collected)//זה אומר שהחבילה מחכה לאספקה
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
                else if (drone.droneStatus == Enums.DroneStatuses.maintenance) //במצב תחזוקה ניתן רק לסיים טעינה
                {
                    chargeOrEndCahrge.Content = "End Charge Drone";
                    parcelDelivery.Visibility = Visibility.Hidden;
                    parcelDelivery.IsEnabled = false;
                }
            }
            if (droneCurrent.parcelInDroneID < 0)
                parcelInDrone.IsEnabled = false;


            SetPBbattery_Foreground();



        }
        /// <summary>
        /// constructor for adding new drone
        /// </summary>
        /// <param name="data"></param>
        public DroneWindow1(IBL data)
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
            parcelInDrone.Visibility = Visibility.Collapsed;
            deleteDrone.Visibility = Visibility.Collapsed;
            AutomaticButton.Visibility = Visibility.Collapsed;

            addTheDrone.MouseDoubleClick += addTheDrone_MouseDoubleClick;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;

            for (int i = 0; i < 3; i++)
            {
                newWeightCategory.Items.Add((Enums.WeightCategories)i);
            }

            foreach (StationToList station in mainData.GetListStationsWithCondition(x => x.availableChargeSlots > 0))
            {
                newStationID.Items.Add(station.ID);
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
        private void SetPBbattery_Foreground()
        {
            if (droneCurrent.battery < 20)
                pbBattery.Foreground = Brushes.Red;
            else if (droneCurrent.battery < 50)
                pbBattery.Foreground = Brushes.Orange;
            else
                pbBattery.Foreground = Brushes.Green;
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
                    parcelDelivery.Visibility = Visibility.Collapsed;

                    updateDisplayOfDroneDetails();
                    updateDisplayOfOpenedWindows();
                }
                else if ((string)chargeOrEndCahrge.Content == "End Charge Drone")
                {
                    enterHoursCharging.Visibility = Visibility.Visible;
                    hoursOfCharging.Visibility = Visibility.Visible;
                    submitHours.Visibility = Visibility.Visible;
                    SetPBbattery_Foreground();
                }

            }
            catch (UpdateProblemException upe)
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
            hoursOfCharging.Visibility = Visibility.Collapsed;
            submitHours.Visibility = Visibility.Collapsed;
            chargeOrEndCahrge.Content = "Charge Drone";

            updateDisplayOfDroneDetails();
            updateDisplayOfOpenedWindows();
            parcelDelivery.Visibility = Visibility.Visible;
            parcelDelivery.IsEnabled = true;
            chargeOrEndCahrge.Visibility = Visibility.Visible;
            parcelDelivery.Content = "assign parcel to drone";
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
                    parcelInDrone.IsEnabled = true;
                }
                else if ((string)parcelDelivery.Content == "collect parcel")
                {
                    mainData.CollectParcel(droneCurrent.ID);
                    MessageBox.Show("collected successfully");
                    parcelDelivery.Content = "delivery parcel";
                    parcelInDrone.IsEnabled = true;
                    SetPBbattery_Foreground();
                }
                else if ((string)parcelDelivery.Content == "delivery parcel")
                {
                    mainData.ParcelDelivery(droneCurrent.ID);
                    MessageBox.Show("delivered successfully");
                    parcelDelivery.Content = "assign parcel to drone";
                    parcelInDrone.IsEnabled = false;
                    chargeOrEndCahrge.Visibility = Visibility.Visible;
                    chargeOrEndCahrge.Content = "Charge Drone";
                    SetPBbattery_Foreground();
                }
                updateDisplayOfDroneDetails();
                updateDisplayOfOpenedWindows();
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
        private void updateDrone_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            mainData.UpdateDrone(droneCurrent.ID, newModel.Text);
            MessageBox.Show("Updated successfully");
            chargeOrEndCahrge.Visibility = Visibility.Visible;
            parcelDelivery.Visibility = Visibility.Visible;
            updateDisplayOfOpenedWindows();
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
        private void parcelInDrone_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelToList parcelToList = mainData.ConvertParcelToParcelInList(mainData.FindAndGetParcel(droneCurrent.parcelInDroneID));
                new ParcelWindow(mainData, parcelToList).Show();
                Close();
            }
            catch (Exception )
            {
                MessageBox.Show("There is not parcel assigned to this drone.");
            }
            
        }
        private void deleteDrone_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.DeleteDrone(droneCurrent.ID);
                MessageBox.Show("The drone has been deleted successfuly");
                new ListDronesWindow(mainData).Show();
                Close();
                updateDisplayOfOpenedWindows();
            }
            catch(DeletedProblemException dpe)
            {
                MessageBox.Show(dpe.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't delete drone");
            }
        }
        private void AutomaticButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            #region הסתרת כל הכפתורים הלא רלוונטים למצב אוטומטי

            enterStationNum.Visibility = Visibility.Collapsed;
            addTheDrone.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;
            newWeightCategory.Visibility = Visibility.Collapsed;
            newStationID.Visibility = Visibility.Collapsed;
            updateDrone.Visibility = Visibility.Collapsed;
            parcelDelivery.Visibility = Visibility.Collapsed;
            chargeOrEndCahrge.Visibility = Visibility.Collapsed;

            parcelInDrone.Visibility = Visibility.Collapsed;
            deleteDrone.Visibility = Visibility.Collapsed;
            AutomaticButton.Visibility = Visibility.Collapsed;
            ManualButton.Visibility = Visibility.Visible;
            #endregion

           
           if (workerDrone.IsBusy != true)
                workerDrone.RunWorkerAsync();

        }
        private void ManualButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            updateDrone.Visibility = Visibility.Visible;
            parcelInDrone.Visibility = Visibility.Visible;
            deleteDrone.Visibility = Visibility.Visible;
            AutomaticButton.Visibility = Visibility.Visible;
            ManualButton.Visibility = Visibility.Collapsed;

            if (workerDrone.WorkerSupportsCancellation == true)
                workerDrone.CancelAsync();

            droneCurrent = mainData.GetDrone(droneCurrent.ID);

            if (droneCurrent.droneStatus == Enums.DroneStatuses.maintenance)
            {
                parcelDelivery.Visibility = Visibility.Collapsed;
                chargeOrEndCahrge.Visibility = Visibility.Visible ;
                chargeOrEndCahrge.Content = "End Charge Drone";
                parcelDelivery.Visibility = Visibility.Collapsed;
            }
            else if (droneCurrent.droneStatus == Enums.DroneStatuses.available)
            {
                parcelDelivery.Visibility = Visibility.Visible;
                chargeOrEndCahrge.Visibility = Visibility.Visible;
                chargeOrEndCahrge.Content = "Charge Drone";
                parcelDelivery.Content = "assign parcel to drone";
            }
            else
            {
                parcelDelivery.Visibility = Visibility.Visible;
                chargeOrEndCahrge.Visibility = Visibility.Collapsed;
                ParcelToList parcel = mainData.GetParcel(droneCurrent.parcelInDroneID);
                if (parcel.parcelStatus == Enums.ParcelStatuses.assigned)
                    parcelDelivery.Content = "collect parcel";
                else if(parcel.parcelStatus == Enums.ParcelStatuses.collected)
                    parcelDelivery.Content = "delivery parcel";
            }


        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (workerDrone.CancellationPending)
                e.Cancel = true;
            else
                try { mainData.AutomaticDroneAct(droneCurrent.ID, UpdateDisplay, CheckStop); }
                catch (UpdateProblemException upe) { MessageBox.Show(upe.Message); }
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        private void UpdateDisplay()
        {
            Dispatcher.Invoke<bool>(updateDisplayOfOpenedWindows);
            Dispatcher.Invoke(updateDisplayOfDroneDetails);
        }
        private void updateDisplayOfDroneDetails()
        {
            droneCurrent = mainData.GetListDrones().ElementAt(mainData.FindDrone(droneCurrent.ID));
            this.DataContext = droneCurrent;
            newLocation.Text = droneCurrent.location.ToString ();
            SetPBbattery_Foreground();
        }
        private bool updateDisplayOfOpenedWindows()
        {
            if (Application.Current.Windows.OfType<ParcelsListWindow>().Any(w => w.GetType().Name.Equals("ParcelsListWindow")))
            {
                Application.Current.Windows.OfType<ParcelsListWindow>().FirstOrDefault().Close();
                ParcelsListWindow pl=new ParcelsListWindow(mainData);
                pl.Show();
            }
            if (Application.Current.Windows.OfType<ListDronesWindow>().Any(w => w.GetType().Name.Equals("ListDronesWindow")))
            {
                Application.Current.Windows.OfType<ListDronesWindow>().FirstOrDefault().Close ();
                ListDronesWindow ld = new ListDronesWindow(mainData);
                ld.Show();
            }

            return true;

        }
        private bool CheckStop()
        {
            return workerDrone .CancellationPending ;
        }

    }
}

