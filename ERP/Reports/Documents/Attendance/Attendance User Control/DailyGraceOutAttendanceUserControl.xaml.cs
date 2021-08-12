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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    /// <summary>
    /// Interaction logic for DailyGraceOutAttendanceUserControl.xaml
    /// </summary>
    public partial class DailyGraceOutAttendanceUserControl : UserControl
    {
        DailyGraceOutAttendanceViewModel viewmodel = new DailyGraceOutAttendanceViewModel();
        public DailyGraceOutAttendanceUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void button1_Copy_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Copy1_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
