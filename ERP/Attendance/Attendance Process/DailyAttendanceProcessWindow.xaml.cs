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
using System.Windows.Shapes;

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for DailyAttendanceProcessWindow.xaml
    /// </summary>
    public partial class DailyAttendanceProcessWindow : Window
    {
        DailyAttendanceProcessViewModel viewmodel;
        public DailyAttendanceProcessWindow()
        {
            InitializeComponent();
            viewmodel = new DailyAttendanceProcessViewModel();
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
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}
