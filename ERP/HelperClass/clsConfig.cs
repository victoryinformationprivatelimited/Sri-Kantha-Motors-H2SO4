using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;

namespace ERP
{
    public static class clsConfig
    {
        public static bool isAdvanceSecurityOn = false;
        public static mas_Employee CurrentEmployee = null;
        public static string DateTimeFormat;

        public static Guid GetViewModelId(Viewmodels view)
        {
            switch (view)
            {
                case Viewmodels.Section:
                    return new Guid("00000000-0000-0000-0000-000000000001");
                case Viewmodels.City:
                    return new Guid("00000000-0000-0000-0000-000000000002");
                case Viewmodels.Town:
                    return new Guid("00000000-0000-0000-0000-000000000003");
                case Viewmodels.Company:
                    return new Guid("00000000-0000-0000-0000-000000000004");
                case Viewmodels.Department:
                    return new Guid("00000000-0000-0000-0000-000000000005");
                case Viewmodels.Bank:
                    return new Guid("00000000-0000-0000-0000-000000000006");
                case Viewmodels.Grade:
                    return new Guid("00000000-0000-0000-0000-000000000007");
                case Viewmodels.Module:
                    return new Guid("00000000-0000-0000-0000-000000000008");
                case Viewmodels.Payment:
                    return new Guid("00000000-0000-0000-0000-000000000009");
                case Viewmodels.Period:
                    return new Guid("00000000-0000-0000-0000-000000000010");
                case Viewmodels.CompanyVariables:
                    return new Guid("00000000-0000-0000-0000-000000000011");
                case Viewmodels.CompanyBranches:
                    return new Guid("00000000-0000-0000-0000-000000000012");
                case Viewmodels.BankBranch:
                    return new Guid("00000000-0000-0000-0000-000000000013");
                case Viewmodels.Designation:
                    return new Guid("00000000-0000-0000-0000-000000000022");
                case Viewmodels.Employee:
                    return new Guid("00000000-0000-0000-0000-000000000023");
                case Viewmodels.PeriodQuntity:
                    return new Guid("00000000-0000-0000-0000-000000000025");
                case Viewmodels.EmployeeBankDetail:
                    return new Guid("00000000-0000-0000-0000-000000000024");

                case Viewmodels.PayrollPayments:
                    return new Guid("00000000-0000-0000-0000-000000000014");
                case Viewmodels.PayProcess:
                    return new Guid("00000000-0000-0000-0000-000000000015");
                case Viewmodels.PayPeriod:
                    return new Guid("00000000-0000-0000-0000-000000000016");
                case Viewmodels.EmployeeRule:
                    return new Guid("00000000-0000-0000-0000-000000000017");
                case Viewmodels.CompanyRule:
                    return new Guid("00000000-0000-0000-0000-000000000018");
                case Viewmodels.EmployeeCompanyVariable:
                    return new Guid("00000000-0000-0000-0000-000000000019");
                case Viewmodels.Deductions:
                    return new Guid("00000000-0000-0000-0000-000000000020");
                case Viewmodels.Benifits :
                    return new Guid("00000000-0000-0000-0000-000000000021");
                case Viewmodels.EmployeeBenifit:
                    return new Guid("00000000-0000-0000-0000-000000000021");

                case Viewmodels.UserMaster:
                    return new Guid("00000000-0000-0000-0000-000000000100");
                case Viewmodels.UserLevelMaster:
                    return new Guid("00000000-0000-0000-0000-000000000101");
                case Viewmodels.UserPermissionMaster:
                    return new Guid("00000000-0000-0000-0000-000000000102");

                case Viewmodels.ShiftCatagory:
                    return new Guid("00000000-0000-0000-0000-000000000030");
                case Viewmodels.ShiftDetail:
                    return new Guid("00000000-0000-0000-0000-000000000031");
                case Viewmodels.EmployeeShift:
                    return new Guid("00000000-0000-0000-0000-000000000032");
                case Viewmodels.ManualAttendance:
                    return new Guid("00000000-0000-0000-0000-000000000033");
                case Viewmodels.AttendanceDivice:
                    return new Guid("00000000-0000-0000-0000-000000000034");
                case Viewmodels.DataDownload:
                    return new Guid("00000000-0000-0000-0000-000000000035");
                case Viewmodels.DailyAttendanceProcess:
                    return new Guid("00000000-0000-0000-0000-000000000036");
                case Viewmodels.AttendanceProcess:
                    return new Guid("00000000-0000-0000-0000-000000000037");
                case Viewmodels.AttendanceManualUpload:
                    return new Guid("00000000-0000-0000-0000-000000000038");
                case Viewmodels.AttendanceTextFileUpload:
                    return new Guid("00000000-0000-0000-0000-000000000039");
                case Viewmodels.AttendanceDataMigration:
                    return new Guid("00000000-0000-0000-0000-000000000040");
                case Viewmodels.RosterGroup:
                    return new Guid("00000000-0000-0000-0000-000000000041");
                case Viewmodels.RosterDetail:
                    return new Guid("00000000-0000-0000-0000-000000000042");
                case Viewmodels.EmployeeRoster:
                    return new Guid("00000000-0000-0000-0000-000000000043");
                case Viewmodels.ExtraOverTime:
                    return new Guid("00000000-0000-0000-0000-000000000044");


                case Viewmodels.NormalAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000150");
                case Viewmodels.DetailAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000151");
                case Viewmodels.GraceOutAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000152");
                case Viewmodels.GraceInAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000153");
                case Viewmodels.EarlyOutAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000154");
                case Viewmodels.EarlyInAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000155");
                case Viewmodels.LateDeductionReport:
                    return new Guid("00000000-0000-0000-0000-000000000156");
                case Viewmodels.DailyAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000157");
                case Viewmodels.DailyAbsentisumReport:
                    return new Guid("00000000-0000-0000-0000-000000000158");
                case Viewmodels.TodayAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000159");
                case Viewmodels.TodayAbsentismReport:
                    return new Guid("00000000-0000-0000-0000-000000000160");
                case Viewmodels.OverTimeSummaryReport:
                    return new Guid("00000000-0000-0000-0000-000000000161");
                case Viewmodels.DailyOverTimeReport:
                    return new Guid("00000000-0000-0000-0000-000000000162");
                case Viewmodels.InvalidAttendanceReport:
                    return new Guid("00000000-0000-0000-0000-000000000163");
                case Viewmodels.AttendanceSummarryReport:
                    return new Guid("00000000-0000-0000-0000-000000000164");


                case Viewmodels.DailyLeaveReport:
                    return new Guid("00000000-0000-0000-0000-000000000175");
                case Viewmodels.DailyApprovedReport:
                    return new Guid("00000000-0000-0000-0000-000000000176");
                case Viewmodels.DailyRejectedReport:
                    return new Guid("00000000-0000-0000-0000-000000000177");
                case Viewmodels.DailyPendingReport:
                    return new Guid("00000000-0000-0000-0000-000000000178");
                case Viewmodels.DailyEntitlementReport:
                    return new Guid("00000000-0000-0000-0000-000000000179");


                case Viewmodels.EmployeeSumarryReport:
                    return new Guid("00000000-0000-0000-0000-000000000200");
                case Viewmodels.EmployeeContactDetailReport:
                    return new Guid("00000000-0000-0000-0000-000000000201");
                case Viewmodels.EmployeeBasicSalaryReport:
                    return new Guid("00000000-0000-0000-0000-000000000202");
                case Viewmodels.EmployeeBirthdayReport:
                    return new Guid("00000000-0000-0000-0000-000000000203");
                case Viewmodels.EmployeeDetailReport:
                    return new Guid("00000000-0000-0000-0000-000000000204");
                case Viewmodels.EmployeePermenentActiveReport:
                    return new Guid("00000000-0000-0000-0000-000000000205");
                case Viewmodels.EmployeeBankAccountDetail:
                    return new Guid("00000000-0000-0000-0000-000000000206");



                case Viewmodels.PaySheet:
                    return new Guid("00000000-0000-0000-0000-000000000250");
                case Viewmodels.PayrollSumarry:
                    return new Guid("00000000-0000-0000-0000-000000000251");
                case Viewmodels.EmployeeFundReport:
                    return new Guid("00000000-0000-0000-0000-000000000252");
                case Viewmodels.BenifitReport:
                    return new Guid("00000000-0000-0000-0000-000000000253");
                case Viewmodels.DeductionReport:
                    return new Guid("00000000-0000-0000-0000-000000000254");
                case Viewmodels.SalaryReport:
                    return new Guid("00000000-0000-0000-0000-000000000255");
                case Viewmodels.EPFContributionReport:
                    return new Guid("00000000-0000-0000-0000-000000000256");
                case Viewmodels.MonthlyPaySumarry:
                    return new Guid("00000000-0000-0000-0000-000000000257");
                case Viewmodels.BasicTransferReport:
                    return new Guid("00000000-0000-0000-0000-000000000258");
                case Viewmodels.PaySheetSumarry:
                    return new Guid("00000000-0000-0000-0000-000000000259");
                case Viewmodels.TotalPayrollSumarry:
                    return new Guid("00000000-0000-0000-0000-000000000260");
                case Viewmodels.HorizontalPayrollSumarry:
                    return new Guid("00000000-0000-0000-0000-000000000261");
                case Viewmodels.SalaryReconcilationReport:
                    return new Guid("00000000-0000-0000-0000-000000000262");


                case Viewmodels.AppointmentLetter:
                    return new Guid("00000000-0000-0000-0000-000000000300");
                case Viewmodels.ConfirmationLetter:
                    return new Guid("00000000-0000-0000-0000-000000000301");
                case Viewmodels.SalaryConfirmationLetter:
                    return new Guid("00000000-0000-0000-0000-000000000302");
                case Viewmodels.WarningLetter:
                    return new Guid("00000000-0000-0000-0000-000000000303");
                case Viewmodels.NopayLetter:
                    return new Guid("00000000-0000-0000-0000-000000000304");
                case Viewmodels.AddUniform:
                    return new Guid("00000000-0000-0000-0000-000000000305");
                case Viewmodels.UniformOrder:
                    return new Guid("00000000-0000-0000-0000-000000000306");
                case Viewmodels.UniformOffering:
                    return new Guid("00000000-0000-0000-0000-000000000307");
                    
                case Viewmodels.GemLot :
                    return new Guid("00000000-0000-0000-0000-000000000350");
                case Viewmodels.GemGroup:
                    return new Guid("00000000-0000-0000-0000-000000000351");
                case Viewmodels.AddPreformTask:
                    return new Guid("00000000-0000-0000-0000-000000000352");
                case Viewmodels.PreformTaskReturn:
                    return new Guid("00000000-0000-0000-0000-000000000353");
                case Viewmodels.AddGem:
                    return new Guid("00000000-0000-0000-0000-000000000354");
                case Viewmodels.AddCalibrateTask:
                    return new Guid("00000000-0000-0000-0000-000000000355");
                case Viewmodels.CalibrateTaskReturn:
                    return new Guid("00000000-0000-0000-0000-000000000356");
                case Viewmodels.AddDesignTask:
                    return new Guid("00000000-0000-0000-0000-000000000357");
                case Viewmodels.DesignTaskReturn:
                    return new Guid("00000000-0000-0000-0000-000000000358");
                case Viewmodels.MedicalReimbursementPeriod:
                    return new Guid("00000000-0000-0000-0000-000000000359");
                case Viewmodels.MedicalReimbursementCategories:
                    return new Guid("00000000-0000-0000-0000-000000000360");
                case Viewmodels.MedicalReimbursement:
                    return new Guid("00000000-0000-0000-0000-000000000361");

                default:
                    return new Guid();
            }          
        }
    }

   enum CompanyVariable
   {
       ETFDeduct = 00000000 - 0000 - 0000 - 0000 - 000000000001,
       ETFCompanyNotdeduct = 00000000 - 0000 - 0000 - 0000 - 000000000002,
       EPF = 00000000 - 0000 - 0000 - 0000 - 000000000003
   };

   public enum Viewmodels
   {
       Section,
       City,
       Town,
       Company,
       Department,
       Bank,
       Grade,
       Module,
       Payment,
       Period,
       CompanyVariables,
       CompanyBranches,
       BankBranch,
       Designation,
       Employee,
       EmployeeBankDetail,

       EmployeeBenifit,
       PayrollPayments,
       PayProcess,
       PayPeriod,
       EmployeeRule,
       CompanyRule,
       EmployeeCompanyVariable,
       Deductions,
       Benifits,
       PeriodQuntity,

       UserMaster,
       UserLevelMaster,
       UserPermissionMaster,

       ShiftCatagory,
       ShiftDetail,
       EmployeeShift,
       ManualAttendance,
       AttendanceDivice,
       DataDownload,
       DailyAttendanceProcess,
       AttendanceProcess,
       AttendanceManualUpload,
       AttendanceTextFileUpload,
       AttendanceDataMigration,
       RosterGroup,
       RosterDetail,
       EmployeeRoster,
       ExtraOverTime,

       NormalAttendanceReport,
       DetailAttendanceReport,
       GraceOutAttendanceReport,
       GraceInAttendanceReport,
       EarlyOutAttendanceReport,
       EarlyInAttendanceReport,
       LateDeductionReport,
       DailyAttendanceReport,
       DailyAbsentisumReport,
       TodayAttendanceReport,
       TodayAbsentismReport,
       OverTimeSummaryReport,
       DailyOverTimeReport,
       InvalidAttendanceReport,
       AttendanceSummarryReport,

       DailyLeaveReport,
       DailyApprovedReport,
       DailyRejectedReport,
       DailyPendingReport,
       DailyEntitlementReport,

       EmployeeSumarryReport,
       EmployeeContactDetailReport,
       EmployeeBasicSalaryReport,
       EmployeeBirthdayReport,
       EmployeeDetailReport,
       EmployeePermenentActiveReport,
       EmployeeBankAccountDetail,

       PaySheet,
       PayrollSumarry,
       EmployeeFundReport,
       BenifitReport,
       DeductionReport,
       SalaryReport,
       EPFContributionReport,
       MonthlyPaySumarry,
       BasicTransferReport,
       PaySheetSumarry,
       TotalPayrollSumarry,
       HorizontalPayrollSumarry,
       SalaryReconcilationReport,

       AppointmentLetter,
       ConfirmationLetter,
       SalaryConfirmationLetter,
       WarningLetter,
       NopayLetter,
       AddUniform,
       UniformOrder,
       UniformOffering,

       GemLot,
       GemGroup,
       AddPreformTask,
       PreformTaskReturn,
       AddGem,
       AddCalibrateTask,
       CalibrateTaskReturn,
       AddDesignTask,
       DesignTaskReturn,

       MedicalReimbursementPeriod,
       MedicalReimbursementCategories,
       MedicalReimbursement
   };

}
