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

namespace ERP.Attendance
{
    /// <summary>
    /// Interaction logic for EmployeeThirdPartyPaymentsWindow.xaml
    /// </summary>
    public partial class ManualAttendanceExcelUploadWindow : Window
    {
        ManualAttendanceExcelUploadViewModel viewModel;
        public ManualAttendanceExcelUploadWindow()
        {
            InitializeComponent();
            viewModel = new ManualAttendanceExcelUploadViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
    }
}
