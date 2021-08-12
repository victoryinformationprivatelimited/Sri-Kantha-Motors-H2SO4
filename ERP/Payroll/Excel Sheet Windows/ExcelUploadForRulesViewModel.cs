using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using System.Data;
using System.IO;
using CustomBusyBox;


namespace ERP.Payroll.Excel_Sheet_Windows
{
    public class ExcelUploadForRulesViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient;
        List<dtl_EmployeeOtherRulesData> QTY;
        List<trns_EmployeeOtherRulesData> ProcessedList;
        List<mas_Employee> employee = new List<mas_Employee>();
        List<string> Employee_no = new List<string>();
        decimal Tot = 0;
        decimal gross = 0;
        decimal payeeTot = 0;
        decimal netValue = 0;
        trns_EmployeeOtherRulesData saveData;
        z_CompanyVariable payeeID;

        #region Constructor
        public ExcelUploadForRulesViewModel()
        {
            serviceClient = new ERPServiceClient();
            QTY = new List<dtl_EmployeeOtherRulesData>();
            refreshPeriods();
            refreshCompanyRules();
            refreshEmployees();
            refreshSlaps();
            IsEffectToPayee = true;
        }
        #endregion

        private NotifyingProperty cursor = new NotifyingProperty("Cursor", typeof(System.Windows.Input.Cursor), System.Windows.Input.Cursors.Arrow);
        public System.Windows.Input.Cursor Cursor
        {
            get { return (System.Windows.Input.Cursor)GetValue(cursor); }
            set { SetValue(cursor, value); }
        }


        #region Properties
        private IEnumerable<dtl_EmployeeOtherRulesData> _UploadedExcelData;

        public IEnumerable<dtl_EmployeeOtherRulesData> UploadedExcelData
        {
            get { return _UploadedExcelData; }
            set { _UploadedExcelData = value; }
        }


        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employee"); }
        }

        private Emp_Sp_Employee _CurrentEmployee;
        public Emp_Sp_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<mas_CompanyRule> _CompanyRules;
        public IEnumerable<mas_CompanyRule> CompanyRules
        {
            get { return _CompanyRules; }
            set { _CompanyRules = value; OnPropertyChanged("CompanyRules"); }
        }

        private mas_CompanyRule _CurrentCompanyRule;
        public mas_CompanyRule CurrentCompanyRule
        {
            get { return _CurrentCompanyRule; }
            set { _CurrentCompanyRule = value; OnPropertyChanged("CurrentCompanyRule"); if (CurrentPeriod != null && CurrentCompanyRule != null) refreshUploadedData(); }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
        }

        private bool _IsEffectToPayee;

        public bool IsEffectToPayee
        {
            get { return _IsEffectToPayee; }
            set { _IsEffectToPayee = value; OnPropertyChanged("IsEffectToPayee"); }
        }


        private bool _IsAllEmployees;
        public bool IsAllEmployees
        {
            get { return _IsAllEmployees; }
            set { _IsAllEmployees = value; OnPropertyChanged("IsAllEmployees"); }
        }

        private IEnumerable<trns_EmployeePayment> _GrossSalaries;

        public IEnumerable<trns_EmployeePayment> GrossSalaries
        {
            get { return _GrossSalaries; }
            set { _GrossSalaries = value; OnPropertyChanged("GrossSalaries"); }
        }

        private IEnumerable<dtl_EmployeeOtherRulesData> _UploadedExcelDataForProcess;

        public IEnumerable<dtl_EmployeeOtherRulesData> UploadedExcelDataForProcess
        {
            get { return _UploadedExcelDataForProcess; }
            set { _UploadedExcelDataForProcess = value; OnPropertyChanged("UploadedExcelDataForProcess"); }
        }

        private IEnumerable<trns_EmployeeOtherRulesData> _ProcessedData;

        public IEnumerable<trns_EmployeeOtherRulesData> ProcessedData
        {
            get { return _ProcessedData; }
            set { _ProcessedData = value; OnPropertyChanged("ProcessedData"); }
        }

        private IEnumerable<z_Slap> _Slaps;
        public IEnumerable<z_Slap> Slaps
        {
            get { return _Slaps; }
            set { _Slaps = value; OnPropertyChanged("Slaps"); }
        }

        public ICommand BtnBrowse
        {
            get { return new RelayCommand(btnbrowse, btnBrowseCanExecute); }
        }

        public ICommand BtnStart
        {
            get { return new RelayCommand(Start, StartCanExecute); }
        }

        public ICommand BtnProcess
        {
            get { return new RelayCommand(Process); }
        }

        private bool StartCanExecute()
        {
            return true;
        }

        private void Start()
        {
            addValues(CurrentCompanyRule, CurrentPeriod);
        }

        private bool btnBrowseCanExecute()
        {
            return true;
        }

        private void btnbrowse()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".csv";
            // fd.Filter = "Excel files (*.xls)|*.xls";

            Nullable<bool> result = fd.ShowDialog();

            if (result == true)
            {
                Path = fd.FileName;
            }

        }

        #endregion

        #region Refresh Methods
        private void refreshEmployees()
        {
            try
            {
                //this.serviceClient.GetMasEmployeeFromViewCompleted += (s, e) =>
                //    {
                Employees = serviceClient.GetEmployees();
                foreach (var item_emp in Employees)
                {
                    employee.Add(item_emp);
                }
                //    };
                //this.serviceClient.GetEmployeesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employees Refresh Failed");
            }
        }

        private void refreshPeriods()
        {
            try
            {
                this.serviceClient.GetPeriodsCompleted += (s, e) =>
                    {
                        Periods = e.Result;
                    };
                this.serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Refresh Failed");
            }
        }

        private void refreshCompanyRules()
        {
            try
            {
                this.serviceClient.GetCompanyRulesCompleted += (s, e) =>
                    {
                        this.CompanyRules = e.Result;
                        CurrentCompanyRule = CompanyRules.FirstOrDefault(z => z.rule_id == Guid.Parse("89a95b5b-ae9d-4ec8-b502-e59d99602895"));

                    };

                this.serviceClient.GetCompanyRulesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Rules Refresh Failed");
            }
        }

        private void refreshUploadedData()
        {
            try
            {
                this.UploadedExcelData = serviceClient.GetEmployeeOtherRulesData(CurrentPeriod.period_id, CurrentCompanyRule.rule_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Uploaded Data Refresh Failed");
            }
        }

        private void refreshUploadedDataForProcess()
        {
            try
            {
                this.UploadedExcelDataForProcess = serviceClient.GetEmployeeOtherRulesDataForProcess(CurrentPeriod.period_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Uploaded Data Refresh Failed");
            }
        }

        private void refreshGrossSalaries()
        {
            try
            {
                this.GrossSalaries = serviceClient.GetEmployeePaymentByPeriod(CurrentPeriod.period_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Gross Salaries Refresh Failed");
            }
        }

        private void refreshProcessedData()
        {
            try
            {
                this.ProcessedData = serviceClient.GetProcessedEmployeeOtherRulesData(CurrentPeriod.period_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Processed Data Refresh Failed");
            }
        }

        private void refreshSlaps()
        {
            try
            {
                this.serviceClient.GetSalpsCompleted += (s, e) =>
                    {
                        this.Slaps = e.Result;
                    };
                this.serviceClient.GetSalpsAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Slaps Refresh Failed");
            }
        }
        #endregion

        private void addValues(mas_CompanyRule rule, z_Period period)
        {
            if (clsSecurity.GetSavePermission(521) && clsSecurity.GetUpdatePermission(521) && clsSecurity.GetDeletePermission(521))
            {
                BusyBox.ShowBusy("Please Wait Until Upload Completed...");
                Cursor = System.Windows.Input.Cursors.Wait;

                if (rule != null && period != null)
                {
                    dtl_EmployeeOtherRulesData existData = UploadedExcelData.FirstOrDefault(c => c.periodID == CurrentPeriod.period_id && c.ruleID == CurrentCompanyRule.rule_id);
                    if (existData != null)
                        serviceClient.DeleteUploadedData(CurrentPeriod.period_id, CurrentCompanyRule.rule_id);
                    try
                    {
                        DataTable dt = ReadExcelFile();

                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                string emp_id = dr["EmpNo"].ToString();
                                string excel_quntity_str = dr["Quantity"].ToString();
                                //string excel_ststus_str = dr["isQuntity"].ToString();
                                decimal excel_quntity = decimal.Parse(dr["Quantity"].ToString());
                                //int excel_ststus = int.Parse(dr["isQuntity"].ToString());

                                Guid employee_id = employee.FirstOrDefault(z => z.emp_id.TrimStart('0') == emp_id.TrimStart('0') && z.isdelete == false).employee_id;
                                if (employee_id != null && employee_id != Guid.Empty)
                                {
                                    string specal = CurrentCompanyRule.rule_name;
                                    string quntity_tt = dr[specal].ToString();
                                    dtl_EmployeeOtherRulesData new_rule = new dtl_EmployeeOtherRulesData();
                                    new_rule.employeeID = employee_id;
                                    new_rule.ruleID = CurrentCompanyRule.rule_id;
                                    new_rule.ruleName = CurrentCompanyRule.rule_name;
                                    new_rule.periodID = CurrentPeriod.period_id;
                                    new_rule.amount = decimal.Parse(quntity_tt);
                                    new_rule.isEffectToPayee = IsEffectToPayee;
                                    new_rule.save_datetime = System.DateTime.Now.Date;
                                    new_rule.save_user_id = clsSecurity.loggedUser.user_id;
                                    QTY.Add(new_rule);
                                }
                                else
                                {
                                    Employee_no.Add(emp_id);
                                }
                            }
                            catch (Exception)
                            {
                            }

                        }
                        //if (QTY.Count > 0 && UpdateRule.Count>0)
                        //{
                        if (serviceClient.saveUploadedData(QTY.ToArray()))
                        {
                            Cursor = System.Windows.Input.Cursors.Arrow;
                            if (Employee_no.Count > 0)
                            {
                                string ErrorMessage = "";
                                BusyBox.CloseBusy();
                                MessageBoxResult exresult = MessageBox.Show("Upload With  Errors..  Do you need to Continue ?", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (exresult == MessageBoxResult.Yes)
                                {
                                    foreach (var item in Employee_no)
                                    {
                                        ErrorMessage += item + "\n";
                                    }

                                    MessageBox.Show("Error employee numbers are  " + "\n" + ErrorMessage);

                                }
                            }
                            else
                            {
                                BusyBox.CloseBusy();
                                MessageBox.Show(CurrentCompanyRule.rule_name.ToString() + " " + " Migration Process Successfully " + QTY.Count + "Item Saved");
                                New();
                            }
                        }
                        else
                        {
                            BusyBox.CloseBusy();
                            MessageBox.Show("Attendance Migration Process Fail ");
                        }
                        //}

                    }
                    catch (Exception ex)
                    {
                        BusyBox.CloseBusy();
                        Cursor = System.Windows.Input.Cursors.Arrow;
                        MessageBox.Show(ex.Message);
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to perform any acton in this form");
        }
        public int getTotalMinits(string totaltime)
        {
            int minit = 0;
            try
            {

                string[] words = totaltime.ToString().Split(':');
                string hours = words[0].ToString();
                string minits = words[1];
                int inthours = int.Parse(hours.ToString());
                int intminits = int.Parse(minits.ToString());
                minit = (inthours * 60 + intminits);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message);
            }
            return minit;
        }

        private DataTable ReadExcelFile()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties=Excel 12.0;";
            string query = @"Select * From [Sheet1$]";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connStr);

            conn.Open();
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(query, conn);
            System.Data.OleDb.OleDbDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            conn.Close();

            return dt;

        }

        private void New()
        {
            BusyBox.ShowBusy("Please Wait...");
            QTY = null;
            QTY = new List<dtl_EmployeeOtherRulesData>();
            Periods = null;
            CurrentPeriod = new z_Period();
            refreshPeriods();
            CompanyRules = null;
            CurrentCompanyRule = new mas_CompanyRule();
            refreshCompanyRules();
            Employees = null;
            CurrentEmployee = new Emp_Sp_Employee();
            refreshEmployees();
            IsEffectToPayee = true;
            BusyBox.CloseBusy();
        }

        private void Process()
        {
            if (clsSecurity.GetSavePermission(521) && clsSecurity.GetUpdatePermission(521) && clsSecurity.GetDeletePermission(521))
            {
                try
                {
                    BusyBox.ShowBusy("Please Wait Until Process Completed...");
                    refreshUploadedDataForProcess();
                    refreshGrossSalaries();
                    refreshEmployees();
                    refreshProcessedData();
                    ProcessedList = new List<trns_EmployeeOtherRulesData>();
                    payeeID = new z_CompanyVariable();

                    trns_EmployeeOtherRulesData existdata = ProcessedData.FirstOrDefault(c => c.periodID == CurrentPeriod.period_id);
                    if (existdata != null)
                        serviceClient.DeleteProcessedData(CurrentPeriod.period_id);

                    foreach (mas_Employee currentEmp in Employees)
                    {
                        trns_EmployeePayment currentRecord = GrossSalaries.FirstOrDefault(c => c.employee_id == currentEmp.employee_id && c.period_id == CurrentPeriod.period_id);
                        if (currentRecord != null)
                        {
                            payeeID.company_variableID = new Guid("00000000-0000-0000-0000-000000000004");
                            dtl_EmployeeOtherRulesData existEmployee = UploadedExcelDataForProcess.FirstOrDefault(c => c.employeeID == currentEmp.employee_id);
                            if (existEmployee != null)
                            {
                                foreach (dtl_EmployeeOtherRulesData currentData in UploadedExcelDataForProcess.Where(c => c.employeeID == currentEmp.employee_id))
                                {
                                    Tot = Tot + Convert.ToDecimal(currentData.amount);
                                    if (currentData.isEffectToPayee == true)
                                        payeeTot = payeeTot + (decimal)currentData.amount;
                                }
                                gross = (decimal)currentRecord.grossSalary;
                                saveData = new trns_EmployeeOtherRulesData();
                                saveData.employeeID = currentEmp.employee_id;
                                saveData.totalforpayee = Math.Round(gross + payeeTot, 2);
                                if (Tot > 25000)
                                {
                                    saveData.stamp_duty = 25;
                                }
                                else
                                    saveData.stamp_duty = 0;
                                saveData.totalAmount = Math.Round(Tot, 2);
                                //saveData.payeeAount = Math.Round(SlapCalculation((decimal)saveData.totalforpayee, payeeID),2);
                                saveData.payeeAount = 0;
                                netValue = Math.Round(Tot - (decimal)saveData.payeeAount - (decimal)saveData.stamp_duty, 2);
                                if (netValue > 0)
                                    saveData.netAmount = netValue;
                                else
                                    saveData.netAmount = 0;
                                if(netValue > 0)
                                    saveData.overdueAmount = 0;
                                else
                                    saveData.overdueAmount = netValue * -1;
                                saveData.periodID = CurrentPeriod.period_id;
                                saveData.is_payee_deducted = true;
                                saveData.save_datetime = System.DateTime.Now;
                                saveData.save_user_id = clsSecurity.loggedUser.user_id;
                                ProcessedList.Add(saveData);
                            }
                            else
                            {
                                Tot = 0;
                                payeeTot = 0;
                                gross = (decimal)currentRecord.grossSalary;
                                saveData = new trns_EmployeeOtherRulesData();
                                saveData.employeeID = currentEmp.employee_id;
                                saveData.totalforpayee = Math.Round(gross + payeeTot, 2);
                                saveData.stamp_duty = 0;
                                saveData.totalAmount = Math.Round(Tot, 2);
                                //saveData.payeeAount = Math.Round(SlapCalculation((decimal)saveData.totalforpayee, payeeID),2);
                                saveData.payeeAount = 0;
                                saveData.netAmount = Math.Round(Tot - (decimal)saveData.payeeAount - (decimal)saveData.stamp_duty, 2) * -1;
                                saveData.overdueAmount = 0;
                                saveData.periodID = CurrentPeriod.period_id;
                                saveData.is_payee_deducted = false;
                                saveData.save_datetime = System.DateTime.Now;
                                saveData.save_user_id = clsSecurity.loggedUser.user_id;
                                ProcessedList.Add(saveData);
                            }
                            Tot = 0;
                            gross = 0;
                            payeeTot = 0;
                            netValue = 0;
                        }
                    }
                    if (serviceClient.saveProcessedData(ProcessedList.ToArray()))
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Process Completed");
                    }
                }
                catch (Exception)
                {
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("Error in Process");
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to perform any action in this form");
        }

        private decimal SlapCalculation(decimal calculationSalary, z_CompanyVariable CurrentCompanyVariable)
        {
            decimal SlapTotal = 0;

            foreach (z_Slap SlapItem in Slaps.Where(e => e.company_variableID.Equals(CurrentCompanyVariable.company_variableID)))
            {
                decimal Slapvalue = 0;

                if (calculationSalary >= SlapItem.slapStart && calculationSalary < SlapItem.slapEnd)
                {
                    Slapvalue = calculationSalary * (decimal)SlapItem.slapValue / 100;
                    Slapvalue = Slapvalue - (decimal)SlapItem.deductValue;
                    SlapTotal += Slapvalue;
                }
            }
            return SlapTotal;
        }
    }
}

