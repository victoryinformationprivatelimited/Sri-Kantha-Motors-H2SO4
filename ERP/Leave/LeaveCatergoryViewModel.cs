using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Masters
{
    class LeaveCatergoryViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Fields

        List<z_LeaveCategory> CategoryList = new List<z_LeaveCategory>();

        #endregion

        #region Constructor
        public LeaveCatergoryViewModel()
        {
            CategoryList = new List<z_LeaveCategory>();
            this.refreshLeaveCatergories();
            this.New();

        }
        #endregion

        #region Properties
        private IEnumerable<z_LeaveCategory> _LeaveCatergory;
        public IEnumerable<z_LeaveCategory> LeaveCatergory
        {
            get
            {
                return this._LeaveCatergory;
            }
            set
            {
                this._LeaveCatergory = value;
                this.OnPropertyChanged("LeaveCatergory");

            }
        }

        private z_LeaveCategory _CurrentLeaveCatergory;
        public z_LeaveCategory CurrentLeaveCatergory
        {
            get
            {
                return this._CurrentLeaveCatergory;
            }
            set
            {
                this._CurrentLeaveCatergory = value;
                this.OnPropertyChanged("CurrentLeaveCatergory");

            }
        }

        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (Search != null)
                    SearchCategory();
            }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        #endregion

        #region LeaveCatergory List

        private void refreshLeaveCatergories()
        {
            try
            {
                this.serviceClient.GetLeaveCatergoriesCompleted += (s, e) =>
                {
                    this.LeaveCatergory = e.Result;
                    if (LeaveCatergory != null)
                        CategoryList = LeaveCatergory.ToList();
                    if (CategoryList.Count() > 0)
                        this.CreateCategory();
                };
                this.serviceClient.GetLeaveCatergoriesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error Occured while Fetching data from the Service");
            }
        }

        private void refreshLeaveCatergoriesAgain()
        {
            try
            {
                this.serviceClient.GetLeaveCatergoriesCompleted += (s, e) =>
                {
                    LeaveCatergory = null;
                    CategoryList.Clear();
                    this.LeaveCatergory = e.Result;
                    if (LeaveCatergory != null)
                        CategoryList = LeaveCatergory.ToList();                 
                };
                this.serviceClient.GetLeaveCatergoriesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error Occured while Fetching data from the Service");
            }
        }

        #endregion

        #region New Method
        public void New()
        {
            SearchIndex = 0;
            Search = "";
            CurrentLeaveCatergory = new z_LeaveCategory();
            CurrentLeaveCatergory.leave_id = Guid.NewGuid();
            refreshLeaveCatergories();
        }
        #endregion

        #region New Button Class & Property
        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }
        #endregion

        #region Save Method

        void Save()
        {
            if (CurrentLeaveCatergory != null && CurrentLeaveCatergory.leave_id != Guid.Empty)
            {
                if (CategoryList.Count(c => c.leave_id == CurrentLeaveCatergory.leave_id) > 0)
                {
                    if (CurrentLeaveCatergory.name == null || CurrentLeaveCatergory.name == string.Empty)
                        clsMessages.setMessage("Leave Name Cannot be Empty");
                    else if (CurrentLeaveCatergory.leave_code == null || CurrentLeaveCatergory.leave_code == string.Empty)
                        clsMessages.setMessage("Leave Code Cannot be Empty");
                    else if (CurrentLeaveCatergory.description == null || CurrentLeaveCatergory.description == string.Empty)
                        clsMessages.setMessage("Leave Description Cannot be Empty");
                    else
                    {
                        if (clsSecurity.GetUpdatePermission(403))
                        {
                            if (this.serviceClient.UpdateLeaveCatergory(CurrentLeaveCatergory))
                            {
                                refreshLeaveCatergories();
                                New();
                                clsMessages.setMessage("Record Updated Successfully");
                            }
                            else
                            {
                                New();
                                clsMessages.setMessage("Record Update Failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }

                else
                {
                    if (CurrentLeaveCatergory.name == null || CurrentLeaveCatergory.name == string.Empty)
                        clsMessages.setMessage("Leave Name Cannot be Empty");
                    else if (CurrentLeaveCatergory.leave_code == null || CurrentLeaveCatergory.leave_code == string.Empty)
                        clsMessages.setMessage("Leave Code Cannot be Empty");
                    else if (CurrentLeaveCatergory.description == null || CurrentLeaveCatergory.description == string.Empty)
                        clsMessages.setMessage("Leave Description Cannot be Empty");
                    else if (CategoryList.Count(c => c.name == CurrentLeaveCatergory.name) > 0)
                        clsMessages.setMessage("Category Name Already Exists");
                    else
                    {
                        CurrentLeaveCatergory.is_active = CurrentLeaveCatergory.is_active == null ? true : CurrentLeaveCatergory.is_active;
                        CurrentLeaveCatergory.is_official = CurrentLeaveCatergory.is_official == null ? false : CurrentLeaveCatergory.is_official;

                        if (clsSecurity.GetSavePermission(403))
                        {
                            if (this.serviceClient.SaveLeaveCatergory(CurrentLeaveCatergory))
                            {
                                refreshLeaveCatergories();
                                New();
                                clsMessages.setMessage("Record Saved Successfully");
                            }
                            else
                            {
                                New();
                                clsMessages.setMessage("Record Saved Failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
            }
        }

        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }

        #endregion

        #region Delete Method

        void Delete()
        {
            if (LeaveCatergory.Count(c => c.leave_id == CurrentLeaveCatergory.leave_id) > 0)
            {
                clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (clsSecurity.GetDeletePermission(403))
                    {
                        if (serviceClient.DeleteLeaveType(CurrentLeaveCatergory))
                        {
                            refreshLeaveCatergories();
                            New();
                            clsMessages.setMessage("Record Deleted Successfully");
                        }
                        else
                        {
                            New();
                            clsMessages.setMessage("Record Delete Failed");
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Delete this record(s)");
                }
            }
        }

        #endregion

        #region Delete Button Class & Property

        bool deleteCaneExecute()
        {
            if (CurrentLeaveCatergory == null)
                return false;
            else
                return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCaneExecute);
            }
        }

        #endregion

        #region Search

        private void SearchCategory()
        {
            LeaveCatergory = null;
            LeaveCatergory = CategoryList;

            try
            {
                if (SearchIndex == 0)
                    LeaveCatergory = LeaveCatergory.Where(c => c.name != null && c.name.ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 1)
                    LeaveCatergory = LeaveCatergory.Where(c => c.leave_code != null && c.leave_code.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        #endregion

        #region Leiu Leave

        void CreateCategory()
        {
            z_LeaveCategory category = CategoryList.FirstOrDefault(c => c.leave_id == new Guid("9b615c80-32d7-4951-babc-04ad7193bc32"));

            if (category == null)
            {
                z_LeaveCategory leavecat = new z_LeaveCategory();
                leavecat.leave_id = new Guid("9b615c80-32d7-4951-babc-04ad7193bc32");
                leavecat.name = "Leiu Leave";
                leavecat.leave_code = "LL";
                leavecat.description = "Lakpohora Leiu Leave";
                leavecat.is_active = true;
                leavecat.is_official = true;
                serviceClient.SaveLeiuLeaveCategory(leavecat);
                refreshLeaveCatergoriesAgain();
            }
        }

        #endregion
    }
}
