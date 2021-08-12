using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Loan_Module.Reports
{
    class LoanInstallment
    {
        public int installmentNo { get; set; }
        public decimal loanAmount { get; set; }
        public decimal installment { get; set; }
        public decimal interest { get; set; }
        public string EmployeeName { get; set; }
        public string Epf { get; set; }


    }
}
