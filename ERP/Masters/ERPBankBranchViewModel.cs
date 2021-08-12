using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP
{
    class ERPBankBranchViewModel : INotifyPropertyChanged
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public ERPBankBranchViewModel()
        {
            this._NewBtn = new NewButton(this);
            this._SaveBtn = new SaveButton(this);
            this._DeleteBtn = new DeleteButton(this);
            this.newRecode();
            refreshBank();
            refreshBranchers();

        }

        #region Property change event
        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string propertyName)
        {

            var pargs = new PropertyChangedEventArgs(propertyName);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Banks Propety
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

        #endregion

        #region Current Bank Propety
        private z_Bank _CurrentBank;
        public z_Bank CurrentBank
        {
            get
            {
                return this._CurrentBank;
            }
            set
            {
                this._CurrentBank = value;
                this.OnPropertyChanged("CurrentBank");
                //refreshBranchers(CurrentBank);

            }
        }
        #endregion

        #region Branch Propety
        private IEnumerable<z_BankBranch> _Branchers;
        public IEnumerable<z_BankBranch> Branchers
        {
            get
            {
                return this._Branchers;
            }
            set
            {
                this._Branchers = value;
                this.OnPropertyChanged("Branchers");
            }

        }
        #endregion

        #region Current Branch Propety
        private z_BankBranch _CurrentBranch;
        public z_BankBranch CurrentBranch
        {
            get
            {
                return this._CurrentBranch;
            }
            set
            {
                this._CurrentBranch = value;
                this.OnPropertyChanged("CurrentBranch");
            }
        }
        #endregion

        #region New Button Class
        public class NewButton : ICommand
        {
            private ERPBankBranchViewModel View;
            public NewButton(ERPBankBranchViewModel View)
            {
                this.View = View;
                this.View.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };

            }
            public bool CanExecute(object parameter)
            {
                if (this.View._CurrentBank != null)
                {
                    return this.View._CurrentBank != null;
                }
                return false;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

                this.View.newRecode();

            }
        }
        #endregion

        #region New Button Propeties
        private NewButton _NewBtn;
        public NewButton NewBtn
        {
            get { return _NewBtn; }
        }
        #endregion

        #region Save Buttion Properties
        private SaveButton _SaveBtn;
        public SaveButton SaveBtn
        {
            get { return _SaveBtn; }
        }
        #endregion

        #region Save Button Class
        public class SaveButton : ICommand
        {
            private ERPBankBranchViewModel View;

            public SaveButton(ERPBankBranchViewModel View)
            {
                this.View = View;
                this.View.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }
            public bool CanExecute(object parameter)
            {
                if (this.View.CurrentBranch != null)
                {
                    return this.View.CurrentBranch != null;
                }
                return false;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.View.Save();
                this.View.newRecode();
                this.View.refreshBranchers();
            }
        }
        #endregion

        #region Delete Button Properties
        private DeleteButton _DeleteBtn;
        public DeleteButton DeleteBtn
        {
            get { return _DeleteBtn; }
        }
        #endregion

        #region Delete Button Properties
        public class DeleteButton : ICommand
        {
            private ERPBankBranchViewModel View;
            public DeleteButton(ERPBankBranchViewModel View)
            {
                this.View = View;
                this.View.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }
            public bool CanExecute(object parameter)
            {
                if (this.View.CurrentBranch != null)
                {
                    return this.View.CurrentBranch != null;
                }
                return false;

            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.View.Delete();
                this.View.newRecode();
            }
        }
        #endregion

        #region Search Propaty

        RelayCommand _operationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_operationCommand == null)
                {
                    _operationCommand = new RelayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }
                return _operationCommand;
            }
        }

        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region search Comand Exicute
        public void ExecuteCommand()
        {
            Search = "Search";
            Search = "";

        }
        #endregion

        #region Search Key
        public class RelayCommand : ICommand
        {
            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }


            public bool CanExecute(object parameter)
            {
                return _canExecute == null ? true : _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }


        }
        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;

            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (this._Search == "")
                {
                    refreshBranchers();
                }
                else
                {
                    searchTextChange();
                }

            }

        }
        #endregion

        public void searchTextChange()
        {
            Branchers = Branchers.Where(e => e.name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #region Search Refresh Methord


        #region Delete Methord
        public void Delete()
        {
            if (clsSecurity.GetDeletePermission(211))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this record ?", "Quesion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    z_BankBranch branch = new z_BankBranch();
                    branch.branch_id = CurrentBranch.branch_id;
                    branch.delete_datetime = System.DateTime.Now;
                    branch.delete_user_id = clsSecurity.loggedUser.user_id;
                    branch.isdelete = true;
                    if (serviceClient.DeleteBranch(branch))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        refreshBranchers();
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
        #endregion

        #region Save Methord
        public void Save()
        {
            bool isUpdate = false;

            z_BankBranch newBranch = new z_BankBranch();
            z_Bank newBank = new z_Bank();
            newBranch.bank_id = CurrentBank.bank_id;
            newBranch.branch_id = CurrentBranch.branch_id;
            newBranch.bank_branch_code = CurrentBranch.bank_branch_code;
            newBranch.name = CurrentBranch.name;
            newBranch.address = CurrentBranch.address;
            newBranch.telephone = CurrentBranch.telephone;
            newBranch.email = CurrentBranch.email;
            newBranch.fax = CurrentBranch.fax;
            newBranch.save_datetime = System.DateTime.Now;
            newBranch.save_user_id = clsSecurity.loggedUser.user_id;
            newBranch.modified_datetime = System.DateTime.Now;
            newBranch.modified_user_id = Guid.Empty;
            newBranch.delete_datetime = System.DateTime.Now;
            newBranch.delete_user_id = Guid.Empty;

            List<z_BankBranch> allBranch = serviceClient.GetBanckBranch().ToList();

            foreach (z_BankBranch emp in allBranch)
            {
                if (emp.branch_id == CurrentBranch.branch_id)
                {
                    isUpdate = true;
                }
            }

            if (newBranch != null && newBranch.name != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(211))
                    {
                        newBranch.modified_datetime = System.DateTime.Now;
                        newBranch.modified_user_id = clsSecurity.loggedUser.user_id;

                        if (serviceClient.UpdateBranch(newBranch))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
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
                    if (clsSecurity.GetSavePermission(211))
                    {
                        if (serviceClient.SaveBranch(newBranch))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }

            }
            else
            {
                MessageBox.Show("Please mension empty fileds !");
            }
        }
        #endregion

        #region New Recode Methord
        public void newRecode()
        {
                CurrentBranch = new z_BankBranch();
                CurrentBranch.branch_id = Guid.NewGuid();
            
        }
        #endregion

        #region Refresh Branchers List
        private void refreshBranchers()
        {
            this.serviceClient.GetBanckBranchCompleted += (s, e) =>
            {
                this.Branchers = e.Result.Where(a => a.isdelete == false);
            };
            this.serviceClient.GetBanckBranchAsync();

        }

        #endregion

        private void refreshBank()
        {
            this.serviceClient.GetBanksCompleted += (s, e) =>
            {
                this.Banks = e.Result;
            };
            this.serviceClient.GetBanksAsync();

        }

        #endregion
    }

}
