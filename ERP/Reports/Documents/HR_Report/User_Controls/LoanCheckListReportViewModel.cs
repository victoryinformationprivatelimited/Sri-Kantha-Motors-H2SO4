using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class LoanCheckListReportViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public LoanCheckListReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployees();
            LoanAmount = 0;
            LoanInstallments = 0;
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployees;
        public EmployeeSumarryView CurrentEmployees
        {
            get { return _CurrentEmployees; }
            set { _CurrentEmployees = value; OnPropertyChanged("CurrentEmployees"); }
        }

        private EmployeeSearchView _CurrentEmployeesForDialogBox;
        public EmployeeSearchView CurrentEmployeesForDialogBox
        {
            get { return _CurrentEmployeesForDialogBox; }
            set { _CurrentEmployeesForDialogBox = value; OnPropertyChanged("CurrentEmployeesForDialogBox"); if (CurrentEmployeesForDialogBox != null)SetEmployeeDetails(); }
        }

        private IEnumerable<EmployeeSumarryView> _Guarantor;
        public IEnumerable<EmployeeSumarryView> Guarantor
        {
            get { return _Guarantor; }
            set { _Guarantor = value; OnPropertyChanged("Guarantor"); }
        }

        private EmployeeSumarryView _CurrentGuarantor1;
        public EmployeeSumarryView CurrentGuarantor1
        {
            get { return _CurrentGuarantor1; }
            set { _CurrentGuarantor1 = value; OnPropertyChanged("CurrentGuarantor1"); }
        }

        private EmployeeSumarryView _CurrentGuarantor2;
        public EmployeeSumarryView CurrentGuarantor2
        {
            get { return _CurrentGuarantor2; }
            set { _CurrentGuarantor2 = value; OnPropertyChanged("CurrentGuarantor2"); }
        }

        private int _LoanAmount;

        public int LoanAmount
        {
            get { return _LoanAmount; }
            set { _LoanAmount = value; OnPropertyChanged("LoanAmount"); }
        }

        private int _LoanInstallments;

        public int LoanInstallments
        {
            get { return _LoanInstallments; }
            set { _LoanInstallments = value; OnPropertyChanged("LoanInstallments");}
        }
        
        #endregion

        #region Refresh Methods
        private void RefreshEmployees()
        {
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result;
                Guarantor = e.Result.Where(c => c.isActive == true && c.isdelete == false).OrderBy(c => c.emp_id);
            };
            serviceClient.GetAllEmployeeDetailAsync();
        }

        #endregion

        #region Button Commands
        public ICommand Print
        {
            get { return new RelayCommand(ViewReport, ViewReportCE); }
        }
        public ICommand ButtonSearchEmployee
        {
            get { return new RelayCommand(searchEmp); }
        }

        #endregion

        #region Methods

        #region SearchEmp Open
        void searchEmp()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentEmployeesForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        #endregion
        private void SetEmployeeDetails()
        {
            CurrentEmployees = Employees.FirstOrDefault(c => c.employee_id == CurrentEmployeesForDialogBox.employee_id);
        }

        private bool ViewReportCE()
        {
            if (CurrentEmployees != null && CurrentGuarantor1 != null && CurrentGuarantor2 != null)
                return true;
            else
                return false;
        }

        private void ViewReport()
        {
            string path = "";
            try
            {
                string Guarantor1EPF = "";
                string Guarantor1EPFName = "";
                DateTime Guarantor1DOB = new DateTime();
                DateTime Guarantor1ResignDate = new DateTime();
                string Guarantor1Designation = "";
                string Guarantor1Grade = "";
                string Guarantor1Department = "";
                string Guarantor1Section = "";
                DateTime Guarantor1JoinDate = new DateTime();
                DateTime Guarantor1ConfirmationDate = new DateTime();
                string Guarantor1SalaryCode = "";
                string Guarantor1SalaryScale = "";
                decimal Guarantor1BasicSalary = 0;

                string Guarantor2EPF = "";
                string Guarantor2EPFName = "";
                DateTime Guarantor2DOB = new DateTime();
                DateTime Guarantor2ResignDate = new DateTime();
                string Guarantor2Designation = "";
                string Guarantor2Grade = "";
                string Guarantor2Department = "";
                string Guarantor2Section = "";
                DateTime Guarantor2JoinDate = new DateTime();
                DateTime Guarantor2ConfirmationDate = new DateTime();
                string Guarantor2SalaryCode = "";
                string Guarantor2SalaryScale = "";
                decimal Guarantor2BasicSalary = 0;

                if (CurrentGuarantor1 != null)
                {
                    Guarantor1EPF = CurrentGuarantor1.epf_no;
                    Guarantor1EPFName = serviceClient.GetEmployeeEPFName(CurrentGuarantor1.employee_id);
                    Guarantor1DOB = (CurrentGuarantor1.birthday != null) ? (DateTime)CurrentGuarantor1.birthday : new DateTime(1800, 01, 01);
                    Guarantor1ResignDate = (CurrentGuarantor1.resign_date != null) ? (DateTime)CurrentGuarantor1.resign_date : new DateTime(1800,01,01);
                    Guarantor1Designation = CurrentGuarantor1.designation;
                    Guarantor1Grade = CurrentGuarantor1.grade;
                    Guarantor1Department = CurrentGuarantor1.department_name;
                    Guarantor1Section = CurrentGuarantor1.section_name;
                    Guarantor1JoinDate = (CurrentGuarantor1.join_date != null) ? (DateTime)CurrentGuarantor1.join_date : new DateTime(1800, 01, 01);
                    Guarantor1ConfirmationDate = (CurrentGuarantor1.prmernant_active_date != null) ? (DateTime)CurrentGuarantor1.prmernant_active_date : new DateTime(1800, 01, 01);
                    Guarantor1SalaryCode = (CurrentGuarantor1.salary_code != null) ? CurrentGuarantor1.salary_code : null;
                    Guarantor1SalaryScale = (CurrentGuarantor1.salary_scale != null) ? CurrentGuarantor1.salary_scale  : null;
                    Guarantor1BasicSalary = (decimal)CurrentGuarantor1.basic_salary;

                    Guarantor2EPF = CurrentGuarantor2.epf_no;
                    Guarantor2EPFName = serviceClient.GetEmployeeEPFName(CurrentGuarantor2.employee_id);
                    Guarantor2DOB = (CurrentGuarantor2.birthday != null) ? (DateTime)CurrentGuarantor2.birthday : new DateTime(1800, 01, 01);
                    Guarantor2ResignDate = (CurrentGuarantor2.resign_date != null) ? (DateTime)CurrentGuarantor2.resign_date : new DateTime(1800,01,01);
                    Guarantor2Designation = CurrentGuarantor2.designation;
                    Guarantor2Grade = CurrentGuarantor2.grade;
                    Guarantor2Department = CurrentGuarantor2.department_name;
                    Guarantor2Section = CurrentGuarantor2.section_name;
                    Guarantor2JoinDate = (CurrentGuarantor2.join_date != null) ? (DateTime)CurrentGuarantor2.join_date : new DateTime(1800, 01, 01);
                    Guarantor2ConfirmationDate = (CurrentGuarantor2.prmernant_active_date != null) ? (DateTime)CurrentGuarantor2.prmernant_active_date : new DateTime(1800, 01, 01);
                    Guarantor2SalaryCode = (CurrentGuarantor2.salary_code != null) ? CurrentGuarantor2.salary_code : null;
                    Guarantor2SalaryScale = (CurrentGuarantor2.salary_scale != null) ? CurrentGuarantor2.salary_scale : null;
                    Guarantor2BasicSalary = (decimal)CurrentGuarantor2.basic_salary;
                }
                
                path = "\\Reports\\Documents\\HR_Report\\loan_list";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@EmployeeID", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                print.setParameterValue("@guarantorEPF01", Guarantor1EPF);
                print.setParameterValue("@guarantorname01", Guarantor1EPFName);
                print.setParameterValue("@guarantorDateofBirth1", Guarantor1DOB);
                print.setParameterValue("@guarantorResignDate1", Guarantor1ResignDate);
                print.setParameterValue("@guarantorDesignation1", Guarantor1Designation);
                print.setParameterValue("@guarantorGrade1", Guarantor1Grade);
                print.setParameterValue("@guarantorDepartment1", Guarantor1Department);
                print.setParameterValue("@guarantorSection1", Guarantor1Section);
                print.setParameterValue("@guarantorJoinDate1", Guarantor1JoinDate);
                print.setParameterValue("@guarantorConfirmationDate1", Guarantor1ConfirmationDate);
                print.setParameterValue("@guarantorSalaryCode1", Guarantor1SalaryCode);
                print.setParameterValue("@guarantorSalaryScale1", Guarantor1SalaryScale);
                print.setParameterValue("@guarantorBasicSalary1", Guarantor1BasicSalary);
                print.setParameterValue("@guarantorEPF02", Guarantor2EPF);
                print.setParameterValue("@guarantorname02", Guarantor2EPFName);
                print.setParameterValue("@guarantorDateofBirth2", Guarantor2DOB);
                print.setParameterValue("@guarantorResignDate2", Guarantor2ResignDate);
                print.setParameterValue("@guarantorDesignation2", Guarantor2Designation);
                print.setParameterValue("@guarantorGrade2", Guarantor2Grade);
                print.setParameterValue("@guarantorDepartment2", Guarantor2Department);
                print.setParameterValue("@guarantorSection2", Guarantor2Section);
                print.setParameterValue("@guarantorJoinDate2", Guarantor2JoinDate);
                print.setParameterValue("@guarantorConfirmationDate2", Guarantor2ConfirmationDate);
                print.setParameterValue("@guarantorSalaryCode2", Guarantor2SalaryCode);
                print.setParameterValue("@guarantorSalaryScale2", Guarantor2SalaryScale);
                print.setParameterValue("@guarantorBasicSalary2", Guarantor2BasicSalary);
                print.setParameterValue("@loanAmount", LoanAmount);
                print.setParameterValue("@loanInstallments", LoanInstallments);
                print.LoadToReportViewer();
            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report loading is failed: " + ex.Message);
            }
        }

        #endregion
    }
}
