using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP.AttendanceModule.AttendanceMasters
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceGroupAssignWindow.xaml
    /// </summary>
    public partial class EmployeeAttendanceGroupAssignWindow : Window
    {
        EmployeeAttendanceGroupAssignViewModel viewModel;
        
        public EmployeeAttendanceGroupAssignWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new EmployeeAttendanceGroupAssignViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; viewModel.empGroupsGrid = _empGrpGrid; };
        }

        private void empAttendanceGroupsAssignTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void empAttendanceGroupsAssignTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void empAttendanceGroupsAssignCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
