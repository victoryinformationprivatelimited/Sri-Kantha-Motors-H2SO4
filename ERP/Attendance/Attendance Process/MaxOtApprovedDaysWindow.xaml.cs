using ERP.Attendance.Basic_Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for MaxOtApprovedDaysWindow.xaml
    /// </summary>
    public partial class MaxOtApprovedDaysWindow : Window
    {
        MaxOtApprovedDaysViewModel maxOt;
        public MaxOtApprovedDaysWindow()
        {
            InitializeComponent();
            maxOt = new MaxOtApprovedDaysViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.maxOt; };
        }

        private void Max_OT_ApproveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
