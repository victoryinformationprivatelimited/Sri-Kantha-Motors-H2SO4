using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ERP.BasicSearch;

namespace ERP.MastersDetails
{
    class EmployeeAttendanceViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public EmployeeAttendanceViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeShift), clsSecurity.loggedUser.user_id))
            {
                this.reafreshEmployeeAttendanceViewList();
                this.reafreshEmployees();
                reafreshShiftCatagory();
                reafreshRosterCatagory();
                this.New();
                IsRoster = false;
                IsOtapplicable = false;
                IsLeaveApplicable = false;
                IsShift = true;
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        } 
        #endregion

        #region Properties
        private IEnumerable<EmployeeAttendanceView> _EmployeeAttendanceView;
        public IEnumerable<EmployeeAttendanceView> EmployeeAttendanceView
        {
            get
            {
                return this._EmployeeAttendanceView;
            }
            set
            {
                this._EmployeeAttendanceView = value;
                this.OnPropertyChanged("EmployeeAttendanceView");
                if (CurrentEmployeeAttendaceView != null)
                {
                    CurrentEmployeeAttendaceView = EmployeeAttendanceView.FirstOrDefault(z => z.employee_id == CurrentEmployeeAttendaceView.employee_id);
                }
            }
        }

        private EmployeeAttendanceView _CurrentEmployeeAttendaceView;
        public EmployeeAttendanceView CurrentEmployeeAttendaceView
        {
            get
            {
                return this._CurrentEmployeeAttendaceView;
            }
            set
            {
                this._CurrentEmployeeAttendaceView = value;
                this.OnPropertyChanged("CurrentEmployeeAttendaceView");
               
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
            }
        }

       
        private IEnumerable<z_ShiftCategory> _ShiftCatagorys;
        public IEnumerable<z_ShiftCategory> ShiftCatagorys
        {
            get { return _ShiftCatagorys; }
            set { _ShiftCatagorys = value; this.OnPropertyChanged("ShiftCatagorys"); }
        }

        private z_ShiftCategory _CurrentShiftCatagory;
        public z_ShiftCategory CurrentShiftCatagory
        {
            get { return _CurrentShiftCatagory; }
            set { _CurrentShiftCatagory = value; this.OnPropertyChanged("CurrentShiftCatagory"); }
        }

        private IEnumerable<z_RosterMaster> _RosterCatagory;
        public IEnumerable<z_RosterMaster> RosterCatagory
        {
            get { return _RosterCatagory; }
            set { _RosterCatagory = value; this.OnPropertyChanged("RosterCatagory"); }
        }

        private z_RosterMaster _CurretRosterCatagory;
        public z_RosterMaster CurretRosterCatagory
        {
            get { return _CurretRosterCatagory; }
            set { _CurretRosterCatagory = value; this.OnPropertyChanged("CurretRosterCatagory"); }
        }

        private bool _IsRoster;
        public bool IsRoster
        {
            get { return _IsRoster; }
            set { _IsRoster = value; this.OnPropertyChanged("IsRoster");
            if (IsRoster == true)
            {
                IsShift = false;
            }
            if (IsRoster == false)
            {
                IsShift = true;
            }
                   
            
            }
        }

        private bool _IsShift;
        public bool IsShift
        {
            get { return _IsShift; }
            set { _IsShift = value; this.OnPropertyChanged("IsShift");
           
            }
        }
        

        private bool _IsLeaveApplicable;
        public bool IsLeaveApplicable
        {
            get { return _IsLeaveApplicable; }
            set { _IsLeaveApplicable = value; this.OnPropertyChanged("IsLeaveApplicable"); }
        }

        private bool _IsOtapplicable;
        public bool IsOtapplicable
        {
            get { return _IsOtapplicable; }
            set { _IsOtapplicable = value; this.OnPropertyChanged("IsOtapplicable"); }
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
                    this.reafreshEmployeeAttendanceViewList();
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
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeShift), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    CurrentEmployeeAttendaceView = null;
                    CurrentEmployeeAttendaceView = new EmployeeAttendanceView();
                    CurrentEmployee = null;
                    CurrentEmployeeAttendaceView.shift_catagory_id = Guid.Empty;
                    CurrentEmployeeAttendaceView.roster_detail_id = Guid.Empty;
                    reafreshEmployeeAttendanceViewList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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

            try
            {
                bool IsSave = false;

                dtl_EmployeeAttendance newAttendance = new dtl_EmployeeAttendance();
                newAttendance.employee_id = CurrentEmployeeAttendaceView.employee_id;
                newAttendance.shift_catagory_id = CurrentEmployeeAttendaceView.shift_catagory_id;
                newAttendance.roster_detail_id = CurrentEmployeeAttendaceView.roster_detail_id;
                newAttendance.is_roster_employee = IsRoster;
                newAttendance.summery_name = CurrentEmployeeAttendaceView.summery_name;
                newAttendance.is_leave_applicable = CurrentEmployeeAttendaceView.is_leave_applicable;
                newAttendance.is_ot_applicable = CurrentEmployeeAttendaceView.is_ot_applicable;
                newAttendance.save_user_id = clsSecurity.loggedUser.user_id;
                newAttendance.save_datetime = System.DateTime.Now;
                newAttendance.modified_user_id = clsSecurity.loggedUser.user_id;
                newAttendance.modified_datetime=System.DateTime.Now;
                newAttendance.delete_user_id = clsSecurity.loggedUser.user_id;
                newAttendance.delete_datetime = System.DateTime.Now;
                newAttendance.isdelete = false;

                IEnumerable<dtl_EmployeeAttendance> allAttendance = this.serviceClient.GetEmployeeAttendance();

                if (allAttendance != null)
                {
                    foreach (var Attendance in allAttendance)
                    {
                        if (Attendance.employee_id == CurrentEmployeeAttendaceView.employee_id)
                        {
                            IsSave = true;
                        }
                    }
                }
                if (newAttendance != null)
                {
                    if (IsSave == true)
                    {
                        if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.EmployeeShift), clsSecurity.loggedUser.user_id))
                        {
                            newAttendance.modified_user_id = clsSecurity.loggedUser.user_id;
                            newAttendance.modified_datetime = System.DateTime.Now;
                            if (this.serviceClient.UpdateEmployeeAttendance(newAttendance))
                            {
                                MessageBox.Show("Update Successfully ");
                            }
                            else
                            {
                                MessageBox.Show("Update Fail ");
                            }
                        }
                    }
                    else
                    {
                        if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.EmployeeShift), clsSecurity.loggedUser.user_id))
                        {
                            newAttendance.save_user_id = clsSecurity.loggedUser.user_id;
                            newAttendance.save_datetime = System.DateTime.Now;

                            if (this.serviceClient.SaveEmployeeAttendance(newAttendance))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                reafreshEmployeeAttendanceViewList();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            }

                        }
                    }
                    reafreshEmployeeAttendanceViewList();
                    this.New();
                }
                else
                {
                    clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
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
            if (CurrentEmployeeAttendaceView != null)
            {

            }
            else
            {

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
                if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.EmployeeShift), clsSecurity.loggedUser.user_id))
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result = MessageBox.Show("Do You Want To Delete This Record..?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        EmployeeAttendanceView EmployeeAttendance = new EmployeeAttendanceView();
                        EmployeeAttendance.employee_id = CurrentEmployeeAttendaceView.employee_id;
                        EmployeeAttendance.delete_user_id = clsSecurity.loggedUser.user_id;
                        EmployeeAttendance.delete_datetime = System.DateTime.Now;

                        //if (this.serviceClient.DeleteEmployeeAttendance(EmployeeAttendance))
                        //{
                        //    MessageBox.Show("Record Deleted");
                        //    clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        //    reafreshEmployeeAttendanceViewList();
                        //    this.New();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Record Delete Failed");
                        //    clsMessages.setMessage(Properties.Resources.DeleteFail);
                        //}
                    }
                }
                else
                {
                    clsMessages.setMessage(Properties.Resources.NoPermissionForDelete);
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
            if (CurrentEmployeeAttendaceView == null)
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

        #region EmployeeAttendanceView List
        private void reafreshEmployeeAttendanceViewList()
        {
            this.serviceClient.GetEmployeeAttendanceViewCompleted += (s, e) =>
                {
                    this.EmployeeAttendanceView = e.Result.Where(z=>z.isdelete==false);
                    CurrentEmployeeAttendaceView = new EmployeeAttendanceView();
                };
            this.serviceClient.GetEmployeeAttendanceViewAsync();
        } 
        #endregion

        #region Employee List
        private void reafreshEmployees()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(z=>z.isdelete==false);
            };
            this.serviceClient.GetEmployeesAsync();
        }
        #endregion

        #region Detail Rosters List
        private void reafreshShiftCatagory()
        {
            this.serviceClient.GetShiftcategoryCompleted += (s, e) =>
                {
                    this.ShiftCatagorys = e.Result.Where(z=>z.isdelete==false && z.is_active==true);
                };
            this.serviceClient.GetShiftcategoryAsync();
        }
        #endregion

        #region Detail Shifts List
        private void reafreshRosterCatagory()
        {
            this.serviceClient.GetMonthlyRostersCompleted += (s, e) =>
                {
                    this.RosterCatagory = e.Result.Where(z=>z.isdelete==false && z.is_active==true);
                };
            this.serviceClient.GetMonthlyRostersAsync();
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
            EmployeeAttendanceView = EmployeeAttendanceView.Where(e => e.summery_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion

             private EmployeeSearchView selectedEmployee;
             public EmployeeSearchView SelectedEmployee
             {
                 get { return selectedEmployee; }
                 set { selectedEmployee = value; }
             }

        #region Employee Search

        private void EmployeeSearchButton()
        {
            EmployeeSearchWindow searchWindow = new EmployeeSearchWindow();
            searchWindow.ShowDialog();
            if (searchWindow.viewModel.CurrentEmployeeSearchView !=null)
                this.CurrentEmployee = Employees.FirstOrDefault(z => z.employee_id == searchWindow.viewModel.CurrentEmployeeSearchView.employee_id);
            searchWindow.Close();
           
        }

        public ICommand EmployeeSearch
        {
            get { return new RelayCommand(EmployeeSearchButton); }
        }
        #endregion
    }
}
