using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HelperClass
{
    public static class clsLeaves
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


                case leaveoption.AnualLeaveDetail:
                    return new Guid("1d07f0ea-e5d5-4ed1-9dde-616cdb8d0142");
                case leaveoption.CasualLeaveDetail:
                    return new Guid("98b6f440-ed7d-4ea0-bf9b-61f4035d713c");
                case leaveoption.MedicalLeaveDetail:
                    return new Guid("89659172-bbfe-4792-b454-9a87fd642476");
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

        AnualLeaveDetail,
        CasualLeaveDetail,
        MedicalLeaveDetail
    };
    
}
