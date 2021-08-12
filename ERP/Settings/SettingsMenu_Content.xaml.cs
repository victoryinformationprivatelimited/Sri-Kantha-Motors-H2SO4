using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for SettingsMenu_Content.xaml
    /// </summary>
    public partial class SettingsMenu_Content : UserControl
    {
        System.Configuration.Configuration config;
        public SettingsMenu_Content()
        {
            InitializeComponent();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["idAutoGenOn"].Value == "true")
            {
                idAutoGenCheck.IsChecked = true;
            }
            else
            {
                idAutoGenCheck.IsChecked = false;
            }
        }

        private void idAutoGenCheck_Click(object sender, RoutedEventArgs e)
        {
            if (idAutoGenCheck.IsChecked == true)
            {
                config.AppSettings.Settings["idAutoGenOn"].Value = "true";
                clsMessages.setMessage("Employee Id auto generation enabled");
            }
            else
            {
                config.AppSettings.Settings["idAutoGenOn"].Value = "false";
                clsMessages.setMessage("Employee Id auto generation disabled");
            }

            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
