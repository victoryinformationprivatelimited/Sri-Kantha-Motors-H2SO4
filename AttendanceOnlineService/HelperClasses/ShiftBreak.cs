using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class ShiftBreak
    {
        #region Constructor

        public ShiftBreak(dtl_Shift_Break_Details shiftBreak)
        {
            this.currentBreak = shiftBreak;
        }

        #endregion

        #region Data Members

        dtl_Shift_Break_Details currentBreak;

        #endregion

        #region Properties

        int shiftBreakID;
        public int ShiftBreakID
        {
            get { return shiftBreakID; }
            set { shiftBreakID = value; }
        }

        DateTime shiftBreakOn;
        public DateTime ShiftBreakOn
        {
            get { return shiftBreakOn; }
            set { shiftBreakOn = value; }
        }

        DateTime shiftBreakOff;
        public DateTime ShiftBreakOff
        {
            get { return shiftBreakOff; }
            set { shiftBreakOff = value; }
        }

        DateTime shiftBreakIn;
        public DateTime ShiftBreakIn
        {
            get { return shiftBreakIn; }
            set { shiftBreakIn = value; }
        }

        DateTime shiftBreakOut;
        public DateTime ShiftBreakOut
        {
            get { return shiftBreakOut; }
            set { shiftBreakOut = value; }
        }

        bool isFreeBreakZone;
        public bool IsFreeBreakZone
        {
            get { return isFreeBreakZone; }
            set { isFreeBreakZone = value; }
        }

        bool isFirstFreeBreakZone;
        public bool IsFirstFreeBreakZone
        {
            get { return isFirstFreeBreakZone; }
            set { isFirstFreeBreakZone = value; }
        }

        bool isLastFreeBreakZone;
        public bool IsLastFreeBreakZone
        {
            get { return isLastFreeBreakZone; }
            set { isLastFreeBreakZone = value; }
        }

        #endregion

        #region Methods
        
        public void ConfigureShiftBreak(ShiftConfiguration currentShift)
        {
            DateTime breakInDate = (DateTime)currentShift.CurrentDate;
            shiftBreakOn = breakInDate.AddDays((double)currentBreak.break_on_day_value).Add((TimeSpan)currentBreak.break_on_time);
            shiftBreakIn = breakInDate.AddDays((double)currentBreak.break_in_day_value).Add((TimeSpan)currentBreak.break_in_time);
            shiftBreakOff = breakInDate.AddDays((double)currentBreak.break_off_day_value).Add((TimeSpan)currentBreak.break_off_time);
            shiftBreakOut = breakInDate.AddDays((double)currentBreak.break_out_day_value).Add((TimeSpan)currentBreak.break_out_time);
        }

        #endregion
    }
}