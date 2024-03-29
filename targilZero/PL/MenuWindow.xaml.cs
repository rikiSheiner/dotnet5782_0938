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
using BL.BlApi;
using DAL.DalApi.DO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private IBL mainData;
        private User currentUser;
        public MenuWindow(IBL data, User user)
        {
            InitializeComponent();
            mainData = data;
            currentUser = user;
            this.GoToListDronesWindow.MouseDoubleClick += GoToListDronesWindow_MouseDoubleClick;
            this.GoToListParcelsWindow.MouseDoubleClick += GoToListParcelsWindow_MouseDoubleClick;
            this.GoToListStationsWindow.MouseDoubleClick += GoToListStationsWindow_MouseDoubleClick;
            this.GoToListCustomersWindow.MouseDoubleClick  += GoToListCustomersWindow_MouseDoubleClick;
            this.closeWindow.Click += closeWindow_Click;
            signOut.MouseDoubleClick += signOut_MouseDoubleClick;
            AddUser.MouseDoubleClick += AddUser_MouseDoubleClick;
            if (user.UserAccessManagement == true)
                AddUser.Visibility = Visibility.Visible;

            DateTime now = DateTime.Now;
            if (now.Hour >= 4 && now.Hour <= 12)
                HelloUser.Text = "Good morning " + currentUser.UserName + "!";
            else if (now.Hour > 12 && now.Hour <= 17)
                HelloUser.Text = "Good afternoon " + currentUser.UserName + "!";
            else if (now.Hour > 17 && now.Hour <= 21)
                HelloUser.Text = "Good evening " + currentUser.UserName + "!";
            else
                HelloUser.Text = "Good night " + currentUser.UserName + "!";

        }

        private void closeWindow_Click(object sender, RoutedEventArgs e) { Close(); }
        private void GoToListDronesWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListDronesWindow listDronesWindow = new ListDronesWindow(mainData);
            listDronesWindow.Show();
        }
        private void GoToListCustomersWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            CustomersListWindow customersListWindow = new CustomersListWindow(mainData);
            customersListWindow.Show();
        }

        private void GoToListParcelsWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new ParcelsListWindow(mainData, currentUser.UserAccessManagement);
            parcelsListWindow.Show();
        }

        private void GoToListStationsWindow_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            StationsListWindow stationsListWindow = new StationsListWindow(mainData);
            stationsListWindow.Show();
        }

        private void signOut_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void AddUser_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(mainData ,true).Show();

        }
    }
}
