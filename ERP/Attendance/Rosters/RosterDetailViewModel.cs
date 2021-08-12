using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Rosters
{
    class RosterDetailViewModel : ViewModelBase
    {
        #region Lists
        List<RosterDetailView> listRosterDetailView = new List<RosterDetailView>();
        List<z_RosterDays> listRosterDays = new List<z_RosterDays>();
        #endregion

        #region Sevice Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public RosterDetailViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.RosterDetail), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    serviceClient = new ERPServiceClient();
                    RefreshRosterDays();
                    New();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }
        #endregion

        #region Properites
        private IEnumerable<RosterDetailView> rosterDetails;
        public IEnumerable<RosterDetailView> RosterDetails
        {
            get
            {
                return this.rosterDetails;
            }
            set
            {
                rosterDetails = value;
                OnPropertyChanged("RosterDetails");
            }
        }

        private RosterDetailView currentRosterDetail;
        public RosterDetailView CurrentRosterDetail
        {
            get
            {
                return currentRosterDetail;
            }
            set
            {
                currentRosterDetail = value;
                OnPropertyChanged("CurrentRosterDetail");
            }
        }

        private IEnumerable<z_RosterDays> in_days;
        public IEnumerable<z_RosterDays> In_Days
        {
            get { return in_days; }
            set { in_days = value; OnPropertyChanged("In_Days"); }
        }

        private z_RosterDays current_in_day;
        public z_RosterDays Current_In_Day
        {
            get { return current_in_day; }
            set { current_in_day = value; OnPropertyChanged("Current_In_Day"); }
        }

        private IEnumerable<z_RosterDays> out_days;
        public IEnumerable<z_RosterDays> Out_Days
        {
            get { return out_days; }
            set { out_days = value; OnPropertyChanged("Out_Days"); }
        }

        private z_RosterDays current_out_day;
        public z_RosterDays Current_Out_Day
        {
            get { return current_out_day; }
            set { current_out_day = value; OnPropertyChanged("Current_Out_Day"); }
        }

        private IEnumerable<z_RosterDays> roster_in_days;
        public IEnumerable<z_RosterDays> Roster_In_Days
        {
            get { return roster_in_days; }
            set { roster_in_days = value; OnPropertyChanged("Roster_In_Days"); }
        }

        private z_RosterDays current_roster_in_day;
        public z_RosterDays Current_Roster_In_Day
        {
            get { return current_roster_in_day; }
            set { current_roster_in_day = value; OnPropertyChanged("Current_Roster_In_Day"); }
        }

        private IEnumerable<z_RosterDays> roster_out_days;
        public IEnumerable<z_RosterDays> Roster_Out_Days
        {
            get { return roster_out_days; }
            set { roster_out_days = value; OnPropertyChanged("Roster_Out_Days"); }
        }

        private z_RosterDays current_roster_out_day;
        public z_RosterDays Current_Roster_Out_Day
        {
            get { return current_roster_out_day; }
            set { current_roster_out_day = value; OnPropertyChanged("Current_Roster_Out_Day"); }
        }

        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged("Search"); SearchRoster(); }
        }
        
        #endregion

        #region Refresh Methods
        public void RefreshRosterView()
        {
            try
            {
                listRosterDetailView.Clear();
                serviceClient.GetRosterDetailViewCompleted += (s, e) =>
                {
                    RosterDetails = e.Result;
                    if (RosterDetails != null)
                        listRosterDetailView = RosterDetails.ToList();
                };
                serviceClient.GetRosterDetailViewAsync();
            }
            catch (Exception)
            { }
        }

        public void RefreshRosterDays()
        {
            listRosterDays.Clear();
            try
            {
                serviceClient.GetRosterDaysCompleted += (s, e) =>
                {
                    Out_Days = e.Result;
                    Roster_Out_Days = Out_Days;
                    if (Out_Days != null)
                    {
                        listRosterDays = Out_Days.ToList();
                        Roster_In_Days = In_Days = listRosterDays.Where(c => c.roster_day_id == 1);
                    }
                };
                serviceClient.GetRosterDaysAsync();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region New Method
        void New()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.RosterDetail), clsSecurity.loggedUser.user_id))
            {
                RefreshRosterView();
                CurrentRosterDetail = null;
                CurrentRosterDetail = new RosterDetailView();
                CurrentRosterDetail.roster_detail_id = Guid.NewGuid();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New);
            }
        }
        #endregion

        #region Save Method
        void Save()
        {
            try
            {
                if (SaveValidation())
                {
                    z_RosterDetail newRoster = new z_RosterDetail();
                    newRoster.roster_detail_id = CurrentRosterDetail.roster_detail_id;
                    newRoster.roster_detail_name = CurrentRosterDetail.roster_detail_name;
                    newRoster.grace_in = CurrentRosterDetail.grace_in;
                    newRoster.grace_out = CurrentRosterDetail.grace_out;
                    newRoster.in_day_id = CurrentRosterDetail.in_day_id;
                    newRoster.in_time = CurrentRosterDetail.in_time;
                    newRoster.is_active = CurrentRosterDetail.is_active;
                    newRoster.is_delete = CurrentRosterDetail.is_delete;
                    newRoster.is_off = CurrentRosterDetail.is_off;
                    newRoster.out_day_id = CurrentRosterDetail.out_day_id;
                    newRoster.out_time = CurrentRosterDetail.out_time;
                    newRoster.roster_in_day_id = CurrentRosterDetail.roster_in_day_id;
                    newRoster.roster_out_day_id = CurrentRosterDetail.roster_out_day_id;
                    newRoster.roster_off_time = CurrentRosterDetail.roster_off_time;
                    newRoster.roster_on_time = CurrentRosterDetail.roster_on_time;
                    newRoster.is_delete = false;

                    if (listRosterDetailView.Count(c => c.roster_detail_id == newRoster.roster_detail_id) == 0)
                    {
                        if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.RosterDetail), clsSecurity.loggedUser.user_id))
                        {
                            newRoster.save_datetime = DateTime.Now;
                            newRoster.save_user_id = clsSecurity.loggedUser.user_id;
                            if (serviceClient.SaveRosterDetail(newRoster))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                New();
                            }
                            else
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
                        }
                    }
                    else
                    {
                        if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.RosterDetail), clsSecurity.loggedUser.user_id))
                        {
                            newRoster.modified_datetime = DateTime.Now;
                            newRoster.modified_user_id = clsSecurity.loggedUser.user_id;
                            if (serviceClient.UpdateRosterDetail(newRoster))
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                New();
                            }
                            else
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.NoPermissionForUpdate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private bool SaveValidation()
        {
            if (CurrentRosterDetail == null)
                return false;
            if (CurrentRosterDetail.roster_detail_id == null || CurrentRosterDetail.roster_detail_id == Guid.Empty)
                return false;
            if (CurrentRosterDetail.roster_detail_name == null)
            {
                ShowMessage("Roster Name should not be Empty", "Invalid Roster Name", 0);
                return false;
            }
            if (CurrentRosterDetail.in_day_id == null)
            {
                ShowMessage("Invalid In day", "Invalid In day", 0);
                return false;
            }
            if (CurrentRosterDetail.out_day_id == null)
            {
                ShowMessage("Invalid Out day", "Invalid Out day", 0);
                return false;
            }
            if (CurrentRosterDetail.in_time == null)
            {
                ShowMessage("Invalid In time", "Invalid In time", 0);
                return false;
            }
            if ((TimeSpan)CurrentRosterDetail.in_time > new TimeSpan(24,00,00))
            {
                ShowMessage("Invalid In time", "Invalid In time", 0);
                return false;
            }
            if (CurrentRosterDetail.out_time == null)
            {
                ShowMessage("Invalid Out time", "Invalid Out time", 0);
                return false;
            }
            if ((TimeSpan)CurrentRosterDetail.out_time > new TimeSpan(24, 00, 00))
            {
                ShowMessage("Invalid Out time", "Invalid Out time", 0);
                return false;
            }
            if (CurrentRosterDetail.roster_in_day_id == null)
            {
                ShowMessage("Invalid Roster In day", "Invalid Roster In day", 0);
                return false;
            }
            if (CurrentRosterDetail.roster_out_day_id == null)
            {
                ShowMessage("Invalid Roster Out day", "Invalid Roster Out day", 0);
                return false;
            }
            if (CurrentRosterDetail.out_day_id == null)
            {
                ShowMessage("Invalid Out day", "Invalid Out day", 0);
                return false;
            }
            if (CurrentRosterDetail.roster_on_time == null)
            {
                ShowMessage("Invalid Roster on time", "Invalid Roster on time", 0);
                return false;
            }
            if ((TimeSpan)CurrentRosterDetail.roster_on_time > new TimeSpan(24, 00, 00))
            {
                ShowMessage("Invalid Roster On time", "Invalid Roster On time", 0);
                return false;
            }
            if (CurrentRosterDetail.roster_off_time == null)
            {
                ShowMessage("Invalid Roster off time", "Invalid Roster off time", 0);
                return false;
            }
            if ((TimeSpan)CurrentRosterDetail.roster_off_time > new TimeSpan(24, 00, 00))
            {
                ShowMessage("Invalid Roster Off time", "Invalid Roster Off time", 0);
                return false;
            }
            if (CurrentRosterDetail.grace_in == null)
            {
                ShowMessage("Invalid Grace In ", "Invalid Grace In time", 0);
                return false;
            }
            if (CurrentRosterDetail.grace_out == null)
            {
                ShowMessage("Invalid Grace Out", "Invalid Grace Out", 0);
                return false;
            }
            return true;
        }
        #endregion

        #region Delete Method
        void Delete()
        {
            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.RosterDetail), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Do you want to delete this Record?", "Delete Record", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                    {
                        z_RosterDetail roster = new z_RosterDetail();
                        roster.roster_detail_id = CurrentRosterDetail.roster_detail_id;
                        roster.delete_datetime = DateTime.Now;
                        roster.delete_user_id = clsSecurity.loggedUser.user_id;
                        if (serviceClient.DeleteRosterDetail(roster))
                        {
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            New();
                        }
                        else
                            clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForDelete);
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentRosterDetail == null)
                return false;
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

        #region Search Method
        private void SearchRoster()
        {
            try
            {
                RosterDetails = null;
                RosterDetails = listRosterDetailView;
                RosterDetails = RosterDetails.Where(c => c.roster_detail_name.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception)
            { }
        }
        #endregion

        private void ShowMessage(String msgbody, String msgheader, int x)
        {
            if (x == 0)
                MessageBox.Show(msgbody, msgheader, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(msgbody, msgheader, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //private string search;
        //public string Search
        //{
        //    get
        //    {
        //        return this.search;
        //    }
        //    set
        //    {
        //        search = value;
        //        OnPropertyChanged("Search");
        //        if (this.search == "")
        //        {
        //            this.reafreshEmployeeRosterViewList();
        //        }
        //        else
        //        {
        //            SearchTextChanged();
        //        }

        //    }
        //}
        //#region Search Class
        //public class relayCommand : ICommand
        //{
        //    readonly Action<object> _Execute;
        //    readonly Predicate<object> _CanExecute;

        //    public relayCommand(Action<object> execute)
        //        : this(execute, null)
        //    {
        //    }

        //    public relayCommand(Action<object> execute, Predicate<object> canExecute)
        //    {
        //        if (execute == null)
        //            throw new ArgumentNullException("execute");

        //        _Execute = execute;
        //        _CanExecute = canExecute;
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return _CanExecute == null ? true : _CanExecute(parameter);
        //    }

        //    public void Execute(object parameter)
        //    {
        //        _Execute(parameter);
        //    }


        //    public event EventHandler CanExecuteChanged;
        //}
        //#endregion

        //#region Search Property
        //relayCommand _OperationCommand;
        //public ICommand OperationCommand
        //{
        //    get
        //    {
        //        if (_OperationCommand == null)
        //        {
        //            _OperationCommand = new relayCommand(param => this.ExecuteCommand(),
        //                param => this.CanExecuteCommand);
        //        }

        //        return this._OperationCommand;
        //    }
        //}

        //bool CanExecuteCommand
        //{
        //    get { return true; }
        //}
        //#endregion

        //#region Search Command Execute
        //public void ExecuteCommand()
        //{
        //    Search = "hh";
        //    Search = "";
        //}

        //#endregion

        //#region Search Method for all Properties
        //public void SearchTextChanged()
        //{
        //    //EmployeeRosterView = EmployeeRosterView.Where(e => e.roster_detail_name.ToUpper().Contains(Search.ToUpper())).ToList();
        //}
        //#endregion
    }
}