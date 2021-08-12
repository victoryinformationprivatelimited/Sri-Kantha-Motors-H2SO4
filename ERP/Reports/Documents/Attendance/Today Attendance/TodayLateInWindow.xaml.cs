using ERP.HelperClass;
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
    /// Interaction logic for TodayLateInWindow.xaml
    /// </summary>
    public partial class TodayLateInWindow : Window
    {

        public TodayLateInWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
        }

        private void today_late_in_report_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void today_late_in_report_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void today_late_in_report_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}