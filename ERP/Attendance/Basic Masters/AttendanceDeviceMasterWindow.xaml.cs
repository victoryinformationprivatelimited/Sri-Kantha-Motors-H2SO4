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

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for AttendanceDeviceMasterWindow.xaml
    /// </summary>
    public partial class AttendanceDeviceMasterWindow : Window
    {
        AttendanceDeviceViewModel viewModel;
        public AttendanceDeviceMasterWindow()
        {
            InitializeComponent();
            viewModel = new AttendanceDeviceViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Attendence_Device_Masters_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        private void searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
