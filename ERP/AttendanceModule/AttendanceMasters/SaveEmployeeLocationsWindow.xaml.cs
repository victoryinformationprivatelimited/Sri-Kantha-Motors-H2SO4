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
    /// Interaction logic for SaveEmployeeLocationsWindow.xaml
    /// </summary>
    public partial class SaveEmployeeLocationsWindow : Window
    {
        private AssignEmployeeLocationViewModel viewModel;
        public SaveEmployeeLocationsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new AssignEmployeeLocationViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
