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
    class TaskSubCategoriesViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<TaskSubcategoriesView> SubcategoriesList;

        #endregion

        #region Constructor

        public TaskSubCategoriesViewModel()
        {
            serviceClient = new ERPServiceClient();
            SubcategoriesList = new List<TaskSubcategoriesView>();
            SearchIndex = 0;
            New();
        }

        #endregion

        #region Properties

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); if (SubcategoriesList != null && SubcategoriesList.Count > 0) Search(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

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

        #endregion

        #region RefreshMethods

        private void refreshTaskSubcategories()
        {
            serviceClient.GetTaskSubcategoriesCompleted += (s, e) =>
            {
                try
                {
                    TaskSubcategories = e.Result;
                    if (TaskSubcategories != null)
                    {
                        SubcategoriesList = null;
                        SubcategoriesList = TaskSubcategories.ToList();
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage(ex.Message);
                }
            };
            serviceClient.GetTaskSubcategoriesAsync();
        }

        private void refreshTaskCategories()
        {
            serviceClient.GetTaskCategoriesCompleted += (s, e) =>
            {
                try
                {
                    TaskCategories = e.Result.Where(c => c.Is_active == true);
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
            refreshTaskSubcategories();
            refreshTaskCategories();
            CurrentTaskSubcategory = new TaskSubcategoriesView();
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanexecute); }
        }

        private bool SaveCanexecute()
        {
            if (CurrentTaskCategory != null && CurrentTaskSubcategory != null && CurrentTaskSubcategory.Task_Subcategory_Name != null && CurrentTaskSubcategory.Task_Subcategory_Description != null)
                return true;
            else
                return false;
        }

        private void Save()
        {
            if (CurrentTaskSubcategory.Task_Subcategory_ID == 0)
            {
                if (SubcategoriesList.Count(c => c.Task_Subcategory_Name.ToUpper() == CurrentTaskSubcategory.Task_Subcategory_Name.ToUpper()) == 0)
                {
                    mas_TaskSubCategory SaveObject = new mas_TaskSubCategory();
                    SaveObject.Task_Category_ID = CurrentTaskCategory.Task_Category_ID;
                    SaveObject.Task_Subcategory_Name = CurrentTaskSubcategory.Task_Subcategory_Name;
                    SaveObject.Task_Subcategory_Description = CurrentTaskSubcategory.Task_Subcategory_Description;
                    SaveObject.save_user_id = clsSecurity.loggedUser.user_id;
                    SaveObject.save_datetime = DateTime.Now;
                    SaveObject.Is_active = true;
                    SaveObject.isdelete = false;

                    if (serviceClient.SaveTaskSubcategories(SaveObject))
                        clsMessages.setMessage("Task Subcategory Saved Successfully.");
                    else
                        clsMessages.setMessage("Task Subcategory Save Failed.");

                    New();
                }

                else
                    clsMessages.setMessage("Task Subcategory Name already Exists.");
            }

            else
            {
                if (SubcategoriesList.Count(c => c.Task_Subcategory_Name.ToUpper() == CurrentTaskSubcategory.Task_Subcategory_Name.ToUpper() && c.Task_Subcategory_ID != CurrentTaskSubcategory.Task_Subcategory_ID) > 0)
                    clsMessages.setMessage("Task Subcategory Name already Exists.");
                else
                {
                    int Result;

                    mas_TaskSubCategory SaveObject = new mas_TaskSubCategory();
                    SaveObject.Task_Category_ID = CurrentTaskCategory.Task_Category_ID;
                    SaveObject.Task_Subcategory_ID = CurrentTaskSubcategory.Task_Subcategory_ID;
                    SaveObject.Task_Subcategory_Name = CurrentTaskSubcategory.Task_Subcategory_Name;
                    SaveObject.Task_Subcategory_Description = CurrentTaskSubcategory.Task_Subcategory_Description;
                    SaveObject.Is_active = CurrentTaskSubcategory.Is_active;
                    SaveObject.modified_user_id = clsSecurity.loggedUser.user_id;
                    SaveObject.modified_datetime = DateTime.Now;

                    Result = serviceClient.UpdateTaskSubcategories(SaveObject);

                    if (Result == 1)
                        clsMessages.setMessage("Task Subcategory Updated Successfully.");
                    else if(Result == 2)
                        clsMessages.setMessage("This Subcategory Contains Tasks and Cannot be Modified");
                    else
                        clsMessages.setMessage("Task Subcategory Update Failed");

                    New();
                }
            }
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanexecute); }
        }

        private bool DeleteCanexecute()
        {
            if (CurrentTaskSubcategory != null && TaskSubcategories != null && TaskSubcategories.Count() > 0 && TaskSubcategories.Count(c => c.Task_Subcategory_ID == CurrentTaskSubcategory.Task_Subcategory_ID) > 0)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            clsMessages.setMessage("Are you sure that you want to Delete this Subcategory?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                int Result;

                mas_TaskSubCategory DeleteObject = new mas_TaskSubCategory();
                DeleteObject.Task_Subcategory_ID = CurrentTaskSubcategory.Task_Subcategory_ID;
                DeleteObject.Task_Category_ID = CurrentTaskCategory.Task_Category_ID;
                DeleteObject.delete_user_id = clsSecurity.loggedUser.user_id;
                DeleteObject.delete_datetime = DateTime.Now;
                DeleteObject.isdelete = true;
                DeleteObject.Is_active = false;

                Result = serviceClient.DeleteTaskSubcategories(DeleteObject);

                if (Result == 1)
                    clsMessages.setMessage("Task Subcategory Deleted Successfully");
                else if(Result == 2)
                    clsMessages.setMessage("This Subcategory Contains Tasks and Cannot be Deleted");
                else
                    clsMessages.setMessage("Task Subcategory Delete Failed");

                New();
            }
        }

        private void Search()
        {
            try
            {
                TaskSubcategories = null;

                try
                {
                    if (SearchIndex == 0)
                        TaskSubcategories = SubcategoriesList.Where(c => c.Task_Subcategory_Name != null && c.Task_Subcategory_Name.ToUpper().Contains(SearchText.ToUpper()));
                    if (SearchIndex == 1)
                        TaskSubcategories = SubcategoriesList.Where(c => c.Task_Subcategory_Description != null && c.Task_Subcategory_Description.ToUpper().Contains(SearchText.ToUpper()));
                    if (SearchIndex == 2)
                        TaskSubcategories = SubcategoriesList.Where(c => c.Task_Category_Name != null && c.Task_Category_Name.ToUpper().Contains(SearchText.ToUpper()));
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage(ex.Message);
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }

        #endregion
    }
}
