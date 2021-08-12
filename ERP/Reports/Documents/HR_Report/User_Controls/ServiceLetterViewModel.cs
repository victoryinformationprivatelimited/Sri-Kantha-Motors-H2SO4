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
    class ServiceLetterViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public ServiceLetterViewModel()
        {
            serviceClient = new ERPServiceClient();
            CurrentDate = DateTime.Now.Date;
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

        private string _AddresserPositionName;

        public string AddresserPositionName
        {
            get { return _AddresserPositionName; }
            set { _AddresserPositionName = value; OnPropertyChanged("AddresserPositionName"); }
        }

        private string _AddresserAddress;

        public string AddresserAddress
        {
            get { return _AddresserAddress; }
            set { _AddresserAddress = value; OnPropertyChanged("AddresserAddress"); }
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
            if (CurrentEmployees != null && AddresserPositionName != null && AddresserAddress != null)
                return true;
            else
                return false;
        }

        private void ViewReport()
        {
            string path = "";
            try
            {

                path = "\\Reports\\Documents\\HR_Report\\Service_letter";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@employeeid", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                print.setParameterValue("@CurrentDate", CurrentDate);
                print.setParameterValue("@AddressName", AddresserPositionName);
                print.setParameterValue("@Address", AddresserAddress);
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
