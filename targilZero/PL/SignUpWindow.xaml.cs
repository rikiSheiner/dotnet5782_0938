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
        IBL mainData;
        public SignUpWindow(IBL data)
        {
            InitializeComponent();
            mainData = data;
            closeWindow.Click += closeWindow_Click;
            ButtonSignUp.MouseDoubleClick += ButtonSignUp_MouseDoubleClick;
            
        }
        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonSignUp_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainData.AddUser(newUserName.Text.ToString (), newUserPassword.Text.ToString ());
                MessageBox.Show("Congratulation! welcome to our site " + newUserName .Text);
                new MenuWindow(mainData).Show();
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
