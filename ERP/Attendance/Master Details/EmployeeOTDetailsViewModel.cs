using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.MastersDetails
{
    class EmployeeOTDetailsViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();

        List<EmployeeOTDetailLeaveView> EmployeeList = new List<EmployeeOTDetailLeaveView>();
        List<dtl_EmployeeOT> tempList = new List<dtl_EmployeeOT>();
        #endregion

        #region Constructor
        public EmployeeOTDetailsViewModel()
        {
            reafreshEmployeeOTDetailsView();
            reafreshEmployees();
            reafreshBasicDays();
            reafreshOTCatergories();
            this.New();
            IsAppliyForDays = false;
            IsActive = false;
        }
        #endregion

        #region Properties
        private IEnumerable<EmployeeOTDetailLeaveView> _EmployeeOTDetailView;
        public IEnumerable<EmployeeOTDetailLeaveView> EmployeeOTDetailView
        {
            get
            {
                return this._EmployeeOTDetailView;
            }
            set
            {
                this._EmployeeOTDetailView = value;
                this.OnPropertyChanged("EmployeeOTDetailView");
            }
        }

        private EmployeeOTDetailLeaveView _CurrentEmployeeOTDetailView;
        public EmployeeOTDetailLeaveView CurrentEmployeeOTDetailView
        {
            get
            {
                return this._CurrentEmployeeOTDetailView;
            }
            set
            {
                this._CurrentEmployeeOTDetailView = value;
                this.OnPropertyChanged("CurrentEmployeeOTDetailView");
                if (CurrentEmployeeOTDetailView != null)
                {
                    if (CurrentEmployeeOTDetailView.day_id != null)
                    {
                        IsAppliyForDays = true;
                    }
                    if (CurrentEmployeeOTDetailView.is_active == true)
                    {
                        this.IsActive = true;
                    }
                    else
                    {
                        this.IsActive = false;
                    }
                   
                }
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
            get
            {
                return this._CurrentEmployee;
            }
            set
            {
                this._CurrentEmployee = value;
                this.OnPropertyChanged("CurrentEmployee");
                this.FilterEmployee();
            }
        }

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

        private IEnumerable<z_OTCategory> _OTCatergories;
        public IEnumerable<z_OTCategory> OTCatergories
        {
            get
            {
                return this._OTCatergories;
            }
            set
            {
                this._OTCatergories = value;
                this.OnPropertyChanged("OTCatergories");
            }
        }

        private z_OTCategory _CurrentOTCatergory;
        public z_OTCategory CurrentOTCatergory
        {
            get
            {
                return this._CurrentOTCatergory;
            }
            set
            {
                this._CurrentOTCatergory = value;
                this.OnPropertyChanged("CurrentOTCatergory");
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
                    this.reafreshEmployeeOTDetailsView();
                }
                else
                {
                    SearchTextChanged();
                }

            }
        }
        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; this.OnPropertyChanged("IsActive"); }
        }
        

        private bool _IsAppliyForDays;
        public bool IsAppliyForDays
        {
            get { return _IsAppliyForDays; }
            set { _IsAppliyForDays = value; this.OnPropertyChanged("IsAppliyForDays"); }
        }

        #endregion

        #region New Method
        void New()
        {
            try
            {
                
                    CurrentEmployeeOTDetailView = null;
                    CurrentEmployeeOTDetailView = new EmployeeOTDetailLeaveView();
                    CurrentEmployee = null;
                    CurrentEmployee = new mas_Employee();
                    CurrentBasicDay = null;
                    CurrentBasicDay = new z_BasicDay();
                    CurrentOTCatergory = null;
                    CurrentOTCatergory = new z_OTCategory();
                    reafreshEmployeeOTDetailsView();
                    IsAppliyForDays = false;
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
            if (IsAppliyForDays == true)
            {
                try
                {
                    bool IsUpdate = false;

                    dtl_EmployeeOT newEmployeeOT = new dtl_EmployeeOT();
                    newEmployeeOT.employee_id = CurrentEmployeeOTDetailView.employee_id;
                    newEmployeeOT.ot_id = CurrentEmployeeOTDetailView.ot_id;
                    newEmployeeOT.day_id = CurrentEmployeeOTDetailView.day_id;
                    newEmployeeOT.ot_name = CurrentEmployeeOTDetailView.ot_name;
                    newEmployeeOT.is_active = IsActive;
                    newEmployeeOT.save_user_id = clsSecurity.loggedUser.user_id;
                    newEmployeeOT.save_datetime = System.DateTime.Now;
                    newEmployeeOT.modified_user_id = clsSecurity.loggedUser.user_id;
                    newEmployeeOT.modified_datetime = System.DateTime.Now;
                    newEmployeeOT.delete_user_id = clsSecurity.loggedUser.user_id;
                    newEmployeeOT.delete_datetime = System.DateTime.Now;
                    newEmployeeOT.isdelete = false;

                    IEnumerable<dtl_EmployeeOT> allEmployeeOTDetails = this.serviceClient.GetEmployeeOTDetails();

                    if (allEmployeeOTDetails != null)
                    {
                        foreach (var EmployeeOT in allEmployeeOTDetails)
                        {
                            if (EmployeeOT.employee_id == CurrentEmployeeOTDetailView.employee_id)
                            {
                                IsUpdate = true;
                            }
                        }
                    }
                    if (newEmployeeOT != null && newEmployeeOT.employee_id != null)
                    {
                        if (IsUpdate)
                        {
                            
                                newEmployeeOT.modified_user_id = clsSecurity.loggedUser.user_id;
                                newEmployeeOT.modified_datetime = System.DateTime.Now;

                                if (this.serviceClient.UpdateEmployeeOT(newEmployeeOT))
                                {
                                    MessageBox.Show("Record Update Successfully");
                                    clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                }
                                else
                                {
                                    MessageBox.Show("You Cannot Update This Record, This Record Attach With Other Oprations");
                                    clsMessages.setMessage(Properties.Resources.UpdateFail);
                                }
                            
                        }
                        else
                        {
                           
                                newEmployeeOT.save_user_id = clsSecurity.loggedUser.user_id;
                                newEmployeeOT.save_datetime = System.DateTime.Now;

                                if (this.serviceClient.SaveEmployeeOT(newEmployeeOT))
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
                        reafreshEmployeeOTDetailsView();
                        this.New();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                GetEmployeeSaveList();

                if (tempList.Count == 0)
                {
                    MessageBox.Show("This Ot Catagory Allredy assign For this employee");
                }
                else
                {
                    if (this.serviceClient.SaveEmployeeOTDetail(tempList.ToArray()))
                    {
                        MessageBox.Show("Save Successfully");
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                    }
                    else
                    {
                        MessageBox.Show("Record SaveFailed");
                        clsMessages.setMessage(Properties.Resources.SaveFail);
                    }

                }
            }
        }
        
        #endregion

        #region SaveButton Class & Property
        bool saveCanExecute()
        {
            if (CurrentEmployee != null && CurrentOTCatergory !=null)
            {
                if (CurrentEmployee.employee_id == null || CurrentEmployee.employee_id == Guid.Empty)
                    return false;
                if (CurrentOTCatergory.ot_id == null || CurrentOTCatergory.ot_id == Guid.Empty)
                    return false;
                if (CurrentOTCatergory.name == null || CurrentOTCatergory.name == string.Empty)
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
            { return new RelayCommand(Save, saveCanExecute); }
        }
        #endregion

        #region Delete Method
        void Delete()
        {
            try
            {
               
                    MessageBoxResult result = new MessageBoxResult();
                    result = MessageBox.Show("Do You To Delete This Record...?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        EmployeeOTDetailLeaveView EmployeeOT = new EmployeeOTDetailLeaveView();
                        EmployeeOT.employee_id = CurrentEmployeeOTDetailView.employee_id;
                        EmployeeOT.delete_user_id = clsSecurity.loggedUser.user_id;
                        EmployeeOT.delete_datetime = System.DateTime.Now;

                        if (this.serviceClient.DeleteEmployeeOT(EmployeeOT))
                        {
                            MessageBox.Show("Record Deleted");
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            reafreshEmployeeOTDetailsView();
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
            }
        }
        #endregion

        #region DeleteButton Class & Property
        bool deleteCanexecute()
        {
            if (CurrentEmployeeOTDetailView == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanexecute);
            }
        }
        #endregion

        #region OT Details View List
        private void reafreshEmployeeOTDetailsView()
        {
            this.serviceClient.GetEmployeeOTViewDetailsCompleted += (s, e) =>
                {
                    EmployeeList = new List<EmployeeOTDetailLeaveView>();
                    this.EmployeeOTDetailView = e.Result.Where(k => k.isdelete == false);
                    foreach (var item in EmployeeOTDetailView)
                    {
                        EmployeeList.Add(item);
                    }
                };
            this.serviceClient.GetEmployeeOTViewDetailsAsync();
        }
        #endregion

        #region Employee List
        private void reafreshEmployees()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(p => p.isdelete == false);
            };
            this.serviceClient.GetEmployeesAsync();
        }
        #endregion

        #region Basic Days List
        private void reafreshBasicDays()
        {
            this.serviceClient.GetBasicDaysCompleted += (s, e) =>
            {
                this.BasicDays = e.Result.Where(d => d.isdelete == false);
            };
            this.serviceClient.GetBasicDaysAsync();
        }
        #endregion

        #region OT Catergories List
        private void reafreshOTCatergories()
        {
            this.serviceClient.GetOTCatagoriesCompleted += (s, e) =>
                {
                    this.OTCatergories = e.Result.Where(p => p.isdelete == false);
                };
            this.serviceClient.GetOTCatagoriesAsync();
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
            EmployeeOTDetailView = EmployeeOTDetailView.Where(e => e.ot_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion

        public void FilterEmployee()
        {
            try
            {
                if (EmployeeList != null && CurrentEmployee != null)
                {
                    EmployeeOTDetailView = EmployeeList.Where(r => r.employee_id == CurrentEmployee.employee_id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void GetEmployeeSaveList()
        {
            if (CurrentEmployee != null || CurrentEmployee.employee_id != Guid.Empty)
            {

                foreach (var item in BasicDays)
                {
                    EmployeeOTDetailLeaveView OtEmployee = EmployeeOTDetailView.FirstOrDefault(z => z.employee_id == CurrentEmployee.employee_id && z.day_id == item.day_id);
                    if (OtEmployee == null)
                    {
                        dtl_EmployeeOT tempEmployee = new dtl_EmployeeOT();
                        tempEmployee.employee_id = CurrentEmployee.employee_id;
                        tempEmployee.day_id = item.day_id;
                        tempEmployee.ot_id = CurrentOTCatergory.ot_id;
                        tempEmployee.ot_name = CurrentOTCatergory.name;
                        tempEmployee.is_active = IsActive;
                        tempEmployee.save_user_id = clsSecurity.loggedUser.user_id;
                        tempEmployee.save_datetime = System.DateTime.Now;
                        tempEmployee.modified_datetime = System.DateTime.Now;
                        tempEmployee.modified_user_id = clsSecurity.loggedUser.user_id;
                        tempEmployee.delete_datetime = System.DateTime.Now;
                        tempEmployee.delete_user_id = clsSecurity.loggedUser.user_id;
                        tempEmployee.isdelete = false;
                        tempList.Add(tempEmployee);

                    }

                }
            }
        }
    }
}
