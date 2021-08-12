using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports.Documents
{
    public static class RptGuidConfig
    {
        public static Guid RptDeductionId(Deduction view)
        {
            switch (view)
            {
                case Deduction.NoPayDeduction:
                    return new Guid("5625956d-dfe1-44f1-bb0d-68bf09722890");
                case Deduction.FestivalLoan:
                    return new Guid("84d87735-725d-421c-9dd6-9054607242a0");
                case Deduction.SpecialLoan:
                    return new Guid("4f34fc44-9276-4e83-bd7b-daec25c0844e");
                case Deduction.LateDeduction:
                    return new Guid("0a35a06d-3744-4e8b-ad8e-18657382387d");

                default:
                    return new Guid();
            }
        }
    }
    public enum Deduction
    {
        NoPayDeduction,
        FestivalLoan,
        SpecialLoan,
        LateDeduction,

    };
}
