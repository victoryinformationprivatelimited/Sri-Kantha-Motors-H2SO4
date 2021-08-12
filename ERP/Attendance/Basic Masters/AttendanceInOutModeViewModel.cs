using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Basic_Masters
{
    class AttendanceInOutModeViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Consructor
        public AttendanceInOutModeViewModel()
        {
            this.reafreshInOutMode();
            this.New();
        } 
        #endregion

        #region Properties
        private IEnumerable<z_AttendanceInOutMode> _InOutMode;
        public IEnumerable<z_AttendanceInOutMode> INOutMode
        {
            get
            {
                return this._InOutMode;
            }
            set
            {
                this._InOutMode = value;
                this.OnPropertyChanged("INOutMode");
            }
        }

        private z_AttendanceInOutMode _CurrentInOutMode;
        public z_AttendanceInOutMode CurrentInOutMode
        {
            get
            {
                return this._CurrentInOutMode;
            }
            set
            {
                this._CurrentInOutMode = value;
                this.OnPropertyChanged("CurrentInOutMode");
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
                    this.reafreshInOutMode();
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
            CurrentInOutMode = null;
            CurrentInOutMode = new z_AttendanceInOutMode();
            CurrentInOutMode.mode_id = Guid.NewGuid();
            reafreshInOutMode();
        }

        #endregion

        #region NewButton Class & Property
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
            try
            {
                bool IsUpdate = false;

                z_AttendanceInOutMode newInOutMode = new z_AttendanceInOutMode();
                newInOutMode.mode_id = CurrentInOutMode.mode_id;
                newInOutMode.mode_name = CurrentInOutMode.mode_name;
                newInOutMode.value = CurrentInOutMode.value;

                IEnumerable<z_AttendanceInOutMode> allInOutModes = this.serviceClient.GetAttendanceInOutMode();

                if (allInOutModes != null)
                {
                    foreach (var InOutMode in allInOutModes)
                    {
                        if (InOutMode.mode_id == CurrentInOutMode.mode_id)
                        {
                            IsUpdate = true;
                        }
                    }
                }
                if (newInOutMode != null && newInOutMode.mode_id != null)
                {
                    if (IsUpdate)
                    {
                        if (this.serviceClient.UpdateInOutDevice(newInOutMode))
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
                        if (this.serviceClient.SaveInOutDevice(newInOutMode))
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
                    reafreshInOutMode();
                    this.New();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        #endregion

        #region SaveButton Class & Property
        bool saveCanExecute()
        {
            if (CurrentInOutMode != null)
            {
                if (CurrentInOutMode.mode_id == null || CurrentInOutMode.mode_id == Guid.Empty)
                    return false;
                if (CurrentInOutMode.mode_name == null || CurrentInOutMode.mode_name == string.Empty)
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
            try
            {
                MessageBoxResult result = new MessageBoxResult();
                result = MessageBox.Show("Do You Want To Delete This Record....?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (this.serviceClient.DeleteInOutDevice(CurrentInOutMode))
                    {
                        MessageBox.Show("Record Deleted");
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        reafreshInOutMode();
                        this.New();
                    }
                    else
                    {
                        MessageBox.Show("Record Delete Failed");
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        } 
        #endregion

        #region DeleteButton Class & Property
        bool deleteCanExecute()
        {
            if (CurrentInOutMode == null)
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

        #region In Out Mode List
        private void reafreshInOutMode()
        {
            this.serviceClient.GetAttendanceInOutModeCompleted += (s, e) =>
                {
                    this.INOutMode = e.Result;
                };
            this.serviceClient.GetAttendanceInOutModeAsync();
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
            INOutMode = INOutMode.Where(e => e.mode_name.ToUpper().Contains(Search.ToUpper())).ToList();           
        }
        #endregion
    }
}
