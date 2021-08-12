using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HelperClass
{
    public static class clsAttendanceMode
    {
        public static Guid GetMode(Mode Modename)
        {
            switch (Modename)
            {
                case Mode.CheckIn:
                    return new Guid("5b60873a-5270-47db-9334-0b876e371d1c");
                case Mode.CheckOut:
                    return new Guid("5b60873a-5270-47db-9334-0b876e481d1c");
                case Mode.LunchIn:
                    return new Guid("5b60873a-5270-47db-9334-0b876e591d1c");
                case Mode.LunchOut:
                    return new Guid("ac09802b-c320-47f2-9128-eab177c983af");
                default:
                    return new Guid();
            }
        }
    }

    public enum Mode
    {
        CheckIn,
        CheckOut,
        LunchIn,
        LunchOut
      

    };
}
