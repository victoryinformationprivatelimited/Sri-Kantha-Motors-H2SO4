using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class EmployeeHoliday
    {
        #region Properties

        string holidayName;
        public string HolidayName
        {
            get { return holidayName; }
            set { holidayName = value; }
        }

        z_HolidayData assignedHoliday;
        public z_HolidayData AssignedHoliday
        {
            get { return assignedHoliday; }
            set { assignedHoliday = value; }
        }

        bool isHolidayStatusSet;
        public bool IsHolidayStatusSet
        {
            get { return isHolidayStatusSet; }
            set { isHolidayStatusSet = value; }
        }

        #endregion

        #region Methods

        public void CheckEmployeeHoliday(AttendEmployee holidayEmployee)
        {
            // Get currently available assigned holidays
            List<z_HolidayData> remainHolidays = holidayEmployee.AssignedHolidays.Where(c => !holidayEmployee.CapturedHolidays.Where(d => d.assignedHoliday != null).Any(d => d.assignedHoliday.holiday_id == c.holiday_id)).ToList();
            if (remainHolidays != null && remainHolidays.Count > 0)
            {
                // Get first of the assigned holiday list
                this.assignedHoliday = remainHolidays.FirstOrDefault();
                if (!isHolidayStatusSet)
                {
                    holidayEmployee.AttendStatusList.Add(new trns_ProcessedAttendanceStatus { attend_status_id = AttendStatus.HOLIDAY, attend_status = true });
                    isHolidayStatusSet = true;
                }
                    
            }
        } 

        #endregion

    }
}