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
    class LeavePeriodMasterViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Fields

        List<z_LeavePeriod> PeriodsList;

        #endregion

        #region Constructor
        public LeavePeriodMasterViewModel()
        {
            PeriodsList = new List<z_LeavePeriod>();
            this.refreshLeavePeriods();
            this.New();
        } 
        #endregion

        #region Properties
        private IEnumerable<z_LeavePeriod> _LeavePeriods;
        public IEnumerable<z_LeavePeriod> LeavePeriods
        {
            get
            {
                return this._LeavePeriods;
            }
            set
            {
                this._LeavePeriods = value;
                this.OnPropertyChanged("LeavePeriods");
            }
        }

        private z_LeavePeriod _CurrentLeavePeriod;
        public z_LeavePeriod CurrentLeavePeriod
        {
            get
            {
                return this._CurrentLeavePeriod;
            }
            set
            {
                this._CurrentLeavePeriod = value;
                this.OnPropertyChanged("CurrentLeavePeriod");
                
            }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if(Search != null) PeriodSearch(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        #endregion

        #region New Method
        void New()
        {
            SearchIndex = 0;
            Search = "";
            CurrentLeavePeriod = new z_LeavePeriod();
            CurrentLeavePeriod.period_id = Guid.NewGuid();
            refreshLeavePeriods();
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
            if (CurrentLeavePeriod != null && CurrentLeavePeriod.period_id != Guid.Empty)
            {
                if (PeriodsList.Count(c => c.period_id == CurrentLeavePeriod.period_id) > 0)
                {
                    if (CurrentLeavePeriod.from_date == null || CurrentLeavePeriod.to_date == null)
                        clsMessages.setMessage("Period 'Start Date' and 'End Date' Cannot be Empty");
                    else if (CurrentLeavePeriod.period_name == null || CurrentLeavePeriod.period_name == string.Empty)
                        clsMessages.setMessage("Period Name Cannot be Empty");
                    else
                    {
                        if (clsSecurity.GetUpdatePermission(402))
                        {
                            if (this.serviceClient.UpadteLeavePeriods(CurrentLeavePeriod))
                            {
                                refreshLeavePeriods();
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
                    if (CurrentLeavePeriod.from_date == null || CurrentLeavePeriod.to_date == null)
                        clsMessages.setMessage("Period 'Start Date' and 'End Date' Cannot be Empty");
                    else if (CurrentLeavePeriod.period_name == null || CurrentLeavePeriod.period_name == string.Empty)
                        clsMessages.setMessage("Period Name Cannot be Empty");
                    else if (PeriodsList.Count(c => c.from_date == CurrentLeavePeriod.from_date && c.to_date == CurrentLeavePeriod.to_date) > 0)
                        clsMessages.setMessage("Leave Period with the Same Date Range Already Exists");

                    else
                    {
                        if (clsSecurity.GetSavePermission(402))
                        {
                            if (this.serviceClient.SaveLeavePeriods(CurrentLeavePeriod))
                            {
                                refreshLeavePeriods();
                                New();
                                clsMessages.setMessage("Record Saved Successfully");
                            }
                            else
                            {
                                New();
                                clsMessages.setMessage("Record Save Failed");
                            } 
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
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
            if (LeavePeriods.Count(c => c.period_id == CurrentLeavePeriod.period_id) > 0) 
            {
                clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK) 
                {
                    if (clsSecurity.GetDeletePermission(402))
                    {
                        if (this.serviceClient.DeleteLeavePeriods(CurrentLeavePeriod))
                        {
                            clsMessages.setMessage("Record Deleted Successfully");
                            refreshLeavePeriods();
                            New();
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

        #region Delete Button Calass & Property
        bool deleteCanExecute()
        {
            if (CurrentLeavePeriod == null)
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

        #region Leave Periods List

        private void refreshLeavePeriods()
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
                {
                    this.LeavePeriods = e.Result;
                    if (LeavePeriods != null)
                        PeriodsList = LeavePeriods.ToList();
                };
            this.serviceClient.GetLeavePeriodsAsync();
        } 

        #endregion

        #region Search

        private void PeriodSearch()
        {
            LeavePeriods = null;
            LeavePeriods = PeriodsList;

            try
            {
                if (SearchIndex == 0) 
                    LeavePeriods = LeavePeriods.Where(c => c.period_name != null && c.period_name.ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 1)
                    LeavePeriods = LeavePeriods.Where(c => c.from_date != null && c.from_date.ToString().ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 2)
                    LeavePeriods = LeavePeriods.Where(c => c.to_date != null && c.to_date.ToString().ToUpper().Contains(Search.ToUpper()));
            }

            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        #endregion
    }
}
