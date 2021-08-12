using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttendanceData;
using System.Text;

namespace AttendanceOnlineService.HelperClasses
{
    public class AttendEmployee
    {
        #region Member Data

        #region Lists 

        #region Leaves

        List<EmployeeLeave> capturedLeaves = new List<EmployeeLeave>();
        List<trns_LeavePool> approveLeave = new List<trns_LeavePool>();
        List<z_LeaveType> availableLeaveTypes = new List<z_LeaveType>();

        #endregion

        #region Holidays

        List<z_HolidayData> assignedHolidays = new List<z_HolidayData>();
        List<EmployeeHoliday> capturedHolidays = new List<EmployeeHoliday>();

        #endregion

        #region Breaks

        List<EmployeeBreak> capturedBreaks = new List<EmployeeBreak>();
        List<dtl_AttendanceData> shift_break_attendance_list = new List<dtl_AttendanceData>();
        List<dtl_AttendanceData> free_break_attendance_list = new List<dtl_AttendanceData>();
        List<ShiftBreak> emp_valid_shift_breaks = new List<ShiftBreak>();
        List<ShiftBreak> emp_valid_free_breaks = new List<ShiftBreak>();

        #endregion

        List<trns_ProcessedAttendanceStatus> attendStatusList = new List<trns_ProcessedAttendanceStatus>();

        #endregion

        #region Variables



        #endregion

        #endregion

        #region Properties

        #region Employee Details

        Guid employeeID;
        public Guid EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        string empID;
        public string EmpID
        {
            get { return empID; }
            set { empID = value; }
        }

        string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        string secondName;
        public string SecondName
        {
            get { return secondName; }
            set { secondName = value; }
        }

        #endregion

        #region Employee Attendance Configuration

        List<dtl_AttendanceData> employeeAttendance;
        public List<dtl_AttendanceData> EmployeeAttendance
        {
            get { return employeeAttendance; }
            set { employeeAttendance = value; }
        }

        List<dtl_EmployeeAttendanceData> employeeSelectedAttendance;
        public List<dtl_EmployeeAttendanceData> EmployeeSelectedAttendance
        {
            get { return employeeSelectedAttendance; }
            set { employeeSelectedAttendance = value; }
        }

        public List<trns_ProcessedAttendanceStatus> AttendStatusList
        {
            get { return attendStatusList; }
        }

        ShiftConfiguration workShift;
        public ShiftConfiguration WorkShift
        {
            get { return workShift; }
            set { workShift = value; }
        }

        TimeSpan? inTime;
        public TimeSpan? InTime
        {
            get { return inTime; }
            set { inTime = value; }
        }

        TimeSpan? outTime;
        public TimeSpan? OutTime
        {
            get { return outTime; }
            set { outTime = value; }
        }

        DateTime? thumbIn;
        public DateTime? ThumbIn
        {
            get { return thumbIn; }
            set { thumbIn = value; }
        }

        DateTime? thumbOut;
        public DateTime? ThumbOut
        {
            get { return thumbOut; }
            set { thumbOut = value; }
        }

        DateTime? actualIn;
        public DateTime? ActualIn
        {
            get { return actualIn; }
            set { actualIn = value; }
        }

        DateTime? actualOut;
        public DateTime? ActualOut
        {
            get { return actualOut; }
            set { actualOut = value; }
        }

        #endregion

        #region Employee Leave Configuration

        public List<trns_LeavePool> ApproveLeave
        {
            get { return approveLeave; }
            set { approveLeave = value; }
        }

        public List<EmployeeLeave> CapturedLeaves
        {
            get { return capturedLeaves; }
            set { capturedLeaves = value; }
        }

        public List<z_LeaveType> AvailableLeaveTypes
        {
            get { return availableLeaveTypes; }
            set { availableLeaveTypes = value; }
        }

        #endregion

        #region Employee Holiday Configuration

        public List<z_HolidayData> AssignedHolidays
        {
            get { return assignedHolidays; }
            set { assignedHolidays = value; }
        }

        public List<EmployeeHoliday> CapturedHolidays
        {
            get { return capturedHolidays; }
            set { capturedHolidays = value; }
        }

        #endregion

        #region Employee OverTime Configuration

        EmployeeOverTime workOT;
        public EmployeeOverTime WorkOT
        {
            get { return workOT; }
            set { workOT = value; }
        }

        trns_EmployeeMaxOTDetails maxOt;
        public trns_EmployeeMaxOTDetails MaxOt
        {
            get { return maxOt; }
            set { maxOt = value; }
        }

        bool hasMorningMaxOt;
        public bool HasMorningMaxOt
        {
            get { return hasMorningMaxOt; }
            set { hasMorningMaxOt = value; }
        }

        bool hasEveningMaxOt;
        public bool HasEveningMaxOt
        {
            get { return hasEveningMaxOt; }
            set { hasEveningMaxOt = value; }
        }

        #endregion

        #region Employee Break Configuration

        public List<EmployeeBreak> CapturedBreaks
        {
            get { return this.capturedBreaks; }
            set { this.capturedBreaks = value; }
        }

        bool hasShiftBreak;
        public bool HasShiftBreak
        {
            get { return hasShiftBreak; }
            set { hasShiftBreak = value; }
        }

        bool hasFreeBreak;
        public bool HasFreeBreak
        {
            get { return hasFreeBreak; }
            set { hasFreeBreak = value; }
        }

        bool hasNoBreak;
        public bool HasNoBreak
        {
            get { return hasNoBreak; }
            set { hasNoBreak = value; }
        }

        #endregion

        /*private bool _IsOTApplicable;

        public bool IsOTApplicable
        {
            get { return _IsOTApplicable; }
            set { _IsOTApplicable = value; }
        }*/

        #region Employee Actual Work

        #region Early In Work

        DateTime workEarlyInTime;
        public DateTime WorkEarlyInTime
        {
            get { return workEarlyInTime; }
            set { workEarlyInTime = value; }
        }

        int workEarlyInDuration;
        public int WorkEarlyInDuration
        {
            get { return workEarlyInDuration; }
            set { workEarlyInDuration = value; }
        }

        #endregion

        #region Late In Work

        DateTime workLateInTime;
        public DateTime WorkLateInTime
        {
            get { return workLateInTime; }
            set { workLateInTime = value; }
        }

        int workLateInDuration;
        public int WorkLateInDuration
        {
            get { return workLateInDuration; }
            set { workLateInDuration = value; }
        }

        DateTime workGraceInTime;
        public DateTime WorkGraceInTime
        {
            get { return workGraceInTime; }
            set { workGraceInTime = value; }
        }

        int workGraceInDuration;
        public int WorkGraceInDuration
        {
            get { return workGraceInDuration; }
            set { workGraceInDuration = value; }
        }

        int totalLateWorkDurationFromShiftIn;
        public int TotalLateWorkDurationFromShiftIn
        {
            get { return totalLateWorkDurationFromShiftIn; }
            set { totalLateWorkDurationFromShiftIn = value; }
        }

        #endregion

        #region Early Out Work

        DateTime workEearlyOutTime;
        public DateTime WorkEearlyOutTime
        {
            get { return workEearlyOutTime; }
            set { workEearlyOutTime = value; }
        }

        int workEarlyOutDuration;
        public int WorkEarlyOutDuration
        {
            get { return workEarlyOutDuration; }
            set { workEarlyOutDuration = value; }
        }

        DateTime workGraceOutTime;
        public DateTime WorkGraceOutTime
        {
            get { return workGraceOutTime; }
            set { workGraceOutTime = value; }
        }

        int workGraceOutDuration;
        public int WorkGraceOutDuration
        {
            get { return workGraceOutDuration; }
            set { workGraceOutDuration = value; }
        }

        int totalEarlyOutWorkDurationFromShiftOut;
        public int TotalEarlyOutWorkDurationFromShiftOut
        {
            get { return totalEarlyOutWorkDurationFromShiftOut; }
            set { totalEarlyOutWorkDurationFromShiftOut = value; }
        }

        #endregion

        #region Late Out Work

        DateTime workLateOutTime;
        public DateTime WorkLateOutTime
        {
            get { return workLateOutTime; }
            set { workLateOutTime = value; }
        }

        int workLateOutDuration;
        public int WorkLateOutDuration
        {
            get { return workLateOutDuration; }
            set { workLateOutDuration = value; }
        }

        #endregion

        #region Short Leave Work

        #region Morning 

        DateTime morningShortLeaveInTime;
        public DateTime MorningShortLeaveInTime
        {
            get { return morningShortLeaveInTime; }
            set { morningShortLeaveInTime = value; }
        }

        int morningShortLeaveDuration;
        public int MorningShortLeaveDuration
        {
            get { return morningShortLeaveDuration; }
            set { morningShortLeaveDuration = value; }
        }

        #endregion

        #region Evening

        DateTime eveningShortLeaveOutTime;
        public DateTime EveningShortLeaveOutTime
        {
            get { return eveningShortLeaveOutTime; }
            set { eveningShortLeaveOutTime = value; }
        }

        int eveningShortLeaveDuration;
        public int EveningShortLeaveDuration
        {
            get { return eveningShortLeaveDuration; }
            set { eveningShortLeaveDuration = value; }
        }

        #endregion

        #endregion

        #region Half Day Leave Work

        #region Morning

        DateTime morningHalfDayLeaveInTime;
        public DateTime MorningHalfDayLeaveInTime
        {
            get { return morningHalfDayLeaveInTime; }
            set { morningHalfDayLeaveInTime = value; }
        }

        int morningHalfDayLeaveDuration;
        public int MorningHalfDayLeaveDuration
        {
            get { return morningHalfDayLeaveDuration; }
            set { morningHalfDayLeaveDuration = value; }
        }

        #endregion

        #region Evening 

        DateTime eveningHalfDayLeaveOutTime;
        public DateTime EveningHalfDayLeaveOutTime
        {
            get { return eveningHalfDayLeaveOutTime; }
            set { eveningHalfDayLeaveOutTime = value; }
        }

        int eveningHalfDayLeaveDuration;
        public int EveningHalfDayLeaveDuration
        {
            get { return eveningHalfDayLeaveDuration; }
            set { eveningHalfDayLeaveDuration = value; }
        }

        #endregion

        #endregion

        #region Full Day

        #region Morning

        DateTime morningFullDayLeaveInTime;
        public DateTime MorningFullDayLeaveInTime
        {
            get { return morningFullDayLeaveInTime; }
            set { morningFullDayLeaveInTime = value; }
        }

        int morningFullDayLeaveDuration;
        public int MorningFullDayLeaveDuration
        {
            get { return morningFullDayLeaveDuration; }
            set { morningFullDayLeaveDuration = value; }
        }

        #endregion

        #region Evening

        DateTime eveningFullDayLeaveOutTime;
        public DateTime EveningFullDayLeaveOutTime
        {
            get { return eveningFullDayLeaveOutTime; }
            set { eveningFullDayLeaveOutTime = value; }
        }

        int eveningFullDayLeaveOutDuration;
        public int EveningFullDayLeaveOutDuration
        {
            get { return eveningFullDayLeaveOutDuration; }
            set { eveningFullDayLeaveOutDuration = value; }
        }

        #endregion

        #endregion

        #region Total Work

        int totalWorkDuration;
        public int TotalWorkDuration
        {
            get { return totalWorkDuration; }
            set { totalWorkDuration = value; }
        }

        int totalMinimumWorkDuration;
        public int TotalMinimumWorkDuration
        {
            get { return totalMinimumWorkDuration; }
            set { totalMinimumWorkDuration = value; }
        }

        // In case of extra ot work
        int actualTotalWorkDuration;
        public int ActualTotalWorkDuration
        {
            get { return actualTotalWorkDuration; }
            set { actualTotalWorkDuration = value; }
        }

        #endregion

        #region MCN Late Covering Work

        bool hasLateCovering;
        public bool HasLateCovering
        {
            get { return hasLateCovering; }
            set { hasLateCovering = value; }
        }

        bool isLateCoverDone;
        public bool IsLateCoverDone
        {
            get { return isLateCoverDone; }
            set { isLateCoverDone = value; }
        }

        #endregion

        #endregion

        #region Employee Split Shift Work

        bool hasSplitBreakMorningOT;
        public bool HasSplitBreakMorningOT
        {
            get { return hasSplitBreakMorningOT; }
            set { hasSplitBreakMorningOT = value; }
        }

        bool hasSplitBreakEveningOT;
        public bool HasSplitBreakEveningOT
        {
            get { return hasSplitBreakEveningOT; }
            set { hasSplitBreakEveningOT = value; }
        }

        #endregion

        #region Attendance Status

        bool isShiftAssigned;
        public bool IsShiftAssigned
        {
            get { return isShiftAssigned; }
            set { isShiftAssigned = value; }
        }

        bool isAttendanceOk;
        public bool IsAttendanceOk
        {
            get { return isAttendanceOk; }
            set { isAttendanceOk = value; }
        }

        bool isAttendanceInvalid;
        public bool IsAttendanceInvalid
        {
            get { return isAttendanceInvalid; }
            set { isAttendanceInvalid = value; }
        }

        bool isAttendanceAbsent;
        public bool IsAttendanceAbsent
        {
            get { return isAttendanceAbsent; }
            set { isAttendanceAbsent = value; }
        }

        bool isEarlyIn;
        public bool IsEarlyIn
        {
            get { return isEarlyIn; }
            set { isEarlyIn = value; }
        }

        bool isLateIn;
        public bool IsLateIn
        {
            get { return isLateIn; }
            set { isLateIn = value; }
        }

        bool isEarlyOut;
        public bool IsEarlyOut
        {
            get { return isEarlyOut; }
            set { isEarlyOut = value; }
        }

        bool isLateOut;
        public bool IsLateOut
        {
            get { return isLateOut; }
            set { isLateOut = value; }
        }

        bool isGraceIn;
        public bool IsGraceIn
        {
            get { return isGraceIn; }
            set { isGraceIn = value; }
        }

        bool isGraceOut;
        public bool IsGraceOut
        {
            get { return isGraceOut; }
            set { isGraceOut = value; }
        }

        #region Short Leave Status

        bool isMorningShortLeave;
        public bool IsMorningShortLeave
        {
            get { return isMorningShortLeave; }
            set { isMorningShortLeave = value; }
        }

        bool isEveningShortLeave;
        public bool IsEveningShortLeave
        {
            get { return isEveningShortLeave; }
            set { isEveningShortLeave = value; }
        }

        #endregion

        #region Halfday Leave Status

        bool isMorningHalfDayLeave;
        public bool IsMorningHalfDayLeave
        {
            get { return isMorningHalfDayLeave; }
            set { isMorningHalfDayLeave = value; }
        }

        bool isEveningHalfDayLeave;
        public bool IsEveningHalfDayLeave
        {
            get { return isEveningHalfDayLeave; }
            set { isEveningHalfDayLeave = value; }
        }

        #endregion

        #region Fullday Leave Status

        bool isMorningFullDayLeave;
        public bool IsMorningFullDayLeave
        {
            get { return isMorningFullDayLeave; }
            set { isMorningFullDayLeave = value; }
        }

        bool isEveningFullDayLeave;
        public bool IsEveningFullDayLeave
        {
            get { return isEveningFullDayLeave; }
            set { isEveningFullDayLeave = value; }
        }

        #endregion

        #endregion

        #region Handled Leave Status

        #region Short Leaves

        bool isMorningShortLeaveHandled;
        public bool IsMorningShortLeaveHandled
        {
            get { return isMorningShortLeaveHandled; }
            set { isMorningShortLeaveHandled = value; }
        }

        bool isEveningShortLeaveHandled;
        public bool IsEveningShortLeaveHandled
        {
            get { return isEveningShortLeaveHandled; }
            set { isEveningShortLeaveHandled = value; }
        }

        #endregion

        #region Halfday Leaves

        bool isMorningHalfDayLeaveHandled;
        public bool IsMorningHalfDayLeaveHandled
        {
            get { return isMorningHalfDayLeaveHandled; }
            set { isMorningHalfDayLeaveHandled = value; }
        }

        bool isEveningHalfDayLeaveHandled;
        public bool IsEveningHalfDayLeaveHandled
        {
            get { return isEveningHalfDayLeaveHandled; }
            set { isEveningHalfDayLeaveHandled = value; }
        }

        #endregion

        #region Fullday Leaves

        bool isMorningFulldayLeaveHandled;
        public bool IsMorningFulldayLeaveHandled
        {
            get { return isMorningFulldayLeaveHandled; }
            set { isMorningFulldayLeaveHandled = value; }
        }

        bool isEveningFulldayLeaveHandled;
        public bool IsEveningFulldayLeaveHandled
        {
            get { return isEveningFulldayLeaveHandled; }
            set { isEveningFulldayLeaveHandled = value; }
        }

        #endregion

        #region Absent Leaves

        bool isAbsentLeaveHandled;
        public bool IsAbsentLeaveHandled
        {
            get { return isAbsentLeaveHandled; }
            set { isAbsentLeaveHandled = value; }
        }

        #endregion

        #endregion

        #region Handled Holiday Status

        bool isHoliday;
        public bool IsHoliday
        {
            get { return isHoliday; }
            set { isHoliday = value; }
        }

        #endregion

        #region Random Thumb-In/ Thumb-Out

        Random rndTimeGenerator;
        public Random RndTimeGenerator
        {
            set { rndTimeGenerator = value; }
        }

        #endregion

        //h 2019-05-24
        #region late

        private int _LateCount;

        public int LateCount
        {
            get { return _LateCount; }
            set { _LateCount = value; }
        }

        private int _ShortLeaveCount;

        public int ShortLeaveCount
        {
            get { return _ShortLeaveCount; }
            set { _ShortLeaveCount = value; }
        }

        private bool isLateORSL;

        public bool IsLateORSL
        {
            get { return isLateORSL; }
            set { isLateORSL = value; }
        }

        #endregion

        // m 2021-06-16
        private bool morningIncentive;

        public bool MorningIncentive
        {
            get { return morningIncentive; }
            set { morningIncentive = value; }
        }

        private bool specialLateDeduction;

        public bool SpecialLateDeduction
        {
            get { return specialLateDeduction; }
            set { specialLateDeduction = value; }
        }

        #endregion

        #region Work Time Calculation Methods

        #region Early In Calculation

        void earlyInCalculation()
        {
            if (thumbIn < this.workShift.ShiftIn)
            {
                TimeSpan earlyIn = this.workShift.ShiftIn.Subtract((DateTime)thumbIn);
                workEarlyInTime = (DateTime)thumbIn;
                workEarlyInDuration = Convert.ToInt32(earlyIn.TotalSeconds);
                isEarlyIn = true;
            }
        }

        #endregion

        #region Grace In Calculation

        void graceInCalculation()
        {
            if (!isEarlyIn)
            {
                if (thumbIn > this.workShift.ShiftIn && thumbIn < this.workShift.LateGraceEndTime)
                {
                    TimeSpan graceIn = thumbIn.Value.Subtract(this.workShift.ShiftIn);
                    workGraceInDuration = (int)graceIn.TotalSeconds;
                    workGraceInTime = thumbIn.Value;
                    isGraceIn = true;
                    // m 2021-06-16
                    if (workShift.EmployeeShift.is_mor_inc == true)
                        morningIncentive = true;
                }
            }
        }

        #endregion

        #region Late In Calculation

        void lateInCalculation()
        {
            if (!isGraceIn)
            {
                if (thumbIn > this.workShift.LateGraceEndTime)
                {
                    TimeSpan lateIn = ((DateTime)thumbIn).Subtract(this.workShift.LateGraceEffectEndTime);
                    workLateInDuration = (int)lateIn.TotalSeconds;
                    workLateInTime = (DateTime)thumbIn;
                    isLateIn = true;
                    isLateORSL = true; // h 2020-09-24 
                    #region MCN MultipleOT
                    if (this.workShift.EmployeeShift.is_ot_shift == true && (this.workShift.EmployeeShift.is_single == true || this.workShift.EmployeeShift.is_multiple == true))
                    {
                        this.MultipleOT(lateIn.TotalSeconds);
                        workLateInDuration = 0;
                        isLateIn = false;
                    }
                    #endregion
                    // m 2021-06-16
                    if (workShift.EmployeeShift.is_late_deduc == true)
                        specialLateDeduction = true;
                }
            }

        }

        #endregion

        #region Early Out Calculation

        void earlyOutCalculation()
        {
            if (thumbOut < this.workShift.EarlyGraceStartTime && !this.workShift.IsOpenShift)
            {
                TimeSpan earlyOut = this.workShift.EarlyGraceEffectStartTime.Subtract((DateTime)thumbOut);
                workEarlyOutDuration = (int)earlyOut.TotalSeconds;
                workEearlyOutTime = (DateTime)thumbOut;
                isEarlyOut = true;
            }
            else if (this.workShift.IsOpenShift && this.workShift.OpenShiftMinWordDuration > 0)
            {

                if (this.workShift.OpenShiftMinWordDuration - totalWorkDuration > 0)
                {
                    workEarlyOutDuration = (int)(this.workShift.OpenShiftMinWordDuration - totalWorkDuration);
                    workEearlyOutTime = (DateTime)thumbOut;
                    isEarlyOut = true;
                }
            }
        }

        #endregion

        #region Grace Out Calculate

        void graceOutCalculation()
        {
            if (!isEarlyOut)
            {
                if (thumbOut < this.workShift.ShiftOut && thumbOut > this.workShift.EarlyGraceStartTime)
                {
                    TimeSpan graceOut = this.workShift.ShiftOut.Subtract(thumbOut.Value);
                    workGraceOutDuration = (int)graceOut.TotalSeconds;
                    workGraceOutTime = thumbOut.Value;
                    isGraceOut = true;
                }
            }
        }

        #endregion

        #region Late Out Calculation

        void lateOutCalculation()
        {
            if (!isGraceOut)
            {
                if (thumbOut > this.workShift.ShiftOut && !this.workShift.IsOpenShift)
                {

                    TimeSpan lateOut = ((DateTime)thumbOut).Subtract(this.workShift.ShiftOut);
                    workLateOutTime = (DateTime)thumbOut;
                    workLateOutDuration = (int)lateOut.TotalSeconds;
                    isLateOut = true;
                }
                else if (this.workShift.IsOpenShift && this.workShift.OpenShiftMinWordDuration > 0)
                {
                    if (totalWorkDuration - this.workShift.OpenShiftMinWordDuration > 0)
                    {
                        workLateOutTime = (DateTime)thumbOut;
                        workLateOutDuration = (int)(totalWorkDuration - this.workShift.OpenShiftMinWordDuration);
                        isLateOut = true;
                    }
                }
            }
        }

        #endregion

        #region Morning Leave Calculation

        #region Morning Short Leave

        void morningShortLeaveCalculate()
        {
            if (isLateIn)        //  There is a chance to employee could be entitled for morning short leave
            {
                if (thumbIn > this.workShift.LateInEndTime && thumbIn <= this.workShift.LateShortLeaveEndTime) // employee first Thumb In during shift's short leave configuration
                {
                    this.morningShortLeaveInTime = (DateTime)thumbIn;
                    TimeSpan shortLeaveDuration = this.morningShortLeaveInTime.Subtract(this.workShift.ShiftIn);
                    this.morningShortLeaveDuration = (int)shortLeaveDuration.TotalSeconds;
                    this.isMorningShortLeave = true;
                    //this.workLateInDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_SHORT_LEAVE)));
                    isLateORSL = true; // h 2020-09-24
                }
            }
        }

        #endregion

        #region Morning Halfday Leave

        void morningHalfdayLeaveCalculate()
        {
            if (isLateIn) // employee could be entitled for morning halfday leave
            {
                if (thumbIn > this.workShift.LateShortLeaveEndTime && thumbIn <= this.workShift.LateHalfDayEndTime) // employee first Thumb In during shift's halfday leave configuration
                {
                    this.morningHalfDayLeaveInTime = (DateTime)thumbIn;
                    TimeSpan halfdayLeaveDuration = this.morningHalfDayLeaveInTime.Subtract(this.workShift.ShiftIn);
                    this.morningHalfDayLeaveDuration = (int)halfdayLeaveDuration.TotalSeconds;
                    this.isMorningHalfDayLeave = true;
                    //this.workLateInDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_HALFDAY_LEAVE)));
                    isLateORSL = false; // h 2020-09-24
                }
            }
        }

        #endregion

        #region Morning Fullday Leave

        void morningFulldayLeaveCalculate()
        {
            if (isLateIn)        // employee could be entitled for morning full day leave
            {
                if (thumbIn > this.workShift.LateHalfDayEndTime)
                {
                    //this.morningFullDayLeaveInTime = (DateTime)thumbIn;
                    //TimeSpan fulldayLeaveDuration = morningFullDayLeaveInTime.Subtract(this.workShift.ShiftIn);
                    //this.morningFullDayLeaveDuration = (int)fulldayLeaveDuration.TotalSeconds;
                    //this.isMorningFullDayLeave = true;
                    ////this.workLateInDuration = 0;
                    //this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_FULL_LEAVE)));
                    isLateORSL = false; // h 2020-09-24
                }
            }
        }

        #endregion

        #endregion

        #region Evening Leave Calculation

        #region Evening Short Leave

        void eveningShortLeaveCalculate()
        {
            if (isEarlyOut)      // employee could be entitled for evening short leave
            {
                if (thumbOut < this.workShift.EarlyOutStartTime && thumbOut >= this.workShift.EarlyShortLeaveStartTime)    // last Thumb Out is between shift's early out short leave configuration
                {
                    this.eveningShortLeaveOutTime = (DateTime)thumbOut;
                    TimeSpan shortLeaveDuration = this.workShift.ShiftOut.Subtract(this.eveningShortLeaveOutTime);
                    this.eveningShortLeaveDuration = (int)shortLeaveDuration.TotalSeconds;
                    this.isEveningShortLeave = true;
                    //this.workEarlyOutDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_SHORT_LEAVE)));
                    isLateORSL = true; // h 2020-09-24
                }
            }
        }

        void openEveningShortLeaveCalculate()
        {
            if (isEarlyOut && !isEveningFullDayLeave && !isEveningHalfDayLeave && this.workShift.OpenShiftShotLeaveTimeDuration > 0)
            {
                if (this.workShift.OpenShiftShotLeaveTimeDuration <= workEarlyOutDuration)
                {

                    this.eveningShortLeaveDuration = (int)workEarlyOutDuration;
                    this.isEveningShortLeave = true;
                    //this.workEarlyOutDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_SHORT_LEAVE)));
                }
            }
        }

        #endregion

        #region Evening Halfday Leave

        void eveningHalfDayLeaveCalculate()
        {
            if (isEarlyOut)      // employee could be entitled for ealry out half day leave
            {
                if (thumbOut < this.workShift.EarlyShortLeaveStartTime && thumbOut >= this.workShift.EarlyHalfDayStartTime)   // employee last Thumb Out is between shift's early half day leave configuration
                {
                    this.eveningHalfDayLeaveOutTime = (DateTime)thumbOut;
                    TimeSpan halfdayLeaveDuration = this.workShift.ShiftOut.Subtract(this.eveningHalfDayLeaveOutTime);
                    this.eveningHalfDayLeaveDuration = (int)halfdayLeaveDuration.TotalSeconds;
                    this.isEveningHalfDayLeave = true;
                    //this.workEarlyOutDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_HALFDAY_LEAVE)));
                    isLateORSL = false; // h 2020-09-24
                }
            }
        }

        void openEveningHalfDayLeaveCalculate()
        {
            if (isEarlyOut && !isEveningFullDayLeave && this.workShift.OpenShiftHalfDayTimeDuration > 0)
            {
                if (this.workShift.OpenShiftHalfDayTimeDuration <= workEarlyOutDuration)
                {

                    this.eveningHalfDayLeaveDuration = (int)workEarlyOutDuration;
                    this.isEveningHalfDayLeave = true;
                    //this.workEarlyOutDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_HALFDAY_LEAVE)));
                }
            }
        }

        #endregion

        #region Evening Fullday Leave

        void eveningFullDayLeaveCalculate()
        {
            if (isEarlyOut)          // employee could be entitled for ealry out full day leave
            {
                if (thumbOut < this.workShift.EarlyHalfDayStartTime)
                {
                    this.eveningFullDayLeaveOutTime = (DateTime)thumbOut;
                    TimeSpan fulldayLeaveDuration = this.workShift.ShiftOut.Subtract(this.eveningFullDayLeaveOutTime);
                    this.eveningFullDayLeaveOutDuration = (int)fulldayLeaveDuration.TotalSeconds;
                    this.isEveningFullDayLeave = true;
                    //this.workEarlyOutDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_FULL_LEAVE)));
                    isLateORSL = false; // h 2020-09-24
                }
            }
        }

        void openShiftEveningFullDayLeaveCalculate()
        {
            if (isEarlyOut && this.workShift.OpenShiftFullDayTimeDuration > 0)          // employee could be entitled for ealry out full day leave
            {
                if (this.workShift.OpenShiftFullDayTimeDuration <= workEarlyOutDuration)
                {

                    this.eveningFullDayLeaveOutDuration = (int)workEarlyOutDuration;
                    this.isEveningFullDayLeave = true;
                    //this.workEarlyOutDuration = 0;
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_FULL_LEAVE)));
                }
            }
        }

        #endregion

        #endregion

        #region Late In/Early Out Approval - Dinemore

        private void findLateInApproval()
        {
            if (isLateIn)
            {
                // Still Late-In means that No leaves obtained for morning and Employee is late to work
                // Check if employee's late is approved via manager using Un-Official leave
                EmployeeLeave empLateLeave = new EmployeeLeave();
                if (capturedLeaves.Count > 0)
                {
                    // Employee might be obtained evening leaves for the assigned shift
                    // So if any remaining approved leaves are existing then check for Un-Official leave that covering employee late
                    List<trns_LeavePool> remainApplyLeaves = this.approveLeave.Where(c => !this.capturedLeaves.Where(d => d.ApplyLeave != null || d.HasCoveringLeaves).Any(g => g.ApplyLeave.pool_id == c.pool_id || g.CoveringLeaves.Any(x => x.pool_id == c.pool_id))).ToList();
                    var currentLeave = remainApplyLeaves.FirstOrDefault(c => c.shift_detail_id == this.workShift.EmployeeShift.shift_detail_id && c.z_LeaveCategory.is_official == false);
                    if (currentLeave != null)
                    {
                        // Employee is approved for Un-official leave
                        empLateLeave.ApplyLeave = currentLeave;
                        empLateLeave.IsAuthorized = true;
                        capturedLeaves.Add(empLateLeave);
                        isLateIn = false;
                        workLateInDuration = 0;
                    }
                }
                else
                {
                    // Employee not obtained any leaves for the assigned shift
                    if (approveLeave.Count > 0)
                    {
                        var currentLeave = approveLeave.FirstOrDefault(c => c.shift_detail_id == this.workShift.EmployeeShift.shift_detail_id && c.z_LeaveCategory.is_official == false);
                        if (currentLeave != null)
                        {
                            // Employee is approved for Un-official leave
                            empLateLeave.ApplyLeave = currentLeave;
                            empLateLeave.IsAuthorized = true;
                            capturedLeaves.Add(empLateLeave);
                            isLateIn = false;
                            workLateInDuration = 0;
                        }
                    }
                }
            }
        }

        private void findEarlyOutApproval()
        {
            if (isEarlyOut)
            {
                // Still Early-Out means that No leaves obtained for evening and Employee is early out from work
                // Check if employee's early out is approved via manager using Un-Official leave
                EmployeeLeave empEarlyLeave = new EmployeeLeave();
                if (capturedLeaves.Count > 0)
                {
                    // Employee might be obtained morning leaves for the assigned shift
                    // So if any remaining approved leaves are existing then check for Un-Official leave that covering employee early out
                    List<trns_LeavePool> remainApplyLeaves = this.approveLeave.Where(c => !this.capturedLeaves.Where(d => d.ApplyLeave != null || d.HasCoveringLeaves).Any(g => g.ApplyLeave.pool_id == c.pool_id || g.CoveringLeaves.Any(x => x.pool_id == c.pool_id))).ToList();
                    var currentLeave = remainApplyLeaves.FirstOrDefault(c => c.shift_detail_id == this.workShift.EmployeeShift.shift_detail_id && c.z_LeaveCategory.is_official == false);
                    if (currentLeave != null)
                    {
                        // Employee is approved for Un-official leave
                        empEarlyLeave.ApplyLeave = currentLeave;
                        empEarlyLeave.IsAuthorized = true;
                        capturedLeaves.Add(empEarlyLeave);
                        isEarlyOut = false;
                        workEarlyOutDuration = 0;
                    }
                }
                else
                {
                    // Employee not obtained any leaves for the assigned shift
                    if (approveLeave.Count > 0)
                    {
                        var currentLeave = approveLeave.FirstOrDefault(c => c.shift_detail_id == this.workShift.EmployeeShift.shift_detail_id && c.z_LeaveCategory.is_official == false);
                        if (currentLeave != null)
                        {
                            // Employee is approved for Un-official leave
                            empEarlyLeave.ApplyLeave = currentLeave;
                            empEarlyLeave.IsAuthorized = true;
                            capturedLeaves.Add(empEarlyLeave);
                            isEarlyOut = false;
                            workEarlyOutDuration = 0;
                        }
                    }
                }
            }
        }

        #endregion

        #region MCN Late-In Covering

        private void captureLateCovering()
        {
            // If employee has late-in attendance then he is allowed for cover certain period of time from evening work time
            // to avoid late-in attendance for the day.
            if (this.workShift.IsLateCoverAllowed && isLateIn)
            {
                // Employee has late-in attendance 
                // Check whether employee is within allowed maximum covering period of time
                if (this.thumbIn.Value <= this.workShift.AllowedLateCoverTime)
                {
                    // Employee late-in attendance was during allowed late-cover time period
                    this.hasLateCovering = true;
                    if (!this.workShift.IsFixedLateCover)
                    {
                        this.workShift.LateCoverEndTime = this.workShift.LateCoverStartTime.Add(TimeSpan.FromSeconds(this.workLateInDuration));
                    }

                    #region SLS

                    // Check whether employee has done covering work
                    if (this.thumbOut.Value >= this.workShift.LateCoverEndTime)
                    {
                        // Employee Thumb-Out was after late-cover end time so he is done covering for late
                        this.isLateCoverDone = true;
                        this.isLateIn = false;
                        this.workLateInDuration = 0;

                    }
                    #endregion

                    //check employee has done covering work in Between LateCoverInTime LateCoverEndTime
                    //if ((int)this.thumbIn.Value.Subtract(this.workShift.ShiftIn).TotalSeconds <= (int)this.workShift.LateCoverEndTime.Subtract(this.workShift.LateCoverStartTime).TotalSeconds)
                    //{
                    //    this.isLateCoverDone = true;
                    //    this.isLateIn = false;
                    //    this.workLateInDuration = 0;
                    //}

                    //check employee has done covering work in Between LateCoverInTime LateCoverEndTime
                    else if ((int)this.thumbIn.Value.Subtract(this.workShift.ShiftIn).TotalSeconds <= (int)this.workShift.LateCoverEndTime.Subtract(this.workShift.LateCoverStartTime).TotalSeconds)
                    {
                        this.isLateCoverDone = false;
                        this.isLateIn = true;
                        //this.workLateInDuration = 0;
                    }
                }
            }
        }

        #endregion

        #region Over Time Calculation

        void overTimeCalculation()
        {
            workOT = new EmployeeOverTime();
            workOT.CalculateOverTime(this, workLateOutDuration);
        }

        #endregion

        #region Total Work Calculation

        void totalWorkTimeCalculation()
        {
            DateTime empMinimumIn, empMinimumOut;
            DateTime emp_thumb_in, emp_thumb_out;
            if (hasMorningMaxOt)
            {
                emp_thumb_in = this.actualIn.Value;
            }
            else
            {
                emp_thumb_in = this.thumbIn.Value;
            }

            if (hasEveningMaxOt)
            {
                emp_thumb_out = this.thumbOut.Value;
            }
            else
            {
                emp_thumb_out = this.thumbOut.Value;
            }

            totalWorkDuration = (int)thumbOut.Value.Subtract(thumbIn.Value).TotalSeconds;

            // h 2020-09-15 get this duration as deduction of break times
            //actualTotalWorkDuration = (int)emp_thumb_out.Subtract(emp_thumb_in).TotalSeconds;

            //// check whether employee obtained any type of break-times that could be deducted from total work time
            //if (capturedBreaks.Count > 0)
            //{
            //    foreach (var empBreak in capturedBreaks)
            //    {
            //        actualTotalWorkDuration -= empBreak.BreakDuration;
            //    }
            //}

            if (thumbIn.Value < this.workShift.ShiftIn)
                empMinimumIn = this.workShift.ShiftIn;
            else
                empMinimumIn = thumbIn.Value;

            if (thumbOut.Value < this.workShift.ShiftOut)
                empMinimumOut = thumbOut.Value;
            else
                empMinimumOut = this.workShift.ShiftOut;

            totalMinimumWorkDuration = (int)empMinimumOut.Subtract(empMinimumIn).TotalSeconds;
        }

        #endregion

        #endregion

        #region h Late & Sl calculation

        private void LateSLCalculation()
        {
            if (isLateORSL)
            {
                if (isLateIn || isMorningShortLeave)
                {
                    if (LateCount < 2)
                    {
                        workLateInDuration = 0;
                        isLateIn = false;
                        LateCount++;
                    }
                    if (ShortLeaveCount < 2)
                    {
                        MorningShortLeaveDuration = 0;
                        IsMorningShortLeave = false;
                        EmployeeLeave existing = capturedLeaves.FirstOrDefault(c => c.LeaveType == availableLeaveTypes.FirstOrDefault(d => d.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_SHORT_LEAVE)));
                        if (existing != null)
                        {
                            capturedLeaves.Remove(existing);
                        }
                        ShortLeaveCount++;
                    }
                }
                else if (isEarlyOut || isEveningShortLeave)
                {
                    if (LateCount < 2)
                    {
                        workEarlyOutDuration = 0;
                        isEarlyOut = false;
                        LateCount++;
                    }
                    if (ShortLeaveCount < 2)
                    {
                        eveningShortLeaveDuration = 0;
                        isEveningShortLeave = false;
                        EmployeeLeave existing = capturedLeaves.FirstOrDefault(c => c.LeaveType == availableLeaveTypes.FirstOrDefault(d => d.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_SHORT_LEAVE)));
                        if (existing != null)
                        {
                            capturedLeaves.Remove(existing);
                        }
                        ShortLeaveCount++;
                    }
                }
            }
        }

        #endregion

        #region Work Calculation Initializing Methods

        public void BeginWorkCalculation()
        {
            if (isAttendanceOk)
            {
                this.SetEmployeeMaxOT();
                this.CaptureEmployeeBreak();
                this.totalWorkTimeCalculation();

                if (!this.workShift.IsOpenShift)
                {
                    this.earlyInCalculation();
                    this.graceInCalculation();
                    this.lateInCalculation();
                }
                this.earlyOutCalculation();
                if (!this.workShift.IsOpenShift)
                {
                    this.graceOutCalculation();
                }

                this.lateOutCalculation();

                this.setWorkLateInAndEarlyOutDuration();
                this.captureLateCovering();

                if (!this.workShift.IsOpenShift)
                {
                    this.morningShortLeaveCalculate();
                    this.morningHalfdayLeaveCalculate();
                    this.morningFulldayLeaveCalculate();
                    this.eveningShortLeaveCalculate();
                    this.eveningHalfDayLeaveCalculate();
                    this.eveningFullDayLeaveCalculate();
                }

                if (this.workShift.IsOpenShift && this.workShift.IsOpenShiftLate && workEarlyOutDuration > 0)
                {
                    openShiftEveningFullDayLeaveCalculate();
                    openEveningHalfDayLeaveCalculate();
                    openEveningShortLeaveCalculate();
                }
                //this.findLateInApproval();
                //this.findEarlyOutApproval();
                this.overTimeCalculation();
            }

        }

        private void setWorkLateInAndEarlyOutDuration()
        {
            if (isGraceIn || isLateIn)
            {
                totalLateWorkDurationFromShiftIn = (int)thumbIn.Value.Subtract(this.workShift.ShiftIn).TotalSeconds;
            }
            else if (isGraceOut || isEarlyOut)
            {
                totalEarlyOutWorkDurationFromShiftOut = (int)this.workShift.ShiftOut.Subtract(thumbOut.Value).TotalSeconds;
            }

        }

        #endregion

        #region Employee Absent Methods

        void CheckAbsentEmployee(trns_EmployeeDailyShiftDetails CalendarShift)
        {
            if (assignedHolidays.Count > 0)
            {
                isHoliday = true;
                foreach (var item in assignedHolidays)
                {
                    this.captureEmployeeHoliday();
                }
            }
            else
            {
                isAttendanceAbsent = true;
                if (CalendarShift.date.DayOfWeek == DayOfWeek.Saturday && workShift.EmployeeShift.is_halfday == true)
                {
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_HALFDAY_LEAVE)));
                }
                else
                    this.CaptureEmployeeLeave(availableLeaveTypes.FirstOrDefault(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_FULL_LEAVE)));
            }
            this.thumbIn = this.workShift.CurrentDate.Value.Date.Add(new TimeSpan(0));
            this.thumbOut = this.workShift.CurrentDate.Value.Date.Add(new TimeSpan(0));
            this.workOT = new EmployeeOverTime();
        }

        #endregion

        #region Leave Calculation Intializing Methods

        void CaptureEmployeeLeave(z_LeaveType typeOfLeave)
        {
            EmployeeLeave empLeave = new EmployeeLeave();
            empLeave.LeaveType = typeOfLeave;
            capturedLeaves.Add(empLeave);
        }

        public void BeginLeaveCalculation()
        {
            if (capturedLeaves.Count > 0)
            {
                capturedLeaves.ForEach(c => c.CheckEmployeeLeave(this));
            }
        }

        #endregion

        #region Holiday Calculation Initializing Methods

        void initializeHolidayAttendance(DateTime shiftDate)
        {
            if (!isShiftAssigned)
            {
                this.ThumbIn = shiftDate.Date.Add(new TimeSpan(0));
                this.ThumbOut = shiftDate.Date.Add(new TimeSpan(0));
                this.WorkShift = new ShiftConfiguration();
                this.WorkShift.CurrentDate = shiftDate;
                this.WorkShift.EmployeeShift = new ShiftDetailAllView();
                this.WorkOT = new EmployeeOverTime();
            }
        }

        void captureEmployeeHoliday()
        {
            EmployeeHoliday holiday = new EmployeeHoliday();
            capturedHolidays.Add(holiday);
        }

        public void BeginHolidayCalculation(DateTime shiftDate)
        {
            if (assignedHolidays.Count > 0)
            {
                if (isAttendanceOk)
                {
                    foreach (var item in assignedHolidays)
                    {
                        this.captureEmployeeHoliday();
                    }
                }

                if (!isShiftAssigned)
                {
                    if (assignedHolidays.Count > 0)
                    {
                        isHoliday = true;
                        foreach (var item in assignedHolidays)
                        {
                            this.captureEmployeeHoliday();
                        }
                    }
                }
                if (capturedHolidays.Count > 0)
                    capturedHolidays.ForEach(c => c.CheckEmployeeHoliday(this));

                this.initializeHolidayAttendance(shiftDate);
            }
        }

        #endregion

        #region Break Calculation Initializing Methods

        void CaptureEmployeeBreak()
        {
            if ((hasShiftBreak || hasFreeBreak) && isAttendanceOk)
            {
                // Breaks captured are depend on emp's Thumb-In & Thumb-Out. But in a case of max-ot Thumb-In & Thumb-Out may be system defined ones.
                // So when capturing breaks we have to consider actual In & Out of employee

                // Use these values to filter out emp's break-times
                DateTime emp_thumb_in;
                DateTime emp_thumb_out;

                // Checking whether emp entitled for morning extra-ot
                if (hasMorningMaxOt)
                {
                    emp_thumb_in = this.actualIn.Value;
                }
                else
                {
                    emp_thumb_in = this.thumbIn.Value;
                }

                // Checking whether emp entitled for evening extra-ot
                if (hasEveningMaxOt)
                {
                    emp_thumb_out = this.actualOut.Value;
                }
                else
                {
                    emp_thumb_out = this.thumbOut.Value;
                }

                // Getting employee all thumbs except Thumb-In and Thumb-Out. There may no thumbs could be found
                List<dtl_AttendanceData> empAttendanceBreakList = this.employeeAttendance.Where(c => c.attend_datetime > emp_thumb_in && c.attend_datetime < emp_thumb_out).OrderBy(c => c.attend_datetime.Value).ToList();
                empAttendanceBreakList = empAttendanceBreakList.Select(c => new dtl_AttendanceData { attend_datetime = breakTimeRoundUp(c.attend_datetime.Value, new TimeSpan(0, 1, 0)), attend_date = c.attend_date, attend_time = c.attend_time, emp_id = c.emp_id, attendance_data_id = c.attendance_data_id }).ToList();
                var processedBreaks = empAttendanceBreakList.Select((c, index) => new { Idx = index, DATA_ID = c.attendance_data_id, A_DATETIME = c.attend_datetime, A_DATE = c.attend_date }).OrderBy(c => c.Idx).ToList();

                // Consecutive thumb prints could be reside on range of 1 minute or less and therefore not recognized as a break duration
                for (int i = 0; i < processedBreaks.Count(); i++)
                {
                    var currentThumb = processedBreaks.FirstOrDefault(c => c.Idx == i);
                    var nextThumb = processedBreaks.FirstOrDefault(c => c.Idx == i + 1);
                    if (nextThumb != null)
                    {
                        if (nextThumb.A_DATETIME.Value.Subtract(currentThumb.A_DATETIME.Value).TotalSeconds <= 60)
                        {
                            // Duration between break-time is less than or equal to 1 minute. So then ignore the selected first thumb leaving other thumb 
                            // is compared with next adjacent thumb
                            empAttendanceBreakList.Remove(empAttendanceBreakList.FirstOrDefault(c => c.attendance_data_id == currentThumb.DATA_ID));
                        }
                    }
                    else
                    {
                        // reached to the final break-Thumb (currentThumb) which is one before to Thumb-Out
                        if (emp_thumb_out.Subtract(currentThumb.A_DATETIME.Value).TotalSeconds <= 60)
                        {
                            // Duration between last break-thumb and Emp's Thumb-Out is less than or equal to 1 minute. So then ignore the last break-thumb 
                            empAttendanceBreakList.Remove(empAttendanceBreakList.FirstOrDefault(c => c.attendance_data_id == currentThumb.DATA_ID));
                        }
                    }
                }

                if (hasShiftBreak)
                {
                    // Employee is assigned for shift-related break option
                    // Getting shift-related breaks for the current employee

                    /* Selection logic:
                     * shift-related break could be within emp's Thumb-In and Thumb-Out of working shift
                     * emp could've been Sign-In within a shift-related break
                     * emp could've been Sign-out within a shift-related break
                     * 
                     * */
                    if (this.workShift.AssignedBreakList.Count > 0) // If not true, then there are no shift-breaks are defined in the shift though emp is assigned for shift-relatd break option
                    {
                        emp_valid_shift_breaks = this.workShift.AssignedBreakList.Where(c => (c.ShiftBreakOn > emp_thumb_in && c.ShiftBreakOff < emp_thumb_out) || (emp_thumb_in > c.ShiftBreakOn && emp_thumb_in < c.ShiftBreakOff) || (emp_thumb_out > c.ShiftBreakOn && emp_thumb_out < c.ShiftBreakOff)).ToList();
                        shift_break_attendance_list = empAttendanceBreakList.Where(c => emp_valid_shift_breaks.Any(d => d.ShiftBreakOn <= c.attend_datetime.Value && d.ShiftBreakOff >= c.attend_datetime.Value)).ToList();
                    }
                }

                if (hasFreeBreak)
                {
                    // Employee is assigned for free-break option
                    // free break is considered as any consecutive two thumbs

                    /* Selection logic:
                     * 1 - If at least one shift-related break  is obtained by employee then consecutive thumbs within shift-related break period should be excluded
                     * 2 - Else consecutive thumbs are taken as break-in and break-out
                     * 
                     */
                    if (emp_valid_shift_breaks.Count > 0)
                    {
                        // emp is on both shift-break option and free-break option
                        // excluding consecutive thumbs within emp obtained shift-related break periods
                        free_break_attendance_list = empAttendanceBreakList.Where(c => !emp_valid_shift_breaks.Any(d => c.attend_datetime.Value > d.ShiftBreakOn && c.attend_datetime.Value < d.ShiftBreakOff)).ToList();

                        // Because of emp's work schedule is sperated by shift-related breaks there are free-break time zones along his work schedule
                        // eg: Since In time to Break-On time of first shift-related break. 
                        // When capturing free breaks these zones should be considered if not total break time could exceeds the total work time.
                        // Those free-break zones could also be represented as un-official shift-related breaks for this particular scenario
                        List<ShiftBreak> validBreaks = emp_valid_shift_breaks.OrderBy(c => c.ShiftBreakOn).ToList();
                        if (validBreaks.Count > 0)
                        {
                            ShiftBreak firstValidBreak = validBreaks.FirstOrDefault();
                            ShiftBreak lastValidBreak = validBreaks.LastOrDefault();
                            if (firstValidBreak.ShiftBreakOn > emp_thumb_in)
                            {
                                // free-break zone between emp In and first of shift-related break
                                ShiftBreak freeBreakZone = new ShiftBreak(null);
                                freeBreakZone.ShiftBreakOn = emp_thumb_in;
                                freeBreakZone.ShiftBreakOff = firstValidBreak.ShiftBreakOn;
                                freeBreakZone.IsFreeBreakZone = true;
                                freeBreakZone.IsFirstFreeBreakZone = true;
                                emp_valid_free_breaks.Add(freeBreakZone);
                            }

                            if (lastValidBreak.ShiftBreakOff < emp_thumb_out)
                            {
                                // free-break zone between emp Out and last of shift-related break
                                ShiftBreak freeBreakZone = new ShiftBreak(null);
                                freeBreakZone.ShiftBreakOn = lastValidBreak.ShiftBreakOff;
                                freeBreakZone.ShiftBreakOff = emp_thumb_out;
                                freeBreakZone.IsFreeBreakZone = true;
                                freeBreakZone.IsLastFreeBreakZone = true;
                                emp_valid_free_breaks.Add(freeBreakZone);
                            }

                            if (validBreaks.Count > 1)
                            {
                                // Ignore first and last bcoz they are already considered
                                validBreaks.Remove(firstValidBreak);
                                validBreaks.Remove(lastValidBreak);

                                // Consider remaining breaks
                                ShiftBreak current = firstValidBreak, next;
                                for (int i = 0; i < validBreaks.Count; i++)
                                {
                                    next = validBreaks[i];
                                    emp_valid_free_breaks.Add(new ShiftBreak(null) { ShiftBreakOn = current.ShiftBreakOff, ShiftBreakOff = next.ShiftBreakOn, IsFreeBreakZone = true });
                                    current = next;
                                }
                            }
                        }
                    }
                    else
                    {
                        // getting consecutive thumbs as free-break option
                        free_break_attendance_list = empAttendanceBreakList.Where(c => c.attend_datetime.Value > emp_thumb_in && c.attend_datetime.Value < emp_thumb_out).ToList();
                    }
                }


                // looking for employee obtained break times
                this.captureShiftBreaks(emp_thumb_in, emp_thumb_out);
                this.captureFreeBreaks(emp_thumb_in, emp_thumb_out);

            }
        }

        void captureShiftBreaks(DateTime empThumbIn, DateTime empThumbOut)
        {
            if (emp_valid_shift_breaks.Count > 0)
            {
                foreach (ShiftBreak assignedBreak in emp_valid_shift_breaks)
                {
                    EmployeeBreak empBreak = new EmployeeBreak();
                    empBreak.IsShiftBreak = true;
                    empBreak.IsBreakInvalid = true;
                    if (shift_break_attendance_list.Count > 0)
                    {
                        var breakThumbs = shift_break_attendance_list.Where(c => c.attend_datetime.Value >= assignedBreak.ShiftBreakOn && c.attend_datetime.Value <= assignedBreak.ShiftBreakOff).OrderBy(c => c.attend_datetime.Value);
                        if (breakThumbs.Count() >= 2)
                        {
                            // At least two thumb-prints are found so we can assume first and last would be the break-in and break-out
                            // valid shift-related break time
                            empBreak.BreakIn = breakThumbs.FirstOrDefault().attend_datetime.Value;
                            empBreak.BreakOut = breakThumbs.LastOrDefault().attend_datetime.Value;
                            empBreak.EmpBreakAttendance = breakThumbs.ToList();
                            empBreak.IsBreakInvalid = false;

                        }
                        else if (breakThumbs.Count() == 0)
                        {
                            // No thumb-prints are found and there could be three possibilities. 
                            // 1 - Employee has not obtained a break, though he has assigned a break
                            // 2 - Employee forgot to mark break-in and break-out thumbs
                            // 3 - Employee's Emp-In or Emp-Out could be during the break-time which in advance excluded from break-attendance thumb list
                            // 1 and 2 scenarios are un-desirable and should be deducted from employee, but scenario 3 have to be analyzed

                            // assume that employee is in 1 st or 2nd scenario
                            empBreak.BreakIn = assignedBreak.ShiftBreakOn;
                            empBreak.BreakOut = assignedBreak.ShiftBreakOff;

                            if (empThumbIn > assignedBreak.ShiftBreakOn && empThumbIn < assignedBreak.ShiftBreakOff)
                            {
                                // Emp-In occurred during the break-time
                                empBreak.BreakIn = empThumbIn;
                                empBreak.BreakOut = assignedBreak.ShiftBreakOff;
                                empBreak.HasEmpInBreak = true;
                            }

                            if (empThumbOut > assignedBreak.ShiftBreakOn && empThumbOut < assignedBreak.ShiftBreakOff)
                            {
                                // Emp-Out occurred during the break-time
                                empBreak.BreakIn = assignedBreak.ShiftBreakOn;
                                empBreak.BreakOut = empThumbOut;
                                empBreak.HasEmpOutBreak = true;
                            }
                        }
                        else if (breakThumbs.Count() == 1)
                        {
                            // Only one break-thumb is found during shift-related break
                            empBreak.EmpBreakAttendance = breakThumbs.ToList();
                            empBreak.HasOnlyOneBreakThumb = true;
                            if (breakThumbs.FirstOrDefault().attend_datetime.Value > assignedBreak.ShiftBreakOn && breakThumbs.FirstOrDefault().attend_datetime.Value < assignedBreak.ShiftBreakOff)
                            {
                                // Emp-In occurred during the break-time
                                empBreak.BreakIn = breakThumbs.FirstOrDefault().attend_datetime.Value;
                                empBreak.BreakOut = assignedBreak.ShiftBreakOff;
                                empBreak.HasEmpInBreak = true;
                            }
                        }

                    }
                    //Change MCN 2016-04-27 For No Break include in attendance list
                    else if (shift_break_attendance_list.Count == 0 && hasShiftBreak)
                    {
                        empBreak.BreakIn = assignedBreak.ShiftBreakOn;
                        empBreak.BreakOut = assignedBreak.ShiftBreakOff;

                        if (empThumbIn > assignedBreak.ShiftBreakOn && empThumbIn < assignedBreak.ShiftBreakOff)
                        {
                            // Emp-In occurred during the break-time
                            empBreak.BreakIn = empThumbIn;
                            empBreak.BreakOut = assignedBreak.ShiftBreakOff;
                            empBreak.HasEmpInBreak = true;
                        }

                        if (empThumbOut > assignedBreak.ShiftBreakOn && empThumbOut < assignedBreak.ShiftBreakOff)
                        {
                            // Emp-Out occurred during the break-time
                            empBreak.BreakIn = assignedBreak.ShiftBreakOn;
                            empBreak.BreakOut = empThumbOut;
                            empBreak.HasEmpOutBreak = true;
                        }

                    }

                    empBreak.EmpAssignedBreak = assignedBreak;
                    capturedBreaks.Add(empBreak);
                }
            }
        }

        void captureFreeBreaks(DateTime empThumbIn, DateTime empThumbOut)
        {
            if (free_break_attendance_list.Count > 0)
            {
                // free break option is on & thumbs are identified
                bool isInvalidFreeBreak = false;
                if (emp_valid_free_breaks.Count == 0)
                {
                    // only free break is assigned to employee
                    if (free_break_attendance_list.Count() % 2 > 0)
                    {
                        // Invalid number of break thumbs and missing one is considered to be emp's Thumb-Out
                        free_break_attendance_list.Add(new dtl_AttendanceData { attendance_data_id = Guid.Empty, emp_id = this.empID, attend_datetime = empThumbOut, attend_date = empThumbOut.Date });
                        isInvalidFreeBreak = true;
                    }
                }
                else
                {
                    // employee is assigned for both shift-break and free-break and should be consider free-break zones
                    if (workShift.IsSplitShift)
                    {
                        bool bothZonesInvalid = false;
                        bool isOneThumbShiftBreakHandled = false;
                        bool isOnlyThumbShiftBreak = false;

                        // split shift only having one shift related break
                        // so only two free break zones would be allocated

                        ShiftBreak firstBreakZone = emp_valid_free_breaks.FirstOrDefault(c => c.IsLastFreeBreakZone == true);
                        ShiftBreak lastBreakZone = emp_valid_free_breaks.FirstOrDefault(c => c.IsLastFreeBreakZone == true);
                        var oneThumbShiftBreak = capturedBreaks.FirstOrDefault(c => c.HasOnlyOneBreakThumb == true);
                        if (oneThumbShiftBreak != null)
                        {
                            isOnlyThumbShiftBreak = true;
                        }

                        var firstBreakZoneThumbs = free_break_attendance_list.Where(c => c.attend_datetime > firstBreakZone.ShiftBreakOn && c.attend_datetime < firstBreakZone.ShiftBreakOff);
                        var lastBreakZoneThumbs = free_break_attendance_list.Where(c => c.attend_datetime > lastBreakZone.ShiftBreakOn && c.attend_datetime < lastBreakZone.ShiftBreakOff);

                        if (isOnlyThumbShiftBreak)
                        {
                            if ((firstBreakZoneThumbs.Count() % 2) > 0 && (lastBreakZoneThumbs.Count() % 2) > 0)
                            {
                                // Both free-break zones are invalid. So consider it for hasOnlyOneBreakThumb shift-break
                                bothZonesInvalid = true;
                            }
                        }

                        foreach (ShiftBreak freeBreakZone in emp_valid_free_breaks.OrderBy(c => c.ShiftBreakOn))
                        {
                            var breakThumbList = free_break_attendance_list.Where(c => c.attend_datetime > freeBreakZone.ShiftBreakOn && c.attend_datetime < freeBreakZone.ShiftBreakOff);
                            if (breakThumbList != null && breakThumbList.Count() > 0)
                            {
                                if (breakThumbList.Count() % 2 > 0)
                                {
                                    // Invalid number of break thumbs are found in free-break zone. 
                                    if (!bothZonesInvalid && isOnlyThumbShiftBreak && !isOneThumbShiftBreakHandled)
                                    {
                                        if (freeBreakZone.IsFirstFreeBreakZone)
                                        {
                                            // last thumb of invalid thumbs is considered as sign-in to split and only thumb of shift-related break 
                                            // is considered as sign-out from split back to work
                                            // So from sign-in to  split-on is consider as one free break and split-on to sign-out is consider as split-break

                                            oneThumbShiftBreak.BreakIn = freeBreakZone.ShiftBreakOff;
                                            oneThumbShiftBreak.BreakOut = oneThumbShiftBreak.EmpBreakAttendance.FirstOrDefault().attend_datetime.Value;
                                            oneThumbShiftBreak.IsBreakInvalid = false;
                                            isOneThumbShiftBreakHandled = true;
                                        }
                                        else if (freeBreakZone.IsLastFreeBreakZone)
                                        {
                                            // first thumb of invalid thumbs is considered as sign-out from split back to work and only thumb of shift-related break 
                                            // is considered as sign-in to split
                                            // So from sign-out to  split-off is consider as one free break and split-off to sign-in is consider as split-break

                                            oneThumbShiftBreak.BreakIn = oneThumbShiftBreak.EmpBreakAttendance.FirstOrDefault().attend_datetime.Value;
                                            oneThumbShiftBreak.BreakOut = freeBreakZone.ShiftBreakOn;
                                            oneThumbShiftBreak.IsBreakInvalid = false;
                                            isOneThumbShiftBreakHandled = true;
                                        }
                                    }

                                    if (freeBreakZone.IsFirstFreeBreakZone)
                                    {
                                        // split-on time is added as missing one if more than 1 thumb is found
                                        free_break_attendance_list.Add(new dtl_AttendanceData { attendance_data_id = Guid.Empty, emp_id = this.empID, attend_datetime = freeBreakZone.ShiftBreakOff, attend_date = freeBreakZone.ShiftBreakOff.Date });
                                    }
                                    else if (freeBreakZone.IsLastFreeBreakZone)
                                    {
                                        // split-off time is added as missing one if more than 1 thumb is found
                                        free_break_attendance_list.Add(new dtl_AttendanceData { attendance_data_id = Guid.Empty, emp_id = this.empID, attend_datetime = freeBreakZone.ShiftBreakOn, attend_date = freeBreakZone.ShiftBreakOn.Date });
                                    }

                                    isInvalidFreeBreak = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        foreach (ShiftBreak freeBreakZone in emp_valid_free_breaks.OrderBy(c => c.ShiftBreakOn))
                        {
                            var breakThumbList = free_break_attendance_list.Where(c => c.attend_datetime > freeBreakZone.ShiftBreakOn && c.attend_datetime < freeBreakZone.ShiftBreakOff);
                            if (breakThumbList != null && breakThumbList.Count() > 0)
                            {
                                if (breakThumbList.Count() % 2 > 0)
                                {
                                    // Invalid number of break thumbs are found in free-break zone. 
                                    if (breakThumbList.Count() > 1)
                                    {
                                        // Break-Off time of zone is added as missing one if more than 1 thumb is found
                                        free_break_attendance_list.Add(new dtl_AttendanceData { attendance_data_id = Guid.Empty, emp_id = this.empID, attend_datetime = freeBreakZone.ShiftBreakOff, attend_date = freeBreakZone.ShiftBreakOff.Date });
                                    }
                                    else
                                    {
                                        // If one thumb is found then missing one is added as greater deduction is occurred.
                                        var break_thumb = breakThumbList.FirstOrDefault();
                                        if (break_thumb.attend_datetime.Value.Subtract(freeBreakZone.ShiftBreakOn).TotalSeconds > freeBreakZone.ShiftBreakOff.Subtract(break_thumb.attend_datetime.Value).TotalSeconds)
                                        {
                                            // thumb is far more from break start than break end
                                            free_break_attendance_list.Add(new dtl_AttendanceData { attendance_data_id = Guid.Empty, emp_id = this.empID, attend_datetime = freeBreakZone.ShiftBreakOn, attend_date = freeBreakZone.ShiftBreakOn.Date });
                                        }
                                        else
                                        {
                                            // thumb is far more from break end than break start
                                            free_break_attendance_list.Add(new dtl_AttendanceData { attendance_data_id = Guid.Empty, emp_id = this.empID, attend_datetime = freeBreakZone.ShiftBreakOff, attend_date = freeBreakZone.ShiftBreakOff.Date });
                                        }
                                    }

                                    isInvalidFreeBreak = true;
                                }
                            }
                        }
                    }
                }
                var freeBreakProcessedBreaks = free_break_attendance_list.Select((c, index) => new { Idx = index, DATA_ID = c.attendance_data_id, A_DATETIME = c.attend_datetime, A_DATE = c.attend_date }).OrderBy(c => c.A_DATETIME.Value).OrderBy(c => c.Idx);
                for (int i = 0; i < freeBreakProcessedBreaks.Count(); i += 2)
                {
                    // capturing pair of consecutive thumbs as break-time
                    DateTime break_in = freeBreakProcessedBreaks.FirstOrDefault(c => c.Idx == i).A_DATETIME.Value;
                    DateTime break_out = freeBreakProcessedBreaks.FirstOrDefault(c => c.Idx == i + 1).A_DATETIME.Value;
                    EmployeeBreak empBreak = new EmployeeBreak();
                    empBreak.IsFreeBreak = true;
                    empBreak.BreakIn = break_in;
                    empBreak.BreakOut = break_out;
                    empBreak.IsBreakInvalid = isInvalidFreeBreak;
                    capturedBreaks.Add(empBreak);
                }
            }
        }

        public void BeginEmployeeBreakCalculation()
        {
            if (capturedBreaks.Count > 0)
            {
                // there are break-times which employee is obtained
                foreach (EmployeeBreak captureBreak in capturedBreaks)
                {
                    // analyze each break-time with emp's work schedule
                    captureBreak.CheckEmployeeBreak(this);
                }
            }
        }

        #region Dinemore split-shift implementation

        /* split-shift is represented by usual shift with one and only shift-related break.
         * This break is used to split the shift between morning part and evening part.
         * break-on time is the split starting time and break-off time is the split ending time
         * break-in time could be used to represent over-time eligible limit if employee is worked from morning part to time somewhere during split
         * break-out time could be used to represent over-time eligible limit if employee is needed before the start of evening part and worked until shift out
         * 
         * Bcoz one and only shift-break then only two free-break zones are to be considered. 
         * 1 - Emp-In to Split-On
         * 2 - Split-Off to Emp-Out
         * If only one break thumb is found in these free-break zones then it could be two scenarios
         * 1 - Employee sign-in to break before the split-on time
         * 2 - Employee sign-off from break after the split-off time
         * 
         * From above two scenarios emp should be detected from his work time duration
         * However emp is automatically deducted for their shift related break then remaining break durations should be considered
         */

        void BeginSplitShiftWorkCalculation()
        {
            if (capturedBreaks.Count > 0)
            {
                // Bcoz split shift there is only one shift-related break
                EmployeeBreak splitBreak = capturedBreaks.FirstOrDefault(c => c.IsShiftBreak == true);

                // split-break's break-in is considered to be overtime start time from split-on if employee is continue his work during split since morning
                DateTime otLimitFromSplitOnTime = splitBreak.EmpAssignedBreak.ShiftBreakIn;

                // split-break's break-out is considered to be overtime start from split-off if employee is begins his work during split and continue until evening
                DateTime otLimitFromSplitOffTime = splitBreak.EmpAssignedBreak.ShiftBreakOut;

                // Check whether employee is eligible for overtime during split-break work
                if (splitBreak.BreakIn > otLimitFromSplitOnTime)
                {
                    // employee is continue his work from morning to time during split-break where overtime could be allocated 
                    // if total of 9 hrs worktime duration is reached
                    hasSplitBreakMorningOT = true;
                }
                else if (splitBreak.BreakOut < otLimitFromSplitOffTime)
                {
                    // employee is begins his work for evening session where overtime is allocated and continue his working and eligible for
                    // overtime if total of 9 hrs worktime duration is reached
                    hasSplitBreakEveningOT = true;
                }

            }
        }

        #endregion

        DateTime breakTimeRoundUp(DateTime dt, TimeSpan d)
        {
            var delta = (d.Ticks - (dt.Ticks % d.Ticks));
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        DateTime breakTimeRoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        #endregion

        #region Max OT Methods

        void SetEmployeeMaxOT()
        {
            if (maxOt != null)
            {
                //Random rnd = new Random();
                if (maxOt.morning_ot_limit > 0)
                {
                    // Employee is entitled for morning Max OT limit
                    DateTime morningOtLimitStart = this.workShift.ShiftIn.Subtract(TimeSpan.FromSeconds((int)maxOt.morning_ot_limit));

                    // Check if employee exceeds ot limit - morning 
                    if (this.workShift.HasPreSingleOT || this.workShift.HasPreDoubleOT || this.workShift.HasPreTripleOT)
                    {
                        // creating system-defined Thumb-In
                        morningOtLimitStart = morningOtLimitStart.Add(new TimeSpan(0, rndTimeGenerator.Next(1, 15), rndTimeGenerator.Next(60)));
                        // Check whether even system defined Thumb-In/Max OT Thumb-In exceed the Actual-Thumb-In during morning ot period
                        if (thumbIn.Value < morningOtLimitStart)
                        {
                            // Employee Thumb-In exceeds the morning ot limit
                            // Set system defined Thumb-In time and Save Actual-In as well.
                            actualIn = thumbIn.Value;
                            thumbIn = morningOtLimitStart;
                            hasMorningMaxOt = true;
                        }
                    }
                }

                if (maxOt.evening_ot_limit > 0)
                {
                    // Employee is entitled for evening Max-OT limit
                    DateTime eveningOtLimitEnd = this.workShift.ShiftOut.Add(TimeSpan.FromSeconds((int)maxOt.evening_ot_limit));

                    // Check if employee exceeds ot limit - evening
                    if (this.workShift.HasPostSingleOT || this.workShift.HasPostDoubleOT || this.workShift.HasPostTripleOT)
                    {
                        // creating system-defined Thumb-Out
                        //eveningOtLimitEnd = eveningOtLimitEnd.Add(new TimeSpan(0, rnd.Next(1, 15), rnd.Next(60)));
                        // Check whether even system defined Thumb-Out/Max OT Thumb-Out exceed the Actual-Thumb-Out during evening ot period
                        if (thumbOut.Value > eveningOtLimitEnd)
                        {
                            // Employee Thumb-Out exceeds the evening ot limit
                            // Set system defined Thumb-Out time and Save Actual-Out as well.
                            actualOut = thumbOut.Value;

                            // For a specific requirement from Unicol ( legal ot time duration should be shown between max ot start time and below limit time)
                            int pickRange;
                            // Defining range start and end where system defined Thumb-Out has been picked
                            if (eveningOtLimitEnd.Subtract(this.workShift.PostNoOtTimeEnd.Subtract(new TimeSpan(0, this.workShift.PostNonOtCompensate, 0))).TotalMinutes > 60)
                            {
                                DateTime otStartTime = this.workShift.PostNoOtTimeEnd.Subtract(new TimeSpan(0, this.workShift.PostNonOtCompensate, 0));
                                pickRange = (int)eveningOtLimitEnd.Subtract(otStartTime.AddMinutes(60)).TotalMinutes;       // literal value 60 could be changed as necessary.
                            }
                            else
                            {
                                pickRange = (int)(eveningOtLimitEnd.Subtract(this.workShift.PostNoOtTimeEnd)).TotalMinutes;
                            }

                            eveningOtLimitEnd = eveningOtLimitEnd.Subtract(new TimeSpan(0, rndTimeGenerator.Next(0, pickRange), rndTimeGenerator.Next(60)));
                            thumbOut = eveningOtLimitEnd;
                            hasEveningMaxOt = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region Override ToString

        public override string ToString()
        {
            //StringBuilder output = new StringBuilder();
            //output.Append("Emp ID: " + this.empID);
            //if (isAttendanceOk)
            //{
            //    output.Append(" shift in: " + this.workShift.ShiftIn.ToString());
            //    output.Append(" shift out: " + this.workShift.ShiftOut.ToString());
            //    output.Append(" Employee In: " + thumbIn.Value.ToString());
            //    output.Append(" Employee Out: " + thumbOut.Value.ToString());
            //    if (isEarlyIn)
            //        output.Append(" Early In: " + workEarlyInTime.ToString() + " Seconds:  " + workEarlyInDuration.ToString());
            //    if (isLateIn)
            //        output.Append(" Late In: " + workLateInTime.ToString() + " Seconds:  " + workLateInDuration.ToString());
            //    if (isEarlyOut)
            //        output.Append(" Early Out: " + workEearlyOutTime.ToString() + " Seconds:  " + workEarlyOutDuration.ToString());
            //    if (isLateOut)
            //        output.Append(" Late Out: " + workLateOutTime.ToString() + " Seconds: " + workLateOutDuration.ToString());
            //}
            //else if (isAttendanceAbsent)
            //{
            //    output.Append(" Employee absent on: " + this.workShift.CurrentDate.Value.ToString());
            //}
            //else if (isAttendanceInvalid)
            //{
            //    output.Append(" Invalid attendance on: " + this.workShift.CurrentDate.Value.ToString());
            //}

            //output.Append("\r\n");
            return this.PrintEmployeeAttendanceRecord().ToString();
        }

        StringBuilder PrintEmployeeAttendanceRecord()
        {
            StringBuilder output = new StringBuilder();
            output.Append("Emp ID: " + this.empID);
            if (isAttendanceOk)
            {
                output.AppendLine("Shift in: " + this.workShift.ShiftIn.ToString());
                output.AppendLine("Shift out: " + this.workShift.ShiftOut.ToString());
                output.AppendLine("Employee In: " + thumbIn.Value.ToString());
                output.AppendLine("Employee Out: " + thumbOut.Value.ToString());
                if (isEarlyIn)
                {
                    output.AppendLine("Early In: " + workEarlyInTime.ToString() + " Seconds:  " + workEarlyInDuration.ToString());
                }
                else
                {
                    output.AppendLine("Early In: ----------");
                }

                if (isLateIn)
                {
                    output.AppendLine("Late In: " + workLateInTime.ToString() + " Seconds:  " + workLateInDuration.ToString());
                    if (this.isMorningShortLeave)
                    {
                        output.AppendLine("Morning short leave: ");
                    }
                    if (this.isMorningHalfDayLeave)
                    {
                        output.AppendLine("Morning half leave :");
                    }
                    if (this.isMorningFullDayLeave)
                    {
                        output.AppendLine("Morning full leave");
                    }
                }
                else
                {
                    output.AppendLine("Late In: ----------");
                }

                if (isEarlyOut)
                {
                    output.AppendLine("Early Out: " + workEearlyOutTime.ToString() + " Seconds:  " + workEarlyOutDuration.ToString());
                    if (this.isMorningHalfDayLeave)
                    {
                        output.AppendLine("Evening short leave: ");
                    }
                    if (this.isMorningHalfDayLeave)
                    {
                        output.AppendLine("Evening half leave :");
                    }
                    if (this.isMorningHalfDayLeave)
                    {
                        output.AppendLine("Evening full leave");
                    }
                }
                else
                {
                    output.AppendLine("Early Out: ----------");
                }

                if (isLateOut)
                {
                    output.AppendLine("Late Out: " + workLateOutTime.ToString() + " Seconds: " + workLateOutDuration.ToString());
                }
                else
                {
                    output.AppendLine("Late Out: ----------");
                }

            }
            else if (isAttendanceAbsent)
            {
                output.AppendLine("Employee absent on: " + this.workShift.CurrentDate.Value.ToString());
            }
            else if (isAttendanceInvalid)
            {
                output.AppendLine("Invalid attendance on: " + this.workShift.CurrentDate.Value.ToString());
            }

            return output;
        }

        #endregion

        public void SetEmployeeAttendanceStatus(trns_EmployeeDailyShiftDetails CalendarShift, bool SelectedAttendance)
        {
            if (SelectedAttendance)
            {
                if (this.EmployeeSelectedAttendance.Count > 0)
                {
                    if (this.EmployeeSelectedAttendance.FirstOrDefault().in_time != null && this.EmployeeSelectedAttendance.FirstOrDefault().out_time != null)
                    {
                        this.thumbIn = this.EmployeeSelectedAttendance.FirstOrDefault().in_time;
                        this.thumbOut = this.EmployeeSelectedAttendance.FirstOrDefault().out_time;
                        this.isAttendanceOk = true;
                    }
                    else if (this.EmployeeSelectedAttendance.FirstOrDefault().in_time != null && this.EmployeeSelectedAttendance.FirstOrDefault().out_time == null)
                    {
                        this.thumbIn = this.EmployeeSelectedAttendance.FirstOrDefault().in_time;
                        this.thumbOut = this.EmployeeSelectedAttendance.FirstOrDefault().in_time;
                        this.workOT = new EmployeeOverTime();
                        this.isAttendanceInvalid = true;
                    }
                    else if (this.EmployeeSelectedAttendance.FirstOrDefault().in_time == null && this.EmployeeSelectedAttendance.FirstOrDefault().out_time != null)
                    {
                        this.thumbIn = this.EmployeeSelectedAttendance.FirstOrDefault().out_time;
                        this.thumbOut = this.EmployeeSelectedAttendance.FirstOrDefault().out_time;
                        this.workOT = new EmployeeOverTime();
                        this.isAttendanceInvalid = true;
                    }
                    else
                    {
                        this.CheckAbsentEmployee(CalendarShift);
                    }
                }
            }
            else
            {
                if (this.EmployeeAttendance.Count >= 2)
                {
                    double InOutDiff = (this.EmployeeAttendance.LastOrDefault().attend_datetime.Value.Subtract(this.EmployeeAttendance.FirstOrDefault().attend_datetime.Value)).TotalSeconds;
                    if (InOutDiff > 300)
                    {
                        this.thumbIn = this.EmployeeAttendance.FirstOrDefault().attend_datetime;
                        this.thumbOut = this.EmployeeAttendance.LastOrDefault().attend_datetime;
                        this.isAttendanceOk = true;
                    }
                    else
                    {
                        this.thumbIn = this.employeeAttendance.FirstOrDefault().attend_datetime;
                        this.thumbOut = this.employeeAttendance.FirstOrDefault().attend_datetime;
                        this.workOT = new EmployeeOverTime();
                        this.isAttendanceInvalid = true;
                    }
                }
                // h 2020-10-09 one attendance means no pay
                //else if (this.EmployeeAttendance.Count == 1)
                //{
                //    this.thumbIn = this.employeeAttendance.FirstOrDefault().attend_datetime;
                //    this.thumbOut = this.employeeAttendance.FirstOrDefault().attend_datetime;
                //    this.workOT = new EmployeeOverTime();
                //    this.isAttendanceInvalid = true;
                //}
                else
                {
                    this.CheckAbsentEmployee(CalendarShift);
                }
            }
        }

        public void SetAllAttendanceStatus(trns_EmployeeDailyShiftDetails CalendarDetails)
        {
            if (isAttendanceOk)
            {
                if (CalendarDetails.date.DayOfWeek == DayOfWeek.Saturday && workShift.EmployeeShift.is_halfday == true)
                {
                    this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.ATTEND_HALFDAY, attend_status = true });
                }
                else
                    this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.ATTEND, attend_status = true });
            }
            if (isAttendanceAbsent)
            {
                if (CalendarDetails.date.DayOfWeek == DayOfWeek.Saturday && workShift.EmployeeShift.is_halfday == true)
                {
                    this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.ABSENT_HALFDAY, attend_status = true });
                }
                else
                    this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.ABSENT, attend_status = true });
            }
            if (isAttendanceInvalid)
                this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.INVALID, attend_status = true });
            if (isGraceIn)
                this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.GRACE_IN, attend_status = true });
            if (isGraceOut)
                this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.GRACE_OUT, attend_status = true });
            if (isLateIn)
                this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.LATE_IN, attend_status = true });
            if (isEarlyOut)
                this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EARLY_OUT, attend_status = true });
            if (isLateCoverDone)
                this.attendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.LATE_COVERING, attend_status = true });
        }

        #region MCN MultipleOT
        public void MultipleOT(double ChangeTime)
        {
            this.workShift.PostSingleOtTimeEnd = this.workShift.PostSingleOtTimeEnd.AddSeconds(ChangeTime);
            this.workShift.PostDoubleOtTimeEnd = this.workShift.PostDoubleOtTimeEnd.AddSeconds(ChangeTime);
            this.workShift.PostTripleOtTimeEnd = this.workShift.PostTripleOtTimeEnd.AddSeconds(ChangeTime);

        }
        #endregion

    }
}