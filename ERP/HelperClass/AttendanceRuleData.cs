using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HelperClass
{
    public static class AttendanceRuleData
    {
        public static Guid GetAttendanceRule(AttendanceRuleName name)
        {
            switch (name)
            {

                case AttendanceRuleName.NormalOverTimeShopAndOffice:
                    return new Guid("00000000-0000-0000-0000-000000000001");
                case AttendanceRuleName.NormalOverTimeWagesBoards:
                    return new Guid("00000000-0000-0000-0000-000000000004");
                case AttendanceRuleName.DoubleOverTimeShopAndOffice:
                    return new Guid("00000000-0000-0000-0000-000000000002");
                case AttendanceRuleName.DoubleOverTimeWagesBoards:
                    return new Guid("00000000-0000-0000-0000-000000000005");
                case AttendanceRuleName.TripleOTShopAndOffice:
                    return new Guid("00000000-0000-0000-0000-000000000003");
                case AttendanceRuleName.TripleOTWagesBoards:
                    return new Guid("00000000-0000-0000-0000-000000000006");
                case AttendanceRuleName.NoPayShopAndOffice:
                    return new Guid("10000000-0000-0000-0000-000000000001");
                case AttendanceRuleName.NoPayWagesBoards:
                    return new Guid("10000000-0000-0000-0000-000000000002");
                case AttendanceRuleName.LateWagesBoards:
                    return new Guid("20000000-0000-0000-0000-000000000002");
                case AttendanceRuleName.LateShopAndOffice:
                    return new Guid("20000000-0000-0000-0000-000000000001");

                case AttendanceRuleName.extaraDayShopAndOffice:
                    return new Guid("00000000-0000-0000-0000-000000000100");
                case AttendanceRuleName.extarDayWagesBord:
                    return new Guid("00000000-0000-0000-0000-000000000101");

                case AttendanceRuleName.ShortLiveHalfdaywages:
                    return new Guid("22000000-0000-0000-0000-000000000002");
                case AttendanceRuleName.ShortLiveHalfdayShopandOffice:
                    return new Guid("22000000-0000-0000-0000-000000000001");


                case AttendanceRuleName.LeaveEntitlement0point5absentAttendanceBonusDeduction:
                    return new Guid("00000000-0000-0000-0000-000000000003");
                case AttendanceRuleName.LeaveEntitlement01absentAttendanceBonusDeduction:
                    return new Guid("00000000-0000-0000-0000-000000000003");
                case AttendanceRuleName.LeaveEntitlement1point5absentAttendanceBonusDeduction:
                    return new Guid("00000000-0000-0000-0000-000000000003");
                case AttendanceRuleName.LeaveEntitlement02absentAttendanceBonusDeduction:
                    return new Guid("00000000-0000-0000-0000-000000000003");




                case AttendanceRuleName.ExtraDays:
                    return new Guid("00000000-0000-0000-0000-000000000003");
                case AttendanceRuleName.poyaOT:
                    return new Guid("10000000-0000-0000-0000-000000000002");
                case AttendanceRuleName.AttendanceBonus:
                    return new Guid("14d0f98c-117a-4001-bcfa-4ab356904837");
                case AttendanceRuleName.ProformanceBonus:
                    return new Guid("cdf0fe4d-d18c-4e93-837b-37009d104db2");

                case AttendanceRuleName.Welfare:
                    return new Guid("d3f86eb5-1eb3-4099-ad55-4ff2ad293d4f");


               



                case AttendanceRuleName.staffLoan:
                    return new Guid("33fd870c-fe5f-4a22-9781-19afab6e7d3e");
                case AttendanceRuleName.BuyerIncentive:
                    return new Guid("e1c45a9b-92fd-420f-b688-1aa46f1e8543");
                case AttendanceRuleName.Meals:
                    return new Guid("d65784be-1187-4355-95e7-505a98d6fdb9");
                case AttendanceRuleName.StaffUniform:
                    return new Guid("b5a661e5-aba5-4d4d-bad0-5248f99c3950");
                case AttendanceRuleName.OtherOverTimeIncentive:
                    return new Guid("0f933fdb-4bce-413f-be01-7957c19638b0");
                case AttendanceRuleName.Medical:
                    return new Guid("4520ec35-ad4c-4361-9321-8f0dd9b97e7b");
                case AttendanceRuleName.BoardingWater:
                    return new Guid("5baeb519-c37e-4fd4-9230-9f02ce01aa30");
                case AttendanceRuleName.OtherDeduction:
                    return new Guid("5e84ff3d-e1e0-4860-9554-a667413e2d67");
                case AttendanceRuleName.BoardingFee:
                    return new Guid("7276e1d5-f988-489e-840a-be45c7e45380");
                case AttendanceRuleName.BondFee:
                    return new Guid("e68ef92d-abe1-4f8f-b341-d993c3a9fe12");
                case AttendanceRuleName.SalaryAdvance:
                    return new Guid("c0f0a282-309e-4570-8d5b-f2c1f1561dd5");

                case AttendanceRuleName.SundayOt:
                    return new Guid("a242a7bb-5f82-45ab-bd2e-35d8dcc06684");
                case AttendanceRuleName.VehicleAllowance:
                    return new Guid("0e978fab-013c-4819-8f56-0c4e53c472b8");
                case AttendanceRuleName.CanteenBil:
                    return new Guid("351460a7-e75b-40f7-9328-181531614c62");
                case AttendanceRuleName.StaffAllowance:
                    return new Guid("fc16cd1c-9f14-4949-8c67-22aa64ea6de5");
                case AttendanceRuleName.Incentive:
                    return new Guid("c4b8fdeb-4772-4a3a-8b5b-24e3375879ac");
                case AttendanceRuleName.GradingAllowance:
                    return new Guid("869529bc-2795-4854-8a2e-33539f1724a6");
                case AttendanceRuleName.Funeralfund:
                    return new Guid("ed215e6c-cd28-4686-a285-bdb4b419f7de");
                case AttendanceRuleName.specialAllowance:
                    return new Guid("4a51766b-25e7-4645-ab9a-cdc5df18bb3e");

                default:
                    return new Guid();
            }
        }
    }

    public enum AttendanceRuleName
    {
        
        NormalOverTimeShopAndOffice,
        NormalOverTimeWagesBoards,
        DoubleOverTimeShopAndOffice,
        DoubleOverTimeWagesBoards,
        TripleOTShopAndOffice,
        TripleOTWagesBoards,
        NoPayShopAndOffice,
        NoPayWagesBoards,
        LateWagesBoards,
        LateShopAndOffice,
        ShortLiveHalfdayShopandOffice,
        ShortLiveHalfdaywages,


        ExtraDays,
        poyaOT,
        LeaveEntitlement0point5absentAttendanceBonusDeduction,
        LeaveEntitlement01absentAttendanceBonusDeduction,
        LeaveEntitlement1point5absentAttendanceBonusDeduction,
        LeaveEntitlement02absentAttendanceBonusDeduction,
        //LeaveNotEntitlement0point5absentAttendanceBonusDeduction,
        //LeaveNotEntitlement01absentAttendanceBonusDeduction,
        //LeaveNotEntitlement1point5absentAttendanceBonusDeduction,
        //LeaveNotEntitlement02absentAttendanceBonusDeduction,

        AttendanceBonus,
    
        ProformanceBonus,

        NopayWages,
        NopayShopAndOffice,
        //LateDeduction,
        Welfare,
      
        attendanceBonusDeduction1,
        attendanceBonusDeduction2,
        attendanceBonusDeduction3,
        attendanceBonusDeduction4,

        staffLoan,
        BuyerIncentive,
        Meals,
        StaffUniform,
        OtherOverTimeIncentive,
        Medical,
        BoardingWater,
        OtherDeduction,
        BoardingFee,
        BondFee,
        SalaryAdvance,
        SundayOt,
        VehicleAllowance,
        CanteenBil,
        StaffAllowance,
        Incentive,
        GradingAllowance,
        Funeralfund,
        specialAllowance,

        extarDayWagesBord,
        extaraDayShopAndOffice


    };
}
    
