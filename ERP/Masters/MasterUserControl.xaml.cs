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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP.MastersDetails;
using ERP.Attendance;
using ERP.UI_UserControlers;


namespace ERP
{
    /// <summary>
    /// Interaction logic for MasterUserControl.xaml
    /// </summary>
    public partial class MasterUserControl : UserControl
    {
        public MasterUserControl()
        {
            InitializeComponent();
            scale();
        }

        private void Masters_usercontrol_Loaded(object sender, RoutedEventArgs e)
        {
            MasterButtons masterbtn = new MasterButtons(this);
            Mdiwrappanel.Children.Add(masterbtn);
        }



        public void scale()
        {
            double W = System.Windows.SystemParameters.PrimaryScreenWidth;
            double h = System.Windows.SystemParameters.PrimaryScreenHeight;
            if (h >= 1024 && W>1025)
                Mdi.Margin = new Thickness(0, 50, 10, 10);
            else if (W == 1024 && h ==768)
                Mdi.Margin = new Thickness(0, 50, 10, 10);
            else
                Mdi.Margin = new Thickness(0, -25, 20, 70);
        }
    }
}
