﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ListDronesWindow.xaml
    /// </summary>
    public partial class ListDronesWindow : Window
    {
        private IBL mainData;
        public ListDronesWindow(IBL data)
        {
            InitializeComponent();
            this.mainData = data;
            this.DataContext = mainData.GetListDrones();
            this.FilterDronesList.SelectionChanged += FilterDronesList_SelectionChanged;
            this.closeWindow.Click += closeWindow_Click;
            AddDroneButton.MouseDoubleClick += AddDroneButton_MouseDoubleClick;
        }

        private void FilterDronesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FilterDronesList.SelectedIndex)
            {
                case 0:
                    this.DataContext = mainData.GetListDrones();
                    break;
                case 1:
                    this.DataContext = mainData.GetListDronesWithCondition(x => x.droneStatus == Enums.DroneStatuses.available);
                    break;
                case 2:
                    this.DataContext = mainData.GetListDronesWithCondition(x => x.droneStatus == Enums.DroneStatuses.maintenance);
                    break;
                case 3:
                    this.DataContext = mainData.GetListDronesWithCondition(x => x.droneStatus == Enums.DroneStatuses.delivery);
                    break;
                case 4:
                    this.DataContext = mainData.GetListDronesWithCondition(x => x.maxWeight == Enums.WeightCategories.light );
                    break;
                case 5:
                    this.DataContext = mainData.GetListDronesWithCondition(x => x.maxWeight == Enums.WeightCategories.intermediate );
                    break;
                case 6:
                    this.DataContext = mainData.GetListDronesWithCondition(x => x.maxWeight == Enums.WeightCategories.heavy );
                    break;
                default:
                    break;

            }

        }

        private void closeWindow_Click(object sender, RoutedEventArgs e) { this.Close();}

        
        private void AddDroneButton_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            DroneWindow droneWindowAdding = new DroneWindow(mainData);
            droneWindowAdding.Show();
        }

        private void ListOfDrones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneWindow droneActions = new DroneWindow(mainData, (DroneToList)ListOfDrones .SelectedItem  );
            droneActions.Show();
            this.Close();
        }
    }
}
