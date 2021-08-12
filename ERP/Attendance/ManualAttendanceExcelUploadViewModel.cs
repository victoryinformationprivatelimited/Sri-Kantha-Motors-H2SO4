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

namespace ERP.Attendance
{
    class ManualAttendanceExcelUploadViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<mas_Employee> employee;
        List<dtl_AttendanceData> QTY;
        List<string> Employee_no = new List<string>();
        bool Success = true;
        string emp_id;
        #endregion

        #region Properties
        private NotifyingProperty cursor = new NotifyingProperty("Cursor", typeof(System.Windows.Input.Cursor), System.Windows.Input.Cursors.Arrow);
        public System.Windows.Input.Cursor Cursor
        {
            get { return (System.Windows.Input.Cursor)GetValue(cursor); }
            set { SetValue(cursor, value); }
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
        public ManualAttendanceExcelUploadViewModel()
        {
            serviceClient = new ERPServiceClient();
            employee = new List<mas_Employee>();
            QTY = new List<dtl_AttendanceData>();
            RefreshEmployees();
            //RefreshEmployeeThirdPartyPayments();
            Date = System.DateTime.Now.Date;
        }
        #endregion

        #region Refresh Methods
        private void RefreshEmployees()
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
            addValues();
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

        private void addValues()
        {
            Cursor = System.Windows.Input.Cursors.Wait;
            try
            {
                DataTable dt = ReadExcelFile();

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        emp_id = dr["EmpNo"].ToString();
                        DateTime? excel_quntity_str = (DateTime)dr["AttendTime"];
                        string specal = "AttendDate";
                        DateTime? quntity_tt = (DateTime)dr[specal];
                        DateTime? datetime = quntity_tt.Value.Add(excel_quntity_str.Value.TimeOfDay);
                        //string excel_ststus_str = dr["isQuntity"].ToString();
                        //decimal excel_quntity = decimal.Parse(dr["AttendTime"].ToString());
                        //int excel_ststus = int.Parse(dr["isQuntity"].ToString());

                        Guid employee_id = employee.FirstOrDefault(z => z.emp_id.TrimStart('0') == emp_id.TrimStart('0') && z.isdelete == false).employee_id;
                        if (employee_id != null && employee_id != Guid.Empty)
                        {
                            dtl_AttendanceData new_rule = new dtl_AttendanceData();
                            new_rule.attendance_data_id = Guid.NewGuid();
                            new_rule.device_id = new Guid("00000000-0000-0000-0000-000000000001");
                            new_rule.emp_id = this.emp_id;
                            new_rule.year = Convert.ToInt32(quntity_tt.Value.Year);
                            new_rule.day = Convert.ToInt32(quntity_tt.Value.Day);
                            new_rule.month = Convert.ToInt32(quntity_tt.Value.Month);
                            new_rule.hour = Convert.ToInt32(excel_quntity_str.Value.Hour);
                            new_rule.minute = Convert.ToInt32(excel_quntity_str.Value.Minute);
                            new_rule.second = Convert.ToInt32(excel_quntity_str.Value.Second);
                            new_rule.attend_datetime = datetime;
                            new_rule.attend_date = quntity_tt.Value.Date;
                            new_rule.attend_time = new TimeSpan(new_rule.hour, new_rule.minute, new_rule.second);
                            new_rule.mode_id = new Guid("00000000-0000-0000-0000-000000000000");
                            new_rule.verify_id = new Guid("00000000-0000-0000-0000-000000000000");
                            new_rule.save_user_id = clsSecurity.loggedUser.user_id;
                            new_rule.save_datetime = System.DateTime.Now;
                            new_rule.isdelete = false;
                            new_rule.is_manual = false;
                            QTY.Add(new_rule);
                        }
                        else
                        {
                            //Employee_no.Add(emp_id);
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
                //if (QTY.Count > 0 && UpdateRule.Count>0)
                //{
                if (serviceClient.SaveManualAttendanceUpload(QTY.ToArray()) == 1)
                {
                    Cursor = System.Windows.Input.Cursors.Arrow;
                    if (Employee_no.Count > 0)
                    {
                        string ErrorMessage = "";
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
                        MessageBox.Show("Attendance Upload Successfull, " + QTY.Count + " Item Saved");
                        New();
                    }
                }
                else
                {
                    MessageBox.Show("Attendance Upload Failed");
                }
                //}

            }
            catch (Exception ex)
            {
                Cursor = System.Windows.Input.Cursors.Arrow;
                MessageBox.Show(ex.Message);
            }
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
            QTY = null;
            QTY = new List<dtl_AttendanceData>();
            Employees = null;
            CurrentEmployee = new Emp_Sp_Employee();
            RefreshEmployees();
            Path = null;
        }
        #endregion
    }
}
