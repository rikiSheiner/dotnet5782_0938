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
using DAL.DalApi.DO;


namespace PL
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        IBL mainData;
        public LogInWindow(IBL data)
        {
            InitializeComponent();
            mainData = data; 
            closeWindow.Click += closeWindow_Click;
            ButtonLogIn.MouseDoubleClick += ButtonLogIn_MouseDoubleClick;
        }
        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonLogIn_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainData.IsUserExist(currentUserName.Text, currentUserPassword.Text) )
                {
                    DAL.DalApi.DO.User currentUser= mainData .FindAndGetUser (currentUserName .Text , currentUserPassword .Text );
                    if(!currentUser .UserAccessManagement  ) //if the user is not employee
                    {
                        MessageBox.Show("Welcome " + currentUserName.Text + "!");
                        new MenuWindow(mainData,currentUser ).Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Welcome worker " + currentUserName.Text + "!");
                        new MenuWindow(mainData, currentUser).Show();
                        Close();
                    }

                }
                else
                {
                    MessageBox.Show("wrong password or user name. Please try again");
                }
            }
            catch(Exception )
            {
                MessageBox.Show("ERROR");
            }
        }
    }
}
