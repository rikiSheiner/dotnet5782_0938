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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private IBL mainData;
        private bool isWorker;
        public SignUpWindow(IBL data)
        {
            InitializeComponent();
            isWorker = false;
            mainData = data;
            closeWindow.Click += closeWindow_Click;
            ButtonSignUp.MouseDoubleClick += ButtonSignUp_MouseDoubleClick;
            
        }
        public SignUpWindow(IBL data, bool chooseUserType)
        {
            InitializeComponent();
            isWorker = chooseUserType;
            mainData = data;
            closeWindow.Click += closeWindow_Click;
            ButtonSignUp.MouseDoubleClick += ButtonSignUp_MouseDoubleClick;

            if(chooseUserType )
            {
                enterUserType.Visibility = Visibility.Visible;
                typesOfUsers.Visibility = Visibility.Visible;
                typesOfUsers.Items.Add("wroker");
                typesOfUsers.Items.Add("normal");
            }    
        }
        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonSignUp_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!isWorker )
                {
                    mainData.AddUser(newUserName.Text.ToString(), newUserPassword.Text.ToString(), false);
                    MessageBox.Show("Congratulation! welcome to our site " + newUserName.Text);
                    DAL.DalApi.DO.User currentUser = mainData.FindAndGetUser(newUserName.Text, newUserPassword.Text);
                    new MenuWindow(mainData, currentUser).Show();

                }
                else
                {
                    if(typesOfUsers .SelectedItem.ToString () == "worker")
                    {
                        mainData.AddUser(newUserName.Text.ToString(), newUserPassword.Text.ToString(), true);
                        MessageBox.Show("The worker has been added successfully");
                    }
                    else
                    {
                        mainData.AddUser(newUserName.Text.ToString(), newUserPassword.Text.ToString(), false);
                        MessageBox.Show("The user has been added successfully");
                    }
                        
                }
                Close();
            }
            catch (AddingProblemException ape)
            {
                MessageBox.Show(ape.Message);
            }
            catch(DAL.DalApi .ExistIdException existID)
            {
                MessageBox.Show(existID.Message);
            }
            catch(Exception )
            {
                MessageBox.Show("ERROR");
            }

        }



    }
}
