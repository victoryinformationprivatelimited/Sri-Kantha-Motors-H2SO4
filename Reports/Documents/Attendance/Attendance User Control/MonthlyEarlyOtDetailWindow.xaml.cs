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
	/// Interaction logic for MonthlyEarlyOtDetailWindow.xaml
	/// </summary>
	public partial class MonthlyEarlyOtDetailWindow : Window
	{
		MounthlyEarlyOtDetailViewModel viewmodel = new MounthlyEarlyOtDetailViewModel();
        public MonthlyEarlyOtDetailWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void early_ot_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void early_ot_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void early_ot_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}