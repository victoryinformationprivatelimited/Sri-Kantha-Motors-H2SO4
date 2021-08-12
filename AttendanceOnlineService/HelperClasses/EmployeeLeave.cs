using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttendanceData;

namespace AttendanceOnlineService.HelperClasses
{
    public class EmployeeLeave
    {
        #region Properties

        string leaveCode;
        public string LeaveCode
        {
            get { return leaveCode; }
            set { leaveCode = value; }
        }

        z_LeaveType leaveType;
        public z_LeaveType LeaveType
        {
            get { return leaveType; }
            set { leaveType = value; }
        }

        leaveoption obtainedLeaveType;
        public leaveoption ObtainedLeaveType
        {
            get { return obtainedLeaveType; }
            set { obtainedLeaveType = value; }
        }

        trns_LeavePool applyLeave;
        public trns_LeavePool ApplyLeave
        {
            get { return applyLeave; }
            set { applyLeave = value; }
        }

        List<trns_LeavePool> coveringLeaves;
        public List<trns_LeavePool> CoveringLeaves
        {
            get { return coveringLeaves; }
            set { coveringLeaves = value; }
        }

        #region Leave Status Properties

        bool isAuthorized;
        public bool IsAuthorized
        {
            get { return isAuthorized; }
            set { isAuthorized = value; }
        }

        bool isUnAuthorized;
        public bool IsUnAuthorized
        {
            get { return isUnAuthorized; }
            set { isUnAuthorized = value; }
        }

        bool isOfficialLeave;
        public bool IsOfficialLeave
        {
            get { return isOfficialLeave; }
            set { isOfficialLeave = value; }
        }

        bool hasCoveringLeaves;
        public bool HasCoveringLeaves
        {
            get { return hasCoveringLeaves; }
            set { hasCoveringLeaves = value; }
        }

        #endregion

        // h 2020-09-18
        //private bool isMorSL;

        //public bool IsMorSL
        //{
        //    get { return isMorSL; }
        //    set { isMorSL = value; }
        //}

        //private bool isEveSL;

        //public bool IsEveSL
        //{
        //    get { return isEveSL; }
        //    set { isEveSL = value; }
        //}

        #endregion

        #region Methods

        public void CheckEmployeeLeave(AttendEmployee leaveEmployee)
        {
            bool isHandled = false;
            if (leaveEmployee.IsLateIn)
            {
                isHandled = CaptureLateInLeaves(leaveEmployee);
                if (isHandled)// && !isMorSL)
                {
                    leaveEmployee.IsLateIn = false;
                    leaveEmployee.WorkLateInDuration = 0;
                }
            }

            if (!isHandled && leaveEmployee.IsEarlyOut)
            {
                isHandled = CaptureEarlyOutLeaves(leaveEmployee);
                if (isHandled)// && !isEveSL)
                {
                    leaveEmployee.IsEarlyOut = false;
                    leaveEmployee.WorkEarlyOutDuration = 0;
                }
            }

            if (!isHandled && leaveEmployee.IsAttendanceAbsent)
            {
                isHandled = CaptureAbsentLeaves(leaveEmployee);
            }

            // h 2020-09-18
            #region leave old
            //if (leaveEmployee.ApproveLeave.Count > 0)
            //{
            //    List<trns_LeavePool> remainApplyLeaves = leaveEmployee.ApproveLeave.Where(c => !leaveEmployee.CapturedLeaves.Where(d => d.applyLeave != null || d.hasCoveringLeaves).Any(g => g.applyLeave.pool_id == c.pool_id || g.coveringLeaves.Any(x => x.pool_id == c.pool_id))).ToList();
            //    var currentLeave = remainApplyLeaves.FirstOrDefault(c => c.shift_detail_id == leaveEmployee.WorkShift.EmployeeShift.shift_detail_id && c.leave_type_id == clsLeave.GetLeaveOption(obtainedLeaveType));
            //    if (currentLeave != null)  // Employee has been approved for a leave for current shift work day.
            //    {
            //        applyLeave = currentLeave;
            //        Guid? leaveTypeID = currentLeave.leave_type_id;
            //        Guid? leaveDetailID = currentLeave.leave_detail_id;
            //        isOfficialLeave = applyLeave.z_LeaveCategory.is_official.Value;
            //        isAuthorized = true;
            //    }
            //    else
            //    {
            //        // though employee has approved leaves for the day he obtained different type of leave than applied one.

            //        // But this obtained leave type might be covered from other one or more approved leaves eg: Election day full leave = election leave + halfday leave
            //        this.hasCoveringLeaves = this.CaptureCoveringLeaves(remainApplyLeaves);
            //        if (hasCoveringLeaves)
            //        {
            //            // Covering leaves are found
            //            isAuthorized = true;
            //        }
            //        else
            //        {
            //            // No approve leave or covering leaves are found
            //            isUnAuthorized = true;
            //        }
            //    }
            //} 
            #endregion
            if (leaveEmployee.ApproveLeave.Count > 0)
            {
                List<trns_LeavePool> remainApplyLeaves = leaveEmployee.ApproveLeave.Where(c => !leaveEmployee.CapturedLeaves.Where(d => d.applyLeave != null || d.hasCoveringLeaves).Any(g => g.applyLeave.pool_id == c.pool_id || g.coveringLeaves.Any(x => x.pool_id == c.pool_id))).ToList();
                var currentLeaves = remainApplyLeaves.Where(c => c.shift_detail_id == leaveEmployee.WorkShift.EmployeeShift.shift_detail_id);


                if (currentLeaves != null)  // Employee has been approved for a leave for current shift work day.
                {
                    decimal obtainleavevalue = (decimal)clsLeave.GetLeaveValue(obtainedLeaveType);
                    // h 2020-10-14 get sick leave value
                    //decimal leavecheck = checkAppliedLeaves(currentLeaves.ToList(), obtainleavevalue);
                    decimal leavecheck = checkAppliedLeaves(currentLeaves.ToList(), obtainleavevalue, leaveEmployee);

                    if (currentLeaves.Count() == 1) // multiple leaves cannot be official
                        applyLeave = currentLeaves.First();
                    else
                    {
                        applyLeave = new trns_LeavePool();
                        applyLeave.z_LeaveCategory = new z_LeaveCategory();
                        applyLeave.z_LeaveCategory.is_official = false;
                    }

                    if (leavecheck < 0) // apply over
                    {
                        isAuthorized = true;
                    }
                    else if (leavecheck == 0) // leave amount matched
                    {
                        isAuthorized = true;
                    }
                    else if (obtainleavevalue == 1 && leavecheck == (decimal)0.5) // fd -> hd => no pay hd
                    {
                        isUnAuthorized = true;
                        obtainedLeaveType = leaveoption.EVENING_HALFDAY_LEAVE;
                    }
                    else // also include fd -> sl as no pay fd
                    {
                        isUnAuthorized = true;
                    }
                    // h 2020-09-18 genext sl ?
                    //else if (obtainleavevalue == 1 && leavecheck == (decimal)0.75) // fd -> sl+hd => no pay sl
                    //{
                    //    isUnAuthorized = true;
                    //    obtainedLeaveType = leaveoption.EVENING_SHORT_LEAVE;
                    //}
                    //else if (obtainleavevalue == (decimal)0.5 && leavecheck == (decimal)0.25) // hd -> sl => no pay sl
                    //{
                    //    isUnAuthorized = true;
                    //    obtainedLeaveType = leaveoption.EVENING_SHORT_LEAVE;
                    //}
                }
                else
                {
                    // though employee has approved leaves for the day he obtained different type of leave than applied one.

                    // But this obtained leave type might be covered from other one or more approved leaves eg: Election day full leave = election leave + halfday leave
                    this.hasCoveringLeaves = this.CaptureCoveringLeaves(remainApplyLeaves);
                    if (hasCoveringLeaves)
                    {
                        // Covering leaves are found
                        isAuthorized = true;
                    }
                    else
                    {
                        // No approve leave or covering leaves are found
                        isUnAuthorized = true;
                    }
                }
            }
            else
            {
                // h 2020-09-24 
                //isUnAuthorized = true;   // employee has not been approved for leaves on the day.
                //if (!isMorSL && !isEveSL)
                //{
                    isUnAuthorized = true;
                //}
            }

            // h 2020-09-18
            //if (isMorSL && isAuthorized)
            //{
            //    leaveEmployee.IsLateIn = false;
            //    leaveEmployee.WorkLateInDuration = 0;
            //}
            //else if (isEveSL && isAuthorized)
            //{
            //    leaveEmployee.IsEarlyOut = false;
            //    leaveEmployee.WorkEarlyOutDuration = 0;
            //}

            this.SetEmployeeLeaveStatus(leaveEmployee);
            this.SetEmployeeBasicLeaveStatus(leaveEmployee);
        }

        bool CaptureLateInLeaves(AttendEmployee leaveEmployee)
        {
            // Checking for late in short leave - morning
            if (leaveEmployee.IsMorningShortLeave && !leaveEmployee.IsMorningShortLeaveHandled)
            {
                obtainedLeaveType = leaveoption.MORNING_SHORT_LEAVE;
                leaveEmployee.IsMorningShortLeaveHandled = true;
                //isMorSL = true; //h 2020-09-18
                return true;
            }

            // Checking for late in halfday leave - morning
            if (leaveEmployee.IsMorningHalfDayLeave && !leaveEmployee.IsMorningHalfDayLeaveHandled)
            {
                obtainedLeaveType = leaveoption.MORNING_HALFDAY_LEAVE;
                leaveEmployee.IsMorningHalfDayLeaveHandled = true;
                return true;
            }

            // Checking for late in fullday leave - morning
            if (leaveEmployee.IsMorningFullDayLeave && !leaveEmployee.IsMorningFulldayLeaveHandled)
            {
                obtainedLeaveType = leaveoption.MORNING_FULL_LEAVE;
                leaveEmployee.IsMorningFulldayLeaveHandled = true;
                return true;
            }
            return false;
        }

        bool CaptureEarlyOutLeaves(AttendEmployee leaveEmployee)
        {
            // Checking for early out short leave - evening
            if (leaveEmployee.IsEveningShortLeave && !leaveEmployee.IsEveningShortLeaveHandled)
            {
                obtainedLeaveType = leaveoption.EVENING_SHORT_LEAVE;
                leaveEmployee.IsEveningShortLeaveHandled = true;
                //isEveSL = true; //h 2020-09-24
                return true;
            }

            // Checking for early out halfday leave - evening
            if (leaveEmployee.IsEveningHalfDayLeave && !leaveEmployee.IsEveningHalfDayLeaveHandled)
            {
                obtainedLeaveType = leaveoption.EVENING_HALFDAY_LEAVE;
                leaveEmployee.IsEveningHalfDayLeaveHandled = true;
                return true;
            }

            // Checking for early out fullday leave - evening
            if (leaveEmployee.IsEveningFullDayLeave && !leaveEmployee.IsEveningFulldayLeaveHandled)
            {
                obtainedLeaveType = leaveoption.EVENING_FULL_LEAVE;
                leaveEmployee.IsEveningFulldayLeaveHandled = true;
                return true;
            }
            return false;

        }

        bool CaptureAbsentLeaves(AttendEmployee leaveEmployee)
        {
            if (!leaveEmployee.IsAbsentLeaveHandled)
            {
                if (leaveEmployee.WorkShift.CurrentDate.Value.DayOfWeek == DayOfWeek.Saturday && leaveEmployee.WorkShift.EmployeeShift.is_halfday == true)
                {
                    obtainedLeaveType = leaveoption.MORNING_HALFDAY_LEAVE;
                    leaveEmployee.IsAbsentLeaveHandled = true;
                    return true;
                }
                else
                {
                    obtainedLeaveType = leaveoption.MORNING_FULL_LEAVE;
                    leaveEmployee.IsAbsentLeaveHandled = true;
                    return true;
                }
            }
            return true;
        }

        void SetEmployeeLeaveStatus(AttendEmployee leaveEmployee)
        {
            if (obtainedLeaveType == leaveoption.MORNING_SHORT_LEAVE)
            {
                if (isAuthorized)
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_SHORT_LEAVE, attend_status = true });
                }
                else
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_SHORT_LEAVE_UNAUTHORIZED, attend_status = true });
                }
            }
            else if (obtainedLeaveType == leaveoption.MORNING_HALFDAY_LEAVE)
            {
                if (isAuthorized)
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_HALFDAY, attend_status = true });
                }
                else
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_HALFDAY_UNAUTHORIZED, attend_status = true });
                }
            }
            else if (obtainedLeaveType == leaveoption.MORNING_FULL_LEAVE)
            {
                if (isAuthorized)
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_FULLDAY, attend_status = true });
                }
                else
                {
                    if (leaveEmployee.WorkShift.CurrentDate.Value.DayOfWeek == DayOfWeek.Saturday && leaveEmployee.WorkShift.EmployeeShift.is_halfday == true)
                    {

                        leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_HALFDAY_UNAUTHORIZED, attend_status = true });
                    }
                    else
                        leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.MORNING_FULLDAY_UNAUTHORIZED, attend_status = true });
                }
            }
            else if (obtainedLeaveType == leaveoption.EVENING_SHORT_LEAVE)
            {
                if (isAuthorized)
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EVENING_SHORT_LEAVE, attend_status = true });
                }
                else
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EVENING_SHORT_LEAVE_UNAUTHORIZED, attend_status = true });
                }
            }
            else if (obtainedLeaveType == leaveoption.EVENING_HALFDAY_LEAVE)
            {
                if (isAuthorized)
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EVENING_HALFDAY, attend_status = true });
                }
                else
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EVENING_HALFDAY_UNAUTHORIZED, attend_status = true });
                }
            }
            else if (obtainedLeaveType == leaveoption.EVENING_FULL_LEAVE)
            {
                if (isAuthorized)
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EVENING_FULLDAY, attend_status = true });
                }
                else
                {
                    leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.EVENING_FULLDAY_UNAUTHORIZED, attend_status = true });
                }
            }
        }

        void SetEmployeeBasicLeaveStatus(AttendEmployee leaveEmployee)
        {
            //if (!isMorSL || !isEveSL)
            //{
                if (isAuthorized)
                {

                    if (leaveEmployee.AttendStatusList.Count(c => c.attend_status_id == AttendStatus.AUTHORIZED_LEAVE) == 0)
                    {
                        leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.AUTHORIZED_LEAVE, attend_status = true });
                    }
                    if (applyLeave.z_LeaveCategory.is_official.Value == false)
                    {
                        if (leaveEmployee.AttendStatusList.Count(c => c.attend_status_id == AttendStatus.UNOFFICIAL_LEAVE) == 0)
                        {
                            leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.UNOFFICIAL_LEAVE, attend_status = true });
                        }
                    }
                }

                else if (isUnAuthorized)
                {
                    if (leaveEmployee.AttendStatusList.Count(c => c.attend_status_id == AttendStatus.UNAUTHORIZED_LEAVE) == 0)
                    {
                        leaveEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.UNAUTHORIZED_LEAVE, attend_status = true });
                    }
                }
            //}
        }

        bool CaptureCoveringLeaves(List<trns_LeavePool> availableLeaveList)
        {
            //if (availableLeaveList.Count > 0)
            //{
            //    List<trns_LeavePool> currentList = availableLeaveList.Where(c => c.z_LeaveType.value < leaveType.value).OrderByDescending(c => c.z_LeaveType.value).ToList();
            //    List<trns_LeavePool> foundLeaves = new List<trns_LeavePool>();
            //    decimal valueCount = 0M;
            //    foreach (trns_LeavePool item in currentList)
            //    {
            //        valueCount += (decimal)item.z_LeaveType.value;
            //        foundLeaves.Add(item);
            //        if (valueCount == leaveType.value)
            //        {
            //            this.coveringLeaves = foundLeaves;
            //            return true;
            //        }
            //    }
            //}
            return false;
        }

        // h 2020-03-03
        decimal checkAppliedLeaves(List<trns_LeavePool> leaves, decimal nopayvalue, AttendEmployee emp)
        {
            decimal leavevaluetotal = 0;
            foreach (trns_LeavePool leave in leaves)
            {
                leavevaluetotal += (decimal)leave.z_LeaveType.value;
            }

            return nopayvalue - leavevaluetotal;
        }

        #endregion
    }
}