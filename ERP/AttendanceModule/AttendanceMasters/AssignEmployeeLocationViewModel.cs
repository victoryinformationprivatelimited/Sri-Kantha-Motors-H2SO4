using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class AssignEmployeeLocationViewModel : ViewModelBase
    {
        #region Service
        private ERPServiceClient serviceClient = new ERPServiceClient();

        List<EmployeeSearchView> currentSearchList = new List<EmployeeSearchView>();
        List<EmployeeSumarryView> DataGridList = new List<EmployeeSumarryView>();


        List<EmployeeSumarryView> AllEmp = new List<EmployeeSumarryView>();
        List<z_Location> AllLoc = new List<z_Location>();
        List<dtl_employeeLocation> AllEmpLoc = new List<dtl_employeeLocation>();
        #endregion

        #region Constructor
        public AssignEmployeeLocationViewModel()
        {
            New();
        }

        #endregion

        #region Properties

        public List<EmployeeSumarryView> DataGrid
        {
            get { return DataGridList; }
            set { DataGridList = value; OnPropertyChanged("DataGrid"); }
        }

        IEnumerable<EmployeeSearchView> searchedEmployees;
        IEnumerable<EmployeeSearchView> SearchedEmployees
        {
            get { return searchedEmployees; }
            set { searchedEmployees = value; OnPropertyChanged("SearchedEmployees"); }
        }

        IEnumerable<EmployeeSearchView> currentSearchedEmployees;
        IEnumerable<EmployeeSearchView> CurrentSearchedEmployees
        {
            get { return currentSearchedEmployees; }
            set { currentSearchedEmployees = value; OnPropertyChanged("CurrentSearchedEmployee"); }
        }

        private IEnumerable<AssignedEmployeeLocationView> _EmployeeLocations;
        public IEnumerable<AssignedEmployeeLocationView> EmployeeLocations
        {
            get
            {
                return this._EmployeeLocations;
            }
            set
            {
                this._EmployeeLocations = value;
                this.OnPropertyChanged("EmployeeLocations");
            }
        }

        private AssignedEmployeeLocationView _CurrentEmployeeLocation;
        public AssignedEmployeeLocationView CurrentEmployeeLocation
        {
            get
            {
                return this._CurrentEmployeeLocation;
            }
            set
            {
                this._CurrentEmployeeLocation = value;
                this.OnPropertyChanged("CurrentEmployeeLocation");
                if (CurrentEmployeeLocation != null && CurrentEmployeeLocation.employee_id != Guid.Empty)
                {
                    filterSelectBoxes();
                }
            }
        }

        private IEnumerable<z_Location> _Locations;
        public IEnumerable<z_Location> Locations
        {
            get
            {
                return this._Locations;
            }
            set
            {
                this._Locations = value;
                this.OnPropertyChanged("Locations");
            }
        }

        private z_Location _CurrentLocation;
        public z_Location CurrentLocation
        {
            get
            {
                return this._CurrentLocation;
            }
            set
            {
                this._CurrentLocation = value;
                this.OnPropertyChanged("CurrentLocation");
            }
        }

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentWantAddEmployee;
        public EmployeeSumarryView CurrentWantAddEmployee
        {
            get { return this._CurrentWantAddEmployee; }
            set
            {
                this._CurrentWantAddEmployee = value; OnPropertyChanged("CurrentWantAddEmployee");
            }
        }

        #endregion

        #region Button Commands

        public ICommand ButtonSearchEmployee
        {
            get { return new RelayCommand(GetEmployees); }
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        public ICommand UpdateButton
        {
            get
            {
                return new RelayCommand(Update, UpdateCanExecute);
            }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, SaveCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }


        #endregion

        #region Refresh Methodes

        private void refreshLocations()
        {
            this.serviceClient.GetLocationCompleted += (s, e) =>
            {
                this.Locations = e.Result.Where(c => c.is_delete == false);
                foreach (var location in Locations)
                {
                    AllLoc.Add(location);
                }
            };
            this.serviceClient.GetLocationAsync();
        }

        private void reafreshEmployee()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(z => z.isdelete == false);
                foreach (var employee in Employees)
                {
                    AllEmp.Add(employee);
                }
            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }

        private void refreshEmployeeLocations()
        {
            this.serviceClient.GetEmployeeLocationViewCompleted += (s, e) =>
            {
                this.EmployeeLocations = e.Result.Where(c => c.is_active == true && c.is_delete == false);
            };
            this.serviceClient.GetEmployeeLocationViewAsync();
        }

        #endregion

        #region Methods

        #region Filter Method

        void ClearSearchedList()
        {
            DataGrid = null;
            DataGridList = new List<EmployeeSumarryView>();
        }

        void FillSelectedDatagridEmplyees()
        {
            if (currentSearchList.Count > 0)
            {
                DataGridList.Clear();
                List<EmployeeSumarryView> empList = new List<EmployeeSumarryView>();
                foreach (var item in currentSearchList)
                {
                    EmployeeSumarryView addList = new EmployeeSumarryView();
                    addList.emp_id = item.emp_id;
                    addList.employee_id = item.employee_id;
                    addList.first_name = item.first_name;
                    addList.second_name = item.second_name;

                    empList.Add(addList);
                }
                DataGrid = empList;
            }
        }

        #endregion

        #region Search Employees

        void GetEmployees()
        {
            EmployeeMultipleSearchWindow searchWindow = new EmployeeMultipleSearchWindow();
            searchWindow.ShowDialog();
            if (searchWindow.viewModel.selectEmployeeList != null)
            {
                currentSearchList.Clear();
                SearchedEmployees = null;
                currentSearchList = searchWindow.viewModel.selectEmployeeList;
                SearchedEmployees = currentSearchList;
                this.FillSelectedDatagridEmplyees();
            }
            searchWindow.Close();
        }

        #endregion

        #region Delete

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(322))
            {
                clsMessages.setMessage("Do you want to Delete this record? ", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    dtl_employeeLocation dtl = new dtl_employeeLocation();
                    dtl.employee_id = CurrentWantAddEmployee.employee_id;
                    dtl.location_id = CurrentLocation.location_id;

                    if (serviceClient.DeleteEmployeeLocation(dtl))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    refreshEmployeeLocations();
                }
            }
            else
            {
                clsMessages.setMessage("You dont have permission to Delete this records");
            }

        }

        bool deleteCanExecute()
        {
            if (CurrentLocation == null)
                return false;

            return true;
        }


        #endregion

        #region Save

        private void Save()
        {
            if (clsSecurity.GetUpdatePermission(322))
            {
                /* if (EmployeeLocations.FirstOrDefault(c => c.location_id == CurrentEmployeeLocation.location_id) == null)
                 {*/
                List<dtl_employeeLocation> AddingList = new List<dtl_employeeLocation>();
                foreach (EmployeeSumarryView current in DataGridList)
                {
                    dtl_employeeLocation newLocation = new dtl_employeeLocation();
                    newLocation.location_id = CurrentLocation.location_id;
                    newLocation.employee_id = current.employee_id;
                    newLocation.distance = CurrentEmployeeLocation.distance;
                    newLocation.is_active = true;
                    newLocation.is_delete = false;
                    newLocation.save_datetime = System.DateTime.Now;

                    AddingList.Add(newLocation);
                }
                if (this.serviceClient.SaveMoreEmployeeLocations(AddingList.ToArray()))
                {
                    clsMessages.setMessage("Employee Locations are saved successfully");
                    New();
                }
                else
                {
                    clsMessages.setMessage(Properties.Resources.SaveFail);
                }
                /* }
                 else
                     clsMessages.setMessage("Location Already Exists, Please Enter Different location");*/
            }
            else
            {
                clsMessages.setMessage(" You don't have permission to save this record(s)");
            }

        }

        bool SaveCanExecute()
        {

            if (CurrentLocation == null)
                return false;
            if (DataGridList == null)
                return false;


            return true;
        }


        #endregion

        #region Update
        private void Update()
        {

            bool isUpdate = false;

            dtl_employeeLocation newLocation = new dtl_employeeLocation();
            newLocation.location_id = CurrentLocation.location_id;
            newLocation.employee_id = CurrentWantAddEmployee.employee_id;
            newLocation.distance = CurrentEmployeeLocation.distance;
            newLocation.is_active = true;
            newLocation.is_delete = false;
            newLocation.save_datetime = System.DateTime.Now;

            IEnumerable<AssignedEmployeeLocationView> All = this.serviceClient.GetEmployeeLocationView();

            if (All != null)
            {
                foreach (var item in All)
                {
                    if (item.location_id == CurrentEmployeeLocation.location_id)
                        isUpdate = true;
                }
            }

            if (newLocation != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(322))
                    {
                        newLocation.modified_datetime = System.DateTime.Now;
                        if (this.serviceClient.UpdateEmployeeLocation(newLocation))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            New();
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }
                    }
                }
                else
                {
                    if (clsSecurity.GetSavePermission(322))
                    {
                        if (EmployeeLocations.FirstOrDefault(c => c.location_id == CurrentEmployeeLocation.location_id) == null)
                        {
                            newLocation.save_datetime = System.DateTime.Now;
                            if (this.serviceClient.SaveEmployeeLocation(newLocation))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                New();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            }
                        }
                        else
                            clsMessages.setMessage("Location Already Exists, Please Enter Different location");
                    }
                    else
                    {
                        clsMessages.setMessage(" You don't have permission to save this record(s)");
                    }
                }
                refreshEmployeeLocations();
            }
            else
            {
                clsMessages.setMessage(" Please Enter Location details !");
            }

        }

        bool UpdateCanExecute()
        {

            if (CurrentLocation == null)
                return false;
            if (CurrentWantAddEmployee == null || CurrentWantAddEmployee.employee_id == Guid.Empty)
                return false;
            /*if (CurrentEmployeeLocation != null)
            {
                CurrentEmployeeLocation.employee_id = CurrentWantAddEmployee.employee_id;
                CurrentEmployeeLocation.location_id = CurrentLocation.location_id;
            }*/

            return true;
        }

        #endregion

        #region New Methode
        private void New()
        {
            try
            {
                refreshLocations();
                reafreshEmployee();
                refreshEmployeeLocations();

                CurrentEmployeeLocation = null;
                CurrentEmployeeLocation = new AssignedEmployeeLocationView();
                ClearSearchedList();
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }


        }

        bool newCanExecute()
        {
            return true;
        }

        #endregion

        public void filterSelectBoxes()
        {
            CurrentWantAddEmployee = Employees.FirstOrDefault(c => c.employee_id == CurrentEmployeeLocation.employee_id);
            CurrentLocation = Locations.FirstOrDefault(c => c.loc_code == CurrentEmployeeLocation.loc_code);
        }

        #endregion

    }
}
