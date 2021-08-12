using ERP.ERPService;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Utility
{
    public class LeaveDataMigrationViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        List<dtl_EmployeeLeave> SaveLeaveList = new List<dtl_EmployeeLeave>();
        List<mas_Employee> tempEmployee = new List<mas_Employee>();

        public LeaveDataMigrationViewModel()
        {
            reafreshEmployee();
            reafreshEmployeeLeaveDetail();
            reafreshLeavePeriod();
        }

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<dtl_EmployeeLeave> _LeaveDetail;
        public IEnumerable<dtl_EmployeeLeave> LeaveDetail
        {
            get { return _LeaveDetail; }
            set { _LeaveDetail = value; this.OnPropertyChanged("LeaveDetail"); }
        }

        private dtl_EmployeeLeave _CurretLeaveDetail;
        public dtl_EmployeeLeave CurretLeaveDetail
        {
            get { return _CurretLeaveDetail; }
            set { _CurretLeaveDetail = value; this.OnPropertyChanged("CurretLeaveDetail"); }
        }

        private IEnumerable<z_LeavePeriod> _LeavePeriods;
        public IEnumerable<z_LeavePeriod> LeavePeriods
        {
            get { return _LeavePeriods; }
            set { _LeavePeriods = value; this.OnPropertyChanged("LeavePeriods"); }
        }

        private z_LeavePeriod _CurrentLeavePeriod;
        public z_LeavePeriod CurrentLeavePeriod
        {
            get { return _CurrentLeavePeriod; }
            set { _CurrentLeavePeriod = value; this.OnPropertyChanged("CurrentLeavePeriod"); }
        }
        
        
        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
        }


        private void reafreshEmployee()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(c => c.isdelete == false);
                tempEmployee.Clear();
                foreach (var emp in e.Result.Where(c => c.isdelete == false))
                {
                    tempEmployee.Add(emp);
                }

            };
            this.serviceClient.GetEmployeesAsync();
        }

        private void reafreshEmployeeLeaveDetail()
        {
            this.serviceClient.GetEmployeeDetailsListCompleted += (s, e) =>
            {
                this.LeaveDetail = e.Result;
            };
            this.serviceClient.GetEmployeeDetailsListAsync();
        }

        private void reafreshLeavePeriod()
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
            {
                this.LeavePeriods = e.Result;
            };
            this.serviceClient.GetLeavePeriodsAsync();
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
            addValues();
        }

        private bool btnBrowseCanExecute()
        {
            return true;
        }

        private void btnbrowse()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".csv";
            fd.Filter = "Excel files (*.xlsx)|*.xlsx";

            Nullable<bool> result = fd.ShowDialog();

            if (result == true)
            {
                Path = fd.FileName;
            }

        }
        private void addValues()
        {

            try
            {
                DataTable dt = ReadExcelFile();

                foreach (DataRow dr in dt.Rows)
                {
                    string emp_id = dr["Employee No"].ToString();
                    string used_anual=dr["Used Annual Leave"].ToString();
                    string used_casual = dr["Used Casual Leave"].ToString();
                   // string used_medical = dr["Employee No"].ToString();
                    string avalable_anual = dr["Remaining Annual Leave"].ToString();
                    string avalable_casual = dr["Remaining Casual Leave"].ToString();

                    CurrentEmployee = Employees.FirstOrDefault(z => z.emp_id.TrimStart('0') == emp_id.TrimStart('0'));
                    if (CurrentEmployee != null)
                    {
                        dtl_EmployeeLeave anewleave = new dtl_EmployeeLeave();
                        anewleave.emp_id = CurrentEmployee.employee_id;
                        anewleave.leave_detail_id = HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.AnualLeaveDetail);
                        anewleave.is_special = false;
                        anewleave.number_of_days = decimal.Parse(dr["Used Annual Leave"].ToString());
                        anewleave.remaining_days = decimal.Parse(dr["Remaining Annual Leave"].ToString());
                        SaveLeaveList.Add(anewleave);

                        dtl_EmployeeLeave cnewleave = new dtl_EmployeeLeave();
                        cnewleave.emp_id = CurrentEmployee.employee_id;
                        cnewleave.leave_detail_id = HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.CasualLeaveDetail);
                        cnewleave.is_special = false;
                        cnewleave.number_of_days = decimal.Parse(dr["Used Casual Leave"].ToString());
                        cnewleave.remaining_days = decimal.Parse(dr["Remaining Casual Leave"].ToString());
                        SaveLeaveList.Add(cnewleave);


                    }
                
                }
                if (SaveLeaveList.Count > 0)
                {
                    //try
                    //{
                    //    if (serviceClient.SaveEmployeeLeaveDataFromExcel(SaveLeaveList.ToArray()))
                    //    {
                    //        MessageBox.Show("Employee Leave Detail Upload Sussfully");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("mployee Leave Detail Upload Fail");
                    //    }
                    //}
                    //catch (Exception exx)
                    //{

                    //    MessageBox.Show(exx.Message);
                    //}
                }
                else
                {
                    MessageBox.Show("No Employee In Excel Sheet");
                }
            }
            catch (Exception ex)
            {
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
    }
}
