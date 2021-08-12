using ERP.Leave;
using ERP.Loan_Module.New_LoanForms;
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
    /// Interaction logic for LoanReportsUserControl.xaml
    /// </summary>
    public partial class LoanReportsUserControl : UserControl
    {
        #region MyRegion
        EmployeeLoanDetailsReportWindow reportWindow;
        ExternalBankLoanReportWindow ExternalBankLoanWindow;
        ExternalBankLoanSlipWindow windowEBLS;
        #endregion
        public LoanReportsUserControl()
        {
            InitializeComponent();
           // RPTEmployeeLoanDetails = new EmployeeLoanDetailsReportWindow();
        }

        private void EmployeeLoan_Click(object sender, RoutedEventArgs e)
        {
            //this.closeWindows();
            EmployeeLoanDetailsReportViewModel ViewModel = new EmployeeLoanDetailsReportViewModel();
            reportWindow = new EmployeeLoanDetailsReportWindow(ViewModel);
            reportWindow.Show();
          
        }

        private void EmployeePendingLoans_Click(object sender, RoutedEventArgs e)
        {
            EmployeePendingLoansViewModel ViewModel = new EmployeePendingLoansViewModel();
            reportWindow = new EmployeeLoanDetailsReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void EmployeeRejectedLoans_Click(object sender, RoutedEventArgs e)
        {
            EmployeesRejectedLoansRPTViewModel ViewModel = new EmployeesRejectedLoansRPTViewModel();
            reportWindow = new EmployeeLoanDetailsReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void EmployeeGuaranteedLoan_Click(object sender, RoutedEventArgs e)
        {
            EmployeeGuarenteedLoansViewModel ViewModel = new EmployeeGuarenteedLoansViewModel();
            reportWindow = new EmployeeLoanDetailsReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void ExternalBankLoanEMPwise_Click(object sender, RoutedEventArgs e)
        {
            ActiveExternalBankLoanEmployeeWiseViewModel ViewModel = new ActiveExternalBankLoanEmployeeWiseViewModel();
            reportWindow = new EmployeeLoanDetailsReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void ExternalBankLoanBankWise_Click(object sender, RoutedEventArgs e)
        {
            BankAndBranchWiseProcessedExternalBankloanViewModel ViewModel = new BankAndBranchWiseProcessedExternalBankloanViewModel();
            ExternalBankLoanWindow = new ExternalBankLoanReportWindow(ViewModel);
            ExternalBankLoanWindow.Show();
        }

        private void InternalBankLoanSummary_Click(object sender, RoutedEventArgs e)
        {
            InternalLoanSummaryForCurrentPayPeriodViewModel ViewModel = new InternalLoanSummaryForCurrentPayPeriodViewModel();
            reportWindow = new EmployeeLoanDetailsReportWindow(ViewModel);
            reportWindow.Show();
        }
    }
}
