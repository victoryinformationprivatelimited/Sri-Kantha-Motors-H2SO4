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
    class LoanBondReportViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public LoanBondReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            CurrentDate = DateTime.Now.Date;
            LoanNo = 0;
            LoanAmount = (decimal)0.00;
            LoanInstallments = 0;
            RefreshEmployees();
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

        private decimal _LoanNo;

        public decimal LoanNo
        {
            get { return _LoanNo; }
            set { _LoanNo = value; OnPropertyChanged("LoanNo"); }
        }

        private decimal _LoanAmount;

        public decimal LoanAmount
        {
            get { return _LoanAmount; }
            set { _LoanAmount = value; OnPropertyChanged("LoanAmount"); }
        }

        private decimal _LoanInstallments;

        public decimal LoanInstallments
        {
            get { return _LoanInstallments; }
            set { _LoanInstallments = value; OnPropertyChanged("LoanInstallments"); }
        }

        private DateTime _CurrentDate;

        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; OnPropertyChanged("CurrentDate"); }
        }
        
        #endregion

        #region Refresh Methods
        private void RefreshEmployees()
        {
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result;
                Guarantor = e.Result.OrderBy(c => c.emp_id);
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
            if (CurrentEmployees != null && CurrentGuarantor1 != null && CurrentGuarantor2 != null && LoanNo != 0 && LoanAmount != 0 && LoanInstallments != 0)
                return true;
            else
                return false;
        }

        private void ViewReport()
        {
            string path = "";
            try
            {
                string Guarantor1Addr1 = "";
                string Guarantor1Addr2 = "";
                string Guarantor1Addr3 = "";
                string Guarantor1Address = "";

                string Guarantor1Surname = "";
                string Guarantor1FirstName = "";
                string Guarantor1SecondName = "";
                string Guarantor1Name = "";
                
                string Guarantor2Addr1 = "";
                string Guarantor2Addr2 = "";
                string Guarantor2Addr3 = "";
                string Guarantor2Address = "";

                string Guarantor2Surname = "";
                string Guarantor2FirstName = "";
                string Guarantor2SecondName = "";
                string Guarantor2Name = "";

                if(CurrentGuarantor1.address_01 != null)
                {
                    Guarantor1Addr1 = CurrentGuarantor1.address_01;
                }
                if (CurrentGuarantor1.address_02 != null)
                {
                    Guarantor1Addr2 = CurrentGuarantor1.address_02;
                }
                if (CurrentGuarantor1.address_03 != null)
                {
                    Guarantor1Addr3 = CurrentGuarantor1.address_03;
                }

                if (CurrentGuarantor1.surname != null)
                {
                    Guarantor1Surname = CurrentGuarantor1.surname;
                }
                if (CurrentGuarantor1.first_name != null)
                {
                    Guarantor1FirstName = CurrentGuarantor1.first_name;
                }
                if (CurrentGuarantor1.second_name != null)
                {
                    Guarantor1SecondName = CurrentGuarantor1.second_name;
                }

                if (CurrentGuarantor2.address_01 != null)
                {
                    Guarantor2Addr1 = CurrentGuarantor2.address_01;
                }
                if (CurrentGuarantor2.address_02 != null)
                {
                    Guarantor2Addr2 = CurrentGuarantor2.address_02;
                }
                if (CurrentGuarantor2.address_03 != null)
                {
                    Guarantor2Addr3 = CurrentGuarantor2.address_03;
                }

                if (CurrentGuarantor2.surname != null)
                {
                    Guarantor2Surname = CurrentGuarantor2.surname;
                }
                if (CurrentGuarantor2.first_name != null)
                {
                    Guarantor2FirstName = CurrentGuarantor2.first_name;
                }
                if (CurrentGuarantor2.second_name != null)
                {
                    Guarantor2SecondName = CurrentGuarantor2.second_name;
                }

                Guarantor1Address = Guarantor1Addr1 + " " + Guarantor1Addr2 + " " + Guarantor1Addr3;
                Guarantor1Name = Guarantor1Surname + " " + Guarantor1FirstName + " " + Guarantor1SecondName;
                Guarantor2Address = Guarantor2Addr1 + " " + Guarantor2Addr2 + " " + Guarantor2Addr3;
                Guarantor2Name = Guarantor2Surname + " " + Guarantor2FirstName + " " + Guarantor2SecondName;
                
                path = "\\Reports\\Documents\\HR_Report\\Loan_Bond";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@EmployeeID", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                print.setParameterValue("@guarantorEPF01", CurrentGuarantor1.emp_id);
                print.setParameterValue("@guarantorname01", Guarantor1Name);
                print.setParameterValue("@guarantorAddress01", Guarantor1Address);
                print.setParameterValue("@guarantorEPF02", CurrentGuarantor2.emp_id);
                print.setParameterValue("@guarantorname02", Guarantor2Name);
                print.setParameterValue("@guarantorAddress02", Guarantor2Address);
                print.setParameterValue("@LoanNo", LoanNo);
                print.setParameterValue("@LoanAmount", LoanAmount);
                print.setParameterValue("@installments", LoanInstallments);
                print.setParameterValue("@approveDate", CurrentDate);
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
