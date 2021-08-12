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
    /// Interaction logic for MonthlyDetailAttendanceUserControl.xaml
    /// </summary>
    public partial class MonthlyDetailAttendanceUserControl : UserControl
    {
        MonthlyDetailAttendanceViewModel viewmodel = new MonthlyDetailAttendanceViewModel(); 
        public MonthlyDetailAttendanceUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }
    }
}
