using ERP.BasicSearch;
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
    class EmployeeBankLoanViewModel : ViewModelBase
    {
        #region Fields

        private ERPServiceClient serviceClient;
        List<EmployeeSumarryView> AllEmployees = new List<EmployeeSumarryView>();
        List<z_BankBranch> AllBankBranch = new List<z_BankBranch>();
        List<z_Bank> AllBank = new List<z_Bank>();

        #endregion

        #region Constructor
        public EmployeeBankLoanViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployees();
            RerfreshBank();
            RefreshBankBranch();
            RefreshExternalBankLoan();
            RefreshAllExternalBankLoan();
            RefreshUserEmployee();
            RefreshSupervisors();
            CurrentEmployee = new EmployeeSumarryView();
            CurrentExternalBankLoan = new ExternalBankLoanView();
            CurrentBankBranch = new z_BankBranch();
            CurrentBank = new z_Bank();
            CurrentExtBankLoan = new mas_ExtBankLoan();
            isBank = true;
            isCheque = false;
            isHold = false;
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private EmployeeSearchView _CurrentEmployeesForDialogBox;
        public EmployeeSearchView CurrentEmployeesForDialogBox
        {
            get { return _CurrentEmployeesForDialogBox; }
            set { _CurrentEmployeesForDialogBox = value; OnPropertyChanged("CurrentEmployeesForDialogBox"); if (CurrentEmployeesForDialogBox != null)SetEmployeeDetails(); }
        }

        private IEnumerable<ExternalBankLoanView> _ExternalBankLoan;
        public IEnumerable<ExternalBankLoanView> ExternalBankLoan
        {
            get { return _ExternalBankLoan; }
            set { _ExternalBankLoan = value; OnPropertyChanged("ExternalBankLoan"); }
        }

        private ExternalBankLoanView _CurrentExternalBankLoan;
        public ExternalBankLoanView CurrentExternalBankLoan
        {
            get { return _CurrentExternalBankLoan; }
            set
            {
                _CurrentExternalBankLoan = value;
                OnPropertyChanged("CurrentExternalBankLoan");
                if (CurrentExternalBankLoan != null)
                    SetBankDetails();
            }
        }

        private IEnumerable<ExternalBankLoanView> _SelectedEmpExternalBankLoan;
        public IEnumerable<ExternalBankLoanView> SelectedEmpExternalBankLoan
        {
            get { return _SelectedEmpExternalBankLoan; }
            set { _SelectedEmpExternalBankLoan = value; OnPropertyChanged("SelectedEmpExternalBankLoan"); }
        }
        
        private IEnumerable<z_Bank> _Bank;
        public IEnumerable<z_Bank> Bank
        {
            get { return _Bank; }
            set { _Bank = value; OnPropertyChanged("Bank"); }
        }

        private z_Bank _CurrentBank;
        public z_Bank CurrentBank
        {
            get { return _CurrentBank; }
            set { _CurrentBank = value; OnPropertyChanged("CurrentBank"); if (CurrentBank != null)FilterBrankBranchByBank(); }
        }

        private IEnumerable<z_BankBranch> _BankBranch;
        public IEnumerable<z_BankBranch> BankBranch
        {
            get { return _BankBranch; }
            set { _BankBranch = value; OnPropertyChanged("BankBranch"); }
        }

        private z_BankBranch _CurrentBankBranch;
        public z_BankBranch CurrentBankBranch
        {
            get { return _CurrentBankBranch; }
            set { _CurrentBankBranch = value; OnPropertyChanged("CurrentBankBranch"); }
        }

        private IEnumerable<mas_ExtBankLoan> _ExtBankLoan;
        public IEnumerable<mas_ExtBankLoan> ExtBankLoan
        {
            get { return _ExtBankLoan; }
            set { _ExtBankLoan = value; OnPropertyChanged("ExtBankLoan"); }
        }

        private mas_ExtBankLoan _CurrentExtBankLoan;
        public mas_ExtBankLoan CurrentExtBankLoan
        {
            get { return _CurrentExtBankLoan; }
            set { _CurrentExtBankLoan = value; OnPropertyChanged("CurrentExtBankLoan"); }
        }

        private IEnumerable<usr_UserEmployee> _Users;
        public IEnumerable<usr_UserEmployee> Users
        {
            get { return _Users; }
            set { _Users = value; OnPropertyChanged("Users"); }
        }

        private IEnumerable<dtl_EmployeeSupervisor> _Supervisors;
        public IEnumerable<dtl_EmployeeSupervisor> Supervisors
        {
            get { return _Supervisors; }
            set { _Supervisors = value; OnPropertyChanged("Supervisors"); }
        }

        private dtl_EmployeeSupervisor _CurrentSupervisors;
        public dtl_EmployeeSupervisor CurrentSupervisors
        {
            get { return _CurrentSupervisors; }
            set { _CurrentSupervisors = value; OnPropertyChanged("CurrentSupervisors"); }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterEmployeeLoansByEPF(); }
        }

        private bool _isBank;

        public bool isBank
        {
            get { return _isBank; }
            set { _isBank = value; OnPropertyChanged("isBank"); }
        }

        private bool _isCheque;

        public bool isCheque
        {
            get { return _isCheque; }
            set { _isCheque = value; OnPropertyChanged("isCheque"); }
        }

        private bool _isHold;

        public bool isHold
        {
            get { return _isHold; }
            set { _isHold = value; OnPropertyChanged("isHold"); }
        }
        
        #endregion

        #region Refresh Methods
        private void RefreshEmployees()
        {
            AllEmployees.Clear();
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
                {
                    Employees = e.Result;
                    if (Employees != null)
                    {
                        AllEmployees = Employees.ToList();
                    }
                };
            serviceClient.GetAllEmployeeDetailAsync();
        }
        private void RefreshExternalBankLoan()
        {
            serviceClient.GetExternalLoanViewForMainWindowCompleted += (s, e) =>
                {
                    ExternalBankLoan = e.Result;
                    CurrentExternalBankLoan = new ExternalBankLoanView();
                };
            serviceClient.GetExternalLoanViewForMainWindowAsync();
        }

        private void RefreshAllExternalBankLoan()
        {
            serviceClient.GetExternalLoanSCompleted += (s, e) =>
            {
                ExtBankLoan = e.Result;
            };
            serviceClient.GetExternalLoanSAsync();
        }

        private void RerfreshBank()
        {
            serviceClient.GetBanksCompleted += (s, e) =>
                {
                    Bank = e.Result;
                    if (Bank != null)
                        AllBank = Bank.ToList();

                };
            serviceClient.GetBanksAsync();
        }
        private void RefreshBankBranch()
        {
            AllBankBranch.Clear();
            serviceClient.GetBanckBranchCompleted += (s, e) =>
                {
                    AllBankBranch = e.Result.ToList();
                };
            serviceClient.GetBanckBranchAsync();
        }
        private void RefreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                Users = e.Result;
            };
            serviceClient.GetUserEmployeesAsync();
        }
        private void RefreshSupervisors()
        {
            serviceClient.GetEmployeeManagerCompleted += (s, e) =>
            {
                Supervisors = e.Result;
            };
            serviceClient.GetEmployeeManagerAsync();
        }

        #endregion

        #region Button Commands
        public ICommand SearchEmpButton
        {
            get { return new RelayCommand(searchEmp); }
        }
        public ICommand SaveButton
        {
            get { return new RelayCommand(SaveExtBankLoan); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }
        public ICommand CancelButton
        {
            get { return new RelayCommand(Cancel); }
        }
        public ICommand InstallmentMonthButton
        {
            get { return new RelayCommand(InstallmentMonths, InstallmentMonthsCE); }
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
            if (CurrentEmployeesForDialogBox != null)
            {
                CurrentEmployee = null;
                CurrentEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentEmployeesForDialogBox.employee_id);
                CurrentExternalBankLoan.employee_id = CurrentEmployee.employee_id;
                CurrentExternalBankLoan.epf_no = CurrentEmployee.epf_no;
                CurrentExternalBankLoan.first_name = CurrentEmployee.first_name;
                CurrentExternalBankLoan.nic = CurrentEmployee.nic;
                CurrentExternalBankLoan.surname = CurrentEmployee.surname;
                SelectedEmpExternalBankLoan = ExternalBankLoan.Where(c => c.employee_id == CurrentExternalBankLoan.employee_id);
            }
            else
            {
                CurrentEmployee = null;
            }
        }
        private void FilterBrankBranchByBank()
        {
            BankBranch = null;
            BankBranch = AllBankBranch.Where(c => c.bank_id == CurrentBank.bank_id);
        }
        private void SetBankDetails()
        {
            BankBranch = null;
            CurrentBank = null;
            CurrentBankBranch = null;
            CurrentBank = AllBank.FirstOrDefault(c => c.bank_id == CurrentExternalBankLoan.bank_id);
            //  BankBranch = AllBankBranch.Where(c => c.bank_id == CurrentBank.bank_id);
            CurrentBankBranch = AllBankBranch.FirstOrDefault(c => c.branch_id == CurrentExternalBankLoan.branch_id);
            if (CurrentExternalBankLoan.BankLoanID > 0)
            {
                isBank = (bool)ExternalBankLoan.FirstOrDefault(c => c.BankLoanID == CurrentExternalBankLoan.BankLoanID).isBank;
                isCheque = (bool)ExternalBankLoan.FirstOrDefault(c => c.BankLoanID == CurrentExternalBankLoan.BankLoanID).isCheque;
                isHold = (bool)ExternalBankLoan.FirstOrDefault(c => c.BankLoanID == CurrentExternalBankLoan.BankLoanID).is_Hold;
            }
        }
        private void SaveExtBankLoan()
        {
            mas_ExtBankLoan existLoan = ExtBankLoan.FirstOrDefault(c => c.BankLoanID == CurrentExternalBankLoan.BankLoanID);
            if (existLoan == null)
            {
                if (clsSecurity.GetSavePermission(608))
                {
                    if (ValidateSave())
                    {
                        clsMessages.setMessage("Do You Want To Save This Record?", Visibility.Visible);
                        if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                        {
                            CurrentExtBankLoan.BankLoanName = CurrentExternalBankLoan.BankLoanName;
                            CurrentExtBankLoan.LoanTotalAmout = CurrentExternalBankLoan.LoanTotalAmout;
                            CurrentExtBankLoan.LoanStartDate = CurrentExternalBankLoan.LoanStartDate;
                            CurrentExtBankLoan.LoanEndDate = CurrentExternalBankLoan.LoanEndDate;
                            CurrentExtBankLoan.LoanInstallmentMonths = CurrentExternalBankLoan.LoanInstallmentMonths;
                            CurrentExtBankLoan.LoanAmountPerMonth = CurrentExternalBankLoan.LoanAmountPerMonth;
                            CurrentExtBankLoan.employee_id = CurrentEmployee.employee_id;
                            CurrentExtBankLoan.bank_id = CurrentBank.bank_id;
                            CurrentExtBankLoan.Bankbranch_id = CurrentBankBranch.branch_id;
                            CurrentExtBankLoan.bankAccountNo = CurrentExternalBankLoan.bankAccountNo;
                            CurrentExtBankLoan.bankAccountName = CurrentExternalBankLoan.bankAccountName;
                            CurrentExtBankLoan.isdelete = false;
                            CurrentExtBankLoan.isComplete = false;
                            CurrentExtBankLoan.isCanceled = false;
                            CurrentExtBankLoan.save_datetime = DateTime.Now;
                            CurrentExtBankLoan.save_user_id = clsSecurity.loggedUser.user_id;
                            CurrentExtBankLoan.isBank = isBank;
                            CurrentExtBankLoan.isCheque = isCheque;
                            CurrentExtBankLoan.is_Hold = isHold;

                            if (serviceClient.SaveExternalBankLoan(CurrentExtBankLoan))
                            {
                                RefreshExternalBankLoan();
                                RefreshAllExternalBankLoan();
                                clsMessages.setMessage("Bank Loan Have Been Saved Successfully...");
                            }
                            else
                            {
                                clsMessages.setMessage("Bank Loan Save Process Failed...");
                            }
                        }
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to save this record");
            }
            else
            {
                if (clsSecurity.GetUpdatePermission(608))
                {
                    existLoan.bank_id = CurrentBank.bank_id;
                    existLoan.Bankbranch_id = CurrentBankBranch.branch_id;
                    existLoan.bankAccountNo = CurrentExternalBankLoan.bankAccountNo;
                    existLoan.bankAccountName = CurrentExternalBankLoan.bankAccountName;
                    existLoan.isBank = isBank;
                    existLoan.isCheque = isCheque;
                    existLoan.is_Hold = isHold;
                    existLoan.modified_user_id = clsSecurity.loggedUser.user_id;
                    existLoan.modified_datetime = DateTime.Now;
                    if (serviceClient.UpdateExternalBankLoan(existLoan))
                    {
                        RefreshExternalBankLoan();
                        RefreshAllExternalBankLoan();
                        clsMessages.setMessage("Bank Loan Have Been Updated Successfully...");
                    }
                    else
                    {
                        clsMessages.setMessage("Bank Loan Update Process Failed...");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to update");
            }

            //else
            //{
            //    clsMessages.setMessage("Bank Loan Save Process Failed...");
            //}
        }
        private bool ValidateSave()
        {

            //if (ValidateSupervisor()== null)
            //{
            //    clsMessages.setMessage("Your Not a Supervisor to This Employee To Add An External Loan...");
            //    return false;
            //}
            //if (CurrentExternalBankLoan.BankLoanID != 0)
            //{
            //    clsMessages.setMessage("External Bank Loan Cannot Be Updated....");
            //    return false;
            //}
            if ((CurrentEmployee.employee_id == null || CurrentEmployee.employee_id == Guid.Empty) && (CurrentExternalBankLoan.employee_id == null || CurrentExternalBankLoan.employee_id == Guid.Empty))
            {
                clsMessages.setMessage("Please Select An Employee From Search Employee Button....");
                return false;
            }
            else if (CurrentBank == null )
            {
                clsMessages.setMessage("Please Select A Bank....");
                return false;
            }
            else if (CurrentBank.bank_name == "Default")
            {
                clsMessages.setMessage("Please Select A Bank....");
                return false;
            }
            else if (CurrentBankBranch == null)
            {
                clsMessages.setMessage("Please Select A Bank Branch....");
                return false;
            }
            else if (CurrentBankBranch.name == "Default")
            {
                clsMessages.setMessage("Please Select A Bank Branch....");
                return false;
            }
            else if (CurrentExternalBankLoan.bankAccountNo == null)
            {
                clsMessages.setMessage("Please Enter Bank Account Number....");
                return false;
            }
            else if (CurrentExternalBankLoan.bankAccountName == null)
            {
                clsMessages.setMessage("Please Enter Bank Account Name....");
                return false;
            }
            else if (CurrentExternalBankLoan.BankLoanName == null)
            {
                clsMessages.setMessage("Please Enter Bank Loan Name....");
                return false;
            }
            else if (CurrentExternalBankLoan.LoanTotalAmout == null)
            {
                clsMessages.setMessage("Please Enter Total Loan Amount. If Loan Amount is not given Enter '0' To Process Further...");
                return false;
            }

            else if (CurrentExternalBankLoan.LoanStartDate == null)
            {
                clsMessages.setMessage("Please Select Loan Start Date....");
                return false;
            }
            //else if (CurrentExternalBankLoan.LoanEndDate == null)
            //{
            //    clsMessages.setMessage("Please Select Loan End Date....");
            //    return false;
            //}
            else if (CurrentExternalBankLoan.LoanInstallmentMonths == null)
            {
                clsMessages.setMessage("Please Enter Loan Installment Months.... If Loan Amount is not given Enter '0' To Process Further...");
                return false;
            }
            else if (CurrentExternalBankLoan.LoanAmountPerMonth == null)
            {
                clsMessages.setMessage("Please Enter Loan Amount Per Month....");
                return false;
            }
            else if (CurrentExternalBankLoan.LoanAmountPerMonth == 0)
            {
                clsMessages.setMessage("You Cannot Enter '0'. Please Enter A Value To Process Furter...");
                return false;
            }
            else if (CurrentExternalBankLoan.LoanInstallmentMonths == 0)
            {
                clsMessages.setMessage("The Loan Installment Months is '0' To Stop The Loan Process You Have To Cancel The Bank Loan Manually....");
                return true;
            }
            else
                return true;
        }
        private void New()
        {
            RefreshExternalBankLoan();
        }
        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(608))
            {
                if (DeleteValidate())
                {
                    CurrentExtBankLoan.BankLoanID = CurrentExternalBankLoan.BankLoanID;
                    CurrentExtBankLoan.isdelete = true;
                    CurrentExtBankLoan.delete_datetime = DateTime.Now;
                    CurrentExtBankLoan.delete_user_id = clsSecurity.loggedUser.user_id;

                    if (serviceClient.DeleteExternalBankLoan(CurrentExtBankLoan))
                    {
                        clsMessages.setMessage("External Bank Loan Have Been Deleted Successfully...");
                        RefreshExternalBankLoan();
                    }
                    else
                    {
                        clsMessages.setMessage("External Bank Loan Delete Process Failed...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");
        }
        private bool DeleteValidate()
        {
            if (CurrentExternalBankLoan.BankLoanID == null || CurrentExternalBankLoan.BankLoanID == 0)
            {
                clsMessages.setMessage("Please Select A Loan To Delete...");
                return false;
            }
            else
                return true;
        }
        private void Cancel()
        {
            if (clsSecurity.GetDeletePermission(608))
            {
                if (CancelValidate())
                {
                    MessageBoxResult Result = new MessageBoxResult();
                    Result = MessageBox.Show("Are You Sure You Want To Cancel The External Bank Loan?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Result == MessageBoxResult.Yes)
                    {
                        CurrentExtBankLoan.BankLoanID = CurrentExternalBankLoan.BankLoanID;
                        CurrentExtBankLoan.isCanceled = true;
                        if (serviceClient.CancelExternalBankLoan(CurrentExtBankLoan))
                        {
                            clsMessages.setMessage("External Bank Loan Have Been Canceled Successfully...");
                            RefreshExternalBankLoan();
                        }
                        else
                        {
                            clsMessages.setMessage("External Bank Loan Cancel Process Failed...");
                        }
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");
        }
        private bool CancelValidate()
        {
            if (CurrentExternalBankLoan.BankLoanID == null || CurrentExternalBankLoan.BankLoanID == 0)
            {
                clsMessages.setMessage("Please Select A Loan To Cancel...");
                return false;
            }
            else
                return true;
        }
        private void InstallmentMonths()
        {
            try
            {
                int months = ((DateTime)CurrentExternalBankLoan.LoanEndDate).Year * 12 + ((DateTime)CurrentExternalBankLoan.LoanEndDate).Month - (((DateTime)CurrentExternalBankLoan.LoanStartDate).Year * 12 + ((DateTime)CurrentExternalBankLoan.LoanStartDate).Month);
                CurrentExternalBankLoan.LoanInstallmentMonths = months;
            }
            catch (Exception)
            {

            }
        }
        private bool InstallmentMonthsCE()
        {
            if (CurrentExternalBankLoan.LoanStartDate == null && CurrentExternalBankLoan.LoanEndDate == null)
                return false;
            else
                return true;
        }
        private dtl_EmployeeSupervisor ValidateSupervisor()
        {
            Guid? CurrentLoggedUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
            CurrentSupervisors = Supervisors.FirstOrDefault(c => c.supervisor_employee_id == CurrentLoggedUserEmployee && c.employee_id == CurrentEmployee.employee_id && c.module_id == new Guid("9A8922B9-BDFE-4198-904C-A19BCAAFB5EB"));
            return CurrentSupervisors;
        }
        private void FilterEmployeeLoansByEPF()
        {
            ExternalBankLoan = ExternalBankLoan.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        }
        
        #endregion

    }
}
