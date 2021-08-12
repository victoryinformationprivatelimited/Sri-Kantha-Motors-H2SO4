using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardMainUI.xaml
    /// </summary>
    public partial class DashboardMainUI : UserControl
    {

        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public DashboardMainUI()
        {
            InitializeComponent();
            DashboardWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            DashboardWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        private void employeeBtn_Click(object sender, RoutedEventArgs e)
        {
            MDIParent.Children.Clear();
            MDIParent.Children.Add(new EmployeeSection());
        }

        private void payrollBtn_Click(object sender, RoutedEventArgs e)
        {
            MDIParent.Children.Clear();
            MDIParent.Children.Add(new PayrollSection());
        }

        private void attendanceBtn_Click(object sender, RoutedEventArgs e)
        {
            MDIParent.Children.Clear();
            MDIParent.Children.Add(new AttendanceSection());
        }

        private void employeeBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            employeeBtn.Foreground = Brushes.Black;
        }

        private void employeeBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            employeeBtn.Foreground = Brushes.White;
        }

        private void payrollBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            payrollBtn.Foreground = Brushes.Black;
        }

        private void payrollBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            payrollBtn.Foreground = Brushes.White;
        }

        private void attendanceBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            attendanceBtn.Foreground = Brushes.Black;
        }

        private void attendanceBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            attendanceBtn.Foreground = Brushes.White;
        }
    }
}
