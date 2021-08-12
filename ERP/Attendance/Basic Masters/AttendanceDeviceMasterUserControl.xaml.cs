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

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for AttendanceDeviceMasterUserControl.xaml
    /// </summary>
    public partial class AttendanceDeviceMasterUserControl : UserControl
    {
        private AttendanceDeviceViewModel viewModel = new AttendanceDeviceViewModel();
        public AttendanceDeviceMasterUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void Attendence_Device_Masters_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
