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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private IBL mainData;
        public ParcelsListWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
            ListOfParcels.ItemsSource = mainData.GetListParcels();
            this.closeWindow.Click += closeWindow_Click;
            AddParcelButton.MouseDoubleClick += AddParcelButton_MouseDoubleClick;
            ListOfParcels.SelectionChanged += ListOfParcels_SelectionChanged;
            FilterParcelsList.SelectionChanged += FilterParcelsList_SelectionChanged;
            refreshWindow.MouseDoubleClick += refreshWindow_MouseDoubleClick;
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void AddParcelButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ParcelWindow parcelWindow = new ParcelWindow(mainData);
            parcelWindow.Show();
        }
        private void ListOfParcels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelWindow parcelActions = new ParcelWindow(mainData, (ParcelToList)ListOfParcels.SelectedItem);
            parcelActions.Show();
            this.Close();
        }

        private void FilterParcelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FilterParcelsList.SelectedIndex)
            {
                case 0:
                    ListOfParcels .ItemsSource = mainData.GetListParcels();
                    break;
                case 1:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.priority == Enums.Priorities.normal) ;
                    break;
                case 2:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.priority == Enums.Priorities.quick);
                    break;
                case 3:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.priority == Enums.Priorities.emergency);
                    break;
                case 4:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.weight == Enums.WeightCategories .light );
                    break;
                case 5:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.weight == Enums.WeightCategories.intermediate );
                    break;
                case 6:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.weight == Enums.WeightCategories.heavy);
                    break;
                case 7:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.parcelStatus == Enums.ParcelStatuses .defined );
                    break;
                case 8:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.parcelStatus == Enums.ParcelStatuses.assigned);
                    break;
                case 9:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.parcelStatus == Enums.ParcelStatuses.collected);
                    break;
                case 10:
                    ListOfParcels.ItemsSource = mainData.GetListParcelsWithCondition(x => x.parcelStatus == Enums.ParcelStatuses.supplied );
                    break;
                case 11:// specific sender of parcel
                    
                    break;
                case 12: // specific target of parcel
                    
                    break;
                default:
                    break;

            }

        }

        private void refreshWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ParcelsListWindow newWindow = new ParcelsListWindow(mainData);
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            Close();
        }
    }
}
