using ERP.AttendanceService;
using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ERP.AttendanceModule.AttendanceMasters
{
    public class EmployeeAttendanceGroupAssignViewModel:ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendClient;

        #endregion

        #region Constructor

        public EmployeeAttendanceGroupAssignViewModel()
        {
            attendClient = new AttendanceServiceClient();
            this.RefreshAttendanceGroups();
        }

        #endregion

        #region Data Members

        public DataGrid empGroupsGrid;

        #endregion

        #region List Members

        List<EmployeeSearchView> currentSearchList = new List<EmployeeSearchView>();
        List<AttendanceGroupWithEmployee> currentGroupList = new List<AttendanceGroupWithEmployee>();
        List<AttendanceGroupWithEmployee> newGroupList = new List<AttendanceGroupWithEmployee>();
        List<AttendanceGroupWithEmployee> updatingList = new List<AttendanceGroupWithEmployee>();
        List<SelectEmployee> datagridList = new List<SelectEmployee>();
        List<AttendanceGroupWithEmployee> availableEmpList = new List<AttendanceGroupWithEmployee>();

        #endregion

        #region Properties

        public List<SelectEmployee> DatagridList
        {
            get { return datagridList; }
            set { datagridList = value; OnPropertyChanged("DatagridList"); }
        }

        IEnumerable<AttendanceGroupDetailsView> attendanceGroups;
        public IEnumerable<AttendanceGroupDetailsView> AttendanceGroups
        {
            get { return attendanceGroups; }
            set { attendanceGroups = value; OnPropertyChanged("AttendanceGroups"); }
        }

        AttendanceGroupDetailsView currentAttendanceGroup;
        public AttendanceGroupDetailsView CurrentAttendanceGroup
        {
            get { return currentAttendanceGroup; }
            set 
            { 
                currentAttendanceGroup = value; OnPropertyChanged("CurrentAttendanceGroup");
                if (currentAttendanceGroup != null)
                {
                    this.RefreshGroupEmployees();
                    this.ClearSearchedList();
                }
            }
        }

        IEnumerable<EmployeeSearchView> searchedEmployees;
        public IEnumerable<EmployeeSearchView> SearchedEmployees
        {
            get { return searchedEmployees; }
            set { searchedEmployees = value; OnPropertyChanged("SearchedEmployees"); }
        }

        EmployeeSearchView currentSearchedEmployee;
        public EmployeeSearchView CurrentSearchedEmployee
        {
            get { return currentSearchedEmployee; }
            set { currentSearchedEmployee = value; OnPropertyChanged("CurrentSearchedEmployee"); }
        }

        IEnumerable<AttendanceGroupWithEmployee> attendanceGroupEmployees;
        public IEnumerable<AttendanceGroupWithEmployee> AttendanceGroupEmployees
        {
            get { return attendanceGroupEmployees; }
            set { attendanceGroupEmployees = value; OnPropertyChanged("AttendanceGroupEmployees"); }
        }

        AttendanceGroupWithEmployee currentGroupEmployee;
        public AttendanceGroupWithEmployee CurrentGroupEmployee
        {
            get { return currentGroupEmployee; }
            set { currentGroupEmployee = value; OnPropertyChanged("CurrentGroupEmployee"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshAttendanceGroups()
        {
            attendClient.GetAttendanceGroupsDetailsCompleted += (s, e) => {
                try
                {
                    AttendanceGroups = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance group details refresh is failed");
                }
            };

            attendClient.GetAttendanceGroupsDetailsAsync();
        }

        void RefreshGroupEmployees()
        {
            AttendanceGroupEmployees = null;
            attendClient.GetAttendanceGroupWithEmployeesDetailsCompleted += (s, e) =>
            {
                try
                {
                    AttendanceGroupEmployees = e.Result;
                    if (attendanceGroupEmployees != null)
                    {
                        currentGroupList.Clear();
                        currentGroupList = attendanceGroupEmployees.ToList();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance group employees refresh is failed");
                }

            };

            attendClient.GetAttendanceGroupWithEmployeesDetailsAsync(currentAttendanceGroup.attendance_group_id);
        }

        void RefreshAssignedGroupDetails()
        {
            attendClient.GetAssignedGroupEmployeesCompleted += (s, e) =>
            {
                try
                {
                    availableEmpList = e.Result.ToList();
                    if (availableEmpList != null && availableEmpList.Count > 0)
                        this.ShowEmployeeAvailability();
                        
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Availability details refresh is failed");
                }
            };
            attendClient.GetAssignedGroupEmployeesAsync(newGroupList.Select(c => c.employee_id).ToArray(), currentAttendanceGroup.attendance_group_id);
        }

        #endregion

        #region Button Methods

        #region Get Employee Button

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
                this.FilterAddingEmployees();
                this.FillSelectedDatagridEmployees();
            }

            searchWindow.Close();
        }

        public ICommand GetEmployeeButton
        {
            get { return new RelayCommand(GetEmployees); }

        } 

        #endregion

        #region Assign Button

        void Assign()
        {
            if (currentAttendanceGroup != null)
            {
                if (newGroupList.Count > 0)
                {
                    if (clsSecurity.GetSavePermission(304))
                    {
                        List<dtl_AttendanceGroup> addingList = new List<dtl_AttendanceGroup>();
                        foreach (AttendanceGroupWithEmployee current in newGroupList)
                        {
                            dtl_AttendanceGroup savingGroup = new dtl_AttendanceGroup();
                            savingGroup.employee_id = current.employee_id;
                            savingGroup.attendance_group_id = currentAttendanceGroup.attendance_group_id;
                            savingGroup.is_active = true;
                            savingGroup.is_delete = false;
                            savingGroup.saved_datetime = DateTime.Now;
                            savingGroup.saved_user_id = clsSecurity.loggedUser.user_id;
                            savingGroup.modified_datetime = DateTime.Now;
                            savingGroup.modified_user_id = clsSecurity.loggedUser.user_id;

                            addingList.Add(savingGroup);
                        }
                        if (attendClient.SaveAttendanceGroupEmployees(addingList.ToArray()))
                        {
                            clsMessages.setMessage("Employee attendance groups are saved successfully");
                            this.RefreshGroupEmployees();
                            this.ClearSearchedList();
                        }
                        else
                        {
                            clsMessages.setMessage("Save is failed");
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                }
                else
                {
                    clsMessages.setMessage("No new employees to save"); 
                }
            }
            else
            {
                clsMessages.setMessage("Attendance group should be selected");
            }
        }

        public ICommand AssignButton
        {
            get { return new RelayCommand(Assign); }
        }

        #endregion

        #region Check Available Button

        public ICommand CheckAvailableButton
        {
            get { return new RelayCommand(CheckEmployeeAvailability); }
        }

        private void CheckEmployeeAvailability()
        {
            if(datagridList.Count > 0)
            {
                if(newGroupList.Count > 0 && currentAttendanceGroup != null)
                {
                    this.RefreshAssignedGroupDetails();
                }
            }
        }
        
        #endregion

        #endregion

        #region Filter Methods

        void FilterAddingEmployees()
        {
            if(currentSearchList.Count > 0)
            {
                if (currentGroupList.Count > 0 )
                {
                    newGroupList.Clear();
                    List<EmployeeSearchView> empList = currentSearchList.Where(c => !currentGroupList.Any(d => d.employee_id == c.employee_id)).ToList();
                    List<AttendanceGroupWithEmployee> itemList = new List<AttendanceGroupWithEmployee>();
                    foreach (EmployeeSearchView emp in empList)
                    {
                        AttendanceGroupWithEmployee item = new AttendanceGroupWithEmployee();
                        item.employee_id = emp.employee_id;
                        itemList.Add(item);
                    }
                    newGroupList = itemList;
                    updatingList = currentGroupList.Where(c => currentSearchList.Any(d => d.employee_id == c.employee_id)).ToList();
                }
                else
                {
                    newGroupList.Clear();
                    List<AttendanceGroupWithEmployee> itemList = new List<AttendanceGroupWithEmployee>();
                    foreach(EmployeeSearchView emp in currentSearchList)
                    {
                        AttendanceGroupWithEmployee item = new AttendanceGroupWithEmployee();
                        item.employee_id = emp.employee_id;
                        itemList.Add(item);
                    }

                    newGroupList = itemList;
                    updatingList.Clear();
                }
            }
        }

        #endregion

        #region Datagrid Methods

        void ChangeDatagridColor()
        {
            //if (empGroupsGrid.Items.Count > 0)
            //{
            //    empGroupsGrid.UpdateLayout();
            //    foreach (SelectEmployee item in empGroupsGrid.ItemsSource)
            //    {
            //        var row = empGroupsGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
            //        if (updatingList.Count(c => c.employee_id == item.EID) > 0)
            //        {
            //            if (row != null)
            //            {
            //                row.IsEnabled = false;
            //            }
            //        }
            //    }
            //}
        }

        void FillSelectedDatagridEmployees()
        {
            if (currentSearchList.Count > 0)
            {
                datagridList.Clear();
                List<SelectEmployee> empList = new List<SelectEmployee>();
                foreach (var item in currentSearchList)
                {
                    SelectEmployee addingEmp = new SelectEmployee();
                    addingEmp.EID = item.employee_id;
                    addingEmp.EmpID = item.emp_id;
                    addingEmp.FirstName = item.first_name;
                    addingEmp.SecondName = item.second_name;
                    addingEmp.IsExist = false;
                    addingEmp.IsSelected = true;
                    if(updatingList.Count(c=>c.employee_id == item.employee_id) > 0)
                    {
                        addingEmp.IsExist = true;
                        addingEmp.IsSelected = false;
                    }
                    empList.Add(addingEmp);
                }
                DatagridList = empList;
            }
        }

        void ClearSearchedList()
        {
            DatagridList = null;
            datagridList = new List<SelectEmployee>();
        }

        void ShowEmployeeAvailability()
        {
            List<SelectEmployee> tempEmpList = datagridList;
            foreach(var item in availableEmpList)
            {
                SelectEmployee selectedItem = datagridList.FirstOrDefault(c => c.EID == item.employee_id);
                selectedItem.GroupName = item.attendance_group_name;
                selectedItem.IsAssigned = true;
            }
            DatagridList = null;
            DatagridList = tempEmpList;
        }

        #endregion
    }

    public class SelectEmployee
    {
        public Guid EID { get; set; }
        public string EmpID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FullName { get { return FirstName + ' ' + SecondName; } }
        public string GroupName { get; set; }
        public bool IsExist { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsSelected { get; set; }
    }
}
