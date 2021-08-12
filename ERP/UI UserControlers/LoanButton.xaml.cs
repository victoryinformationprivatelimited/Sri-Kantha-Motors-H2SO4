using ERP.HelperClass;
using ERP.Loan_Module;
using ERP.Loan_Module.Basic_Masters;
using ERP.Loan_Module.Detail_Masters;
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

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for LoanButton.xaml
    /// </summary>
    public partial class LoanButton : UserControl
    {
        //private double defaultHeight = 700;
        //private double defaultWidth = 900;
        #region Forms

        LoanCatergoryWindow LoanCatergoryW;
        ManualInternalLoanPaymentWindow LoanPaymentW;
        LoansWindow LoansW;
        EmployeeLoansWindow EmployeeLoansW;
        InternalLoanApprovalWindow LoanApprovalW;
        CheckEmployeeLoanWindow ChkEmpLoanW;
        EmployeeBankLoanWindow EmpBankLoanW;
        EmployeeNewLoanWindow InternalLoanW;
        ExternalBankLoanProcessWindow ExternalBankLoanProcessW;
        #endregion

        WrapPanel MDIWrip = new WrapPanel();

        public LoanButton(LoanUserControl LoanUserControl)
        {
            InitializeComponent();
            MDIWrip = LoanUserControl.Mdi;
        }

        private void Hr_Checkbox_two_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Hr_Checkbox_two_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void LoadToWrappanel(UserControl userControl)
        {
            Hr_Checkbox_two.IsChecked = false;
            MDIWrip.Children.Clear();

            //if (userControl.Width > defaultWidth)
            //  MDIWrip.Width = defaultWidth;
            // else
            MDIWrip.Width = userControl.Width;

            // if (userControl.Height > defaultHeight)
            //MDIWrip.Height = defaultHeight;
            // else
            MDIWrip.Height = userControl.Height;
            MDIWrip.Children.Add(userControl);
        }

        void FormClose()
        {
            try
            {
                if (LoanCatergoryW != null)
                    LoanCatergoryW.Close();
                if (LoanPaymentW != null)
                    LoanPaymentW.Close();
                if (LoansW != null)
                    LoansW.Close();
                if (EmployeeLoansW != null)
                    EmployeeLoansW.Close();
                if (LoanApprovalW != null)
                    LoanApprovalW.Close();

            }
            catch (Exception)
            {

            }
        }

        private void WrapPanel_Unloaded_1(object sender, RoutedEventArgs e)
        {
            FormClose();
        }

        private void Loan_Payment1_Click(object sender, RoutedEventArgs e)
        {
            LoanProcessWindow window = new LoanProcessWindow();
            window.Show();
        }

        private void LoanCatergory_Click(object sender, RoutedEventArgs e)
        {
            // Hr_Checkbox_two.IsChecked = false;
            //MDIWrip.Children.Clear();
            if (clsSecurity.GetViewPermission(601))
            {
                FormClose();
                LoanCatergoryW = new LoanCatergoryWindow();
                LoanCatergoryW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
            //LoanCatergoryUserControl userControl = new LoanCatergoryUserControl();
            //LoadToWrappanel(userControl);
            // MDIWrip.Children.Add(userControl);
        }

        private void Loans_Click(object sender, RoutedEventArgs e)
        {
            // Hr_Checkbox_two.IsChecked = false;
            // MDIWrip.Children.Clear();
            if (clsSecurity.GetViewPermission(602))
            {
                FormClose();
                LoansW = new LoansWindow();
                LoansW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
            //LoansUserControl userControl = new LoansUserControl();
            //LoadToWrappanel(userControl);
            //MDIWrip.Children.Add(userControl);
        }

        private void EmployeeLoans_Click(object sender, RoutedEventArgs e)
        {
            //EmployeeLoansUserControl userControl = new EmployeeLoansUserControl();
            //LoadToWrappanel(userControl);
            if (clsSecurity.GetViewPermission(603))
            {
                FormClose();
                InternalLoanW = new EmployeeNewLoanWindow();
                InternalLoanW.Show(); 
                //
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Loan_Approval_Click(object sender, RoutedEventArgs e)
        {           
            if (clsSecurity.GetViewPermission(604))
            {             
                FormClose();
                LoanApprovalW = new InternalLoanApprovalWindow();
                LoanApprovalW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Loan_Payment_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(605))
            {
                FormClose();
                LoanPaymentW = new ManualInternalLoanPaymentWindow();
                LoanPaymentW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
            //LoanPaymentUserControl userControl = new LoanPaymentUserControl();
            //LoadToWrappanel(userControl);
        }

        private void Loan_Process_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(606))
            {
                FormClose();
                InternalLoanAutoPaymentWindow window = new InternalLoanAutoPaymentWindow();
                window.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Chk_Loan_Availability_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(607))
            {
                FormClose();
                ChkEmpLoanW = new CheckEmployeeLoanWindow();
                ChkEmpLoanW.Show();  
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void ExternalBankLoan_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(608))
            {
                FormClose();
                EmpBankLoanW = new EmployeeBankLoanWindow();
                EmpBankLoanW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void ExternalBankLoanProcess_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(609))
            {
                FormClose();
                ExternalBankLoanProcessW = new ExternalBankLoanProcessWindow();
                ExternalBankLoanProcessW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

    }
}