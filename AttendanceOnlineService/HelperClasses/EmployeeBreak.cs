using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class EmployeeBreak
    {
        #region Properties

        ShiftBreak empAssignedBreak;
        public ShiftBreak EmpAssignedBreak
        {
            get { return empAssignedBreak; }
            set { empAssignedBreak = value; }
        }

        DateTime breakIn;
        public DateTime BreakIn
        {
            get { return breakIn; }
            set { breakIn = value; }
        }

        DateTime breakOut;
        public DateTime BreakOut
        {
            get { return breakOut; }
            set { breakOut = value; }
        }

        List<dtl_AttendanceData> empBreakAttendance;
        public List<dtl_AttendanceData> EmpBreakAttendance
        {
            get { return empBreakAttendance; }
            set { empBreakAttendance = value; }
        }

        // return duration as seconds
        int breakDuration;
        public int BreakDuration
        {
            get
            { 
                if(breakIn != null && breakOut != null)
                {
                    breakDuration = (int)breakOut.Subtract(breakIn).TotalSeconds;
                }
                else
                {
                    breakDuration = 0;
                }
                return breakDuration; 
            }
            
        }

        bool isBreakInvalid;
        public bool IsBreakInvalid
        {
            get { return isBreakInvalid; }
            set { isBreakInvalid = value; }
        }

        bool isShiftBreak;
        public bool IsShiftBreak
        {
            get { return isShiftBreak; }
            set { isShiftBreak = value; }
        }

        bool isFreeBreak;
        public bool IsFreeBreak
        {
            get { return isFreeBreak; }
            set { isFreeBreak = value; }
        }

        bool hasEmpInBreak;
        public bool HasEmpInBreak
        {
            get { return hasEmpInBreak; }
            set { hasEmpInBreak = value; }
        }

        bool hasEmpOutBreak;
        public bool HasEmpOutBreak
        {
            get { return hasEmpOutBreak; }
            set { hasEmpOutBreak = value; }
        }

        bool hasOnlyOneBreakThumb;
        public bool HasOnlyOneBreakThumb
        {
            get { return hasOnlyOneBreakThumb; }
            set { hasOnlyOneBreakThumb = value; }
        }

        #endregion

        #region Methods

        public void CheckEmployeeBreak(AttendEmployee breakEmployee)
        {
            // Break has On and Off time. Also In and Out time
            // Employee could be consume whole break time or more than he has been allocated.

            // If employee consume whole break time Break-In Thumb and Break-Out Thumb should be found between
            // break on time and break off time. Then time duration between Break In and Break Out time is considered as 
            // consumed break duration.
            
        }

        private void CheckEmployeeBreakWithOverTime()
        {

        }

        private void CheckBreakWithPreOverTime()
        {

        }

        private void CheckBreakWithPostOverTime()
        {

        }

        #endregion
    }
}