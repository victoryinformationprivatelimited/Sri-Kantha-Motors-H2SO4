using ERPData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardService.Services
{
    public interface IDashboardServiceERP
    {
        #region Employee Section

        IEnumerable<EmployeeSumarryView> getEmployeeDetails();
        EmployeeSumarryView getEmployeeDetailsByemployee_id(string employee_id);
        IEnumerable<EmployeeSumarryView> getEmployeeList();
        IEnumerable<EmployeeSumarryView> getEmployeeListOrderByDept();
        IEnumerable<EmployeeSumarryView> getEmployeeListOrderByDes();
        IEnumerable<EmployeeSumarryView> getEmployeeListOrderByJoinDate();
        IEnumerable<EmployeeSumarryView> getEmployeeListOrderByDesignation();
        IEnumerable<EmployeeSumarryView> getEmployee(string empId);
        IEnumerable<z_Department> getDeparments();
        IEnumerable<z_Designation> getDesignations();
        IEnumerable<EmployeeSumarryView> getEmployeeListOrderByPermanentDate(DateTime startDate, DateTime endDate);
        IEnumerable<EmployeeSumarryView> getEmployeeListOrderByResignDate(DateTime startDate, DateTime endDate);

        #endregion
    }
}
