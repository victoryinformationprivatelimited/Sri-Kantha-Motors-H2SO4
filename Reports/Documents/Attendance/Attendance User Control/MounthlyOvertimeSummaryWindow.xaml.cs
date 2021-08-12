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
	public partial class MounthlyOvertimeSummaryWindow : Window
	{
		public MounthlyOvertimeSummaryWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            MonthlyOvertimeSummaryViewModel viewmodel = new MonthlyOvertimeSummaryViewModel();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void monthly_ot_summary_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void monthly_ot_summary_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void monthly_ot_summary_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}