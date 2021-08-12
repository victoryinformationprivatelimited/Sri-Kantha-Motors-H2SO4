using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Security
{
    class EmployeeManagerViewModel : ViewModelBase
    {
        #region ServiceCilent

        ERPServiceClient serviceClient;
        #endregion

        #region List
        List<dtl_EmployeeSupervisor> EmpManger = new List<dtl_EmployeeSupervisor>();
        #endregion

        #region Constructor
        public EmployeeManagerViewModel()
        {
            serviceClient = new ERPServiceClient();
            //refreshSupervisorEmployees();
            refreshEmployeeSearch();
            refreshModules();
            refreshSupervisorLevels();
            IsRefreshed = false;
            LevelEnabled = true;
            IsOnlySupervisors = true;

        }
        #endregion

        #region Properties

        private bool _LevelEnabled;
        public bool LevelEnabled
        {
            get { return _LevelEnabled; }
            set { _LevelEnabled = value; OnPropertyChanged("LevelEnabled"); }
        }


        private bool _IsRefreshed;
        public bool IsRefreshed
        {
            get { return _IsRefreshed; }
            set { _IsRefreshed = value; OnPropertyChanged("IsRefreshed"); }
        }

        private bool _IsOnlySupervisors;
        public bool IsOnlySupervisors
        {
            get { return _IsOnlySupervisors; }
            set { _IsOnlySupervisors = value; OnPropertyChanged("IsOnlySupervisors"); }
        }
        

        private EmployeeSearchView _currentEmployee;
        public EmployeeSearchView CurrentEmployee
        {
            get { return _currentEmployee; }
            set { _currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<dtl_EmployeeSupervisor> _employeeManager;
        public IEnumerable<dtl_EmployeeSupervisor> EmployeeManager
        {
            get { return _employeeManager; }
            set { _employeeManager = value; OnPropertyChanged("EmployeeManager"); }
        }

        private IEnumerable<EmployeeSearchView> _employeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _employeeSearch; }
            set { _employeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        private string _managerName;
        public string ManagerName
        {
            get { return _managerName; }
            set { _managerName = value; OnPropertyChanged("ManagerName"); }
        }

        private EmployeeSearchView _currentManager;
        public EmployeeSearchView CurrentManager
        {
            get { return _currentManager; }
            set { _currentManager = value; OnPropertyChanged("CurrentManager"); }
        }

        private List<EmployeeSearchView> _selectedEmployee = new List<EmployeeSearchView>();
        public List<EmployeeSearchView> SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged("SelectedEmployee"); if (SelectedEmployee != null && SelectedEmployee.Count() > 0) LevelEnabled = false; else LevelEnabled = true; }
        }

        private IEnumerable<z_SupervisorLevel> _SupervisorLevels;
        public IEnumerable<z_SupervisorLevel> SupervisorLevels
        {
            get { return _SupervisorLevels; }
            set { _SupervisorLevels = value; OnPropertyChanged("SupervisorLevels"); }
        }

        private z_SupervisorLevel _CurrentSupervisorLevel;
        public z_SupervisorLevel CurrentSupervisorLevel
        {
            get { return _CurrentSupervisorLevel; }
            set { _CurrentSupervisorLevel = value; OnPropertyChanged("CurrentSupervisorLevel"); }
        }

        private IEnumerable<z_module> _Modules;
        public IEnumerable<z_module> Modules
        {
            get { return _Modules; }
            set { _Modules = value; OnPropertyChanged("Modules"); }
        }

        private z_module _CurrentModule;
        public z_module CurrentModule
        {
            get { return _CurrentModule; }
            set { _CurrentModule = value; OnPropertyChanged("CurrentModule"); if (CurrentModule != null) { SelectedEmployee = null; ManagerName = null; CurrentSupervisorLevel = null; EmpManger.Clear(); IsRefreshed = false; } }
        }


        #endregion

        #region Manager

        public ICommand BtnGetModuleManagerEmp
        {
            get { return new RelayCommand(refreshSupervisorEmployees, refreshSupervisorEmployeesCE); }
        }

        private bool refreshSupervisorEmployeesCE()
        {
            if (CurrentModule != null)
                return true;
            else
                return false;
        }


        #region Manager Button
        public ICommand ManagerSelect
        {
            get { return new RelayCommand(managaerSelect, managaerSelectCE); }
        }

        private bool managaerSelectCE()
        {
            if (CurrentModule != null && IsRefreshed != false)
                return true;
            else
                return false;
        }

        #endregion

        #region Manager Method

        void managaerSelect()
        {
            EmployeeSearchWindow window = IsOnlySupervisors == true ? (EmpManger.Count == 0 ? new EmployeeSearchWindow() : new EmployeeSearchWindow(EmpManger.Select(c => c.supervisor_employee_id).Distinct().ToList())) : new EmployeeSearchWindow();
            window.ShowDialog();
            if (window.viewModel.CurrentEmployeeSearchView != null)
                CurrentManager = window.viewModel.CurrentEmployeeSearchView;
            window.Close();
            window = null;
            if (CurrentManager != null)
            {
                ManagerName = CurrentManager.first_name + " " + CurrentManager.surname;
                EmployeeManager = null;
                EmployeeManager = EmpManger.Where(c => c.supervisor_employee_id == CurrentManager.employee_id);
                if (SupervisorLevels != null && SupervisorLevels.Count() > 0 && EmpManger != null && EmpManger.Count > 0)
                {
                    CurrentSupervisorLevel = null;
                    CurrentSupervisorLevel = SupervisorLevels.FirstOrDefault(c => c.supervisor_leve_id == (EmpManger.FirstOrDefault(d => d.supervisor_employee_id == CurrentManager.employee_id) == null ? Guid.Empty : EmpManger.FirstOrDefault(d => d.supervisor_employee_id == CurrentManager.employee_id).supervisor_level_id));
                }


                List<EmployeeSearchView> temp = new List<EmployeeSearchView>();
                foreach (var item in EmployeeManager.ToList())
                {
                    temp.Add(EmployeeSearch.Where(c => c.employee_id == item.employee_id).FirstOrDefault());
                }

                #region MCN
                if (temp.Any())
                {
                    temp.RemoveAll(item => item == null);
                }
                #endregion

                SelectedEmployee = null;
                SelectedEmployee = temp;
            }
        }

        #endregion
        #endregion

        #region Employee
        #region Employee Button

        public ICommand EmployeeButton
        {
            get { return new RelayCommand(emloyeesSelect, employeeCanExecute); }
        }
        bool employeeCanExecute()
        {
            if (CurrentManager != null && CurrentSupervisorLevel != null)
                return true;
            else
                return false;
        }

        #endregion

        #region Employee Select

        void emloyeesSelect()
        {
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.SelectedList != null)
                if (SelectedEmployee != null && SelectedEmployee.Count() > 0)
                {
                    List<EmployeeSearchView> temp = new List<EmployeeSearchView>();
                    temp = SelectedEmployee;
                    foreach (EmployeeSearchView item in window.viewModel.SelectedList.ToList())
                    {
                        if (SelectedEmployee.Count(c => c.employee_id == item.employee_id) == 0 && item.employee_id != CurrentManager.employee_id)
                            temp.Add(item);
                    }
                    SelectedEmployee = null;
                    SelectedEmployee = temp;
                    temp = null;
                }
                else
                    SelectedEmployee = window.viewModel.SelectedList.Where(c => c.employee_id != CurrentManager.employee_id).ToList();
            window.Close();
            window = null;
        }

        #endregion
        #endregion

        #region Save

        #region SaveButton
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanExcute); }
        }
        #endregion

        bool SaveCanExcute()
        {
            if (SelectedEmployee == null)
                return false;
            else if (SelectedEmployee.Count == 0)
                return false;
            else
                return true;
        }

        #region Save Method

        void Save()
        {
            if (CurrentSupervisorLevel != null)
            {
                List<dtl_EmployeeSupervisor> saveList = new List<dtl_EmployeeSupervisor>();

                foreach (var SelectedEmployeeItem in SelectedEmployee)
                {
                    dtl_EmployeeSupervisor save = new dtl_EmployeeSupervisor();
                    save.employee_id = (Guid)SelectedEmployeeItem.employee_id;
                    save.supervisor_employee_id = (Guid)CurrentManager.employee_id;
                    save.supervisor_level_id = CurrentSupervisorLevel.supervisor_leve_id;
                    save.module_id = CurrentModule.module_id;
                    if (EmployeeManager.Count(c => c.employee_id == save.employee_id && c.supervisor_employee_id == save.supervisor_employee_id) == 0)
                        saveList.Add(save);
                }
                if (saveList != null && saveList.Count > 0)
                {
                    try
                    {
                        if (clsSecurity.GetUpdatePermission(105) && clsSecurity.GetSavePermission(105))
                        {
                            if (serviceClient.SaveEmployeeManager(saveList.ToArray()))
                            {
                                refreshSupervisorEmployees();
                                refreshEmployeeSearch();
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                clsSecurity.moduleSupervision = this.serviceClient.GetAllEmployeeSupervisorByUserEmployee(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id);

                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            } 
                        }
                        else
                        {
                            clsMessages.setMessage("You don't have permission to Save or Update in this form...");
                        }
                    }
                    catch (Exception)
                    {

                        clsMessages.setMessage("Error");
                    }
                }
                else
                    clsMessages.setMessage("Nothing to Save");
            }
            else
                clsMessages.setMessage("Please select a supervisor level");

        }
        #endregion

        #endregion

        #region Delete
        #region DeleteButton
        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }
        bool DeleteCanExecute()
        {
            if (CurrentEmployee == null)
                return false;
            else
                return true;
        }
        #endregion

        #region Delete Method
        void Delete()
        {
            if (clsSecurity.GetDeletePermission(105))
            {
                if (CurrentEmployee != null)
                {
                    List<dtl_EmployeeSupervisor> deleteList = new List<dtl_EmployeeSupervisor>();
                    dtl_EmployeeSupervisor temp = new dtl_EmployeeSupervisor();

                    temp = EmployeeManager.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id);

                    if (temp != null && temp.employee_id != null && temp.employee_id != Guid.Empty)
                    {
                        clsMessages.setMessage("Do you want to Delete From the DataBase");
                        if (clsMessages.Messagebox.viewModel.Result == "OK")
                        {
                            deleteList.Add(temp);
                            try
                            {
                                if (serviceClient.DeleteEmployeeManger(deleteList.ToArray()))
                                {
                                    clsMessages.setMessage(Properties.Resources.DeleteSucess);
                                    SelectedEmployee.Remove(CurrentEmployee);
                                    refreshSupervisorEmployees();
                                    refreshEmployeeSearch();
                                    clsSecurity.moduleSupervision = this.serviceClient.GetAllEmployeeSupervisorByUserEmployee(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id);
                                }
                                else
                                {
                                    clsMessages.setMessage(Properties.Resources.DeleteFail);
                                }
                            }
                            catch (Exception)
                            {

                                clsMessages.setMessage("Error");
                            }
                        }
                    }
                    else
                    {
                        SelectedEmployee.Remove(CurrentEmployee);
                    }
                    List<EmployeeSearchView> selecttemp = new List<EmployeeSearchView>();
                    selecttemp = SelectedEmployee;
                    SelectedEmployee = null;
                    SelectedEmployee = selecttemp;
                    selecttemp = null;
                } 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete in this form...");
            }
        }
        #endregion
        #endregion

        #region New

        #region New Method

        void New()
        {
            CurrentEmployee = null;
            CurrentManager = null;
            ManagerName = null;
            CurrentSupervisorLevel = null;
            SelectedEmployee = null;
            SelectedEmployee = new List<EmployeeSearchView>();
            refreshSupervisorEmployees();
            refreshEmployeeSearch();
        }

        #endregion

        #region New Button

        public ICommand Newbtn
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #endregion

        #region Refresh Mehod

        void refreshSupervisorEmployees()
        {
            try
            {
                //serviceClient.GetEmployeeManagerByModuleCompleted += (s, e) =>
                //{
                //    EmpManger.Clear();
                //    EmployeeManager = e.Result;
                //    if (EmployeeManager != null)
                //        EmpManger = EmployeeManager.ToList();
                //    IsRefreshed = true;
                //};
                //serviceClient.GetEmployeeManagerByModuleAsync(CurrentModule.module_id);

                EmpManger.Clear();
                EmployeeManager = serviceClient.GetEmployeeManagerByModule(CurrentModule.module_id);
                if (EmployeeManager != null && EmployeeManager.Count()>0)
                    EmpManger = EmployeeManager.ToList();
                else
                    clsMessages.setMessage("No supervisors found for the selected module.");
                IsRefreshed = true;

            }
            catch (Exception)
            {
                clsMessages.setMessage("Refresh Error");
            }

        }

        void refreshEmployeeSearch()
        {
            try
            {
                serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
                {
                    EmployeeSearch = e.Result;
                };
                serviceClient.GetEmloyeeSearchAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Refresh Error");
            }
        }

        void refreshSupervisorLevels()
        {
            serviceClient.GetEmpoyeeSupervisorLevelsCompleted += (s, e) =>
            {
                try
                {
                    SupervisorLevels = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmpoyeeSupervisorLevelsAsync();
        }

        void refreshModules()
        {
            serviceClient.GetModulesCompleted += (s, e) =>
            {
                try
                {
                    Modules = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetModulesAsync();
        }

        #endregion

    }
}
