using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;

namespace ERP
{
    class EmployeeMasterViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient = new ERPServiceClient(); 
        #endregion

        #region Constructor
        public EmployeeMasterViewModel()
        {
                this.refreshEmployees();
                this.refershGender();
                this.refreshCivilState();
                this.refreshTown();
                this.refreshCity();
                this._SaveBtn = new SaveButton(this);
                this._NewBtn = new NewButton(this);
                this._DeleteBtn = new DeleteButton(this);
        }         
        #endregion

        #region Properties {get,set}
        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return this._Employees; }
            set { this._Employees = value; OnPropertyChanged("Employees"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return this._CurrentEmployee; }
            set
            {
                this._CurrentEmployee = value; OnPropertyChanged("CurrentEmployee");
                clsConfig.CurrentEmployee = CurrentEmployee;
                if (CurrentEmployee != null && CurrentTown != null && CurrentCity != null)
                {

                    this.Fullname = CurrentEmployee.initials + " " + CurrentEmployee.first_name + " " + CurrentEmployee.second_name;
                    this.FullAddress = CurrentEmployee.address_01 + ",\n" + CurrentEmployee.address_02 + ", \n" + CurrentEmployee.address_03 + ", \n" + CurrentTown.town_name + ", \n" + CurrentCity.city + ".";
                }
            }
        }

        private z_Gender _CurrentGender;
        public z_Gender CurrentGender
        {
            get { return this._CurrentGender; }
            set { this._CurrentGender = value; OnPropertyChanged("CurrentGender"); }
        }

        private z_CivilState _CurrentCivilState;
        public z_CivilState CurrentCivilState
        {
            get { return this._CurrentCivilState; }
            set { this._CurrentCivilState = value; OnPropertyChanged("CurrentCivilState"); }
        }

        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get { return this._CurrentTown; }
            set { this._CurrentTown = value; OnPropertyChanged("CurrentTown"); }
        }

        private z_City _CurrntCity;
        public z_City CurrentCity
        {
            get
            {
                return this._CurrntCity;
            }
            set
            {
                this._CurrntCity = value;
                OnPropertyChanged("CurrentCity");
            }
        }

        private String _FullName;
        public String Fullname
        {
            get { return this._FullName; }
            set { this._FullName = value; OnPropertyChanged("Fullname"); }
        }

        private String _FullAddress;
        public String FullAddress
        {
            get { return this._FullAddress; }
            set { this._FullAddress = value; OnPropertyChanged("FullAddress"); }
        }

        private IEnumerable<z_City> _Cities;
        public IEnumerable<z_City> Cities
        {
            get { return this._Cities; }
            set { this._Cities = value; OnPropertyChanged("Cities"); }
        }

        private IEnumerable<z_Town> _Towns;
        public IEnumerable<z_Town> Towns
        {
            get { return this._Towns; }
            set { this._Towns = value; OnPropertyChanged("Towns"); }
        }

        private IEnumerable<z_Gender> _Genders;
        public IEnumerable<z_Gender> Genders
        {
            get { return this._Genders; }
            set { this._Genders = value; OnPropertyChanged("Genders"); }
        }

        private IEnumerable<z_CivilState> _CivilStates;
        public IEnumerable<z_CivilState> CivilStates
        {
            get { return this._CivilStates; }
            set { this._CivilStates = value; OnPropertyChanged("CivilStates"); }
        }

        #endregion

        #region Button Properties
        private SaveButton _SaveBtn;
        public SaveButton SaveBtn
        {
            get { return this._SaveBtn; }
        }

        private NewButton _NewBtn;
        public NewButton NewBtn
        {
            get { return this._NewBtn; }
        }

        private DeleteButton _DeleteBtn;
        public DeleteButton DeleteBtn
        {
            get { return this._DeleteBtn; }
        } 
        #endregion

        #region Button Classes
        public class SaveButton : ICommand
        {
            private EmployeeMasterViewModel ViewModel;

            public SaveButton(EmployeeMasterViewModel ViewModel)
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
                return ViewModel.CurrentEmployee != null;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.saveRecord();
            }
        }

        public class NewButton : ICommand
        {

            private EmployeeMasterViewModel ViewModel;

            public NewButton(EmployeeMasterViewModel ViewModel)
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

        public class DeleteButton : ICommand
        {
            private EmployeeMasterViewModel ViewModel;

            public DeleteButton(EmployeeMasterViewModel ViewModel)
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
                return this.ViewModel.CurrentEmployee != null;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.deleteRecode();
            }
        } 
        #endregion

        #region User Define Method
        private void saveRecord()
        {
            if (validation())
            {
                bool isUpdate = false;

                mas_Employee newEmployee = new mas_Employee();
                newEmployee = CurrentEmployee;
                newEmployee.city_id = CurrentCity.city_id;
                newEmployee.town_id = CurrentTown.town_id;
                newEmployee.civil_status_id = CurrentCivilState.civi_status_id;
                newEmployee.gender_id = CurrentGender.gender_id;
                if (newEmployee != null)
                {
                    IEnumerable<mas_Employee> allEmployees = this.serviceClient.GetEmployees();

                    foreach (mas_Employee user in allEmployees)
                    {
                        if (user.employee_id == CurrentEmployee.employee_id)
                        {
                            isUpdate = true;
                        }
                    }

                    if (isUpdate)
                    {
                        if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.Employee), clsSecurity.loggedUser.user_id))
                        {
                            newEmployee.modified_datetime = DateTime.Now;
                            newEmployee.modified_user_id = clsSecurity.loggedUser.user_id;

                            bool isSucess = this.serviceClient.UpdateEmployee(newEmployee,new DateTime(),new DateTime());
                            if (isSucess)
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
                        if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.Employee), clsSecurity.loggedUser.user_id))
                        {
                            newEmployee.save_datetime = DateTime.Now;
                            newEmployee.save_user_id = clsSecurity.loggedUser.user_id;
                            bool isSucess = this.serviceClient.SaveEmployee(newEmployee, new DateTime(), new DateTime());
                            if (isSucess)
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
                    refreshEmployees();
                    newRecord();
                }
            }
        }

        private void newRecord()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.Employee), clsSecurity.loggedUser.user_id))
            {
                dtl_Employee CurrentEmployee = new dtl_Employee();
                CurrentEmployee.employee_id = Guid.NewGuid();
                refreshEmployees();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void deleteRecode()
        {
            //MessageBoxResult result = MessageBox.Show("Do you want to delete this record ?", "ERP", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    try
            //    {
            //        bool isSucess = this.serviceClient.DeleteEmployee(CurrentEmployee);
            //        if (isSucess)
            //        {
            //            clsMessages.setMessage(Properties.Resources.DeleteSucess);
            //        }
            //        else
            //        {
            //            clsMessages.setMessage(Properties.Resources.DeleteFail);
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
        }

        private bool validation()
        {
            return true;
        } 

        #endregion

        #region Refresh Methods

        private void refreshEmployees()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
                {
                    this.Employees = e.Result;
                };
            this.serviceClient.GetEmployeesAsync();
        }

        private void refershGender()
        {
            this.serviceClient.getGendersCompleted += (s, e) =>
                {
                    this.Genders = e.Result;
                    Genders = Genders.Where(z=> !z.gender_id.Equals(new Guid()));
                };
            this.serviceClient.getGendersAsync();
        }

        private void refreshCity()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
            {
                this.Cities = e.Result;
            };
            this.serviceClient.GetCitiesAsync();
        }

        private void refreshTown()
        {
            this.serviceClient.GetTownsCompleted += (s, e) =>
            {
                this.Towns = e.Result;
            };
            this.serviceClient.GetTownsAsync();
        }

        private void refreshCivilState()
        {
            this.serviceClient.getCivilStatesCompleted += (s, e) =>
            {
                this.CivilStates = e.Result;
            };
            this.serviceClient.getCivilStatesAsync();
        } 

        #endregion
    }
}
