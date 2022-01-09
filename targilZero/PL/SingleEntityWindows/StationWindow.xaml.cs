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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private IBL mainData;
        private StationToList stationCurrent;
        /*int id, int name, double longitude, double latitude, int cs*/

        /// <summary>
        /// constructor for actions on specific station
        /// </summary>
        /// <param name="data"></param>
        /// <param name="station"></param>
        public StationWindow(IBL data, StationToList station)
        {
            InitializeComponent();
            mainData = data;
            stationCurrent = station;
            closeWindow.Click += closeWindow_Click;
            

            //הסתרת כל הכפתורים הקשורים לחלון תחנה במצב הוספה
            newChargeSlots.Visibility = Visibility.Collapsed;
            newID.Visibility = Visibility.Collapsed;
            newLatitude.Visibility = Visibility.Collapsed;
            newLongitude.Visibility = Visibility.Collapsed;
            newName.Visibility = Visibility.Collapsed;
            enterChargeSlots.Visibility = Visibility.Collapsed;
            enterID.Visibility = Visibility.Collapsed;
            enterLatitude.Visibility = Visibility.Collapsed;
            enterLongitude.Visibility = Visibility.Collapsed;
            enterName.Visibility = Visibility.Collapsed;
            addTheStation.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;

            ListOfDrones.Visibility = Visibility.Visible;
            chooseDrone.Visibility = Visibility.Visible;

            StationDetails.Text = stationCurrent.ToString();

            updateStation.MouseDoubleClick += updateStation_MouseDoubleClick;
            submitButton.MouseDoubleClick += submitButton_MouseDoubleClick;
            ListOfDrones.SelectionChanged += ListOfDrones_SelectionChanged;
            deleteStation.MouseDoubleClick += deleteStation_MouseDoubleClick;

           

            foreach (DroneToList drone in stationCurrent.dronesInCharge)
            {
                ListOfDrones.Items.Add(drone);
            }


        }
        public StationWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            
            closeWindow.Click += closeWindow_Click;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            addTheStation.MouseDoubleClick += addTheStation_MouseDoubleClick;
            updateStation.Visibility = Visibility.Collapsed;
            StationDetails.Visibility = Visibility.Collapsed;
            deleteStation.Visibility = Visibility.Collapsed;

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

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addTheStation_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            try
            {
                mainData.AddStation(int.Parse(newID.Text.ToString()), int.Parse (newName.Text.ToString ()),double.Parse(newLongitude.Text.ToString()), double.Parse(newLatitude.Text.ToString()),int.Parse (newChargeSlots .Text .ToString ()));
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

        private void updateStation_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            enterUpdatedName.Visibility = Visibility.Visible;
            enterUpdatedChargeSlots.Visibility = Visibility.Visible;
            updatedName.Visibility = Visibility.Visible;
            updatedChargeSlots.Visibility = Visibility.Visible;
            submitButton.Visibility = Visibility.Visible;


        }

        private void submitButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            int n = 0,cs=0;
            if (int.TryParse(updatedName.Text, out n))
                n = int.Parse(updatedName.Text);
            if (int.TryParse(updatedChargeSlots.Text, out cs))
                cs = int.Parse(updatedChargeSlots.Text);

            mainData.UpdateStation(stationCurrent.ID,n, cs);
            stationCurrent = mainData.GetStation(stationCurrent.ID);

            submitButton.Visibility = Visibility.Collapsed;
            enterUpdatedName.Visibility = Visibility.Collapsed;
            enterUpdatedChargeSlots.Visibility = Visibility.Collapsed;
            updatedName.Visibility = Visibility.Collapsed;
            updatedChargeSlots.Visibility = Visibility.Collapsed;

            MessageBox.Show("Updated successfully");

            StationDetails.Text = stationCurrent.ToString();

        }

        private void ListOfDrones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DroneWindow1 droneActions = new DroneWindow1(mainData, (DroneToList )ListOfDrones .SelectedItem  );
                droneActions.Show();
                this.Close();
            }
            catch(Exception )
            {
                MessageBox.Show("ERROR");
            }
        }

        private void deleteStation_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.DeleteStation(stationCurrent.ID);
                MessageBox.Show("The station has been deleted successfuly");
                new StationsListWindow(mainData).Show();
                Close();
            }
            catch (DeletedProblemException dpe)
            {
                MessageBox.Show(dpe.Message);
            }
            catch (Exception )
            {
                MessageBox.Show("Can't delete station");
            }
            
        }
    }
}
