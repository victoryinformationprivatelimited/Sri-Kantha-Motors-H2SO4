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
    class LeaveAmountMasterViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Fields

        List<z_LeaveType> LeaveTypesList;

        #endregion

        #region Constructor
        public LeaveAmountMasterViewModel()
        {
            LeaveTypesList = new List<z_LeaveType>();
            this.refreshLeaveTypes();
            this.New();
        }
        #endregion

        #region Properties
        private IEnumerable<z_LeaveType> _LeaveTypes;
        public IEnumerable<z_LeaveType> LeaveTypes
        {
            get
            {
                return this._LeaveTypes;
            }
            set
            {
                this._LeaveTypes = value;
                this.OnPropertyChanged("LeaveTypes");
            }
        }

        private z_LeaveType _CurrentLeaveType;
        public z_LeaveType CurrentLeveType
        {
            get
            {
                return this._CurrentLeaveType;
            }
            set
            {
                this._CurrentLeaveType = value;
                this.OnPropertyChanged("CurrentLeveType");
            }
        }

        private string _Search;
        public string Search
        {
            get
            { return this._Search; }
            set { this._Search = value; this.OnPropertyChanged("Search"); if(Search != null) Searchtypes(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }
        

        #region New Method
        void New()
        {
            SearchIndex = 0;
            Search = "";
            CurrentLeveType = new z_LeaveType();
            CurrentLeveType.leave_type_id = Guid.NewGuid();
            refreshLeaveTypes();
        }
        #endregion

        #region NewButton Class & Property
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
            if (CurrentLeveType != null && CurrentLeveType.leave_type_id != Guid.Empty)
            {
                if (LeaveTypesList.Count(c => c.leave_type_id == CurrentLeveType.leave_type_id) > 0)
                {
                    if (clsSecurity.GetUpdatePermission(401))
                    {
                        if (CurrentLeveType.name == null || CurrentLeveType.name == string.Empty)
                            clsMessages.setMessage("Leave Type Name Cannot be Empty");
                        else if (CurrentLeveType.value == null)
                            clsMessages.setMessage("Leave Type Value Cannot be Empty");
                        else
                        {
                            if (this.serviceClient.UpdateLeaveTypeMaster(CurrentLeveType))
                            {
                                refreshLeaveTypes();
                                New();
                                clsMessages.setMessage("Record Updated Successfully");
                            }
                            else
                            {
                                New();
                                clsMessages.setMessage("Record Update Failed");
                            }
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }

                else
                {
                    if (CurrentLeveType.name == null || CurrentLeveType.name == string.Empty)
                        clsMessages.setMessage("Leave Type Name Cannot be Empty");
                    else if (CurrentLeveType.value == null)
                        clsMessages.setMessage("Leave Type Value Cannot be Empty");
                    else if (LeaveTypesList.Count(c => c.value == CurrentLeveType.value) > 0)
                        clsMessages.setMessage("Entered Leave Value Already Exists");
                    else
                    {
                        //if (clsSecurity.GetSavePermission(401))
                        //{
                        //    if (this.serviceClient.SaveLeaveTypeMaster(CurrentLeveType))
                        //    {
                        //        refreshLeaveTypes();
                        //        New();
                        //        clsMessages.setMessage("Record Saved Successfully");
                        //    }
                        //    else
                        //    {
                        //        New();
                        //        clsMessages.setMessage("Record Save Failed");
                        //    } 
                        //}
                        //else
                        //    clsMessages.setMessage("You don't have permission to Save this record(s)");
                        clsMessages.setMessage("You don can't Save any leave types other than the existing record(s)");
                    }
                }
            }
        }

        #endregion

        #region SaveButton Class & Property

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
            if (LeaveTypes.Count(c => c.leave_type_id == CurrentLeveType.leave_type_id) > 0)
            {
                clsMessages.setMessage("Are you sure that you want to Delete this Record?",Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (clsSecurity.GetDeletePermission(401))
                    {
                        if (this.serviceClient.DeleteLeaveTypeMaster(CurrentLeveType))
                        {
                            refreshLeaveTypes();
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

        #region DeleteButton Class & Property
        bool deleteCanExecute()
        {
            if (CurrentLeveType == null)
            {
                return false;
            }

            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }
        #endregion

        #region Leave Types List
        private void refreshLeaveTypes()
        {
            this.serviceClient.GetAllLeaveTypesCompleted += (s, e) =>
            {
                this.LeaveTypes = e.Result;
                if (LeaveTypes != null)
                    LeaveTypesList = LeaveTypes.ToList();
            };
            this.serviceClient.GetAllLeaveTypesAsync();
        }

        #endregion

        #region Search

        private void Searchtypes()
        {
            LeaveTypes = null;
            LeaveTypes = LeaveTypesList;

            try
            {
                if (SearchIndex == 0)
                    LeaveTypes = LeaveTypes.Where(c => c.name != null && c.name.ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 1)
                    LeaveTypes = LeaveTypes.Where(c => c.value != null && c.value.ToString().ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        #endregion


        #endregion
    }
}
