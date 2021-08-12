using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class AttendanceSummaryEmployee
    {
        #region Memeber Data

        int preSingleOt, preDoubleOt, preTripleOt, postSingleOt, postDoubleOt, postTripleOt;
        int pre_single_extra_ot, pre_double_extra_ot, pre_triple_extra_ot, post_single_extra_ot, post_double_extra_ot, post_triple_extra_ot;

        #endregion

        #region List Properties

        List<trns_ProcessedEmployeeAttendance> processedAttendance;
        public List<trns_ProcessedEmployeeAttendance> ProcessedAttendance
        {
            get { return processedAttendance; }
            set { processedAttendance = value; }
        }

        List<trns_ProcessedAttendanceStatus> empAttendanceStatusList;
        public List<trns_ProcessedAttendanceStatus> EmpAttendanceStatusList
        {
            get { return empAttendanceStatusList; }
            set { empAttendanceStatusList = value; }
        }

        List<trns_ProcessedEmployeeMaxOT> empMaxOtList;
        public List<trns_ProcessedEmployeeMaxOT> EmpMaxOtList
        {
            get { return empMaxOtList; }
            set { empMaxOtList = value; }
        }

        List<trns_ProcessedLeaveStatus> empLeaveStatusList;
        public List<trns_ProcessedLeaveStatus> EmpLeaveStatusList
        {
            get { return empLeaveStatusList; }
            set { empLeaveStatusList = value; }
        }

        #endregion

        #region Properties

        #region Employee Data

        Guid employeeID;
        public Guid EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        Guid summaryPeriod;
        public Guid SummaryPeriod
        {
            get { return summaryPeriod; }
            set { summaryPeriod = value; }
        }

        #endregion

        #region Time Format Properties

        #region Late/Early Out

        TimeSpan lateInTime;
        public TimeSpan LateInTime
        {
            get { return lateInTime; }
        }

        TimeSpan lateOutTime;
        public TimeSpan LateOutTime
        {
            get { return lateOutTime; }
        }

        TimeSpan earlyInTime;
        public TimeSpan EarlyInTime
        {
            get { return earlyInTime; }
        }

        TimeSpan earlyOutTime;
        public TimeSpan EarlyOutTime
        {
            get { return earlyOutTime; }
        } 

        #endregion

        #region Overtime

        TimeSpan singleOtTime;
        public TimeSpan SingleOtTime
        {
            get { return singleOtTime; }
        }

        TimeSpan doubleOtTime;
        public TimeSpan DoubleOtTime
        {
            get { return doubleOtTime; }
        }

        TimeSpan tripleOtTime;
        public TimeSpan TripleOtTime
        {
            get { return tripleOtTime; }
        }

        TimeSpan morningAllOtTime;
        public TimeSpan MorningAllOtTime
        {
            get { return morningAllOtTime; }
        }

        TimeSpan eveningAllOtTime;
        public TimeSpan EveningAllOtTime
        {
            get { return eveningAllOtTime; }
        }

        TimeSpan extraAllOtTime;
        public TimeSpan ExtraAllOtTime
        {
            get { return extraAllOtTime; }
        }

        #endregion

        #region Extra Overtime

        TimeSpan extraSingleOtTime;
        public TimeSpan ExtraSingleOtTime
        {
            get { return extraSingleOtTime; }
        }

        TimeSpan extraDoubleOtTime;
        public TimeSpan ExtraDoubleOtTime
        {
            get { return extraDoubleOtTime; }
        }

        TimeSpan extraTripleOtTime;
        public TimeSpan ExtraTripleOtTime
        {
            get { return extraTripleOtTime; }
        }

        #endregion

        TimeSpan freeDayWorkTime;
        public TimeSpan FreeDayWorkTime
        {
            get { return freeDayWorkTime; }
        }

        TimeSpan mercantileWorkTime;
        public TimeSpan MercantileWorkTime
        {
            get { return mercantileWorkTime; }
        }

        TimeSpan holidayWorkTime;
        public TimeSpan HolidayWorkTime
        {
            get { return holidayWorkTime; }
        }

        TimeSpan poyaWorkTime;
        public TimeSpan PoyaWorkTime
        {
            get { return poyaWorkTime; }
        }

        // h 2020-09-17
        private TimeSpan totalBreakTime;

        public TimeSpan TotalBreakTime
        {
            get { return totalBreakTime; }
            set { totalBreakTime = value; }
        }

        private TimeSpan totalBreakLateTime;

        public TimeSpan TotalBreakLateTime
        {
            get { return totalBreakLateTime; }
            set { totalBreakLateTime = value; }
        }

        #endregion

        decimal netWorkDayCount;
        public decimal NetWorkDayCount
        {
            get 
            {
                return
                    (decimal)
                (workingDayCount -
                ((morningShortLeaveCount + morningShortLeaveNopayCount) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_SL_VALUE)) -
                ((morningHalfDayLeaveCount + morningHalfDayNopayCount) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_HD_VALUE)) -
                ((eveningShortLeaveCount + eveningShortLeaveNopayCount) * clsLeave.GetLeaveTypeValue(leaveType.EVENING_SL_VALUE)) -
                ((eveningHalfdayLeaveCount + eveningHalfDayNopayCount) * clsLeave.GetLeaveTypeValue(leaveType.EVENING_HD_VALUE)));
            }
            
        }

        int invalidDayCount;
        public int InvalidDayCount
        {
            get { return invalidDayCount; }
            set { invalidDayCount = value; }
        }

        int absentDayCount;
        public int AbsentDayCount
        {
            get { return absentDayCount; }
            set { absentDayCount = value; }
        }

        int freeDayCount;
        public int FreeDayCount
        {
            get { return freeDayCount; }
            set { freeDayCount = value; }
        }

        int freeDayWorkCount;
        public int FreeDayWorkCount
        {
            get { return freeDayWorkCount; }
            set { freeDayWorkCount = value; }
        }

        int mercantileWorkCount;
        public int MercantileWorkCount
        {
            get { return mercantileWorkCount; }
            set { mercantileWorkCount = value; }
        }

        int workingDayCount;
        public int WorkingDayCount
        {
            get { return workingDayCount; }
            set { workingDayCount = value; }
        }

        //decimal _TeaDays;

        //public decimal TeaDays
        //{
        //    get { return _TeaDays; }
        //    set { _TeaDays = value; }
        //}

        //decimal _DustDays;

        //public decimal DustDays
        //{
        //    get { return _DustDays; }
        //    set { _DustDays = value; }
        //}

        double lateInDuration;
        public double LateInDuration
        {
            get { return lateInDuration; }
            set { lateInDuration = value; }
        }

        double lateOutDuration;
        public double LateOutDuration
        {
            get { return lateOutDuration; }
            set { lateOutDuration = value; }
        }

        double earlyInDuration;
        public double EarlyInDuration
        {
            get { return earlyInDuration; }
            set { earlyInDuration = value; }
        }

        double earlyOutDuration;
        public double EarlyOutDuration
        {
            get { return earlyOutDuration; }
            set { earlyOutDuration = value; }
        }

        decimal freeDayWorkDuration;
        public decimal FreeDayWorkDuration
        {
            get { return freeDayWorkDuration; }
            set { freeDayWorkDuration = value; }
        }

        decimal mercantileWorkDuration;
        public decimal MercantileWorkDuration
        {
            get { return mercantileWorkDuration; }
            set { mercantileWorkDuration = value; }
        }

        #region Leave Properties

        #region Morning Leave

        int morningShortLeaveCount;
        public int MorningShortLeaveCount
        {
            get { return morningShortLeaveCount; }
            set { morningShortLeaveCount = value; }
        }

        int morningShortLeaveNopayCount;
        public int MorningShortLeaveNopayCount
        {
            get { return morningShortLeaveNopayCount; }
            set { morningShortLeaveNopayCount = value; }
        }

        int morningHalfDayLeaveCount;
        public int MorningHalfDayLeaveCount
        {
            get { return morningHalfDayLeaveCount; }
            set { morningHalfDayLeaveCount = value; }
        }

        int morningHalfDayNopayCount;
        public int MorningHalfDayNopayCount
        {
            get { return morningHalfDayNopayCount; }
            set { morningHalfDayNopayCount = value; }
        }

        int morningFullDayLeaveCount;
        public int MorningFullDayLeaveCount
        {
            get { return morningFullDayLeaveCount; }
            set { morningFullDayLeaveCount = value; }
        }

        int morningFullDayNopayCount;
        public int MorningFullDayNopayCount
        {
            get { return morningFullDayNopayCount; }
            set { morningFullDayNopayCount = value; }
        } 

        #endregion

        #region Evening Leave

        int eveningShortLeaveCount;
        public int EveningShortLeaveCount
        {
            get { return eveningShortLeaveCount; }
            set { eveningShortLeaveCount = value; }
        }

        int eveningShortLeaveNopayCount;
        public int EveningShortLeaveNopayCount
        {
            get { return eveningShortLeaveNopayCount; }
            set { eveningShortLeaveNopayCount = value; }
        }

        int eveningHalfdayLeaveCount;
        public int EveningHalfdayLeaveCount
        {
            get { return eveningHalfdayLeaveCount; }
            set { eveningHalfdayLeaveCount = value; }
        }

        int eveningHalfDayNopayCount;
        public int EveningHalfDayNopayCount
        {
            get { return eveningHalfDayNopayCount; }
            set { eveningHalfDayNopayCount = value; }
        }

        int eveningFullDayLeaveCount;
        public int EveningFullDayLeaveCount
        {
            get { return eveningFullDayLeaveCount; }
            set { eveningFullDayLeaveCount = value; }
        }

        int eveningFullDayNopayCount;
        public int EveningFullDayNopayCount
        {
            get { return eveningFullDayNopayCount; }
            set { eveningFullDayNopayCount = value; }
        }

        #endregion

        #region Other Leave

        int morningShortLeaveAuthorizedCount;
        public int MorningShortLeaveAuthorizedCount
        {
            get { return morningShortLeaveAuthorizedCount; }
            set { morningShortLeaveAuthorizedCount = value; }
        }

        int morningHalfDayAuthorizedCount;
        public int MorningHalfDayAuthorizedCount
        {
            get { return morningHalfDayAuthorizedCount; }
            set { morningHalfDayAuthorizedCount = value; }
        }

        int morningFullDayAuthorizedCount;
        public int MorningFullDayAuthorizedCount
        {
            get { return morningFullDayAuthorizedCount; }
            set { morningFullDayAuthorizedCount = value; }
        }

        int eveningShortLeaveAuthorizedCount;
        public int EveningShortLeaveAuthorizedCount
        {
            get { return eveningShortLeaveAuthorizedCount; }
            set { eveningShortLeaveAuthorizedCount = value; }
        }

        int eveningHalfDayAuthorizedCount;
        public int EveningHalfDayAuthorizedCount
        {
            get { return eveningHalfDayAuthorizedCount; }
            set { eveningHalfDayAuthorizedCount = value; }
        }

        int eveningFullDayAuthorizedCount;
        public int EveningFullDayAuthorizedCount
        {
            get { return eveningFullDayAuthorizedCount; }
            set { eveningFullDayAuthorizedCount = value; }
        }


        #endregion

        #endregion

        #region Overtime Properties

        decimal actualOtInTime;
        public decimal ActualOtInTime
        {
            get { return actualOtInTime; }
            set { actualOtInTime = value; }
        }

        decimal actualOtOutTime;
        public decimal ActualOtOutTime
        {
            get { return actualOtOutTime; }
            set { actualOtOutTime = value; }
        }

        decimal extraOtHours;
        public decimal ExtraOtHours
        {
            get { return extraOtHours; }
            set { extraOtHours = value; }
        }


        #endregion

        #region Other

        decimal holidayWorkDuration;
        public decimal HolidayWorkDuration
        {
            get { return holidayWorkDuration; }
            set { holidayWorkDuration = value; }
        }

        decimal poyaWorkDuration;
        public decimal PoyaWorkDuration
        {
            get { return poyaWorkDuration; }
            set { poyaWorkDuration = value; }
        }

        decimal additionalDayCount;
        public decimal AdditionalDayCount
        {
            get { return additionalDayCount; }
            set { additionalDayCount = value; }
        }

        #endregion

        #endregion

        #region Methods

        public void CalculateEmployeeAttendanceSummary()
        {
            // Begins summarization of several parts of attendance related data
            if(processedAttendance != null && processedAttendance.Count > 0)
            {
                calculateEmployeeLeaveSummary();
                calculateEmployeeAttendSummary();
                calculateLateInAndEarlyOutSummary();
                calculateEmployeeOverTimeSummary();
            }
            else
            {
                this.setDefaultAttendanceSummaryData();
            }
            
        }

        void calculateEmployeeLeaveSummary()
        {
            if(empLeaveStatusList != null && empLeaveStatusList.Count > 0)
            {
                // obtained morning leaves
                var morning_SL = empLeaveStatusList.Where(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_SHORT_LEAVE));
                var morning_HD = empLeaveStatusList.Where(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_HALFDAY_LEAVE));
                var morning_FD = empLeaveStatusList.Where(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_FULL_LEAVE));

                // obtained evening leaves
                var evening_SL = empLeaveStatusList.Where(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_SHORT_LEAVE));
                var evening_HD = empLeaveStatusList.Where(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_HALFDAY_LEAVE));
                var evening_FD = empLeaveStatusList.Where(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_FULL_LEAVE));

                // find count of each morning leave type that employee has been obtained
                if(morning_SL != null)
                {
                    this.morningShortLeaveCount = morning_SL.Count(c => c.is_approved == true && c.is_official_leave == true);
                    this.morningShortLeaveNopayCount = morning_SL.Count(c => c.is_approved == false);
                }

                if(morning_HD != null)
                {
                    this.morningHalfDayLeaveCount = morning_HD.Count(c => c.is_approved == true && c.is_official_leave == true);
                    this.morningHalfDayNopayCount = morning_HD.Count(c => c.is_approved == false);
                }

                if(morning_FD != null)
                {
                    this.morningFullDayLeaveCount = morning_FD.Count(c => c.is_approved == true && c.is_official_leave == true);
                    this.morningFullDayNopayCount = morning_FD.Count(c => c.is_approved == false);
                }
                
                // find count of each evening leave type that employee has been obtained
                if(evening_SL != null)
                {
                    this.eveningShortLeaveCount = evening_SL.Count(c => c.is_approved == true && c.is_official_leave == true);
                    this.eveningShortLeaveNopayCount = evening_SL.Count(c => c.is_approved == false);
                }

                if(evening_HD != null)
                {
                    this.eveningHalfdayLeaveCount = evening_HD.Count(c => c.is_approved == true && c.is_official_leave == true);
                    this.eveningHalfDayNopayCount = evening_HD.Count(c => c.is_approved == false);
                }

                if(evening_FD != null)
                {
                    this.eveningFullDayLeaveCount = evening_FD.Count(c => c.is_approved == true && c.is_official_leave == true);
                    this.eveningFullDayNopayCount = evening_FD.Count(c => c.is_approved == false);
                }
            }
        }

        void calculateEmployeeAttendSummary()
        {
            if(empAttendanceStatusList != null)
            {
                this.absentDayCount += empAttendanceStatusList.Count(c => c.attend_status_id == AttendStatus.ABSENT);
                this.invalidDayCount = empAttendanceStatusList.Count(c => c.attend_status_id == AttendStatus.INVALID);
                foreach(var dayAttendance in processedAttendance)
                {
                    if(dayAttendance.trns_ProcessedAttendanceStatus.Any(c=>c.attend_status_id == AttendStatus.ATTEND) && !(dayAttendance.trns_ProcessedAttendanceStatus.Any(c=>c.attend_status_id == AttendStatus.MORNING_FULLDAY_UNAUTHORIZED || c.attend_status_id == AttendStatus.EVENING_FULLDAY_UNAUTHORIZED || c.attend_status_id == AttendStatus.HOLIDAY)))
                    {
                        this.workingDayCount++;
                    }
                }
                #region lakpohora
                //foreach (var dayAttendance in processedAttendance.OrderBy(c =>c.attend_date))
                //{
                //    TimeSpan InTime = (TimeSpan)dayAttendance.in_time.Value.TimeOfDay;
                //    TimeSpan OutTime = (TimeSpan)dayAttendance.out_time.Value.TimeOfDay;
                //    double WorkDuration = 0;
                //    TimeSpan ShiftOut = new TimeSpan(16, 15, 0);
                //    //if (dayAttendance.out_time.Value.Date == dayAttendance.in_time.Value.Date)
                //    //{
                //    //    if (OutTime <= ShiftOut)
                //    //    {
                //    //        WorkDuration = (OutTime - InTime).TotalSeconds;
                //    //        if (WorkDuration >= 12600 && WorkDuration <= 21600)
                //    //        {
                //    //            this._DustDays = this._DustDays + (decimal)0.5;
                //    //        }
                //    //        else if (WorkDuration > 21600)
                //    //        {
                //    //            this._DustDays = this._DustDays + (decimal)1;
                //    //        }
                //    //    }
                //    //    else
                //    //    {
                //    //        WorkDuration = (ShiftOut - InTime).TotalSeconds;
                //    //        if (WorkDuration >= 12600 && WorkDuration <= 21600)
                //    //        {
                //    //            this._DustDays = this._DustDays + (decimal)0.5;
                //    //        }
                //    //        else if (WorkDuration > 21600)
                //    //        {
                //    //            this._DustDays = this._DustDays + (decimal)1;
                //    //        }
                //    //    } 
                //    //}
                //    //else
                //    //{
                //    //    this._DustDays = this._DustDays + (decimal)1;
                //    //}
                //}
                ////this._TeaDays = this._DustDays;
                //foreach (var dayAttendance in processedAttendance.OrderBy(c => c.attend_date))
                //{
                //    TimeSpan InTime = (TimeSpan)dayAttendance.in_time.Value.TimeOfDay;
                //    TimeSpan OutTime = (TimeSpan)dayAttendance.out_time.Value.TimeOfDay;
                //    double WorkDuration = (OutTime - InTime).TotalSeconds;

                //    //if (dayAttendance.out_time.Value.Date == dayAttendance.in_time.Value.Date)
                //    //{
                //    //    if (dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.ATTEND_HALFDAY) && dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.HOLIDAY))
                //    //    {
                //    //        if (WorkDuration >= 12600 && WorkDuration <= 21600)
                //    //        {
                //    //            this._TeaDays = this._TeaDays - (decimal)0.5;
                //    //        }
                //    //        else if (WorkDuration > 21600)
                //    //        {
                //    //            this._TeaDays = this._TeaDays - (decimal)1;
                //    //        }
                //    //    }
                //    //    else if (dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.ATTEND) && dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.HOLIDAY))
                //    //    {
                //    //        if (WorkDuration >= 12600 && WorkDuration <= 21600)
                //    //        {
                //    //            this._TeaDays = this._TeaDays - (decimal)0.5;
                //    //        }
                //    //        else if (WorkDuration > 21600)
                //    //        {
                //    //            this._TeaDays = this._TeaDays - (decimal)1;
                //    //        }
                //    //    }
                //    //    else if (dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.ATTEND_HALFDAY) && dayAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Saturday)
                //    //    {
                //    //        if (WorkDuration > 21600)
                //    //        {
                //    //            this._TeaDays = this._TeaDays - (decimal)0.5;
                //    //        }
                //    //    } 
                //    //}
                //    //else
                //    //{
                //    //    if (dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.ATTEND_HALFDAY) && dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.HOLIDAY))
                //    //    {
                //    //        this._TeaDays = this._TeaDays - (decimal)1;
                //    //    }
                //    //    else if (dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.ATTEND) && dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.HOLIDAY))
                //    //    {
                //    //        this._TeaDays = this._TeaDays - (decimal)1;
                //    //    }
                //    //    else if (dayAttendance.trns_ProcessedAttendanceStatus.Any(c => c.attend_status_id == AttendStatus.ATTEND_HALFDAY) && dayAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Saturday)
                //    //    {
                //    //        this._TeaDays = this._TeaDays - (decimal)0.5;
                //    //    }
                //    //}
                //} 
                #endregion
            }
        }

        void calculateLateInAndEarlyOutSummary()
        {
            this.earlyInDuration = processedAttendance.Sum(c => c.ealry_in_duration.Value);
            this.lateInDuration = processedAttendance.Sum(c => c.late_in_duration.Value);
            this.earlyOutDuration = processedAttendance.Sum(c => c.early_out_duration.Value);
            this.lateOutDuration = processedAttendance.Sum(c => c.late_out_duration.Value);

            earlyInTime = TimeSpan.FromSeconds(this.earlyInDuration);
            lateInTime = TimeSpan.FromSeconds(this.lateInDuration);
            earlyOutTime = TimeSpan.FromSeconds(this.earlyOutDuration);
            lateOutTime = TimeSpan.FromSeconds(this.lateOutDuration);
        }

        void calculateEmployeeOverTimeSummary()
        {
            // find total of each type of legal overtime work duration of current employee

            preSingleOt = processedAttendance.Sum(c => c.pre_single_ot_duration.Value);
            preDoubleOt = processedAttendance.Sum(c => c.pre_double_ot_duration.Value);
            preTripleOt = processedAttendance.Sum(c => c.pre_triple_ot_duration.Value);

            postSingleOt = processedAttendance.Sum(c => c.post_single_ot_duration.Value);
            postDoubleOt = processedAttendance.Sum(c => c.post_double_ot_duration.Value);
            postTripleOt = processedAttendance.Sum(c => c.post_triple_ot_duration.Value);

            singleOtTime = TimeSpan.FromSeconds(preSingleOt + postSingleOt);
            doubleOtTime = TimeSpan.FromSeconds(preDoubleOt + postDoubleOt);
            tripleOtTime = TimeSpan.FromSeconds(preTripleOt + postTripleOt);

            morningAllOtTime = TimeSpan.FromSeconds(preSingleOt + preDoubleOt + preTripleOt);
            eveningAllOtTime = TimeSpan.FromSeconds(postSingleOt + postDoubleOt + postTripleOt);

            // if employee is done extra ot work then find total of each type of extra ot work duration of current employee
            if(empMaxOtList != null && empMaxOtList.Count > 0)
            {
                pre_single_extra_ot = empMaxOtList.Sum(c => c.pre_extra_single_ot.Value);
                pre_double_extra_ot = empMaxOtList.Sum(c => c.pre_extra_double_ot.Value);
                pre_triple_extra_ot = empMaxOtList.Sum(c => c.pre_extra_triple_ot.Value);

                post_single_extra_ot = empMaxOtList.Sum(c => c.post_extra_single_ot.Value);
                post_double_extra_ot = empMaxOtList.Sum(c => c.pre_extra_double_ot.Value);
                post_triple_extra_ot = empMaxOtList.Sum(c => c.post_extra_triple_ot.Value);


                extraSingleOtTime = TimeSpan.FromSeconds(pre_single_extra_ot + post_single_extra_ot);
                extraDoubleOtTime = TimeSpan.FromSeconds(pre_double_extra_ot + post_double_extra_ot);
                extraTripleOtTime = TimeSpan.FromSeconds(pre_triple_extra_ot + post_triple_extra_ot);

                extraAllOtTime = extraSingleOtTime + extraDoubleOtTime + extraTripleOtTime;
            }
            
        }

        void setDefaultAttendanceSummaryData()
        {
            invalidDayCount = 0;
            absentDayCount = 0;
            freeDayCount = 0;
            freeDayWorkCount = 0;
            mercantileWorkCount = 0;
            workingDayCount = 0;
            lateInDuration = 0;
            lateOutDuration = 0;
            earlyInDuration = 0;
            earlyOutDuration = 0;
            freeDayWorkDuration = 0;
            mercantileWorkDuration = 0;

            morningShortLeaveCount = 0;
            morningShortLeaveNopayCount = 0;
            morningShortLeaveAuthorizedCount = 0;

            morningHalfDayLeaveCount = 0;
            morningHalfDayNopayCount = 0;
            morningHalfDayAuthorizedCount = 0;

            eveningShortLeaveCount = 0;
            eveningShortLeaveNopayCount = 0;
            eveningShortLeaveAuthorizedCount = 0;

            eveningHalfdayLeaveCount = 0;
            eveningHalfDayNopayCount = 0;
            eveningHalfDayAuthorizedCount = 0;

            actualOtInTime = 0;
            actualOtOutTime = 0;
            extraOtHours = 0;

            holidayWorkDuration = 0;
            poyaWorkDuration = 0;
            additionalDayCount = 0;

        }

        #endregion
    }
}