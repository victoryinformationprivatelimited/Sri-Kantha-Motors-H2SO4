using AttendanceOnlineService.HelperInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses.Loggers
{
    public class TextLogger:IErrorLogger
    {
        void IErrorLogger.WriteLog()
        {
            throw new NotImplementedException();
        }
    }
}