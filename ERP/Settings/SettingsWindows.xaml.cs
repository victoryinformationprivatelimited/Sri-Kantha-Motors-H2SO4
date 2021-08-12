using ERP.UI_UserControlers;
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

namespace ERP.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindows.xaml
    /// </summary>
    public partial class SettingsWindows : UserControl
    {
        AdministratorUserControl administratorUserControl;
        public SettingsWindows(AdministratorUserControl administratorUserControl)
        {
            InitializeComponent();
            this.administratorUserControl = administratorUserControl;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsMenu_Content settingsMenuContent = new SettingsMenu_Content();
            Mdiwrappanel.Children.Add(settingsMenuContent);
            administratorUserControl.windowName.Content = "Settings";
            administratorUserControl.windowIcon.Width = 30;
            administratorUserControl.windowIcon.Height = 30;
            administratorUserControl.windowIcon.Margin = new Thickness(0, 3, 113, 0);
            var uriSource = new Uri(@"/ERP;component/Images/New_Button/New_Admin_Button/Settings.png", UriKind.Relative);
            administratorUserControl.windowIcon.Source = new BitmapImage(uriSource);
            /*/ERP;component/Images/New_Button/New_Admin_Button/Settings.png*/

        }
    }
}
