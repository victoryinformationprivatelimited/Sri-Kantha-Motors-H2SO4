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
    /// Interaction logic for AttendanceDataMigrationWindow.xaml
    /// </summary>
    public partial class AttendanceDataMigrationWindow : Window
    {
        AttendanceDataMigrationViewModel viewModel;
        public AttendanceDataMigrationWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            viewModel = new AttendanceDataMigrationViewModel();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void dataMaigrationTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void dataMigrationTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void dataMigrationCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
