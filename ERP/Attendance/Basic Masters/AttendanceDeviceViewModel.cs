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
    class AttendanceDeviceViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public AttendanceDeviceViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.AttendanceDivice), clsSecurity.loggedUser.user_id))
            {
                reafreshAttendanceDeviceView();
                reafreshDeviceModels();
                this.New();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }
        #endregion

        #region Properties
        private IEnumerable<AttendanceDeviceView> _AttendanceDeviceView;
        public IEnumerable<AttendanceDeviceView> AttendanceDeviceView
        {
            get
            {
                return this._AttendanceDeviceView;
            }
            set
            {
                this._AttendanceDeviceView = value;
                this.OnPropertyChanged("AttendanceDeviceView");
            }
        }

        private AttendanceDeviceView _CurrentAttendanceView;
        public AttendanceDeviceView CurrentAttendanceView
        {
            get
            {
                return this._CurrentAttendanceView;
            }
            set
            {
                this._CurrentAttendanceView = value;
                this.OnPropertyChanged("CurrentAttendanceView");

            }
        }

        private IEnumerable<Z_AttendanceDevice> _AttendanceDevice;
        public IEnumerable<Z_AttendanceDevice> AttendanceDevice
        {
            get
            {
                return this._AttendanceDevice;
            }
            set
            {
                this._AttendanceDevice = value;
                this.OnPropertyChanged("AttendanceDevice");
            }
        }

        private Z_AttendanceDevice _CurrentAttendanceDevice;
        public Z_AttendanceDevice CurrentAttendanceDevice
        {
            get
            {
                return this._CurrentAttendanceDevice;
            }
            set
            {
                this._CurrentAttendanceDevice = value;
                this.OnPropertyChanged("CurrentAttendanceDevice");
            }
        }

        private IEnumerable<z_DeviceModel> _DeviceModels;
        public IEnumerable<z_DeviceModel> DeviceModel
        {
            get
            {
                return this._DeviceModels;
            }
            set
            {
                this._DeviceModels = value;
                this.OnPropertyChanged("DeviceModel");
            }
        }

        private z_DeviceModel _CurrentDeviceModel;
        public z_DeviceModel CurrentDeviceModel
        {
            get
            {
                return this._CurrentDeviceModel;
            }
            set
            {
                this._CurrentDeviceModel = value;
                this.OnPropertyChanged("CurrentDeviceModel");
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
                    this.reafreshAttendanceDeviceView();
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
                CurrentAttendanceDevice = null;
                CurrentAttendanceDevice = new Z_AttendanceDevice();
                CurrentAttendanceDevice.device_id = Guid.NewGuid();
                CurrentAttendanceView = null;
                CurrentAttendanceView = new AttendanceDeviceView();
                CurrentAttendanceView.device_id = Guid.NewGuid();
                CurrentDeviceModel = null;
                CurrentDeviceModel = new z_DeviceModel();
                CurrentDeviceModel.device_model_id = new Guid();
                reafreshAttendanceDeviceView();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

                Z_AttendanceDevice newAttendanceDevice = new Z_AttendanceDevice();
                newAttendanceDevice.device_id = CurrentAttendanceView.device_id;
                newAttendanceDevice.device_model_id = CurrentAttendanceView.device_model_id;
                newAttendanceDevice.device_name = CurrentAttendanceView.device_name;
                newAttendanceDevice.device_number2 = CurrentAttendanceView.device_number2;
                newAttendanceDevice.ip_address = CurrentAttendanceView.ip_address;
                newAttendanceDevice.port = CurrentAttendanceView.port;
                newAttendanceDevice.is_active = CurrentAttendanceView.is_active;
                newAttendanceDevice.save_user_id = clsSecurity.loggedUser.user_id;
                newAttendanceDevice.save_datetime = System.DateTime.Now;
                newAttendanceDevice.modified_user_id = clsSecurity.loggedUser.user_id;
                newAttendanceDevice.modified_datetime = System.DateTime.Now;
                newAttendanceDevice.delete_user_id = clsSecurity.loggedUser.user_id;
                newAttendanceDevice.delete_datetime = System.DateTime.Now;
                newAttendanceDevice.isdelete = false;

                IEnumerable<Z_AttendanceDevice> allAttendanceDevice = this.serviceClient.GetAttendanceDevice();

                if (allAttendanceDevice != null)
                {
                    foreach (var Attendance in allAttendanceDevice)
                    {
                        if (Attendance.device_id == CurrentAttendanceView.device_id)
                        {
                            IsUpdate = true;
                        }
                    }
                }
                if (newAttendanceDevice != null && newAttendanceDevice.device_id != null)
                {
                    if (IsUpdate)
                    {
                        if (clsSecurity.GetUpdatePermission(313))
                        {
                            newAttendanceDevice.modified_datetime = System.DateTime.Now;
                            newAttendanceDevice.modified_user_id = clsSecurity.loggedUser.user_id;

                            if (this.serviceClient.UpdateAttendanceDevice(newAttendanceDevice))
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
                        if (clsSecurity.GetSavePermission(313))
                        {
                            newAttendanceDevice.save_datetime = System.DateTime.Now;
                            newAttendanceDevice.save_user_id = clsSecurity.loggedUser.user_id;

                            if (serviceClient.SaveAttendanceDevice(newAttendanceDevice))
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
                    reafreshAttendanceDeviceView();
                    this.New();
                }
                clsMessages.setMessage("Please Mention the Device Name");
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
            if (CurrentAttendanceView != null)
            {
                if (CurrentAttendanceView.device_id == null || CurrentAttendanceView.device_id == Guid.Empty)
                    return false;
                if (CurrentAttendanceView.device_model_id == null || CurrentAttendanceView.device_model_id == Guid.Empty)
                    return false;
                if (CurrentAttendanceView.device_name == null || CurrentAttendanceView.device_name == string.Empty)
                    return false;
                if (CurrentAttendanceView.ip_address == null || CurrentAttendanceView.ip_address == string.Empty)
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
                if (clsSecurity.GetDeletePermission(313))
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result = MessageBox.Show("Do You Want To Delete This Record...?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        AttendanceDeviceView newDevice = new AttendanceDeviceView();
                        newDevice.device_id = CurrentAttendanceView.device_id;
                        newDevice.delete_user_id = clsSecurity.loggedUser.user_id;
                        newDevice.delete_datetime = System.DateTime.Now;

                        if (this.serviceClient.DeleteAttedanceDevice(newDevice))
                        {
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            reafreshAttendanceDeviceView();
                            this.New();
                        }
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
                else
                {
                    clsMessages.setMessage("You don't have permission to Delete this record(s)");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region DeleteButton Class & Property
        bool deleteCanExecute()
        {
            if (CurrentAttendanceView == null)
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

        #region Attendance Device List
        private void reafreshAttendanceDeviceView()
        {
            this.serviceClient.GetAttendanceDeviceViewCompleted += (s, e) =>
                {
                    this.AttendanceDeviceView = e.Result.Where(z => z.isdelete == false);
                };
            this.serviceClient.GetAttendanceDeviceViewAsync();
        }
        #endregion

        #region Device List
        private void reafreshDeviceModels()
        {
            this.serviceClient.GetDeviceModelesCompleted += (s, e) =>
                {
                    this.DeviceModel = e.Result;
                };
            this.serviceClient.GetDeviceModelesAsync();
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
            AttendanceDeviceView = AttendanceDeviceView.Where(e => e.device_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion

    }
}
