using ERP.Notification;
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
    /// Interaction logic for DailyAttendanceProcess.xaml
    /// </summary>
    public partial class DailyAttendanceProcess : UserControl
    {
        DailyAttendanceProcessViewModel viewmodel = new DailyAttendanceProcessViewModel();
        public DailyAttendanceProcess()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewmodel; };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            progressBar DailyProgress = new progressBar(viewmodel);
            ProcessMDI.Children.Clear();
            ProcessMDI.Children.Add(DailyProgress);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
