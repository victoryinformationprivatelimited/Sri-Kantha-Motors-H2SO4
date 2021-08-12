using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class EmployeeResignDetailsViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient;
        public EmployeeResignDetailsViewModel()
        {
            serviceClient = new ERPServiceClient();
        }

        #region refreshMethods

        #region Refresh Methods

        private void ResfershDesignations()
        {
            serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                Designation = e.Result;
            };
            serviceClient.GetDesignationsAsync();
        }

        private void ResfershDepartment()
        {
            serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                Departments = e.Result;
            };
            serviceClient.GetDepartmentsAsync();

        }

        private void ResfershSection()
        {
            serviceClient.GetSectionsCompleted += (s, e) =>
            {
                Section = e.Result;
            };

            serviceClient.GetSectionsAsync();
        }

        private void ResfreshBranch()
        {
            serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                CompanyBranche = e.Result;
            };
            serviceClient.GetCompanyBranchesAsync();
        }


        #endregion

        private IEnumerable<z_CompanyBranches> companyBranches;
        public IEnumerable<z_CompanyBranches> CompanyBranche
        {
            get { return companyBranches; }
            set { companyBranches = value; this.OnPropertyChanged("CompanyBranche"); }
        }

        private z_CompanyBranches currentCompanyBranch;
        public z_CompanyBranches CurrentCompanyBranch
        {
            get { return currentCompanyBranch; }
            set { currentCompanyBranch = value; }
        }

        private IEnumerable<z_Department> departments;
        public IEnumerable<z_Department> Departments
        {
            get { return departments; }
            set { departments = value; this.OnPropertyChanged("Departments"); }
        }

        private z_Department currentDepartment;
        public z_Department CurrentDepartment
        {
            get { return currentDepartment; }
            set { currentDepartment = value; this.OnPropertyChanged("CurrentDepartment"); }
        }

        private IEnumerable<z_Section> sectionN;
        public IEnumerable<z_Section> Section
        {
            get { return sectionN; }
            set { sectionN = value; this.OnPropertyChanged("Section"); }
        }

        private z_Section currentSection;
        public z_Section CurrentSection
        {
            get { return currentSection; }
            set { currentSection = value; this.OnPropertyChanged("CurrentSection"); }
        }

        private IEnumerable<z_Grade> grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return grades; }
            set { grades = value; this.OnPropertyChanged("Grades"); }
        }
        private z_Grade currentGrade;
        public z_Grade CurrentGrade
        {
            get { return currentGrade; }
            set { currentGrade = value; this.OnPropertyChanged("CurrentGrade"); }
        }

        private IEnumerable<z_Designation> designationN;
        public IEnumerable<z_Designation> Designation
        {
            get { return designationN; }
            set { designationN = value; this.OnPropertyChanged("Designation"); }
        }

        private z_Designation currentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return currentDesignation; }
            set { currentDesignation = value; this.OnPropertyChanged("CurrentDesignation"); }
        }

        private int resign;
        public int Resign
        {
            get { return resign; }
            set { resign = value; this.OnPropertyChanged("Resign"); }
        }

        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private void Print()
        {

                string path = "\\Reports\\Documents\\HR_Report\\EmployeeResignDaYDetails";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@month", Resign);
                print.LoadToReportViewer();
            
        }


        private bool PrintCanExecute()
        {
            return true;
        }

        //public void GetPeriodList()
        //{
        //    this.serviceClient.GetPeriodsCompleted += (s, e) =>
        //    {
        //        Periods = e.Result.Where(z => z.isdelete == false);
        //    };
        //    this.serviceClient.GetPeriodsAsync();

        //}



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