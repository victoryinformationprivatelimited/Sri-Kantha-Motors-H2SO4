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

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for ManualEmployeeAttendance.xaml
    /// </summary>
    public partial class ManualEmployeeAttendance : UserControl
    {
        ManualEmployeeAttendanceViewModel viewModel;
        public ManualEmployeeAttendance()
        {
            viewModel = new ManualEmployeeAttendanceViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel;};
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
