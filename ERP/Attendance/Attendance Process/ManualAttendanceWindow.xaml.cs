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
    /// Interaction logic for ManualAttendanceWindow.xaml
    /// </summary>
    public partial class ManualAttendanceWindow : Window
    {
        ManualAttendanceViewModelcs ViewModel;

        public ManualAttendanceWindow()
        {
            InitializeComponent();
            ViewModel = new ManualAttendanceViewModelcs();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
