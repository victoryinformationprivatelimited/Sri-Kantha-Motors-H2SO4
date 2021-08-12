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
    public partial class MaxOtAttendanceDataWindow : Window
    {
        MaxOtAttendanceDataViewModel viewModel;
        public MaxOtAttendanceDataWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.viewModel = new MaxOtAttendanceDataViewModel();
            Loaded += (s, e) =>
            {
                DataContext = viewModel;
            };
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}