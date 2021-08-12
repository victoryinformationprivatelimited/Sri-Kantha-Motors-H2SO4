using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPData;
using AttendanceData;

namespace DashboardService.Services
{
    public class DashboardService : IDashboardServiceERP, IDashboardServiceAttendance, IDashboardServicePayroll
    {
        #region Employee Section

        private ERPEntities ERPData = new ERPEntities();

        public IEnumerable<ERPData.EmployeeSumarryView> getEmployeeDetails()
        {
            return ERPData.EmployeeSumarryViews;
        }

        public ERPData.EmployeeSumarryView getEmployeeDetailsByemployee_id(string employee_id)
        {
            return ERPData.EmployeeSumarryViews.Where(e => e.employee_id == new Guid(employee_id)).First();
        }

        public IEnumerable<EmployeeSumarryView> getEmployee(string empId)
        {
            return ERPData.EmployeeSumarryViews.Where(e => e.emp_id == empId);
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeList()
        {
            return ERPData.EmployeeSumarryViews;
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeListOrderByDept()
        {
            return ERPData.EmployeeSumarryViews.OrderBy(e => e.department_id);
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeListOrderByDes()
        {
            return ERPData.EmployeeSumarryViews.OrderBy(e => e.designation_id);
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeListOrderByJoinDate()
        {
            return ERPData.EmployeeSumarryViews.OrderBy(e => e.join_date);
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeListOrderByDesignation()
        {
            return ERPData.EmployeeSumarryViews.OrderBy(e => e.designation_id);
        }

        public IEnumerable<z_Department> getDeparments()
        {
            return ERPData.z_Department.OrderBy(e => e.department_id);
        }

        public IEnumerable<z_Designation> getDesignations()
        {
            return ERPData.z_Designation.OrderBy(e => e.designation_id);
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeListOrderByPermanentDate(DateTime startDate, DateTime endDate)
        {
            return ERPData.EmployeeSumarryViews.Where(e => e.prmernant_active_date >= startDate && e.prmernant_active_date <= endDate).OrderBy(e => e.prmernant_active_date);
        }

        public IEnumerable<EmployeeSumarryView> getEmployeeListOrderByResignDate(DateTime startDate, DateTime endDate)
        {
            return ERPData.EmployeeSumarryViews.Where(e => e.resign_date >= startDate && e.resign_date <= endDate).OrderBy(e => e.resign_date);
        }

        #endregion

        #region Attendance Section

        private AttendanceEntities AttendanceData = new AttendanceEntities();

        public IEnumerable<AttendanceData.z_Period> getPeriodList()
        {
            return AttendanceData.z_Period.OrderByDescending(e => e.start_date);
        }

        public IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceData()
        {
            return AttendanceData.trns_EmployeeAttendanceSumarry.OrderBy(e => e.employee_id);
        }

        public IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByNoPayCount()
        {
            return AttendanceData.trns_EmployeeAttendanceSumarry.OrderByDescending(e => e.morning_halfday_nopay_count + e.evening_halfday_nopay_count + e.authorized_nopay_fulldays_count + e.nopay_fulldays_count);
        }

        public IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByAbasentCount()
        {
            return AttendanceData.trns_EmployeeAttendanceSumarry.OrderByDescending(e => e.absent_days);
        }

        public IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByTotalLate()
        {
            return AttendanceData.trns_EmployeeAttendanceSumarry;
        }

        public IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByInvalidAttendance()
        {
            return AttendanceData.trns_EmployeeAttendanceSumarry.OrderByDescending(e => e.invalid_days);
        }

        public IEnumerable<AttendanceData.trns_EmployeeAttendanceSumarry> getEmployeeAttendanceDataOrderByOT()
        {
            return AttendanceData.trns_EmployeeAttendanceSumarry;
        }

        #endregion

        #region Payroll Section

        #endregion
    }
}