using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Loan_Module.Reports
{
    class EligibilityForGurantorCLS
    {
        public int MaxNoLoansInProgressRule { get; set; }
        public int MaxNoLoansInProgressActual { get; set; }
        public double DeductionPercentageOfPayrollRule { get; set; }
        public double DeductionPercentageOfPayrollActual { get; set; }
        public bool PermentemployeeRule { get; set; }
        public bool PermentemployeeActual { get; set; }
        public double MinServicePeriodRule { get; set; }
        public double MinServicePeriodActual { get; set; }
        public int MaxNoGuranteedLoansRule { get; set; }
        public int MaxNoGuranteedLoansActual { get; set; }
        public string DeductionPercentageOfPayrollEligibility { get; set; }
        public string MaxNoGuranteedLoansEligibility { get; set; }
        public string MaxNoLoansInProgressEligibility { get; set; }
        public string MinServicePeriodEligibility { get; set; }
        public string PermentemployeeEligibility { get; set; }
        public string GurantorFirstName { get; set; }
        public string GurantorLastName { get; set; }
        public string GurantorNIC { get; set; }
        public string GurantorEPF { get; set; }
        public string LoanName { get; set; }

    }
}
