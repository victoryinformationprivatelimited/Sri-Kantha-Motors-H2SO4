using ERP.WebPortal;
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

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for WebPortalButton.xaml
    /// </summary>
    public partial class WebPortalButton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        PortalEventsWindow PotealEventsW;
        PortalMeetingsWindow PortalMeetingsW;
        PortalNewsWindow PortalNewsW;

        public WebPortalButton(WebPortal.WebPortalUserControl Portal)
        {
            InitializeComponent();
            MDIWrip = Portal.Mdi;
        }

        private void Hr_Checkbox_two_Checked_1(object sender, RoutedEventArgs e)
        {

            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Hr_Checkbox_two_Unchecked_1(object sender, RoutedEventArgs e)
        {

            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Portal_Events_Click_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1101))
            {
                Hr_Checkbox_two.IsChecked = false;
                WindowClose();
                PotealEventsW = new PortalEventsWindow();
                PotealEventsW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Portal_Meetings_Click_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1102))
            {
                Hr_Checkbox_two.IsChecked = false;
                WindowClose();
                PortalMeetingsW = new PortalMeetingsWindow();
                PortalMeetingsW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }


        private void Portal_News_Click_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1103))
            {
                Hr_Checkbox_two.IsChecked = false;
                WindowClose();
                PortalNewsW = new PortalNewsWindow();
                PortalNewsW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void WindowClose()
        {
            if (PotealEventsW != null)
                PotealEventsW.Close();
            if (PortalMeetingsW != null)
                PortalMeetingsW.Close();
            if (PortalNewsW != null)
                PortalNewsW.Close();
        }

    }
}
