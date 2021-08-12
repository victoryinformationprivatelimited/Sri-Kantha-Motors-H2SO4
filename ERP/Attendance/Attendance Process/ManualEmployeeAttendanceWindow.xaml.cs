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
using ERP.Attendance;

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for ManualEmployeeAttendanceWindow.xaml
    /// </summary>
    public partial class ManualEmployeeAttendanceWindow : Window
    {

        ManualEmployeeAttendanceViewModel viewModel;
        public ManualEmployeeAttendanceWindow()
        {
            InitializeComponent();
            viewModel = new ManualEmployeeAttendanceViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ManualAttendanceExcelUploadWindow ManualAttendanceExcelUploadW = new ManualAttendanceExcelUploadWindow();
            ManualAttendanceExcelUploadW.Show();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            ManualAttendanceSelectionWindow ManualAttendanceSelectionW = new ManualAttendanceSelectionWindow();
            ManualAttendanceSelectionW.Show();
        }
    }
}
