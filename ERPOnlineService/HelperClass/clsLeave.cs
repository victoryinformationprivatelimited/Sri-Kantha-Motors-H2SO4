using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPOnlineService.HelperClass
{
    public static class clsLeave
    {
        public static Guid GetLeaveOption(leaveoption option)
        {
            switch (option)
            {
                case leaveoption.MonthlyShortDays:
                    return new Guid("7329eb28-250e-4467-9bb4-8d42442c7154");
                case leaveoption.MonthlyHalfDays:
                    return new Guid("7a565537-54a5-4e82-a147-14e393844c0e");
                case leaveoption.MonthlyFullLeaveDays:
                    return new Guid("877a1ea2-af78-4544-8d95-5c37991e60c6");
                case leaveoption.MonthlyElectionLeave:
                    return new Guid("76797555-abc6-4b29-bb0d-365d396153a8");
                default:
                    return new Guid();
            }
        }
    }
    public enum leaveoption
    {
        MonthlyShortDays,
        MonthlyHalfDays,
        MonthlyFullLeaveDays,
        MonthlyElectionLeave
    };
}