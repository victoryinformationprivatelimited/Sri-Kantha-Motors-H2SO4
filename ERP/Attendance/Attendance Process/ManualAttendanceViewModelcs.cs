using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Attendance_Process
{
    public class ManualAttendanceViewModelcs:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        List<AttendanceManual_view> ListItem = new List<AttendanceManual_view>();
        List<dtl_AttendanceData> AttendanceDataList = new List<dtl_AttendanceData>();

        #region Constructor

        public ManualAttendanceViewModelcs()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.ManualAttendance), clsSecurity.loggedUser.user_id))
            {
                viewrefreshEmployees();
                CurrentDate = DateTime.Now.Date;
                RefreshAttendanceMode();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        } 

        #endregion

        #region Properties

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; this.OnPropertyChanged("CurrentDate"); }
        }

        private IEnumerable<AttendanceManual_view> _EmployeeAttendance;
        public IEnumerable<AttendanceManual_view> EmployeeAttendance
        {
            get { return _EmployeeAttendance; }
            set { _EmployeeAttendance = value; this.OnPropertyChanged("EmployeeAttendance"); }
        }

        private AttendanceManual_view _CurrentEmployeeAttendance;
        public AttendanceManual_view CurrentEmployeeAttendance
        {
            get { return _CurrentEmployeeAttendance; }
            set
            {
                _CurrentEmployeeAttendance = value; this.OnPropertyChanged("CurrentEmployeeAttendance");
                if (CurrentEmployeeAttendance != null)
                {
                    Time = (TimeSpan)CurrentEmployeeAttendance.attend_time;
                }
            }
        }

        private IEnumerable<z_AttendanceInOutMode> _Mode;
        public IEnumerable<z_AttendanceInOutMode> Mode
        {
            get { return _Mode; }
            set { _Mode = value; this.OnPropertyChanged("Mode"); }
        }

        private z_AttendanceInOutMode _CurrentMode;
        public z_AttendanceInOutMode CurrentMode
        {
            get { return _CurrentMode; }
            set
            {
                _CurrentMode = value; this.OnPropertyChanged("CurrentMode");

            }
        }

        private List<AttendanceManual_view> _SelectionItems = new List<AttendanceManual_view>();
        public List<AttendanceManual_view> SelectionItems
        {
            get { return this._SelectionItems; }
            set
            {
                this._SelectionItems = value;
                this.OnPropertyChanged("SelectionItems");
            }
        }

        private TimeSpan _Time;
        public TimeSpan Time
        {
            get { return _Time; }
            set { _Time = value; this.OnPropertyChanged("Time"); }
        } 

        #endregion

        #region Button Methdos

        #region Find
        
        public ICommand FindAttendance
        {
            get
            {
                return new RelayCommand(Find, FindCanExecute);
            }
        }

        private bool FindCanExecute()
        {
            if (CurrentEmployee == null)
                return false;
            if (CurrentDate == null)
                return false;
            return true;
        }

        private void Find()
        {
            GetEmployeeAttendance();
            if (EmployeeAttendance != null)
            {
                SelectionItems = EmployeeAttendance.ToList();
            }
        } 

        #endregion

        #region Delete Button

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Deleteattendance, DeleteattendanceCanExecute);
            }
        }

        private void Deleteattendance()
        {
            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.ManualAttendance), clsSecurity.loggedUser.user_id))
            {
                serviceClient.DeleteEmployeeAttendanceForManualAttendanceUpload(getDateString(CurrentDate), CurrentEmployee.emp_id.TrimStart('0'));
                clsMessages.setMessage(Properties.Resources.DeleteSucess);
                GetEmployeeAttendance();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.DeleteSucess);
            }

        }

        private bool DeleteattendanceCanExecute()
        {
            if (EmployeeAttendance == null)
                return false;
            return true;
        } 

        #endregion

        #region Save Button

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, SaveCanExecute);
            }
        }

        private void Save()
        {
            if (ListItem.Count > 0)
            {
                foreach (var att_item in ListItem)
                {
                    dtl_AttendanceData att = new dtl_AttendanceData();
                    att.attendance_data_id = att_item.attendance_data_id;
                    att.device_id = att_item.device_id;
                    att.emp_id = att_item.emp_id.TrimStart('0');
                    att.year = att_item.year;
                    att.month = att_item.month;
                    att.day = att_item.day;
                    att.hour = att_item.hour;
                    att.minute = att_item.minute;
                    att.second = att_item.second;
                    att.attend_datetime = att_item.attend_datetime;
                    att.attend_date = att_item.attend_date;
                    att.attend_time = att_item.attend_time;
                    att.mode_id = att_item.mode_id;
                    att.verify_id = att_item.verify_id;
                    att.save_user_id = clsSecurity.loggedUser.user_id;
                    att.save_datetime = DateTime.Now;
                    att.modified_datetime = DateTime.Now;
                    att.modified_user_id = clsSecurity.loggedUser.user_id;
                    att.delete_user_id = clsSecurity.loggedUser.user_id;
                    att.delete_datetime = DateTime.Now;
                    AttendanceDataList.Add(att);

                }
                if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.ManualAttendance), clsSecurity.loggedUser.user_id))
                {
                    serviceClient.DeleteEmployeeAttendanceForManualAttendanceUpload(getDateString(_CurrentDate), _CurrentEmployee.emp_id.TrimStart('0'));
                    if (serviceClient.SaveEmployeeDailyAttendanceManualData(AttendanceDataList.ToArray()))
                    {
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                        Find();
                        AttendanceDataList.Clear();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.SaveFail);
                        Find();
                    }
                }
                else
                {
                    clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
                }
            }

        }

        private bool SaveCanExecute()
        {
            if (_CurrentEmployee == null)
                return false;
            if (_CurrentDate == null)
                return false;
            if (_SelectionItems == null)
                return false;
            return true;
        } 

        #endregion

        #region Add Button
        
        public ICommand AddAttendanceButton
        {
            get
            {
                return new RelayCommand(AddAttendance, AddAttendanceCanExecute);
            }
        }

        private bool AddAttendanceCanExecute()
        {
            if (_CurrentMode == null)
                return false;
            return true;

        }

        private void AddAttendance()
        {
            ListItem = SelectionItems;
            AttendanceManual_view data = new AttendanceManual_view();
            data.emp_id = _CurrentEmployee.emp_id.TrimStart('0');
            data.attendance_data_id = Guid.NewGuid();
            data.attend_datetime = new DateTime(_CurrentDate.Year, _CurrentDate.Month, _CurrentDate.Day, _Time.Hours, _Time.Minutes, _Time.Seconds);
            data.attend_time = Time;
            data.mode_id = _CurrentMode.mode_id;
            data.mode_name = _CurrentMode.mode_name;
            data.device_id = Guid.Empty;
            data.device_name = "";
            data.year = _CurrentDate.Year;
            data.month = _CurrentDate.Month;
            data.day = _CurrentDate.Day;
            data.hour = _Time.Hours;
            data.minute = _Time.Minutes;
            data.second = _Time.Seconds;
            data.attend_date = _CurrentDate.Date;
            data.verify_id = Guid.Empty;
            data.value = 0;
            ListItem.Add(data);
            SelectionItems = null;
            SelectionItems = ListItem;
        } 

        #endregion

        #region New Attendance Button
        
        public ICommand NewAttendanceBtn
        {
            get
            {
                return new RelayCommand(newattendance, newattendanceCanExecute);
            }
        }

        private bool newattendanceCanExecute()
        {
            if (_CurrentEmployee == null)
                return false;
            if (_CurrentDate == null)
                return false;
            return true;
        }

        private void newattendance()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.ManualAttendance), clsSecurity.loggedUser.user_id))
            {
                CurrentEmployeeAttendance = null;
                CurrentMode = null;

            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        } 

        #endregion

        #endregion

        #region Refresh Methods

        private void viewrefreshEmployees()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(c => c.isdelete == false && c.isActive == true);
            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }

        private void GetEmployeeAttendance()
        {
            ListItem.Clear();
            this.EmployeeAttendance = this.serviceClient.GetAttendanceDataForManualAttendance(getDateString(_CurrentDate), _CurrentEmployee.emp_id.TrimStart('0'));
        }

        private void RefreshAttendanceMode()
        {
            this.serviceClient.GetAttendanceInOutModeCompleted += (s, e) =>
            {
                this.Mode = e.Result;
            };
            this.serviceClient.GetAttendanceInOutModeAsync();
        } 

        #endregion

        public string getDateString(DateTime date)
        {
            string date_string = "";
            // 6/18/2014 12:00:00 AM
            try
            {

                //string[] words = date.ToString().Split(' ');
                //date_string = words[0];
                //string[] words2 = date_string.Split('/');
                //string day = words2[1];
                //string Month = words2[0];
                //string year = words2[2].ToString();
                //if (Month.Length == 1)
                //{
                //    Month = "0" + Month;
                //}
                //if (day.Length == 1)
                //{
                //    day = "0" + day;
                //}
                date_string = CurrentDate.Year + "-" + CurrentDate.Month.ToString("00") + "-" + CurrentDate.Day.ToString("00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message);
            }
            return date_string;
        }
        
    }
}
