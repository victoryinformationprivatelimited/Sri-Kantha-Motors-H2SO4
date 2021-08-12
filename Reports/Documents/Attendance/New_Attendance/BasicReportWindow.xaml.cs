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

namespace ERP.Reports.Documents.Attendance.New_Attendance
{
    /// <summary>
    /// Interaction logic for BasicReportWindow.xaml
    /// </summary>
    public partial class BasicReportWindow : Window
    {
        public BasicReportWindow(ViewModelBase viewModel)
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void basic_report_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void basic_report_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
