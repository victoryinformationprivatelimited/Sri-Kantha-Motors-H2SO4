using ERP.HelperClass;
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

namespace ERP.AttendanceModule.AttendanceMasters
{
    /// <summary>
    /// Interaction logic for AssignEmployeeLocations.xaml
    /// </summary>
    public partial class AssignEmployeeLocations : Window
    {
        private AssignEmployeeLocationViewModel viewModel;

        SaveEmployeeLocationsWindow saveEmployeeLocations;
        public AssignEmployeeLocations()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new AssignEmployeeLocationViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        void closeWindows()
        {
            if (saveEmployeeLocations != null)
                saveEmployeeLocations.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(322))
            {

                this.closeWindows();
                saveEmployeeLocations = new SaveEmployeeLocationsWindow();
                saveEmployeeLocations.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }
    }
}
