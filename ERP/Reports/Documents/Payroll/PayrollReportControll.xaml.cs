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
using ERP.ERPService;
using ERP.Reports.Documents.Summery_Reports;
using ERP.Reports.Documents.Advance_Report;
using ERP.Reports.Documents.PayrollReport.PayrollReportUserControl;
using ERP.Payroll.SlipTransfer;
using ERP.Payroll.Employee_Bonus;
using ERP.Reports.Documents.PayrollReport.PayrollReportData;
using ERP.Reports.Documents.PayrollReport.PayrollReportData.NewUserControl;
using ERP.Reports.Documents.Bonus_Report;
using ERP.Reports.Documents.BonusReport;

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for PayrollReportControll.xaml
    /// </summary>
    public partial class PayrollReportControll : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        SlipTransferWindow window;
        BonusSlipTransferWindow windowBST;
        EmployeeOtherPaymentWindow EmployeeOtherPaymentW;
        EmployeeThirdPartyPaymentReportWindow EmployeeThirdPartyPaymentReportW;
        public PayrollReportControll()
        {
            InitializeComponent();
        }

        #region Forms

        SalaryCoinAnalysis SalaryCoinAnalysisWindow;
        EPFETFGenerateExcelWindow EPFETFGenerateExcelW;
        #endregion

        #region Details Report Button
        public void AddDetailsReportButton(string containt, string name)
        {
            Button type_btn_advance = new Button();
            type_btn_advance.Width = 122;
            type_btn_advance.Height = 25;
            //btn.Padding = new Thickness(5, 5, 5, 5);
            type_btn_advance.Margin = new Thickness(15, 10, 0, 0);
            type_btn_advance.Name = name.Replace(" ", "");
            type_btn_advance.Content = containt;
            advancerptwrapanel.Children.Add(type_btn_advance);
            type_btn_advance.Click += new RoutedEventHandler(SubDetailsReportType_Click);
        }

        private void SubDetailsReportType_Click(object sender, RoutedEventArgs e)
        {
            Button btnc = (Button)sender;
            switch (btnc.Name)
            {

                case "PaySheet":
                    Documents.Payroll.PaySheetFilterUserControll paySheetFilter = new Documents.Payroll.PaySheetFilterUserControll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(paySheetFilter);
                    break;

                case "SalaryReport":
                    ERP.Reports.Documents.Payroll.SalarySheetFilterUserControll salarySheetFilter = new ERP.Reports.Documents.Payroll.SalarySheetFilterUserControll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(salarySheetFilter);
                    break;

                case "ETFReport":
                    ERP.Reports.Documents.Payroll.ETPFilterControll etffilter = new ETPFilterControll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(etffilter);

                    break;

                case "EPFContribution":
                    ERP.Reports.Documents.Summery_Reports.EPFFilterControll epffilter = new Summery_Reports.EPFFilterControll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(epffilter);
                    break;

                case "PayrollSummary":
                    ERP.Reports.Documents.Summery_Reports.PayrollSummaryFiltercontroll psummary = new Summery_Reports.PayrollSummaryFiltercontroll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(psummary);
                    break;

                case "PaySheetSummery":
                    ERP.Reports.Documents.Payroll.PaySheetSummeryFilter paysheetsumarry = new PaySheetSummeryFilter();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(paysheetsumarry);
                    break;

                case "MonthlyPaySummery":
                    ERP.Reports.Documents.Summery_Reports.MonthlyPaySummery mounthlypaysumarry = new Summery_Reports.MonthlyPaySummery();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(mounthlypaysumarry);
                    break;

                case "BankTransferReport":
                    ReportViewer rep1 = new ReportViewer();
                    rep1.ReportLoad("Detail Report", "BankTransferReport", "", "");
                    rep1.Show();
                    break;

            }
        }
        #endregion

        #region Refresh Details Report Date
        public void refreshReportDetails(Guid catidforservice, Guid moduleidforservice)
        {
            this.serviceClient.GetReportDataForReportViewerCompleted += (s, e) =>
            {
                foreach (var subItems in e.Result.Where(c => c.isNormal == true))
                {
                    string subname = subItems.rpt_name;
                    AddDetailsReportButton(subItems.rpt_name, subname);
                }
            };
            this.serviceClient.GetReportDataForReportViewerAsync(catidforservice, moduleidforservice);
        }
        #endregion

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Report_ui_UserControlers.Report_payroll_btn payrollbtn = new Report_ui_UserControlers.Report_payroll_btn();
            //advancerptwrapanel.Children.Clear();
            //advancerptwrapanel.Children.Add(payrollbtn);




        }


        private void Pay_Sheet_check_box_Checked_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheet), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewEmployeePaySheetUserControl ps = new NewEmployeePaySheetUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ps);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Pay_Sheet_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheet), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewEmployeePaySheetUserControl ps = new NewEmployeePaySheetUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ps);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Payroll_Summary_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheetSumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewPayrollSummaryUserControl ps = new NewPayrollSummaryUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ps);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Payroll_Summary_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheetSumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewPayrollSummaryUserControl ps = new NewPayrollSummaryUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ps);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void ETF_Reports_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeFundReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewEmployeeTaxUserControl ef = new NewEmployeeTaxUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ef);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void ETF_Reports_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeFundReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewEmployeeTaxUserControl ef = new NewEmployeeTaxUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ef);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Salary_Reports_check_box__Checked(object sender, RoutedEventArgs e)
        {

            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.SalaryReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    SalarySheetFilterUserControll ss = new SalarySheetFilterUserControll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ss);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }

        }

        private void Salary_Reports_check_box__Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.SalaryReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    SalarySheetFilterUserControll ss = new SalarySheetFilterUserControll();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(ss);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void EPF_Contribution_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    //EPFFilterControll ef = new EPFFilterControll();
                    //AdvanceFilterMDI.Children.Clear();
                    //AdvanceFilterMDI.Children.Add(ef);

                    EPFFilterWindow EpF = new EPFFilterWindow();
                    EpF.Show();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void EPF_Contribution_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    EPFFilterWindow EpF = new EPFFilterWindow();
                    EpF.Show();
                    //EPFFilterControll ef = new EPFFilterControll();
                    //AdvanceFilterMDI.Children.Clear();
                    //AdvanceFilterMDI.Children.Add(ef);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Monthly_Pay_Summary_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.MonthlyPaySumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    MonthlyPaySummery mps = new MonthlyPaySummery();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(mps);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Monthly_Pay_Summary_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.MonthlyPaySumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    MonthlyPaySummery mps = new MonthlyPaySummery();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(mps);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Basic_Tranfer_Report_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.BasicTransferReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    ReportViewer rv = new ReportViewer();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(rv);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Basic_Tranfer_Report_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.BasicTransferReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    ReportViewer rv = new ReportViewer();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(rv);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Pay_Sheet_Summary_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheetSumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    PaySheetSummeryFilter pss = new PaySheetSummeryFilter();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(pss);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Pay_Sheet_Summary_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheetSumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    PaySheetSummeryFilter pss = new PaySheetSummeryFilter();
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(pss);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Deduction_Report_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DeductionReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewDeductionUserControl duc = new NewDeductionUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(duc);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }

        }

        private void Deduction_Report_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DeductionReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewDeductionUserControl duc = new NewDeductionUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(duc);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Benfits_Report_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.BenifitReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewBenifitUserControl buc = new NewBenifitUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(buc);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Benfits_Report_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.BenifitReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    NewBenifitUserControl buc = new NewBenifitUserControl(AdvanceFilterMDI);
                    AdvanceFilterMDI.Children.Clear();
                    AdvanceFilterMDI.Children.Add(buc);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Total_payroll_Amount_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.TotalPayrollSumarry), clsSecurity.loggedUser.user_id))
            {
                TotalPayrollSumarryControl payrollSumarry = new TotalPayrollSumarryControl();
                AdvanceFilterMDI.Children.Clear();
                AdvanceFilterMDI.Children.Add(payrollSumarry);
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void horizontal_payroll_sumarry_check_box_Checked(object sender, RoutedEventArgs e)
        {
            //TotalPayrollSumarryControl payrollSumarry = new TotalPayrollSumarryControl();
            //AdvanceFilterMDI.Children.Clear();
            //AdvanceFilterMDI.Children.Add(payrollSumarry);
        }

        private void horizontal_payroll_sumarry_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            //HorisantalPayrollSumarry payrollSumarry = new HorisantalPayrollSumarry();
            //AdvanceFilterMDI.Children.Clear();
            //AdvanceFilterMDI.Children.Add(payrollSumarry);
        }

        private void horizantal_payroll_sumarry_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.HorizontalPayrollSumarry), clsSecurity.loggedUser.user_id))
            {
                HorisantalPayrollSumarry payrollSumarry = new HorisantalPayrollSumarry(AdvanceFilterMDI);
                AdvanceFilterMDI.Children.Clear();
                AdvanceFilterMDI.Children.Add(payrollSumarry);
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void horizantal_payroll_sumarry_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.HorizontalPayrollSumarry), clsSecurity.loggedUser.user_id))
            {
                HorisantalPayrollSumarry payrollSumarry = new HorisantalPayrollSumarry(AdvanceFilterMDI);
                AdvanceFilterMDI.Children.Clear();
                AdvanceFilterMDI.Children.Add(payrollSumarry);
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void salary_reconcilition_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.SalaryReconcilationReport), clsSecurity.loggedUser.user_id))
            {
                NewSalaryReconciliationUserControl payrollSumarry = new NewSalaryReconciliationUserControl(AdvanceFilterMDI);
                AdvanceFilterMDI.Children.Clear();
                AdvanceFilterMDI.Children.Add(payrollSumarry);
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void salary_reconcilition_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.SalaryReconcilationReport), clsSecurity.loggedUser.user_id))
            {
                NewSalaryReconciliationUserControl payrollSumarry = new NewSalaryReconciliationUserControl(AdvanceFilterMDI);
                AdvanceFilterMDI.Children.Clear();
                AdvanceFilterMDI.Children.Add(payrollSumarry);
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        void FormClose()
        {
            if (SalaryCoinAnalysisWindow != null)
                SalaryCoinAnalysisWindow.Close();

            if (EPFETFGenerateExcelW != null)
                EPFETFGenerateExcelW.Close();
        }

        private void salary_reconcilition_check_box_Copy_Unchecked_1(object sender, RoutedEventArgs e)
        {
            FormClose();
            SalaryCoinAnalysisWindow = new SalaryCoinAnalysis();
            SalaryCoinAnalysisWindow.Show();
        }

        private void salary_reconcilition_check_box_Copy_Checked_1(object sender, RoutedEventArgs e)
        {
            FormClose();
            SalaryCoinAnalysisWindow = new SalaryCoinAnalysis();
            SalaryCoinAnalysisWindow.Show();
        }

        private void salary_reconcilition_check_box_Copy1_Unchecked_1(object sender, RoutedEventArgs e)
        {
            FormClose();
            EPFETFGenerateExcelW = new EPFETFGenerateExcelWindow();
            EPFETFGenerateExcelW.Show();
        }

        private void salary_reconcilition_check_box_Copy1_Checked_1(object sender, RoutedEventArgs e)
        {
            FormClose();
            EPFETFGenerateExcelW = new EPFETFGenerateExcelWindow();
            EPFETFGenerateExcelW.Show();

        }

        private void unilak_payment_detail_check_box_Copy_Checked(object sender, RoutedEventArgs e)
        {
            PaymentComparisonUserControl payment = new PaymentComparisonUserControl();
            AdvanceFilterMDI.Children.Clear();
            AdvanceFilterMDI.Children.Add(payment);


        }

        private void unilak_payment_detail_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            PaymentComparisonUserControl payment = new PaymentComparisonUserControl();
            AdvanceFilterMDI.Children.Clear();
            AdvanceFilterMDI.Children.Add(payment);
        }

        private void unilak_payroll_check_box_Copy_Checked(object sender, RoutedEventArgs e)
        {
            UnilakPayrollUserControl payrollSumarry = new UnilakPayrollUserControl();
            AdvanceFilterMDI.Children.Clear();
            AdvanceFilterMDI.Children.Add(payrollSumarry);
        }

        private void unilak_payroll_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            UnilakPayrollUserControl payrollSumarry = new UnilakPayrollUserControl();
            AdvanceFilterMDI.Children.Clear();
            AdvanceFilterMDI.Children.Add(payrollSumarry);
        }

        private void new_slip_check_box_Checked(object sender, RoutedEventArgs e)
        {
            NewPaySlipUserControl pay_slip = new NewPaySlipUserControl(AdvanceFilterMDI);
            AdvanceFilterMDI.Children.Clear();
            AdvanceFilterMDI.Children.Add(pay_slip);
        }

        private void new_payslip_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            NewPaySlipUserControl pay_slip = new NewPaySlipUserControl(AdvanceFilterMDI);
            AdvanceFilterMDI.Children.Clear();
            AdvanceFilterMDI.Children.Add(pay_slip);
        }

        private void epf_eretunn_check_box_Checked(object sender, RoutedEventArgs e)
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
            if (EmployeeOtherPaymentW != null)
                EmployeeOtherPaymentW.Close();
            if (EmployeeThirdPartyPaymentReportW != null)
                EmployeeThirdPartyPaymentReportW.Close();
        }

        private void epf_ereturn_check_box_Copy_Checked_1(object sender, RoutedEventArgs e)
        {
            formClose();
            ETFEreturnWindow etf_window = new ETFEreturnWindow();
            etf_window.Show();
        }

        private void bonus_payslip_checked(object sender, RoutedEventArgs e)
        {
            formClose();
            // Hr_Checkbox_two.IsChecked = false;
            windowBST = new BonusSlipTransferWindow();
            windowBST.Show();
        }

        private void bonus_report1_checked(object sender, RoutedEventArgs e)
        {
            BonusWithPayeeViewModel ViewModel = new BonusWithPayeeViewModel();
            BonusPayeeWindow reportWindow = new BonusPayeeWindow(ViewModel);
            reportWindow.Show();
        }

        private void bonus_report2_checked(object sender, RoutedEventArgs e)
        {
            BonusSlipViewModel ViewModel = new BonusSlipViewModel();
            BonusPayeeWindow reportWindow = new BonusPayeeWindow(ViewModel);
            reportWindow.Show();
        }

        private void OtherPaymentReportNSlip_Checked(object sender, RoutedEventArgs e)
        {
            formClose();
            // Hr_Checkbox_two.IsChecked = false;
            EmployeeOtherPaymentW = new EmployeeOtherPaymentWindow();
            EmployeeOtherPaymentW.Show();
        }

        private void ThirdPartyPaymentsReportNSlip_Checked(object sender, RoutedEventArgs e)
        {
            formClose();
            // Hr_Checkbox_two.IsChecked = false;
            EmployeeThirdPartyPaymentReportW = new EmployeeThirdPartyPaymentReportWindow();
            EmployeeThirdPartyPaymentReportW.Show();
        }
        //private void epf_ereturn_Copy_Click_1(object sender, RoutedEventArgs e)
        //{
        //    string path = "\\Reports\\Documents\\Payroll\\R4";
        //    ReportPrint print = new ReportPrint(path);
        //    print.LoadToReportViewer();
        //}

        //private void epf_ereturn_Copy1_Click_1(object sender, RoutedEventArgs e)
        //{
        //    string path = "\\Reports\\Documents\\Payroll\\R1";
        //    ReportPrint print = new ReportPrint(path);
        //    print.LoadToReportViewer();

        //}

        //private void epf_ereturn_Copy_Checked_1(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
        //    {
        //        try
        //        {
        //            EPFFilterControll ef = new EPFFilterControll();
        //            AdvanceFilterMDI.Children.Clear();
        //            AdvanceFilterMDI.Children.Add(ef);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        clsMessages.setMessage(Properties.Resources.NoPermissionForView);
        //    }

        //}

        //private void epf_ereturn_Copy_Unchecked_1(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
        //    {
        //        try
        //        {
        //            EPFFilterControll ef = new EPFFilterControll();
        //            AdvanceFilterMDI.Children.Clear();
        //            AdvanceFilterMDI.Children.Add(ef);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        clsMessages.setMessage(Properties.Resources.NoPermissionForView);
        //    }

        //}

        //private void epf_ereturn_Copy1_Checked_1(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
        //    {
        //        try
        //        {
        //            EPFFilterControll ef = new EPFFilterControll();
        //            AdvanceFilterMDI.Children.Clear();
        //            AdvanceFilterMDI.Children.Add(ef);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        clsMessages.setMessage(Properties.Resources.NoPermissionForView);
        //    }
        //}

        //private void epf_ereturn_Copy1_Unchecked_1(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
        //    {
        //        try
        //        {
        //            EPFFilterControll ef = new EPFFilterControll();
        //            AdvanceFilterMDI.Children.Clear();
        //            AdvanceFilterMDI.Children.Add(ef);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        clsMessages.setMessage(Properties.Resources.NoPermissionForView);
        //    }
        //}


    }
}
