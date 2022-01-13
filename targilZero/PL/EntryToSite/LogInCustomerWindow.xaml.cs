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

namespace PL
{
    /// <summary>
    /// Interaction logic for LogInCustomerWindow.xaml
    /// </summary>
    public partial class LogInCustomerWindow : Window
    {
        private IBL mainData;
        public LogInCustomerWindow(IBL data)
        {
            mainData = data;
            InitializeComponent();
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
                
                int id = int.Parse(currentID.Text);
                if (mainData.FindCustomer(id )>=0)
                {
                    new CustomerActionsWindow(mainData, mainData.ConvertCustomerToCustomerInList(mainData.FindAndGetCustomer(id))).Show ();
                    Close();
                }
                else
                {
                    MessageBox.Show("wrong ID number of customer. Please try again");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
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
    }
}
