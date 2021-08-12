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


namespace ERP.Utility
{
    public class DataMigrationViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient = new ERPServiceClient();
        List<trns_EmployeePeriodQunatity> QTY;
        List<mas_Employee> employee = new List<mas_Employee>();
        List<dtl_EmployeeRule> employeeRule = new List<dtl_EmployeeRule>();
        List<dtl_EmployeeRule> UpdateRule = new List<dtl_EmployeeRule>();
        List<string> Employee_no = new List<string>();

        #region Constructor
        public DataMigrationViewModel()
        {
            QTY = new List<trns_EmployeePeriodQunatity>();
            refreshPeriods();
            refreshCompanyRules();
            refreshEmployees();
            //refreshPeriodQuantiy();
            IsClear = false;
            SpecialAmount = true;
        }
        #endregion

        private bool _IsClear;
        public bool IsClear
        {
            get { return _IsClear; }
            set { _IsClear = value; this.OnPropertyChanged("IsClear"); }
        }

        private NotifyingProperty cursor = new NotifyingProperty("Cursor", typeof(System.Windows.Input.Cursor), System.Windows.Input.Cursors.Arrow);
        public System.Windows.Input.Cursor Cursor
        {
            get { return (System.Windows.Input.Cursor)GetValue(cursor); }
            set { SetValue(cursor, value); }
        }
        private bool _QuntityData;
        public bool QuntityData
        {
            get { return _QuntityData; }
            set
            {
                _QuntityData = value; this.OnPropertyChanged("QuntityData");
                if (QuntityData == true)
                {
                    SpecialAmount = false;
                }
            }


        }
        private bool _SpecialAmount;

        public bool SpecialAmount
        {
            get { return _SpecialAmount; }
            set
            {
                _SpecialAmount = value; this.OnPropertyChanged("SpecialAmount");
                if (SpecialAmount == true)
                {
                    QuntityData = false;
                }
            }

        }


        #region Propertiese
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod != null) refreshPeriodQuantiy(); }
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
            set { _CurrentCompanyRule = value; OnPropertyChanged("CurrentCompanyRule"); }
        }

        private IEnumerable<trns_EmployeePeriodQunatity> _EmployeePeriodQantity;
        public IEnumerable<trns_EmployeePeriodQunatity> EmployeePeriodQantity
        {
            get { return _EmployeePeriodQantity; }
            set { _EmployeePeriodQantity = value; OnPropertyChanged("EmployeePeriodQantity"); }
        }



        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
        }

        private bool _IsAllEmployees;
        public bool IsAllEmployees
        {
            get { return _IsAllEmployees; }
            set { _IsAllEmployees = value; OnPropertyChanged("IsAllEmployees"); }
        }

        public ICommand BtnBrowse
        {
            get { return new RelayCommand(btnbrowse, btnBrowseCanExecute); }
        }

        public ICommand BtnStart
        {
            get { return new RelayCommand(Start, StartCanExecute); }
        }

        private bool StartCanExecute()
        {
            return true;
        }

        private void Start()
        {
            if (IsClear)
            {
                if (clsSecurity.GetDeletePermission(512))
                {
                    BusyBox.ShowBusy("Please Wait Until Data Clear Completed");
                    if (serviceClient.ClearRuleData(CurrentPeriod.period_id, CurrentCompanyRule.rule_id))
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show("Clear Successfully ");
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show("Clear Fail ");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to clear data");
            }
            else
            {
                BusyBox.ShowBusy("Please Wait Until Excel Data Upload Completed");
                refreshEmployeeRule();
                addValues(CurrentCompanyRule, CurrentPeriod);
            }
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

        private void refreshPeriods()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result;
            };
            this.serviceClient.GetPeriodsAsync();
        }

        private void refreshCompanyRules()
        {
            this.serviceClient.GetCompanyRulesCompleted += (s, e) =>
            {
                this.CompanyRules = e.Result;
                CurrentCompanyRule = CompanyRules.FirstOrDefault(z => z.rule_id == Guid.Parse("89a95b5b-ae9d-4ec8-b502-e59d99602895"));

            };

            this.serviceClient.GetCompanyRulesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void refreshEmployeeRule()
        {
            //this.serviceClient.GetEmployeeRuleCompleted += (s, e) =>
            //{
            foreach (var item_rule in serviceClient.GetEmployeeRule().Where(t => t.isactive == true))
            {
                employeeRule.Add(item_rule);
            }
            //};
            //this.serviceClient.GetEmployeeRuleAsync();
        }

        private void refreshPeriodQuantiy()
        {
            //this.serviceClient.GetAllTrnsPeriodQuantityCompleted += (s, e) => 
            //{
            this.EmployeePeriodQantity = serviceClient.GetAllTrnsPeriodQuantity(CurrentPeriod.period_id);
            //};

            //this.serviceClient.GetAllTrnsPeriodQuantityAsync();
        }

        #endregion

        private void addValues(mas_CompanyRule rule, z_Period period)
        {
            Cursor = System.Windows.Input.Cursors.Wait;

            if (rule != null && period != null)
            {
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
                            dtl_EmployeeRule currentRule = new dtl_EmployeeRule();
                            currentRule = employeeRule.FirstOrDefault(z => z.rule_id == CurrentCompanyRule.rule_id);
                            trns_EmployeePeriodQunatity temp_qty = EmployeePeriodQantity.FirstOrDefault(z => z.employee_id == employee_id && z.period_id == CurrentPeriod.period_id && z.rule_id == currentRule.rule_id);
                            if (temp_qty != null)
                            {
                                if (currentRule != null)
                                {
                                    if (employee_id != null && employee_id != Guid.Empty)
                                    {
                                        string specal = CurrentCompanyRule.rule_name;
                                        string quntity_tt = dr[specal].ToString();
                                        if (SpecialAmount == true)
                                        {
                                            dtl_EmployeeRule new_rule = new dtl_EmployeeRule();
                                            new_rule.rule_id = currentRule.rule_id;
                                            new_rule.employee_id = employee_id;
                                            new_rule.is_special = true;
                                            new_rule.special_amount = decimal.Parse(quntity_tt);
                                            new_rule.modified_datetime = System.DateTime.Now.Date;
                                            new_rule.modified_user_id = clsSecurity.loggedUser.user_id;
                                            UpdateRule.Add(new_rule);

                                            trns_EmployeePeriodQunatity period_quntity = new trns_EmployeePeriodQunatity();
                                            period_quntity.employee_id = employee_id;
                                            period_quntity.rule_id = CurrentCompanyRule.rule_id;
                                            period_quntity.period_id = CurrentPeriod.period_id;
                                            period_quntity.quantity = 1;
                                            period_quntity.is_proceed = false;
                                            period_quntity.save_user_id = clsSecurity.loggedUser.user_id;
                                            period_quntity.save_datetime = DateTime.Now;
                                            period_quntity.modified_user_id = clsSecurity.loggedUser.user_id;
                                            period_quntity.modified_datetime = DateTime.Now;
                                            period_quntity.delete_user_id = clsSecurity.loggedUser.user_id;
                                            period_quntity.delete_datetime = DateTime.Now;
                                            period_quntity.isdelete = false;
                                            QTY.Add(period_quntity);
                                        }
                                        else
                                        {
                                            dtl_EmployeeRule new_rule = new dtl_EmployeeRule();
                                            new_rule.rule_id = currentRule.rule_id;
                                            new_rule.employee_id = employee_id;
                                            new_rule.is_special = true;
                                            new_rule.special_amount = decimal.Parse(quntity_tt);
                                            new_rule.modified_datetime = System.DateTime.Now.Date;
                                            new_rule.modified_user_id = clsSecurity.loggedUser.user_id;
                                            UpdateRule.Add(new_rule);

                                            trns_EmployeePeriodQunatity period_quntity = new trns_EmployeePeriodQunatity();
                                            period_quntity.employee_id = employee_id;
                                            period_quntity.rule_id = CurrentCompanyRule.rule_id;
                                            period_quntity.period_id = CurrentPeriod.period_id;
                                            period_quntity.quantity = excel_quntity;
                                            period_quntity.is_proceed = false;
                                            period_quntity.save_user_id = clsSecurity.loggedUser.user_id;
                                            period_quntity.save_datetime = DateTime.Now;
                                            period_quntity.modified_user_id = clsSecurity.loggedUser.user_id;
                                            period_quntity.modified_datetime = DateTime.Now;
                                            period_quntity.delete_user_id = clsSecurity.loggedUser.user_id;
                                            period_quntity.delete_datetime = DateTime.Now;
                                            period_quntity.isdelete = false;
                                            QTY.Add(period_quntity);
                                        }
                                    }
                                    else
                                    {
                                        Employee_no.Add(emp_id);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {


                        }

                    }
                    //if (QTY.Count > 0 && UpdateRule.Count > 0)
                    //{

                    List<trns_EmployeePeriodQunatity> chunkQTY = new List<trns_EmployeePeriodQunatity>();
                    List<dtl_EmployeeRule> chunkRule = new List<dtl_EmployeeRule>();
                    int size = 50;
                    foreach (var item in QTY)
                    {
                        if (chunkQTY.Count() == size)
                        {
                            chunkQTY.Add(item);
                            serviceClient.saveQuantityFromAttendance(chunkQTY.ToArray(), CurrentPeriod.period_id);
                            chunkQTY = new List<trns_EmployeePeriodQunatity>();
                        }
                        else
                        {
                            chunkQTY.Add(item);
                        }
                    }
                    serviceClient.saveQuantityFromAttendance(chunkQTY.ToArray(), CurrentPeriod.period_id);

                    foreach (var item in UpdateRule)
                    {
                        if (chunkRule.Count() == size)
                        {
                            chunkRule.Add(item);
                            serviceClient.AttendanceBonusUpdate(chunkRule.ToArray());
                            chunkRule = new List<dtl_EmployeeRule>();
                        }
                        else
                        {
                            chunkRule.Add(item);

                        }
                    }
                    serviceClient.AttendanceBonusUpdate(chunkRule.ToArray());


                    //       if (serviceClient.saveQuantityFromAttendance(QTY.ToArray(), CurrentPeriod.period_id))
                    //       {
                    //           if (serviceClient.AttendanceBonusUpdate(UpdateRule.ToArray()))
                    //           {
                    //               Cursor = System.Windows.Input.Cursors.Arrow;
                    //               if (Employee_no.Count > 0)
                    //               {
                    //                   string ErrorMessage = "";
                    //                   MessageBoxResult exresult = MessageBox.Show("Upload With  Errors..  Do you need to Continue ?", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //                   if (exresult == MessageBoxResult.Yes)
                    //                   {
                    //                       foreach (var item in Employee_no)
                    //                       {
                    //                           ErrorMessage += item + "\n";
                    //                       }

                    //                       MessageBox.Show("Error employee numbers are  " + "\n" + ErrorMessage);

                    //                   }
                    //               }
                    //else
                    //{
                    MessageBox.Show(CurrentCompanyRule.ToString() + " " + " Migration Process Successfully " + QTY.Count + "Item Saved");
                }
                //           }
                //       }
                //       else
                //       {
                //           MessageBox.Show("Attendance Migration Process Fail ");
                //       }
                //  }

                //}
                catch (Exception ex)
                {
                    Cursor = System.Windows.Input.Cursors.Arrow;
                    MessageBox.Show(ex.Message);
                }
            }
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
    }
}
