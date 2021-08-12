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

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for PayrollReportControll.xaml
    /// </summary>
    public partial class PayrollReportControll : UserControl
    {
        #region Service Client

        private ERPServiceClient serviceClient = new ERPServiceClient(); 

        #endregion

        #region Forms

        PaySheetFilterWindow empPaySheetReportWindow;
        PayrollSumarryWindow payrollSummaryReportWindow;
        EmployeeFundWindow empFundReportWindow;
        EmployeeBenifitWindow empBenefitsReportWindow;
        EmployeeDeductionWindow empDeductionReportWindow;
        EPFFilterWindow epfReportWindow;
        ReportViewer rv;
        TotalPayrollSumarryWindow totalPayrollSummaryReportWindow;
        HorisantalPayrollSumarryWindow horizontalPayrollSummaryReportWindow;
        SalaryReconciliationWindow salaryReconcileReportWindow;
        SalaryCoinAnalysis SalaryCoinAnalysisWindow;
        EPFeReturnWindow EPFeReturnWindow;
        NewPaySlipWindow newPaySlipReportWindow;
        ETFEreturnWindow ETFeReturnWindow;
        SlipTransferWindow window;

        #endregion

        #region Constructor

        public PayrollReportControll()
        {
            InitializeComponent();
        } 

        #endregion

        #region Windows close methods

        void formClose()
        {
            if (empPaySheetReportWindow != null)
                empPaySheetReportWindow.Close();
            if (payrollSummaryReportWindow != null)
                payrollSummaryReportWindow.Close();
            if (empFundReportWindow != null)
                empFundReportWindow.Close();
            if (empBenefitsReportWindow != null)
                empBenefitsReportWindow.Close();
            if (empDeductionReportWindow != null)
                empDeductionReportWindow.Close();
            if (epfReportWindow != null)
                epfReportWindow.Close();
            if (rv != null)
                rv.Close();
            if (totalPayrollSummaryReportWindow != null)
                totalPayrollSummaryReportWindow.Close();
            if (horizontalPayrollSummaryReportWindow != null)
                horizontalPayrollSummaryReportWindow.Close();
            if (salaryReconcileReportWindow != null)
                salaryReconcileReportWindow.Close();
            if (SalaryCoinAnalysisWindow != null)
                SalaryCoinAnalysisWindow.Close();
            if (EPFeReturnWindow != null)
                EPFeReturnWindow.Close();
            if (newPaySlipReportWindow != null)
                newPaySlipReportWindow.Close();
            if (window != null)
                window.Close();
            if (ETFeReturnWindow != null)
                ETFeReturnWindow.Close();

        }

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

        #region Payroll Reports Buttons Click Event Handlers

        private void Pay_Sheet_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheet), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    empPaySheetReportWindow = new PaySheetFilterWindow();
                    empPaySheetReportWindow.Show();
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

        private void Payroll_Summary_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PaySheetSumarry), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    payrollSummaryReportWindow = new PayrollSumarryWindow();
                    payrollSummaryReportWindow.Show();
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

        private void ETF_Reports_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeFundReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    empFundReportWindow = new EmployeeFundWindow();
                    empFundReportWindow.Show();
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

        private void Benfits_Report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.BenifitReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    empBenefitsReportWindow = new EmployeeBenifitWindow();
                    empBenefitsReportWindow.Show();
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

        private void Deduction_Report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DeductionReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    empDeductionReportWindow = new EmployeeDeductionWindow();
                    empDeductionReportWindow.Show();
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

        private void EPF_Contribution_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EPFContributionReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    epfReportWindow = new EPFFilterWindow();
                    epfReportWindow.Show();
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

        private void Basic_Tranfer_Report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.BasicTransferReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    rv = new ReportViewer();
                    rv.Show();
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

        private void Total_payroll_Amount_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.TotalPayrollSumarry), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                totalPayrollSummaryReportWindow = new TotalPayrollSumarryWindow();
                totalPayrollSummaryReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void horizantal_payroll_sumarry_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.HorizontalPayrollSumarry), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                horizontalPayrollSummaryReportWindow = new HorisantalPayrollSumarryWindow();
                horizontalPayrollSummaryReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void salary_reconcilition_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.SalaryReconcilationReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                salaryReconcileReportWindow = new SalaryReconciliationWindow();
                salaryReconcileReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void coin_analysis_btn_Click(object sender, RoutedEventArgs e)
        {
            this.formClose();
            SalaryCoinAnalysisWindow = new SalaryCoinAnalysis();
            SalaryCoinAnalysisWindow.Show();
        }

        private void epf_e_return_btn_Click(object sender, RoutedEventArgs e)
        {
            this.formClose();
            EPFeReturnWindow = new EPFeReturnWindow();
            EPFeReturnWindow.Show();
        }

        private void new_payslip_btn_Click(object sender, RoutedEventArgs e)
        {
            this.formClose();
            newPaySlipReportWindow = new NewPaySlipWindow();
            newPaySlipReportWindow.Show();
        }

        private void etf_e_return_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            ETFeReturnWindow = new ETFEreturnWindow();
            ETFeReturnWindow.Show();
        } 

        #endregion
    }
}
