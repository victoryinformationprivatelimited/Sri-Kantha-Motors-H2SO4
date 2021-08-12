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
    class SalaryIncrementLetterViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public SalaryIncrementLetterViewModel()
        {
            serviceClient = new ERPServiceClient();
            CurrentDate = DateTime.Now.Date;
            IncrementValue = (decimal)0.00;
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

        private IEnumerable<EmployeeSumarryView> _Supervisors;
        public IEnumerable<EmployeeSumarryView> Supervisors
        {
            get { return _Supervisors; }
            set { _Supervisors = value; OnPropertyChanged("Supervisors"); }
        }

        private EmployeeSumarryView _CurrentSupervisor;
        public EmployeeSumarryView CurrentSupervisor
        {
            get { return _CurrentSupervisor; }
            set { _CurrentSupervisor = value; OnPropertyChanged("CurrentSupervisor"); }
        }

        private decimal _IncrementValue;

        public decimal IncrementValue
        {
            get { return _IncrementValue; }
            set { _IncrementValue = value; OnPropertyChanged("IncrementValue"); }
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
                Supervisors = e.Result.OrderBy(c => c.emp_id);
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
            if (CurrentEmployees != null && CurrentSupervisor != null && IncrementValue != 0)
                return true;
            else
                return false;
        }

        private void ViewReport()
        {
            string path = "";
            try
            {
                string DMDesignation = CurrentSupervisor.designation;
                decimal BasicSalary = 0;
                if (CurrentEmployees.basic_salary != null && IncrementValue != 0)
                {
                    BasicSalary = (decimal)CurrentEmployees.basic_salary + IncrementValue; 
                }
                decimal col = serviceClient.GetEmployeeCOLAmount(CurrentEmployees.employee_id);
                decimal gross = BasicSalary + col;
                path = "\\Reports\\Documents\\HR_Report\\SalaryIN";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@HeadOfDepartment", DMDesignation);
                print.setParameterValue("@SalaryIncrementDate", CurrentDate);
                print.setParameterValue("@ValueoftheIncrement", IncrementValue);
                print.setParameterValue("@BasicSalarywithIncrement", BasicSalary);
                print.setParameterValue("@CostoflivingAllowance", col);
                print.setParameterValue("@GrossSalaryasat", gross);
                print.setParameterValue("@employeeid", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
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
