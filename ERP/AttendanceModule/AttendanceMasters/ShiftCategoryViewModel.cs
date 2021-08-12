using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.AttendanceService;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class ShiftCategoryViewModel:ViewModelBase
    {
        #region ServiceClient

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public ShiftCategoryViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshShiftCategories();
            this.New();
        }

        #endregion

        #region Properties

        IEnumerable<z_Shift_Category> shiftCategories;
        public IEnumerable<z_Shift_Category> ShiftCategories
        {
            get { return shiftCategories; }
            set { shiftCategories = value; OnPropertyChanged("ShiftCategories"); }
        }

        z_Shift_Category currentShiftCategory;
        public z_Shift_Category CurrentShiftCategory
        {
            get { return currentShiftCategory; }
            set 
            { 
                currentShiftCategory = value; OnPropertyChanged("CurrentShiftCategory"); 
                if(currentShiftCategory != null)
                {
                    this.SetCurrentDetails();
                }
            }
        }

        string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; OnPropertyChanged("CategoryName"); }
        }

        string categoryDescription;
        public string CategoryDescription
        {
            get { return categoryDescription; }
            set { categoryDescription = value; OnPropertyChanged("CategoryDescription"); }
        }

        bool? isActive;
        public bool? IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        #endregion

        #region Refresh Methods

        // Get current non-deleted shift categories
        void RefreshShiftCategories()
        {

            attendServiceClient.GetShiftCategoryDetailsCompleted += (s, e) =>
            {
                try
                {
                    ShiftCategories = e.Result;
                }
                catch (Exception)
                {

                    clsMessages.setMessage("Shift categories refresh is failed");
                }
            };
            attendServiceClient.GetShiftCategoryDetailsAsync();
        }

        #endregion

        #region Button Methods

        #region New Button

        // Clear all filled data and initialize new shift category 
        void New()
        {
            CurrentShiftCategory = null;
            CurrentShiftCategory = new z_Shift_Category();
            CategoryName = "";
            CategoryDescription = "";
            IsActive = false;
        }

        // binding to New Button 
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Save Button

        void Save()
        {
            try
            {
                if (shiftCategories.Count(c => c.shift_category_name == categoryName && c.shift_category_id != currentShiftCategory.shift_category_id) == 0)
                {
                    if (currentShiftCategory.shift_category_id == 0)
                    {
                        if (clsSecurity.GetSavePermission(301))
                        {
                            // Adding new shift category
                            z_Shift_Category addingCategory = new z_Shift_Category();
                            addingCategory.shift_category_name = categoryName;
                            addingCategory.description = categoryDescription;
                            addingCategory.is_active = isActive;
                            addingCategory.is_delete = false;
                            addingCategory.save_user_id = clsSecurity.loggedUser.user_id;
                            addingCategory.save_datetime = DateTime.Now;
                            if (attendServiceClient.SaveShiftCategory(addingCategory))
                            {
                                clsMessages.setMessage("Save is successful");
                                this.Reset();
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
                        if (clsSecurity.GetUpdatePermission(301))
                        {
                            // Updating current shift category
                            z_Shift_Category updatingCategory = new z_Shift_Category();
                            updatingCategory.shift_category_id = currentShiftCategory.shift_category_id;
                            updatingCategory.shift_category_name = categoryName;
                            updatingCategory.description = categoryDescription;
                            updatingCategory.is_active = isActive;
                            updatingCategory.modified_user_id = clsSecurity.loggedUser.user_id;
                            updatingCategory.modified_datetime = DateTime.Now;
                            if (attendServiceClient.UpdateShiftCategory(updatingCategory))
                            {
                                clsMessages.setMessage("Update is successful");
                                this.Reset();
                            }
                            else
                            {
                                clsMessages.setMessage("Update is failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
                else
                {
                    clsMessages.setMessage("Category name is already exists");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Save/Update cannot be proceed");
            }
        }

        bool SaveCanExecute()
        {
            if (currentShiftCategory == null)
                return false;
            if (categoryName == null || categoryName == string.Empty)
                return false;
            if (categoryDescription == null || categoryDescription == string.Empty)
                return false;
            return true;
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        #endregion

        #region Delete Button

        void Delete()
        {
            if (clsSecurity.GetDeletePermission(301))
            {
                z_Shift_Category deletingCategory = new z_Shift_Category();
                deletingCategory.shift_category_id = currentShiftCategory.shift_category_id;
                deletingCategory.is_delete = true;
                deletingCategory.delete_datetime = DateTime.Now;
                deletingCategory.delete_user_id = clsSecurity.loggedUser.user_id;
                if (attendServiceClient.DeleteShiftCategory(deletingCategory))
                {
                    clsMessages.setMessage("Delete is successful");
                    this.Reset();
                }
                else
                {
                    clsMessages.setMessage("Delete is failed");
                }
            }
            else
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }

        bool DeleteCanExecute()
        {
            if (currentShiftCategory == null)
                return false;
            if (currentShiftCategory.shift_category_id == 0)
                return false;
            return true;
        }

        #endregion

        #endregion

        #region Reset methods

        void Reset()
        {
            this.New();
            this.RefreshShiftCategories();
        }

        #endregion

        #region Data Set Methods

        void SetCurrentDetails()
        {
            CategoryName = currentShiftCategory.shift_category_name;
            CategoryDescription = currentShiftCategory.description;
            IsActive = currentShiftCategory.is_active;
        }

        #endregion
    }
}
