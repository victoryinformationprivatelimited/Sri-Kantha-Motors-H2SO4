using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HelperClass
{
    public static class clsReportData
    {
         public static Guid GetReportID(reportname name)
        {
            switch (name)
            {
                //case reportname.AttendanceAllowance:
                //    return new Guid(ConfigurationManager.AppSettings["attendance_incentive"]);
                //case reportname.SingleOverTime:
                //    return new Guid(ConfigurationManager.AppSettings["over_time"]);
                //case reportname.LateDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["late_arriaval_deduction"]);
                //case reportname.SalaryAdvance:
                //    return new Guid(ConfigurationManager.AppSettings["advance_deduction"]);
                //case reportname.LoanDeductionReport:
                //    return new Guid(ConfigurationManager.AppSettings["laon_deduction_100"]);
                //case reportname.NoPayDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["nopay_deduction"]);
                //case reportname.HolidayOverTime:
                //    return new Guid(ConfigurationManager.AppSettings["holiday_ot"]);
                //case reportname.BudgetAllowance:
                //    return new Guid(ConfigurationManager.AppSettings["budjet_allowance"]);
                //case reportname.PerformanceIncentive:
                //    return new Guid(ConfigurationManager.AppSettings["performance_incentive"]);
                //case reportname.SpecialIncentive:
                //    return new Guid(ConfigurationManager.AppSettings["special_incentive"]);



                //case reportname.SpecialIncentiveDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["special_incentive_deduction"]);
                //case reportname.LateArrivalDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["late_arriaval_deduction"]);
                //case reportname.LeavePay:
                //    return new Guid(ConfigurationManager.AppSettings["leave_pay"]);
                //case reportname.NopayDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["nopay_deduction"]);
                //case reportname.TShirtDeduction275:
                //    return new Guid(ConfigurationManager.AppSettings["tshirt_deduction_250"]);
                //case reportname.AttendanceIncentiveDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["attendance_incentive_deduction"]);
                //case reportname.TShirtDeduction100:
                //    return new Guid(ConfigurationManager.AppSettings["tshirt_deduction_100"]);
                //case reportname.PerformanceIncentiveDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["performance_incentive_deduction"]);
                //case reportname.LoanDeduction100:
                //    return new Guid(ConfigurationManager.AppSettings["laon_deduction_100"]);
                //case reportname.LoanDeduction1000:
                //    return new Guid(ConfigurationManager.AppSettings["laon_deduction"]);
                //case reportname.WelfareDeduction:
                //    return new Guid(ConfigurationManager.AppSettings["welfare_deduction"]);
                //case reportname.HolidayWorking:
                //    return new Guid(ConfigurationManager.AppSettings["holiday_working"]);
                //case reportname.SatadayWorking:
                //    return new Guid(ConfigurationManager.AppSettings["sataday_working"]);

                //case reportname.TotalEpf:
                //    return new Guid(ConfigurationManager.AppSettings["epf_8"]);
                //case reportname.Epf8:
                //    return new Guid(ConfigurationManager.AppSettings["epf_8"]);
                //case reportname.Epf12:
                //    return new Guid(ConfigurationManager.AppSettings["epf_12"]);
                //case reportname.Etf:
                //    return new Guid(ConfigurationManager.AppSettings["etf"]);
                //case reportname.Payee:
                //    return new Guid(ConfigurationManager.AppSettings["payee"]);
                //case reportname.StampDuty:
                //    return new Guid(ConfigurationManager.AppSettings["stamp_deauty"]);

                default:
                    return new Guid();
            }          
        }
    }

         public enum reportname
         {
             //AttendanceAllowance,
             //SingleOverTime,
             //LateDeduction,
             //SalaryAdvance,
             //LoanDeductionReport,
             //NoPayDeduction,
             //HolidayOverTime,
             //BudgetAllowance,
             //PerformanceIncentive,
             //SpecialIncentive,


             //SpecialIncentiveDeduction,
             //LateArrivalDeduction,
             //LeavePay,
             //NopayDeduction,
             //TShirtDeduction275,
             //AttendanceIncentiveDeduction,
             //TShirtDeduction100,
             //PerformanceIncentiveDeduction,
             //LoanDeduction100,
             //LoanDeduction1000,
             //WelfareDeduction,
             //HolidayWorking,
             //SatadayWorking,



             //TotalEpf,
             //Epf8,
             //Epf12,
             //Etf,
             //Payee,
             //StampDuty
            

         };
    
}
