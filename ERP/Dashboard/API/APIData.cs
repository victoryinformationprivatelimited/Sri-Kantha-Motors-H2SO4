using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Dashboard.API
{
    class APIData
    {
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

            public void setMonthWiseEmployeeList(List<MonthWiseEmploees> monthWiseEmployees)
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

        #region EmployeeBirthday Model
        public class EmployeeBirthday
        {
            private string emp_id;
            private string name;
            private string birthday;

            public EmployeeBirthday(string id, string name, string birthday)
            {
                this.emp_id = id;
                this.name = name;
                this.birthday = birthday;
            }

            public string getId()
            {
                return emp_id;
            }
            public string getName()
            {
                return name;
            }
            public string getBirthday()
            {
                return birthday;
            }
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

            public EmployeesByDepartment(string department_id, string departmentName, string count)
            {
                this.employeeList = new List<EmployeeSumarryView>();
                this.department_Name = departmentName;
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
    }
}
