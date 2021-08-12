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
using CustomBusyBox;

namespace ERP.Attendance
{
    /// <summary>
    /// Interaction logic for ManualAttendanceSelectionWindow.xaml
    /// </summary>
    public partial class ManualAttendanceSelectionWindow : Window
    {
        ManualAttendanceSelectionViewModel viewModel;
        public ManualAttendanceSelectionWindow()
        {
            BusyBox.InitializeThread(this);
            InitializeComponent();
            viewModel = new ManualAttendanceSelectionViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
    }
}
