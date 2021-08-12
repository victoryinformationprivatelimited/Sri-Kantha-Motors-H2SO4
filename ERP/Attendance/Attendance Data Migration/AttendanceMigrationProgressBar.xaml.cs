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

namespace ERP.Attendance.Attendance_Data_Migration
{
    /// <summary>
    /// Interaction logic for AttendanceMigrationProgressBar.xaml
    /// </summary>
    public partial class AttendanceMigrationProgressBar : UserControl
    {
        public AttendanceMigrationProgressBar(AttendanceDataMigrationViewModel attendanceProgress)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = attendanceProgress; };
        }
    }
}
