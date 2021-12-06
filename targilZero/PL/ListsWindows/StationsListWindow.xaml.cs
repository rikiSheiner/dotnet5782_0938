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
using PL.SingleEntityWindows;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsListWindow.xaml
    /// </summary>
    public partial class StationsListWindow : Window
    {
        private IBL mainData;
        public StationsListWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            closeWindow.Click += closeWindow_Click;
            this.DataContext = mainData.GetListStations ();
            AddStationButton.MouseDoubleClick += AddStationButton_MouseDoubleClick;
            ListOfStations.SelectionChanged += ListOfStations_SelectionChanged;
            FilterStationsList.SelectionChanged += FilterStationsList_SelectionChanged;
        }
        private void closeWindow_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void AddStationButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            StationWindow stationWindow = new StationWindow(mainData);
            stationWindow.Show();
        }

        private void FilterStationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FilterStationsList.SelectedIndex)
            {
                case 0:
                    this.DataContext = mainData.GetListStations();
                    break;
                case 1:
                    this.DataContext = mainData.GetListStationsWithCondition(x => x.availableChargeSlots>0);
                    break;
                case 2:
                    this.DataContext = mainData.GetListStationsWithCondition(x => x.availableChargeSlots <1);
                    break;
                default:
                    break;

            }

        }

        private void ListOfStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationWindow stationActions = new StationWindow(mainData, (StationToList)ListOfStations.SelectedItem);
            stationActions.Show();
            this.Close();
        }
    }
}
