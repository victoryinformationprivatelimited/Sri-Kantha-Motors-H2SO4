using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ERP.Reports.Documents.HR_Report.NewHrReports.ViewModels
{
    class ExtracurryculerActivityViweModel : ViewModelBase
    {
     private ERPServiceClient serviceClient = new ERPServiceClient();

        public ExtracurryculerActivityViweModel()
        {
            ActivityVisible = Visibility.Visible;
            GetCompanyBranchList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            GetEmployeeList();
            GetEmployeeList();
            GetActivity();

            FormHeader = "Extra Curricular Activities Report";
        }
        private Visibility _ActivityVisible;
        public Visibility ActivityVisible
        {
            get { return _ActivityVisible; }
            set { _ActivityVisible = value; OnPropertyChanged("ActivityVisible"); }
        }


        private string _FormHeader;
        public string FormHeader
        {
            get { return _FormHeader; }
            set { _FormHeader = value; OnPropertyChanged("FormHeader"); }
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

        private IEnumerable<z_ExtraCurricularActivities> _Activity;
        public IEnumerable<z_ExtraCurricularActivities> Activity
        {
            get { return _Activity; }
            set { _Activity = value; OnPropertyChanged("Activity"); }
        }

        private z_ExtraCurricularActivities _CurrentActivity;

        public z_ExtraCurricularActivities CurrentActivity
        {
            get { return _CurrentActivity; }
            set { _CurrentActivity = value; OnPropertyChanged("CurrentActivity"); }
        }



        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private void Print()
        {
            try
            {
                string path = "\\Reports\\Documents\\HR_Report\\NewHrReports\\Reports\\ExtracurycilerActivity";

                ReportPrint print = new ReportPrint(path);

                print.setParameterValue("@companyBranchid", CurrentCompanyBranch == null ? string.Empty : CurrentCompanyBranch.companyBranch_id.ToString());
                print.setParameterValue("@dapartmentid", CurrentDepartment == null ? string.Empty : CurrentDepartment.department_id.ToString());
                print.setParameterValue("@designationid", CurrentDesignation == null ? string.Empty : CurrentDesignation.designation_id.ToString());
                print.setParameterValue("@sectionid", CurrentSection == null ? string.Empty : CurrentSection.section_id.ToString());
                print.setParameterValue("@gradeid", CurrentGrade == null ? string.Empty : CurrentGrade.grade_id.ToString());
                print.setParameterValue("@employeeid", CurrentEmployee == null ? string.Empty : CurrentEmployee.employee_id.ToString());
                print.setParameterValue("@companyBranchName", CurrentCompanyBranch == null ? "Not Selected" : CurrentCompanyBranch.companyBranch_Name);
                print.setParameterValue("@departmentName", CurrentDepartment == null ? "Not Selected" : CurrentDepartment.department_name);
                print.setParameterValue("@designationName", CurrentDesignation == null ? "Not Selected" : CurrentDesignation.designation);
                print.setParameterValue("@sectionName", CurrentSection == null ? "Not Selected" : CurrentSection.section_name);
                print.setParameterValue("@gradeName", CurrentGrade == null ? "Not Selected" : CurrentGrade.grade);
                print.setParameterValue("@activityType", CurrentActivity == null ? "Not Selected" : CurrentActivity.activities_category_id.ToString());

                print.LoadToReportViewer();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool PrintCanExecute()
        {
            return true;
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

        #region Activity

        private void GetActivity()
        {
            serviceClient.GetExtraCurricularActivitiesCompleted += (s, e) =>
            {
                try
                {
                    Activity = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetExtraCurricularActivitiesAsync();
        }

        #endregion

    }
}
