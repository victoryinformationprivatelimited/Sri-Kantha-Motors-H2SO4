using ERP.Reports;
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

namespace ERP.Loan_Module.Reports
{
    /// <summary>
    /// Interaction logic for ApprovedAndRejectUserControl.xaml
    /// </summary>
    public partial class ApprovedAndRejectUserControl : UserControl
    {
        public ApprovedAndRejectUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {//2014-09-01

            try
            {
                string RptStartDate = ((DateTime)StartDate.SelectedDate).Year + "-" + ((DateTime)StartDate.SelectedDate).Month + "-" + ((DateTime)StartDate.SelectedDate).Day;
                string RptEndDate = ((DateTime)EndDate.SelectedDate).Year + "-" + ((DateTime)EndDate.SelectedDate).Month + "-" + ((DateTime)EndDate.SelectedDate).Day;


                ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\ApprovalReport");

                print.setParameterValue("@StartDate", RptStartDate);

                print.setParameterValue("@EndDate", RptEndDate);

                print.setParameterValue("ApprovalType", "Approved List");
                print.setParameterValue("@Approved", "False");
                print.PrintReportWithReportViewer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            try
            {
                string RptStartDate = ((DateTime)StartDate.SelectedDate).Year + "-" + ((DateTime)StartDate.SelectedDate).Month + "-" + ((DateTime)StartDate.SelectedDate).Day;
                string RptEndDate = ((DateTime)EndDate.SelectedDate).Year + "-" + ((DateTime)EndDate.SelectedDate).Month + "-" + ((DateTime)EndDate.SelectedDate).Day;


                ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\ApprovalReport");

                print.setParameterValue("@StartDate", RptStartDate);

                print.setParameterValue("@EndDate", RptEndDate);
                print.setParameterValue("ApprovalType", "Reject List");
                print.setParameterValue("@Approved", "True");
                print.PrintReportWithReportViewer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
