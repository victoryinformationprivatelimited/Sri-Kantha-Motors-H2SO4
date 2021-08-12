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
	/// Interaction logic for DailyMorningGraceAttendanceWindow.xaml
	/// </summary>
	public partial class DailyMorningGraceAttendanceWindow : Window
	{
		DailyMorningAttendanceViewModel viewmodel = new DailyMorningAttendanceViewModel();
        public DailyMorningGraceAttendanceWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void button2_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
			this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

	}
}