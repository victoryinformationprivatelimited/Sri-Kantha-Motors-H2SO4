using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using CustomBusyBox;

namespace ERP.Payroll.Excel_Sheet_Windows
{
    class ExcelGenerateForRulesViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        string DirectoryPath = "";
        decimal tot = 0;
        #endregion

        #region Properties
        private IEnumerable<z_Period> _Periods;

        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set 
            { 
                _CurrentPeriod = value; 
                OnPropertyChanged("CurrentPeriod");
                if (CurrentPeriod != null && CurrentRule.rule_id == new Guid("BF00DA1D-9E80-4E66-A0B4-82E6437A67D4")) 
                { 
                    RefreshExecutivePaymentDetails();
                    RefreshExecutives(); 
                }
                else if (CurrentPeriod != null && CurrentRule.rule_id == new Guid("00000000-0000-0000-0000-000000000001"))
                {
                    RefreshEmployeeOT();
                    RefreshEmployees();
                    RefreshEmployeeRates();
                }
            }
        }

        private IEnumerable<mas_CompanyRule> _Rules;

        public IEnumerable<mas_CompanyRule> Rules
        {
            get { return _Rules; }
            set { _Rules = value; OnPropertyChanged("Rules"); }
        }

        private mas_CompanyRule _CurrentRule;

        public mas_CompanyRule CurrentRule
        {
            get { return _CurrentRule; }
            set { _CurrentRule = value; OnPropertyChanged("CurrentRule"); }
        }

        private IEnumerable<EmployeeSumarryView> _ExecutiveEmployees;

        public IEnumerable<EmployeeSumarryView> ExecutiveEmployees
        {
            get { return _ExecutiveEmployees; }
            set { _ExecutiveEmployees = value; OnPropertyChanged("ExecutiveEmployees"); }
        }
        

        private IEnumerable<LeiuLeavePaymentViewForExcel> _PaymentDetails;

        public IEnumerable<LeiuLeavePaymentViewForExcel> PaymentDetails
        {
            get { return _PaymentDetails; }
            set { _PaymentDetails = value; OnPropertyChanged("PaymentDetails"); }
        }

        private IEnumerable<trns_EmployeePeriodQunatity> _EmployeeOT;

        public IEnumerable<trns_EmployeePeriodQunatity> EmployeeOT
        {
            get { return _EmployeeOT; }
            set { _EmployeeOT = value; OnPropertyChanged("EmployeeOT"); }
        }

        private IEnumerable<EmployeeSumarryView> _Employees;

        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<dtl_EmployeeRule> _EmployeeRates;

        public IEnumerable<dtl_EmployeeRule> EmployeeRates
        {
            get { return _EmployeeRates; }
            set { _EmployeeRates = value; OnPropertyChanged("EmployeeRates"); }
        }
        
        #endregion

        #region Constructor
        public ExcelGenerateForRulesViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshPeriods();
            RefreshRules();
        }
        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                    {
                        Periods = e.Result.OrderBy(c => c.start_date);
                    };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Refresh Failed");
            }
        }

        private void RefreshRules()
        {
            try
            {
                serviceClient.GetCompanyCompleted += (s, e) =>
                    {
                        Rules = e.Result.Where(c => c.status == "F");
                    };
                serviceClient.GetCompanyAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Rules Refresh Failed");
            }
        }

        private void RefreshExecutivePaymentDetails()
        {
            try
            {
                PaymentDetails = serviceClient.GetEmployeeLeiuLeaveDetails(CurrentPeriod.period_id).OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Weekend Payments Refresh Failed");
            }
        }

        private void RefreshExecutives()
        {
            try
            {
                ExecutiveEmployees = serviceClient.GetAllEmployeeDetail().Where(c => c.isActive == true && c.isExecutive == true).OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Executives Refresh Failed");
            }
        }

        private void RefreshEmployeeOT()
        {
            try
            {
                EmployeeOT = serviceClient.GetAllTrnsPeriodQuantityForExcel(CurrentPeriod.period_id, CurrentRule.rule_id).Where(c => c.quantity > 0);
            }
            catch (Exception)
            {
                clsMessages.setMessage("OT Data Refresh Failed");
            }
        }

        private void RefreshEmployees()
        {
            try
            {
                Employees = serviceClient.GetAllEmployeeDetail().Where(c => c.isActive == true).OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employees Refresh Failed");
            }
        }

        private void RefreshEmployeeRates()
        {
            try
            {
                EmployeeRates = serviceClient.GetFilteredEmployeeRules();
            }
            catch (Exception)
            {
                clsMessages.setMessage("OT Rates Refresh Failed");
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand GenerateButton
        {
            get { return new RelayCommand(Generate); }
        }

        private void Generate()
        {
            try
            {
                if (clsSecurity.GetSavePermission(520) && clsSecurity.GetUpdatePermission(520) && clsSecurity.GetDeletePermission(520))
                {
                    BusyBox.ShowBusy("Please Wait Until Generate Completed...");
                    if (CurrentRule.rule_id == new Guid("BF00DA1D-9E80-4E66-A0B4-82E6437A67D4"))
                    {
                        if (PaymentDetails.Count() > 0 && ExecutiveEmployees.Count() > 0)
                        {
                            DirectoryPath = @"C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentRule.rule_name + "\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            ws.Range["A1"].Value = "EmpNo";
                            ws.Range["B1"].Value = CurrentRule.rule_name;
                            ws.Range["C1"].Value = "Quantity";

                            int j = 2;
                            foreach (var employee in ExecutiveEmployees)
                            {
                                LeiuLeavePaymentViewForExcel ExistExecutive = PaymentDetails.FirstOrDefault(c => c.employee_id == employee.employee_id);
                                if (ExistExecutive != null)
                                {
                                    ws.Range["A" + j].Value = employee.emp_id;
                                    foreach (var payment in PaymentDetails.Where(c => c.employee_id == employee.employee_id))
                                    {
                                        tot = Convert.ToDecimal(tot + payment.amount);
                                    }
                                    ws.Range["B" + j].Value = tot;
                                    ws.Range["C" + j].Value = "1";
                                    tot = 0;
                                    ExistExecutive = null;
                                    j++;
                                }
                            }
                            wb.SaveAs("C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentRule.rule_name + "\\RuleDeduction.xlsx");
                            Marshal.ReleaseComObject(app);
                            New();
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Rule Deduction Excel Generated Successfully");
                        }
                        else
                        {
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("No Data Found For The Selected Period");
                        }
                    }
                    else if (CurrentRule.rule_id == new Guid("00000000-0000-0000-0000-000000000001"))
                    {
                        if (EmployeeOT.Count() > 0 && Employees.Count() > 0)
                        {
                            DirectoryPath = @"C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentRule.rule_name + "\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            ws.Range["A1"].Value = "EmpNo";
                            ws.Range["B1"].Value = CurrentRule.rule_name;
                            ws.Range["C1"].Value = "Quantity";

                            int j = 2;
                            foreach (var employee in Employees)
                            {
                                trns_EmployeePeriodQunatity ExistEmployee = EmployeeOT.FirstOrDefault(c => c.employee_id == employee.employee_id);
                                if (ExistEmployee != null)
                                {
                                    ws.Range["A" + j].Value = employee.emp_id;
                                    ws.Range["B" + j].Value = Convert.ToDecimal(EmployeeRates.FirstOrDefault(c => c.employee_id == ExistEmployee.employee_id).special_amount).ToString("F");
                                    ws.Range["C" + j].Value = Convert.ToDecimal(ExistEmployee.quantity).ToString("F");
                                    tot = 0;
                                    ExistEmployee = null;
                                    j++;
                                }
                            }
                            wb.SaveAs("C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentRule.rule_name + "\\RuleDeduction.xlsx");
                            Marshal.ReleaseComObject(app);
                            New();
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Rule Deduction Excel Generated Successfully");
                        }
                        else
                        {
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("No Data Found For The Selected Period");
                        }
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("An Excel Cannot Be Generated To The Selected Rule");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to perform any action in this form");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void CreatSubFolder()
        {
            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        private void New()
        {
            BusyBox.ShowBusy("Please Wait...");
            Periods = null;
            RefreshPeriods();
            CurrentPeriod = new z_Period();
            Rules = null;
            RefreshRules();
            CurrentRule = new mas_CompanyRule();
            BusyBox.CloseBusy();
        }
        #endregion
    }
}
