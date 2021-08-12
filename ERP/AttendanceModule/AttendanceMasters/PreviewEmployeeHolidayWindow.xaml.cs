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

namespace ERP.AttendanceModule.AttendanceMasters
{
	/// <summary>
	/// Interaction logic for PreviewEmployeeHolidayWindow.xaml
	/// </summary>
	public partial class PreviewEmployeeHolidayWindow : Window
	{
        public AssignEmployeeHolidayWindow ParentWindow;

        public PreviewEmployeeHolidayWindow(AssignEmployeeHolidayViewModel dataViewModel)
        {
            this.InitializeComponent();
            if (dataViewModel != null)
                this.Loaded += (s, e) => { DataContext = dataViewModel; };
            if (ParentWindow != null)
                this.Owner = ParentWindow;
        }

		private void viewEmpHolidayTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void holidayAssignedTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void holidayAssignedCloseBtn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}
	}
}