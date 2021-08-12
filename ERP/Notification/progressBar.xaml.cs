using ERP.Attendance.Attendance_Data_Migration;
using ERP.Attendance.Attendance_Process;
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

namespace ERP.Notification
{
    /// <summary>
    /// Interaction logic for progressBar.xaml
    /// </summary>
    public partial class progressBar : UserControl
    {
        public progressBar(AttendanceProcessViewModel viewmodel)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }
        public progressBar(DailyAttendanceProcessViewModel Dailyviewmodel)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = Dailyviewmodel; };
        }
        //public progressBar(AttendanceDataMigrationViewModel progressbar)
        //{
        //    InitializeComponent();
        //    this.Loaded += (s, e) => { this.DataContext = progressbar; };
        //}
    }
}
