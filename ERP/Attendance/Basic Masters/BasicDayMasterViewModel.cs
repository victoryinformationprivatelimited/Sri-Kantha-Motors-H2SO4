using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Masters
{
    class BasicDayMasterViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public BasicDayMasterViewModel()
        {
            this.refreshBasicDays();
            this.New();
        } 
        #endregion

        #region Properties
        private IEnumerable<z_BasicDay> _BasicDays;
        public IEnumerable<z_BasicDay> BasicDays
        {
            get
            {
                return this._BasicDays;
            }
            set
            {
                this._BasicDays = value;
                this.OnPropertyChanged("BasicDays");
            }
        }

        private z_BasicDay _CurrentBasicDay;
        public z_BasicDay CurrentBasicDay
        {
            get
            {
                return this._CurrentBasicDay;
            }
            set
            {
                this._CurrentBasicDay = value;
                this.OnPropertyChanged("CurrentBasicDay");

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
                    this.refreshBasicDays();
                }
                else
                {
                    SearchTextChanged();
                }

            }
        } 
        #endregion

        #region New Method
        void New()
        {
            try
            {
               
                    CurrentBasicDay = null;
                    CurrentBasicDay = new z_BasicDay();
                    CurrentBasicDay.day_id = Guid.NewGuid();
                    refreshBasicDays(); 
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        } 
        #endregion

        #region New Button Class & Property
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
        #endregion

        #region Save Method
        void Save()
        {
            bool IsUpdate = false;

            z_BasicDay newBasicday = new z_BasicDay();
            newBasicday.day_id = CurrentBasicDay.day_id;
            newBasicday.day_of_week = CurrentBasicDay.day_of_week;
            newBasicday.day_position = CurrentBasicDay.day_position;
            newBasicday.save_user_id = clsSecurity.loggedUser.user_id;
            newBasicday.save_datetime = System.DateTime.Now;
            newBasicday.modified_user_id = clsSecurity.loggedUser.user_id;
            newBasicday.modified_datetime = System.DateTime.Now;
            newBasicday.delete_user_id = clsSecurity.loggedUser.user_id;
            newBasicday.delete_datetime = System.DateTime.Now;
            newBasicday.isdelete = false;

            IEnumerable<z_BasicDay> allbasicDays = this.serviceClient.GetBasicDays();

            if (allbasicDays != null)
            {
                foreach (var Day in allbasicDays)
                {
                    if (Day.day_id == CurrentBasicDay.day_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newBasicday != null && newBasicday.day_id != null)
            {
                if (IsUpdate)
                {
                    
                        newBasicday.modified_user_id = clsSecurity.loggedUser.user_id;
                        newBasicday.modified_datetime = System.DateTime.Now;

                        if (this.serviceClient.UpdateBasicDays(newBasicday))
                        {
                            MessageBox.Show("Record Update Successfully");
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                        else
                        {
                            MessageBox.Show("Record Update Failed");
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        } 
                    
                }
                else
                {
                    
                        newBasicday.save_user_id = clsSecurity.loggedUser.user_id;
                        newBasicday.save_datetime = System.DateTime.Now;

                        if (this.serviceClient.SaveBasicDays(newBasicday))
                        {
                            MessageBox.Show("Record Save Successfully");
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        }
                        else
                        {
                            MessageBox.Show("Record SaveFailed");
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        } 
                    
                }
                refreshBasicDays();
            }
            clsMessages.setMessage("Please Mention the Day of Week...");
        } 
        #endregion

        #region SaveButton Class & Button Property
        bool saveCanExecute()
        {
            if (CurrentBasicDay != null)
            {
                if (CurrentBasicDay.day_id == null || CurrentBasicDay.day_id == Guid.Empty)
                    return false;
                if (CurrentBasicDay.day_of_week == null || CurrentBasicDay.day_of_week == string.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        } 
        #endregion

        #region Delete Method
        void Delete()
        {
            
                MessageBoxResult result = new MessageBoxResult();
                result = MessageBox.Show("Do You want To Delete This Record..?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                if (result == MessageBoxResult.Yes)
                {
                    z_BasicDay newDay = new z_BasicDay();
                    newDay.day_id = CurrentBasicDay.day_id;
                    newDay.delete_user_id = clsSecurity.loggedUser.user_id;
                    newDay.delete_datetime = System.DateTime.Now;

                    if (this.serviceClient.DeleteBasicDay(newDay))
                    {
                        MessageBox.Show("Record Deleted");
                        refreshBasicDays();
                    }
                    else
                    {
                        MessageBox.Show("Record Delete Failed");
                    }
                
                this.New(); 
            }
        } 
        #endregion

        #region DeleteButton & Button Property
        bool deleteCanExecute()
        {
            if (CurrentBasicDay == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        } 
        #endregion

        #region List of Basic Days
        private void refreshBasicDays()
        {
            this.serviceClient.GetBasicDaysCompleted += (s, e) =>
                {
                    this.BasicDays = e.Result.Where(i => i.isdelete==false);
                };
            this.serviceClient.GetBasicDaysAsync();
        } 
        #endregion

        #region Search Class
        public class relayCommand : ICommand
        {
            readonly Action<object> _Execute;
            readonly Predicate<object> _CanExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public relayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _Execute = execute;
                _CanExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _CanExecute == null ? true : _CanExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _Execute(parameter);
            }


            public event EventHandler CanExecuteChanged;
        }
        #endregion

        #region Search Property
        relayCommand _OperationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_OperationCommand == null)
                {
                    _OperationCommand = new relayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }

                return this._OperationCommand;
            }
        }

        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region Search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }

        #endregion

        #region Search Method for all Properties
        public void SearchTextChanged()
        {
            BasicDays = BasicDays.Where(e => e.day_of_week.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion


    }
}
