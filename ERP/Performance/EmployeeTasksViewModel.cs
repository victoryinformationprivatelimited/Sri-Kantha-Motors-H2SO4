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
using System.Collections;

namespace ERP.Performance
{
    class EmployeeTasksViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<TaskSubcategoriesView> ListSubCategories;
        List<EmployeeTasksView> ListTasks;
        EmployeeSubTasksDetailForUserView Reassinged = null;

        #endregion

        #region Constructor

        public EmployeeTasksViewModel(EmployeeSubTasksDetailForUserView Subtask) 
        {
            serviceClient = new ERPServiceClient();
            Reassinged = Subtask;
            ListSubCategories = new List<TaskSubcategoriesView>();
            ListTasks = new List<EmployeeTasksView>();
            SearchIndex = 0;
            New();
        }

        public EmployeeTasksViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListSubCategories = new List<TaskSubcategoriesView>();
            ListTasks = new List<EmployeeTasksView>();
            SearchIndex = 0;
            New();
        }

        #endregion

        #region Properties

        private bool _CategoryEnabled;
        public bool CategoryEnabled
        {
            get { return _CategoryEnabled; }
            set { _CategoryEnabled = value; OnPropertyChanged("CategoryEnabled"); }
        }

        private bool _SubCategoryEnabled;
        public bool SubCategoryEnabled
        {
            get { return _SubCategoryEnabled; }
            set { _SubCategoryEnabled = value; OnPropertyChanged("SubCategoryEnabled"); }
        }

        private bool _StartDateEnabled;
        public bool StartDateEnabled
        {
            get { return _StartDateEnabled; }
            set { _StartDateEnabled = value; OnPropertyChanged("StartDateEnabled"); }
        }

        private bool _EndDateEnabled;
        public bool EndDateEnabled
        {
            get { return _EndDateEnabled; }
            set { _EndDateEnabled = value; OnPropertyChanged("EndDateEnabled"); }
        }

        private EmployeeTasksWindow _Window;
        public EmployeeTasksWindow Window
        {
            get { return _Window; }
            set { _Window = value; OnPropertyChanged("Window"); }
        }
        
        

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged("StartDate"); }
        }

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; OnPropertyChanged("EndDate"); }
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
            set { _CurrentTask = value; OnPropertyChanged("CurrentTask"); if (CurrentTask != null) { if (CurrentTask.Task_From != 0) EnableDisableElements(false); else EnableDisableElements(true); } }
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
            set { _CurrentSupervisor = value; OnPropertyChanged("CurrentSupervisor"); if (CurrentSupervisor != null) SupervisorName = CurrentSupervisor.first_name; }
        }

        private IEnumerable<usr_UserEmployee> _UserEmployee;
        public IEnumerable<usr_UserEmployee> UserEmployee
        {
            get { return _UserEmployee; }
            set { _UserEmployee = value; OnPropertyChanged("UserEmployee"); }
        }

        private IEnumerable<z_TaskCategory> _TaskCategory;
        public IEnumerable<z_TaskCategory> TaskCategory
        {
            get { return _TaskCategory; }
            set { _TaskCategory = value; OnPropertyChanged("TaskCategory"); }
        }

        private z_TaskCategory _CurrentTaskCategory;
        public z_TaskCategory CurrentTaskCategory
        {
            get { return _CurrentTaskCategory; }
            set { _CurrentTaskCategory = value; OnPropertyChanged("CurrentTaskCategory"); if (CurrentTaskCategory != null && ListSubCategories != null && ListSubCategories.Count > 0) {TaskSubcategories = ListSubCategories.Where(c => c.Task_Category_ID == CurrentTaskCategory.Task_Category_ID); if(CurrentTask != null && CurrentTask.Task_ID != 0) CurrentTaskSubcategory = TaskSubcategories.FirstOrDefault(c=> c.Task_Subcategory_ID == CurrentTask.Task_Subcategory_ID);} }
        }
        
        private IEnumerable<TaskSubcategoriesView> _TaskSubcategories;
        public IEnumerable<TaskSubcategoriesView> TaskSubcategories
        {
            get { return _TaskSubcategories; }
            set { _TaskSubcategories = value; OnPropertyChanged("TaskSubcategories"); }
        }

        private TaskSubcategoriesView _CurrentTaskSubcategory;
        public TaskSubcategoriesView CurrentTaskSubcategory
        {
            get { return _CurrentTaskSubcategory; }
            set { _CurrentTaskSubcategory = value; OnPropertyChanged("CurrentTaskSubcategory"); }
        }

        private string _SupervisorName;
        public string SupervisorName
        {
            get { return _SupervisorName; }
            set { _SupervisorName = value; OnPropertyChanged("SupervisorName"); }
        }

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); if (ListTasks != null && ListTasks.Count > 0) Search(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        
        #endregion

        #region RefreshMethods

        private void RefreshTasks()
        {
            serviceClient.GetEmployeeTasksCompleted += (s, e) =>
            {
                try
                {
                    Tasks = e.Result;
                    if (Tasks != null && Tasks.Count() > 0) 
                    {
                        ListTasks = Tasks.ToList();
                    }
                    CurrentTask = new EmployeeTasksView();
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshTasks() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeTasksAsync(clsSecurity.loggedUser.user_id);
        }

        private void RefreshTasks(EmployeeSubTasksDetailForUserView Subtask)
        {
            serviceClient.GetEmployeeTaskBySubtaskCompleted += (s, e) =>
            {
                try
                {
                    EmployeeTasksView Temp = e.Result;

                    if (Temp != null)
                    {
                        CurrentTask = new EmployeeTasksView();

                        CurrentTask.Task_Category_ID = Temp.Task_Category_ID;
                        CurrentTask.Task_Subcategory_ID = Temp.Task_Subcategory_ID;
                        CurrentTask.Task_StartDate = Subtask.SubTask_StartDate;
                        CurrentTask.Task_EndDate = Subtask.SubTask_EndDate;
                        CurrentTask.Task_From = (int)Subtask.SubTask_ID;
                        CurrentTask.Task_From_Employee = Subtask.Employee_ID;

                        EnableDisableElements(false);
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshTasks() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeTaskBySubtaskAsync(Reassinged);
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
            serviceClient.GetAllEmployeeSupervisorsAsync(new Guid("46181184-5CA4-4622-813C-2AE2A3513A40"));
        }

        private void RsfreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                try
                {
                    UserEmployee = e.Result;
                    if (UserEmployee != null && UserEmployee.Count() > 0 && Supervisors != null && Supervisors.Count() > 0)
                    {
                        if (UserEmployee.Count(c=>c.user_id == clsSecurity.loggedUser.user_id) > 0)
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

        private void RefreshCategories()
        {
            serviceClient.GetTaskCategoriesCompleted += (s, e) => 
            {
                try
                {
                    TaskCategory = e.Result.Where(c=> c.Is_active == true);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshCategories() \n\n" + ex.Message);
                }
            };
            serviceClient.GetTaskCategoriesAsync();
        }

        private void RefreshSubCategories()
        {
            serviceClient.GetTaskSubcategoriesCompleted += (s, e) =>
            {
                try
                {
                    TaskSubcategories = e.Result.Where(c=> c.Is_active == true);
                    if (TaskSubcategories != null && TaskSubcategories.Count() > 0)
                    {
                        ListSubCategories = TaskSubcategories.ToList();
                        TaskSubcategories = null;
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshSubCategories() \n\n" + ex.Message);
                }
            };
            serviceClient.GetTaskSubcategoriesAsync();
        }

        #endregion

        #region Commands & Methods

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            if (Reassinged == null)
            {
                RefreshSupervisor();
                RsfreshUserEmployee();
                RefreshTasks();
                RefreshCategories();
                RefreshSubCategories();
                SearchIndex = 0;
            }
            else 
            {
                try
                {
                    RefreshSupervisor();
                    RsfreshUserEmployee();
                    RefreshCategories();
                    RefreshSubCategories();
                    RefreshTasks(Reassinged);
                    SearchIndex = 0;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("An error Occured while Refreshing the Data.");
                }
            }
        }

        private void EnableDisableElements(bool Action)
        {
            if (Action)
            {
                CategoryEnabled = true;
                SubCategoryEnabled = true;
                StartDateEnabled = true;
                EndDateEnabled = true;
            }
            else 
            {
                CategoryEnabled = false;
                SubCategoryEnabled = false;
                StartDateEnabled = false;
                EndDateEnabled = false;
            }
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        
        private void Save()
        {
            if (Validate())
            {
                mas_Tasks SaveTask;

                if (CurrentTask.Task_ID == 0)
                {
                    SaveTask = new mas_Tasks();
                    SaveTask.Task_Subcategory_ID = CurrentTaskSubcategory.Task_Subcategory_ID;
                    SaveTask.Task_From = 0;
                    SaveTask.Task_Name = CurrentTask.Task_Name;
                    SaveTask.Task_Description = CurrentTask.Task_Description;
                    SaveTask.Task_StartDate = CurrentTask.Task_StartDate;
                    SaveTask.Task_EndDate = CurrentTask.Task_EndDate;
                    SaveTask.Task_Supervisior = CurrentSupervisor.supervisor_employee_id;
                    SaveTask.Is_active = true;
                    SaveTask.is_Finished = false;
                    SaveTask.isdelete = false;
                    SaveTask.save_user_id = clsSecurity.loggedUser.user_id;
                    SaveTask.save_datetime = DateTime.Now;

                    if (Reassinged != null)
                    {
                        SaveTask.Task_From = (int)Reassinged.SubTask_ID;
                        SaveTask.Task_From_Employee = Reassinged.Employee_ID;

                        if (serviceClient.SaveEmployeeTasks(SaveTask, true))
                            clsMessages.setMessage("You have just Created a Task, now you can Create new Subtasks and assign employees in the Subtasks Window");
                        else
                            clsMessages.setMessage("Task Save Failed.");

                        Window.Close();
                    }

                    else 
                    {
                        if (serviceClient.SaveEmployeeTasks(SaveTask, false))
                            clsMessages.setMessage("Task Saved Successfully.");
                        else
                            clsMessages.setMessage("Task Save Failed.");
                    }
                }

                else 
                {
                    if (CurrentTask.is_Finished != true)
                    {
                        int Result;

                        SaveTask = new mas_Tasks();
                        SaveTask.Task_ID = CurrentTask.Task_ID;
                        SaveTask.Task_Subcategory_ID = CurrentTaskSubcategory.Task_Subcategory_ID;
                        SaveTask.Task_Name = CurrentTask.Task_Name;
                        SaveTask.Task_Description = CurrentTask.Task_Description;
                        SaveTask.Task_StartDate = CurrentTask.Task_StartDate;
                        SaveTask.Task_EndDate = CurrentTask.Task_EndDate;
                        SaveTask.Task_Supervisior = CurrentSupervisor.supervisor_employee_id;
                        SaveTask.Is_active = CurrentTask.Is_active;
                        SaveTask.is_Finished = CurrentTask.is_Finished;
                        SaveTask.isdelete = CurrentTask.isdelete;
                        SaveTask.modified_user_id = clsSecurity.loggedUser.user_id;
                        SaveTask.modified_datetime = DateTime.Now;

                        Result = serviceClient.UpdateEmployeeTasks(SaveTask);

                        if (Result == 1)
                            clsMessages.setMessage("Task Updated Successfully.");
                        else if (Result == 2)
                            clsMessages.setMessage("This Task Contains Subtasks and Cannot be Modified.");
                        else
                            clsMessages.setMessage("Task Update Failed."); 
                    }
                    else
                        clsMessages.setMessage("Finished Tasks Cannot be Modified"); 
                }

                New();
            }
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExcute); }
        }

        private bool DeleteCanExcute()
        {
            if (CurrentTask != null && CurrentTask.Task_ID != 0)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            clsMessages.setMessage("Are you sure that you want to delete this Task?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                int Result;

                mas_Tasks DeleteTask = new mas_Tasks();

                DeleteTask.Task_ID = CurrentTask.Task_ID;
                DeleteTask.Is_active = false;
                DeleteTask.isdelete = true;
                DeleteTask.delete_user_id = clsSecurity.loggedUser.user_id;
                DeleteTask.delete_datetime = DateTime.Now;

                Result = serviceClient.DeleteEmployeeTasks(DeleteTask);

                if (Result == 1)
                    clsMessages.setMessage("Task Deleted Successfully.");
                else if (Result == 2)
                    clsMessages.setMessage("This Task Contains Subtasks and Cannot be Deleted.");
                else
                    clsMessages.setMessage("Task Delete Failed.");

                New(); 
            }
        }

        private bool Validate()
        {
            if (CurrentTask == null)
            {
                clsMessages.setMessage("Please Refresh And Try Again");
                return false;
            }
            else if (CurrentTaskCategory == null)
            {
                clsMessages.setMessage("Please Select a Task category");
                return false;
            }
            else if (CurrentTaskSubcategory == null)
            {
                clsMessages.setMessage("Please Select a Task sub category");
                return false;
            }
            else if (CurrentTask.Task_Name == null || CurrentTask.Task_Name == string.Empty)
            {
                clsMessages.setMessage("Task Name Cannot Be Empty");
                return false;
            }
            else if (CurrentTask.Task_StartDate == null || CurrentTask.Task_EndDate == null)
            {
                clsMessages.setMessage("Either 'Start Date' or 'End Date' is Invalid");
                return false;
            }
            else if (CurrentTask.Task_EndDate < CurrentTask.Task_StartDate)
            {
                clsMessages.setMessage("Task 'Start Date' cannot be greater than Task 'End Date'");
                return false;
            }
            else if (CurrentSupervisor == null)
            {
                clsMessages.setMessage("Current user Is not a Supervisor");
                return false;
            }
           
            else if (UserEmployee.Count(c => c.employee_id == CurrentSupervisor.supervisor_employee_id) == 0)
            {
                clsMessages.setMessage("Current supervisor is not a User");
                return false;
            }
            else if (UserEmployee.FirstOrDefault(c => c.employee_id == CurrentSupervisor.supervisor_employee_id).user_id != clsSecurity.loggedUser.user_id)
            {
                clsMessages.setMessage("Current supervisor doesn't have permission to set this Task");
                return false;
            }
            else
                return true;
        }

        private void Search()
        {
            try
            {
                Tasks = null;

                if (SearchIndex == 0)
                    Tasks = ListTasks.Where(c => c.Task_Name != null && c.Task_Name.ToUpper().Contains(SearchText.ToUpper()));
                else if (SearchIndex == 1)
                    Tasks = ListTasks.Where(c => c.Task_Description.ToString().ToUpper().Contains(SearchText.ToUpper()));
                else if (SearchIndex == 2)
                    Tasks = ListTasks.Where(c => c.Task_StartDate.Value.ToString().Contains(SearchText.ToUpper()));
                else if (SearchIndex == 3)
                    Tasks = ListTasks.Where(c => c.Task_EndDate.Value.ToString().Contains(SearchText.ToUpper()));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #endregion
    }
}
