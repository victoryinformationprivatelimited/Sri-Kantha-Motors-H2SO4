﻿using ERP.HelperClass;
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
	/// Interaction logic for Attendance.xaml
	/// </summary>
	public partial class ShiftDetail : Window
	{
        ShiftDetailViewModel viewModel;
		public ShiftDetail()
		{
            this.Owner= clsWindow.Mainform;
			this.InitializeComponent();
            viewModel = new ShiftDetailViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
		}

        private void shiftDetailCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void shiftDetailTaskTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void shiftDetailTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

	}
}