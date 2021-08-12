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

namespace ERP.Reports.Documents.Leave.LeaveUserControl
{
    /// <summary>
    /// Interaction logic for LeaveSummaryReportWindow.xaml
    /// </summary>
    public partial class LeaveSummaryReportWindow : Window
    {
        LeaveSumarryViewModel viewmodel = new LeaveSumarryViewModel();
        public LeaveSummaryReportWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void leave_summary_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leave_summary_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leave_summary_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
