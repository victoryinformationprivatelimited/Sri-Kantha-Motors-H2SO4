using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class EmployeeDetailsViewmodel : ViewModelBase
    {
        List<EmployeeSumarryView> TempEmployees = new List<EmployeeSumarryView>();
        #region Constructor
        ERPServiceClient serviceClient;
        public EmployeeDetailsViewmodel()
        {
            serviceClient = new ERPServiceClient();

            ResfershDesignations();
            ResfershDepartment();
            ResfershSection();
            ResfreshBranch();
            RefreshPayment();
            RefreshGrade();
            RefreshGender();
            RefreshReligen();
            RefreshCivil();
            FilterFomula = "";
            GenderButton = false;
            CivilButton = false;
            PaymentButton = false;
            RegionButton = false;
            BirthDate = System.DateTime.Now;
            ConfirmDate = System.DateTime.Now;
            JoinDate = System.DateTime.Now;
            Sdate = System.DateTime.Now;
            Edate = System.DateTime.Now;
            Clean();

        }


        #region Refresh Methods

        private void ResfershDesignations()
        {
            serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                Designations = e.Result;
            };
            serviceClient.GetDesignationsAsync();
        }

        private void ResfershDepartment()
        {
            serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                Depatments = e.Result;
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
        #endregion

        #endregion

        #region PropertiesforDesignation

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
            set { depatments = value; this.OnPropertyChanged("Departments"); }
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
            set { payment = value; this.OnPropertyChanged("Paymet"); }
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

        private IEnumerable<EmployeeSumarryView> employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return this.employees; }
            set { this.employees = value; }
        }

        private IEnumerable<dtl_Employee> isactive;
        public IEnumerable<dtl_Employee> Isactive
        {
            get { return isactive; }
            set { isactive = value; OnPropertyChanged("Isactive"); }
        }

        private dtl_Employee currentIsactive;
        public dtl_Employee CurrentIsactive
        {
            get { return currentIsactive; }
            set { currentIsactive = value; this.OnPropertyChanged("CurrentIsactive"); }
        }
        private DateTime birthDate;
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value; OnPropertyChanged("BirthDate");

            }
        }

        private int byear;
        public int BYear
        {
            get { return byear; }
            set { byear = value; OnPropertyChanged("BYear"); }
        }

        private int bmonth;
        public int BMonth
        {
            get { return bmonth; }
            set { bmonth = value; OnPropertyChanged("BMonth"); }
        }


        private DateTime confirmDate;
        public DateTime ConfirmDate
        {
            get { return confirmDate; }
            set
            { confirmDate = value; OnPropertyChanged("ConfirmDate"); }
        }

        private int cyear;
        public int CYear
        {
            get { return cyear; }
            set { cyear = value; OnPropertyChanged("CYear"); }
        }

        private int cmonth;
        public int CMonth
        {
            get { return cmonth; }
            set { cmonth = value; OnPropertyChanged("CMonth"); }
        }


        private DateTime joinDate;
        public DateTime JoinDate
        {
            get { return joinDate; }
            set
            {
                joinDate = value; OnPropertyChanged("JoinDate");
            }

        }
        private int jyear;
        public int JYear
        {
            get { return jyear; }
            set { jyear = value; OnPropertyChanged("JYear"); }
        }

        private int jmonth;
        public int JMonth
        {
            get { return jmonth; }
            set { jmonth = value; OnPropertyChanged("JMonth"); }
        }



        private DateTime resignDate;
        public DateTime ResignDate
        {
            get { return resignDate; }
            set
            {
                resignDate = value; OnPropertyChanged("ResignDate");
                if (ResignDate != null)
                {
                    RYear = JoinDate.Year;
                    RMonth = JoinDate.Month;
                }
            }
        }
        private int ryear;
        public int RYear
        {
            get { return ryear; }
            set { ryear = value; OnPropertyChanged("RYear"); }
        }

        private int rmonth;
        public int RMonth
        {
            get { return rmonth; }
            set { rmonth = value; OnPropertyChanged("RMonth"); }
        }

        private DateTime sdate;
        public DateTime Sdate
        {
            get { return sdate; }
            set { sdate = value; OnPropertyChanged("Sdate"); }
        }

        private DateTime edate;
        public DateTime Edate
        {
            get { return edate; }
            set { edate = value; OnPropertyChanged("Edate"); }
        }


        #region gender

        private bool genderButton;
        public bool GenderButton
        {
            get { return genderButton; }
            set { genderButton = value; OnPropertyChanged("GenderButton"); if (GenderButton == true) GenderEnabled = true; if (GenderButton == false) GenderEnabled = false; }
        }

        private bool genderEnabled;
        public bool GenderEnabled
        {
            get { return genderEnabled; }
            set { genderEnabled = value; OnPropertyChanged("GenderEnabled"); }
        }

        #endregion

        #region CivilButton

        private bool civilButton;
        public bool CivilButton
        {
            get { return civilButton; }
            set { civilButton = value; OnPropertyChanged("CivilButton"); if (CivilButton == true) CivilEnabled = true; if (CivilButton == false) CivilEnabled = false; }
        }

        private bool civilEnabled;
        public bool CivilEnabled
        {
            get { return civilEnabled; }
            set { civilEnabled = value; OnPropertyChanged("CivilEnabled"); }
        }
        #endregion

        #region PaymentButton

        private bool paymentButton;
        public bool PaymentButton
        {
            get { return paymentButton; }
            set { paymentButton = value; OnPropertyChanged("PaymentButton"); if (PaymentButton == true) PaymentEnabled = true; if (PaymentButton == false) PaymentEnabled = false; }
        }

        private bool paymentEnabled;
        public bool PaymentEnabled
        {
            get { return paymentEnabled; }
            set { paymentEnabled = value; OnPropertyChanged("PaymentEnabled"); }
        }
        #endregion

        #region RegionButton

        private bool regionButton;
        public bool RegionButton
        {
            get { return regionButton; }
            set { regionButton = value; OnPropertyChanged("RegionButton"); if (RegionButton == true) RegionEnabled = true; if (RegionButton == false) RegionEnabled = false; }
        }

        private bool regionEnabled;
        public bool RegionEnabled
        {
            get { return regionEnabled; }
            set { regionEnabled = value; OnPropertyChanged("RegionEnabled"); }
        }
        #endregion

        #region activation

        private bool activeButton;
        public bool ActiveButton
        {
            get { return activeButton; }
            set { activeButton = value; OnPropertyChanged("ActiveButton"); if (ActiveButton == true) ActiveEnabled = true; if (ActiveButton == false) ActiveEnabled = false; }
        }

        private bool activeEnabled;
        public bool ActiveEnabled
        {
            get { return activeEnabled; }
            set { activeEnabled = value; OnPropertyChanged("ActiveEnabled"); }
        }


        #endregion

        #region Bday

        private bool bdayButton;
        public bool BdayButton
        {
            get { return bdayButton; }
            set { bdayButton = value; OnPropertyChanged("BdayButton"); if (BdayButton == true) BdayEnabled = true; if (BdayButton == false) BdayEnabled = false; }
        }

        private bool bdayEnabled;
        public bool BdayEnabled
        {
            get { return bdayEnabled; }
            set { bdayEnabled = value; OnPropertyChanged("BdayEnabled"); }
        }


        #endregion

        #region Jday

        private bool jdayButton;
        public bool JdayButton
        {
            get { return jdayButton; }
            set { jdayButton = value; OnPropertyChanged("JdayButton"); if (JdayButton == true) JdayEnabled = true; if (JdayButton == false) JdayEnabled = false; }
        }

        private bool jdayEnabled;
        public bool JdayEnabled
        {
            get { return jdayEnabled; }
            set { jdayEnabled = value; OnPropertyChanged("JdayEnabled"); }
        }
        #endregion

        #region Cday

        private bool cdayButton;
        public bool CdayButton
        {
            get { return cdayButton; }
            set { cdayButton = value; OnPropertyChanged("CdayButton"); if (CdayButton == true) CdayEnabled = true; if (CdayButton == false) CdayEnabled = false; }
        }

        private bool cdayEnabled;
        public bool CdayEnabled
        {
            get { return cdayEnabled; }
            set { cdayEnabled = value; OnPropertyChanged("CdayEnabled"); }
        }
        #endregion

        #region Rday

        private bool rdayButton;
        public bool RdayButton
        {
            get { return rdayButton; }
            set { rdayButton = value; OnPropertyChanged("RdayButton"); if (RdayButton == true) RdayEnabled = true; if (RdayButton == false) RdayEnabled = false; }
        }

        private bool rdayEnabled;
        public bool RdayEnabled
        {
            get { return rdayEnabled; }
            set { rdayEnabled = value; OnPropertyChanged("RdayEnabled"); }
        }
        #endregion

        #region Birthday

        private bool birthButton;
        public bool BirthButton
        {
            get { return birthButton; }
            set { birthButton = value; OnPropertyChanged("BirthButton"); if (BirthButton == true) BirthEnabled = true; if (BirthButton == false) BirthEnabled = false; }
        }

        private bool birthEnabled;
        public bool BirthEnabled
        {
            get { return birthEnabled; }
            set { birthEnabled = value; OnPropertyChanged("BirthEnabled"); }
        }

        #endregion

        #region ConfirmDate

        private bool confirmButton;
        public bool ConfirmButton
        {
            get { return confirmButton; }
            set { confirmButton = value; OnPropertyChanged("ConfirmButton"); if (ConfirmButton == true) ConfirmEnabled = true; if (ConfirmButton == false) ConfirmEnabled = false; }
        }

        private bool confirmEnabled;
        public bool ConfirmEnabled
        {
            get { return confirmEnabled; }
            set { confirmEnabled = value; OnPropertyChanged("ConfirmEnabled"); }
        }

        #endregion

        #region JoinDate

        private bool joinButton;
        public bool JoinButton
        {
            get { return joinButton; }
            set { joinButton = value; OnPropertyChanged("JoinButton"); if (JoinButton == true) JoinEnabled = true; if (JoinButton == false) JoinEnabled = false; }
        }

        private bool joinEnabled;
        public bool JoinEnabled
        {
            get { return joinEnabled; }
            set { joinEnabled = value; OnPropertyChanged("JoinEnabled"); }
        }

        #endregion

        #region BrangeDate

        private bool brangeButton;
        public bool BrangeButton
        {
            get { return brangeButton; }
            set { brangeButton = value; OnPropertyChanged("BrangeButton"); if (BrangeButton == true) BrangeEnabled = true; if (BrangeButton == false) BrangeEnabled = false; }
        }

        private bool brangeEnabled;
        public bool BrangeEnabled
        {
            get { return brangeEnabled; }
            set { brangeEnabled = value; OnPropertyChanged("BrangeEnabled"); }
        }

        #endregion

        #region JrangeDate

        private bool jrangeButton;
        public bool JrangeButton
        {
            get { return jrangeButton; }
            set { jrangeButton = value; OnPropertyChanged("JrangeButton"); if (JrangeButton == true) JrangeEnabled = true; if (JrangeButton == false) JrangeEnabled = false; }
        }

        private bool jrangeEnabled;
        public bool JrangeEnabled
        {
            get { return jrangeEnabled; }
            set { jrangeEnabled = value; OnPropertyChanged("JrangeEnabled"); }
        }

        #endregion

        #region CrangeDate

        private bool crangeButton;
        public bool CrangeButton
        {
            get { return crangeButton; }
            set { crangeButton = value; OnPropertyChanged("CrangeButton"); if (CrangeButton == true) CrangeEnabled = true; if (CrangeButton == false) CrangeEnabled = false; }
        }

        private bool crangeEnabled;
        public bool CrangeEnabled
        {
            get { return crangeEnabled; }
            set { crangeEnabled = value; OnPropertyChanged("CrangeEnabled"); }
        }

        #endregion

        private string _FilterFomula;
        public string FilterFomula
        {
            get { return _FilterFomula; }
            set { _FilterFomula = value; this.OnPropertyChanged("FilterFomula"); }
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

        #region Clean Method
        private void Clean()
        {
            //this.CurrentBranch = null;
            //CurrentBranch = new EmployeeDetailsView();
            //CurrentBranch.companyBranch_id = Guid.NewGuid();
            //ResfreshBranch();
        }

        bool cleanCanExecute()
        {
            return true;
        }
        #endregion

        #region print
        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private void Print()
        {


            if (GenderButton == true && CurrentGender != null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsBygender";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@gender", CurrentGender.gender);
                print.LoadToReportViewer();
            }

            if (CivilButton == true && CurrentCivil != null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsBycivilstate";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@civil", CurrentCivil.civil_status);
                print.LoadToReportViewer();
            }

            if (RegionButton == true && CurrentReligen != null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsByreligion";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@religion", CurrentReligen.religen_name);
                print.LoadToReportViewer();
            }

            if (PaymentButton == true && CurrentPayment != null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsBypaymentmethod";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@method", CurrentPayment.payment_method);
                print.LoadToReportViewer();
            }

            if (CurrentBranch != null && CurrentCivil == null && CurrentGender == null && CurrentReligen == null && CurrentPayment == null && CurrentIsactive == null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\Employeedetailsbybranchname";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@BranchName", CurrentBranch.companyBranch_Name);
                print.LoadToReportViewer();
            }

            if (CurrentSection != null && CurrentCivil == null && CurrentGender == null && CurrentReligen == null && CurrentPayment == null && CurrentIsactive == null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\Employeedetailsbysection";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@section", CurrentSection.section_name);
                print.LoadToReportViewer();
            }

            if (CurrentDepatments != null && CurrentCivil == null && CurrentGender == null && CurrentReligen == null && CurrentPayment == null && CurrentIsactive == null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsbyDepartment";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@Department", CurrentDepatments.department_name);
                print.LoadToReportViewer();
            }

            if (CurrentDesignations != null && CurrentCivil == null && CurrentGender == null && CurrentReligen == null && CurrentPayment == null && CurrentIsactive == null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsbyDesignation";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@designation", CurrentDesignations.designation);
                print.LoadToReportViewer();
            }

            if (currentGrade != null && CurrentCivil == null && CurrentGender == null && CurrentReligen == null && CurrentPayment == null && CurrentIsactive == null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\employeedetailsbyGrade";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@grade", CurrentGrade.grade);
                print.LoadToReportViewer();
            }

            if (ActiveButton == true && CurrentIsactive != null)
            {
                string path = "\\Reports\\Documents\\HR_Report\\employeedetailsbyActivate";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@isactive", CurrentIsactive.isActive);
                print.LoadToReportViewer();
            }

            if (BYear != 0 && BdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsBybirthdayYear";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@year", BYear);
                print.LoadToReportViewer();
            }

            if (BMonth != 0 && BdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeedetailsBybirthdaymonth";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@month", BMonth);
                print.LoadToReportViewer();
            }

            if (BirthButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByBirthDay";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@day_YYYY_MM_DD", BirthDate);
                print.LoadToReportViewer();

            }

            if (CYear != 0 && CdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\employeeDetailsByYearofConfiermDay";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@year", CYear);
                print.LoadToReportViewer();
            }
            if (CMonth != 0 && CdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByConfirmDayMonth";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@month", CMonth);
                print.LoadToReportViewer();
            }


            if (ConfirmButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByConfirmDayDate";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@confirm", BirthDate);
                print.LoadToReportViewer();
            }




            if (JoinButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByJoinedDayDate";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@joindate", JoinDate);
                print.LoadToReportViewer();
            }


            if (JYear != 0 && JdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\employeeDetailsByYearOfJoinDay";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@year", JYear);
                print.LoadToReportViewer();
            }
            if (JMonth != 0 && JdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsBYJoinDayMonth";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@month", JMonth);
                print.LoadToReportViewer();
            }

            if (BrangeButton == true && BdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByBirthDayRange";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@startdate_yyyy_mm_dd", Sdate.Date);
                print.setParameterValue("@enddate_yyyy_mm_dd", Edate.Date);
                print.LoadToReportViewer();
            }

            if (CrangeButton == true && CdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByConfirmedDayRange";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@startdate", Sdate.Date);
                print.setParameterValue("@enddate", Edate.Date);
                print.LoadToReportViewer();
            }

            if (JrangeButton == true && JdayButton == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsByJoinedDayRange";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@startdate", Sdate.Date);
                print.setParameterValue("@enddate", Edate.Date);
                print.LoadToReportViewer();
            }



            #region resign

            //if (RdayButton == true && RoinDate != null)
            //{
            //    if (RYear != null)
            //    {
            //        string path = "\\Reports\\Documents\\HR_Report\\employeeDetailsByYearOfJoinDay";
            //        ReportPrint print = new ReportPrint(path);
            //        print.setParameterValue("@year", RYear);
            //        print.LoadToReportViewer();
            //    }
            //    if (RMonth != null)
            //    {
            //        string path = "\\Reports\\Documents\\HR_Report\\EmployeeDetailsBYJoinDayMonth";
            //        ReportPrint print = new ReportPrint(path);
            //        print.setParameterValue("@month", RMonth);
            //        print.LoadToReportViewer();
            //    }
            //}

            #endregion
        }
        #endregion


        private bool PrintCanExecute()
        {
            if (CurrentBranch != null || CurrentSection != null || CurrentDepatments != null || CurrentDesignations != null || CurrentGender != null || CurrentCivil != null || CurrentReligen != null || CurrentPayment != null || CurrentIsactive != null || BdayButton == true || JdayButton == true || CdayButton == true || CurrentGrade != null)
            {
                return true;

            }
            return false;
        }

    }
}

        #endregion
