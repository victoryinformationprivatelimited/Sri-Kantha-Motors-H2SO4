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

namespace ERP.Reports.Documents.Payroll
{
    class EPFContributionViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public EPFContributionViewModel()
        {
            GetCompanyBranches();
            GetPeriodId();
            GetDepartmentId();
            GetDesignationId();
            GetSectionId();
            GetGradeId();
            GcForm = true;

        }

        #region properties

        private bool _GcForm;

        public bool GcForm
        {
            get { return _GcForm; }
            set { _GcForm = value; OnPropertyChanged("GcForm"); }
        }

        private bool _GR4;

        public bool GR4
        {
            get { return _GR4; }
            set { _GR4 = value; OnPropertyChanged("GR4"); }
        }

        private bool _GR2;

        public bool GR2
        {
            get { return _GR2; }
            set { _GR2 = value; OnPropertyChanged("GR2"); }
        }


        private List<z_CompanyBranches> _CompanyBranches;

        public List<z_CompanyBranches> CompanyBranches
        {
            get { return _CompanyBranches; }
            set { _CompanyBranches = value; OnPropertyChanged("CompanyBranches"); }
        }

        private z_CompanyBranches _CurrentCompanyBranch;

        public z_CompanyBranches CurrentCompanyBranch
        {
            get { return _CurrentCompanyBranch; }
            set { _CurrentCompanyBranch = value; OnPropertyChanged("CurrentCompanyBranch"); }
        }


        private List<z_Period> _Period;

        public List<z_Period> Period
        {
            get { return _Period; }
            set { _Period = value; OnPropertyChanged("Period"); }
        }

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private List<z_Department> _Department;

        public List<z_Department> Department
        {
            get { return _Department; }
            set { _Department = value; OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepartment;

        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { z_Department _CurrentDepartment = value; OnPropertyChanged("CurrentDepartment"); }
        }

        private List<z_Designation> _Designation;

        public List<z_Designation> Designation
        {
            get { return _Designation; }
            set { _Designation = value; OnPropertyChanged("Designation"); }
        }

        private z_Designation _CurrentDesignation;

        public z_Designation CurrentDesignation
        {
            get { return _CurrentDesignation; }
            set { _CurrentDesignation = value; OnPropertyChanged("CurrentDesignation"); }
        }

        private List<z_Section> _Section;

        public List<z_Section> Section
        {
            get { return _Section; }
            set { _Section = value; OnPropertyChanged("Section"); }
        }

        private z_Section _CurrentSection;

        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; OnPropertyChanged("CurrentSection"); }
        }

        private List<z_Grade> _Grade;

        public List<z_Grade> Grade
        {
            get { return _Grade; }
            set { _Grade = value; OnPropertyChanged("Grade"); }
        }

        private z_Grade _CurrentGrade;

        public z_Grade CurrentGrade
        {
            get { return _CurrentGrade; }
            set { _CurrentGrade = value; OnPropertyChanged("CurrentGrade"); }
        }


        #endregion

        #region refresh
        public void GetCompanyBranches()
        {
            CompanyBranches = serviceClient.GetCompanyBranches().ToList();
        }
        public void GetPeriodId()
        {
            Period = serviceClient.GetPeriods().ToList();
        }

        public void GetDepartmentId()
        {
            Department = serviceClient.GetDepartment().ToList();
        }

        public void GetDesignationId()
        {
            Designation = serviceClient.GetDesignations().ToList();
        }

        public void GetSectionId()
        {
            Section = serviceClient.GetSections().ToList();
        }

        public void GetGradeId()
        {
            Grade = serviceClient.GetGrade().ToList();
        }
        #endregion
        
        #region print

        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private void Print()
        {
            string path = "";

            if (GcForm)
            {
                path = "\\Reports\\Documents\\Payroll\\EPFContributionSheet";
            }
            else if (GR4)
            {
                path = "\\Reports\\Documents\\Payroll\\R4";
            }
            else if (GR2)
            {
                path = "\\Reports\\Documents\\Payroll\\R1";
            }

            ReportPrint print = new ReportPrint(path);
            print.setParameterValue("@companybranchid", CurrentCompanyBranch == null ? string.Empty : CurrentCompanyBranch.companyBranch_id.ToString());
            print.setParameterValue("@periodid", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
            print.setParameterValue("@departmentid", CurrentDepartment == null ? string.Empty : CurrentDepartment.department_id.ToString());
            print.setParameterValue("@designationid", CurrentDesignation == null ? string.Empty : CurrentDesignation.designation_id.ToString());
            print.setParameterValue("@sectionid", CurrentSection == null ? string.Empty : CurrentSection.section_id.ToString());
            print.setParameterValue("@gradeid", CurrentGrade == null ? string.Empty : CurrentGrade.grade_id.ToString());
            print.LoadToReportViewer();
        }

        private bool PrintCanExecute()
        {
            if (CurrentCompanyBranch != null && CurrentPeriod != null)
            {
                return true;

            }
            return false;
        }
        #endregion
    }
}
