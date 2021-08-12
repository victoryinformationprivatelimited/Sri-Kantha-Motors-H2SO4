using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class EmployeeOverTime
    {
        #region Data Members

        #region OT Status

        #region Pre OT

        bool preSingleOTWorkDone;
        bool preDoubleOTWorkDone;
        bool preTripleOTWorkDone;

        #endregion

        #region Post OT

        bool postSingleOTWorkDone;
        bool postDoubleOTWorkDone;
        bool postTripleOTWorkDone;

        #endregion

        #endregion

        #region Extra-OT Status

        #region Pre_Extra OT

        bool hasPreExtraSingleOt;
        bool hasPreExtraDoubleOt;
        bool hasPreExtraTripleOt;

        #endregion

        #region Post_Extra OT

        bool hasPostExtraSingleOt;
        bool hasPostExtraDoubleOt;
        bool hasPostExtraTripleOt;

        #endregion

        #endregion

        #region Extra-OT START & END

        DateTime preExtraOtStart;
        DateTime postExtraOtStart;

        #endregion

        #endregion

        #region Overtime Properties

        #region Pre-OT Work

        #region Single OT

        DateTime workPreSingleOtStartTime;
        public DateTime WorkPreSingleOtStartTime
        {
            get { return workPreSingleOtStartTime; }
            set { workPreSingleOtStartTime = value; }
        }

        DateTime workPreSingleOtEndTime;
        public DateTime WorkPreSingleOtEndTime
        {
            get { return workPreSingleOtEndTime; }
            set { workPreSingleOtEndTime = value; }
        }

        int workPreSingleOtDuration;
        public int WorkPreSingleOtDuration
        {
            get { return workPreSingleOtDuration; }
            set { workPreSingleOtDuration = value; }
        }

        #endregion

        #region Double OT

        DateTime workPreDoubleOtStartTime;
        public DateTime WorkPreDoubleOtStartTime
        {
            get { return workPreDoubleOtStartTime; }
            set { workPreDoubleOtStartTime = value; }
        }

        DateTime workPreDoubleOtEndTime;
        public DateTime WorkPreDoubleOtEndTime
        {
            get { return workPreDoubleOtEndTime; }
            set { workPreDoubleOtEndTime = value; }
        }

        int workPreDoubleOtDuration;
        public int WorkPreDoubleOtDuration
        {
            get { return workPreDoubleOtDuration; }
            set { workPreDoubleOtDuration = value; }
        }

        #endregion

        #region Triple OT

        DateTime workPreTripleOtStartTime;
        public DateTime WorkPreTripleOtStartTime
        {
            get { return workPreTripleOtStartTime; }
            set { workPreTripleOtStartTime = value; }
        }

        DateTime workPreTripleOtEndTime;
        public DateTime WorkPreTripleOtEndTime
        {
            get { return workPreTripleOtEndTime; }
            set { workPreTripleOtEndTime = value; }
        }

        int workPreTripleOtDuration;
        public int WorkPreTripleOtDuration
        {
            get { return workPreTripleOtDuration; }
            set { workPreTripleOtDuration = value; }
        }

        #endregion

        #endregion

        #region Post-OT Work

        #region Single OT

        DateTime workPostSingleOtStatrtTime;
        public DateTime WorkPostSingleOtStatrtTime
        {
            get { return workPostSingleOtStatrtTime; }
            set { workPostSingleOtStatrtTime = value; }
        }

        DateTime workPostSingleOtEndTime;
        public DateTime WorkPostSingleOtEndTime
        {
            get { return workPostSingleOtEndTime; }
            set { workPostSingleOtEndTime = value; }
        }

        int workPostSingleOtDuration;
        public int WorkPostSingleOtDuration
        {
            get { return workPostSingleOtDuration; }
            set { workPostSingleOtDuration = value; }
        }

        private int openShiftWorkLateOutDuration;

        public int OpenShiftWorkLateOutDuration
        {
            get { return openShiftWorkLateOutDuration; }
            set { openShiftWorkLateOutDuration = value; }
        }

        #endregion

        #region Double OT

        DateTime workPostDoubleOtStartTime;
        public DateTime WorkPostDoubleOtStartTime
        {
            get { return workPostDoubleOtStartTime; }
            set { workPostDoubleOtStartTime = value; }
        }

        DateTime workPostDoubleOtEndTime;
        public DateTime WorkPostDoubleOtEndTime
        {
            get { return workPostDoubleOtEndTime; }
            set { workPostDoubleOtEndTime = value; }
        }

        int workPostDoubleOtDuration;
        public int WorkPostDoubleOtDuration
        {
            get { return workPostDoubleOtDuration; }
            set { workPostDoubleOtDuration = value; }
        }

        #endregion

        #region Triple OT

        DateTime workPostTripleOtStartTime;
        public DateTime WorkPostTripleOtStartTime
        {
            get { return workPostTripleOtStartTime; }
            set { workPostTripleOtStartTime = value; }
        }

        DateTime workPostTripleOtEndTime;
        public DateTime WorkPostTripleOtEndTime
        {
            get { return workPostTripleOtEndTime; }
            set { workPostTripleOtEndTime = value; }
        }

        int workPostTripleOtDuration;
        public int WorkPostTripleOtDuration
        {
            get { return workPostTripleOtDuration; }
            set { workPostTripleOtDuration = value; }
        }

        #endregion

        #endregion

        #region Max-OT work

        DateTime morningMaxOTStart;
        public DateTime MorningMaxOTStart
        {
            get { return morningMaxOTStart; }
            set { morningMaxOTStart = value; }
        }

        DateTime eveningMaxOTEnd;
        public DateTime EveningMaxOTEnd
        {
            get { return eveningMaxOTEnd; }
            set { eveningMaxOTEnd = value; }
        }

        #region Pre-Extra OT

        #region Single Extra-OT

        int preSingleExtraOtDuration;
        public int PreSingleExtraOtDuration
        {
            get { return preSingleExtraOtDuration; }
            set { preSingleExtraOtDuration = value; }
        }

        #endregion

        #region Double Extra-OT

        int preDoubleExtraOtDuration;
        public int PreDoubleExtraOtDuration
        {
            get { return preDoubleExtraOtDuration; }
            set { preDoubleExtraOtDuration = value; }
        }

        #endregion

        #region Triple Extra-OT

        int preTripleExtraOtDuration;
        public int PreTripleExtraOtDuration
        {
            get { return preTripleExtraOtDuration; }
            set { preTripleExtraOtDuration = value; }
        }

        #endregion

        #endregion

        #region Post-Extra OT

        #region Single Extra-OT

        int postSingleExtraOtDuration;
        public int PostSingleExtraOtDuration
        {
            get { return postSingleExtraOtDuration; }
            set { postSingleExtraOtDuration = value; }
        }

        #endregion

        #region Double Extra-OT

        int postDoubleExtraOtDuration;
        public int PostDoubleExtraOtDuration
        {
            get { return postDoubleExtraOtDuration; }
            set { postDoubleExtraOtDuration = value; }
        }

        #endregion

        #region Triple Extra-OT

        int postTripleExtraOtDuration;
        public int PostTripleExtraOtDuration
        {
            get { return postTripleExtraOtDuration; }
            set { postTripleExtraOtDuration = value; }
        }

        #endregion

        #endregion

        #endregion

        #region OT START & END

        DateTime? workOtStart;
        public DateTime? WorkOtStart
        {
            get { return workOtStart; }
            set { workOtStart = value; }
        }

        DateTime? workOtEnd;
        public DateTime? WorkOtEnd
        {
            get { return workOtEnd; }
            set { workOtEnd = value; }
        }

        #endregion

        #endregion

        #region Methods

        #region Over Time Calculation Methods

        public void CalculateOverTime(AttendEmployee otEmployee, int workLateOutDuration)
        {
            if (!otEmployee.WorkShift.IsOpenShift/* && otEmployee.IsOTApplicable*/)
            {
                this.preOTCalculation(otEmployee);
                this.postOTCalculation(otEmployee);
            }
            else if (otEmployee.WorkShift.IsOpenShift && otEmployee.WorkShift.IsOpenShiftOT && workLateOutDuration > 0)
            {
                openShiftWorkLateOutDuration = workLateOutDuration;
                this.openPostOTCalculation(otEmployee);
            }
            //this.preOTCalculation(otEmployee);
            //this.postOTCalculation(otEmployee);
            //this.CalculateOvertimeBasedOnWork(otEmployee);
        }

        void preOTCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.IsEarlyIn && otEmployee.WorkEarlyInTime < otEmployee.WorkShift.PreNoOtTimeStart)      // employee could be entitled for early in over time work
            {
                bool isSingleOtOnly = false;
                bool isDoubleOtOnly = false;
                bool isOtCompensate = false;
                //========================================================================= Morning Single OT Calculation begins here ===============================================================

                if (otEmployee.WorkShift.HasPreSingleOT)      // employee's workshift has pre single ot 
                {
                    if (otEmployee.WorkShift.HasPreNonOtCompensation)
                    {
                        // employee's pre non ot time period is considered for pre-single ot work
                        otEmployee.WorkShift.PreNoOtTimeStart = otEmployee.WorkShift.PreNoOtTimeStart.Add(TimeSpan.FromMinutes(otEmployee.WorkShift.PreNonOtCompensate));
                        isOtCompensate = true;
                    }

                    if (otEmployee.WorkEarlyInTime > otEmployee.WorkShift.PreSingleOtTimeStart)
                    {
                        // employee Thumb In was after morning single ot start time
                        this.workPreSingleOtStartTime = otEmployee.WorkEarlyInTime;
                        workOtStart = this.workPreSingleOtStartTime;
                        isSingleOtOnly = true;

                        // Over time eligible for rounding up by user defined amount when employee Thumb-In was during morning single ot period
                        if (otEmployee.WorkShift.HasPreSingleOtRoundUp)
                        {
                            if (otEmployee.WorkShift.PreSingleOtRoundValue > 0)
                                workOtStart = this.overtimeRoundUp(workOtStart.Value, TimeSpan.FromMinutes(otEmployee.WorkShift.PreSingleOtRoundValue));
                            else
                                workOtStart = this.overtimeRoundDown(workOtStart.Value, TimeSpan.FromMinutes(-otEmployee.WorkShift.PreSingleOtRoundValue));
                        }
                        if (otEmployee.HasMorningMaxOt)
                        {
                            // Even if the employee's pre-single ot end up, there could be extra-ot
                            this.hasPreExtraSingleOt = true;
                            preExtraOtStart = workOtStart.Value;
                            this.PreExtraOTCalculation(otEmployee);
                        }
                    }
                    else
                    {
                        // Though employee has done pre single ot work Thumb-In was before single ot start time
                        this.workPreSingleOtStartTime = otEmployee.WorkShift.PreSingleOtTimeStart;
                        workOtStart = this.workPreSingleOtStartTime;
                    }

                    this.workPreSingleOtDuration = (int)otEmployee.WorkShift.PreNoOtTimeStart.Subtract(workOtStart.Value).TotalSeconds;
                    preSingleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.PRE_SINGLE_OT, attend_status = true });
                }
                //========================================================== morning single ot calculation end here =================================================================================

                //========================================================== morning double ot calculation begins here ==============================================================================

                if (otEmployee.WorkShift.HasPreDoubleOT && !isSingleOtOnly)    // employee's work shift has pre double ot
                {
                    if (!isOtCompensate && otEmployee.WorkShift.HasPreNonOtCompensation)
                    {
                        // employee's pre non ot time period is considered for pre-double ot work when no pre-single ot is not allocated.
                        otEmployee.WorkShift.PreSingleOtTimeStart = otEmployee.WorkShift.PreSingleOtTimeStart.Add(TimeSpan.FromMinutes(otEmployee.WorkShift.PreNonOtCompensate));
                        isOtCompensate = true;
                    }
                    if (otEmployee.WorkEarlyInTime > otEmployee.WorkShift.PreDoubleOtTimeStart)
                    {
                        // employee Thumb-In was after morning double ot start time
                        this.workPreDoubleOtStartTime = otEmployee.WorkEarlyInTime;
                        workOtStart = this.workPreDoubleOtStartTime;
                        isDoubleOtOnly = true;

                        // Over time eligible for rounding up by user defined amount when employee Thumb-In was during morning double ot period
                        if (otEmployee.WorkShift.HasPreDoubleOtRoundUp)
                        {
                            if (otEmployee.WorkShift.PreDoubleOtRoundValue > 0)
                                workOtStart = this.overtimeRoundUp(workOtStart.Value, TimeSpan.FromMinutes(otEmployee.WorkShift.PreDoubleOtRoundValue));
                            else
                                workOtStart = this.overtimeRoundDown(workOtStart.Value, TimeSpan.FromMinutes(-otEmployee.WorkShift.PreDoubleOtRoundValue));
                        }

                        if (otEmployee.HasMorningMaxOt)
                        {
                            // Even if the employee's pre-double ot end up here, there could be extra-ot
                            this.hasPreExtraDoubleOt = true;
                            preExtraOtStart = workOtStart.Value;
                            this.PreExtraOTCalculation(otEmployee);
                        }
                    }
                    else
                    {
                        // Though employee has done pre double ot work Thumb-In was before double ot start time
                        this.workPreDoubleOtStartTime = otEmployee.WorkShift.PreDoubleOtTimeStart;
                        workOtStart = this.workPreDoubleOtStartTime;
                    }

                    this.workPreDoubleOtDuration = (int)otEmployee.WorkShift.PreSingleOtTimeStart.Subtract(this.workOtStart.Value).TotalSeconds;
                    preDoubleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.PRE_DOUBLE_OT, attend_status = true });
                }

                //=============================================================== morning double ot calculation end here ============================================================================

                //=============================================================== morning triple ot calculation begins here =========================================================================

                if (otEmployee.WorkShift.HasPreTripleOT && !isSingleOtOnly && !isDoubleOtOnly) // employee's work shift has pre triple ot
                {
                    if (!isOtCompensate && otEmployee.WorkShift.HasPreNonOtCompensation)
                    {
                        // employee's pre non ot time period is considered for pre-triple ot work when no pre-single ot or pre-double ot are not allocated.
                        otEmployee.WorkShift.PreDoubleOtTimeStart = otEmployee.WorkShift.PreDoubleOtTimeStart.Add(TimeSpan.FromMinutes(otEmployee.WorkShift.PreNonOtCompensate));
                        isOtCompensate = true;
                    }
                    if (otEmployee.WorkEarlyInTime > otEmployee.WorkShift.PreTripleOtTimeStart)
                    {
                        // employee Thumb-In was after morning triple ot start time
                        this.workPreTripleOtStartTime = otEmployee.WorkEarlyInTime;
                        workOtStart = this.workPreTripleOtStartTime;

                        // Over time eligible for rounding up by user defined amount when employee Thumb-In was during morning triple ot period
                        if (otEmployee.WorkShift.HasPreTripleOtRoundUp)
                        {
                            if (otEmployee.WorkShift.PreTripleOtRoundValue > 0)
                                workOtStart = this.overtimeRoundUp(workOtStart.Value, TimeSpan.FromMinutes(otEmployee.WorkShift.PreTripleOtRoundValue));
                            else
                                workOtStart = this.overtimeRoundDown(workOtStart.Value, TimeSpan.FromMinutes(-otEmployee.WorkShift.PreTripleOtRoundValue));
                        }
                        if (otEmployee.HasMorningMaxOt)
                        {
                            // Even if the employee's pre-triple ot end up here, there could be extra-ot
                            this.hasPreExtraTripleOt = true;
                            preExtraOtStart = workOtStart.Value;
                            this.PreExtraOTCalculation(otEmployee);
                        }
                    }
                    else
                    {
                        // Though employee has done pre triple ot work Thumb-In was before triple ot start time
                        this.workPreTripleOtStartTime = otEmployee.WorkShift.PreTripleOtTimeStart;
                        workOtStart = this.workPreTripleOtStartTime;
                    }

                    this.workPreTripleOtDuration = (int)otEmployee.WorkShift.PreDoubleOtTimeStart.Subtract(this.workOtStart.Value).TotalSeconds;
                    preTripleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.PRE_TRIPLE_OT, attend_status = true });
                }
                //============================================================ morning triple ot calculation end here ==============================================================================
            }
        }

        void postOTCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.IsLateOut && otEmployee.WorkLateOutTime > otEmployee.WorkShift.PostNoOtTimeEnd)        // employee could be entitled for late out over time work
            {
                bool isSingleOtOnly = false;
                bool isDoubleOtOnly = false;
                bool isOtCompensate = false;

                #region MCN
                bool isSingleOt = false;
                bool isDoubleOt = false;
                #endregion

                if (otEmployee.WorkShift.HasPostSingleOT)    // employee's working shift has post single ot
                {
                    if (otEmployee.WorkShift.HasPostNonOtCompensation)
                    {
                        // employee's post non ot time period is considered for post-single ot work
                        otEmployee.WorkShift.PostNoOtTimeEnd = otEmployee.WorkShift.PostNoOtTimeEnd.Subtract(TimeSpan.FromMinutes(otEmployee.WorkShift.PostNonOtCompensate));
                        isOtCompensate = true;

                        #region MCN Add Full day Post Single OT

                        if ((otEmployee.WorkShift.ShiftOut.Subtract(otEmployee.WorkShift.ShiftIn).TotalMinutes) < 0)
                        {
                            otEmployee.WorkShift.PostNoOtTimeEnd = otEmployee.WorkShift.ShiftOut.Subtract(TimeSpan.FromMinutes(otEmployee.WorkShift.PostNonOtCompensate));

                        }
                        #endregion

                    }

                    #region MCN Full day Post Single OT

                    if ((otEmployee.WorkShift.ShiftOut.Subtract(otEmployee.WorkShift.ShiftIn).TotalMinutes) < 15 && !isOtCompensate && !isSingleOt && !isDoubleOt)
                    {
                        //change employe post non ot time period 
                        //if (otEmployee.ThumbIn < otEmployee.WorkShift.ShiftIn)
                        //    otEmployee.WorkShift.PostNoOtTimeEnd = otEmployee.WorkShift.ShiftIn;
                        //else
                        otEmployee.WorkShift.PostNoOtTimeEnd = otEmployee.ThumbIn.Value;
                    }

                    #endregion

                    if (otEmployee.WorkLateOutTime <= otEmployee.WorkShift.PostSingleOtTimeEnd)
                    {
                        // employee's Thumb-Out was during the time period between the no ot end and post single ot end
                        this.workPostSingleOtEndTime = otEmployee.WorkLateOutTime;
                        workOtEnd = this.workPostSingleOtEndTime;
                        isSingleOtOnly = true;

                        // Over time eligible for rounding up by user defined amount when employee Thumb-Out was during evening single ot period
                        if (otEmployee.WorkShift.HasPostSingleOtRoundUp)
                        {
                            if (otEmployee.WorkShift.PostSingleOtRoundValue > 0)
                                workOtEnd = this.overtimeRoundUp(workOtEnd.Value, TimeSpan.FromMinutes(otEmployee.WorkShift.PostSingleOtRoundValue));
                            else
                                workOtEnd = this.overtimeRoundDown(workOtEnd.Value, TimeSpan.FromMinutes(-otEmployee.WorkShift.PostSingleOtRoundValue));
                        }

                        if (otEmployee.HasEveningMaxOt)
                        {
                            // Even if the employee's post-single ot end up here, there could be extra-ot
                            this.hasPostExtraSingleOt = true;
                            postExtraOtStart = workOtEnd.Value;
                            this.PostExtraOTCalculation(otEmployee);
                        }
                        #region MCN
                        isSingleOt = true;
                        #endregion
                    }
                    else
                    {
                        // Though employee has done single ot work Thumb-Out was after single ot end time
                        this.workPostSingleOtEndTime = otEmployee.WorkShift.PostSingleOtTimeEnd;
                        workOtEnd = this.workPostSingleOtEndTime;
                        #region MCN
                        isSingleOt = true;
                        #endregion
                    }

                    this.workPostSingleOtDuration = (int)this.workOtEnd.Value.Subtract(otEmployee.WorkShift.PostNoOtTimeEnd).TotalSeconds;

                    postSingleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.POST_SINGLE_OT, attend_status = true });
                }

                if (otEmployee.WorkShift.HasPostDoubleOT && !isSingleOtOnly)
                {
                    if (!isOtCompensate && otEmployee.WorkShift.HasPostNonOtCompensation)
                    {
                        // employee's post non ot time period is considered for post-double ot work when post-single ot has not been allocated.
                        otEmployee.WorkShift.PostSingleOtTimeEnd = otEmployee.WorkShift.PostSingleOtTimeEnd.Subtract(TimeSpan.FromMinutes(otEmployee.WorkShift.PostNonOtCompensate));
                        isOtCompensate = true;

                        #region MCN Full day Post Double OT

                        if ((otEmployee.WorkShift.ShiftOut.Subtract(otEmployee.WorkShift.ShiftIn).TotalMinutes) < 15)
                        {
                            otEmployee.WorkShift.PostSingleOtTimeEnd = otEmployee.WorkShift.ShiftOut.Subtract(TimeSpan.FromMinutes(otEmployee.WorkShift.PostNonOtCompensate));

                        }
                        #endregion

                    }

                    #region MCN Full day Post Double OT

                    if ((otEmployee.WorkShift.ShiftOut.Subtract(otEmployee.WorkShift.ShiftIn).TotalMinutes) < 15 && !isOtCompensate && !isSingleOt && !isDoubleOt)
                    {
                        //change employe post non ot time period 
                        if (otEmployee.ThumbIn < otEmployee.WorkShift.ShiftIn)
                            otEmployee.WorkShift.PostSingleOtTimeEnd = otEmployee.WorkShift.ShiftIn;
                        else
                            otEmployee.WorkShift.PostSingleOtTimeEnd = otEmployee.ThumbIn.Value;

                    }

                    #endregion

                    if (otEmployee.WorkLateOutTime <= otEmployee.WorkShift.PostDoubleOtTimeEnd)
                    {
                        // employee's Thumb-Out was during the time period between the single ot end and double ot end
                        this.workPostDoubleOtEndTime = otEmployee.WorkLateOutTime;
                        workOtEnd = this.workPostDoubleOtEndTime;
                        isDoubleOtOnly = true;

                        // Over time eligible for rounding up by user defined amount when employee Thumb-Out was during evening double ot period
                        if (otEmployee.WorkShift.HasPostDoubleOtRoundUp)
                        {
                            if (otEmployee.WorkShift.PostDoubleOtRoundValue > 0)
                                workOtEnd = this.overtimeRoundUp(workOtEnd.Value, TimeSpan.FromMinutes(otEmployee.WorkShift.PostDoubleOtRoundValue));
                            else
                                workOtEnd = this.overtimeRoundDown(workOtEnd.Value, TimeSpan.FromMinutes(-otEmployee.WorkShift.PostDoubleOtRoundValue));
                        }

                        if (otEmployee.HasEveningMaxOt)
                        {
                            // Even if the employee's post-double ot end up here, there could be extra-ot
                            this.hasPostExtraDoubleOt = true;
                            postExtraOtStart = workOtEnd.Value;
                            this.PostExtraOTCalculation(otEmployee);
                        }
                        #region MCN
                        isDoubleOt = true;
                        #endregion
                    }
                    else
                    {
                        // Though employee has done double ot work Thumb-Out was after double ot end time
                        this.workPostDoubleOtEndTime = otEmployee.WorkShift.PostDoubleOtTimeEnd;
                        workOtEnd = this.workPostDoubleOtEndTime;
                        #region MCN
                        isDoubleOt = true;
                        #endregion
                    }

                    this.workPostDoubleOtDuration = (int)this.workOtEnd.Value.Subtract(otEmployee.WorkShift.PostSingleOtTimeEnd).TotalSeconds;
                    postDoubleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.POST_DOUBLE_OT, attend_status = true });
                }
                if (otEmployee.WorkShift.HasPostTripleOT && !isSingleOtOnly && !isDoubleOtOnly)
                {
                    if (!isOtCompensate && otEmployee.WorkShift.HasPostNonOtCompensation)
                    {
                        // employee's post non ot time period is considered for post-triple ot work when post-single ot and post-double ot have not been allocated.
                        otEmployee.WorkShift.PostDoubleOtTimeEnd = otEmployee.WorkShift.PostDoubleOtTimeEnd.Subtract(TimeSpan.FromMinutes(otEmployee.WorkShift.PostNonOtCompensate));
                        isOtCompensate = true;

                        #region MCN Full day Post Triple OT

                        if ((otEmployee.WorkShift.ShiftOut.Subtract(otEmployee.WorkShift.ShiftIn).TotalMinutes) < 15)
                        {

                            otEmployee.WorkShift.PostDoubleOtTimeEnd = otEmployee.WorkShift.ShiftOut.Subtract(TimeSpan.FromMinutes(otEmployee.WorkShift.PostNonOtCompensate));

                        }
                        #endregion
                    }

                    #region MCN Full day Post Triple OT

                    if ((otEmployee.WorkShift.ShiftOut.Subtract(otEmployee.WorkShift.ShiftIn).TotalMinutes) < 15 && !isOtCompensate && !isSingleOt && !isDoubleOt)
                    {
                        // change employe post non ot time period 
                        if (otEmployee.ThumbIn < otEmployee.WorkShift.ShiftIn)
                            otEmployee.WorkShift.PostDoubleOtTimeEnd = otEmployee.WorkShift.ShiftIn;
                        else
                            otEmployee.WorkShift.PostDoubleOtTimeEnd = otEmployee.ThumbIn.Value;

                    }

                    #endregion

                    if (otEmployee.WorkLateOutTime <= otEmployee.WorkShift.PostTripleOtTimeEnd)
                    {
                        // employee's Thumb-Out was during the time period between the double ot end and triple ot end time
                        this.workPostTripleOtEndTime = otEmployee.WorkLateOutTime;
                        workOtEnd = this.workPostTripleOtEndTime;

                        // Over time eligible for rounding up by user defined amount when employee Thumb-Out was during evening triple ot period
                        if (otEmployee.WorkShift.HasPostTripleOtRoundUp)
                        {
                            if (otEmployee.WorkShift.PostTripleOtRoundValue > 0)
                                workOtEnd = this.overtimeRoundUp(workOtEnd.Value, TimeSpan.FromMinutes(otEmployee.WorkShift.PostTripleOtRoundValue));
                            else
                                workOtEnd = this.overtimeRoundDown(workOtEnd.Value, TimeSpan.FromMinutes(-otEmployee.WorkShift.PostTripleOtRoundValue));
                        }

                        if (otEmployee.HasEveningMaxOt)
                        {
                            // Even if the employee's post-double ot end up here, there could be extra-ot
                            this.hasPostExtraTripleOt = true;
                            postExtraOtStart = workOtEnd.Value;
                            this.PostExtraOTCalculation(otEmployee);
                        }
                    }
                    else
                    {
                        // Though employee has done triple ot work, Thumb-Out was after triple ot end time
                        this.workPostTripleOtEndTime = otEmployee.WorkShift.PostTripleOtTimeEnd;
                        workOtEnd = this.workPostTripleOtEndTime;
                    }

                    this.workPostTripleOtDuration = (int)this.workOtEnd.Value.Subtract(otEmployee.WorkShift.PostDoubleOtTimeEnd).TotalSeconds;
                    postTripleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.PRE_TRIPLE_OT, attend_status = true });
                }
            }
        }

        #region Open Shift
        void openPostOTCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.IsLateOut)
            {

                if (otEmployee.WorkShift.OpenShiftNoOtDuration > 0 && openShiftWorkLateOutDuration > 0)
                {
                    if (openShiftWorkLateOutDuration - otEmployee.WorkShift.OpenShiftNoOtDuration > 0)
                    {
                        openShiftWorkLateOutDuration -= otEmployee.WorkShift.OpenShiftNoOtDuration;

                    }
                    else
                        openShiftWorkLateOutDuration = 0;
                }
                if (otEmployee.WorkShift.OpenShiftSingleOtDuration > 0 && openShiftWorkLateOutDuration > 0)
                {
                    if (openShiftWorkLateOutDuration - otEmployee.WorkShift.OpenShiftSingleOtDuration > 0)
                    {
                        openShiftWorkLateOutDuration -= otEmployee.WorkShift.OpenShiftSingleOtDuration;
                        this.workPostSingleOtDuration = (int)otEmployee.WorkShift.OpenShiftSingleOtDuration;
                    }
                    else
                    {
                        this.workPostSingleOtDuration = (int)(openShiftWorkLateOutDuration);
                        openShiftWorkLateOutDuration = 0;
                    }
                    postSingleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.POST_SINGLE_OT, attend_status = true });
                }
                if (otEmployee.WorkShift.OpenShiftDoubleOtDuration > 0 && openShiftWorkLateOutDuration > 0)
                {
                    if (openShiftWorkLateOutDuration - otEmployee.WorkShift.OpenShiftDoubleOtDuration > 0)
                    {
                        openShiftWorkLateOutDuration -= otEmployee.WorkShift.OpenShiftDoubleOtDuration;
                        this.workPostDoubleOtDuration = (int)otEmployee.WorkShift.OpenShiftDoubleOtDuration;
                    }
                    else
                    {
                        this.workPostDoubleOtDuration = (int)(openShiftWorkLateOutDuration);
                        openShiftWorkLateOutDuration = 0;
                    }
                    postDoubleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.POST_DOUBLE_OT, attend_status = true });
                }
                if (otEmployee.WorkShift.OpenShiftTripleOtDuration > 0 && openShiftWorkLateOutDuration > 0)
                {
                    if (openShiftWorkLateOutDuration - otEmployee.WorkShift.OpenShiftTripleOtDuration > 0)
                    {
                        openShiftWorkLateOutDuration -= otEmployee.WorkShift.OpenShiftTripleOtDuration;
                        this.workPostTripleOtDuration = (int)otEmployee.WorkShift.OpenShiftTripleOtDuration;
                    }
                    else
                    {
                        this.workPostTripleOtDuration = (int)(openShiftWorkLateOutDuration);
                        openShiftWorkLateOutDuration = 0;
                    }
                    postTripleOTWorkDone = true;
                    otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.PRE_TRIPLE_OT, attend_status = true });
                }
            }

        }

        #endregion

        #region Extra Over Time Calculation Methods

        #region Pre Extra-OT

        void PreExtraOTCalculation(AttendEmployee otEmployee)
        {
            if (hasPreExtraSingleOt)
            {
                this.PreExtraSingleOtCalculation(otEmployee);
            }
            if (hasPreExtraDoubleOt)
            {
                this.PreExtraDoubleOtCalculation(otEmployee);
            }
            if (hasPreExtraTripleOt)
            {
                this.PreExtraTripleOtCalculation(otEmployee);
            }
        }

        void PreExtraSingleOtCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.ActualIn < otEmployee.WorkShift.PreSingleOtTimeStart)
            {
                // employee's extra-ot has started before pre-single ot time period
                this.preSingleExtraOtDuration = (int)otEmployee.WorkShift.PreSingleOtTimeStart.Subtract(preExtraOtStart).TotalSeconds;
                preExtraOtStart = otEmployee.WorkShift.PreSingleOtTimeStart;
                if (otEmployee.WorkShift.HasPreDoubleOT)
                    this.hasPreExtraDoubleOt = true;
                else if (otEmployee.WorkShift.HasPreTripleOT)
                    this.hasPreExtraTripleOt = true;
            }
            else
            {
                // employee's extra-ot has ended up during pre-single ot time period
                DateTime extraOtEnd = otEmployee.ActualIn.Value;

                //Extra Over-Time eligible for employee rounding up/down by user defined amount 
                //when employee Actual-In was during morning single ot period

                if (otEmployee.WorkShift.HasPreSingleOtRoundUp)
                {
                    if (otEmployee.WorkShift.PreSingleOtRoundValue > 0)
                        extraOtEnd = overtimeRoundUp(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PreSingleOtRoundValue));
                    else
                        extraOtEnd = overtimeRoundDown(extraOtEnd, TimeSpan.FromMinutes(-otEmployee.WorkShift.PreSingleOtRoundValue));

                }
                this.preSingleExtraOtDuration = (int)extraOtEnd.Subtract(preExtraOtStart).TotalSeconds;
            }
        }

        void PreExtraDoubleOtCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.ActualIn < otEmployee.WorkShift.PreDoubleOtTimeStart)
            {
                // employee's extra-ot has started before pre-double ot time period
                this.preDoubleExtraOtDuration = (int)otEmployee.WorkShift.PreDoubleOtTimeStart.Subtract(preExtraOtStart).TotalSeconds;
                preExtraOtStart = otEmployee.WorkShift.PreDoubleOtTimeStart;
                if (otEmployee.WorkShift.HasPreTripleOT)
                    this.hasPreExtraTripleOt = true;
            }
            else
            {
                // employee's extra-ot has ended up during pre-double ot time period
                DateTime extraOtEnd = otEmployee.ActualIn.Value;

                //Extra Over-Time eligible for employee rounding up/down by user defined amount 
                //when employee Actual-In was during morning double ot period
                if (otEmployee.WorkShift.HasPreDoubleOtRoundUp)
                {
                    if (otEmployee.WorkShift.PreDoubleOtRoundValue > 0)
                        extraOtEnd = this.overtimeRoundUp(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PreDoubleOtRoundValue));
                    else
                        extraOtEnd = this.overtimeRoundDown(extraOtEnd, TimeSpan.FromMinutes(-otEmployee.WorkShift.PreDoubleOtRoundValue));
                }


                this.preDoubleExtraOtDuration = (int)extraOtEnd.Subtract(preExtraOtStart).TotalSeconds;
            }
        }

        void PreExtraTripleOtCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.ActualIn < otEmployee.WorkShift.PreTripleOtTimeStart)
            {
                // employee's extra-ot has started before pre-triple ot time period
                this.preTripleExtraOtDuration = (int)otEmployee.WorkShift.PreTripleOtTimeStart.Subtract(preExtraOtStart).TotalSeconds;
                preExtraOtStart = otEmployee.WorkShift.PreTripleOtTimeStart;
            }
            else
            {
                // employee's extra-ot has ended up during pre-triple ot time period
                DateTime extraOtEnd = otEmployee.ActualIn.Value;

                //Extra Over-Time eligible for employee rounding up/down by user defined amount 
                //when employee Actual-In was during morning triple ot period
                if (otEmployee.WorkShift.HasPreTripleOtRoundUp)
                {
                    if (otEmployee.WorkShift.PreTripleOtRoundValue > 0)
                        extraOtEnd = this.overtimeRoundUp(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PreTripleOtRoundValue));
                    else
                        extraOtEnd = this.overtimeRoundDown(extraOtEnd, TimeSpan.FromMinutes(-otEmployee.WorkShift.PreTripleOtRoundValue));
                }


                this.preTripleExtraOtDuration = (int)extraOtEnd.Subtract(preExtraOtStart).TotalSeconds;
            }
        }

        #endregion

        #region Post Extra-OT

        void PostExtraOTCalculation(AttendEmployee otEmployee)
        {
            if (hasPostExtraSingleOt)
            {
                this.PostExtraSingleOtCalculation(otEmployee);
            }

            if (hasPostExtraDoubleOt)
            {
                this.PostExtraDoubleOtCalculation(otEmployee);
            }

            if (hasPostExtraTripleOt)
            {
                this.PostExtraTripleOtCalculation(otEmployee);
            }
        }

        void PostExtraSingleOtCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.ActualOut > otEmployee.WorkShift.PostSingleOtTimeEnd)
            {
                // employee's extra-ot has ended after post-single ot time period
                this.postSingleExtraOtDuration = (int)otEmployee.WorkShift.PostSingleOtTimeEnd.Subtract(postExtraOtStart).TotalSeconds;
                postExtraOtStart = otEmployee.WorkShift.PostSingleOtTimeEnd;
                if (otEmployee.WorkShift.HasPostDoubleOT)
                    this.hasPostExtraDoubleOt = true;
                else if (otEmployee.WorkShift.HasPostTripleOT)
                    this.hasPostExtraTripleOt = true;
            }
            else
            {
                // employee's extra-ot has ended up during post-single ot time period
                DateTime extraOtEnd = otEmployee.ActualOut.Value;

                //Extra Over-Time eligible for employee rounding up/down by user defined amount 
                //when employee Actual-Out was during evening single ot period
                if (otEmployee.WorkShift.HasPostSingleOtRoundUp)
                {
                    if (otEmployee.WorkShift.PostSingleOtRoundValue > 0)
                        extraOtEnd = this.overtimeRoundUp(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PostSingleOtRoundValue));
                    else
                        extraOtEnd = this.overtimeRoundDown(extraOtEnd, TimeSpan.FromMinutes(-otEmployee.WorkShift.PostSingleOtRoundValue));
                }
                this.postSingleExtraOtDuration = (int)extraOtEnd.Subtract(postExtraOtStart).TotalSeconds;
            }
        }

        void PostExtraDoubleOtCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.ActualOut > otEmployee.WorkShift.PostDoubleOtTimeEnd)
            {
                // employee's extra-ot has ended after post-double ot time period
                this.postDoubleExtraOtDuration = (int)otEmployee.WorkShift.PostDoubleOtTimeEnd.Subtract(postExtraOtStart).TotalSeconds;
                postExtraOtStart = otEmployee.WorkShift.PostDoubleOtTimeEnd;
                if (otEmployee.WorkShift.HasPostTripleOT)
                    this.hasPostExtraTripleOt = true;
            }
            else
            {
                // employee's extra-ot has ended up during post-double ot time period
                DateTime extraOtEnd = otEmployee.ActualOut.Value;

                //Extra Over-Time eligible for employee rounding up/down by user defined amount 
                //when employee Actual-Out was during evening double ot period
                if (otEmployee.WorkShift.HasPostDoubleOtRoundUp)
                {
                    if (otEmployee.WorkShift.PostDoubleOtRoundValue > 0)
                        extraOtEnd = this.overtimeRoundUp(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PostDoubleOtRoundValue));
                    else
                        extraOtEnd = this.overtimeRoundDown(extraOtEnd, TimeSpan.FromMinutes(-otEmployee.WorkShift.PostDoubleOtRoundValue));
                }
                this.postDoubleExtraOtDuration = (int)extraOtEnd.Subtract(postExtraOtStart).TotalSeconds;
            }
        }

        void PostExtraTripleOtCalculation(AttendEmployee otEmployee)
        {
            if (otEmployee.ActualOut > otEmployee.WorkShift.PostTripleOtTimeEnd)
            {
                // employee's extra-ot has ended after post-triple ot time period
                this.postTripleExtraOtDuration = (int)otEmployee.WorkShift.PostTripleOtTimeEnd.Subtract(postExtraOtStart).TotalSeconds;
                postExtraOtStart = otEmployee.WorkShift.PostTripleOtTimeEnd;
            }
            else
            {
                // employee's extra-ot has ended up during post-triple ot time period
                DateTime extraOtEnd = otEmployee.ActualOut.Value;

                //Extra Over-Time eligible for employee rounding up/down by user defined amount 
                //when employee Actual-Out was during evening triple ot period
                if (otEmployee.WorkShift.HasPostTripleOtRoundUp)
                {
                    if (otEmployee.WorkShift.PostTripleOtRoundValue > 0)
                        extraOtEnd = this.overtimeRoundUp(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PostTripleOtRoundValue));
                    else
                        extraOtEnd = this.overtimeRoundDown(extraOtEnd, TimeSpan.FromMinutes(otEmployee.WorkShift.PostTripleOtRoundValue));
                }
                this.postTripleExtraOtDuration = (int)extraOtEnd.Subtract(postExtraOtStart).TotalSeconds;
            }
        }

        #endregion

        #endregion

        #region Dinemore Overtime Methods

        public void CalculateOvertimeBasedOnWork(AttendEmployee otEmployee)
        {
            // Though employees are worked during over time assigned periods total work duration of 9 hrs or more should be reached
            // to entitled for over time
            // Employee's actual work time is calculated as (total work time between Emp-In and Emp-Out - emp breaks)
            // Also during the usual overtime calculation lower limit of ot and upper limit of ot is considered
            // Apart from that for the specific split shift we need to consider ot done within the split break
            if (otEmployee.ActualTotalWorkDuration < new TimeSpan(9, 0, 0).TotalSeconds)
            {
                // void the calculated over-time because emp not completed his total work hours
                if (preSingleOTWorkDone)
                {
                    this.workPreSingleOtDuration = 0;
                    var attendStat = otEmployee.AttendStatusList.FirstOrDefault(c => c.attend_status_id == AttendStatus.PRE_SINGLE_OT);
                    otEmployee.AttendStatusList.Remove(attendStat);
                }
                if (preDoubleOTWorkDone)
                {
                    this.workPreDoubleOtDuration = 0;
                    var attendStat = otEmployee.AttendStatusList.FirstOrDefault(c => c.attend_status_id == AttendStatus.PRE_DOUBLE_OT);
                    otEmployee.AttendStatusList.Remove(attendStat);
                }

                if (preTripleOTWorkDone)
                {
                    this.workPreTripleOtDuration = 0;
                    var attendStat = otEmployee.AttendStatusList.FirstOrDefault(c => c.attend_status_id == AttendStatus.PRE_TRIPLE_OT);
                    otEmployee.AttendStatusList.Remove(attendStat);
                }

                if (postSingleOTWorkDone)
                {
                    this.workPostSingleOtDuration = 0;
                    var attendStat = otEmployee.AttendStatusList.FirstOrDefault(c => c.attend_status_id == AttendStatus.POST_SINGLE_OT);
                    otEmployee.AttendStatusList.Remove(attendStat);
                }

                if (postDoubleOTWorkDone)
                {
                    this.workPostDoubleOtDuration = 0;
                    var attendStat = otEmployee.AttendStatusList.FirstOrDefault(c => c.attend_status_id == AttendStatus.POST_DOUBLE_OT);
                    otEmployee.AttendStatusList.Remove(attendStat);
                }

                if (postTripleOTWorkDone)
                {
                    this.workPostTripleOtDuration = 0;
                    var attendStat = otEmployee.AttendStatusList.FirstOrDefault(c => c.attend_status_id == AttendStatus.POST_TRIPLE_OT);
                    otEmployee.AttendStatusList.Remove(attendStat);
                }
            }
            else
            {
                // employee is eligible for overtime
                // pre and post ot is already calculated as usual
                // ot for specific split-shift should be considered
                if (otEmployee.WorkShift.IsSplitShift)
                {
                    if (otEmployee.HasSplitBreakMorningOT || otEmployee.HasSplitBreakEveningOT)
                    {
                        // emp is also eligible for ot done during the split break
                        // OT done is considered as Single OT
                        if (otEmployee.HasSplitBreakMorningOT)
                        {
                            // emp continue his work from morning to split-break. So start on split-on to sign-in for break 
                            //could be considered as overtime
                            // overtime done is added to morning single ot duration
                            EmployeeBreak obtainedBreak = otEmployee.CapturedBreaks.FirstOrDefault(c => c.IsShiftBreak == true);
                            int otDuration = (int)obtainedBreak.BreakIn.Subtract(obtainedBreak.EmpAssignedBreak.ShiftBreakOn).TotalSeconds;
                            this.workPreSingleOtDuration += otDuration;
                            if (otEmployee.AttendStatusList.Count(c => c.attend_status_id == AttendStatus.PRE_SINGLE_OT) == 0)
                            {
                                otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.PRE_SINGLE_OT, attend_status = true });
                            }
                        }
                        else if (otEmployee.HasSplitBreakEveningOT)
                        {
                            // emp start his work during split-break and continue to evening part. So sign-out time from break to split-off time
                            //could be considered as overtime
                            // overtime done is added to evening single ot duration
                            EmployeeBreak obtainedBreak = otEmployee.CapturedBreaks.FirstOrDefault(c => c.IsShiftBreak == true);
                            int otDuration = (int)obtainedBreak.BreakOut.Subtract(obtainedBreak.EmpAssignedBreak.ShiftBreakOff).TotalSeconds;
                            this.workPostSingleOtDuration += otDuration;
                            if (otEmployee.AttendStatusList.Count(c => c.attend_status_id == AttendStatus.POST_SINGLE_OT) == 0)
                            {
                                otEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.POST_SINGLE_OT, attend_status = true });
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region OT Round Up Methods

        DateTime overtimeRoundUp(DateTime dt, TimeSpan d)
        {
            var delta = (d.Ticks - (dt.Ticks % d.Ticks));
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        DateTime overtimeRoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        #endregion

        #endregion
    }
}