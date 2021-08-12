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

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for PayrollReportsOnlyUserControl.xaml
    /// </summary>
    public partial class PayrollReportsOnlyUserControl : UserControl
    {
        public PayrollReportsOnlyUserControl()
        {
            InitializeComponent();
        }

        private void Payroll_Report_Click(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            PayrollReportControll dt = new PayrollReportControll();
            MDIWrip.Children.Add(dt);
        }

        private void Slip_Transfer_Click(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            SlipTransferControll dt = new SlipTransferControll();
            MDIWrip.Children.Add(dt);
        }

        private void Journal_Report_Click(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            JournalReportsControl dt = new JournalReportsControl();
            MDIWrip.Children.Add(dt);
        }

        private void Payrol_Report_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
