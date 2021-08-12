using AttendanceData;
using DashboardService.Services;
using DashboardService.Tools;
using ERP.Dashboard.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static ERP.Dashboard.API.APIData;

namespace DashboardWebAPI.Controllers
{

    [RoutePrefix("Api/Attendance")]
    public class AttendanceController : ApiController
    {

        #region Dashboard Service
        private IDashboardServiceAttendance dashboardService = new DashboardService.Services.DashboardService();
        private IDashboardServiceERP dashboardServiceERP = new DashboardService.Services.DashboardService();
        #endregion

        #region Properties

        #region Lists

        IEnumerable<ERPData.z_Department> departmentList;
        IEnumerable<trns_EmployeeAttendanceSumarry> employeeAttendanceData;
        IEnumerable<z_Period> periodList;

        APIData.EmployeesByPeriod employeeNoPayData;
        APIData.EmployeesByPeriod employeeAbsentData;
        APIData.EmployeesByPeriod employeeLateData;
        APIData.EmployeesByPeriod employeeInvalidAttendanceData;
        APIData.EmployeesByPeriod employeeOTData;

        List<EmployeesByPeriod> employeeNoPayDataList;
        List<EmployeesByPeriod> employeeAbsentDataList;
        List<EmployeesByPeriod> employeeLateDataList;
        List<EmployeesByPeriod> employeeInvalidAttendanceDataList;
        List<EmployeesByPeriod> employeeOTDataList;

        #endregion

        #endregion

        #region Root Route

        [Route("")]
        public IHttpActionResult Get()
        {
            var result = "Welcome to H2SO4 Dashboard API : Attendance Section";
            if (result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        #endregion

        #region Attendance Section

        #region Common Methods

        private void loadEmployeeAttendanceData()
        {
            employeeAttendanceData = dashboardService.getEmployeeAttendanceData();
        }

        private void loadEmployeeAttendanceDataOrderByNoPay()
        {
            employeeAttendanceData = dashboardService.getEmployeeAttendanceDataOrderByNoPayCount();
        }

        private void loadEmployeeAttendanceDataOrderByAbsent()
        {
            employeeAttendanceData = dashboardService.getEmployeeAttendanceDataOrderByAbasentCount();
        }

        private void loadEmployeeAttendanceDataOrderByTotalLate()
        {
            employeeAttendanceData = dashboardService.getEmployeeAttendanceDataOrderByTotalLate();
        }

        private void loadEmployeeAttendanceDataOrderByInvalidAttendance()
        {
            employeeAttendanceData = dashboardService.getEmployeeAttendanceDataOrderByInvalidAttendance();
        }

        private void loadEmployeeAttendanceDataOrderByOT()
        {
            employeeAttendanceData = dashboardService.getEmployeeAttendanceDataOrderByOT();
        }

        private void loadPeriods()
        {
            periodList = dashboardService.getPeriodList();
        }

        #endregion

        #region Common Method For multiple Functionalities

        #region Function

        [HttpGet]
        [Route("loadAttendanceData/{period}")]
        public IHttpActionResult loadAttendanceData(string period = null)
        {
            try
            {
                loadEmployeeAttendanceData();
                loadPeriods();
                if (period != "default")
                {
                    //For a specific period
                    IEnumerable<trns_EmployeeAttendanceSumarry> selectedPeriodDataList = employeeAttendanceData.Where(e => e.period_id == new Guid(period));
                    z_Period current_Period = null;
                    foreach (var x in periodList)
                    {
                        if (x.period_id.ToString() == period)
                        {
                            current_Period = x;
                        }
                    }

                    employeeNoPayData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d1 = new List<Top10CountForPeriod>();

                    employeeAbsentData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d2 = new List<Top10CountForPeriod>();

                    employeeLateData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d3 = new List<Top10CountForPeriod>();

                    employeeInvalidAttendanceData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d4 = new List<Top10CountForPeriod>();

                    employeeOTData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d5 = new List<Top10CountForPeriod>();

                    foreach (var x in selectedPeriodDataList)
                    {
                        if (Convert.ToInt32(x.morning_halfday_nopay_count) + Convert.ToInt32(x.evening_halfday_nopay_count) + Convert.ToInt32(x.authorized_nopay_fulldays_count) + Convert.ToInt32(x.nopay_fulldays_count) > 0)
                        {
                            Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                            data.setCount((Convert.ToInt32(x.morning_halfday_nopay_count) + Convert.ToInt32(x.evening_halfday_nopay_count) + Convert.ToInt32(x.authorized_nopay_fulldays_count) + Convert.ToInt32(x.nopay_fulldays_count)).ToString());
                            d1.Add(data);
                        }

                        if (Convert.ToInt32(x.absent_days) > 0)
                        {
                            Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                            data.setCount(x.absent_days);
                            d2.Add(data);
                        }

                        if (TimeConverter.convertMinuteToSeconds(x.late_in_time) + TimeConverter.convertMinuteToSeconds(x.late_out_time) > 0)
                        {
                            Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                            data.setCount((TimeConverter.convertMinuteToSeconds(x.late_in_time) + TimeConverter.convertMinuteToSeconds(x.late_out_time)).ToString());
                            d3.Add(data);
                        }

                        if (Convert.ToInt32(x.invalid_days) > 0)
                        {
                            Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                            data.setCount(x.invalid_days);
                            d4.Add(data);
                        }

                        if (TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime) > 0)
                        {
                            Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                            data.setCount((TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime)).ToString());
                            d5.Add(data);
                        }
                    }

                    employeeNoPayData.setList(d1);
                    employeeNoPayData.getTop10();

                    employeeAbsentData.setList(d2);
                    employeeAbsentData.getTop10();

                    employeeLateData.setList(d3);
                    employeeLateData.orderList();
                    employeeLateData.getTop10FormatTime();

                    employeeInvalidAttendanceData.setList(d4);
                    employeeInvalidAttendanceData.getTop10();

                    employeeOTData.setList(d5);
                    employeeOTData.orderList();
                    employeeOTData.getTop10FormatTime();
                }
                else
                {
                    //For all periods
                    employeeNoPayDataList = new List<EmployeesByPeriod>();
                    employeeAbsentDataList = new List<EmployeesByPeriod>();
                    employeeLateDataList = new List<EmployeesByPeriod>();
                    employeeInvalidAttendanceDataList = new List<EmployeesByPeriod>();
                    employeeOTDataList = new List<EmployeesByPeriod>();

                    foreach (var y in periodList)
                    {
                        employeeNoPayData = new APIData.EmployeesByPeriod(y.period_id.ToString(), y.period_name.ToString());
                        List<Top10CountForPeriod> d1 = new List<Top10CountForPeriod>();

                        employeeAbsentData = new APIData.EmployeesByPeriod(y.period_id.ToString(), y.period_name.ToString());
                        List<Top10CountForPeriod> d2 = new List<Top10CountForPeriod>();

                        employeeLateData = new APIData.EmployeesByPeriod(y.period_id.ToString(), y.period_name.ToString());
                        List<Top10CountForPeriod> d3 = new List<Top10CountForPeriod>();

                        employeeInvalidAttendanceData = new APIData.EmployeesByPeriod(y.period_id.ToString(), y.period_name.ToString());
                        List<Top10CountForPeriod> d4 = new List<Top10CountForPeriod>();

                        employeeOTData = new APIData.EmployeesByPeriod(y.period_id.ToString(), y.period_name.ToString());
                        List<Top10CountForPeriod> d5 = new List<Top10CountForPeriod>();

                        foreach (var x in employeeAttendanceData)
                        {
                            if (TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime) > 0)
                            {
                                if (x.period_id.ToString() == y.period_id.ToString())
                                {
                                    if (Convert.ToInt32(x.morning_halfday_nopay_count) + Convert.ToInt32(x.evening_halfday_nopay_count) + Convert.ToInt32(x.authorized_nopay_fulldays_count) + Convert.ToInt32(x.nopay_fulldays_count) > 0)
                                    {
                                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                        data.setCount((Convert.ToInt32(x.morning_halfday_nopay_count) + Convert.ToInt32(x.evening_halfday_nopay_count) + Convert.ToInt32(x.authorized_nopay_fulldays_count) + Convert.ToInt32(x.nopay_fulldays_count)).ToString());
                                        d1.Add(data);
                                    }

                                    if (Convert.ToInt32(x.absent_days) > 0)
                                    {
                                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                        data.setCount(x.absent_days);
                                        d2.Add(data);
                                    }

                                    if (TimeConverter.convertMinuteToSeconds(x.late_in_time) + TimeConverter.convertMinuteToSeconds(x.late_out_time) > 0)
                                    {
                                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                        data.setCount((TimeConverter.convertMinuteToSeconds(x.late_in_time) + TimeConverter.convertMinuteToSeconds(x.late_out_time)).ToString());
                                        d3.Add(data);
                                    }

                                    if (Convert.ToInt32(x.invalid_days) > 0)
                                    {
                                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                        data.setCount(x.invalid_days);
                                        d4.Add(data);
                                    }

                                    if (TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime) > 0)
                                    {
                                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                        data.setCount((TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime)).ToString());
                                        d5.Add(data);
                                    }
                                }
                            }
                        }

                        employeeNoPayData.setList(d1);
                        employeeNoPayData.getTop10();
                        employeeNoPayDataList.Add(employeeNoPayData);

                        employeeAbsentData.setList(d2);
                        employeeAbsentData.getTop10();
                        employeeAbsentDataList.Add(employeeAbsentData);

                        employeeLateData.setList(d3);
                        employeeLateData.orderList();
                        employeeLateData.getTop10FormatTime();
                        employeeLateDataList.Add(employeeLateData);

                        employeeInvalidAttendanceData.setList(d4);
                        employeeInvalidAttendanceData.getTop10();
                        employeeInvalidAttendanceDataList.Add(employeeInvalidAttendanceData);

                        employeeOTData.setList(d5);
                        employeeOTData.orderList();
                        employeeOTData.getTop10FormatTime();
                        employeeOTDataList.Add(employeeOTData);
                    }
                }
                List<AttendanceObjectList> objList = new List<AttendanceObjectList>();
                List<AttendanceObject> objs = new List<AttendanceObject>();
                if (period == "default")
                {
                    objList.Add(new AttendanceObjectList("NoPay", employeeNoPayDataList));
                    objList.Add(new AttendanceObjectList("Absent", employeeAbsentDataList));
                    objList.Add(new AttendanceObjectList("Late", employeeLateDataList));
                    objList.Add(new AttendanceObjectList("InvalidAttendance", employeeInvalidAttendanceDataList));
                    objList.Add(new AttendanceObjectList("OT", employeeOTDataList));
                    return Ok(objList);
                }
                else
                {
                    objs.Add(new AttendanceObject("NoPay", employeeNoPayData));
                    objs.Add(new AttendanceObject("Absent", employeeAbsentData));
                    objs.Add(new AttendanceObject("Late", employeeLateData));
                    objs.Add(new AttendanceObject("InvalidAttendance", employeeInvalidAttendanceData));
                    objs.Add(new AttendanceObject("OT", employeeOTData));
                    return Ok(objs);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region Top Attendance Employees - For a period

        #region Default

        #endregion

        #region Function

        public IEnumerable<string> getTopAttendanceEmployees(string startDate, string endDate)
        {
            return null;
        }

        #endregion

        #endregion

        #region Nopay List - Last Period Done

        #region Default

        [HttpGet]
        [Route("nopay")]
        public IHttpActionResult getTopNopayList()
        {
            return Ok("Please specify attendance period or default as input for a result");
        }

        #endregion

        #region Function

        [HttpGet]
        [Route("nopay/{period}")]
        public IHttpActionResult getTopNopayList(string period = null)
        {
            /*try
            {
                loadEmployeeAttendanceDataOrderByNoPay();
                loadPeriods();
                if (period != "default")
                {
                    IEnumerable<trns_EmployeeAttendanceSumarry> selectedPeriodDataList = employeeAttendanceData.Where(e => e.period_id == new Guid(period));
                    z_Period current_Period = null;
                    foreach (var x in periodList)
                    {
                        if (x.period_id.ToString() == period)
                        {
                            current_Period = x;
                        }
                    }
                    APIData.EmployeesByPeriod employeeNoPayData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();

                    foreach (var x in selectedPeriodDataList)
                    {
                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                        data.setCount((Convert.ToInt32(x.morning_halfday_nopay_count) + Convert.ToInt32(x.evening_halfday_nopay_count) + Convert.ToInt32(x.authorized_nopay_fulldays_count) + Convert.ToInt32(x.nopay_fulldays_count)).ToString());
                        d.Add(data);
                    }
                    employeeNoPayData.setList(d);
                    employeeNoPayData.getTop10();
                    return Ok(employeeNoPayData);
                }
                else
                {
                    List<EmployeesByPeriod> employeeNoPayDataList = new List<EmployeesByPeriod>();
                    foreach (var y in periodList)
                    {
                        EmployeesByPeriod employeeNoPayData = new EmployeesByPeriod(y.period_id.ToString(), y.period_name);
                        List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();
                        foreach (var x in employeeAttendanceData)
                        {
                            if (x.period_id.ToString() == y.period_id.ToString())
                            {
                                Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                data.setCount((Convert.ToInt32(x.morning_halfday_nopay_count) + Convert.ToInt32(x.evening_halfday_nopay_count) + Convert.ToInt32(x.authorized_nopay_fulldays_count) + Convert.ToInt32(x.nopay_fulldays_count)).ToString());
                                d.Add(data);
                            }
                        }
                        employeeNoPayData.setList(d);
                        employeeNoPayData.getTop10();
                        employeeNoPayDataList.Add(employeeNoPayData);
                    }
                    return Ok(employeeNoPayDataList);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }*/
            return Ok(employeeNoPayDataList);
        }

        #endregion

        #endregion

        #region Absent List - Last Period Done

        #region Default

        [HttpGet]
        [Route("absent")]
        public IHttpActionResult getTopAbsentList()
        {
            return Ok("Please specify attendance period or default as input for a result");
        }

        #endregion

        #region Function

        [HttpGet]
        [Route("absent/{period}")]
        public IHttpActionResult getTopAbsentList(string period = null)
        {
            /*try
            {
                loadEmployeeAttendanceDataOrderByAbsent();
                loadPeriods();
                if (period != "default")
                {
                    IEnumerable<trns_EmployeeAttendanceSumarry> selectedPeriodDataList = employeeAttendanceData.Where(e => e.period_id == new Guid(period));
                    z_Period current_Period = null;
                    foreach (var x in periodList)
                    {
                        if (x.period_id.ToString() == period)
                        {
                            current_Period = x;
                        }
                    }
                    APIData.EmployeesByPeriod employeeAbsentData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();

                    foreach (var x in selectedPeriodDataList)
                    {
                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                        data.setCount(x.absent_days);
                        d.Add(data);
                    }
                    employeeAbsentData.setList(d);
                    employeeAbsentData.getTop10();
                    return Ok(employeeAbsentData);
                }
                else
                {
                    List<EmployeesByPeriod> employeeAbsentDataList = new List<EmployeesByPeriod>();
                    foreach (var y in periodList)
                    {
                        EmployeesByPeriod employeeAbsentData = new EmployeesByPeriod(y.period_id.ToString(), y.period_name);
                        List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();
                        foreach (var x in employeeAttendanceData)
                        {
                            if (x.period_id.ToString() == y.period_id.ToString())
                            {
                                Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                data.setCount(x.absent_days);
                                d.Add(data);
                            }
                        }
                        employeeAbsentData.setList(d);
                        employeeAbsentData.getTop10();
                        employeeAbsentDataList.Add(employeeAbsentData);
                    }
                    return Ok(employeeAbsentDataList);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }*/
            return Ok(employeeAbsentDataList);
        }

        #endregion

        #endregion

        #region Total Late for each Employee - Last Period Done

        #region Default

        [HttpGet]
        [Route("totalLate")]
        public IHttpActionResult getTopTotalLateList()
        {
            return Ok("Please specify attendance period or default as input for a result");
        }

        #endregion

        #region Function

        [HttpGet]
        [Route("totalLate/{period}")]
        public IHttpActionResult getTopTotalLateEmployees(string period = null)
        {
            /*try
            {
                loadEmployeeAttendanceDataOrderByTotalLate();
                loadPeriods();
                if (period != "default")
                {
                    IEnumerable<trns_EmployeeAttendanceSumarry> selectedPeriodDataList = employeeAttendanceData.Where(e => e.period_id == new Guid(period));
                    z_Period current_Period = null;
                    foreach (var x in periodList)
                    {
                        if (x.period_id.ToString() == period)
                        {
                            current_Period = x;
                        }
                    }
                    APIData.EmployeesByPeriod employeeLateData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();

                    foreach (var x in selectedPeriodDataList)
                    {
                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                        data.setCount((TimeConverter.convertMinuteToSeconds(x.late_in_time) + TimeConverter.convertMinuteToSeconds(x.late_out_time)).ToString());
                        d.Add(data);
                    }
                    employeeLateData.setList(d);
                    employeeLateData.orderList();
                    employeeLateData.getTop10FormatTime();
                    return Ok(employeeLateData);
                }
                else
                {
                    List<EmployeesByPeriod> employeeLateDataList = new List<EmployeesByPeriod>();
                    foreach (var y in periodList)
                    {
                        EmployeesByPeriod employeeLateData = new EmployeesByPeriod(y.period_id.ToString(), y.period_name);
                        List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();
                        foreach (var x in employeeAttendanceData)
                        {
                            if (x.period_id.ToString() == y.period_id.ToString())
                            {
                                Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                data.setCount((TimeConverter.convertMinuteToSeconds(x.late_in_time) + TimeConverter.convertMinuteToSeconds(x.late_out_time)).ToString());
                                d.Add(data);
                            }
                        }
                        employeeLateData.setList(d);
                        employeeLateData.orderList();
                        employeeLateData.getTop10FormatTime();
                        employeeLateDataList.Add(employeeLateData);
                    }
                    return Ok(employeeLateDataList);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }*/
            return Ok(employeeLateDataList);
        }

        #endregion

        #endregion

        #region Invalid Attendance Employee Wise - Last Period Done

        #region Default

        [HttpGet]
        [Route("invalidAttendance")]
        public IHttpActionResult getTopInvalidAttendanceList()
        {
            return Ok("Please specify attendance period or default as input for a result");
        }

        #endregion

        #region Function

        [HttpGet]
        [Route("invalidAttendance/{period}")]
        public IHttpActionResult getTopInvalidAttendanceEmployees(string period = null)
        {
            /*try
            {
                loadEmployeeAttendanceDataOrderByInvalidAttendance();
                loadPeriods();
                if (period != "default")
                {
                    IEnumerable<trns_EmployeeAttendanceSumarry> selectedPeriodDataList = employeeAttendanceData.Where(e => e.period_id == new Guid(period));
                    z_Period current_Period = null;
                    foreach (var x in periodList)
                    {
                        if (x.period_id.ToString() == period)
                        {
                            current_Period = x;
                        }
                    }
                    APIData.EmployeesByPeriod employeeInvalidAttendanceData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();

                    foreach (var x in selectedPeriodDataList)
                    {
                        Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                        data.setCount(x.invalid_days);
                        d.Add(data);
                    }
                    employeeInvalidAttendanceData.setList(d);
                    employeeInvalidAttendanceData.getTop10();
                    return Ok(employeeInvalidAttendanceData);
                }
                else
                {
                    List<EmployeesByPeriod> employeeInvalidAttendanceDataList = new List<EmployeesByPeriod>();
                    foreach (var y in periodList)
                    {
                        EmployeesByPeriod employeeInvalidAttendanceData = new EmployeesByPeriod(y.period_id.ToString(), y.period_name);
                        List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();
                        foreach (var x in employeeAttendanceData)
                        {
                            if (x.period_id.ToString() == y.period_id.ToString())
                            {
                                Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                data.setCount(x.invalid_days);
                                d.Add(data);
                            }
                        }
                        employeeInvalidAttendanceData.setList(d);
                        employeeInvalidAttendanceData.getTop10();
                        employeeInvalidAttendanceDataList.Add(employeeInvalidAttendanceData);
                    }
                    return Ok(employeeInvalidAttendanceDataList);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }*/
            return Ok(employeeInvalidAttendanceDataList);
        }

        #endregion

        #endregion

        #region OT list - Last Period Done

        #region Default

        [HttpGet]
        [Route("ot")]
        public IHttpActionResult getAttandanceGroupWise()
        {
            return Ok("Please specify attendance period or default as input for a result");
        }

        #endregion

        #region Function

        [HttpGet]
        [Route("ot/{period}")]
        public IHttpActionResult getTopOTList(string period = null)
        {
            /*try
            {
                loadEmployeeAttendanceDataOrderByOT();
                loadPeriods();
                if (period != "default")
                {
                    IEnumerable<trns_EmployeeAttendanceSumarry> selectedPeriodDataList = employeeAttendanceData.Where(e => e.period_id == new Guid(period));
                    z_Period current_Period = null;
                    foreach (var x in periodList)
                    {
                        if (x.period_id.ToString() == period)
                        {
                            current_Period = x;
                        }
                    }
                    APIData.EmployeesByPeriod employeeOTData = new APIData.EmployeesByPeriod(current_Period.period_id.ToString(), current_Period.period_name.ToString());
                    List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();

                    foreach (var x in selectedPeriodDataList)
                    {
                        if (TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime) > 0)
                        {
                            Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                            data.setCount((TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime)).ToString());
                            d.Add(data);
                        }
                    }
                    employeeOTData.setList(d);
                    employeeOTData.orderList();
                    employeeOTData.getTop10FormatTime();
                    return Ok(employeeOTData);
                }
                else
                {
                    List<EmployeesByPeriod> employeeOTDataList = new List<EmployeesByPeriod>();
                    foreach (var y in periodList)
                    {
                        EmployeesByPeriod employeeOTData = new EmployeesByPeriod(y.period_id.ToString(), y.period_name);
                        List<Top10CountForPeriod> d = new List<Top10CountForPeriod>();
                        foreach (var x in employeeAttendanceData)
                        {
                            if (TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime) > 0)
                            {
                                if (x.period_id.ToString() == y.period_id.ToString())
                                {
                                    Top10CountForPeriod data = new Top10CountForPeriod(x.period_id.ToString(), dashboardServiceERP.getEmployeeDetailsByemployee_id(x.employee_id.ToString()));
                                    data.setCount((TimeConverter.convertMinuteToSeconds(x.actual_ot_intime) + TimeConverter.convertMinuteToSeconds(x.actual_ot_outtime)).ToString());
                                    d.Add(data);
                                }
                            }
                        }
                        employeeOTData.setList(d);
                        employeeOTData.orderList();
                        employeeOTData.getTop10FormatTime();
                        employeeOTDataList.Add(employeeOTData);
                    }
                    return Ok(employeeOTDataList);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }*/
            return Ok(employeeOTDataList);
        }

        #endregion

        #endregion

        #region  Attendance Group Wise, Period Wise

        #region Default

        /*[HttpGet]
        [Route("attGroupWise")]
        public IHttpActionResult getAttandanceGroupWise()
        {
            return Ok("Please specify attendance period or default as input for a result");
        }
        */
        #endregion

        #region Function

        [HttpGet]
        [Route("attGroupWise")]
        public IEnumerable<string> getTopAttendanceGroupWise()
        {
            return null;
        }

        #endregion

        #endregion

        #region OT by department Wise - Last Period

        #region Default

        #endregion

        #region Function

        [HttpGet]
        [Route("otByDept/{period}")]
        public IEnumerable<string> getTopOTListByDepartment(string period = null)
        {

            if (period != "default")
            {
                departmentList = dashboardServiceERP.getDeparments();

                foreach (var x in departmentList)
                {

                }

                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #endregion

        #endregion

    }

}
