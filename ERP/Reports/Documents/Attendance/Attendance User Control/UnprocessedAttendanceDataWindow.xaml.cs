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

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    /// <summary>
    /// Interaction logic for UnprocessedAttendanceDataWindow.xaml
    /// </summary>
    public partial class UnprocessedAttendanceDataWindow : Window
    {
        UnprocessedAttendanceDataViewModel viewModel;
        public UnprocessedAttendanceDataWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new UnprocessedAttendanceDataViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void unprocess_attendance_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void unprocess_attendance_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void unprocess_attendance_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
