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

namespace ERP.Reports.Documents.Advance_Report
{
    /// <summary>
    /// Interaction logic for SMSReportWindow.xaml
    /// </summary>
    public partial class SMSReportWindow : Window
    {
        SMSReportViewModel viewModel = new SMSReportViewModel();
        public SMSReportWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
