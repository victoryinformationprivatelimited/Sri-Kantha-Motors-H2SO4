using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HelperClass
{
    public static class clsDays
    {
        public static Guid GetDay(dayname name)
        {
            switch (name)
            {
                case dayname.Monday:
                    return new Guid("e2d0ed44-3504-4c73-b4cc-173fe487a4fd");
                case dayname.Tuesday:
                    return new Guid("14349bba-7e11-4098-8ea5-7e4b1de7e149");
                case dayname.Wednesday:
                    return new Guid("8e507219-6183-42ec-9435-fb5e43a4215e");
                case dayname.Thursday:
                    return new Guid("ac09802b-c320-47f2-9128-eab177c983af");
                case dayname.Friday:
                    return new Guid("89296ad3-566e-4766-9d64-c4b41b81a23d");
                case dayname.Saturday:
                    return new Guid("fcff0bd8-72a1-4fdb-b31a-53825896ffb9");
                case dayname.Sunday:
                    return new Guid("9d27cbfe-4a8c-47bc-978e-8ab5476e9b67");
                default:
                    return new Guid();
            }
        }
    }

    public enum dayname
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday

    };
}
