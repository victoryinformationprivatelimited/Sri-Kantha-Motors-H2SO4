using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class EmployeeAttendanceData
    {
        public EmployeeAttendanceData(Guid employeeid)
        {
            Employee_id = employeeid;
            SLCount = 0;
            LateCount = 0;
        }

        private int _LateCount;

        public int LateCount
        {
            get { return _LateCount; }
            set { _LateCount = value; }
        }

        private int _SLCount;

        public int SLCount
        {
            get { return _SLCount; }
            set { _SLCount = value; }
        }

        private Guid _Employee_id;

        public Guid Employee_id
        {
            get { return _Employee_id; }
            set { _Employee_id = value; }
        }


    }
}