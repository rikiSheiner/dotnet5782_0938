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
            this.DataContext = stationCurrent;

            //הסתרת כל הכפתורים הקשורים לחלון תחנה במצב הוספה
            
            enterLatitude.Visibility = Visibility.Collapsed;
            enterLongitude.Visibility = Visibility.Collapsed;
            newLatitude.Visibility = Visibility.Collapsed;
            newLongitude.Visibility = Visibility.Collapsed;

            addTheStation.Visibility = Visibility.Collapsed;
            cancelAdding.Visibility = Visibility.Collapsed;

            ListOfDrones.Visibility = Visibility.Visible;
            chooseDrone.Visibility = Visibility.Visible;

            updateStation.MouseDoubleClick += updateStation_MouseDoubleClick;
            ListOfDrones.SelectionChanged += ListOfDrones_SelectionChanged;
            deleteStation.MouseDoubleClick += deleteStation_MouseDoubleClick;

            ListOfDrones.ItemsSource = stationCurrent.dronesInCharge;
            newChargeSlots.Text = (stationCurrent.availableChargeSlots + stationCurrent.fullChargeSlots).ToString();
            newID.IsReadOnly = true;
            
        }
        public StationWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            
            closeWindow.Click += closeWindow_Click;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            addTheStation.MouseDoubleClick += addTheStation_MouseDoubleClick;
            updateStation.Visibility = Visibility.Collapsed;
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
        private void updateDisplayOfOpenedWindows()
        {
            if (Application.Current.Windows.OfType<StationsListWindow>().Any(w => w.GetType().Name.Equals("StationsListWindow")))
            {
                Application.Current.Windows.OfType<StationsListWindow>().FirstOrDefault().Close();
                StationsListWindow sl = new StationsListWindow(mainData);
                sl.Show();
            }
        }
        private void addTheStation_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.AddStation(int.Parse(newID.Text.ToString()), int.Parse (newName.Text.ToString ()),double.Parse(newLongitude.Text.ToString()), double.Parse(newLatitude.Text.ToString()),int.Parse (newChargeSlots .Text .ToString ()));
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

        private void updateStation_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            mainData.UpdateStation(stationCurrent.ID, int.Parse (newName.Text.ToString ()), int.Parse (newChargeSlots.Text.ToString ()));
            MessageBox.Show("Updated successfully");
            updateDisplayOfOpenedWindows();
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
                updateDisplayOfOpenedWindows();
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
