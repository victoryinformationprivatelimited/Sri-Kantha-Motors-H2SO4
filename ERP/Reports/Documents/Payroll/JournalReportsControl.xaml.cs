using ERP.Reports.Documents.PayrollReport.PayrollReportUserControl;
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

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for JournalReportsControl.xaml
    /// </summary>
    public partial class JournalReportsControl : UserControl
    {
        CostCenterWiseReportWindow CostCenterWiseReportW;

        public JournalReportsControl()
        {
            InitializeComponent();
        }

        void formClose()
        {
            if (CostCenterWiseReportW != null)
                CostCenterWiseReportW.Close();
        }

        private void costcenterwisereport_Checked(object sender, RoutedEventArgs e)
        {
            formClose();
            CostCenterWiseReportW = new CostCenterWiseReportWindow();
            CostCenterWiseReportW.Show();
            
        }
    }
}
