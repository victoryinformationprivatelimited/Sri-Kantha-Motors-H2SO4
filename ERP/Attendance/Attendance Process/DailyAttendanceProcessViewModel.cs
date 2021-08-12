using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ERP.Attendance.Attendance_Process
{
    public class DailyAttendanceProcessViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        List<trns_DailyAttendanceProcess> EmployeeAttendance = new List<trns_DailyAttendanceProcess>();

        RelayCommand _IncrementAsBackgroundProcess;
        RelayCommand _ResetCounter;
        DispatcherTimer dis = new DispatcherTimer();

        Double _Value;
        bool _IsInProgress;
        int _Min = 0, _Max = 100;

        public DailyAttendanceProcessViewModel()
        {
            
            //refreshEmployee();
            refreshLeaveType();
            refreshHolidays();
            refreshDetailEmployeeShift();
            refreshShiftDetaila();
            CurretDate = DateTime.Now.Date;
           
        }
        private DateTime _CurretDate;
        public DateTime CurretDate
        {
            get { return _CurretDate; }
            set { _CurretDate = value; this.OnPropertyChanged("CurretDate");
            if (CurretDate != DateTime.MinValue)
            {
                //refreshAttendanceData();
                //refreshLeavePool();
            }
            }
        }
        

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }
        private IEnumerable<dtl_AttendanceData> _AttendanceData;
        public IEnumerable<dtl_AttendanceData> AttendanceData
        {
            get { return _AttendanceData; }
            set { _AttendanceData = value; this.OnPropertyChanged("AttendanceData"); }
        }

        private IEnumerable<trns_LeavePool> _LeavePool;
        public IEnumerable<trns_LeavePool> LeavePool
        {
            get { return _LeavePool; }
            set { _LeavePool = value; }
        }

        private IEnumerable<z_Holiday> _Holidays;
        public IEnumerable<z_Holiday> Holidays
        {
            get { return _Holidays; }
            set { _Holidays = value; }
        }

        private IEnumerable<z_LeaveType> _LeaveType;
        public IEnumerable<z_LeaveType> LeaveType
        {
            get { return _LeaveType; }
            set { _LeaveType = value; }
        }

        private IEnumerable<dtl_EmployeeAttendance> _DetailEmployeeShift;
        public IEnumerable<dtl_EmployeeAttendance> DetailEmployeeShift
        {
            get { return _DetailEmployeeShift; }
            set { _DetailEmployeeShift = value; }
        }
         private dtl_EmployeeAttendance _CurrentDetailEmployeeShift;
        public dtl_EmployeeAttendance CurrentDetailEmployeeShift
        {
            get { return _CurrentDetailEmployeeShift; }
            set { _CurrentDetailEmployeeShift = value; }
        }

        private IEnumerable<dtl_Shift> _Shift;
        public IEnumerable<dtl_Shift> Shift
        {
            get { return _Shift; }
            set { _Shift = value; this.OnPropertyChanged("Shift"); }
        }

        private dtl_Shift _CurrentShift;
        public dtl_Shift CurrentShift
        {
            get { return _CurrentShift; }
            set { _CurrentShift = value; this.OnPropertyChanged("CurrentShift"); }
        }
        private z_Holiday _CurrentHoliday;
        public z_Holiday CurrentHoliday
        {
            get { return _CurrentHoliday; }
            set { _CurrentHoliday = value; }
        }

        private string _StatusString2;
        public string StatusString2
        {
            get { return _StatusString2; }
            set { _StatusString2 = value; this.OnPropertyChanged("StatusString2"); }
        }


        private string _StatusString1;
        public string StatusString1
        {
            get { return _StatusString1; }
            set { _StatusString1 = value; this.OnPropertyChanged("StatusString1"); }
        }
        //private void refreshEmployee()
        //{
        //    this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
        //    {
        //        this.Employees = e.Result.Where(z => z.isdelete == false && z.isActive == true);
        //    };
        //    this.serviceClient.GetAllEmployeeDetailAsync();
        //}

        //private void refreshAttendanceData()
        //{
        //    this.serviceClient.GetAttendanceFromDateRangeCompleted += (s, e) =>
        //    {
        //        this.AttendanceData = e.Result.Where(z => z.isdelete == false);

        //    };
        //    this.serviceClient.GetAttendanceFromDateRangeAsync(CurretDate.Date.AddDays(-1), CurretDate.Date.AddDays(1));
        //}

        private void refreshLeavePool()
        {
            this.serviceClient.GetLeavePoolFromDateCompleted += (s, e) =>
            {
                this.LeavePool = e.Result;
            };
            this.serviceClient.GetLeavePoolFromDateAsync(CurretDate.Date.AddDays(-1), CurretDate.Date.AddDays(1));
        }
        private void refreshHolidays()
        {
            this.serviceClient.GetHolidaysCompleted += (s, e) =>
            {
                this.Holidays = e.Result.Where(z => z.isActive == true);
            };
            this.serviceClient.GetHolidaysAsync();
        }

        private void refreshLeaveType()
        {
            this.serviceClient.GetAllLeaveTypesCompleted += (s, e) =>
            {
                this.LeaveType = e.Result;
            };
            this.serviceClient.GetAllLeaveTypesAsync();
        }

        private void refreshDetailEmployeeShift()
        {
            this.serviceClient.GetEmployeeAttendanceCompleted += (s, e) =>
            {
                this.DetailEmployeeShift =  e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetEmployeeAttendanceAsync();
        }
        private void refreshShiftDetaila()
        {
            this.serviceClient.GetShiftDetailsCompleted += (s, e) =>
            {
                this.Shift = e.Result;
               
            };
            this.serviceClient.GetShiftDetailsAsync();
        }

        //public ICommand StartBTN
        //{
        //    get { return new RelayCommand(StartProcess, StartProcessCanExecute); }
        //}

        //private bool StartProcessCanExecute()
        //{
        //    if (CurretDate == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
           

        //}

        public bool StartProcess()
        {

            foreach (var emp in Employees.ToList())
            {
                CurrentDetailEmployeeShift = DetailEmployeeShift.FirstOrDefault(z => z.employee_id == emp.employee_id);
                if (CurrentDetailEmployeeShift != null)
                {
                    if (CurrentDetailEmployeeShift.is_roster_employee == true)
                    {

                        // roster employee Attendance Calculation
                    }
                    else
                    {
                        //StatusString1 = emp.emp_id;
                        StatusString2 = "Daily Attendance Process Completed";
                        CalculateEmployeeDailyAttendance(emp, CurrentDetailEmployeeShift);
                       
                        // Normal Shift Employee Attendance calculation
                        // string dt = System.DateTime.Now.DayOfWeek.ToString();
                    }

                }
                else
                {
                    // No shift for this employee
                    // ShiftNotDifineEmployee.Add(emp);
                }
            }
            if (EmployeeAttendance.Count > 0)
            {
                StatusString1 = "Saving Attendance Data ..";

                if (SaveDailyAttendanceList())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsInProgress
        {
            get { return _IsInProgress; }
            set
            {
                _IsInProgress = value;
                this.OnPropertyChanged("IsInProgress");
                this.OnPropertyChanged("IsNotInProgress");
            }
        }

        public bool IsNotInProgress
        {
            get { return !IsInProgress; }
        }

        public int Max
        {
            get { return _Max; }
            set { _Max = value; this.OnPropertyChanged("Max"); }
        }

        public int Min
        {
            get { return _Min; }
            set { _Min = value; this.OnPropertyChanged("Min"); }
        }

        public Double Value
        {
            get { return _Value; }
            set
            {
                if (value <= _Max)
                {
                    if (value >= _Min) { _Value = value; }
                    else { _Value = _Min; }
                }
                else { _Value = _Max; }
                this.OnPropertyChanged("Value");
            }
        }

        bool incrementCanExecute()
        {
            return true;
        }


        public ICommand IncrementAsBackgroundProcess
        {
            get
            {
                if (_IncrementAsBackgroundProcess == null)
                {
                    _IncrementAsBackgroundProcess = new RelayCommand(IncrementProgressBackgroundWorker, incrementCanExecute);
                }
                return _IncrementAsBackgroundProcess;
            }
        }

        public ICommand ResetCounter
        {
            get
            {
                if (_ResetCounter == null)
                {
                    _ResetCounter = new RelayCommand(Reset, incrementCanExecute);
                }
                return _ResetCounter;
            }
        }

        public void IncrementProgressBackgroundWorker()
        {
            if (IsInProgress)
                return;

            Reset();
            IsInProgress = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.DailyAttendanceProcess), clsSecurity.loggedUser.user_id))
            {
                dis.Tick += dis_Tick;
                dis.Interval = new TimeSpan(0, 0, 0, 1, 0);
                dis.Start();
            }
            else
            {
                clsMessages.setMessage("No Permission For Daily Attendance Process");
            }

        }

        private void Reset()
        {
            Value = Min;
        }
        void dis_Tick(object sender, EventArgs e)
        {
            Value++;
            if (Value == 5)
            {
                StatusString1 = "Starting Daily Attendance Process ....";
                dis.Stop();
                StatusString1 = null;
                StatusString2 = "Getting All Employees ...";
                Employees = this.serviceClient.GetAllEmployeeDetail();
                dis.Start();
               // dis.Interval = new TimeSpan(0, 0, 0, 1, 0);
                StatusString1 = "Employee Download Successfully ...";
               
            }
            if (Value == 10)
            {
                dis.Stop();
                StatusString1 = "";
                StatusString2 = "Downloading Attendance Data ...";
                AttendanceData=this.serviceClient.GetAttendanceFromDateRange(CurretDate.Date.AddDays(-1), CurretDate.Date.AddDays(1));
                dis.Start();
                StatusString1 = "Attendance Data Download Successfully ...";
            }
            if (Value == 25)
            {
                dis.Stop();
                if (StartProcess())
                {
                    StatusString1 = "Daily Attendance Process Successfully ...";
                }
                else
                {
                    StatusString1 = "Daily Attendance Process Fail ...";
                    StatusString2 = "Please Try again ..";
                }
                dis.Interval = new TimeSpan(0, 0, 0, 0, 1);
                dis.Start();
            }
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsInProgress = false;
            // MessageBox.Show("Process Compleated");

        }
        public void CalculateEmployeeDailyAttendance(EmployeeSumarryView employee,dtl_EmployeeAttendance EmpShift)
        {
                Guid cal_attendanceId = Guid.NewGuid();
                Guid cal_employeeId = employee.employee_id;
                Guid cal_shiftCatagoryId = Guid.Empty;
                Guid cal_dayid = Guid.Empty;
                DateTime cal_attendDate = CurretDate.Date;
                string cal_dayofweek = CurretDate.DayOfWeek.ToString();
                TimeSpan cal_inTime = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_outTime = System.DateTime.MinValue.TimeOfDay;
                DateTime cal_inDateTime = System.DateTime.Today;
                DateTime cal_OutDateTime = System.DateTime.Today;

                TimeSpan cal_earlyInTime = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_lateInTime = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_lateOutTime = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_earlyOutTime = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_earlyCutOffMinits = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_EviningCutOffMinits = System.DateTime.MinValue.TimeOfDay;

                dtl_AttendanceData cal_startAttendancedata = new dtl_AttendanceData();
                dtl_AttendanceData cal_endAttendance = new dtl_AttendanceData();

                bool cal_isHoliday = false;
                bool cal_isAbsent = false;
                bool cal_isFreeDay = false;
                bool cal_isLateIn = false;
                bool cal_isEarlyIn = false;
                bool cal_isLateOut = false;
                bool cal_isEarlyOut = false;
                //bool cal_isInCutOff = false;
                //bool cal_isOutCutOff = false;

                bool cal_isNopay = false;
                //bool cal_isInvalid = false;
                bool cal_isEveningShortDay = false;
                bool cal_isMorningShortDay = false;
                bool cal_isMorningHalfDay = false;
                bool cal_isEveningHalfDay = false;
                bool cal_isLeave = false;
                bool cal_isFullDay = false;
                bool cal_isholidayWork = false;

                //bool cal_isMorningNomalAttendance = false;
                //bool cal_isEviningNormalAttendance = false;

                bool cal_isMorningGraceAttend = false;
                bool cal_isEviningGraceOut = false;

                bool isMorningInvalid=false;
                bool isEveningInvalid=false;

                bool cal_isFreedayWork = false;
                //bool cal_isNormalAttendance = false;
                //bool cal_isMecantileWork = false;
                DateTime cal_ot_date = System.DateTime.Now.Date;
                TimeSpan cal_ot_freeDayWork = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_ot_CutOffTime = System.DateTime.MinValue.TimeOfDay;
                TimeSpan cal_mecantile_work_time = System.DateTime.MinValue.TimeOfDay;


                TimeSpan cal_ot_holidayWork = System.DateTime.MinValue.TimeOfDay;

                dtl_AttendanceData cal_ot_startAttendancedata = new dtl_AttendanceData();
                dtl_AttendanceData cal_ot_endAttendance = new dtl_AttendanceData();
                List<dtl_AttendanceData> cal_startAll = AttendanceData.Where(z => z.attend_date == CurretDate && z.emp_id == employee.emp_id.TrimStart('0')).ToList();

                cal_shiftCatagoryId = (Guid)EmpShift.shift_catagory_id;

                cal_dayid = getDayOfWeek(CurretDate.DayOfWeek.ToString());
                dtl_Shift cal_inShift = Shift.FirstOrDefault(a => a.shift_in_day_id == cal_dayid && a.shift_category_id == cal_shiftCatagoryId);

                if (Holidays != null)
                {
                    CurrentHoliday = Holidays.FirstOrDefault(z => z.holiday_Date.Value.Date == CurretDate.Date);
                }
                if (CurrentHoliday != null)
                {
                    cal_isHoliday = true;
                    cal_attendDate = CurretDate.Date;
                    if (cal_startAll.Count >= 2)
                    {
                            if (cal_startAll.Count > 2)
                            {
                                cal_ot_startAttendancedata = cal_startAll.OrderByDescending(z => z.attend_datetime.GetValueOrDefault(DateTime.MaxValue)).Last();
                                cal_ot_endAttendance = cal_startAll.OrderByDescending(z => z.attend_datetime.GetValueOrDefault(DateTime.MinValue)).First();
                                if (CurrentHoliday.isMercantileHoliday == true)
                                {
                                    if (cal_ot_startAttendancedata != null && cal_ot_endAttendance != null)
                                    {
                                            //cal_isMecantileWork = true;
                                            cal_attendDate = cal_ot_startAttendancedata.attend_date.Value.Date;
                                            cal_inTime = cal_ot_startAttendancedata.attend_time.Value;
                                            cal_outTime = cal_ot_endAttendance.attend_time.Value;
                                            cal_mecantile_work_time = cal_ot_endAttendance.attend_time.Value - cal_ot_startAttendancedata.attend_time.Value;
                                    }

                                }
                                else
                                {
                                    
                                    cal_isFreeDay = true;
                                    cal_attendDate = cal_ot_startAttendancedata.attend_date.Value.Date;
                                    cal_inTime = cal_ot_startAttendancedata.attend_time.Value;
                                    cal_outTime = cal_ot_endAttendance.attend_time.Value;
                                    cal_ot_holidayWork = cal_ot_endAttendance.attend_time.Value - cal_ot_startAttendancedata.attend_time.Value;
                                    cal_isholidayWork = true;

                                }
                        }
                    }
                }
                else
                {
                    if (cal_startAll.Count == 0)
                    {
                        if (checkIsFreeday(CurretDate.Date.DayOfWeek.ToString(), (Guid)EmpShift.shift_catagory_id))
                        {
                            cal_isFreeDay = true;
                        }
                        else
                        {
                            cal_isAbsent = true;
                            if (checkLeave(CurretDate.Date, employee.employee_id, HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyFullLeaveDays)) == true)
                            {
                                cal_isLeave = true;
                            }
                            else
                            {
                                cal_isNopay = true;
                            }
                        }
                    }
                    else
                    {
                        if (cal_inShift != null)
                        {
                                cal_startAttendancedata = cal_startAll.OrderByDescending(z => z.attend_datetime.GetValueOrDefault(DateTime.MaxValue)).LastOrDefault();
                                cal_endAttendance = cal_startAll.OrderByDescending(z => z.attend_datetime.GetValueOrDefault(DateTime.MinValue)).FirstOrDefault();
                                if (cal_startAttendancedata != null)
                                {
                                    cal_dayid = getDayOfWeek(CurretDate.DayOfWeek.ToString());
                                    if (cal_dayid == Guid.Empty)
                                    {
                                        MessageBox.Show("Date Time String MissMatch");
                                    }
                                    else
                                    {
                                        if (Shift != null)
                                        {
                                            if (cal_inShift != null)
                                            {
                                                #region Normal in and cut off attendance
                                                if (cal_startAttendancedata.attend_time <= cal_inShift.shift_in_time)
                                                {
                                                    //cal_isInCutOff = true;
                                                    cal_earlyInTime = cal_inShift.in_time.Value - cal_inShift.shift_in_time.Value;
                                                    cal_earlyCutOffMinits = cal_inShift.shift_in_time.Value - cal_startAttendancedata.attend_time.Value;
                                                    cal_inTime = cal_startAttendancedata.attend_time.Value;
                                                    cal_inDateTime = cal_startAttendancedata.attend_datetime.Value;
                                                    //cal_isMorningNomalAttendance = true;
                                                }
                                                #endregion

                                                #region Normal in Attendance
                                                else if (cal_startAttendancedata.attend_time <= cal_inShift.in_time)
                                                {
                                                    if (cal_inShift.shift_in_time <= cal_startAttendancedata.attend_time && cal_startAttendancedata.attend_time <= cal_inShift.in_time)
                                                    {
                                                        cal_isEarlyIn = true;
                                                        cal_earlyInTime = cal_inShift.in_time.Value - cal_startAttendancedata.attend_time.Value;
                                                        cal_inTime = cal_startAttendancedata.attend_time.Value;
                                                        cal_inDateTime = cal_startAttendancedata.attend_datetime.Value;
                                                        //cal_isMorningNomalAttendance = true;
                                                    }
                                                }
                                                #endregion

                                                if (cal_inShift.in_time <= cal_startAttendancedata.attend_time)
                                                {
                                                    #region calculate attendance if intime between grace time
                                                    if ((cal_startAttendancedata.attend_time - cal_inShift.grace_in) <= cal_inShift.in_time)
                                                    {
                                                        cal_lateInTime = cal_startAttendancedata.attend_time.Value - cal_inShift.in_time.Value;
                                                        cal_isLateIn = true;
                                                        cal_isMorningGraceAttend = true;
                                                        cal_inTime = cal_startAttendancedata.attend_time.Value;
                                                        cal_inDateTime = cal_startAttendancedata.attend_datetime.Value;
                                                    }
                                                    #endregion

                                                    #region calculate attendance with leave
                                                    else if (cal_inShift.in_time <= (cal_startAttendancedata.attend_time - cal_inShift.grace_in))
                                                    {
                                                        if (LeaveType != null)
                                                        {
                                                            z_LeaveType shortDayValue = LeaveType.FirstOrDefault(z => z.leave_type_id == HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyShortDays));
                                                            z_LeaveType helfDayValue = LeaveType.FirstOrDefault(z => z.leave_type_id == HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyHalfDays));
                                                            TimeSpan leaveHour = (cal_inShift.out_time.Value - cal_inShift.in_time.Value);
                                                            int st_time = int.Parse((1 / shortDayValue.value).ToString());
                                                            int ht_time = int.Parse((1 / helfDayValue.value).ToString());
                                                            TimeSpan st_leaveTime = new TimeSpan(leaveHour.Ticks / st_time);
                                                            TimeSpan ht_leaveTime = new TimeSpan(leaveHour.Ticks / ht_time);

                                                            #region calculate if morning short live
                                                            if (cal_startAttendancedata.attend_time.Value - st_leaveTime <= cal_inShift.in_time)
                                                            {
                                                                cal_lateInTime = cal_startAttendancedata.attend_time.Value - cal_inShift.in_time.Value;
                                                                cal_isMorningShortDay = true;
                                                                cal_isLateIn = true;
                                                                cal_inTime = cal_startAttendancedata.attend_time.Value;
                                                                cal_inDateTime = cal_startAttendancedata.attend_datetime.Value;
                                                                if (checkLeave(CurretDate.Date, employee.employee_id, HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyShortDays)) == true)
                                                                {
                                                                    cal_isLeave = true;
                                                                }
                                                                else
                                                                {
                                                                    cal_isNopay = true;
                                                                }

                                                            }
                                                            #endregion

                                                            #region calcuate if morning half Day
                                                            else if (cal_startAttendancedata.attend_time.Value - ht_leaveTime <= cal_inShift.in_time)
                                                            {
                                                                cal_lateInTime = cal_startAttendancedata.attend_time.Value - cal_inShift.in_time.Value;
                                                                cal_isMorningHalfDay = true;
                                                                cal_isLateIn = true;
                                                                cal_inTime = cal_startAttendancedata.attend_time.Value;
                                                                cal_inDateTime = cal_endAttendance.attend_datetime.Value;
                                                                if (checkLeave(CurretDate.Date, employee.employee_id, HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyHalfDays)) == true)
                                                                {
                                                                    cal_isLeave = true;
                                                                }
                                                                else
                                                                {
                                                                    cal_isNopay = true;
                                                                }
                                                            }
                                                            #endregion

                                                            #region calculate if morning others
                                                            else
                                                            {
                                                                isMorningInvalid = true;
                                                              
                                                            }
                                                            #endregion

                                                        }
                                                    }
                                                    #endregion

                                                }
                                                if (cal_endAttendance != null)
                                                {
                                                    #region out attendance is cut off
                                                    if (cal_endAttendance.attend_time >= cal_inShift.shift_out_time)
                                                    {
                                                        //cal_isOutCutOff = true;
                                                        cal_lateOutTime = cal_inShift.shift_out_time.Value - cal_inShift.out_time.Value;
                                                        cal_EviningCutOffMinits = cal_endAttendance.attend_time.Value - cal_inShift.shift_out_time.Value;
                                                        cal_outTime = cal_endAttendance.attend_time.Value;
                                                        cal_OutDateTime = cal_endAttendance.attend_datetime.Value;
                                                        //cal_isEviningNormalAttendance = true;

                                                    }
                                                    #endregion

                                                    #region out attendance normal
                                                    else if (cal_endAttendance.attend_time >= cal_inShift.out_time)
                                                    {
                                                        cal_isLateOut = true;
                                                        cal_lateOutTime = cal_endAttendance.attend_time.Value - cal_inShift.out_time.Value;
                                                        cal_outTime = cal_endAttendance.attend_time.Value;
                                                        cal_OutDateTime = cal_endAttendance.attend_datetime.Value;
                                                        //cal_isEviningNormalAttendance = true;
                                                    }
                                                    #endregion

                                                    if (cal_endAttendance.attend_time <= cal_inShift.out_time)
                                                    {
                                                        #region calculate out attendance between grace time
                                                        if ((cal_endAttendance.attend_time + cal_inShift.grace_out) >= cal_inShift.out_time)
                                                        {
                                                            cal_earlyOutTime = cal_inShift.out_time.Value - cal_endAttendance.attend_time.Value;
                                                            cal_isEarlyOut = true;
                                                            cal_isEviningGraceOut = true;
                                                            cal_outTime = cal_endAttendance.attend_time.Value;
                                                            cal_OutDateTime = cal_endAttendance.attend_datetime.Value;
                                                        }
                                                        #endregion

                                                        #region calculate if out attendance is early
                                                        else if ((cal_endAttendance.attend_time + cal_inShift.grace_out) <= cal_inShift.out_time)
                                                        {
                                                            if (LeaveType != null)
                                                            {
                                                                z_LeaveType shortDayValue = LeaveType.FirstOrDefault(z => z.leave_type_id == HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyShortDays));
                                                                z_LeaveType helfDayValue = LeaveType.FirstOrDefault(z => z.leave_type_id == HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyHalfDays));
                                                                TimeSpan leaveHour = (cal_inShift.out_time.Value - cal_inShift.in_time.Value);
                                                                int st_time = int.Parse((1 / shortDayValue.value).ToString());
                                                                int ht_time = int.Parse((1 / helfDayValue.value).ToString());
                                                                TimeSpan st_leaveTime = new TimeSpan(leaveHour.Ticks / st_time);
                                                                TimeSpan ht_leaveTime = new TimeSpan(leaveHour.Ticks / ht_time);

                                                                #region out attendance between short live
                                                                if (cal_endAttendance.attend_time.Value + st_leaveTime >= cal_inShift.out_time)
                                                                {
                                                                    cal_earlyOutTime = cal_inShift.out_time.Value - cal_endAttendance.attend_time.Value;
                                                                    cal_isEveningShortDay = true;
                                                                    cal_isEarlyOut = true;
                                                                    cal_outTime = cal_endAttendance.attend_time.Value;
                                                                    cal_OutDateTime = cal_endAttendance.attend_datetime.Value;
                                                                    if (checkLeave(CurretDate.Date, employee.employee_id, HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyShortDays)) == true)
                                                                    {
                                                                        cal_isLeave = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        cal_isNopay = true;
                                                                    }

                                                                }
                                                                #endregion

                                                                #region if out time between half day Attendance
                                                                else if (cal_endAttendance.attend_time.Value + ht_leaveTime >= cal_inShift.out_time)
                                                                {
                                                                    cal_earlyOutTime = cal_inShift.out_time.Value - cal_endAttendance.attend_time.Value;
                                                                    cal_isEveningHalfDay = true;
                                                                    cal_isEarlyOut = true;
                                                                    cal_outTime = cal_endAttendance.attend_time.Value;
                                                                    cal_OutDateTime = cal_endAttendance.attend_datetime.Value;
                                                                    if (checkLeave(CurretDate.Date, employee.employee_id, HelperClass.clsLeaves.GetLeaveOption(HelperClass.leaveoption.MonthlyHalfDays)) == true)
                                                                    {
                                                                        cal_isLeave = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        cal_isNopay = true;
                                                                    }
                                                                }
                                                                #endregion

                                                                #region other calculation
                                                                else
                                                                {
                                                                    isEveningInvalid = true;
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                        }
                        else
                        {
                            if (cal_startAll.Count >= 2)
                            {
                                cal_ot_startAttendancedata = cal_startAll.OrderByDescending(z => z.attend_datetime.GetValueOrDefault(DateTime.MaxValue)).Last();
                                cal_ot_endAttendance = cal_startAll.OrderByDescending(z => z.attend_datetime.GetValueOrDefault(DateTime.MinValue)).First();
                                if (cal_ot_startAttendancedata != null && cal_ot_endAttendance != null && cal_ot_startAttendancedata.attend_time != System.DateTime.MinValue.TimeOfDay && cal_ot_endAttendance.attend_time != System.DateTime.MinValue.TimeOfDay && cal_ot_startAttendancedata.attend_time !=null && cal_ot_endAttendance.attend_time !=null)
                                {
                                    cal_shiftCatagoryId = Guid.Empty;
                                    cal_isFreeDay = true;
                                    cal_inTime = cal_ot_startAttendancedata.attend_time.Value;
                                    cal_outTime = cal_ot_endAttendance.attend_time.Value;
                                    cal_attendDate = cal_ot_startAttendancedata.attend_date.Value.Date;
                                    cal_ot_freeDayWork = cal_ot_endAttendance.attend_time.Value - cal_ot_startAttendancedata.attend_time.Value;
                                    cal_isholidayWork = true;
                                }
                            }
                        }
                    }
                }
                //
                trns_DailyAttendanceProcess attendance = new trns_DailyAttendanceProcess();
                attendance.employee_id = cal_employeeId;
                attendance.attend_date = cal_attendDate;
                if (cal_inShift == null)
                {
                    attendance.shift_catagory_id = Guid.Empty;
                }
                else
                {
                    attendance.shift_catagory_id = cal_inShift.shift_category_id;
                }
                attendance.cal_dayid = cal_dayid;
                attendance.in_time = cal_inTime;
                attendance.out_time = cal_outTime;
                attendance.in_datetime = cal_inDateTime;
                attendance.out_datetime = cal_OutDateTime;
                attendance.early_in_time = cal_earlyInTime;
                attendance.late_in_time = cal_lateInTime;
                attendance.early_out_time = cal_earlyOutTime;
                attendance.late_out_time = cal_lateOutTime;
                attendance.is_late_in = cal_isLateIn;
                attendance.is_early_in = cal_isEarlyIn;
                attendance.total_work_time = cal_earlyInTime.Add(cal_lateOutTime);
                attendance.is_morning_gracein = cal_isMorningGraceAttend;
                attendance.is_evening_graceout = cal_isEviningGraceOut;
                attendance.is_late_out = cal_isLateOut;
                attendance.is_early_out = cal_isEarlyOut;
                attendance.is_morning_short_day = cal_isMorningShortDay;
                attendance.is_evining_short_day = cal_isEveningShortDay;
                attendance.is_morning_halfday = cal_isMorningHalfDay;
                attendance.is_evining_halfday = cal_isEveningHalfDay;
                attendance.is_leave = cal_isLeave;
                attendance.is_holiday = cal_isHoliday;
                attendance.is_absent = cal_isAbsent;
                attendance.is_nopay = cal_isNopay;
                attendance.is_free_day = cal_isFreeDay;
                attendance.is_fullday = cal_isFullDay;
                attendance.isholiday_work = cal_isholidayWork;
                attendance.isfreeday_work = cal_isFreedayWork;
                attendance.free_dayWork_time =cal_ot_freeDayWork;
                attendance.holiday_work_time = cal_ot_holidayWork;
                attendance.mercantile_holiday_work_time = cal_mecantile_work_time;
                attendance.isMorningInvalid = isMorningInvalid;
                attendance.isEveningInvalid = isEveningInvalid;
                EmployeeAttendance.Add(attendance);

            }

        public bool SaveDailyAttendanceList()
        {
            if (EmployeeAttendance.Count > 0)
            {
               
                    if (serviceClient.SaveEmployeeDailyAttendance(EmployeeAttendance.ToArray()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                
            }
            else
            {
                return false;
            }
        }

        public Guid getDayOfWeek(string dayOfWeek)
        {
            if (dayOfWeek == "Monday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Monday);
            }
            else if (dayOfWeek == "Tuesday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Tuesday);
            }
            else if (dayOfWeek == "Wednesday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Wednesday);
            }
            else if (dayOfWeek == "Thursday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Thursday);
            }
            else if (dayOfWeek == "Friday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Friday);
            }
            else if (dayOfWeek == "Saturday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Saturday);
            }
            else if (dayOfWeek == "Sunday")
            {
                return HelperClass.clsDays.GetDay(HelperClass.dayname.Sunday);
            }
            else
            {

                return Guid.Empty;

            }

        }

        public bool checkIsFreeday(string day, Guid shiftCatagory)
        {
            Guid d_id = getDayOfWeek(day);
            if (Shift != null)
            {
                dtl_Shift cal_inShift = Shift.FirstOrDefault(a => a.shift_in_day_id == d_id && a.shift_category_id == shiftCatagory);
                if (cal_inShift == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool checkLeave(DateTime date, Guid employeeid, Guid leaveType)
        {
            if (LeavePool != null)
            {
                trns_LeavePool CurrentLeavePoolList = LeavePool.FirstOrDefault(z => z.leave_date == date && z.leave_type_id == leaveType);
                if (CurrentLeavePoolList != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
