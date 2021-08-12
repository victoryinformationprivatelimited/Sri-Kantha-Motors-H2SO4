using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using ERP.BasicSearch;
using ERP.Properties;
using System.Windows;


namespace ERP.Attendance.Attendance_Process
{
    class ManualEmployeeAttendanceViewModel : ViewModelBase
    {
        #region Service Client

        ERPServiceClient serviceClient;

        #endregion

        #region Constructor
        public ManualEmployeeAttendanceViewModel()
        {
            serviceClient = new ERPServiceClient();
            this.refreshBasicEmployeeDetails();
            Date = DateTime.Now.Date;
        }

        #endregion

        #region Data Members

        DateTime? DateInTime;
        DateTime? DateOutTime;
        List<dtl_AttendanceData> listAttendanceData = new List<dtl_AttendanceData>(); 

        #endregion

        #region Propeties

        private bool _typeSelect;
        public bool TypeSelect
        {
            get { return _typeSelect; }
            set
            {
                _typeSelect = value; OnPropertyChanged("TypeSelect");
                if (TypeSelect == false)
                    TypeManual = true;
            }
        }

        private bool _typeManual;
        public bool TypeManual
        {
            get { return _typeManual; }
            set
            {
                _typeManual = value; OnPropertyChanged("TypeManual");
                if (TypeManual == false)
                    TypeSelect = true;
            }
        }

        private TimeSpan? _inTime;
        public TimeSpan? InTime
        {
            get { return _inTime; }
            set { _inTime = value; OnPropertyChanged("InTime"); }
        }

        private string _outTime;
        public string OutTime
        {
            get { return _outTime; }
            set { _outTime = value; OnPropertyChanged("OutTime"); }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged("Date"); }
        }

        private IEnumerable<EmployeeShiftDetailView> _Shift;
        public IEnumerable<EmployeeShiftDetailView> Shift
        {
            get { return _Shift; }
            set { _Shift = value; OnPropertyChanged("Shift"); }
        }

        private EmployeeShiftDetailView _CurrentShift;
        public EmployeeShiftDetailView CurrentShift
        {
            get { return _CurrentShift; }
            set { _CurrentShift = value; OnPropertyChanged("CurrentShift"); }
        }

        private IEnumerable<EmployeeSearchView> _employeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _employeeSearch; }
            set { _employeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        IEnumerable<mas_Employee> employees;
        public IEnumerable<mas_Employee> Employees
        {
            get
            { 
                if(employees != null)
                {
                    employees = employees.OrderBy(c => Convert.ToInt32(c.emp_id));
                }
                return employees; 
            }
            set { employees = value; OnPropertyChanged("Employees"); }
        }

        mas_Employee currentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set 
            { 
                currentEmployee = value; OnPropertyChanged("CurrentEmployee");
                if (currentEmployee != null)
                {
                    FullName = (currentEmployee.initials == null ? "" : currentEmployee.initials + " ") +
                        (currentEmployee.first_name == null ? "" : currentEmployee.first_name + " ") +
                        (currentEmployee.second_name == null ? "" : currentEmployee.second_name);
                }
            }

        }

        IEnumerable<dtl_AttendanceData> employeeAttendance;
        public IEnumerable<dtl_AttendanceData> EmployeeAttendance
        {
            get { return employeeAttendance; }
            set { employeeAttendance = value; OnPropertyChanged("EmployeeAttendance"); }
        }

        dtl_AttendanceData currentSelectedEmployeeAttendance;
        public dtl_AttendanceData CurrentSelectedEmployeeAttendance
        {
            get { return currentSelectedEmployeeAttendance; }
            set { currentSelectedEmployeeAttendance = value; OnPropertyChanged("CurrentSelectedEmployeeAttendance"); }
        }

        string fullName;
        public string FullName
        {
            get 
            {
                return fullName;
            }
            set
            {
                fullName = value; OnPropertyChanged("FullName");
            }
        }

        #endregion

        #region Refresh Methods

        void refreshShift()
        {
            try
            {
                serviceClient.GetShiftDetailsViewListCompleted += (s, e) =>
                {
                    this.Shift = e.Result;
                };
                this.serviceClient.GetShiftDetailsViewListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        void refreshBasicEmployeeDetails()
        {
            serviceClient.GetEmployeeBasicDetailsCompleted += (s, e) =>
                {
                    try
                    {
                        Employees = e.Result;
                    }
                    catch (Exception)
                    {
                        clsMessages.setMessage("Employees refresh is failed");
                    }
                };
            serviceClient.GetEmployeeBasicDetailsAsync();
        }

        void FindCurrentEmployeeAttendanceDetails()
        {
            try
            {
                EmployeeAttendance = null;
                EmployeeAttendance = serviceClient.GetEmployeeDailyAttendance(currentEmployee.emp_id.TrimStart('0'), _Date.Value);
                if(employeeAttendance != null)
                {
                    if(employeeAttendance.Count() == 0)
                    {
                        clsMessages.setMessage("No attendance found for selected date");
                    }
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Finding attendance has failed");
            }
        }

        #endregion

        #region Button Methods

        #region Select Button

        public ICommand SelectButton
        {
            get { return new RelayCommand(Select); }
        }

        void Select()
        {
            List<EmployeeSearchView> tempEmployeeSearch = new List<EmployeeSearchView>();
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            EmployeeSearch = null;
            if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                CurrentEmployee = window.viewModel.selectEmployeeList.Select(c => new mas_Employee { emp_id = c.emp_id, employee_id = c.employee_id, first_name = c.first_name, second_name = c.second_name, initials = c.initials}).FirstOrDefault();
            window.Close();
            window = null;

        }

        #endregion

        #region Insert Button

        public ICommand InsertButton
        {
            get { return new RelayCommand(Insert, InsertCanExecute); }
        }

        bool InsertCanExecute()
        {
            if (currentEmployee == null)
                return false;
            if (Date == null)
                return false;
            if (Date > System.DateTime.Now)
                return false;
            return true;
        }

        void Insert()
        {
            In();
            Save();
        }

        private void DateMethod()
        {
            // 2014-03-11 16:58:00.000
            //if (TypeSelect)
            //{
            //    if (CurrentShift != null)
            //    {
            //        DateInTime = Convert.ToDateTime(Date.Value.ToShortDateString() + " " + CurrentShift.in_time + ".000");
            //        In();
            //        DateOutTime = Convert.ToDateTime(Date.Value.ToShortDateString() + " " + CurrentShift.out_time + ".000");
            //        Out();
            //    }
            //    else
            //        clsMessages.setMessage("Please Select A Shift");
            //}
            //else
            //{
            //    if (String.IsNullOrEmpty(InTime) == false)
            //    {
            //        DateInTime = Convert.ToDateTime(Date.Value.ToShortDateString() + " " + InTime + ".000");
            //        In();
            //    }
            //    if (String.IsNullOrEmpty(OutTime) == false)
            //    {
            //        DateOutTime = Convert.ToDateTime(Date.Value.ToShortDateString() + " " + OutTime + ".000");
            //        Out();
            //    }
            //}
        }

        void In()
        {
            if (currentEmployee != null)
            {
                if (_Date != null)
                {
                    DateInTime = _Date;
                    if (_inTime != null)
                    {
                        DateInTime = DateInTime.Value.Add(_inTime.Value);
                        try
                        {
                            // Device ID S
                            dtl_AttendanceData newAttendanceData = new dtl_AttendanceData();
                            Guid device_id = new Guid("00000000-0000-0000-0000-000000000001");// Virtual Device ID
                            newAttendanceData.attendance_data_id = Guid.NewGuid();
                            newAttendanceData.device_id = device_id;//Device ID
                            newAttendanceData.emp_id = currentEmployee.emp_id.TrimStart('0');//Employee ID
                            newAttendanceData.year = DateInTime.Value.Year;//Year
                            newAttendanceData.day = DateInTime.Value.Day;//Day
                            newAttendanceData.month = DateInTime.Value.Month;//Month
                            newAttendanceData.hour = DateInTime.Value.Hour;//Hour
                            newAttendanceData.minute = DateInTime.Value.Minute;//Minute
                            newAttendanceData.second = DateInTime.Value.Second;//Seconds
                            newAttendanceData.attend_datetime = DateInTime;//Attend Date Time
                            newAttendanceData.attend_date = DateInTime;//Attend Date
                            newAttendanceData.attend_time = DateInTime.Value.TimeOfDay;//Attend Time
                            newAttendanceData.mode_id = Guid.Empty;//Mode ID
                            newAttendanceData.verify_id = Guid.Empty;//Verify ID
                            newAttendanceData.save_user_id = clsSecurity.loggedUser.user_id;
                            newAttendanceData.save_datetime = DateTime.Now;
                            newAttendanceData.isdelete = false;
                            //newAttendanceData.is_manual = true;
                            listAttendanceData.Add(newAttendanceData);

                        }
                        catch (Exception)
                        {
                            clsMessages.setMessage("data setting error");
                        } 
                    }
                    else
                    {
                        clsMessages.setMessage("Attendance time should be added");
                    }
                }
                else
                {
                    clsMessages.setMessage("Attendance date should be selected");
                }
            }
            else
            {
                clsMessages.setMessage("Employee should be selected");
            }
        }

        void Out()
        {
            if (EmployeeSearch != null)
                try
                {
                    foreach (var item in EmployeeSearch)
                    {   // Device ID S
                        if (DateOutTime != null)
                        {
                            dtl_AttendanceData newAttendanceData = new dtl_AttendanceData();
                            Guid device_id = new Guid("00000000-0000-0000-0000-000000000001");// Virtual Device ID
                            newAttendanceData.attendance_data_id = Guid.NewGuid();
                            newAttendanceData.device_id = device_id;//Device ID
                            newAttendanceData.emp_id = item.emp_id;//Employee ID
                            newAttendanceData.year = DateOutTime.Value.Year;//Year
                            newAttendanceData.day = DateOutTime.Value.Day;//Day
                            newAttendanceData.month = DateOutTime.Value.Month;//Month
                            newAttendanceData.hour = DateOutTime.Value.Hour;//Hour
                            newAttendanceData.minute = DateOutTime.Value.Minute;//Minute
                            newAttendanceData.second = DateOutTime.Value.Second;//Seconds
                            newAttendanceData.attend_datetime = DateOutTime;//Attend Date Time
                            newAttendanceData.attend_date = DateOutTime;//Attend Date
                            newAttendanceData.attend_time = DateOutTime.Value.TimeOfDay;//Attend Time
                            newAttendanceData.mode_id = Guid.Empty;//Mode ID
                            newAttendanceData.verify_id = Guid.Empty;//Verify ID
                            newAttendanceData.save_user_id = clsSecurity.loggedUser.user_id;
                            newAttendanceData.save_datetime = DateTime.Now;
                            newAttendanceData.isdelete = false;
                            listAttendanceData.Add(newAttendanceData);
                        }
                    }
                }
                catch
                {
                    clsMessages.setMessage("List Error");
                }
        }

        #endregion

        #region Find Button

        public ICommand FindButton
        {
            get { return new RelayCommand(FindAttendance); }
        }

        private void FindAttendance()
        {
            if(currentEmployee != null && _Date != null)
            {
                FindCurrentEmployeeAttendanceDetails();
            }
            else
            {
                clsMessages.setMessage("Required details should be selected");
            }
        }

        #endregion

        #region Clear Button

        public ICommand CleartButton
        {
            get { return new RelayCommand(Clear); }
        }

        void Clear()
        {
            listAttendanceData.Clear();
            EmployeeAttendance = null;
            InTime = null;
            FullName = null;
            CurrentEmployee = null;
            Date = DateTime.Now.Date;
        }

        #endregion

        #region Delete Button

        public ICommand DeletetButton
        {
            get { return new RelayCommand(Delete); }
        }

        private void Delete()
        {
            if(currentSelectedEmployeeAttendance != null)
            {
                if (clsSecurity.GetDeletePermission(315))
                {
                    dtl_AttendanceData deletingAttendance = new dtl_AttendanceData();
                    deletingAttendance.attendance_data_id = currentSelectedEmployeeAttendance.attendance_data_id;
                    deletingAttendance.device_id = currentSelectedEmployeeAttendance.device_id;
                    deletingAttendance.emp_id = currentSelectedEmployeeAttendance.emp_id;
                    deletingAttendance.delete_datetime = DateTime.Now;
                    deletingAttendance.delete_user_id = clsSecurity.loggedUser.user_id;
                    deletingAttendance.isdelete = true;

                    if (serviceClient.DeleteEmployeeCurrentAttendance(deletingAttendance))
                    {
                        clsMessages.setMessage("Employee attendance deleted successfully");
                        this.FindAttendance();
                    }
                    else
                    {
                        clsMessages.setMessage("Employee attendance delete is failed");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
            else
            {
                clsMessages.setMessage("Employee attendance should be selected");
            }
        }

        #endregion

        #endregion

        void Save()
        {
            string SaveList = "";
            if (listAttendanceData.Count > 0)
            {
                if (clsSecurity.GetSavePermission(315))
                {
                    var item = listAttendanceData.FirstOrDefault();
                    List<dtl_AttendanceData> temp = new List<dtl_AttendanceData>();
                    temp.Add(item);
                    if (serviceClient.SaveManualAttendanceUpload(temp.ToArray()) == 1)
                    {
                        SaveList += Resources.SaveSucess + "  " + item.emp_id + "  " + fullName + "\n";
                        listAttendanceData.Clear();
                        this.FindAttendance();
                    }
                    else
                    {
                        SaveList += "Already exists in the system" + " " + item.emp_id + " " + item.attend_time + "\n";
                    }
                    // MessageBox.Show(, , System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    Xceed.Wpf.Toolkit.MessageBox.Show(SaveList, "Save List", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    clsMessages.setMessage("You don't have permission to Save this record(s)");
            }
            
        }
    }
}
