using ERP.Loan_Module.New_LoanForms;
using ERP.Payroll;
using ERP.Payroll.Employee_Bonus;
using ERP.Payroll.Employee_Third_Party_Payments;
using ERP.Payroll.Excel_Sheet_Windows;
using ERP.Payroll.SlipTransfer;
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
    /// Interaction logic for SlipTransferControll.xaml
    /// </summary>
    public partial class SlipTransferControll : UserControl
    {
        SlipTransferWindow window;
        BonusSlipTransferWindow windowBST;
        ExternalBankLoanSlipWindow windowEBLS;
        DeductionRuleWiseSlipTransferWindow windowDRWST;
        ExcelUploadedSlipTransferWindow windowEUST;
        EmployeeThirdPartyPaymentsSlipTransferWindow EmployeeThirdPartyPaymentsSlipTransferW;
        public SlipTransferControll()
        {
            InitializeComponent();
        }

        private void salary_sliptransfer_Checked(object sender, RoutedEventArgs e)
        {
            formClose();

            // Hr_Checkbox_two.IsChecked = false;
            window = new SlipTransferWindow();
            window.Show();
        }
        void formClose()
        {
            if (window != null)
                window.Close();
            if (windowBST != null)
                windowBST.Close();
            if (windowEBLS != null)
                windowEBLS.Close();
            if (windowDRWST != null)
                windowDRWST.Close();
            if (windowEUST != null)
                windowEUST.Close();
            if (EmployeeThirdPartyPaymentsSlipTransferW != null)
                EmployeeThirdPartyPaymentsSlipTransferW.Close();
        }

        private void bonus_payslip_checked(object sender, RoutedEventArgs e)
        {
            formClose();
            windowBST = new BonusSlipTransferWindow();
            windowBST.Show();
        }

        private void external_sliptransfer_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            windowEBLS = new ExternalBankLoanSlipWindow();
            windowEBLS.Show();
        }

        private void deductions_sliptransfer_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            windowDRWST = new DeductionRuleWiseSlipTransferWindow();
            windowDRWST.Show();
        }

        private void other_payments_sliptransfer_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            windowEUST = new ExcelUploadedSlipTransferWindow();
            windowEUST.Show();
        }

        private void thirdparty_payments_sliptransfer_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            EmployeeThirdPartyPaymentsSlipTransferW = new EmployeeThirdPartyPaymentsSlipTransferWindow();
            EmployeeThirdPartyPaymentsSlipTransferW.Show();
        }
    }
}
