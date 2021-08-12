using CustomBusyBox;
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

namespace ERP.AttendanceModule.AttendanceProcessMaster
{
    /// <summary>
    /// Interaction logic for AttendanceProcessWindow.xaml
    /// </summary>
    public partial class AttendanceProcessWindow : Window
    {
        AttendanceProcessViewModel viewModel;
        public AttendanceProcessWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            viewModel = new AttendanceProcessViewModel();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void attendanceProcessTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void attendanceProcessTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void attendanceProcessCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
