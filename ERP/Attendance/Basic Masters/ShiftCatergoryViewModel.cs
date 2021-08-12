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
    class ShiftCatergoryViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public ShiftCatergoryViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.ShiftCatagory), clsSecurity.loggedUser.user_id))
            {
                this.reafreshShiftCatergories();
                this.New();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }
        #endregion

        #region Properties
        private IEnumerable<z_ShiftCategory> _ShiftCatagories;
        public IEnumerable<z_ShiftCategory> ShiftCatagories
        {
            get
            {
                return this._ShiftCatagories;
            }
            set
            {
                this._ShiftCatagories = value;
                this.OnPropertyChanged("ShiftCatagories");
            }
        }

        private z_ShiftCategory _CurrentShiftCatergory;
        public z_ShiftCategory CurrentShiftCatagory
        {
            get
            {
                return this._CurrentShiftCatergory;
            }
            set
            {
                this._CurrentShiftCatergory = value;
                this.OnPropertyChanged("CurrentShiftCatagory");                
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
                    this.reafreshShiftCatergories();
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
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.ShiftCatagory), clsSecurity.loggedUser.user_id))
            {
                this.CurrentShiftCatagory = null;
                CurrentShiftCatagory = new z_ShiftCategory();
                CurrentShiftCatagory.shift_id = Guid.NewGuid();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
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
            bool IsUpdate = false;

            z_ShiftCategory newShiftCatagory = new z_ShiftCategory();
            newShiftCatagory.shift_id = CurrentShiftCatagory.shift_id;
            newShiftCatagory.shift_name = CurrentShiftCatagory.shift_name;
            newShiftCatagory.description = CurrentShiftCatagory.description;
            newShiftCatagory.save_user_id = clsSecurity.loggedUser.user_id;
            newShiftCatagory.save_datetime = System.DateTime.Now;
            newShiftCatagory.modified_datetime = System.DateTime.Now;
            newShiftCatagory.modified_user_id = clsSecurity.loggedUser.user_id;
            newShiftCatagory.delete_datetime = System.DateTime.Now;
            newShiftCatagory.delete_user_id = clsSecurity.loggedUser.user_id;
            newShiftCatagory.isdelete = false;
            newShiftCatagory.is_active = CurrentShiftCatagory.is_active;

            IEnumerable<z_ShiftCategory> allShifts = this.serviceClient.GetShiftcategory();

            if (allShifts != null)
            {
                foreach (var Shift in allShifts)
                {
                    if (Shift.shift_id == CurrentShiftCatagory.shift_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newShiftCatagory != null && newShiftCatagory.shift_id != null)
            {
                if (IsUpdate)
                {
                    if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.ShiftCatagory), clsSecurity.loggedUser.user_id))
                    {

                        newShiftCatagory.modified_user_id = clsSecurity.loggedUser.user_id;
                        newShiftCatagory.modified_datetime = System.DateTime.Now;

                        if (this.serviceClient.UpdateShiftCategory(newShiftCatagory))
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
                        clsMessages.setMessage(Properties.Resources.NoPermissionForUpdate);
                    }
                }
                else
                {
                    if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.ShiftCatagory), clsSecurity.loggedUser.user_id))
                    {
                        newShiftCatagory.save_user_id = clsSecurity.loggedUser.user_id;
                        newShiftCatagory.save_datetime = System.DateTime.Now;

                        if (this.serviceClient.SaveShiftCategory(newShiftCatagory))
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
                        clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
                    }
                }
                reafreshShiftCatergories();
            }
            clsMessages.setMessage("Please Mention the Shift Name....!");
        }
        #endregion

        #region SaveButton Calss & Property
        bool saveCanExecute()
        {
            if (CurrentShiftCatagory != null)
            {
                if (CurrentShiftCatagory.shift_id == null || CurrentShiftCatagory.shift_id == Guid.Empty)
                    return false;
                if (CurrentShiftCatagory.shift_name == null || CurrentShiftCatagory.shift_name == string.Empty)
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
            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.ShiftCatagory), clsSecurity.loggedUser.user_id))
            {
                MessageBoxResult rs = new MessageBoxResult();
                rs = MessageBox.Show("Do You Want To Delete This Record...?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (rs == MessageBoxResult.Yes)
                {
                    z_ShiftCategory shiftCatergory = new z_ShiftCategory();
                    shiftCatergory.shift_id = CurrentShiftCatagory.shift_id;
                    shiftCatergory.delete_user_id = clsSecurity.loggedUser.user_id;
                    shiftCatergory.delete_datetime = System.DateTime.Now;

                    if (this.serviceClient.DeleteShiftCategory(shiftCatergory))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        reafreshShiftCatergories();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
                this.New();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForDelete);
            }
        } 
        #endregion

        #region DeleteButton Calss & Property
        bool deleteCanExecute()
        {
            if (CurrentShiftCatagory == null)
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

        #region Shift List
        private void reafreshShiftCatergories()
        {
            this.serviceClient.GetShiftcategoryCompleted += (s, e) =>
                {
                    this.ShiftCatagories = e.Result.Where(k => k.isdelete==false);
                };
            this.serviceClient.GetShiftcategoryAsync();
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
            ShiftCatagories = ShiftCatagories.Where(e => e.shift_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion
    }
}
