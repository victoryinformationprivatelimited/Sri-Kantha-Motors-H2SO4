using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Performance
{
    class EmployeeSubTasksViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<EmployeeSubTasksView> ListAllSubtasks;
        List<EmployeeSubTasksDetailView> ListAllEmployees;

        #endregion

        #region Constructor

        public EmployeeSubTasksViewModel()
        {
            serviceClient = new ERPServiceClient();
            SearchIndex = 0;
            SearchText = "";
            New();
        }

        #endregion

        #region Properties

        private DateTime? _SearchFrom;
        public DateTime? SearchFrom
        {
            get { return _SearchFrom; }
            set { _SearchFrom = value; OnPropertyChanged("SearchFrom"); }
        }

        private DateTime? _SearchTo;
        public DateTime? SearchTo
        {
            get { return _SearchTo; }
            set { _SearchTo = value; OnPropertyChanged("SearchTo"); }
        }
        

        private Visibility _TaskDateRangeVisible;
        public Visibility TaskDateRangeVisible
        {
            get { return _TaskDateRangeVisible; }
            set { _TaskDateRangeVisible = value; OnPropertyChanged("TaskDateRangeVisible"); }
        }

        private string _TaskDateRange;
        public string TaskDateRange
        {
            get { return _TaskDateRange; }
            set { _TaskDateRange = value; OnPropertyChanged("TaskDateRange"); }
        }

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private IList _CurrentSubTasksList = new ArrayList();
        public IList CurrentSubTasksList
        {
            get { return _CurrentSubTasksList; }
            set { _CurrentSubTasksList = value; OnPropertyChanged("CurrentSubTasksList"); if (CurrentSubTasksList != null && CurrentSubTasksList.Count > 0) FilterSubtaskemployees(); }
        }

        private IEnumerable<EmployeeSupervisorsView> _Supervisors;
        public IEnumerable<EmployeeSupervisorsView> Supervisors
        {
            get { return _Supervisors; }
            set { _Supervisors = value; OnPropertyChanged("Supervisors"); }
        }

        private EmployeeSupervisorsView _CurrentSupervisor;
        public EmployeeSupervisorsView CurrentSupervisor
        {
            get { return _CurrentSupervisor; }
            set { _CurrentSupervisor = value; OnPropertyChanged("CurrentSupervisor"); }
        }

        private IEnumerable<usr_UserEmployee> _UserEmployee;
        public IEnumerable<usr_UserEmployee> UserEmployee
        {
            get { return _UserEmployee; }
            set { _UserEmployee = value; OnPropertyChanged("UserEmployee"); }
        }

        private IEnumerable<EmployeeSupervisorsByUserView> _Employees;
        public IEnumerable<EmployeeSupervisorsByUserView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<EmployeeSearchView> _EmployeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _EmployeeSearch; }
            set { _EmployeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        private IEnumerable<EmployeeTasksView> _Tasks;
        public IEnumerable<EmployeeTasksView> Tasks
        {
            get { return _Tasks; }
            set { _Tasks = value; OnPropertyChanged("Tasks"); }
        }

        private EmployeeTasksView _CurrentTask;
        public EmployeeTasksView CurrentTask
        {
            get { return _CurrentTask; }
            set { _CurrentTask = value; OnPropertyChanged("CurrentTask"); if (CurrentTask != null) { TaskDateRangeVisible = Visibility.Visible; TaskDateRange = "From" + "  " + CurrentTask.Task_StartDate.Value.ToShortDateString() + "  " + "To" + "  " + CurrentTask.Task_EndDate.Value.ToShortDateString(); FilterSubtasks(); } else { TaskDateRangeVisible = Visibility.Hidden; TaskDateRange = ""; } }
        }

        private IEnumerable<EmployeeSubTasksView> _SubTasks;
        public IEnumerable<EmployeeSubTasksView> SubTasks
        {
            get { return _SubTasks; }
            set { _SubTasks = value; OnPropertyChanged("SubTasks"); }
        }

        private EmployeeSubTasksView _CurrentSubTask;
        public EmployeeSubTasksView CurrentSubTask
        {
            get { return _CurrentSubTask; }
            set { _CurrentSubTask = value; OnPropertyChanged("CurrentSubTask"); }
        }

        private IEnumerable<EmployeeSubTasksDetailView> _SubtaskEmployee;
        public IEnumerable<EmployeeSubTasksDetailView> SubtaskEmployee
        {
            get { return _SubtaskEmployee; }
            set { _SubtaskEmployee = value; OnPropertyChanged("SubtaskEmployee");}
        }

        private IList _CurrrentSubtaskEmployee;
        public IList CurrrentSubtaskEmployee
        {
            get { return _CurrrentSubtaskEmployee; }
            set { _CurrrentSubtaskEmployee = value; OnPropertyChanged("CurrrentSubtaskEmployee"); }
        }

        private IEnumerable<z_TaskPriority> _Priorities;
        public IEnumerable<z_TaskPriority> Priorities
        {
            get { return _Priorities; }
            set { _Priorities = value; OnPropertyChanged("Priorities"); }
        }

        private IEnumerable<z_EvaluationRateGoup> _RateGroups;
        public IEnumerable<z_EvaluationRateGoup> RateGroups 
        {
            get { return _RateGroups; }
            set { _RateGroups = value; OnPropertyChanged("RateGroups"); }
        }
        

        #endregion

        #region RefreshMethods

        private void RsfreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                try
                {
                    UserEmployee = e.Result;
                    if (UserEmployee != null && UserEmployee.Count() > 0 && Supervisors != null && Supervisors.Count() > 0)
                    {
                        if (UserEmployee.Count(c => c.user_id == clsSecurity.loggedUser.user_id) > 0)
                        {
                            CurrentSupervisor = Supervisors.FirstOrDefault(c => c.supervisor_employee_id == UserEmployee.FirstOrDefault(d => d.user_id == clsSecurity.loggedUser.user_id).employee_id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RsfreshUserEmployee() \n\n" + ex.Message);
                }
            };
            serviceClient.GetUserEmployeesAsync();
        }

        private void RefreshSupervisor()
        {
            serviceClient.GetAllEmployeeSupervisorsCompleted += (s, e) =>
            {
                try
                {
                    Supervisors = e.Result;

                    if (Supervisors != null && Supervisors.Count() > 0 && UserEmployee != null && UserEmployee.Count() > 0)
                    {
                        if (UserEmployee.Count(c => c.user_id == clsSecurity.loggedUser.user_id) > 0)
                        {
                            CurrentSupervisor = Supervisors.FirstOrDefault(c => c.supervisor_employee_id == UserEmployee.FirstOrDefault(d => d.user_id == clsSecurity.loggedUser.user_id).employee_id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshSupervisor() \n\n" + ex.Message);
                }

            };
            serviceClient.GetAllEmployeeSupervisorsAsync(clsSecurity.loggedUser.user_id);
        }

        private void RefreshTasks()
        {
            serviceClient.GetEmployeeTasksByDateCompleted += (s, e) =>
            {
                try
                {
                    Tasks = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshTasks() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeTasksByDateAsync((DateTime)SearchFrom,(DateTime)SearchTo,clsSecurity.loggedUser.user_id);
        }

        private void ResfreshSubtasks()
        {
            serviceClient.GetEmployeeSubTasksByDateCompleted += (s, e) =>
            {
                try
                {
                    SubTasks = e.Result;
                    if (SubTasks != null && SubTasks.Count() > 0)
                    {
                        ListAllSubtasks = SubTasks.ToList();
                        SubTasks = null;
                    }
                    CurrentSubTask = null;
                    CurrentSubTask = new EmployeeSubTasksView();
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("ResfreshSubtasks() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeSubTasksByDateAsync((DateTime)SearchFrom,(DateTime)SearchTo,clsSecurity.loggedUser.user_id);
        }

        private void ResfreshSubtaskEmployees()
        {
            serviceClient.GetEmployeeSubTaskDeatilsByDateCompleted += (s, e) =>
            {
                try
                {
                    SubtaskEmployee = e.Result;
                    if (SubtaskEmployee != null && SubtaskEmployee.Count() > 0)
                    {
                        ListAllEmployees = SubtaskEmployee.ToList();
                        SubtaskEmployee = null;
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("ResfreshSubtaskEmployees() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeSubTaskDeatilsByDateAsync((DateTime)SearchFrom,(DateTime)SearchTo,clsSecurity.loggedUser.user_id);
        }

        private void RefreshPriorities()
        {
            serviceClient.GetTaskPrioritiesCompleted += (s, e) =>
            {
                try
                {
                    Priorities = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshPriorities() \n\n" + ex.Message);
                }
            };
            serviceClient.GetTaskPrioritiesAsync();
        }

        private void ResfeshEmployees()
        {
            serviceClient.GetEmployeesBySupervisorUserCompleted += (s, e) =>
            {
                try
                {
                    Employees = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("ResfeshEmployees() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeesBySupervisorUserAsync(clsSecurity.loggedUser.user_id, new Guid("46181184-5CA4-4622-813C-2AE2A3513A40"));
        }

        private void RefreshEmployeeSearch()
        {
            serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
            {
                try
                {
                    EmployeeSearch = e.Result.OrderBy(c => c.emp_id);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEmployeeSearch() \n\n" + ex.Message);
                }

            };
            serviceClient.GetEmloyeeSearchAsync();
        }

        private void RefreshRateGroups() 
        {
            serviceClient.GetEvaluationRateGroupsCompleted += (s, e) => 
            {
                try
                {
                    RateGroups = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshRateGroups() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEvaluationRateGroupsAsync();
        }


        #endregion

        #region Commands & Methods
         
        #region New

        public ICommand SearchTasks 
        {
            get { return new RelayCommand(New2,SearchTaskCanExecute); }
        }

        private void New2()
        {
            ListAllEmployees = new List<EmployeeSubTasksDetailView>();
            ListAllSubtasks = new List<EmployeeSubTasksView>();

            ResfreshSubtaskEmployees();
            ResfreshSubtasks();
            RefreshTasks();
        }

        private bool SearchTaskCanExecute()
        {
            if (SearchFrom != null && SearchTo != null)
                return true;
            else
                return false;
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            SearchIndex = 0;
            SearchText = "";
            Tasks = null;
            SubTasks = null;
            SubtaskEmployee = null;
            ListAllEmployees = new List<EmployeeSubTasksDetailView>();
            ListAllSubtasks = new List<EmployeeSubTasksView>();

            RefreshSupervisor();
            RsfreshUserEmployee();
            RefreshPriorities();
            ResfeshEmployees();
            RefreshEmployeeSearch();
            RefreshRateGroups();
        }

        #endregion

        #region Add/Remove Subtask

        public ICommand AddSubtaskBtn
        {
            get { return new RelayCommand(AddSubtask, AddSubtaskCanExecute); }
        }

        private bool AddSubtaskCanExecute()
        {
            return true;
        }

        private void AddSubtask()
        {
            if (ValidateAddSubtask())
            {
                EmployeeSubTasksView tempsubtask = new EmployeeSubTasksView();

                if (ListAllSubtasks.Count(c=> c.Task_ID == CurrentTask.Task_ID)==0)
                {

                    tempsubtask.Task_ID = CurrentSubTask.Task_ID;
                    tempsubtask.SubTask_ID = -1;
                    tempsubtask.SubTask_StartDate = CurrentSubTask.SubTask_StartDate;
                    tempsubtask.SubTask_StartTime = CurrentSubTask.SubTask_StartTime;
                    tempsubtask.SubTask_EndDate = CurrentSubTask.SubTask_EndDate;
                    tempsubtask.SubTask_EndTime = CurrentSubTask.SubTask_EndTime;
                    tempsubtask.SubTaskName = CurrentSubTask.SubTaskName;
                    tempsubtask.Task_Proprity_ID = CurrentSubTask.Task_Proprity_ID;
                    tempsubtask.evaluation_rate_group_id = CurrentSubTask.evaluation_rate_group_id;
                    tempsubtask.Is_Reassignable = CurrentSubTask.Is_Reassignable;
                    tempsubtask.Is_active = true;
                    tempsubtask.is_Finished = false;
                    tempsubtask.isdelete = false;

                    ListAllSubtasks.Add(tempsubtask);
                    SubTasks = null;
                    SubTasks = ListAllSubtasks.Where(c=> c.Task_ID == CurrentTask.Task_ID);
                }
                else
                {
                    tempsubtask.Task_ID = CurrentSubTask.Task_ID;
                    tempsubtask.SubTask_ID = (SubTasks.Min(c => c.SubTask_ID) < 0) ? (SubTasks.Min(c => c.SubTask_ID) - 1) : (((SubTasks.Min(c => c.SubTask_ID) - SubTasks.Min(c => c.SubTask_ID))) - 1);
                    tempsubtask.SubTask_StartDate = CurrentSubTask.SubTask_StartDate;
                    tempsubtask.SubTask_StartTime = CurrentSubTask.SubTask_StartTime;
                    tempsubtask.SubTask_EndDate = CurrentSubTask.SubTask_EndDate;
                    tempsubtask.SubTask_EndTime = CurrentSubTask.SubTask_EndTime;
                    tempsubtask.SubTaskName = CurrentSubTask.SubTaskName;
                    tempsubtask.Task_Proprity_ID = CurrentSubTask.Task_Proprity_ID;
                    tempsubtask.evaluation_rate_group_id = CurrentSubTask.evaluation_rate_group_id;
                    tempsubtask.Is_Reassignable = CurrentSubTask.Is_Reassignable;
                    tempsubtask.Is_active = true;
                    tempsubtask.is_Finished = false;
                    tempsubtask.isdelete = false;

                    ListAllSubtasks.Add(tempsubtask);
                    SubTasks = null;
                    SubTasks = ListAllSubtasks.Where(c => c.Task_ID == CurrentTask.Task_ID);
                }
            }
        }

        private bool ValidateAddSubtask()
        {
            if (CurrentTask == null) 
            {
                clsMessages.setMessage("Please select a Task and Try again");
                return false;
            }
            else if (CurrentTask.is_Finished == true) 
            {
                clsMessages.setMessage("Finished Tasks Cannot be Modified");
                return false;
            }
            else if (CurrentSubTask == null)
            {
                clsMessages.setMessage("Please Refresh and Try again");
                return false;
            }
            else if (CurrentSubTask.Task_ID == null || CurrentSubTask.Task_ID == 0)
            {
                clsMessages.setMessage("Please Select a Task");
                return false;
            }
            else if (CurrentSubTask.SubTask_StartDate == null)
            {
                clsMessages.setMessage("Please Select a Subtask Start Date");
                return false;
            }
            else if (CurrentSubTask.SubTask_StartDate < CurrentTask.Task_StartDate || CurrentSubTask.SubTask_StartDate > CurrentTask.Task_EndDate)
            {
                clsMessages.setMessage("Subtask Start Date Should be within the Task Date Range");
                return false;
            }
            else if (CurrentSubTask.SubTask_EndDate == null)
            {
                clsMessages.setMessage("Please Select a Subtask End Date");
                return false;
            }
            else if (CurrentSubTask.SubTask_EndDate < CurrentTask.Task_StartDate || CurrentSubTask.SubTask_EndDate > CurrentTask.Task_EndDate)
            {
                clsMessages.setMessage("Subtask End Date Should be within the Task Date Range");
                return false;
            }
            else if (CurrentSubTask.SubTask_StartDate > CurrentSubTask.SubTask_EndDate)
            {
                clsMessages.setMessage("Subtask Start Date cannot be greater than Subtask End Date");
                return false;
            }
            else if (CurrentSubTask.Task_Proprity_ID == 0)
            {
                clsMessages.setMessage("Please Select a Task Priority");
                return false;
            }
            else if (CurrentSubTask.evaluation_rate_group_id == 0)
            {
                clsMessages.setMessage("Please Select a Rate Group");
                return false;
            }
            else if (CurrentSubTask.SubTaskName == null || CurrentSubTask.SubTaskName == string.Empty)
            {
                clsMessages.setMessage("Please Enter a Subtask Name");
                return false;
            }
            else if (ListAllSubtasks.Count(c => c.SubTask_ID == CurrentSubTask.SubTask_ID && c.SubTaskName == CurrentSubTask.SubTaskName) > 0)
            {
                clsMessages.setMessage("Subtask Name already exists");
                return false;
            }
            else
                return true;
        }

        public ICommand RemoveSubtaskBtn 
        {
            get { return new RelayCommand(RemoveSubask, RemoveSubtaskCanExecute); }
        }

        private bool RemoveSubtaskCanExecute()
        {
            if (CurrentSubTasksList != null && CurrentSubTasksList.Count > 0)
                return true;
            else
                return false;
        }

        private void RemoveSubask()
        {
            EmployeeSubTasksView temp = new EmployeeSubTasksView();
            string SubtaskList = string.Empty;

            foreach (var item in CurrentSubTasksList)
            {
                temp = item as EmployeeSubTasksView;
                if (temp.SubTask_ID > 0)
                    SubtaskList += temp.SubTaskName + ",\n";
                else
                {
                    ListAllEmployees.RemoveAll(c => c.SubTask_ID == temp.SubTask_ID);
                    ListAllSubtasks.Remove(temp);
                }
                    
            }

            if (SubtaskList != string.Empty)
            {
                SubtaskList = SubtaskList.Remove(SubtaskList.Length - 2);
                clsMessages.setMessage("Subtask(s)\n"+SubtaskList + " " + "\n\nCould not be Removed As they have already been Saved");
            }

            SubtaskEmployee = null;
            SubTasks = null;
            SubTasks = ListAllSubtasks.Where(c => c.Task_ID == temp.Task_ID);
        }

        #endregion

        #region Add/Remove Employees

        public ICommand AddEmployeesBtn
        {
            get { return new RelayCommand(AddEmployees, AddEmployeesCanExecute); }
        }

        private bool AddEmployeesCanExecute()
        {
            if (CurrentSubTasksList != null && CurrentSubTasksList.Count > 0)
                return true;
            else
                return false;
        }

        private void AddEmployees()
        {
            if (CheckFinishedTasks())
            {

                if (EmployeeSearch != null && EmployeeSearch.Count() > 0 && Employees != null && Employees.Count() > 0)
                {
                    EmployeeSearch = EmployeeSearch.Where(c => Employees.Count(d => c.employee_id == d.employee_id) > 0);
                    EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow(EmployeeSearch);
                    window.ShowDialog();
                    if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                    {
                        foreach (var subtask in CurrentSubTasksList)
                        {
                            foreach (var employee in window.viewModel.selectEmployeeList)
                            {
                                EmployeeSubTasksView castedSubtask = subtask as EmployeeSubTasksView;
                                EmployeeSubTasksDetailView temp = new EmployeeSubTasksDetailView();
                                temp.SubTask_ID = castedSubtask.SubTask_ID;
                                temp.Employee_ID = employee.employee_id;
                                temp.emp_id = employee.emp_id;
                                temp.initials = employee.initials;
                                temp.first_name = employee.first_name;
                                temp.second_name = employee.second_name;
                                temp.Is_active = castedSubtask.Is_active;
                                temp.Is_Reassigned = false;
                                temp.is_accepted = false;
                                temp.is_rejected = false;
                                temp.is_pending = false;
                                temp.is_Finished = false;
                                temp.is_Removed = false;
                                temp.is_expired = false;
                                temp.isdelete = false;

                                if (ListAllEmployees.Count(c => c.Employee_ID == employee.employee_id && c.SubTask_ID == castedSubtask.SubTask_ID) == 0)
                                {
                                    ListAllEmployees.Add(temp);
                                }
                            }
                        }

                        FilterSubtaskemployees();
                    }
                }
                else if (CurrentSupervisor == null)
                    clsMessages.setMessage("Current user is not a supervisor");
                else
                    clsMessages.setMessage("There's no Employees under this Supervisor");
            }
            else
                clsMessages.setMessage("Finished subtasks cannot be modified");


        }

        public bool CheckFinishedTasks() 
        {
            EmployeeSubTasksView temp = new EmployeeSubTasksView();
            bool status = true;

            foreach (var item in CurrentSubTasksList)
            {
                temp = item as EmployeeSubTasksView;
                if (temp.is_Finished == true) 
                {
                    status = false;
                    break;
                }
            }
            return status;
        }

        public ICommand RemoveSubtaskEmp
        {
            get { return new RelayCommand(RemoveSubtaskEmployees, RemoveSubtaskEmployeesCanExecute); }
        }

        private bool RemoveSubtaskEmployeesCanExecute()
        {
            if (CurrrentSubtaskEmployee != null && CurrrentSubtaskEmployee.Count > 0 && CurrentTask!= null)
                return true;
            else
                return false;
        }

        private void RemoveSubtaskEmployees()
        {
            EmployeeSubTasksDetailView temp = new EmployeeSubTasksDetailView();
            string EmpList = string.Empty;

            foreach (var item in CurrrentSubtaskEmployee)
            {
                temp = item as EmployeeSubTasksDetailView;

                if (temp.is_accepted == true)
                    EmpList += temp.emp_id + " ,";
                else
                    ListAllEmployees.Remove(temp);
            }

            if (EmpList != string.Empty)
            {
                EmpList = EmpList.Remove(EmpList.Length - 1, 1);
                clsMessages.setMessage(EmpList + " " + " Cannot be Removed as they have been accepted");
            }

            SubtaskEmployee = null;
            SubtaskEmployee = ListAllEmployees.Where(c => c.SubTask_ID == CurrentSubTask.SubTask_ID);
        }

        #endregion

        #region Filter Subtask Employees

        private void FilterSubtaskemployees()
        {
            EmployeeSubTasksView temp = new EmployeeSubTasksView();

            foreach (var item in CurrentSubTasksList)
            {
                temp = item as EmployeeSubTasksView;
                break;
            }
            SubtaskEmployee = null;
            SubtaskEmployee = ListAllEmployees.Where(c => c.SubTask_ID == temp.SubTask_ID);
        }

        #endregion

        #region Filter Subtasks

        private void FilterSubtasks()
        {
            if (ListAllSubtasks != null && ListAllSubtasks.Count > 0)
            {
                SubtaskEmployee = null;
                SubTasks = null;
                SubTasks = ListAllSubtasks.Where(c => c.Task_ID == CurrentTask.Task_ID);
            }
        }

        #endregion

        #region Save

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave())
            {
                try
                {
                    List<dtl_SubTasks> SaveSubtaskList = new List<dtl_SubTasks>();

                    foreach (var Subtask in SubTasks)
                    {
                        dtl_SubTasks SaveSubtaskObject = new dtl_SubTasks();
                        SaveSubtaskObject.Task_ID = (int)Subtask.Task_ID;
                        SaveSubtaskObject.SubTask_ID = Subtask.SubTask_ID;
                        SaveSubtaskObject.Task_Proprity_ID = Subtask.Task_Proprity_ID;
                        SaveSubtaskObject.evaluation_rate_group_id = Subtask.evaluation_rate_group_id;
                        SaveSubtaskObject.SubTaskName = Subtask.SubTaskName;
                        SaveSubtaskObject.SubTask_StartDate = Subtask.SubTask_StartDate;
                        SaveSubtaskObject.SubTask_StartTime = Subtask.SubTask_StartTime;
                        SaveSubtaskObject.SubTask_EndDate = Subtask.SubTask_EndDate;
                        SaveSubtaskObject.SubTask_EndTime = Subtask.SubTask_EndTime;
                        SaveSubtaskObject.Is_Reassignable = Subtask.Is_Reassignable;
                        SaveSubtaskObject.Is_active = Subtask.Is_active;
                        SaveSubtaskObject.is_Finished = Subtask.is_Finished;
                        SaveSubtaskObject.isdelete = Subtask.isdelete;
                        SaveSubtaskObject.save_datetime = DateTime.Now;
                        SaveSubtaskObject.save_user_id = clsSecurity.loggedUser.user_id;
                        SaveSubtaskObject.modified_user_id = clsSecurity.loggedUser.user_id;
                        SaveSubtaskObject.modified_datetime = DateTime.Now;

                        List<dtl_EmployeeSubTasks> SaveEmployeeList = new List<dtl_EmployeeSubTasks>();

                        foreach (var SubtaskEmp in ListAllEmployees.Where(c => c.SubTask_ID == Subtask.SubTask_ID))
                        {
                            dtl_EmployeeSubTasks SaveEmployeesObject = new dtl_EmployeeSubTasks();
                            SaveEmployeesObject.Employee_ID = SubtaskEmp.Employee_ID;
                            SaveEmployeesObject.SubTask_ID = Subtask.SubTask_ID;
                            SaveEmployeesObject.evaluation_rate_id = 0;
                            SaveEmployeesObject.Is_Reassigned = SubtaskEmp.Is_Reassigned;
                            SaveEmployeesObject.Is_active = SubtaskEmp.Is_active;
                            SaveEmployeesObject.is_accepted = SubtaskEmp.is_accepted;
                            SaveEmployeesObject.is_rejected = SubtaskEmp.is_rejected;
                            SaveEmployeesObject.is_pending = SubtaskEmp.is_pending;
                            SaveEmployeesObject.is_Finished = SubtaskEmp.is_Finished;
                            SaveEmployeesObject.is_Removed = SubtaskEmp.is_Removed;
                            SaveEmployeesObject.is_expired = SubtaskEmp.is_expired;
                            SaveEmployeesObject.isdelete = SubtaskEmp.isdelete;

                            SaveEmployeeList.Add(SaveEmployeesObject);
                        }

                        SaveSubtaskObject.dtl_EmployeeSubTasks = SaveEmployeeList.ToArray();
                        SaveSubtaskList.Add(SaveSubtaskObject);
                    }

                    if (serviceClient.SaveUpdateEmployeeSubtasks(SaveSubtaskList.ToArray()))
                        clsMessages.setMessage("Subtasks Saved/Updated Successfully.");
                    else
                        clsMessages.setMessage("Subtasks Save/Update Failed.");
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("Error occured while Saving/Updating records\n\n" + ex.Message.ToString());
                }

                New();
            }
        }

        private bool ValidateSave()
        {
            if (SubTasks == null || SubTasks.Count() == 0)
            {
                clsMessages.setMessage("Please Add Subtasks and try Again");
                return false;
            }
            else
            {
                bool Status = true;

                foreach (var subTask in SubTasks)
                {
                    if (subTask == null)
                    {
                        clsMessages.setMessage("Please Refresh and Try again");
                        Status = false;
                        break;
                    }
                    else if (subTask.Task_ID == null || subTask.Task_ID == 0)
                    {
                        clsMessages.setMessage("Please select a Task for the Current Subtask");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.SubTask_StartDate == null)
                    {
                        clsMessages.setMessage("Please Select a Subtask Start Date for the Current Subtask");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.SubTask_StartDate < CurrentTask.Task_StartDate || subTask.SubTask_StartDate > CurrentTask.Task_EndDate)
                    {
                        clsMessages.setMessage("Current Subtask Start Date Should be within the Task Date Range");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.SubTask_EndDate == null)
                    {
                        clsMessages.setMessage("Please Select a Subtask End Date for the Current Subtask");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.SubTask_EndDate < CurrentTask.Task_StartDate || subTask.SubTask_EndDate > CurrentTask.Task_EndDate)
                    {
                        clsMessages.setMessage("Current Subtask End Date Should be within the Task Date Range");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.SubTask_StartDate > subTask.SubTask_EndDate)
                    {
                        clsMessages.setMessage("Current Subtask Start Date cannot be greater than Subtask End Date");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.Task_Proprity_ID == 0 || subTask.Task_Proprity_ID == null)
                    {
                        clsMessages.setMessage("Please Select a Task Priority for the Current Subtask");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.evaluation_rate_group_id == 0 || subTask.evaluation_rate_group_id == null)
                    {
                        clsMessages.setMessage("Please Select a Rate Group for the Current Subtask");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (subTask.SubTaskName == null || subTask.SubTaskName == string.Empty)
                    {
                        clsMessages.setMessage("Please Enter a Subtask Name for the Current Subtask");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (ListAllSubtasks.Count(c => c.SubTask_ID == subTask.SubTask_ID && c.SubTaskName == subTask.SubTaskName) > 1)
                    {
                        clsMessages.setMessage("Current Subtask Name already exists");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                    else if (ListAllEmployees.Count(c => c.SubTask_ID == subTask.SubTask_ID) == 0)
                    {
                        clsMessages.setMessage("Current Subtask Doesn't have any Assigned employees");
                        CurrentSubTask = subTask;
                        Status = false;
                        break;
                    }
                }

                return Status;
            }
        }

        #endregion

        #region Delete

        public ICommand DeleteButton 
        {
            get { return new RelayCommand(DeleteSubtask,DeleteSubtaskCanExecute); }
        }

        private bool DeleteSubtaskCanExecute()
        {
            if (CurrentSubTasksList != null && CurrentSubTasksList.Count > 0)
                return true;
            else
                return false;
        }

        private void DeleteSubtask()
        {
            List<dtl_SubTasks> DeleteList = new List<dtl_SubTasks>();
            string SubtaskList = string.Empty;

            foreach (var DeleteSubtask in CurrentSubTasksList)
            {
                EmployeeSubTasksView DeleteObj = DeleteSubtask as EmployeeSubTasksView;

                if (DeleteObj.SubTask_ID < 0)
                {
                    ListAllEmployees.RemoveAll(c => c.SubTask_ID == DeleteObj.SubTask_ID);
                    ListAllSubtasks.Remove(DeleteObj);
                    SubTasks = null;
                    SubTasks = ListAllSubtasks.Where(c => c.Task_ID == DeleteObj.Task_ID);
                }
                else 
                {
                    if (ListAllEmployees.Count(c => c.SubTask_ID == DeleteObj.SubTask_ID && c.is_accepted == true) > 0)
                    {
                        SubtaskList += DeleteObj.SubTaskName + ",\n";
                    }
                    else 
                    {
                        dtl_SubTasks Subtask = new dtl_SubTasks();
                        Subtask.SubTask_ID = DeleteObj.SubTask_ID;
                        Subtask.SubTaskName = DeleteObj.SubTaskName;
                        Subtask.delete_datetime = DateTime.Now;
                        Subtask.delete_user_id = clsSecurity.loggedUser.user_id;

                        DeleteList.Add(Subtask);
                    }
                }
            }

            if (DeleteList.Count>0)
            {
                clsMessages.setMessage("Are you sure that you want to delete These Subtasks?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (SubtaskList != string.Empty)
                    {
                        SubtaskList = SubtaskList.Remove(SubtaskList.Length - 2);

                        if (serviceClient.DeleteEmployeesSubtasks(DeleteList.ToArray()))
                            clsMessages.setMessage("Subtasks Deleted Successfully Except \n"+SubtaskList+"\nWhich are Currently Active");
                        else
                            clsMessages.setMessage("Subtasks Delete Failed");
                    }
                    else
                    {
                        if (serviceClient.DeleteEmployeesSubtasks(DeleteList.ToArray()))
                            clsMessages.setMessage("Subtasks Deleted Successfully");
                        else
                            clsMessages.setMessage("Subtasks Delete Failed");
                    }  
                }
            }

            New();
        }

        #endregion

        #region Refresh Window

        public ICommand RefreshCurrent 
        {
            get { return new RelayCommand(RefreshCurrentsubtask); }
        }

        private void RefreshCurrentsubtask()
        {
            if (CurrentSubTask == null)
            {
                CurrentSubTask = new EmployeeSubTasksView();
            }
            else 
            {    
                EmployeeSubTasksView TempCurrentSubTask = new EmployeeSubTasksView();
                TempCurrentSubTask.Task_ID = CurrentSubTask.Task_ID == null ? 0 : CurrentSubTask.Task_ID;
                TempCurrentSubTask.SubTask_ID = 0;
                TempCurrentSubTask.Task_Proprity_ID = CurrentSubTask.Task_Proprity_ID;
                TempCurrentSubTask.evaluation_rate_group_id = CurrentSubTask.evaluation_rate_group_id;
                TempCurrentSubTask.SubTask_StartDate = CurrentSubTask.SubTask_StartDate == null ? null : CurrentSubTask.SubTask_StartDate;
                TempCurrentSubTask.SubTask_EndDate = CurrentSubTask.SubTask_EndDate == null ? null : CurrentSubTask.SubTask_EndDate;

                CurrentSubTask = null;
                CurrentSubTask = TempCurrentSubTask;
            }
        }

        #endregion

        #endregion
    }
}
