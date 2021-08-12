using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class AssignedGroupsViewModel:ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendClient;

        #endregion

        #region Constructor

        public AssignedGroupsViewModel()
        {
            attendClient = new AttendanceServiceClient();

        }
        
        #endregion

        #region Properties

        
        #endregion

        #region Refresh Methods



        #endregion

        #region Button Methods
        
        #endregion

    }
}
