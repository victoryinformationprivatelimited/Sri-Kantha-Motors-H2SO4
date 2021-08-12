using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class NewEmploymentStatusReportViewModel : ViewModelBase
    {

        ERPServiceClient serviceClient;

        public NewEmploymentStatusReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            WindowHeader = "Employment Status Report";
            HistoryVisblie = Visibility.Hidden;
            Clean();
        }

        #region Refresh Methods

        private void ResfershDepartment()
        {
            serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                Depatments = e.Result;
            };
            serviceClient.GetDepartmentsAsync();

        }

        private void ResfershDesignations()
        {
            serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                Designations = e.Result;
            };
            serviceClient.GetDesignationsAsync();
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
                Branch = e.Result;
            };
            serviceClient.GetCompanyBranchesAsync();
        }

        private void RefreshPayment()
        {
            serviceClient.GetPaymentMethodsCompleted += (s, e) =>
            {
                Payment = e.Result;
            };
            serviceClient.GetPaymentMethodsAsync();
        }

        private void RefreshGrade()
        {
            serviceClient.GetGradeCompleted += (s, e) =>
            {
                Grade = e.Result;
            };
            serviceClient.GetGradeAsync();
        }

        private void RefreshGender()
        {
            serviceClient.getGendersCompleted += (s, e) =>
            {
                Gender = e.Result;
            };
            serviceClient.getGendersAsync();
        }

        private void RefreshReligen()
        {
            serviceClient.GetReligenCompleted += (s, e) =>
            {
                Religen = e.Result;
            };
            serviceClient.GetReligenAsync();
        }

        private void RefreshCivil()
        {
            serviceClient.getCivilStatesCompleted += (s, e) =>
            {
                Civil = e.Result;
            };
            serviceClient.getCivilStatesAsync();
        }

        private void RefreshCity()
        {
            serviceClient.GetCitiesCompleted += (s, e) =>
            {
                City = e.Result;
            };
            serviceClient.GetCitiesAsync();
        }

        private void RefreshTown()
        {
            serviceClient.GetTownsCompleted += (s, e) =>
            {
                Town = e.Result;
            };
            serviceClient.GetTownsAsync();
        }

        private void RefreshSubDepartment()
        {
            serviceClient.GetBloodTypesCompleted += (s, e) =>
            {
                SubDepartment = e.Result;
            };
            serviceClient.GetBloodTypesAsync();
        }

        public void RefreshEmployees()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result.Where(z => z.isdelete == false && z.isActive == true);
            };
            this.serviceClient.GetAllEmployeeDetailAsync();

        }

        #endregion

        #region Clean Method

        private void Clean()
        {
            ResfershDepartment();
            ResfershDesignations();
            ResfershSection();
            ResfreshBranch();
            RefreshPayment();
            RefreshGrade();
            RefreshGender();
            RefreshReligen();
            RefreshCivil();
            RefreshCity();
            RefreshTown();
            RefreshEmployees();
        }

        bool cleanCanExecute()
        {
            return true;
        }
        #endregion

        #region Properties

        private string _WindowHeader;
        public string WindowHeader
        {
            get { return _WindowHeader; }
            set { _WindowHeader = value; OnPropertyChanged("WindowHeader"); }
        }


        private Visibility _HistoryVisblie;
        public Visibility HistoryVisblie
        {
            get { return _HistoryVisblie; }
            set { _HistoryVisblie = value; OnPropertyChanged("HistoryVisblie"); }
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

        private IEnumerable<z_Designation> designations;
        public IEnumerable<z_Designation> Designations
        {
            get { return designations; }
            set { designations = value; OnPropertyChanged("Designations"); }
        }

        private z_Designation currentDesignations;
        public z_Designation CurrentDesignations
        {
            get { return currentDesignations; }
            set { currentDesignations = value; OnPropertyChanged("CurrentDesignations"); }
        }


        private IEnumerable<z_Department> depatments;
        public IEnumerable<z_Department> Depatments
        {
            get { return depatments; }
            set { depatments = value; this.OnPropertyChanged("Depatments"); }
        }

        private z_Department currentDepatments;
        public z_Department CurrentDepatments
        {
            get { return currentDepatments; }
            set { currentDepatments = value; this.OnPropertyChanged("CurrentDepatments"); }
        }

        private IEnumerable<z_Section> section;
        public IEnumerable<z_Section> Section
        {
            get { return section; }
            set { section = value; this.OnPropertyChanged("Section"); }
        }

        private z_Section currentSection;
        public z_Section CurrentSection
        {
            get { return currentSection; }
            set { currentSection = value; this.OnPropertyChanged("CurrentSection"); }
        }

        private IEnumerable<z_CompanyBranches> branch;
        public IEnumerable<z_CompanyBranches> Branch
        {
            get { return branch; }
            set { branch = value; this.OnPropertyChanged("Branch"); }
        }

        private z_CompanyBranches currentBranch;
        public z_CompanyBranches CurrentBranch
        {
            get { return currentBranch; }
            set { currentBranch = value; this.OnPropertyChanged("CurrentBranch"); }
        }


        private IEnumerable<z_PaymentMethod> payment;
        public IEnumerable<z_PaymentMethod> Payment
        {
            get { return payment; }
            set { payment = value; this.OnPropertyChanged("Payment"); }
        }

        private z_PaymentMethod currentPaymet;
        public z_PaymentMethod CurrentPayment
        {
            get { return currentPaymet; }
            set { currentPaymet = value; this.OnPropertyChanged("CurrentPayment"); }
        }

        private IEnumerable<z_Grade> grade;
        public IEnumerable<z_Grade> Grade
        {
            get { return grade; }
            set { grade = value; OnPropertyChanged("Grade"); }
        }
        private z_Grade currentGrade;
        public z_Grade CurrentGrade
        {
            get { return currentGrade; }
            set { currentGrade = value; this.OnPropertyChanged("CurrentGrade"); }
        }


        private IEnumerable<z_Gender> gender;
        public IEnumerable<z_Gender> Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged("Gender"); }
        }
        private z_Gender currentGender;
        public z_Gender CurrentGender
        {
            get { return currentGender; }
            set { currentGender = value; this.OnPropertyChanged("CurrentGender"); }
        }


        private IEnumerable<z_Religen> religen;
        public IEnumerable<z_Religen> Religen
        {
            get { return religen; }
            set { religen = value; OnPropertyChanged("Religen"); }
        }
        private z_Religen currentReligen;
        public z_Religen CurrentReligen
        {
            get { return currentReligen; }
            set { currentReligen = value; this.OnPropertyChanged("CurrentReligen"); }
        }

        private IEnumerable<z_CivilState> civil;
        public IEnumerable<z_CivilState> Civil
        {
            get { return civil; }
            set { civil = value; OnPropertyChanged("Civil"); }
        }

        private z_CivilState currentCivil;
        public z_CivilState CurrentCivil
        {
            get { return currentCivil; }
            set { currentCivil = value; this.OnPropertyChanged("CurrentCivil"); }
        }

        private IEnumerable<z_City> _City;
        public IEnumerable<z_City> City
        {
            get { return _City; }
            set { _City = value; OnPropertyChanged("City"); }
        }

        private z_City _CurrentCity;
        public z_City CurrentCity
        {
            get { return _CurrentCity; }
            set { _CurrentCity = value; OnPropertyChanged("CurrentCity"); }
        }

        private IEnumerable<z_Town> _Town;
        public IEnumerable<z_Town> Town
        {
            get { return _Town; }
            set { _Town = value; OnPropertyChanged("Town"); }
        }

        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get { return _CurrentTown; }
            set { _CurrentTown = value; OnPropertyChanged("CurrentTown"); }
        }

        private IEnumerable<z_blood_type> _SubDepartment;
        public IEnumerable<z_blood_type> SubDepartment
        {
            get { return _SubDepartment; }
            set { _SubDepartment = value; OnPropertyChanged("SubDepartment"); }
        }

        private z_blood_type _CurrentSubDepartment;
        public z_blood_type CurrentSubDepartment
        {
            get { return _CurrentSubDepartment; }
            set { _CurrentSubDepartment = value; OnPropertyChanged("CurrentSubDepartment"); }
        }

        #endregion

        #region Button Command

        public ICommand CleanButton
        {
            get
            {
                return new RelayCommand(Clean, cleanCanExecute);
            }
        }


        #region print

        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private void Print()
        {
            string path = "\\Reports\\Documents\\HR_Report\\Resing_Or_Working";
            ReportPrint print = new ReportPrint(path);
            print.setParameterValue("@Employee", CurrentEmployee == null ? "" : CurrentEmployee.employee_id.ToString());
            print.setParameterValue("@Religion", CurrentReligen == null ? "" : CurrentReligen.religen_id.ToString());
            print.setParameterValue("@Town", CurrentTown == null ? "" : CurrentTown.town_id.ToString());
            print.setParameterValue("@City", CurrentCity == null ? "" : CurrentCity.city_id.ToString());
            print.setParameterValue("@CivilStatus", CurrentCivil == null ? "" : CurrentCivil.civi_status_id.ToString());
            print.setParameterValue("@Gender", CurrentGender == null ? "" : CurrentGender.gender_id.ToString());
            print.setParameterValue("@Section", CurrentSection == null ? "" : CurrentSection.section_id.ToString());
            print.setParameterValue("@Department", CurrentDepatments == null ? "" : CurrentDepatments.department_id.ToString());
            print.setParameterValue("@Branch", CurrentBranch == null ? "" : CurrentBranch.companyBranch_id.ToString());
            print.setParameterValue("@Grade", CurrentGrade == null ? "" : CurrentGrade.grade_id.ToString());
            print.setParameterValue("@Designation", CurrentDesignations == null ? "" : CurrentDesignations.designation_id.ToString());
            print.setParameterValue("@Paymethod", CurrentPayment == null ? "" : CurrentPayment.paymet_method_id.ToString());
            print.setParameterValue("@Company", "");
            print.setParameterValue("@ReligionName", CurrentReligen == null ? "" : CurrentReligen.religen_name);
            print.setParameterValue("@TownName", CurrentTown == null ? "" : CurrentTown.town_name);
            print.setParameterValue("@CityName", CurrentCity == null ? "" : CurrentCity.city);
            print.setParameterValue("@CivilStatusName", CurrentCivil == null ? "" : CurrentCivil.civil_status);
            print.setParameterValue("@GenderName", CurrentGender == null ? "" : CurrentGender.gender);
            print.setParameterValue("@SectionName", CurrentSection == null ? "" : CurrentSection.section_name);
            print.setParameterValue("@DepartmentName", CurrentDepatments == null ? "" : CurrentDepatments.department_name);
            print.setParameterValue("@BranchName", CurrentBranch == null ? "" : CurrentBranch.companyBranch_Name);
            print.setParameterValue("@GradeName", CurrentGrade == null ? "" : CurrentGrade.grade);
            print.setParameterValue("@DesignationName", CurrentDesignations == null ? "" : CurrentDesignations.designation);
            print.setParameterValue("@PaymethodName", CurrentPayment == null ? "" : CurrentPayment.payment_method);
            print.setParameterValue("@CompanyName", "");
            print.setParameterValue("@logeduser", clsSecurity.loggedUser.user_name == null ? "" : clsSecurity.loggedUser.user_name);

            print.LoadToReportViewer();
        }

        #endregion


        private bool PrintCanExecute()
        {
            return true;
        }

        #endregion
    }
}
