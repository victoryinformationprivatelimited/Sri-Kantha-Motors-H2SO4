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
	/// Interaction logic for MonthlyEarlyOutWindow.xaml
	/// </summary>
	public partial class MonthlyEarlyOutWindow : Window
	{
		public MonthlyEarlyOutWindow(MonthlyEarlyOutViewModel viewmodel)
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

		private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Close();
		}

		private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.DragMove();
		}

		private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.DragMove();
		}
	}
}