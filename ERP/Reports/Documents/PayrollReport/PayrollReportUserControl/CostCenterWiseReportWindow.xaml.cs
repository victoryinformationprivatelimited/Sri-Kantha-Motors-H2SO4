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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    /// <summary>
    /// Interaction logic for CostCenterWiseReportWindow.xaml
    /// </summary>
    public partial class CostCenterWiseReportWindow : Window
    {
        CostCenterWiseReportViewModel viewModel;
        public CostCenterWiseReportWindow()
        {
            InitializeComponent();
            viewModel = new CostCenterWiseReportViewModel();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }
    }
}
