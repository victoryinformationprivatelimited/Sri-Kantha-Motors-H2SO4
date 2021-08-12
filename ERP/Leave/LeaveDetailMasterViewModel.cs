using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using ERP.Properties;

namespace ERP.MastersDetails
{
    class LeaveDetailMasterViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Fields

        List<LeaveDetailMasterView> ListLeaveDetails;

        #endregion

        #region Constructor
        public LeaveDetailMasterViewModel()
        {
            ListLeaveDetails = new List<LeaveDetailMasterView>();
            //this.refreshLeaveDetailView();
            this.refreshLeaveCatergories();
            this.refreshLeavePeriods();
            //this.reafreshMasLeaveDetails();
            this.New();
        }
        #endregion

        #region Properties
        private IEnumerable<LeaveDetailMasterView> _LeaveDetailView;
        public IEnumerable<LeaveDetailMasterView> LeaveDetailView
        {
            get
            {
                return this._LeaveDetailView;
            }
            set
            {
                this._LeaveDetailView = value;
                this.OnPropertyChanged("LeaveDetailView");
            }
        }

        private LeaveDetailMasterView _CurrentLeaveDetailView;
        public LeaveDetailMasterView CurrentLeaveDetailView
        {
            get
            {
                return this._CurrentLeaveDetailView;
            }
            set
            {
                this._CurrentLeaveDetailView = value;
                this.OnPropertyChanged("CurrentLeaveDetailView");

            }
        }

        private IEnumerable<mas_LeaveDetail> _MasLeaveDetail;
        public IEnumerable<mas_LeaveDetail> MasLeaveDetail
        {
            get
            {
                return this._MasLeaveDetail;
            }
            set
            {
                this._MasLeaveDetail = value;
                this.OnPropertyChanged("MasLeaveDetail");
            }
        }

        private mas_LeaveDetail _CurrentLeaveDetail;
        public mas_LeaveDetail CurrentLeaveDetail
        {
            get
            {
                return this._CurrentLeaveDetail;
            }
            set
            {
                this._CurrentLeaveDetail = value;
                this.OnPropertyChanged("CurrentLeaveDetail");
            }
        }

        private IEnumerable<z_LeaveCategory> _LeaveCatergories;
        public IEnumerable<z_LeaveCategory> LeaveCatergories
        {
            get
            {
                return this._LeaveCatergories;
            }
            set
            {
                this._LeaveCatergories = value;
                this.OnPropertyChanged("LeaveCatergories");
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

        private z_LeavePeriod _CurrentLeavePeriods;
        public z_LeavePeriod CurrentLeavePeriods
        {
            get
            {
                return this._CurrentLeavePeriods;
            }
            set
            {
                this._CurrentLeavePeriods = value;
                this.OnPropertyChanged("CurrentLeavePeriods");

                if (CurrentLeavePeriods != null)
                {
                    if (CurrentLeaveDetailView != null && CurrentLeaveDetailView.leave_period_id != null && CurrentLeaveDetailView.leave_period_id != CurrentLeavePeriods.period_id)
                    {
                        LeaveDetailView = null;
                        ListLeaveDetails.Clear();
                        CurrentLeaveDetailView = null;
                        CurrentLeaveDetailView = new LeaveDetailMasterView();
                        CurrentLeaveDetailView.leave_detail_id = Guid.NewGuid();
                    }
                }

            }
        }

        private IEnumerable<z_LeaveCategory> _CategoryDetail;
        public IEnumerable<z_LeaveCategory> CategoryDetail
        {
            get
            {
                return this._CategoryDetail;
            }
            set
            {
                this._CategoryDetail = value;
                this.OnPropertyChanged("CategoryDetail");
            }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null) SearchLeaveDetail(); }
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
            try
            {
                SearchIndex = 0;
                Search = "";
                CurrentLeaveDetailView = null;
                CurrentLeaveDetailView = new LeaveDetailMasterView();
                CurrentLeaveDetailView.leave_detail_id = Guid.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

            if (CurrentLeaveDetailView != null)
            {
                mas_LeaveDetail LeaveDetailSaveObject = new mas_LeaveDetail();
                LeaveDetailSaveObject.leave_detail_id = CurrentLeaveDetailView.leave_detail_id == Guid.Empty ? Guid.NewGuid() : CurrentLeaveDetailView.leave_detail_id;
                LeaveDetailSaveObject.leave_category_id = CurrentLeaveDetailView.leave_category_id;
                LeaveDetailSaveObject.is_automate = CurrentLeaveDetailView.is_automate;
                LeaveDetailSaveObject.leave_period_id = CurrentLeaveDetailView.leave_period_id;
                LeaveDetailSaveObject.number_of_days = CurrentLeaveDetailView.number_of_days;
                LeaveDetailSaveObject.leave_detail_name = CurrentLeaveDetailView.leave_detail_name;

                if (CurrentLeaveDetailView.leave_category_id == null)
                    clsMessages.setMessage("Please Select a Leave Category");
                else if (CurrentLeaveDetailView.leave_period_id == null)
                    clsMessages.setMessage("Please Select a Leave Period");
                else if (CurrentLeaveDetailView.leave_detail_name == null || CurrentLeaveDetailView.leave_detail_name == string.Empty)
                    clsMessages.setMessage("Leave Detail Name Cannot be Empty");
                else if (CurrentLeaveDetailView.number_of_days == null || CurrentLeaveDetailView.number_of_days < 0)
                    clsMessages.setMessage("Please enter a Valid Number Of Days");
                else
                {
                    if (clsSecurity.GetSavePermission(404))
                    {
                        if (this.serviceClient.SaveUpdateLeaveMasterDetail(LeaveDetailSaveObject))
                        {
                            refreshLeaveDetailView();
                            New();
                            clsMessages.setMessage("Record Saved/Updated Successfully");
                        }
                        else
                        {
                            New();
                            clsMessages.setMessage("Record Save/Update Failed");
                        } 
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save/Update this record(s)");
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

        #region Delete method

        void Delete()
        {
            try
            {
                if (LeaveDetailView.Count(c => c.leave_detail_id == CurrentLeaveDetailView.leave_detail_id) > 0)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {

                        if (clsSecurity.GetDeletePermission(404))
                        {
                            if (this.serviceClient.DeleteLeaveMasterDetail(CurrentLeaveDetailView))
                            {
                                refreshLeaveDetailView();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region DeleteButton Class & Property
        bool deleteCanExecute()
        {
            if (CurrentLeaveDetailView == null)
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

        #region Leave Details View List

        private void refreshLeaveDetailView()
        {
            this.serviceClient.GetLeaveMasterDetailViewByPeriodCompleted += (s, e) =>
                {
                    ListLeaveDetails.Clear();

                    this.LeaveDetailView = e.Result;
                    if (LeaveDetailView != null)
                        ListLeaveDetails = LeaveDetailView.ToList();
                };
            this.serviceClient.GetLeaveMasterDetailViewByPeriodAsync(CurrentLeavePeriods.period_id == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        private void refreshLeaveDetailViewAgain()
        {
            this.serviceClient.GetLeaveMasterDetailViewByPeriodCompleted += (s, e) =>
            {
                ListLeaveDetails.Clear();

                this.LeaveDetailView = e.Result;
                if (LeaveDetailView != null)
                    ListLeaveDetails = LeaveDetailView.ToList();
            };
            this.serviceClient.GetLeaveMasterDetailViewByPeriodAsync(CurrentLeavePeriods.period_id == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }
        #endregion

        #region Leave Catergories List
        private void refreshLeaveCatergories()
        {
            this.serviceClient.GetLeaveCatergoriesCompleted += (s, e) =>
                {
                    this.LeaveCatergories = e.Result;
                };
            this.serviceClient.GetLeaveCatergoriesAsync();
        }
        #endregion

        #region Leave Periods List
        private void refreshLeavePeriods()
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
                {
                    this.LeavePeriods = e.Result;
                };
            this.serviceClient.GetLeavePeriodsAsync();
        }
        #endregion

        #region Leave Master Detail List

        private void reafreshMasLeaveDetails()
        {
            this.serviceClient.GetMasLeaveDetailsByPeriodCompleted += (s, e) =>
                {
                    this.MasLeaveDetail = e.Result;
                    if(MasLeaveDetail.Count()>0)
                        CreateLeiuLeaveDetails();
                };
            this.serviceClient.GetMasLeaveDetailsByPeriodAsync(CurrentLeavePeriods.period_id == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        private void reafreshMasLeaveDetailsAgain()
        {
            this.serviceClient.GetMasLeaveDetailsByPeriodCompleted += (s, e) =>
            {
                this.MasLeaveDetail = e.Result;
            };
            this.serviceClient.GetMasLeaveDetailsByPeriodAsync(CurrentLeavePeriods.period_id == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }
        #endregion

        #region Search


        private void SearchLeaveDetail()
        {
            LeaveDetailView = null;
            LeaveDetailView = ListLeaveDetails;

            try
            {
                if (SearchIndex == 0)
                    LeaveDetailView = LeaveDetailView.Where(c => c.name != null && c.name.ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 1)
                    LeaveDetailView = LeaveDetailView.Where(c => c.period_name != null && c.period_name.ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 2)
                    LeaveDetailView = LeaveDetailView.Where(c => c.leave_detail_name != null && c.leave_detail_name.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        #endregion

        #region Get Leave Details

        public ICommand BtnGetDetails
        {
            get { return new RelayCommand(GetLeaveDetails, GetLeaveDetailsCE); }
        }

        private bool GetLeaveDetailsCE()
        {
            if (CurrentLeavePeriods != null)
                return true;
            else
                return false;
        }

        private void GetLeaveDetails()
        {
            reafreshMasLeaveDetails();
            refreshLeaveDetailView();
        }

        #endregion

        #region Leiu Leave

        void CreateLeiuLeaveDetails()
        {
            mas_LeaveDetail leavecat = MasLeaveDetail.FirstOrDefault(c => c.leave_category_id == new Guid("9b615c80-32d7-4951-babc-04ad7193bc32"));
            if(leavecat == null)
            {
                mas_LeaveDetail leiuleavedetail = new mas_LeaveDetail();
                leiuleavedetail.leave_detail_id = Guid.NewGuid();
                leiuleavedetail.leave_category_id = new Guid("9b615c80-32d7-4951-babc-04ad7193bc32");
                leiuleavedetail.leave_period_id = CurrentLeavePeriods.period_id;
                leiuleavedetail.number_of_days = 0;
                leiuleavedetail.leave_detail_name = "Leiu Leave";
                serviceClient.SaveLeiuLeaveDetails(leiuleavedetail);
                reafreshMasLeaveDetailsAgain();
                refreshLeaveDetailViewAgain();
            }
        }

        #endregion
    }
}
