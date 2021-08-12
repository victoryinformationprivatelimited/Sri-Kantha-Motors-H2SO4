using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public static class clsLeave
    {
        public static Guid GetLeaveOption(leaveoption option)
        {
            switch (option)
            {
                case leaveoption.MORNING_SHORT_LEAVE:
                    return new Guid("7329eb28-250e-4467-9bb4-8d42442c7154");
                case leaveoption.MORNING_HALFDAY_LEAVE:
                    return new Guid("7a565537-54a5-4e82-a147-14e393844c0e");
                case leaveoption.MORNING_FULL_LEAVE:
                    return new Guid("877a1ea2-af78-4544-8d95-5c37991e60c6");
                case leaveoption.EVENING_SHORT_LEAVE:
                    return new Guid("ed696e0f-83ad-4cec-a66f-d5456c09b305");
                case leaveoption.EVENING_HALFDAY_LEAVE:
                    return new Guid("c92a502d-6be1-4726-98d3-f4d704861062");
                case leaveoption.EVENING_FULL_LEAVE:
                    return new Guid("1876664d-ac3b-4b25-8549-1e17bbd70494");
                default:
                    return new Guid();
            }
        }

        public static double GetLeaveTypeValue(leaveType typeOfLeave)
        {
            switch (typeOfLeave)
            {
                case leaveType.MORNING_SL_VALUE:
                    return 0.27;
                case leaveType.MORNING_HD_VALUE:
                    return 0.50;
                case leaveType.MORNING_FD_VALUE:
                    return 1.0;
                case leaveType.EVENING_SL_VALUE:
                    return 0.27;
                case leaveType.EVENING_HD_VALUE:
                    return 0.50;
                case leaveType.EVENING_FD_VALUE:
                    return 1.0;
                default:
                    return 0;
            }
        }

        // h 2020-03-03
        public static double GetLeaveValue(leaveoption option)
        {
            switch (option)
            {
                case leaveoption.MORNING_SHORT_LEAVE:
                    return 0.25;
                case leaveoption.MORNING_HALFDAY_LEAVE:
                    return 0.50;
                case leaveoption.MORNING_FULL_LEAVE:
                    return 1.00;
                case leaveoption.EVENING_SHORT_LEAVE:
                    return 0.25;
                case leaveoption.EVENING_HALFDAY_LEAVE:
                    return 0.50;
                case leaveoption.EVENING_FULL_LEAVE:
                    return 1.00;
                default:
                    return 0;
            }
        }
    }
    public enum leaveoption
    {
        MORNING_SHORT_LEAVE,
        MORNING_HALFDAY_LEAVE,
        MORNING_FULL_LEAVE,
        EVENING_SHORT_LEAVE,
        EVENING_HALFDAY_LEAVE,
        EVENING_FULL_LEAVE
    }

    public enum leaveType
    {
        MORNING_SL_VALUE,
        MORNING_HD_VALUE,
        MORNING_FD_VALUE,
        EVENING_SL_VALUE,
        EVENING_HD_VALUE,
        EVENING_FD_VALUE
    }
}