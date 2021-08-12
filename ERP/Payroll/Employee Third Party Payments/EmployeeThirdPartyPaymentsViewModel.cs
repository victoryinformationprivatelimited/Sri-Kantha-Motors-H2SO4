using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using Microsoft.Win32;
using System.Data;
using System.Windows;
using CustomBusyBox;

namespace ERP.Payroll.Employee_Third_Party_Payments
{
    class EmployeeThirdPartyPaymentsViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<mas_Employee> employee;
        List<dtl_trns_EmployeeThirdPartyPayments> QTY;
        List<string> Employee_no = new List<string>();
        bool Success = true;
        #endregion

        #region Properties
        private NotifyingProperty cursor = new NotifyingProperty("Cursor", typeof(System.Windows.Input.Cursor), System.Windows.Input.Cursors.Arrow);
        public System.Windows.Input.Cursor Cursor
        {
            get { return (System.Windows.Input.Cursor)GetValue(cursor); }
            set { SetValue(cursor, value); }
        }

        private IEnumerable<z_EmployeeThirdPartyPayments> _ThirdPartyPaymentCategories;

        public IEnumerable<z_EmployeeThirdPartyPayments> ThirdPartyPaymentCategories
        {
            get { return _ThirdPartyPaymentCategories; }
            set { _ThirdPartyPaymentCategories = value; OnPropertyChanged("ThirdPartyPaymentCategories"); }
        }

        private z_EmployeeThirdPartyPayments _CurrentThirdPartyPaymentCategories;

        public z_EmployeeThirdPartyPayments CurrentThirdPartyPaymentCategories
        {
            get { return _CurrentThirdPartyPaymentCategories; }
            set { _CurrentThirdPartyPaymentCategories = value; OnPropertyChanged("CurrentThirdPartyPaymentCategories"); if (CurrentThirdPartyPaymentCategories != null) RefreshEmployeeThirdPartyPayments(); }
        }

        private IEnumerable<dtl_trns_EmployeeThirdPartyPayments> _EmployeeThirdPartyPayments;

        public IEnumerable<dtl_trns_EmployeeThirdPartyPayments> EmployeeThirdPartyPayments
        {
            get { return _EmployeeThirdPartyPayments; }
            set { _EmployeeThirdPartyPayments = value; OnPropertyChanged("EmployeeThirdPartyPayments"); }
        }

        private IEnumerable<EmployeeThirdPartyPaymentsView> _EmployeeThirdPartyPaymentsForGrid;

        public IEnumerable<EmployeeThirdPartyPaymentsView> EmployeeThirdPartyPaymentsForGrid
        {
            get { return _EmployeeThirdPartyPaymentsForGrid; }
            set { _EmployeeThirdPartyPaymentsForGrid = value; OnPropertyChanged("EmployeeThirdPartyPaymentsForGrid"); }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged("Date"); }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
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
        #endregion

        #region Constructor
        public EmployeeThirdPartyPaymentsViewModel()
        {
            serviceClient = new ERPServiceClient();
            employee = new List<mas_Employee>();
            QTY = new List<dtl_trns_EmployeeThirdPartyPayments>();
            RefreshThirdPartyCategories();
            RefreshEmployees();
            //RefreshEmployeeThirdPartyPayments();
            Date = System.DateTime.Now.Date;
        }
        #endregion

        #region Refresh Methods
        private void RefreshThirdPartyCategories()
        {
            try
            {
                serviceClient.GetThirdPartyCategoriesCompleted += (s, e) =>
                        {
                            ThirdPartyPaymentCategories = e.Result;
                        };
                serviceClient.GetThirdPartyCategoriesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Categories Refresh Failed");
            }
        }

        private void RefreshEmployeeThirdPartyPayments()
        {
            try
            {
                //serviceClient.GetEmployeeThirdPartyPaymentsCompleted += (s, e) =>
                //{
                EmployeeThirdPartyPayments = serviceClient.GetEmployeeThirdPartyPayments(CurrentThirdPartyPaymentCategories.category_id);
                //};
                //serviceClient.GetEmployeeThirdPartyPaymentsAsync(CurrentThirdPartyPaymentCategories.category_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Third Party Payments Refresh Failed");
            }
        }

        private void RefreshEmployees()
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

        //private void RefreshEmployeeThirdPartyPaymentsForGrid()
        //{
        //    serviceClient.GetEmployeeThirdPartyPaymentsFromViewCompleted += (s, e) =>
        //    {
        //        EmployeeThirdPartyPaymentsForGrid = e.Result;
        //    };
        //    serviceClient.GetEmployeeThirdPartyPaymentsFromViewAsync(CurrentThirdPartyPaymentCategories.category_id);
        //}
        #endregion

        #region Commands And Methods
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

        private bool btnBrowseCanExecute()
        {
            return true;
        }

        private bool StartCanExecute()
        {
            return true;
        }

        private void Start()
        {
            addValues(CurrentThirdPartyPaymentCategories.category_id, Date);
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

        private void addValues(int categoryID, DateTime date)
        {
            if (clsSecurity.GetSavePermission(522) && clsSecurity.GetUpdatePermission(522) && clsSecurity.GetDeletePermission(522))
            {
                BusyBox.ShowBusy("Please Wait Until Excel Data Upload Completed...");
                Cursor = System.Windows.Input.Cursors.Wait;

                if (categoryID != null && date != null)
                {
                    dtl_trns_EmployeeThirdPartyPayments existData = EmployeeThirdPartyPayments.FirstOrDefault(c => c.category_id == categoryID && c.save_datetime == date && c.is_processed == false);
                    if (existData != null)
                    {
                        serviceClient.DeleteThirdPartyPaymentsData(categoryID, date);
                    }
                    dtl_trns_EmployeeThirdPartyPayments existProcessedData = EmployeeThirdPartyPayments.FirstOrDefault(c => c.category_id == categoryID && c.save_datetime == date && c.is_processed == true);
                    if (existProcessedData != null)
                    {
                        Success = false;
                    }
                    if (Success)
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
                                    if (employee_id != null && employee_id != Guid.Empty)
                                    {
                                        string specal = CurrentThirdPartyPaymentCategories.category_name;
                                        string quntity_tt = dr[specal].ToString();
                                        dtl_trns_EmployeeThirdPartyPayments new_rule = new dtl_trns_EmployeeThirdPartyPayments();
                                        new_rule.employee_id = employee_id;
                                        new_rule.category_id = CurrentThirdPartyPaymentCategories.category_id;
                                        new_rule.amount = decimal.Parse(quntity_tt);
                                        new_rule.save_datetime = Date;
                                        new_rule.save_userid = clsSecurity.loggedUser.user_id;
                                        new_rule.is_processed = false;
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
                            if (serviceClient.saveThirdPartyPaymentsData(QTY.ToArray()))
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
                                    MessageBox.Show(CurrentThirdPartyPaymentCategories.category_name.ToString() + " " + " Migration Process Successfully " + QTY.Count + "Item Saved");
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
                    else
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show("You Have Already Processed Data For Selected Payment Category");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to perform any action in this form");
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
            QTY = new List<dtl_trns_EmployeeThirdPartyPayments>();
            ThirdPartyPaymentCategories = null;
            CurrentThirdPartyPaymentCategories = new z_EmployeeThirdPartyPayments();
            RefreshThirdPartyCategories();
            Employees = null;
            CurrentEmployee = new Emp_Sp_Employee();
            RefreshEmployees();
            Path = null;
            BusyBox.CloseBusy();
        }

        private void Process()
        {
            if (clsSecurity.GetSavePermission(522) && clsSecurity.GetUpdatePermission(522) && clsSecurity.GetDeletePermission(522))
            {
                BusyBox.ShowBusy("Please Wait Until Process Completed...");
                try
                {
                    List<dtl_trns_EmployeeThirdPartyPayments> existEmployee = EmployeeThirdPartyPayments.Where(c => c.category_id == CurrentThirdPartyPaymentCategories.category_id && c.save_datetime == Date && c.is_processed == false).ToList();
                    if (existEmployee != null)
                    {
                        foreach (dtl_trns_EmployeeThirdPartyPayments currentData in existEmployee)
                        {
                            currentData.is_processed = true;
                            currentData.processed_datetime = System.DateTime.Now.Date;
                            currentData.processed_userid = clsSecurity.loggedUser.user_id;
                            QTY.Add(currentData);
                        }

                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show("You Have Already Processed Data For Selected Payment Category");
                    }
                    if (serviceClient.processThirdPartyPaymentsData(QTY.ToArray()))
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Process Completed");
                        New();
                    }
                }
                catch (Exception)
                {
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("Error in Process");
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to perform any action in this form.");
        }
        #endregion
    }
}
