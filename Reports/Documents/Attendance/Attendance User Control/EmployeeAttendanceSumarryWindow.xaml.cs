using ERP.HelperClass;
using ERP.Reports.Documents.Attendance.Attendance_User_Control;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP
{
	/// <summary>
	/// Interaction logic for EmployeeAttendanceSumarryWindow.xaml
	/// </summary>
	public partial class EmployeeAttendanceSumarryWindow : Window
	{
		EmployeeAttendanceSumarryViewModel viewmodel = new EmployeeAttendanceSumarryViewModel();
        public EmployeeAttendanceSumarryWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void emp_attendance_summary_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_attendance_summary_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_attendance_summary_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}