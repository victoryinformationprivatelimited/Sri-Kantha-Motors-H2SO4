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

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceAfterProcess.xaml
    /// </summary>
    public partial class EmployeeAttendanceAfterProcess : UserControl
    {
        public EmployeeAttendanceAfterProcess(AttendanceProcessViewModel view)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = view; };
        }
    }
}
