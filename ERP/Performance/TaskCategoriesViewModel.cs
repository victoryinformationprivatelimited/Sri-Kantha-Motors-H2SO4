using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Performance
{
    class TaskCategoriesViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<z_TaskCategory> ListTaskCategories;

        #endregion

        #region Constructor

        public TaskCategoriesViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListTaskCategories = new List<z_TaskCategory>();
            SearchIndex = 0;
            New();
        }

        #endregion

        #region Properties

        private IEnumerable<z_TaskCategory> _TaskCategories;
        public IEnumerable<z_TaskCategory> TaskCategories
        {
            get { return _TaskCategories; }
            set { _TaskCategories = value; OnPropertyChanged("TaskCategories"); }
        }

        private z_TaskCategory _CurrentTaskCategory;
        public z_TaskCategory CurrentTaskCategory
        {
            get { return _CurrentTaskCategory; }
            set { _CurrentTaskCategory = value; OnPropertyChanged("CurrentTaskCategory"); }
        }

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); if (ListTaskCategories != null && ListTaskCategories.Count > 0) SearchTaskCategory(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }


        #endregion

        #region RefreshMethods

        private void RefreshTaskCategories()
        {
            serviceClient.GetTaskCategoriesCompleted += (s, e) =>
            {
                try
                {
                    TaskCategories = e.Result;
                    if (TaskCategories != null)
                    {
                        ListTaskCategories = null;
                        ListTaskCategories = TaskCategories.ToList();
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage(ex.Message);
                }
            };
            serviceClient.GetTaskCategoriesAsync();
        }

        #endregion

        #region Commands & Methods

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            SearchText = string.Empty;
            SearchIndex = 0;
            RefreshTaskCategories();
            CurrentTaskCategory = new z_TaskCategory();
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        private bool SaveCanExecute()
        {
            if (CurrentTaskCategory != null && CurrentTaskCategory.Task_Category_Name != null && CurrentTaskCategory.Task_Category_Description != null && CurrentTaskCategory.Task_Category_Name != string.Empty && CurrentTaskCategory.Task_Category_Description != string.Empty)
                return true;
            else
                return false;
        }

        private void Save()
        {
            if (ListTaskCategories.Count(c => c.Task_Category_ID == CurrentTaskCategory.Task_Category_ID) == 0)
            {
                if (ListTaskCategories.Count(c => c.Task_Category_Name.ToUpper() == CurrentTaskCategory.Task_Category_Name.ToUpper()) == 0)
                {
                    CurrentTaskCategory.save_user_id = clsSecurity.loggedUser.user_id;
                    CurrentTaskCategory.save_datetime = DateTime.Now;
                    CurrentTaskCategory.isdelete = false;
                    CurrentTaskCategory.Is_active = true;

                    if (serviceClient.SaveTaskCategories(CurrentTaskCategory))
                        clsMessages.setMessage("Task Category Saved Successfully.");
                    else
                        clsMessages.setMessage("Task Category Save Failed.");

                    New();
                }

                else
                    clsMessages.setMessage("Task Category Name already Exists.");
            }

            else
            {
                if (ListTaskCategories.Count(c => c.Task_Category_Name.ToUpper() == CurrentTaskCategory.Task_Category_Name.ToUpper() && c.Task_Category_ID != CurrentTaskCategory.Task_Category_ID) > 0)
                    clsMessages.setMessage("Task Category Name already Exists.");
                else
                {
                    int Result;

                    CurrentTaskCategory.modified_user_id = clsSecurity.loggedUser.user_id;
                    CurrentTaskCategory.modified_datetime = DateTime.Now;

                    Result = serviceClient.UpdateTaskCategories(CurrentTaskCategory);

                    if (Result == 1)
                        clsMessages.setMessage("Task Category Updated Successfully.");
                    else if (Result == 2)
                        clsMessages.setMessage("This Task Category Contains SubCategories and Cannot be Modified.");
                    else
                        clsMessages.setMessage("Task Category Update Failed.");

                    New();
                }
            }
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }

        private bool DeleteCanExecute()
        {
            if (CurrentTaskCategory != null && ListTaskCategories.Count(c => c.Task_Category_ID == CurrentTaskCategory.Task_Category_ID) > 0)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            clsMessages.setMessage("Are you sure that you want to delete this Category?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                int Result;

                CurrentTaskCategory.isdelete = true;
                CurrentTaskCategory.Is_active = false;
                CurrentTaskCategory.delete_user_id = clsSecurity.loggedUser.user_id;
                CurrentTaskCategory.delete_datetime = DateTime.Now;

                Result = serviceClient.DeleteTaskCategories(CurrentTaskCategory);

                if (Result == 1)
                    clsMessages.setMessage("Task Category Deleted Successfully.");
                else if (Result == 2)
                    clsMessages.setMessage("This Task Category Contains SubCategories and Cannot be Deleted.");
                else
                    clsMessages.setMessage("Task Category Delete Failed.");

                New();
            }
        }

        private void SearchTaskCategory()
        {
            TaskCategories = null;

            try
            {
                if (SearchIndex == 0)
                    TaskCategories = ListTaskCategories.Where(c => c.Task_Category_Name != null && c.Task_Category_Name.ToUpper().Contains(SearchText.ToUpper()));
                if (SearchIndex == 1)
                    TaskCategories = ListTaskCategories.Where(c => c.Task_Category_Description != null && c.Task_Category_Description.ToUpper().Contains(SearchText.ToUpper()));
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }

        #endregion
    }
}
