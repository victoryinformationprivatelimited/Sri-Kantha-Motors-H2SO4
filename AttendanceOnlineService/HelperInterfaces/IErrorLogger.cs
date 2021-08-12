using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperInterfaces
{
    public interface IErrorLogger
    {
        void WriteLog();
    }
}