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

            StationDetails.Text = stationCurrent.ToString();

            updateStation.MouseDoubleClick += updateStation_MouseDoubleClick;
            submitButton.MouseDoubleClick += submitButton_MouseDoubleClick;
        }
        public StationWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            StationDetails.Visibility = Visibility.Collapsed;
            closeWindow.Click += closeWindow_Click;
            cancelAdding.MouseDoubleClick += cancelAdding_MouseDoubleClick;
            addTheStation.MouseDoubleClick += addTheStation_MouseDoubleClick;
            updateStation.Visibility = Visibility.Collapsed;
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
    }
}
