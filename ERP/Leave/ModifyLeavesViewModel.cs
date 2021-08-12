using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using ERP.AttendanceService;
using System.Windows.Input;
using System.Collections;
using System.Windows.Documents;

namespace ERP.Performance
{
    class ModifyLeavesViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        AttendanceServiceClient attSvcClient;
        #endregion

        #region Constructor

        public ModifyLeavesViewModel()
        {
            serviceClient = new ERPServiceClient();
            attSvcClient = new AttendanceServiceClient();
            SearchIndex = 0;
            NormalLeave = true;
        }

        #endregion

        #region Properties

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

        private IEnumerable<ERP.ERPService.PendingLeavesView> _ApprovedLeaves;
        public IEnumerable<ERP.ERPService.PendingLeavesView> ApprovedLeaves
        {
            get { return _ApprovedLeaves; }
            set { _ApprovedLeaves = value; OnPropertyChanged("ApprovedLeaves"); }
        }

        private IList _SelectedApprovedLeaves = new ArrayList();
        public IList SelectedApprovedLeaves
        {
            get { return _SelectedApprovedLeaves; }
            set { _SelectedApprovedLeaves = value; OnPropertyChanged("SelectedApprovedLeaves"); }
        }


        private DateTime? _fromDate;
        public DateTime? FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime? _toDate;
        public DateTime? ToDate
        {
            get { return _toDate; }
            set { _toDate = value; OnPropertyChanged("ToDate"); }
        }

        private bool _NormalLeave;

        public bool NormalLeave
        {
            get { return _NormalLeave; }
            set { _NormalLeave = value; OnPropertyChanged("NormalLeave"); }
        }

        private bool _LeiuLeave;

        public bool LeiuLeave
        {
            get { return _LeiuLeave; }
            set { _LeiuLeave = value; OnPropertyChanged("LeiuLeave"); }
        }

        private IEnumerable<PendingLeiuLeavesView> _ApprovedLeiuLeaves;

        public IEnumerable<PendingLeiuLeavesView> ApprovedLeiuLeaves
        {
            get { return _ApprovedLeiuLeaves; }
            set { _ApprovedLeiuLeaves = value; OnPropertyChanged("ApprovedLeiuLeaves"); }
        }

        private IList _SelectedApprovedLeiuLeaves = new ArrayList();

        public IList SelectedApprovedLeiuLeaves
        {
            get { return _SelectedApprovedLeiuLeaves; }
            set { _SelectedApprovedLeiuLeaves = value; OnPropertyChanged("SelectedApprovedLeiuLeaves"); }
        }
                

        #endregion

        #region RefreshMethods

        private void RefreshApprovedLeaves()
        {
            serviceClient.GetApprovedLeavePoolDataByDateCompleted += (s, e) =>
            {
                try
                {
                    ApprovedLeaves = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshApprovedLeaves()\n\n" + ex.Message.ToString());
                }
            };
            serviceClient.GetApprovedLeavePoolDataByDateAsync((DateTime)FromDate, (DateTime)ToDate, clsSecurity.loggedUser.user_id);
        }

        private void RefreshApprovedLeiuLeaves()
        {
            attSvcClient.GetApprovedLeiuLeavePoolDataByDateCompleted += (s, e) =>
            {
                try
                {
                    ApprovedLeiuLeaves = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshApprovedLeiuLeaves()\n\n" + ex.Message.ToString());
                }
            };
            attSvcClient.GetApprovedLeiuLeavePoolDataByDateAsync((DateTime)FromDate, (DateTime)ToDate, clsSecurity.loggedUser.user_id);
        }

        #endregion

        #region Commands & Methods

        public ICommand GetDataBtn
        {
            get { return new RelayCommand(GetData); }
        }

        private void GetData()
        {
            if (ValidateGetData())
            {
                RefreshApprovedLeaves();
                RefreshApprovedLeiuLeaves();
            }
        }

        public ICommand Savebtn
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        private bool SaveCanExecute()
        {
            if ((SelectedApprovedLeaves != null && SelectedApprovedLeaves.Count > 0) || (SelectedApprovedLeiuLeaves != null && SelectedApprovedLeiuLeaves.Count>0))
                return true;
            else
                return false;
        }

        private void Save()
        {
            if (clsSecurity.GetSavePermission(410))
            {
                if (NormalLeave == true)
                {
                    List<ERP.ERPService.PendingLeavesView> Savelist = new List<ERP.ERPService.PendingLeavesView>();

                    foreach (var Leave in SelectedApprovedLeaves)
                    {
                        ERP.ERPService.PendingLeavesView SaveLeave = Leave as ERP.ERPService.PendingLeavesView;
                        Savelist.Add(SaveLeave);
                    }

                    if (Savelist != null && Savelist.Count > 0)
                    {
                        if (serviceClient.ModifyApprovedLeaves(Savelist.ToArray()))
                        {
                            ApprovedLeaves = null;
                            clsMessages.setMessage("Leave(s) Modified Successfully");
                        }
                        else
                            clsMessages.setMessage("Leave(s) Modification Failed");
                    }
                }
                if (LeiuLeave == true)
                {
                    List<PendingLeiuLeavesView> SaveLeiuLeavelist = new List<PendingLeiuLeavesView>();

                    foreach (var LeiuLeaves in SelectedApprovedLeiuLeaves)
                    {
                        PendingLeiuLeavesView SaveLeiuLeave = LeiuLeaves as PendingLeiuLeavesView;
                        SaveLeiuLeavelist.Add(SaveLeiuLeave);
                    }

                    if (SaveLeiuLeavelist != null && SaveLeiuLeavelist.Count > 0)
                    {
                        if (attSvcClient.ModifyApprovedLeiuLeaves(SaveLeiuLeavelist.ToArray()))
                        {
                            ApprovedLeiuLeaves = null;
                            clsMessages.setMessage("Leiu Leave(s) Modified Successfully(Used Leiu Leave(s) Have to Update Manually)");
                        }
                        else
                            clsMessages.setMessage("Leiu Leave(s) Modification Failed");
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to modify");
        }

        private bool ValidateGetData()
        {
            if (FromDate == null)
            {
                clsMessages.setMessage("Please Select a Start Date");
                return false;
            }
            else if (ToDate == null)
            {
                clsMessages.setMessage("Please Select an End Date");
                return false;
            }
            else if (FromDate > ToDate)
            {
                clsMessages.setMessage("'From Date' Cannot be Greater than 'To Date'");
                return false;
            }
            else
                return true;
        }

        #endregion
    }
}
