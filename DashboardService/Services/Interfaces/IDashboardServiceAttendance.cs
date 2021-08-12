using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardService.Services
{
    public interface IDashboardServiceAttendance
    {
        #region Attendance Section

        IEnumerable<z_Period> getPeriodList();

        IEnumerable<trns_EmployeeAttendanceSumarry> getEmployeeAttendanceData();
        IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByNoPayCount();
        IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByAbasentCount();
        IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByTotalLate();
        IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByInvalidAttendance();
        IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByOT();

        #endregion
    }
}
