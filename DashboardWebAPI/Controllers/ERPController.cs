using DashboardService.Services;
using ERP.Dashboard.API;
using ERPData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToJSON;
using static ERP.Dashboard.API.APIData;

namespace DashboardWebAPI.Controllers
{

    [RoutePrefix("Api/ERP")]
    public class ERPController : ApiController
    {

        #region Dashboard Service
        private IDashboardServiceERP dashboardService = new DashboardService.Services.DashboardService();
        #endregion

        #region Properties

        #region Lists

        IEnumerable<EmployeeSumarryView> EmployeeList;
        List<EmployeeSumarryView> SortedBirthday;
        List<EmployeeSumarryView> newJoinees;

        List<string> YearList;
        List<APIData.YearWiseEmploees> yearWiseEmployees;

        IEnumerable<z_Department> DepartmentList;
        List<APIData.EmployeesByDepartment> EmployeesByDepartmentObjList;

        IEnumerable<z_Designation> DesignationList;
        List<APIData.EmployeesByDesignation> EmployeesByDesignationObjList;

        List<YearWiseEmploees> permanentYearWiseEmployees;
        List<YearWiseEmploees> resignYearWiseEmployees;
        List<YearWiseEmploees> companyGrowthYearWiseEmployees;

        #endregion

        #endregion

        #region Root Route

        [Route("")]
        public IHttpActionResult Get() {
            var result = "Welcome to H2SO4 Dashboard API : ERP Section";
            if (result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        #endregion

        #region Employee Section Done

        #region common method to get all data

        [HttpGet]
        [Route("common/{startDate}/{endDate}")]
        public IHttpActionResult common(string startDate = null, string endDate = null)
        {
            try
            {
                DateTime[] DatesArr = APIData.SplitDateTimeinputs(startDate, endDate);

                EmployeeList = dashboardService.getEmployeeList();
                DepartmentList = dashboardService.getDeparments();
                DesignationList = dashboardService.getDesignations();

                getPermanentEmployeeYearList();
                getCompanyGrowthYearList(null);
                getResignEmployeeYearList();

                SortedBirthday = new List<EmployeeSumarryView>();
                EmployeesByDepartmentObjList = new List<APIData.EmployeesByDepartment>();
                EmployeesByDesignationObjList = new List<APIData.EmployeesByDesignation>();

                List<ERPObjectList> objColl = new List<ERPObjectList>();

                foreach (var x in EmployeeList)
                {
                    if (x.birthday != null && x != null)
                    {
                        if (x.birthday.Value.Month == Convert.ToDateTime(DatesArr[0]).Month || x.birthday.Value.Month == Convert.ToDateTime(DatesArr[1]).Month)
                        {
                            if (x.birthday.Value.Month == Convert.ToDateTime(DatesArr[0]).Month)
                            {
                                if (x.birthday.Value.Day >= Convert.ToDateTime(DatesArr[0]).Day)
                                {
                                    SortedBirthday.Add(x);
                                }
                            }
                            else if (x.birthday.Value.Month == Convert.ToDateTime(DatesArr[1]).Month)
                            {
                                if (x.birthday.Value.Day <= Convert.ToDateTime(DatesArr[1]).Day)
                                {
                                    SortedBirthday.Add(x);
                                }
                            }
                        }
                    }

                    if (x.prmernant_active_date != null)
                    {
                        foreach (var y in permanentYearWiseEmployees)
                        {
                            if (x.prmernant_active_date.Value.Year.ToString() == y.year)
                            {
                                y.AddEmp(x);
                                y.increase();
                            }
                        }
                    }

                    foreach (var y in permanentYearWiseEmployees)
                    {
                        y.countAndStoreMonthWisePermanentDateWise();
                    }


                    if (x.resign_date != null)
                    {
                        foreach (var y in resignYearWiseEmployees)
                        {
                            if (x.resign_date.Value.Year.ToString() == y.year)
                            {
                                y.AddEmp(x);
                                y.increase();
                            }
                        }
                    }

                    foreach (var y in resignYearWiseEmployees)
                    {
                        y.countAndStoreMonthWiseResignDateWise();
                    }


                    if (x.join_date != null)
                    {
                        foreach (var y in companyGrowthYearWiseEmployees)
                        {
                            if (x.join_date.Value.Year.ToString() == y.year)
                            {
                                y.AddEmp(x);
                                y.increase();
                            }
                        }
                    }

                    foreach (var y in companyGrowthYearWiseEmployees)
                    {
                        y.countAndStoreMonthWiseJoinDateWise();
                    }


                }

                foreach (var x in DepartmentList)
                {
                    APIData.EmployeesByDepartment edpt = new APIData.EmployeesByDepartment(x.department_id.ToString(), x.department_name.ToString(), 0.ToString());
                    foreach (var y in EmployeeList)
                    {
                        if (y.department_id == x.department_id)
                        {
                            edpt.AddEmp(y);
                            edpt.increase();
                        }
                    }
                    EmployeesByDepartmentObjList.Add(edpt);
                }

                foreach (var x in DesignationList)
                {
                    APIData.EmployeesByDesignation edpt = new APIData.EmployeesByDesignation(x.designation_id.ToString(), x.designation.ToString(), 0.ToString());
                    foreach (var y in EmployeeList)
                    {
                        if (y.designation_id == x.designation_id)
                        {
                            edpt.AddEmp(y);
                            edpt.increase();
                        }
                    }
                    EmployeesByDesignationObjList.Add(edpt);
                }


                objColl.Add(new ERPObjectList("upcomingBirthdaysList", SortedBirthday));
                objColl.Add(new ERPObjectList("permanentEmpList", permanentYearWiseEmployees));
                objColl.Add(new ERPObjectList("resignList", resignYearWiseEmployees));
                objColl.Add(new ERPObjectList("companyGrowthList", companyGrowthYearWiseEmployees));
                objColl.Add(new ERPObjectList("employeesByDepartment", EmployeesByDepartmentObjList));
                objColl.Add(new ERPObjectList("employeesByDesignation", EmployeesByDesignationObjList));

                return Ok(objColl);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Employee Birthdays Done

            #region default

        [HttpGet]
        [Route("birthdays")]
        public IHttpActionResult UpcomingBirthdays()
        {
            return Ok("Please specify value 'byMonth', 'byWeek' or 'default' for a result");
        }

        #endregion

        #region Function

        [HttpGet]
        [Route("birthdays/{startDate}/{endDate}")]
        public IHttpActionResult UpcomingBirthdays(string startDate = null, string endDate = null) {
            try
            {
                DateTime[] DatesArr = APIData.SplitDateTimeinputs(startDate, endDate);
                EmployeeList = dashboardService.getEmployeeList();
                SortedBirthday = new List<EmployeeSumarryView>();
                    foreach (var x in EmployeeList)
                    {
                        if (x.birthday != null && x != null)
                        {
                        if (x.birthday.Value.Month == Convert.ToDateTime(DatesArr[0]).Month || x.birthday.Value.Month == Convert.ToDateTime(DatesArr[1]).Month)
                        {
                            if (x.birthday.Value.Month == Convert.ToDateTime(DatesArr[0]).Month)
                            {
                                if (x.birthday.Value.Day >= Convert.ToDateTime(DatesArr[0]).Day)
                                {
                                    SortedBirthday.Add(x);
                                }
                            }else if (x.birthday.Value.Month == Convert.ToDateTime(DatesArr[1]).Month)
                            {
                                if (x.birthday.Value.Day <= Convert.ToDateTime(DatesArr[1]).Day)
                                {
                                    SortedBirthday.Add(x);
                                }
                            }
                        }
                        }
                    }
                return Ok(SortedBirthday);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Permanent List Done

        #region Methods

        private void getPermanentEmployeeYearList()
        {
            YearList = new List<string>();
            yearWiseEmployees = new List<APIData.YearWiseEmploees>();
            EmployeeList = dashboardService.getEmployeeList();
            try
            {
                foreach (var x in EmployeeList)
                {
                    if (x.prmernant_active_date != null)
                    {
                        if (YearList.Count == 0)
                        {
                            YearList.Add(x.join_date.Value.Year.ToString());
                            APIData.YearWiseEmploees year = new APIData.YearWiseEmploees(x.prmernant_active_date.Value.Year.ToString(), 0.ToString());
                            yearWiseEmployees.Add(year);
                        }
                        else
                        {
                            bool isExist = false;
                            foreach (var y in YearList)
                            {
                                if (y == x.prmernant_active_date.Value.Year.ToString())
                                {
                                    isExist = true;
                                }
                            }
                            if (!isExist)
                            {
                                YearList.Add(x.prmernant_active_date.Value.Year.ToString());
                                APIData.YearWiseEmploees year = new APIData.YearWiseEmploees(x.prmernant_active_date.Value.Year.ToString(), 0.ToString());
                                yearWiseEmployees.Add(year);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Function

        [HttpGet]
        [Route("permanentList")]
        public IHttpActionResult PermanentList()
        {
            try
            {
                getPermanentEmployeeYearList();

                foreach (var x in EmployeeList)
                {
                    if (x.prmernant_active_date != null)
                    {
                        foreach (var y in yearWiseEmployees)
                        {
                            if (x.prmernant_active_date.Value.Year.ToString() == y.year)
                            {
                                y.AddEmp(x);
                                y.increase();
                            }
                        }
                    }
                }
                foreach (var y in yearWiseEmployees)
                {
                    y.countAndStoreMonthWisePermanentDateWise();
                }
                return Ok(yearWiseEmployees);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Resign List Done

        #region Methods

        private void getResignEmployeeYearList()
        {

            YearList = new List<string>();
            EmployeeList = dashboardService.getEmployeeList();
            yearWiseEmployees = new List<APIData.YearWiseEmploees>();
            try
            {
                foreach (var x in EmployeeList)
                {
                    if (x.resign_date != null)
                    {
                        if (YearList.Count == 0)
                        {
                            YearList.Add(x.resign_date.Value.Year.ToString());
                            APIData.YearWiseEmploees year = new APIData.YearWiseEmploees(x.resign_date.Value.Year.ToString(), 0.ToString());
                            yearWiseEmployees.Add(year);
                        }
                        else
                        {
                            bool isExist = false;
                            foreach (var y in YearList)
                            {
                                if (y == x.resign_date.Value.Year.ToString())
                                {
                                    isExist = true;
                                }
                            }
                            if (!isExist)
                            {
                                YearList.Add(x.resign_date.Value.Year.ToString());
                                APIData.YearWiseEmploees year = new APIData.YearWiseEmploees(x.resign_date.Value.Year.ToString(), 0.ToString());
                                yearWiseEmployees.Add(year);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }
        #endregion

        #region Function

        [HttpGet]
        [Route("resignList")]
        public IHttpActionResult ResignList()
        {
            try
            {
                getResignEmployeeYearList();

                foreach (var x in EmployeeList)
                {
                    if (x.resign_date != null)
                    {
                        foreach (var y in yearWiseEmployees)
                        {
                            if (x.resign_date.Value.Year.ToString() == y.year)
                            {
                                y.AddEmp(x);
                                y.increase();
                            }
                        }
                    }
                }
                foreach (var y in yearWiseEmployees)
                {
                    y.countAndStoreMonthWiseResignDateWise();
                }
                return Ok(yearWiseEmployees);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Employee Count by Department Done

        #region Function

        [HttpGet]
        [Route("employeesByDept")]
        public IHttpActionResult EmployeesByDept()
        {
            try
            {
                EmployeeList = dashboardService.getEmployeeListOrderByDept();
                DepartmentList = dashboardService.getDeparments();
                EmployeesByDepartmentObjList = new List<APIData.EmployeesByDepartment>();
                foreach (var x in DepartmentList)
                {
                    APIData.EmployeesByDepartment edpt = new APIData.EmployeesByDepartment(x.department_id.ToString(), x.department_name.ToString(), 0.ToString());
                    foreach (var y in EmployeeList)
                    {
                        if (y.department_id == x.department_id)
                        {
                            edpt.AddEmp(y);
                            edpt.increase();
                        }
                    }
                    EmployeesByDepartmentObjList.Add(edpt);
                }
                return Ok(EmployeesByDepartmentObjList);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Employee Count By Designation

        #region Function

        [HttpGet]
        [Route("employeesByDesignation")]
        public IHttpActionResult EmployeesByDesignation()
        {
            try
            {
                EmployeeList = dashboardService.getEmployeeListOrderByDesignation();
                DesignationList = dashboardService.getDesignations();
                EmployeesByDesignationObjList = new List<APIData.EmployeesByDesignation>();
                foreach (var x in DesignationList)
                {
                    APIData.EmployeesByDesignation edpt = new APIData.EmployeesByDesignation(x.designation_id.ToString(), x.designation.ToString(), 0.ToString());
                    foreach (var y in EmployeeList)
                    {
                        if (y.designation_id == x.designation_id)
                        {
                            edpt.AddEmp(y);
                            edpt.increase();
                        }
                    }
                    EmployeesByDesignationObjList.Add(edpt);
                }
                return Ok(EmployeesByDesignationObjList);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Company growth Employee count Done

        #region Methods

        private void getCompanyGrowthYearList(string startDate)
        {
            YearList = new List<string>();
            EmployeeList = dashboardService.getEmployeeList();
            yearWiseEmployees = new List<APIData.YearWiseEmploees>();

            if (startDate != null)
            {

            }
            else
            {
                foreach (var x in EmployeeList)
                {
                    if (x.join_date != null)
                    {
                        if (YearList.Count == 0)
                        {
                            YearList.Add(x.join_date.Value.Year.ToString());
                            APIData.YearWiseEmploees year = new APIData.YearWiseEmploees(x.join_date.Value.Year.ToString(), 0.ToString());
                            yearWiseEmployees.Add(year);
                        }
                        else
                        {
                            bool isExist = false;
                            foreach (var y in YearList)
                            {
                                if (y == x.join_date.Value.Year.ToString())
                                {
                                    isExist = true;
                                }
                            }
                            if (!isExist)
                            {
                                YearList.Add(x.join_date.Value.Year.ToString());
                                APIData.YearWiseEmploees year = new APIData.YearWiseEmploees(x.join_date.Value.Year.ToString(), 0.ToString());
                                yearWiseEmployees.Add(year);
                            }
                        }
                    }
                }
            }


        }

        #endregion

        #region Function

        [HttpGet]
        [Route("employeesCountByYear")]
        public IHttpActionResult EmployeeCountByRange()
        {
            try
            {
                EmployeeList = dashboardService.getEmployeeListOrderByJoinDate();
                getCompanyGrowthYearList(null);

                foreach (var x in EmployeeList)
                {
                    if (x.join_date != null)
                    {
                        foreach (var y in yearWiseEmployees)
                        {
                            if (x.join_date.Value.Year.ToString() == y.year)
                            {
                                y.AddEmp(x);
                                y.increase();
                            }
                        }
                    }
                }

                foreach (var y in yearWiseEmployees)
                {
                    y.countAndStoreMonthWiseJoinDateWise();
                }

                return Ok(yearWiseEmployees);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #endregion

    }

}
