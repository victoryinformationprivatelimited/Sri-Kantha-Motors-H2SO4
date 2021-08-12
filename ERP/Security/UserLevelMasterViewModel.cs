using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using System.Resources;

namespace ERP
{
    class UserLevelMasterViewModel : INotifyPropertyChanged
    {
        private ERPServiceClient serviceClinet = new ERPServiceClient();

        public UserLevelMasterViewModel()
        {
                this.refreshUserLevels();
                this._Savebtn = new SaveButton(this);
                this._Deletebtn = new DeleteButton(this);
                this._Newbtn = new NewButton(this);
                this.newRecord();   
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
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
                    refreshUserLevels();
                }
                else
                {
                    //MessageBox.Show("nhghhg");
                    SearchTextChanged();
                }
                UserLevels = null;
          
        }

        }

        private IEnumerable<usr_UserLevel> _UserLevels;
        public IEnumerable<usr_UserLevel> UserLevels
        {
            get { return this._UserLevels; }
            set { this._UserLevels = value; OnPropertyChanged("UserLevels"); }
        }

        private usr_UserLevel _CurrentUserLevel;
        public usr_UserLevel CurrentUserLevel
        {
            get { return this._CurrentUserLevel; }
            set { this._CurrentUserLevel = value; OnPropertyChanged("CurrentUserLevel"); }
        }

        private SaveButton _Savebtn;
        public SaveButton Savebtn
        {
            get { return this._Savebtn; }    
        }

        private NewButton _Newbtn;
        public NewButton Newbtn
        {
            get { return this._Newbtn; }
        }

        private DeleteButton _Deletebtn;
        public DeleteButton Deletebtn
        {
            get { return this._Deletebtn; }           
        }

        public class NewButton : ICommand
        {
            private UserLevelMasterViewModel ViewModel;
            public NewButton(UserLevelMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.newRecord();
            }
        }

        public class SaveButton: ICommand
        {
            private UserLevelMasterViewModel ViewModel;

            public SaveButton(UserLevelMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return this.ViewModel.CurrentUserLevel != null;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.saveRecord();
            }
        }

        public class DeleteButton : ICommand
        {
            private UserLevelMasterViewModel ViewModel;
            public DeleteButton(UserLevelMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return this.ViewModel.CurrentUserLevel != null;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.deleteRecord();
            }
        }

        private void refreshUserLevels()
        {
            this.serviceClinet.GetUserLevelCompleted += (s, e) =>
                {
                    this.UserLevels = e.Result;
                };
            this.serviceClinet.GetUserLevelAsync();
        }

        public void SearchTextChanged()
        {
            RefreshUserLevelSearchResult();
            UserLevels = this.serviceClinet.GetSearchResultUserLevel(Search);

            //MessageBox.Show(text.ToString());
        }

        public void RefreshUserLevelSearchResult()
        {
            this.serviceClinet.GetSearchResultUserLevelCompleted += (s, e) =>
            {
                this.UserLevels = (IEnumerable<usr_UserLevel>)this.serviceClinet.GetSearchResultUserLevel(Search);
            };
            this.serviceClinet.GetSearchResultUserLevelAsync(Search);
        }

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

        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";

        }

        private void saveRecord()
        {
            if (validation())
            {
                bool isUpdate = false;               
                usr_UserLevel newUserLevel = new usr_UserLevel();
                newUserLevel.user_level_id = CurrentUserLevel.user_level_id;
                newUserLevel.user_level = CurrentUserLevel.user_level;
                newUserLevel.save_datetime = System.DateTime.Now;
                newUserLevel.save_user_id = clsSecurity.loggedUser.user_id;
                newUserLevel.modified_datetime = System.DateTime.Now;
                newUserLevel.modified_user_id = clsSecurity.loggedUser.user_id;
                newUserLevel.delete_datetime = System.DateTime.Now;
                newUserLevel.delete_user_id = clsSecurity.loggedUser.user_id;
                newUserLevel.isdelete = false;

                if (newUserLevel != null)
                {
                    IEnumerable<usr_UserLevel> allUserLevel = this.serviceClinet.GetUserLevel();

                    foreach (usr_UserLevel user in allUserLevel)
                    {
                        if (user.user_level_id == CurrentUserLevel.user_level_id)
                        {
                            isUpdate = true;
                        }
                    }

                    if (isUpdate)
                    {
                        if (clsSecurity.GetUpdatePermission(101))
                        {
                            this.serviceClinet.updateUserLevel(newUserLevel);
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                        else
                        {
                            clsMessages.setMessage("You don't have permission to Update in this form...");
                        }

                    }
                    else
                    {
                        if (clsSecurity.GetSavePermission(101))
                        {
                            this.serviceClinet.saveUserLevel(newUserLevel);
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        }
                        else
                        {
                            clsMessages.setMessage("You don't have permission to Delete in this form...");
                        }
                    }
                    refreshUserLevels();
                    newRecord();
                }
            } 
        }

        private void newRecord()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.UserLevelMaster), clsSecurity.loggedUser.user_id))
            {
                CurrentUserLevel = new usr_UserLevel();
                CurrentUserLevel.user_level_id = Guid.NewGuid();
                refreshUserLevels();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void deleteRecord()
        {
            MessageBoxResult result  = MessageBox.Show("Do you want to delete this record ?", "ERP", MessageBoxButton.YesNo, MessageBoxImage.Question);           
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (clsSecurity.GetDeletePermission(101))
                    {
                        this.serviceClinet.deleteUserLevel(CurrentUserLevel);
                        refreshUserLevels();
                        MessageBox.Show("Record Delete Sucessful");
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Delete in this form...");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }               
            }
        }

        private bool validation()
        {
            string Message = "ERP System says..! please mention \n";
            bool isValidate = true;

            try
            {
                if (CurrentUserLevel.user_level == null || CurrentUserLevel.user_level.Trim().Length <= 0)
                {
                    Message += "User Level\n";
                    isValidate = false;
                }

                Message += " filed(s)";
                if (!isValidate)
                {
                    MessageBox.Show(Message, "ERP", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {                
            }
            return isValidate;
        }        
    }
}
