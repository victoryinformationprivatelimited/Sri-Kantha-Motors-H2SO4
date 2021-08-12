using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.AttendanceService;
using ERP.ERPService;
using System.Windows.Input;
using System.Collections;
using System.Windows;
using ERP.Properties;
using CustomBusyBox;

namespace ERP.Attendance
{
    class ManualAttendanceSelectionViewModel : ViewModelBase
    {
        #region Fields
        AttendanceServiceClient attServiceClient;
        ERPServiceClient erpServiceClient;
        #endregion

        #region Constructor
        public ManualAttendanceSelectionViewModel()
        {
            attServiceClient = new AttendanceServiceClient();
            erpServiceClient = new ERPServiceClient();
            RefreshEmployees();
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }
        #endregion

        #region Properties
        private IEnumerable<EmployeeSumarryView> _Employees;

        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;

        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value;
                OnPropertyChanged("CurrentEmployee");
                if (CurrentEmployee != null)
                {
                    EmployeeName = CurrentEmployee.initials + " " + CurrentEmployee.first_name + " " + CurrentEmployee.second_name;
                }
            }
        }

        private String _EmployeeName;

        public String EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; OnPropertyChanged("EmployeeName"); }
        }

        private DateTime _FromDate;

        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; }
        }

        private DateTime _ToDate;

        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; }
        }

        private IEnumerable<EmployeeShiftView> _Shifts;

        public IEnumerable<EmployeeShiftView> Shifts
        {
            get { return _Shifts; }
            set { _Shifts = value; OnPropertyChanged("Shifts"); }
        }

        private EmployeeShiftView _CurrentShift;

        public EmployeeShiftView CurrentShift
        {
            get { return _CurrentShift; }
            set { _CurrentShift = value; OnPropertyChanged("CurrentShift"); }
        }

        private IEnumerable<ERP.AttendanceService.dtl_AttendanceData> _AttendanceData;

        public IEnumerable<ERP.AttendanceService.dtl_AttendanceData> AttendanceData
        {
            get { return _AttendanceData; }
            set { _AttendanceData = value; OnPropertyChanged("AttendanceData"); }
        }

        private ERP.AttendanceService.dtl_AttendanceData _CurrentAttendanceData;

        public ERP.AttendanceService.dtl_AttendanceData CurrentAttendanceData
        {
            get { return _CurrentAttendanceData; }
            set { _CurrentAttendanceData = value; OnPropertyChanged("CurrentAttendanceData"); }
        }

        private IList _CurrentShiftList = new ArrayList();
        public IList CurrentShiftList
        {
            get { return _CurrentShiftList; }
            set { _CurrentShiftList = value; OnPropertyChanged("CurrentShiftList"); }
        }
        #endregion

        #region Refresh Methods
        private void RefreshEmployees()
        {
            try
            {
                erpServiceClient.GetAllEmployeeDetailCompleted += (s, e) =>
                    {
                        Employees = e.Result.OrderBy(c => c.emp_id);
                    };
                erpServiceClient.GetAllEmployeeDetailAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error in Employees Loading");
            }
        }

        private void RefreshEmployeeShifts()
        {
            try
            {
                Shifts = attServiceClient.GetEmployeeShifts(FromDate, ToDate, CurrentEmployee.employee_id).OrderBy(c => c.date);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error in Shifts Loading");
            }
        }

        private void RefreshEmployeeAttendance()
        {
            try
            {
                AttendanceData = attServiceClient.GetEmployeeAttendanceData(FromDate, ToDate, CurrentEmployee.emp_id.TrimStart('0')).OrderBy(c => c.attend_datetime);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error in Attendance Data Loading");
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand GetData
        {
            get
            {
                return new RelayCommand(GetShiftAndAttendanceData);
            }
        }

        private void GetShiftAndAttendanceData()
        {
            BusyBox.ShowBusy("Please Wait...");
            RefreshEmployeeShifts();
            RefreshEmployeeAttendance();
            BusyBox.CloseBusy();
        }

        public ICommand INTime
        {
            get
            {
                return new RelayCommand(IN);
            }
        }

        private void IN()
        {
            try
            {
                if (CurrentShiftList.Count > 0)
                {
                    List<EmployeeShiftView> temp = new List<EmployeeShiftView>();
                    foreach (EmployeeShiftView item in CurrentShiftList)
                    {
                        item.in_time = CurrentAttendanceData.attend_datetime;
                        temp.Add(item);
                    }
                    foreach (var item in Shifts.Where(c => !temp.Any(d => d.date == c.date)))
                    {
                        temp.Add(item);
                    }
                    Shifts = null;
                    Shifts = temp.OrderBy(c => c.date).ToList();
                    CurrentShiftList.Clear();
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Erro Please Try Again...");
            }
        }

        public ICommand OUTTime
        {
            get
            {
                return new RelayCommand(OUT);
            }
        }

        private void OUT()
        {
            try
            {
                if (CurrentShiftList.Count > 0)
                {
                    List<EmployeeShiftView> temp = new List<EmployeeShiftView>();
                    foreach (EmployeeShiftView item in CurrentShiftList)
                    {
                        item.out_time = CurrentAttendanceData.attend_datetime;
                        temp.Add(item);
                    }
                    foreach (var item in Shifts.Where(c => !temp.Any(d => d.date == c.date)))
                    {
                        temp.Add(item);
                    }
                    Shifts = null;
                    Shifts = temp.OrderBy(c => c.date).ToList();
                    CurrentShiftList.Clear();
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error Please Try Again...");
            }
        }

        public ICommand ClearTime
        {
            get
            {
                return new RelayCommand(clear_data);
            }
        }

        private void clear_data()
        {
            try
            {
                if (CurrentShiftList.Count > 0)
                {
                    List<EmployeeShiftView> temp = new List<EmployeeShiftView>();
                    foreach (EmployeeShiftView item in CurrentShiftList)
                    {
                        item.in_time = null;
                        item.out_time = null;
                        temp.Add(item);
                    }
                    foreach (var item in Shifts.Where(c => !temp.Any(d => d.date == c.date)))
                    {
                        temp.Add(item);
                    }
                    Shifts = null;
                    Shifts = temp.OrderBy(c => c.date).ToList();
                    CurrentShiftList.Clear();
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error Please Try Again...");
            }
        }

        public ICommand SaveData
        {
            get
            {
                return new RelayCommand(SaveAttendanceData);
            }
        }

        private void SaveAttendanceData()
        {
            try
            {
                clsMessages.setMessage("Are You Sure You Want To Save Attendance Data?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    BusyBox.ShowBusy("Please Wait Until Data Get Saved...");
                    List<EmployeeShiftView> SaveAttendanceData = new List<EmployeeShiftView>();
                    foreach (var item in Shifts)
                    {
                        if (item.in_time != null || item.out_time != null)
                        {
                            item.employee_id = CurrentEmployee.employee_id;
                            item.in_time = item.in_time;
                            item.out_time = item.out_time;
                            item.date = item.date;
                            item.shift_detail_id = item.shift_detail_id;
                            SaveAttendanceData.Add(item); 
                        }
                    }
                    if (attServiceClient.SaveEmployeeAttendanceData(SaveAttendanceData.ToArray(), clsSecurity.loggedUser.user_id))
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Employee Attendance Saved SuccessFully...");
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Employee Attendance Save Failed...");
                    }
                }
            }
            catch (Exception)
            {
                BusyBox.CloseBusy();
                clsMessages.setMessage("Error in Employee Attendance Saving...");
            }
        }
        #endregion
    }
}
