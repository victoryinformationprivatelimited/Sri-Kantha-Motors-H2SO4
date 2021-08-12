using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Loan_Module.Reports
{
    class EligibilityForApplicantCLS
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
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicantNIC { get; set; }
        public string ApplicantEPF { get; set; }
        public string LoanName { get; set; }

    }
}
