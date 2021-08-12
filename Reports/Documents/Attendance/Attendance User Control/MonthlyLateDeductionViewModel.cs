﻿using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    public class MonthlyLateDeductionViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public List<z_Department> department = new List<z_Department>();
        public List<z_Section> section = new List<z_Section>();
        public List<EmployeeSumarryView> employee = new List<EmployeeSumarryView>();
        public List<z_Designation> designation = new List<z_Designation>();

        public MonthlyLateDeductionViewModel()
        {
            GetCompanyBranchList();
            GetPeriodList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            GetEmployeeList();
            GetEmployeeShift();
            FilterFomula = "";
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;

        }

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; this.OnPropertyChanged("StartDate"); }
        }
        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; this.OnPropertyChanged("EndDate"); }
        }


        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; this.OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; this.OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<z_CompanyBranches> _CompanyBranches;
        public IEnumerable<z_CompanyBranches> CompanyBranche
        {
            get { return _CompanyBranches; }
            set { _CompanyBranches = value; this.OnPropertyChanged("CompanyBranche"); }
        }

        private z_CompanyBranches _CurrentCompanyBranch;
        public z_CompanyBranches CurrentCompanyBranch
        {
            get { return _CurrentCompanyBranch; }
            set { _CurrentCompanyBranch = value; }
        }

        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Departments"); }
        }

        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; this.OnPropertyChanged("CurrentDepartment"); }
        }

        private IEnumerable<z_Section> _Section;
        public IEnumerable<z_Section> Section
        {
            get { return _Section; }
            set { _Section = value; this.OnPropertyChanged("Section"); }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; this.OnPropertyChanged("CurrentSection"); }
        }

        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return _Grades; }
            set { _Grades = value; this.OnPropertyChanged("Grades"); }
        }
        private z_Grade _CurrentGrade;
        public z_Grade CurrentGrade
        {
            get { return _CurrentGrade; }
            set { _CurrentGrade = value; this.OnPropertyChanged("CurrentGrade"); }
        }

        private IEnumerable<z_Designation> _Designation;
        public IEnumerable<z_Designation> Designation
        {
            get { return _Designation; }
            set { _Designation = value; this.OnPropertyChanged("Designation"); }
        }

        private z_Designation _CurrentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return _CurrentDesignation; }
            set { _CurrentDesignation = value; this.OnPropertyChanged("CurrentDesignation"); }
        }

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<z_ShiftCategory> _ShiftCatagory;
        public IEnumerable<z_ShiftCategory> ShiftCatagory
        {
            get { return _ShiftCatagory; }
            set { _ShiftCatagory = value; this.OnPropertyChanged("ShiftCatagory"); }
        }


        private z_ShiftCategory _CurrentShiftCatagory;
        public z_ShiftCategory CurrentShiftCatagory
        {
            get { return _CurrentShiftCatagory; }
            set { _CurrentShiftCatagory = value; this.OnPropertyChanged("CurrentShiftCatagory"); }
        }

        private string _FilterFomula;
        public string FilterFomula
        {
            get { return _FilterFomula; }
            set { _FilterFomula = value; this.OnPropertyChanged("FilterFomula"); }
        }


        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private void Print()
        {
            string rpt_period_id = "";
            string rpt_companyBranch_id = "";
            string rpt_department_id = "";
            string rpt_section_id = "";
            string rpt_designation_id = "";
            string rpt_grade_id = "";
            string rpt_employee_id = "";
            string rpt_shift_id = "";

            if (CurrentCompanyBranch != null)
            {
                rpt_companyBranch_id = CurrentCompanyBranch.companyBranch_id.ToString();
            }
            if (CurrentPeriod != null)
            {
                rpt_period_id = CurrentPeriod.period_id.ToString();
            }
            if (CurrentDepartment != null)
            {
                rpt_department_id = CurrentDepartment.department_id.ToString();
            }
            if (CurrentDesignation != null)
            {
                rpt_designation_id = CurrentDesignation.designation_id.ToString();
            }
            if (CurrentSection != null)
            {
                rpt_section_id = CurrentSection.section_id.ToString();
            }
            if (CurrentGrade != null)
            {
                rpt_grade_id = CurrentGrade.grade_id.ToString();
            }

            if (CurrentEmployee != null)
            {
                rpt_employee_id = CurrentEmployee.employee_id.ToString();
            }
            if (CurrentShiftCatagory != null)
            {
                rpt_shift_id = CurrentShiftCatagory.shift_id.ToString();
            }
            string[,] parameetors = new string[10, 2] {{ "@startDate", getDateString(StartDate) },{ "@endDate", getDateString(EndDate) }, { "@timePeriodID", rpt_period_id }, { "@companyBranchid",rpt_companyBranch_id  },
                                        { "@dapartmentid", rpt_department_id },{"@designationid",rpt_designation_id},{"@sectionid", rpt_section_id},{"@gradeid", rpt_grade_id},{"@employeeid",rpt_employee_id},{"@shiftid",rpt_shift_id} };

            ReportViewer form = new ReportViewer();
            form.GetReportUsingParameetors(parameetors, "Attendance", "MonthlyLateInAndEarlyOutAttendance", FilterFomula, "");
            form.Show();
        }
        public string getDateString(DateTime date)
        {
            string date_string = "";
            // 6/18/2014 12:00:00 AM
            try
            {

                //string[] words = date.ToString().Split(' ');
                //date_string = words[0];
                //string[] words2 = date_string.Split('/');
                //string day = words2[1];
                //string Month = words2[0];
                //string year = words2[2].ToString();
                //if (Month.Length == 1)
                //{
                //    Month = "0" + Month;
                //}
                //if (day.Length == 1)
                //{
                //    day = "0" + day;
                //}
                date_string = date.Year + "-" + date.Month.ToString("00") + "-" + date.Day.ToString("00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message);
            }
            return date_string;
        }
        private bool PrintCanExecute()
        {
            if (StartDate != null)
            {
                return true;

            }
            if (EndDate != null)
            {
                return true;

            }
            return false;
        }

        public void GetPeriodList()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetPeriodsAsync();

        }

        #region Company Branch
        public void GetCompanyBranchList()
        {
            this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                CompanyBranche = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetCompanyBranchesAsync();

        }
        #endregion

        #region Department List
        public void GetDepartmentList()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                Departments = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDepartmentsAsync();

        }
        #endregion

        #region Section List
        public void GetSectionList()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                Section = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetSectionsAsync();

        }
        #endregion

        #region Grade List
        public void GetGradeList()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                Grades = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetGradeAsync();

        }
        #endregion

        #region Designation List
        public void GetDesignationList()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                Designation = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDesignationsAsync();

        }
        #endregion

        #region Employee List
        public void GetEmployeeList()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result.Where(z => z.isdelete == false && z.isActive == true);
            };
            this.serviceClient.GetAllEmployeeDetailAsync();

        }
        #endregion

        #region Shift List
        public void GetEmployeeShift()
        {
            this.serviceClient.GetShiftcategoryCompleted += (s, e) =>
            {
                ShiftCatagory = e.Result.Where(z => z.isdelete == false && z.is_active == true);

            };
            this.serviceClient.GetShiftcategoryAsync();

        }
        #endregion


        #region Filter Employee
        //public void EmployeeFiltering()
        //{
        //    if (Employees != null)
        //    {
        //        if (CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty && CurrentDepartment == null)
        //        {
        //            Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
        //            Departments = Departments.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
        //        }
        //        if (CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty && CurretSection == null)
        //        {
        //            if (CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty && CurretSection == null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id);
        //                //Sections = Sections.Where(z => z.department_id == CurrentDepartment.department_id);
        //            }
        //            else
        //            {
        //                Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id);
        //            }
        //        }
        //        if (CurretSection != null && CurretSection.section_id != Guid.Empty)
        //        {

        //            if (CurretCompanyBranch != null && CurrentDepartment != null && CurretSection != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id);
        //            }
        //            else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty)
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.department_id == CurrentDepartment.department_id);
        //            }
        //            else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty)
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.branch_id == CurretCompanyBranch.companyBranch_id);
        //            }
        //            else
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id);
        //            }


        //        }
        //        if (CurretDesignation != null && CurretDesignation.designation_id != Guid.Empty)
        //        {
        //            if (CurretDesignation != null && CurretCompanyBranch != null && CurrentDepartment != null && CurretSection != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty && CurretDesignation != null && CurretDesignation.designation_id == Guid.Empty)
        //            {
        //                Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id && z.section_id == CurretSection.section_id);
        //            }
        //            else if (CurretSection != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else if (CurretCompanyBranch != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else if (CurrentDepartment != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else
        //            {
        //                Employees = AllEmployee.Where(z => z.designation_id == CurretDesignation.designation_id);
        //            }
        //        }
        //        if (CurretGrade != null && CurretGrade.grade_id != Guid.Empty)
        //        {
        //            Employees = AllEmployee.Where(z => z.grade_id == CurretGrade.grade_id);
        //        }

        //    }
        //}
        #endregion
    }
}
