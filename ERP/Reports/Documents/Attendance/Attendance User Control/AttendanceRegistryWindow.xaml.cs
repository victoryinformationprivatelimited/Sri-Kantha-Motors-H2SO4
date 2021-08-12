using ERP.HelperClass;
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

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    /// <summary>
    /// Interaction logic for AttendanceRegistryWindow.xaml
    /// </summary>
    public partial class AttendanceRegistryWindow : Window
    {
        AttendanceRegistryViewModel ViewModel;

        public AttendanceRegistryWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new AttendanceRegistryViewModel();
            Loaded += (s, e) => { DataContext = ViewModel; };

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
