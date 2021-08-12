using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ERP.Attendance.Rosters
{
    class RosterGroupViewModel : ViewModelBase
    {
        #region Service Client
        private ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<EmployeeRosterDetailView> listEmployees = new List<EmployeeRosterDetailView>();
        List<EmployeeRosterDetailView> listTempEmp = new List<EmployeeRosterDetailView>();
        List<EmployeesForRosterView> listEmp = new List<EmployeesForRosterView>();
        #endregion

        #region Constructor
        public RosterGroupViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.RosterGroup), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    serviceClient = new ERPServiceClient();
                    RefresDesignations();
                    RefreshEmployeesForRosterView();
                    New();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in connecting server [" + ex.ToString() + "]", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }
        #endregion

        #region Properties
        private IEnumerable<z_RosterGroup> rosterGroups;
        public IEnumerable<z_RosterGroup> RosterGroups
        {
            get { return rosterGroups; }
            set { rosterGroups = value; OnPropertyChanged("RosterGroups"); }
        }

        private z_RosterGroup currentRosterGroup;
        public z_RosterGroup CurrentRosterGroup
        {
            get { return currentRosterGroup; }
            set { currentRosterGroup = value; OnPropertyChanged("CurrentRosterGroup"); FilterEmployees(); }
        }

        private IEnumerable<EmployeesForRosterView> employees;
        public IEnumerable<EmployeesForRosterView> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeesForRosterView currentEmployees;
        public EmployeesForRosterView CurrentEmployees
        {
            get { return currentEmployees;}
            set { currentEmployees = value; OnPropertyChanged("CurrentEmployees"); }
        }

        private IEnumerable<EmployeeRosterDetailView> rosterGroupEmployees;
        public IEnumerable<EmployeeRosterDetailView> RosterGroupEmployees
        {
            get { return rosterGroupEmployees; }
            set { rosterGroupEmployees = value; OnPropertyChanged("RosterGroupEmployees"); }
        }

        private EmployeeRosterDetailView currentRosterGroupEmployee;
        public EmployeeRosterDetailView CurrentRosterGroupEmployee
        {
            get { return currentRosterGroupEmployee; }
            set { currentRosterGroupEmployee = value; OnPropertyChanged("CurrentRosterGroupEmployee"); }
        }

        private IEnumerable<z_Designation> designations;
        public IEnumerable<z_Designation> Designations
        {
            get { return designations; }
            set { designations = value; OnPropertyChanged("Designations"); }
        }

        private z_Designation currentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return currentDesignation;}
            set { currentDesignation = value; OnPropertyChanged("CurrentDesignation"); FilterEmployeeByDesignation(); }
        }
        #endregion

        #region Add Method
        private bool addCanExecute()
        {
            if (CurrentRosterGroup == null)
                return false;
            if (CurrentEmployees == null)
                return false;
            return true;
        }

        private void Add()
        {
            if (listTempEmp.Count(c => c.emp_id == CurrentEmployees.emp_id) == 0)
            {
                EmployeeRosterDetailView rs = new EmployeeRosterDetailView();
                rs.employee_id = CurrentEmployees.employee_id;
                rs.emp_id = CurrentEmployees.emp_id;
                rs.first_name = CurrentEmployees.first_name;
                rs.second_name = CurrentEmployees.second_name;
                rs.roster_group_id = CurrentRosterGroup.roster_group_id;
                rs.is_active = true;
                listTempEmp.Add(rs);
                RosterGroupEmployees = null;
                RosterGroupEmployees = listTempEmp;
            }
        }

        public ICommand AddButton
        {
            get { return new RelayCommand(Add, addCanExecute); }
        }
        #endregion

        #region Remove Method
        private bool removeCanExecute()
        {
            if (CurrentRosterGroupEmployee == null)
                return false;
            return true;
        }

        private void Remove()
        {
            if (listTempEmp.Count(c => c.emp_id == CurrentRosterGroupEmployee.emp_id) != 0)
            {
                listTempEmp.Remove(CurrentRosterGroupEmployee);
                RosterGroupEmployees = null;
                RosterGroupEmployees = listTempEmp;
            }
        }

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove, removeCanExecute); }
        }
        #endregion

        #region New Method
        private void New()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.RosterGroup), clsSecurity.loggedUser.user_id))
            {
                CurrentDesignation = null;
                RefreshRosterGroups();
                //RefreshEmployeesForRosterView();
                RefreshRosterGroupEmployees();
                listTempEmp.Clear();
                try
                {
                    CurrentRosterGroup = null;
                    CurrentRosterGroup = new z_RosterGroup();
                    RosterGroupEmployees = null;
                    CurrentRosterGroup.roster_group_id = serviceClient.GetLastRosterGroupNo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        #endregion

        #region Save Method
        private bool saveCanExecute()
        {
            if (CurrentRosterGroup == null)
                return false;
            if (CurrentRosterGroup.roster_group_name == null)
                return false;
            if (listTempEmp.Count() == 0)
                return false;
            return true;
        }

        private void Save()
        {
            DateTime saveDate = DateTime.Now;
            if (listTempEmp.Count != 0)
            {
                List<dtl_RosterGroupDetail> listrosterGroup = new List<dtl_RosterGroupDetail>();
                dtl_RosterGroupDetail rg;
                foreach (var item in listTempEmp)
                {
                    rg = new dtl_RosterGroupDetail();
                    rg.employee_id = item.employee_id;
                    rg.roster_group_id = CurrentRosterGroup.roster_group_id;
                    rg.is_active = true;
                    rg.is_delete = false;
                    rg.save_user_id = clsSecurity.loggedUser.user_id;
                    rg.save_datetime = saveDate;
                    listrosterGroup.Add(rg);
                }

                bool Is_Update = false;
                CurrentRosterGroup.is_delete = false;
                if (CurrentRosterGroup.is_active == null)
                    CurrentRosterGroup.is_active = false;
                if(RosterGroups.Count(c => c.roster_group_id == CurrentRosterGroup.roster_group_id)!=0)
                {
                    Is_Update = true;
                    CurrentRosterGroup.modified_datetime = saveDate;
                    CurrentRosterGroup.modified_user_id = clsSecurity.loggedUser.user_id;
                }
                if (Is_Update)
                {
                    if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.RosterGroup), clsSecurity.loggedUser.user_id))
                    {
                        if (serviceClient.UpdateRosterGroup(CurrentRosterGroup, listrosterGroup.ToArray()))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            New();
                        }
                        else
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.NoPermissionForUpdate);
                    }
                }
                else
                {
                    if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.RosterGroup), clsSecurity.loggedUser.user_id))
                    {
                        CurrentRosterGroup.save_datetime = saveDate;
                        CurrentRosterGroup.save_user_id = clsSecurity.loggedUser.user_id;
                        if (serviceClient.SaveRosterGroup(CurrentRosterGroup, listrosterGroup.ToArray()))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                            New();
                        }
                        else
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
                    }
                }
            }
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, saveCanExecute); }
        }
        #endregion

        #region Delete Method
        private bool deleteCanExecute()
        {
            if (CurrentRosterGroup == null)
                return false;
            return true;
        }

        private void Delete()
        {
            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.RosterGroup), clsSecurity.loggedUser.user_id))
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Are you sure, You want to Delete this Roster Group", "Deleting Roster Group", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    try
                    {
                        CurrentRosterGroup.delete_datetime = DateTime.Now;
                        CurrentRosterGroup.delete_user_id = clsSecurity.loggedUser.user_id;
                        if (serviceClient.DeleteRosterGroup(CurrentRosterGroup))
                        {
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        }
                        else
                            clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.YesNo, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForDelete);
            }
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, deleteCanExecute); }
        }
        #endregion

        #region Refresh Methods
        private void RefreshRosterGroups()
        {
            try
            {
                serviceClient.GetRosterGroupCompleted += (s, e) =>
                    {
                        RosterGroups = e.Result;
                    };
                serviceClient.GetRosterGroupAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshEmployeesForRosterView()
        {
            try
            {
                listEmp.Clear();
                serviceClient.GetEmployeesForRosterViewCompleted += (s, e) =>
                    {
                        Employees = e.Result;
                        if (Employees != null)
                            listEmp = Employees.ToList();
                    };
                serviceClient.GetEmployeesForRosterViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshRosterGroupEmployees()
        {
            try
            {
                listEmployees.Clear();
                IEnumerable<EmployeeRosterDetailView> ie;
                serviceClient.GetEmployeeRosterDetailViewCompleted += (s, e) =>
                {
                    ie = e.Result;
                    if (ie != null)
                        listEmployees = ie.ToList();
                };
                serviceClient.GetEmployeeRosterDetailViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefresDesignations()
        {
            try
            {
                serviceClient.GetDesignationsCompleted += (s, e) =>
                {
                    Designations = e.Result;
                };
                serviceClient.GetDesignationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Filter Employees
        private void FilterEmployees()
        {
            try
            {
                RosterGroupEmployees = null;
                if (CurrentRosterGroup != null && CurrentRosterGroup.roster_group_id!=0)
                {
                    RosterGroupEmployees = listEmployees;
                    RosterGroupEmployees = RosterGroupEmployees.Where(c => c.roster_group_id == CurrentRosterGroup.roster_group_id);
                    listTempEmp.Clear();
                    if (RosterGroupEmployees != null)
                        listTempEmp = RosterGroupEmployees.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
        }

        private void FilterEmployeeByDesignation()
        {
            Employees = null;
            Employees = listEmp;
            if (CurrentDesignation != null)
                Employees = Employees.Where(c => c.designation_id == CurrentDesignation.designation_id);
        }
        #endregion
    }
}