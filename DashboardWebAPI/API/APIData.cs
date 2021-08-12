using DashboardService.Tools;
using ERPData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace ERP.Dashboard.API
{
    class APIData
    {

        #region Common

        #region Split Given datetime format from string to DateTime

        public static DateTime[] SplitDateTimeinputs(string startDate, string endDate)
        {
            try
            {
                string[] startDateDataList = startDate.Split('-');
                string[] endDateDataList = endDate.Split('-');
                DateTime startDateDate = Convert.ToDateTime(startDateDataList[0] + "/" + startDateDataList[1] + "/" + startDateDataList[2]);
                DateTime endDateDate = Convert.ToDateTime(endDateDataList[0] + "/" + endDateDataList[1] + "/" + endDateDataList[2]);

                DateTime[] output = { startDateDate, endDateDate };
                return output;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region Employee Section

        #region Month Wise Employee Count

        public class MonthWiseEmploees
            {
            
                #region Properties
                public string monthNo;
                public List<EmployeeSumarryView> employeeList;
                public string count;
                #endregion

                #region Getters and Setters

                public string getMonthNo()
                {
                    return monthNo;
                }

                public List<EmployeeSumarryView> getEmployeeList()
                {
                    return employeeList;
                }

                public string getCount()
                {
                    return count;
                }

                public void setMonthNo(string monthNo)
                {
                    this.monthNo = monthNo;
                }

                public void setEmployeeList(List<EmployeeSumarryView> employeeList)
                {
                    this.employeeList = employeeList;
                }

                public void setCount(string count)
                {
                    this.count = count;
                }

                #endregion

                #region Constructor

                public MonthWiseEmploees(string monthNo, string count)
                {
                    this.employeeList = new List<EmployeeSumarryView>();
                    this.monthNo = monthNo;
                    this.count = count;
                }

                #endregion

                #region Methods

                public void AddEmp(EmployeeSumarryView emp)
                {
                    this.employeeList.Add(emp);
                }

                public void increase()
                {
                    int temp = Convert.ToInt32(this.count);
                    ++temp;
                    this.count = temp.ToString();
                }

                #endregion

            }

            #endregion

            #region Year Wise Employee Count

            public class YearWiseEmploees
            {

                #region Properties

                public string year;
                public string count;
                public List<EmployeeSumarryView> employeeList;
                public List<MonthWiseEmploees> monthWiseEmployees;

                #endregion

                #region Getters and Setters

                public string getYear()
                {
                    return year;
                }

                public string getCount()
                {
                    return count;
                }

                public List<EmployeeSumarryView> getEmployeeList()
                {
                    return employeeList;
                }

                public List<MonthWiseEmploees> getMonthWiseEmployeeList()
                {
                    return monthWiseEmployees;
                }

                public void setYear(string year)
                {
                    this.year = year;
                }

                public void setCount(string count)
                {
                    this.count = count;
                }

                public void setEmployeeList(List<MonthWiseEmploees> monthWiseEmployees)
                {
                    this.monthWiseEmployees = monthWiseEmployees;
                }

                #endregion

                #region Constructor

                public YearWiseEmploees(string year, string count)
                {
                    this.employeeList = new List<EmployeeSumarryView>();
                    this.monthWiseEmployees = new List<MonthWiseEmploees>();
                    this.year = year;
                    this.count = count;
                }

                #endregion

                #region Methods

                public void AddEmp(EmployeeSumarryView emp)
                {
                    this.employeeList.Add(emp);
                }

                public void increase()
                {
                    int temp = Convert.ToInt32(this.count);
                    ++temp;
                    this.count = temp.ToString();
                }

                public void countAndStoreMonthWisePermanentDateWise()
                {
                    try
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            MonthWiseEmploees m = new MonthWiseEmploees((i + 1).ToString(), 0.ToString());
                            monthWiseEmployees.Add(m);
                        }

                        foreach (var x in employeeList)
                        {
                            monthWiseEmployees[x.prmernant_active_date.Value.Month - 1].AddEmp(x);
                            monthWiseEmployees[x.prmernant_active_date.Value.Month - 1].increase();
                        }
                    }
                    catch (Exception e)
                    {
                
                    }
                }

                public void countAndStoreMonthWiseResignDateWise()
                {
                    try
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            MonthWiseEmploees m = new MonthWiseEmploees((i + 1).ToString(), 0.ToString());
                            monthWiseEmployees.Add(m);
                        }

                        foreach (var x in employeeList)
                        {
                            monthWiseEmployees[x.resign_date.Value.Month - 1].AddEmp(x);
                            monthWiseEmployees[x.resign_date.Value.Month - 1].increase();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                public void countAndStoreMonthWiseJoinDateWise()
                {
                    try
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            MonthWiseEmploees m = new MonthWiseEmploees((i + 1).ToString(), 0.ToString());
                            monthWiseEmployees.Add(m);
                        }

                        foreach (var x in employeeList)
                        {
                            monthWiseEmployees[x.join_date.Value.Month - 1].AddEmp(x);
                            monthWiseEmployees[x.join_date.Value.Month - 1].increase();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                #endregion
            }

            #endregion

            #region Employee Count By Department

            public class EmployeesByDepartment
            {

                #region Properties

                public string department_id;
                public string department_Name;
                public List<EmployeeSumarryView> employeeList;
                public string count;

                #endregion

                #region Setters and Getters

                public string getDepartment_id()
                {
                    return department_id;
                }

                public List<EmployeeSumarryView> getEmployeeList()
                {
                    return employeeList;
                }

                public string getCount()
                {
                    return count;
                }

                public string getDepartmentName()
                {
                    return department_Name;
                }

                public void setDepartment_id(string department_id)
                {
                    this.department_id = department_id;
                }

                public void setEmployeeList(List<EmployeeSumarryView> employeeList)
                {
                    this.employeeList = employeeList;
                }

                public void setCount(string count)
                {
                    this.count = count;
                }

                public void setDepartmentName(string departmentName)
                {
                    this.department_Name = departmentName;
                }

                #endregion

                #region Methods

                public void AddEmp(EmployeeSumarryView emp)
                {
                    this.employeeList.Add(emp);
                }

                public void increase()
                {
                    int temp = Convert.ToInt32(this.count);
                    ++temp;
                    this.count = temp.ToString();

                }

                #endregion

                #region Constructor

                public EmployeesByDepartment(string department_id, string department_Name,string count) {
                    this.employeeList = new List<EmployeeSumarryView>();
                    this.department_Name = department_Name;
                    this.department_id = department_id;
                    this.count = count;
                }

            #endregion

            }

            #endregion

            #region Employee Count By Designation

            public class EmployeesByDesignation
            {

                #region Properties

                public string designation_id;
                public string designation_Name;
                public List<EmployeeSumarryView> employeeList;
                public string count;

                #endregion

                #region Setters and Getters

                public string getDesignation_id()
                {
                    return designation_id;
                }

                public List<EmployeeSumarryView> getEmployeeList()
                {
                    return employeeList;
                }

                public string getCount()
                {
                    return count;
                }

                public string getDesignationName()
                {
                    return designation_Name;
                }

                public void setDesignation_id(string designation_id)
                {
                    this.designation_id = designation_id;
                }

                public void setEmployeeList(List<EmployeeSumarryView> employeeList)
                {
                    this.employeeList = employeeList;
                }

                public void setCount(string count)
                {
                    this.count = count;
                }

                public void setDesignationName(string designationName)
                {
                    this.designation_Name = designationName;
                }

                #endregion

                #region Methods

                public void AddEmp(EmployeeSumarryView emp)
                {
                    this.employeeList.Add(emp);
                }

                public void increase()
                {
                    int temp = Convert.ToInt32(this.count);
                    ++temp;
                    this.count = temp.ToString();

                }

                #endregion

                #region Constructor

                public EmployeesByDesignation(string designation_id, string designation_Name, string count)
                {
                    this.employeeList = new List<EmployeeSumarryView>();
                    this.designation_Name = designation_Name;
                    this.designation_id = designation_id;
                    this.count = count;
                }

                #endregion

            }

        #endregion

            #region Object List
            public class ERPObjectList
            {
                public string mode;
                public List<EmployeeSumarryView> empObjList;
                public List<YearWiseEmploees> yEmpObjList;
                public List<EmployeesByDepartment> empByDeptList;
                public List<EmployeesByDesignation> empByDesignationList;

                public ERPObjectList(string mode, List<EmployeeSumarryView> empObjList)
                {
                    this.mode = mode;
                    this.empObjList = empObjList;
                }

                public ERPObjectList(string mode, List<YearWiseEmploees> yEmpObjList)
                {
                    this.mode = mode;
                    this.yEmpObjList = yEmpObjList;
                }

                public ERPObjectList(string mode, List<EmployeesByDepartment> empByDeptList)
                {
                    this.mode = mode;
                    this.empByDeptList = empByDeptList;
                }

                public ERPObjectList(string mode, List<EmployeesByDesignation> empByDesignationList)
                {
                    this.mode = mode;
                    this.empByDesignationList = empByDesignationList;
                }
            }
            #endregion

        #endregion

        #region Attendance Section

        #region Top10CountForPeriod

        public class Top10CountForPeriod
        {
            public EmployeeSumarryView employeeDetails;
            public string period_id;
            /*public string morning_halfday_nopay_count;
            public string evening_halfday_nopay_count;
            public string authorized_nopay_fulldays_count;
            public string nopay_fulldays_count;*/
            public string count;

            public Top10CountForPeriod(string period_id, EmployeeSumarryView employee)
            {
                this.employeeDetails = employee;
                this.period_id = period_id;
                this.count = 0.ToString();
            }

            public void setCount(string count)
            {
                this.count = count;
            }
        }

        public class EmployeesByPeriod
        {
            public string period_id;
            public string period_name;
            private IEnumerable<Top10CountForPeriod> FullList;
            public IEnumerable<Top10CountForPeriod> Top10;

            public EmployeesByPeriod(string period_id)
            {
                this.period_id = period_id;
                FullList = new List<Top10CountForPeriod>();
            }

            public EmployeesByPeriod(string period_id, string period_name)
            {
                this.period_id = period_id;
                this.period_name = period_name;
                FullList = new List<Top10CountForPeriod>();
            }

            public void setList(List<Top10CountForPeriod> list)
            {
                this.FullList = list;
            }

            public IEnumerable<Top10CountForPeriod> getTop10()
            {
                Top10 = FullList.OrderByDescending(e => Convert.ToInt32(e.count)).Take(10);
                return Top10;
            }

            public IEnumerable<Top10CountForPeriod> getTop10FormatTime()
            {
                Top10 = FullList.OrderByDescending(e => Convert.ToInt32(e.count)).Take(10);
                List<Top10CountForPeriod> temp = Top10.ToList();
                foreach (var x in temp)
                {
                    x.count = TimeConverter.convertSecondsToMinutes(Convert.ToInt32(x.count));
                }
                Top10 = temp;
                return Top10;
            }

            public void orderList()
            {
                FullList.OrderBy(e => e.count);
            }
        }

        #endregion

        #region Object List
        public class AttendanceObject
        {
            public string mode;
            public EmployeesByPeriod obj;

            public AttendanceObject(string mode, EmployeesByPeriod obj)
            {
                this.mode = mode;
                this.obj = obj;
            }
        }

        public class AttendanceObjectList
        {
            public string mode;
            public List<EmployeesByPeriod> objList;

            public AttendanceObjectList(string mode, List<EmployeesByPeriod> objList)
            {
                this.mode = mode;
                this.objList = objList;
            }
        }
        #endregion

        /*#region Attendance Top 10 Nopay

        public class AttendanceNopayData
        {
            public string employee_id;
            public string period_id;
            /*public string morning_halfday_nopay_count;
            public string evening_halfday_nopay_count;
            public string authorized_nopay_fulldays_count;
            public string nopay_fulldays_count;
            public string nopayCount;

            public AttendanceNopayData(string employee_id, string period_id)
            {
                this.employee_id = employee_id;
                this.period_id = period_id;
                this.nopayCount = 0.ToString();
            }

            public void setNoPayCount(string count)
            {
                this.nopayCount = count;
            }
        }

        public class NoPayEmployeesByPeriod
        {
            public string period_id;
            public string period_name;
            private IEnumerable<AttendanceNopayData> NoPayList;
            public IEnumerable<AttendanceNopayData> Top10;

            public NoPayEmployeesByPeriod(string period_id)
            {
                this.period_id = period_id;
                NoPayList = new List<AttendanceNopayData>();
            }

            public NoPayEmployeesByPeriod(string period_id, string period_name)
            {
                this.period_id = period_id;
                this.period_name = period_name;
                NoPayList = new List<AttendanceNopayData>();
            }

            public void setList(List<AttendanceNopayData> list)
            {
                this.NoPayList = list;
            }

            public IEnumerable<AttendanceNopayData> getTop10()
            {
                Top10 = NoPayList.OrderByDescending(e => Convert.ToInt32(e.nopayCount)).Take(10);
                return Top10;
            }
        }

        #endregion

        #region Attendance Top  10 Absent

        public class AttendanceAbsentData
        {
            public string employee_id;
            public string period_id;
            /*public string morning_halfday_nopay_count;
            public string evening_halfday_nopay_count;
            public string authorized_nopay_fulldays_count;
            public string nopay_fulldays_count;
            public string absentCount;

            public AttendanceAbsentData(string employee_id, string period_id)
            {
                this.employee_id = employee_id;
                this.period_id = period_id;
                this.absentCount = 0.ToString();
            }

            public void setAbsentCount(string count)
            {
                this.absentCount = count;
            }
        }

        public class AbsentEmployeesByPeriod
        {
            public string period_id;
            public string period_name;
            private IEnumerable<AttendanceAbsentData> absentList;
            public IEnumerable<AttendanceAbsentData> Top10;

            public AbsentEmployeesByPeriod(string period_id)
            {
                this.period_id = period_id;
                absentList = new List<AttendanceAbsentData>();
            }

            public AbsentEmployeesByPeriod(string period_id, string period_name)
            {
                this.period_id = period_id;
                this.period_name = period_name;
                absentList = new List<AttendanceAbsentData>();
            }

            public void setList(List<AttendanceAbsentData> list)
            {
                this.absentList = list;
            }

            public IEnumerable<AttendanceAbsentData> getTop10()
            {
                Top10 = absentList.OrderByDescending(e => Convert.ToInt32(e.absentCount)).Take(10);
                return Top10;
            }
        }

        #endregion*/

        #endregion

        #region Payroll Section

        #endregion
    }
}
