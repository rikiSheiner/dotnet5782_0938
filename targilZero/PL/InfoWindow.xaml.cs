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

namespace PL
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            infoCompanyTB.Text = "Tired of standing in line at the post office for hours?\nTired of waiting weeks for delivery?\nThat's why we're here.\nFlying packages, the future is already here.\nOur company provides air and fast delivery service by dedicated drones.\nSo, how does it work ? very simple... \nEnter your packages into the system, choose who you want\nto send them to and set the urgency level of the package. \nWe will provide you with immediate deliveries. \nWe have no delays and no excuses.";
        }
    }
}
