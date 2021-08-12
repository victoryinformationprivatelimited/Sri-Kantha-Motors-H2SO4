using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.New_LoanForms
{
    class InternalLoanApprovalViewModel : ViewModelBase
    {
        #region Fields

        private ERPServiceClient serviceClient;
        List<EmployeeInternalLoanWithSupervisorsView> AllSupervisorsForLoans;


        #endregion

        #region Constructor

        public InternalLoanApprovalViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllSupervisorsForLoans = new List<EmployeeInternalLoanWithSupervisorsView>();
            New();
        }

        #endregion

        #region Properties

        private IEnumerable<InternalLoanMainView> _FilteredLoanWiseSupervisor;
        public IEnumerable<InternalLoanMainView> FilteredLoanWiseSupervisor
        {
            get { return _FilteredLoanWiseSupervisor; }
            set { _FilteredLoanWiseSupervisor = value; OnPropertyChanged("FilteredLoanWiseSupervisor"); }
        }

        private InternalLoanMainView _CurrentFilteredLoanWiseSupervisor;
        public InternalLoanMainView CurrentFilteredLoanWiseSupervisor
        {
            get { return _CurrentFilteredLoanWiseSupervisor; }
            set { _CurrentFilteredLoanWiseSupervisor = value; OnPropertyChanged("CurrentFilteredLoanWiseSupervisor"); if (CurrentFilteredLoanWiseSupervisor != null)FilterSupervisorsByLoan();}
        }

        private IEnumerable<EmployeeInternalLoanWithSupervisorsView> _SupervisorsForLoans;
        public IEnumerable<EmployeeInternalLoanWithSupervisorsView> SupervisorsForLoans
        {
            get { return _SupervisorsForLoans; }
            set { _SupervisorsForLoans = value; OnPropertyChanged("SupervisorsForLoans"); }
        }

        private EmployeeInternalLoanWithSupervisorsView _CurrentSupervisorsForLoans;
        public EmployeeInternalLoanWithSupervisorsView CurrentSupervisorsForLoans
        {
            get { return _CurrentSupervisorsForLoans; }
            set { _CurrentSupervisorsForLoans = value; OnPropertyChanged("CurrentSupervisorsForLoans"); }
        }

        private IEnumerable<usr_UserEmployee> _Users;
        public IEnumerable<usr_UserEmployee> Users
        {
            get { return _Users; }
            set { _Users = value; OnPropertyChanged("Users"); }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterLoans(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        #endregion

        #region Refresh Methods
        private void RefreshLoansFilteredWiseSupervisor()
        {
            serviceClient.GetEmpLoanDetailsOfloggedSupervisorCompleted += (s, e) =>
                {
                    try
                    {
                        FilteredLoanWiseSupervisor = e.Result;
                    }
                    catch (Exception)
                    {

                        FilteredLoanWiseSupervisor = null;
                    }
                };
            serviceClient.GetEmpLoanDetailsOfloggedSupervisorAsync(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id);
        }
        private void RefreshSupervisorsForLoans()
        {
            serviceClient.GetEmployeeInternalLoanWithSupervisorsViewCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.Count() > 0)
                        AllSupervisorsForLoans = e.Result.ToList();

                };
            serviceClient.GetEmployeeInternalLoanWithSupervisorsViewAsync();
        }
        private void RefreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                Users = e.Result;
            };
            serviceClient.GetUserEmployeesAsync();
        }

        #endregion

        #region Button Commands

        public ICommand BTNApprove
        {
            get { return new RelayCommand(Approve); }
        }
        public ICommand BTNReject
        {
            get { return new RelayCommand(Reject); }
        }
        public ICommand BTNTransfer
        {
            get { return new RelayCommand(Transfer); }
        }


        #endregion

        #region Methods
        private void Approve()
        {
            if (clsSecurity.GetSavePermission(604))
            {
                if (ValidateApproval())
                {
                    clsMessages.setMessage("Are You Sure You Want To Approve Internal Loan?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        Guid? CurrentUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
                        EmployeeInternalLoanWithSupervisorsView temp = new EmployeeInternalLoanWithSupervisorsView();
                        temp.Is_Approved = true;
                        temp.Is_Pending = false;
                        temp.Is_Rejected = false;
                        temp.InternalLoanID = CurrentFilteredLoanWiseSupervisor.InternalLoanID;
                        temp.ApprovedorRejected_UserID = clsSecurity.loggedUser.user_id;
                        temp.Supervisor_EmployeeID = CurrentUserEmployee;
                        temp.Comments = CurrentSupervisorsForLoans.Comments;
                        if (serviceClient.PenddingLoanApprove(temp))
                        {
                            New();
                            clsMessages.setMessage("Internal Loan Has Been Approved By the Supervisor...");
                        }
                        else
                            clsMessages.setMessage("Internal Loan Approval Process Has Been Failed...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission approve");
        }
        private bool ValidateApproval()
        {
            if (CurrentFilteredLoanWiseSupervisor == null)
            {
                clsMessages.setMessage("Please Select A Loan to Process The Task...");
                return false;
            }
            else if (CurrentFilteredLoanWiseSupervisor.InternalLoanID == null || CurrentFilteredLoanWiseSupervisor.InternalLoanID == null)
            {
                clsMessages.setMessage("Please Select A Loan to Approve...");
                return false;
            }
            else if (CurrentSupervisorsForLoans.Supervisor_EmployeeID != Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id)
            {
                clsMessages.setMessage("Please Select Your Name Under Supervisor's Details to Process The Task...");
                return false;
            }
            else
                return true;
        }
        private void Transfer()
        {
            if (clsSecurity.GetSavePermission(604))
            {
                if (ValidateApproval())
                {
                    clsMessages.setMessage("Are You Sure You Want To Transfer Internal Loan?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        Guid? CurrentUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
                        EmployeeInternalLoanWithSupervisorsView temp = new EmployeeInternalLoanWithSupervisorsView();
                        temp.Is_Approved = false;
                        temp.Is_Pending = true;
                        temp.Is_Rejected = false;
                        temp.Is_Active = false;
                        temp.InternalLoanID = CurrentFilteredLoanWiseSupervisor.InternalLoanID;
                        temp.Supervisor_EmployeeID = CurrentUserEmployee;
                        temp.Comments = CurrentSupervisorsForLoans.Comments;

                        if (serviceClient.PendingLoanTransfer(temp))
                        {
                            New();
                            clsMessages.setMessage("Internal Loan Has Been Transfered By the Supervisor...");
                        }
                        else
                            clsMessages.setMessage("Internal Loan Transfer Process Has Been Failed...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to transfer");
        }
        private void Reject()
        {
            if (clsSecurity.GetDeletePermission(604))
            {
                if (ValidateApproval())
                {
                    clsMessages.setMessage("Are You Sure You Want To Cancel Internal Loan?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        Guid? CurrentUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
                        EmployeeInternalLoanWithSupervisorsView temp = new EmployeeInternalLoanWithSupervisorsView();
                        temp.Is_Approved = false;
                        temp.Is_Pending = false;
                        temp.Is_Rejected = true;
                        temp.InternalLoanID = CurrentFilteredLoanWiseSupervisor.InternalLoanID;
                        temp.ApprovedorRejected_UserID = clsSecurity.loggedUser.user_id;
                        temp.Supervisor_EmployeeID = CurrentUserEmployee;
                        temp.Comments = CurrentSupervisorsForLoans.Comments;
                        if (serviceClient.PenddingLoanReject(temp))
                        {
                            New();
                            clsMessages.setMessage("Internal Loan Has Been Rejected By the Supervisor...");
                        }
                        else
                            clsMessages.setMessage("Internal Loan Reject Process Has Been Failed...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have pemission to reject");
        }
        private void FilterSupervisorsByLoan()
        {
            SupervisorsForLoans = null;
            SupervisorsForLoans = AllSupervisorsForLoans.Where(c => c.InternalLoanID == CurrentFilteredLoanWiseSupervisor.InternalLoanID).OrderBy(c => c.supervisor_level);
        }
        private void New()
        {
            CurrentSupervisorsForLoans = new EmployeeInternalLoanWithSupervisorsView();
            SupervisorsForLoans = null;
            RefreshLoansFilteredWiseSupervisor();
            RefreshSupervisorsForLoans();
            RefreshUserEmployee();
        }
        private void FilterLoans()
        {
            if (SearchIndex == 0)
            {
                FilteredLoanWiseSupervisor = FilteredLoanWiseSupervisor.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
            }
            if (SearchIndex == 1)
            {
                FilteredLoanWiseSupervisor = FilteredLoanWiseSupervisor.Where(c => c.surname != null && c.surname.ToUpper().Contains(Search.ToUpper()));
            }
            if (SearchIndex == 2)
            {
                FilteredLoanWiseSupervisor = FilteredLoanWiseSupervisor.Where(c => c.loan_name != null && c.loan_name.ToUpper().Contains(Search.ToUpper()));
            }
        }

        #endregion
    }
}
