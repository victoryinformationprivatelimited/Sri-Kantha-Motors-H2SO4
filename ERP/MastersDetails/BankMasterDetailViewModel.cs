using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.MastersDetails
{
    class BankMasterDetailViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        public BankMasterDetailViewModel()
        {
            this.reafreshEmployees();
            this.reafreshBanks();
            this.reafreshBranches();
            this.reafreshEmployeeBankBranchView();
            this.reafreshAccountNames();
            //this.New();
            CurrentEmployeeBankBranch = new EmployeeBankBranchView();
            CurrentEmployeeBankBranch.account_id = Guid.NewGuid();
            isActive = true;
        }

        private EmployeeBankBranchView _PreviousDefault;
        public EmployeeBankBranchView PreviousDefault
        {
            get { return _PreviousDefault; }
            set { _PreviousDefault = value; OnPropertyChanged("PreviousDefault"); }
        }


        private IEnumerable<dtl_EmployeeBank> _EmployeeBank;
        public IEnumerable<dtl_EmployeeBank> EmployeeBank
        {
            get
            {
                return this._EmployeeBank;
            }
            set
            {
                this._EmployeeBank = value;
                this.OnPropertyChanged("EmployeeBank");
            }
        }

        private EmployeeBankBranchView _TempCurrentBank;

        public EmployeeBankBranchView TempCurrentBank
        {
            get { return _TempCurrentBank; }
            set { _TempCurrentBank = value; }
        }

        private dtl_EmployeeBank _CurrentEmloyeeBank;
        public dtl_EmployeeBank CurrentEmloyeeBank
        {
            get
            {
                return this._CurrentEmloyeeBank;
            }
            set
            {
                this._CurrentEmloyeeBank = value;

                this.OnPropertyChanged("CurrentEmloyeeBank");
            }
        }

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get
            {
                return this._Employees;
            }
            set
            {
                this._Employees = value;
                this.OnPropertyChanged("Employees");
            }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee");
                try
                {
                    if (CurrentEmployee.employee_id != null && CurrentEmployee.employee_id != Guid.Empty)
                        CurrentEmployeeBankBranch.account_name = AccountNames.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id).account_name;
                }
                catch (Exception)
                {
                    //clsMessages.setMessage("No Account Name in Employee Master For The Selected Employee");
                }
            }
        }

        private IEnumerable<z_Bank> _Banks;
        public IEnumerable<z_Bank> Banks
        {
            get
            {
                return this._Banks;
            }
            set
            {
                this._Banks = value;
                this.OnPropertyChanged("Banks");
            }
        }

        private z_Bank _CurrentBank;
        public z_Bank CurrentBank
        {
            get { return _CurrentBank; }
            set
            {
                _CurrentBank = value;
                this.OnPropertyChanged("CurrentBank");
                if (CurrentBank != null && CurrentBank.bank_id != Guid.Empty)
                {
                    try
                    {
                        Branches = Branches.Where(z => z.bank_id == CurrentBank.bank_id);
                    }
                    catch (Exception)
                    {


                    }
                }

            }
        }

        private IEnumerable<z_BankBranch> _Branches;
        public IEnumerable<z_BankBranch> Branches
        {
            get
            {
                return this._Branches;
            }
            set
            {
                this._Branches = value;
                this.OnPropertyChanged("Branches");
            }
        }

        private z_BankBranch _CurrentBankBrnach;
        public z_BankBranch CurrentBankBrnach
        {
            get { return _CurrentBankBrnach; }
            set { _CurrentBankBrnach = value; this.OnPropertyChanged("CurrentBankBrnach"); }
        }

        private IEnumerable<EmployeeBankBranchView> _EmployeeBankBranch;
        public IEnumerable<EmployeeBankBranchView> EmployeeBankBranch
        {
            get
            {
                return this._EmployeeBankBranch;
            }
            set
            {
                this._EmployeeBankBranch = value;

                this.OnPropertyChanged("EmployeeBankBranch");
            }
        }

        private List<EmployeeBankBranchView> _EmployeeBankBranchList;

        public List<EmployeeBankBranchView> EmployeeBankBranchList
        {
            get { return _EmployeeBankBranchList; }
            set { _EmployeeBankBranchList = value; }
        }

        private EmployeeBankBranchView _CurrentEmployeeBankBranch;
        public EmployeeBankBranchView CurrentEmployeeBankBranch
        {
            get
            {
                return this._CurrentEmployeeBankBranch;
            }
            set
            {
                this._CurrentEmployeeBankBranch = value;
                TempCurrentBank = CurrentEmployeeBankBranch;
                this.OnPropertyChanged("CurrentEmployeeBankBranch");
                if (CurrentEmployeeBankBranch != null && CurrentEmployeeBankBranch.employee_id != Guid.Empty)
                {
                    try
                    {
                        CurrentEmployee = Employees.FirstOrDefault(z => z.employee_id == CurrentEmployeeBankBranch.employee_id);
                        CurrentBank = Banks.FirstOrDefault(z => z.bank_id == CurrentEmployeeBankBranch.bank_id);
                        CurrentBankBrnach = Branches.FirstOrDefault(z => z.branch_id == CurrentEmployeeBankBranch.branch_id);
                        isActive = (bool)EmployeeBankBranch.FirstOrDefault(z => z.account_id == CurrentEmployeeBankBranch.account_id).isactive;
                    }
                    catch (Exception)
                    {


                    }
                }
            }
        }

        private bool _isActive;

        public bool isActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("isActive"); }
        }
        

        private IEnumerable<dtl_EmployeeOtherOfficialDetails> _AccountNames;

        public IEnumerable<dtl_EmployeeOtherOfficialDetails> AccountNames
        {
            get { return _AccountNames; }
            set { _AccountNames = value; OnPropertyChanged("AccountName"); }
        }
        

        void New()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeBankDetail), clsSecurity.loggedUser.user_id))
            {
                Employees = null;
                reafreshEmployees();
                CurrentEmployee = null;
                CurrentEmployee = new mas_Employee();
                Banks = null;
                reafreshBanks();
                CurrentBank = new z_Bank();
                Branches = null;
                reafreshBranches();
                CurrentBankBrnach = new z_BankBranch();
                this.CurrentEmployeeBankBranch = null;
                this.CurrentEmloyeeBank = null;
                CurrentEmployeeBankBranch = new EmployeeBankBranchView();
                CurrentEmployeeBankBranch.account_id = Guid.NewGuid();
                isActive = true;
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }

        }

        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        private void reafreshEmployees()
        {
            //this.serviceClient.GetEmployeesCompleted += (s, e) =>
                //{
                    this.Employees = serviceClient.GetEmployees().OrderBy(c => c.emp_id);
                //};
            //this.serviceClient.GetEmployeesAsync();
        }

        private void reafreshBanks()
        {
            this.serviceClient.GetBanksCompleted += (s, e) =>
                {
                    this.Banks = e.Result.OrderBy(c => c.bank_name);
                };
            this.serviceClient.GetBanksAsync();
        }

        private void reafreshBranches()
        {
            this.serviceClient.GetBranchesCompleted += (s, e) =>
                {
                    this.Branches = e.Result.OrderBy(c => c.name);
                };
            this.serviceClient.GetBranchesAsync();
        }



        /*private void reafreshEmployeeBankBranchView()
        {
            this.serviceClient.GetEmployeeBankBranchViewCompleted += (s, e) =>
                {
                    this.EmployeeBankBranch = e.Result.OrderBy(c => c.emp_id);
                };
            this.serviceClient.GetEmployeeBankBranchViewAsync();
        }*/

        private void reafreshEmployeeBankBranchView()
        {
            //this.serviceClient.GetEmployeeBankBranchViewCompleted += (s, e) =>
            //    {
            this.EmployeeBankBranch = serviceClient.GetEmployeeBankBranchView().OrderBy(c => c.emp_id);
            _EmployeeBankBranchList = EmployeeBankBranch.ToList();
            //    };
            //this.serviceClient.GetEmployeeBankBranchViewAsync();
        }

        private void reafreshAccountNames()
        {
            this.serviceClient.GetAccountNamesCompleted += (s, e) =>
            {
                this.AccountNames = e.Result;
            };
            this.serviceClient.GetAccountNamesAsync();
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, SaveCanExecute);
            }
        }

        private bool SaveCanExecute()
        {
            if (CurrentEmployeeBankBranch != null)
            {
                if (CurrentEmployeeBankBranch.account_id == null)
                {
                    return false;
                }
                if (CurrentEmployee == null)
                {
                    return false;
                }
                if (this.CurrentBank == null)
                {
                    return false;
                }
                if (CurrentBankBrnach == null)
                {
                    return false;
                }

                if (CurrentEmployeeBankBranch.account_no == null)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Save()
        {
            bool isUpdate = false;


            dtl_EmployeeBank newbank = new dtl_EmployeeBank();
            newbank.account_id = CurrentEmployeeBankBranch.account_id;
            newbank.employee_id = CurrentEmployee.employee_id;
            newbank.bank_id = CurrentBank.bank_id;
            newbank.branch_id = CurrentBankBrnach.branch_id;
            newbank.account_no = CurrentEmployeeBankBranch.account_no;
            newbank.account_name = CurrentEmployeeBankBranch.account_name;
            newbank.isactive = isActive;
            newbank.isDefault = CurrentEmployeeBankBranch.isDefault;
            newbank.save_datetime = System.DateTime.Now;
            newbank.save_user_id = clsSecurity.loggedUser.user_id;
            newbank.modified_datetime = System.DateTime.Now;
            newbank.modified_user_id = clsSecurity.loggedUser.user_id;
            List<EmployeeBankBranchView> allEmpb = serviceClient.GetEmployeeBankBranchView().ToList();
            if (CurrentEmployeeBankBranch.isDefault == true)
            {
                PreviousDefault = allEmpb.Where(c => c.employee_id == TempCurrentBank.employee_id && c.isDefault == CurrentEmployeeBankBranch.isDefault).FirstOrDefault();
                if (PreviousDefault != null)
                {
                    dtl_EmployeeBank defaultbank = new dtl_EmployeeBank();
                    defaultbank.account_id = PreviousDefault.account_id;
                    defaultbank.bank_id = PreviousDefault.bank_id;
                    defaultbank.employee_id = PreviousDefault.employee_id;
                    defaultbank.branch_id = PreviousDefault.branch_id;
                    defaultbank.account_no = PreviousDefault.account_no;
                    defaultbank.account_name = PreviousDefault.account_name;
                    defaultbank.isactive = PreviousDefault.isactive;
                    defaultbank.isDefault = false;
                    if (serviceClient.UpdateDtlBank(defaultbank))
                        clsMessages.setMessage("Previous Default Bank Cancel");
                }
            }
            foreach (EmployeeBankBranchView empb in allEmpb)
            {
                if (empb.employee_id == TempCurrentBank.employee_id && empb.account_id == CurrentEmployeeBankBranch.account_id)
                {
                    isUpdate = true;
                    break;
                }
            }

            if (newbank != null && newbank.employee_id != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(212))
                    {
                        if (serviceClient.UpdateDtlBank(newbank))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            New();
                            this.reafreshEmployeeBankBranchView();
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }

                }
                else
                {
                    if (clsSecurity.GetSavePermission(212))
                    {
                        EmployeeBankBranchView existBank = EmployeeBankBranch.FirstOrDefault(c => c.employee_id == newbank.employee_id);

                        if (existBank == null)
                        {
                            if (serviceClient.SaveUserBank(newbank))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                New();
                                this.reafreshEmployeeBankBranchView();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            } 
                        }
                        else
                            clsMessages.setMessage("Employee Bank Already Exist");
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
            }
            else
            {
                clsMessages.setMessage("Please mension empty fileds !");
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, SaveCanExecute);
            }
        }

        private void Delete()
        {
            dtl_EmployeeBank delEmpbank = new dtl_EmployeeBank();
            delEmpbank.employee_id = CurrentEmployee.employee_id;
            delEmpbank.bank_id = CurrentBank.bank_id;
            delEmpbank.branch_id = CurrentBankBrnach.branch_id;
            delEmpbank.account_no = CurrentEmployeeBankBranch.account_no;
            delEmpbank.account_name = CurrentEmployeeBankBranch.account_name;
            delEmpbank.isactive = isActive;
            delEmpbank.delete_datetime = System.DateTime.Now;
            delEmpbank.delete_user_id = clsSecurity.loggedUser.user_id;
            if (clsSecurity.GetDeletePermission(212))
            {

                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do You Want to Delete This Record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteDtlBank(delEmpbank))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        this.reafreshEmployeeBankBranchView();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        private bool DeleteCanExecute()
        {
            if (CurrentEmployeeBankBranch != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Search

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); serachTextChanged(); }
        }

        private void serachTextChanged()
        {
            EmployeeBankBranch = null;
            List<EmployeeBankBranchView> searched = new List<EmployeeBankBranchView>();
            try
            {
                // h 2019-10-24
                //searched = _EmployeeBankBranchList.Where(c => c.emp_id.Contains(Search)).ToList();
                //EmployeeBankBranch = searched;


                searched = _EmployeeBankBranchList.Where(c => c.emp_id != null && c.emp_id.Contains(Search)).ToList();


                EmployeeBankBranch = searched;
            }
            catch (Exception)
            {
                EmployeeBankBranch = _EmployeeBankBranchList;
            }
        }

        #endregion


    }
}
