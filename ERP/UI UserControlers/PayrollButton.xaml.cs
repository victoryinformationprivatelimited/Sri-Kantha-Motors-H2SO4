using ERP.Attendance.Attendance_Process;
using ERP.Masters;
using ERP.MastersDetails;
using ERP.Payroll;
using ERP.Payroll.Employee_Bonus;
using ERP.Payroll.Employee_Third_Party_Payments;
using ERP.Payroll.Excel_Sheet_Windows;
using ERP.Payroll.ManualPayroll;
using ERP.Payroll.RI_Allowance;
using ERP.Payroll.SlipTransfer;
using ERP.Utility;
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
    /// Interaction logic for PayrollButton.xaml
    /// </summary>
    public partial class PayrollButton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();

        #region Window Members

        BenifitsWindow empBenefitsWindow;
        DeductionsWindow empDeductionWindow;
        CompanyVariableWindow empFundsWindow;
        CompanyRulesWindow empCompanyRuleWindow;
        EmployeeRuleDetailsWindow empRuleDetailWindow;
        PeriodWindow payPeriodWindow;
        EmployeePayrollProcessWindow payrollProcessWindow;
        PayrollPaymentWindow payrollPaymentWindow;
        CompanyVariablsWindow empCompanyVariablesWindow;
        CompanyVariableCompanyRuleDetailWindow empCompanyVaribaleRuleDetailWindow;
        EmployeePeriodQuantityWindow periodQtyWindow;
        DataMigrationWindow dataMigrateWindow;
        //EmployeeAllowanceWindow empAllowanceWindow;
        OtherPaymentsCategoriesWindow OtherPaymentsCategoriesW;
        AllowanceTransactionWindow empAllowanceTransWindow;
        sinhala_payslip Sinhalapayrule;
        EmployeePayRuleDetailsWindow EmployeeRuleDetailsW;
        BonusPeriodWindow BonusPeriodW;
        AsignEmployeeForBonusWindow AssignEmployeeForBonusW;
        PayBonusForAssignedEmployeesWindow PayBonusForAssignedEmployeesW;
        ExcelGenerateForRulesWindow ExcelGenerateForRulesW;
        ExcelUploadForRulesWindow ExcelUploadForRulesW;
        EmployeeThirdPartyPaymentsWindow EmployeeThirdPartyPaymentsW;
        RuleWiseAssignEmployeesWindow RuleWiseAssignEmployeesW;

        Assigning_Basic_Salary_Window BasicWindow;
        #endregion

        public PayrollButton(PayrollUserControl PayrollUserControl)
        {

            InitializeComponent();
            MDIWrip = PayrollUserControl.Mdi;

        }

        #region Forms Clear Methods

        void closeWindows()
        {
            if (empBenefitsWindow != null)
                empBenefitsWindow.Close();
            if (empDeductionWindow != null)
                empDeductionWindow.Close();
            if (empFundsWindow != null)
                empFundsWindow.Close();
            if (empCompanyRuleWindow != null)
                empCompanyRuleWindow.Close();
            if (empRuleDetailWindow != null)
                empRuleDetailWindow.Close();
            if (payPeriodWindow != null)
                payPeriodWindow.Close();
            if (payrollProcessWindow != null)
                payrollProcessWindow.Close();
            if (payrollPaymentWindow != null)
                payrollPaymentWindow.Close();
            if (empCompanyVariablesWindow != null)
                empCompanyVariablesWindow.Close();
            if (empCompanyVaribaleRuleDetailWindow != null)
                empCompanyVaribaleRuleDetailWindow.Close();
            if (periodQtyWindow != null)
                periodQtyWindow.Close();
            if (dataMigrateWindow != null)
                dataMigrateWindow.Close();
            //if (empAllowanceWindow != null)
            //    empAllowanceWindow.Close();
            if (OtherPaymentsCategoriesW != null)
                OtherPaymentsCategoriesW.Close();
            if (empAllowanceTransWindow != null)
                empAllowanceTransWindow.Close();
            if (EmployeeRuleDetailsW != null)
                EmployeeRuleDetailsW.Close();
            if (ExcelGenerateForRulesW != null)
                ExcelGenerateForRulesW.Close();
            if (ExcelUploadForRulesW != null)
                ExcelUploadForRulesW.Close();
            if (EmployeeThirdPartyPaymentsW != null)
                EmployeeThirdPartyPaymentsW.Close();
            if (RuleWiseAssignEmployeesW != null)
                RuleWiseAssignEmployeesW.Close();
        }

        #endregion
        
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

        private void MultipleBankPayment_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            EmployeeMultiplePaymentsUserControl userControl = new EmployeeMultiplePaymentsUserControl();
            MDIWrip.Children.Add(userControl);
        }

        private void Benifits_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(501))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                empBenefitsWindow = new BenifitsWindow();
                empBenefitsWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Deduction_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(502))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                empDeductionWindow = new DeductionsWindow();
                empDeductionWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_Funds_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(503))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                empFundsWindow = new CompanyVariableWindow();
                empFundsWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Company_rules_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(504))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                empCompanyRuleWindow = new CompanyRulesWindow();
                empCompanyRuleWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Rules_Details_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(505))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                empRuleDetailWindow = new EmployeeRuleDetailsWindow();
                empRuleDetailWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Pay_Period1_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(506))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                payPeriodWindow = new PeriodWindow();
                payPeriodWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Pay_Process1_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(507))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                payrollProcessWindow = new EmployeePayrollProcessWindow();
                payrollProcessWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(508))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                payrollPaymentWindow = new PayrollPaymentWindow();
                payrollPaymentWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Company_Variables_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(509))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                empCompanyVariablesWindow = new CompanyVariablsWindow();
                empCompanyVariablesWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Variables_Rules_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(510))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                empCompanyVaribaleRuleDetailWindow = new CompanyVariableCompanyRuleDetailWindow();
                empCompanyVaribaleRuleDetailWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_period_Quantity_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(511))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                periodQtyWindow = new EmployeePeriodQuantityWindow();
                periodQtyWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_Data_Migration_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(512))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                dataMigrateWindow = new DataMigrationWindow();
                dataMigrateWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void extra_allowance_data_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(513))
            {
                //Hr_Checkbox_two.IsChecked = false;
                //this.closeWindows();
                //empAllowanceWindow = new EmployeeAllowanceWindow();
                //empAllowanceWindow.Show(); 
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                OtherPaymentsCategoriesW = new OtherPaymentsCategoriesWindow();
                OtherPaymentsCategoriesW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Extra_allowance_process_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(514))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                Sinhalapayrule = new sinhala_payslip();
                Sinhalapayrule.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void AssigningBasicSalary_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(515))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                BasicWindow = new Assigning_Basic_Salary_Window();
                BasicWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void ManualPayRules_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(516))
            {
                //this.closeWindows();
                //Hr_Checkbox_two.IsChecked = false;
                //EmployeeRuleDetailsW = new EmployeePayRuleDetailsWindow();
                //EmployeeRuleDetailsW.Show();
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                RuleWiseAssignEmployeesW = new RuleWiseAssignEmployeesWindow();
                RuleWiseAssignEmployeesW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void BonusPayPeriod_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(517))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                BonusPeriodW = new BonusPeriodWindow();
                BonusPeriodW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void AssignBonusForEmployees_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(518))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                AssignEmployeeForBonusW = new AsignEmployeeForBonusWindow();
                AssignEmployeeForBonusW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void BonusPayProcess_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(519))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                PayBonusForAssignedEmployeesW = new PayBonusForAssignedEmployeesWindow();
                PayBonusForAssignedEmployeesW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void GenerateExcel_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(520))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                ExcelGenerateForRulesW = new ExcelGenerateForRulesWindow();
                ExcelGenerateForRulesW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void UploadExcel_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(521))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                ExcelUploadForRulesW = new ExcelUploadForRulesWindow();
                ExcelUploadForRulesW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void EmployeeThirdPartyPayments_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(522))
            {
                this.closeWindows();
                Hr_Checkbox_two.IsChecked = false;
                EmployeeThirdPartyPaymentsW = new EmployeeThirdPartyPaymentsWindow();
                EmployeeThirdPartyPaymentsW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        //private void Extra_allowance_process_Checked(object sender, RoutedEventArgs e)
        //{
        //    Hr_Checkbox_two.IsChecked = false;
        //    this.closeWindows();
        //    Sinhalapayrule = new sinhala_payslip();
        //    Sinhalapayrule.Show();
        //}
        
    }
}
