using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class ShiftConfiguration
    {
        #region MCN Properties

        #region Shift Configuration Properties

        #region Basic Shift Configuration

        DateTime shiftOn;
        public DateTime ShiftOn
        {
            get { return shiftOn; }
            set { shiftOn = value; }
        }

        DateTime shiftOff;
        public DateTime ShiftOff
        {
            get { return shiftOff; }
            set { shiftOff = value; }
        }

        DateTime shiftIn;
        public DateTime ShiftIn
        {
            get { return shiftIn; }
            set { shiftIn = value; }
        }

        DateTime shiftOut;
        public DateTime ShiftOut
        {
            get { return shiftOut; }
            set { shiftOut = value; }
        }

        #endregion

        #region Pre-OT Shift Configuration

        DateTime preNoOtTimeStart;
        public DateTime PreNoOtTimeStart
        {
            get { return preNoOtTimeStart; }
            set
            {
                preNoOtTimeStart = value;
            }
        }

        DateTime preSingleOtTimeStart;
        public DateTime PreSingleOtTimeStart
        {
            get { return preSingleOtTimeStart; }
            set
            {
                preSingleOtTimeStart = value;
            }
        }

        DateTime preDoubleOtTimeStart;
        public DateTime PreDoubleOtTimeStart
        {
            get { return preDoubleOtTimeStart; }
            set
            {
                preDoubleOtTimeStart = value;
            }
        }

        DateTime preTripleOtTimeStart;
        public DateTime PreTripleOtTimeStart
        {
            get { return preTripleOtTimeStart; }
            set
            {
                preTripleOtTimeStart = value;
            }
        }

        #region Pre OT Round-Up

        int preSingleOtRoundValue;
        public int PreSingleOtRoundValue
        {
            get { return preSingleOtRoundValue; }
            set { preSingleOtRoundValue = value; }
        }

        int preDoubleOtRoundValue;
        public int PreDoubleOtRoundValue
        {
            get { return preDoubleOtRoundValue; }
            set { preDoubleOtRoundValue = value; }
        }

        int preTripleOtRoundValue;
        public int PreTripleOtRoundValue
        {
            get { return preTripleOtRoundValue; }
            set { preTripleOtRoundValue = value; }
        }

        #endregion

        #region Pre OT Compensation

        int preNonOtCompensate;
        public int PreNonOtCompensate
        {
            get { return preNonOtCompensate; }
            set { preNonOtCompensate = value; }
        }

        #endregion

        #region Pre-OT status

        bool hasPreNonOT;
        public bool HasPreNonOT
        {
            get { return hasPreNonOT; }
            set { hasPreNonOT = value; }
        }

        bool hasPreSingleOT;
        public bool HasPreSingleOT
        {
            get { return hasPreSingleOT; }
            set { hasPreSingleOT = value; }
        }

        bool hasPreDoubleOT;
        public bool HasPreDoubleOT
        {
            get { return hasPreDoubleOT; }
            set { hasPreDoubleOT = value; }
        }

        bool hasPreTripleOT;
        public bool HasPreTripleOT
        {
            get { return hasPreTripleOT; }
            set { hasPreTripleOT = value; }
        }

        #endregion

        #region Pre-OT Round-Up status

        bool hasPreSingleOtRoundUp;
        public bool HasPreSingleOtRoundUp
        {
            get { return hasPreSingleOtRoundUp; }
            set { hasPreSingleOtRoundUp = value; }
        }

        bool hasPreDoubleOtRoundUp;
        public bool HasPreDoubleOtRoundUp
        {
            get { return hasPreDoubleOtRoundUp; }
            set { hasPreDoubleOtRoundUp = value; }
        }

        bool hasPreTripleOtRoundUp;
        public bool HasPreTripleOtRoundUp
        {
            get { return hasPreTripleOtRoundUp; }
            set { hasPreTripleOtRoundUp = value; }
        }

        #endregion

        #region Pre-OT Compensation status

        bool hasPreNonOtCompensation;
        public bool HasPreNonOtCompensation
        {
            get { return hasPreNonOtCompensation; }
            set { hasPreNonOtCompensation = value; }
        }

        #endregion

        #endregion

        #region Post-OT Shift Configuration

        DateTime postNoOtTimeEnd;
        public DateTime PostNoOtTimeEnd
        {
            get { return postNoOtTimeEnd; }
            set { postNoOtTimeEnd = value; }
        }

        DateTime postSingleOtTimeEnd;
        public DateTime PostSingleOtTimeEnd
        {
            get { return postSingleOtTimeEnd; }
            set { postSingleOtTimeEnd = value; }
        }

        DateTime postDoubleOtTimeEnd;
        public DateTime PostDoubleOtTimeEnd
        {
            get { return postDoubleOtTimeEnd; }
            set { postDoubleOtTimeEnd = value; }
        }

        DateTime postTripleOtTimeEnd;
        public DateTime PostTripleOtTimeEnd
        {
            get { return postTripleOtTimeEnd; }
            set { postTripleOtTimeEnd = value; }
        }

        #region Post-OT Round-Up

        int postSingleOtRoundValue;
        public int PostSingleOtRoundValue
        {
            get { return postSingleOtRoundValue; }
            set { postSingleOtRoundValue = value; }
        }

        int postDoubleOtRoundValue;
        public int PostDoubleOtRoundValue
        {
            get { return postDoubleOtRoundValue; }
            set { postDoubleOtRoundValue = value; }
        }

        int postTripleOtRoundValue;
        public int PostTripleOtRoundValue
        {
            get { return postTripleOtRoundValue; }
            set { postTripleOtRoundValue = value; }
        }

        #endregion

        #region Post-OT Compensation

        int postNonOtCompensate;
        public int PostNonOtCompensate
        {
            get { return postNonOtCompensate; }
            set { postNonOtCompensate = value; }
        }

        #endregion

        #region Post-OT Status

        bool hasPostNonOT;
        public bool HasPostNonOT
        {
            get { return hasPostNonOT; }
            set { hasPostNonOT = value; }
        }

        bool hasPostSingleOT;
        public bool HasPostSingleOT
        {
            get { return hasPostSingleOT; }
            set { hasPostSingleOT = value; }
        }

        bool hasPostDoubleOT;
        public bool HasPostDoubleOT
        {
            get { return hasPostDoubleOT; }
            set { hasPostDoubleOT = value; }
        }

        bool hasPostTripleOT;
        public bool HasPostTripleOT
        {
            get { return hasPostTripleOT; }
            set { hasPostTripleOT = value; }
        }

        #endregion

        #region Post-OT Round-Up status

        bool hasPostSingleOtRoundUp;
        public bool HasPostSingleOtRoundUp
        {
            get { return hasPostSingleOtRoundUp; }
            set { hasPostSingleOtRoundUp = value; }
        }

        bool hasPostDoubleOtRoundUp;
        public bool HasPostDoubleOtRoundUp
        {
            get { return hasPostDoubleOtRoundUp; }
            set { hasPostDoubleOtRoundUp = value; }
        }

        bool hasPostTripleOtRoundUp;
        public bool HasPostTripleOtRoundUp
        {
            get { return hasPostTripleOtRoundUp; }
            set { hasPostTripleOtRoundUp = value; }
        }

        #endregion

        #region Post-OT Compensation status

        bool hasPostNonOtCompensation;
        public bool HasPostNonOtCompensation
        {
            get { return hasPostNonOtCompensation; }
            set { hasPostNonOtCompensation = value; }
        }

        #endregion

        #endregion

        #region Late Shift Configuration

        DateTime lateInEndTime;
        public DateTime LateInEndTime
        {
            get { return lateInEndTime; }
            set { lateInEndTime = value; }
        }

        DateTime lateGraceEndTime;
        public DateTime LateGraceEndTime
        {
            get { return lateGraceEndTime; }
            set { lateGraceEndTime = value; }
        }

        DateTime lateGraceEffectEndTime;
        public DateTime LateGraceEffectEndTime
        {
            get { return lateGraceEffectEndTime; }
            set { lateGraceEffectEndTime = value; }
        }

        DateTime lateShortLeaveEndTime;
        public DateTime LateShortLeaveEndTime
        {
            get { return lateShortLeaveEndTime; }
            set { lateShortLeaveEndTime = value; }
        }

        DateTime lateHalfDayEndTime;
        public DateTime LateHalfDayEndTime
        {
            get { return lateHalfDayEndTime; }
            set { lateHalfDayEndTime = value; }
        }

        DateTime lateFullDayLeaveTime;
        public DateTime LateFullDayLeaveTime
        {
            get { return lateFullDayLeaveTime; }
            set { lateFullDayLeaveTime = value; }
        }

        #endregion

        #region Early Shift Configuration

        DateTime earlyOutStartTime;
        public DateTime EarlyOutStartTime
        {
            get { return earlyOutStartTime; }
            set
            {
                earlyOutStartTime = value;
            }
        }

        DateTime earlyGraceStartTime;
        public DateTime EarlyGraceStartTime
        {
            get { return earlyGraceStartTime; }
            set
            {
                earlyGraceStartTime = value;
            }
        }

        DateTime earlyGraceEffectStartTime;
        public DateTime EarlyGraceEffectStartTime
        {
            get { return earlyGraceEffectStartTime; }
            set
            {
                earlyGraceEffectStartTime = value;
            }
        }

        DateTime earlyShortLeaveStartTime;
        public DateTime EarlyShortLeaveStartTime
        {
            get { return earlyShortLeaveStartTime; }
            set
            {
                earlyShortLeaveStartTime = value;
            }
        }

        DateTime earlyHalfDayStartTime;
        public DateTime EarlyHalfDayStartTime
        {
            get { return earlyHalfDayStartTime; }
            set
            {
                earlyHalfDayStartTime = value;
            }
        }

        DateTime earlyFullDayTime;
        public DateTime EarlyFullDayTime
        {
            get { return earlyFullDayTime; }
            set { earlyFullDayTime = value; }
        }

        #endregion

        #region Shift Break Configuration

        List<dtl_Shift_Break_Details> shiftBreaksList;
        public List<dtl_Shift_Break_Details> ShiftBreaksList
        {
            get { return shiftBreaksList; }
            set { shiftBreaksList = value; }
        }

        List<ShiftBreak> assignedBreakList = new List<ShiftBreak>();
        public List<ShiftBreak> AssignedBreakList
        {
            get { return assignedBreakList; }
            set { assignedBreakList = value; }
        }

        bool isSplitShift;
        public bool IsSplitShift
        {
            get { return isSplitShift; }
            set { isSplitShift = value; }
        }

        #endregion

        #region MCN Covering

        #region Late Cover Configuration

        dtl_Shift_Covering_Details shiftLateCoverDetail;
        public dtl_Shift_Covering_Details ShiftLateCoverDetail
        {
            get { return shiftLateCoverDetail; }
            set { shiftLateCoverDetail = value; }
        }

        bool isLateCoverAllowed;
        public bool IsLateCoverAllowed
        {
            get { return isLateCoverAllowed; }
            set { isLateCoverAllowed = value; }
        }

        bool isFixedLateCover;
        public bool IsFixedLateCover
        {
            get { return isFixedLateCover; }
            set { isFixedLateCover = value; }
        }

        DateTime allowedLateCoverTime;
        public DateTime AllowedLateCoverTime
        {
            get { return allowedLateCoverTime; }
            set { allowedLateCoverTime = value; }
        }

        DateTime lateCoverStartTime;
        public DateTime LateCoverStartTime
        {
            get { return lateCoverStartTime; }
            set { lateCoverStartTime = value; }
        }

        DateTime lateCoverEndTime;
        public DateTime LateCoverEndTime
        {
            get { return lateCoverEndTime; }
            set { lateCoverEndTime = value; }
        }

        #endregion

        #endregion

        #region MCN Open Shift
        #region Bool
        private bool isOpenShift;

        public bool IsOpenShift
        {
            get { return isOpenShift; }
            set { isOpenShift = value; }
        }

        private bool isOpenShiftOT;

        public bool IsOpenShiftOT
        {
            get { return isOpenShiftOT; }
            set { isOpenShiftOT = value; }
        }

        private bool isOpenShiftLate;

        public bool IsOpenShiftLate
        {
            get { return isOpenShiftLate; }
            set { isOpenShiftLate = value; }
        }
        #endregion
        #region Minimum Word Duration

        private int openShiftMinWordDuration;

        public int OpenShiftMinWordDuration
        {
            get { return openShiftMinWordDuration; }
            set { openShiftMinWordDuration = value; }
        }


        #endregion
        #region OT
        private int openShiftNoOtDuration;

        public int OpenShiftNoOtDuration
        {
            get { return openShiftNoOtDuration; }
            set { openShiftNoOtDuration = value; }
        }

        private int openShiftSingleOtDuration;

        public int OpenShiftSingleOtDuration
        {
            get { return openShiftSingleOtDuration; }
            set { openShiftSingleOtDuration = value; }
        }

        private int openShiftDoubleOtDuration;

        public int OpenShiftDoubleOtDuration
        {
            get { return openShiftDoubleOtDuration; }
            set { openShiftDoubleOtDuration = value; }
        }

        private int openShiftTripleOtDuration;

        public int OpenShiftTripleOtDuration
        {
            get { return openShiftTripleOtDuration; }
            set { openShiftTripleOtDuration = value; }
        }

        #endregion
        #region Late
        private int openShiftLateDuration;

        public int OpenShiftLateDuration
        {
            get { return openShiftLateDuration; }
            set { openShiftLateDuration = value; }
        }

        private int openShiftShotLeaveTimeDuration;

        public int OpenShiftShotLeaveTimeDuration
        {
            get { return openShiftShotLeaveTimeDuration; }
            set { openShiftShotLeaveTimeDuration = value; }
        }

        private int openShiftHalfDayTimeDuration;

        public int OpenShiftHalfDayTimeDuration
        {
            get { return openShiftHalfDayTimeDuration; }
            set { openShiftHalfDayTimeDuration = value; }
        }

        private int openShiftFullDayTimeDuration;

        public int OpenShiftFullDayTimeDuration
        {
            get { return openShiftFullDayTimeDuration; }
            set { openShiftFullDayTimeDuration = value; }
        }
        #endregion
        #endregion

        #endregion 

        #region Attendance Data

        ShiftDetailAllView employeeShift;
        public ShiftDetailAllView EmployeeShift
        {
            get { return employeeShift; }
            set
            {
                employeeShift = value;
            }
        }

        DateTime? currentDate;
        public DateTime? CurrentDate
        {
            get { return currentDate; }
            set
            { 
                currentDate = value;
                if (currentDate != null && employeeShift != null)
                    this.SetCurrentShiftConfiguration();
            }
        }  

        #endregion

        #endregion

        #region Shift data setting methods

        public void SetCurrentShiftConfiguration()
        {
            DateTime inDate = (DateTime)currentDate;
            shiftIn = inDate.Date.Add((TimeSpan)employeeShift.shift_in_time);
            shiftOut = inDate.Date.AddDays((double)employeeShift.shift_out_day_value).Add((TimeSpan)employeeShift.shift_out_time);
            shiftOn = shiftIn.AddMinutes(-(double)employeeShift.shift_on_time);
            shiftOff = shiftOut.AddMinutes((double)employeeShift.shift_off_time);

            preNoOtTimeStart = shiftIn.AddMinutes(-(double)employeeShift.pre_non_ot);
            preSingleOtTimeStart = preNoOtTimeStart.AddMinutes(-(double)employeeShift.pre_single_ot);
            preDoubleOtTimeStart = preSingleOtTimeStart.AddMinutes(-(double)employeeShift.pre_double_ot);
            preTripleOtTimeStart = preDoubleOtTimeStart.AddMinutes(-(double)employeeShift.pre_triple_ot);

            preSingleOtRoundValue = (int)employeeShift.pre_single_ot_roundup;
            preDoubleOtRoundValue = (int)employeeShift.pre_double_ot_roundup;
            preTripleOtRoundValue = (int)employeeShift.pre_triple_ot_roundup;
            preNonOtCompensate = (int)employeeShift.pre_non_ot_compensate;

            postNoOtTimeEnd = shiftOut.AddMinutes((double)employeeShift.post_non_ot);
            postSingleOtTimeEnd = postNoOtTimeEnd.AddMinutes((double)employeeShift.post_single_ot);
            postDoubleOtTimeEnd = postSingleOtTimeEnd.AddMinutes((double)employeeShift.post_double_ot);
            postTripleOtTimeEnd = postDoubleOtTimeEnd.AddMinutes((double)employeeShift.post_triple_ot);

            postSingleOtRoundValue = (int)employeeShift.post_single_ot_roundup;
            postDoubleOtRoundValue = (int)employeeShift.post_double_ot_roundup;
            postTripleOtRoundValue = (int)employeeShift.post_triple_ot_roundup;
            postNonOtCompensate = (int)employeeShift.post_non_ot_compensate;

            lateGraceEffectEndTime = shiftIn.AddMinutes((double)employeeShift.late_grace_effect_time);
            lateGraceEndTime = lateGraceEffectEndTime.AddMinutes((double)employeeShift.late_grace_time);
            lateInEndTime = lateGraceEndTime.AddMinutes((double)employeeShift.late_in);
            lateShortLeaveEndTime = lateInEndTime.AddMinutes((double)employeeShift.morning_short_leave);
            lateHalfDayEndTime = lateShortLeaveEndTime.AddMinutes((double)employeeShift.morning_halfday_leave);
            //lateFullDayLeaveTime = lateHalfDayTime.AddMinutes((double)employeeShift.fullday_leave);

            earlyGraceEffectStartTime = shiftOut.AddMinutes(-(double)employeeShift.early_grace_effect_time);
            earlyGraceStartTime = earlyGraceEffectStartTime.AddMinutes(-(double)employeeShift.early_grace_time);
            earlyOutStartTime = earlyGraceStartTime.AddMinutes(-(double)employeeShift.early_out);
            earlyShortLeaveStartTime = earlyOutStartTime.AddMinutes(-(double)employeeShift.evening_short_leave);
            earlyHalfDayStartTime = earlyShortLeaveStartTime.AddMinutes(-(double)employeeShift.evening_halfday_leave);
            //earlyFullDayTime = earlyHalfDayTime.AddMinutes(-(double)employeeShift.fullday_leave);

            // split shift status setting
            isSplitShift = employeeShift.is_split_shift.Value;

            // open Shift 
            isOpenShift = employeeShift.is_open_shift.GetValueOrDefault();
            isOpenShiftOT = employeeShift.is_open_shift_ot.GetValueOrDefault();
            isOpenShiftLate = employeeShift.is_open_shift_late.GetValueOrDefault();

            openShiftMinWordDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_min_word_time.GetValueOrDefault())).TotalSeconds);

            openShiftNoOtDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_non_ot.GetValueOrDefault())).TotalSeconds);
            openShiftSingleOtDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_single_ot.GetValueOrDefault())).TotalSeconds);
            openShiftDoubleOtDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_double_ot.GetValueOrDefault())).TotalSeconds);
            openShiftTripleOtDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_triple_ot.GetValueOrDefault())).TotalSeconds);

            openShiftLateDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_late.GetValueOrDefault())).TotalSeconds);
            openShiftShotLeaveTimeDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_short_leave.GetValueOrDefault())).TotalSeconds);
            openShiftHalfDayTimeDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_halfday_leave.GetValueOrDefault())).TotalSeconds);
            openShiftFullDayTimeDuration = Convert.ToInt32((TimeSpan.FromMinutes((double)employeeShift.open_fullday_leave.GetValueOrDefault())).TotalSeconds);

            this.SetPreOTStatus();
            this.SetPostOTStatus();
            this.SetPreOtRoundUpStatus();
            this.SetPostOtRoundUpStatus();
            this.SetPreOtCompensationStatus();
            this.SetPostOtCompensationStatus();
            this.SetShiftBreakDetails();
            this.SetShiftLateCoveringDetails(inDate);
        }

        void SetPreOTStatus()
        {
            if(shiftIn.Subtract(preNoOtTimeStart).TotalSeconds > 0)
            {
                this.hasPreNonOT = true;
            }
            if (preNoOtTimeStart.Subtract(preSingleOtTimeStart).TotalSeconds > 0)
            {
                this.hasPreSingleOT = true;
            }
            if(preDoubleOtTimeStart.Subtract(preSingleOtTimeStart).TotalSeconds > 0)
            {
                this.hasPreDoubleOT = true;
            }
            if(preTripleOtTimeStart.Subtract(preDoubleOtTimeStart).TotalSeconds > 0)
            {
                this.hasPreTripleOT = true;
            }
        }

        void SetPostOTStatus()
        {
            if (postNoOtTimeEnd.Subtract(shiftOut).TotalSeconds > 0)
            {
                this.hasPostNonOT = true;
            }
            if (postSingleOtTimeEnd.Subtract(postNoOtTimeEnd).TotalSeconds > 0)
            {
                this.hasPostSingleOT = true;
            }
            if (postDoubleOtTimeEnd.Subtract(postSingleOtTimeEnd).TotalSeconds > 0)
            {
                this.hasPostDoubleOT = true;
            }
            if(postTripleOtTimeEnd.Subtract(postDoubleOtTimeEnd).TotalSeconds > 0)
            {
                this.hasPostTripleOT = true;
            }
        }

        void SetPreOtRoundUpStatus()
        {
            if (preSingleOtRoundValue != 0)
                hasPreSingleOtRoundUp = true;
            if (preDoubleOtRoundValue != 0)
                hasPreDoubleOtRoundUp = true;
            if (preTripleOtRoundValue != 0)
                hasPreTripleOtRoundUp = true;
        }

        void SetPostOtRoundUpStatus()
        {
            if(postSingleOtRoundValue != 0)
                hasPostSingleOtRoundUp = true;
            if (postDoubleOtRoundValue != 0)
                hasPostDoubleOtRoundUp = true;
            if (postTripleOtRoundValue != 0)
                hasPostTripleOtRoundUp = true;
        }

        void SetPreOtCompensationStatus()
        {
            if (preNonOtCompensate != 0)
                hasPreNonOtCompensation = true;
        }

        void SetPostOtCompensationStatus()
        {
            if (postNonOtCompensate != 0)
                hasPostNonOtCompensation = true;
        }

        void SetShiftBreakDetails()
        {
            if(shiftBreaksList != null)
            {
                foreach(dtl_Shift_Break_Details breakItem in shiftBreaksList)
                {
                    ShiftBreak breakConfig = new ShiftBreak(breakItem);
                    breakConfig.ConfigureShiftBreak(this);
                    assignedBreakList.Add(breakConfig);
                }
            }
        }

        #region MCN Covering
        void SetShiftLateCoveringDetails(DateTime shiftDate)
        {
            if (shiftLateCoverDetail != null)
            {
                this.lateCoverStartTime = shiftDate.AddDays(shiftLateCoverDetail.covering_on_day_value.Value);
                this.lateCoverStartTime = this.lateCoverStartTime.Add(shiftLateCoverDetail.covering_on_time.Value);
                this.lateCoverEndTime = shiftDate.AddDays(shiftLateCoverDetail.covering_off_day_value.Value);
                this.lateCoverEndTime = this.lateCoverEndTime.Add(shiftLateCoverDetail.covering_off_time.Value);
                this.allowedLateCoverTime = this.lateGraceEffectEndTime.Add(TimeSpan.FromMinutes(shiftLateCoverDetail.maximum_late_time.Value));
                if (shiftLateCoverDetail.covering_effect_time.Value != 0)
                {
                    this.isFixedLateCover = true;
                }

                this.isLateCoverAllowed = true;
            }
        }        
        #endregion

        #endregion
    }
}