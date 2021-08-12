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
    class RosterCalenderDateViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<RosterCalenderDetailView_PROC_Result> listScheduledRostersDetails = new List<RosterCalenderDetailView_PROC_Result>();
        private List<EmployeeRosterDetailView> listEmployees=new List<EmployeeRosterDetailView>();
        #endregion

        #region Constructor
        public RosterCalenderDateViewModel(DateTime? calenderDate)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    serviceClient = new ERPServiceClient();
                    CalenderDate = calenderDate;
                    RefreshRosterGroupEmployees();
                    New();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }
        #endregion

        #region Properites
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged("SearchText"); Search(); }
        }
        
        private int? searchIndex;
        public int? SearchIndex
        {
            get { return searchIndex; }
            set { searchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private DateTime? calenderDate;
        public DateTime? CalenderDate
        {
            get { return calenderDate; }
            set { calenderDate = value; OnPropertyChanged("CalenderDate"); if (CalenderDate != null)DayOfWeek = ((DateTime)CalenderDate).DayOfWeek.ToString(); }
        }

        private string dayOfWeek;
        public string DayOfWeek
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; OnPropertyChanged("DayOfWeek"); }
        }

        private IEnumerable<RosterCalenderHeaderView_PROC_Result> scheduledRosters;
        public IEnumerable<RosterCalenderHeaderView_PROC_Result> ScheduledRosters
        {
            get { return scheduledRosters; }
            set { scheduledRosters = value; OnPropertyChanged("scheduledRosters"); }
        }

        private RosterCalenderHeaderView_PROC_Result currentScheduledRosters;
        public RosterCalenderHeaderView_PROC_Result CurrentScheduledRosters
        {
            get { return currentScheduledRosters;}
            set { currentScheduledRosters = value; OnPropertyChanged("CurrentScheduledRosters"); FilterScheduledRostersDetails(); FilterEmployees(); }
        }

        private IEnumerable<RosterCalenderDetailView_PROC_Result> scheduledRostersDetails;
        public IEnumerable<RosterCalenderDetailView_PROC_Result> ScheduledRostersDetails
        {
            get { return scheduledRostersDetails; }
            set { scheduledRostersDetails = value; OnPropertyChanged("ScheduledRostersDetails"); }
        }

        private RosterCalenderDetailView_PROC_Result currentScheduledRostersDetails;
        public RosterCalenderDetailView_PROC_Result CurrentScheduledRostersDetails
        {
            get { return currentScheduledRostersDetails; }
            set { currentScheduledRostersDetails = value; OnPropertyChanged("CurrentScheduledRostersDetails"); }
        }

        private IEnumerable<EmployeeRosterDetailView> rosterGroupEmployees;
        public IEnumerable<EmployeeRosterDetailView> RosterGroupEmployees
        {
            get { return rosterGroupEmployees; }
            set { rosterGroupEmployees = value; OnPropertyChanged("RosterGroupEmployees"); }
        }

        private EmployeeRosterDetailView currentRosterGroupEmployee;
        public EmployeeRosterDetailView CurrentRosterGroupEmployee
        {
            get { return currentRosterGroupEmployee; }
            set { currentRosterGroupEmployee = value; OnPropertyChanged("CurrentRosterGroupEmployee"); }
        }
        #endregion

        #region New Method
        private void New()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                RefreshScheduledRosters();
                RefreshScheduledRostersDetails();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        #endregion

        #region Save Method
        private void Save()
        {

        }

        private bool SaveCanExecute()
        {
            return true;
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }
        #endregion

        #region Delete Method
        private void Delete()
        {
            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Do you want to delete this Roster?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    z_RosterCalenderHeader rosterHeader = new z_RosterCalenderHeader();
                    rosterHeader.roster_header_id = CurrentScheduledRosters.roster_header_id;
                    rosterHeader.delete_datetime = DateTime.Now;
                    rosterHeader.delete_user_id = clsSecurity.loggedUser.user_id;
                    if (serviceClient.DeleteRosterCalenderHeader(rosterHeader))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        New();
                    }
                    else
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForDelete);
            }
        }

        private bool DeleteCanExecute()
        {
            if (CurrentScheduledRosters == null)
                return false;
            return true;
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }
        #endregion

        #region Refresh Methods
        private void RefreshScheduledRosters()
        {
            try
            {
                ScheduledRosters = null;
               ScheduledRosters = serviceClient.GetRosterCalenderHeaderView_PROC(CalenderDate);
            }
            catch (Exception)
            { }
        }

        private void RefreshScheduledRostersDetails()
        {
            try
            {
                listScheduledRostersDetails.Clear();
                ScheduledRostersDetails = null;
                ScheduledRostersDetails = serviceClient.GetRosterCalenderDetailView_PROC(CalenderDate);
                if (ScheduledRostersDetails != null)
                    listScheduledRostersDetails = ScheduledRostersDetails.ToList();
                ScheduledRostersDetails = null;
            }
            catch (Exception)
            { }
        }

        private void RefreshRosterGroupEmployees()
        {
            try
            {
                listEmployees.Clear();
                IEnumerable<EmployeeRosterDetailView> ie;
                serviceClient.GetEmployeeRosterDetailViewCompleted += (s, e) =>
                {
                    ie = e.Result;
                    if (ie != null)
                        listEmployees = ie.ToList();
                };
                serviceClient.GetEmployeeRosterDetailViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Filter Methods
        private void FilterScheduledRostersDetails()
        {
            try
            {
                if (CurrentScheduledRosters != null)
                {
                    ScheduledRostersDetails = listScheduledRostersDetails;
                    ScheduledRostersDetails = ScheduledRostersDetails.Where(c => c.roster_header_id == CurrentScheduledRosters.roster_header_id && c.roster_group_id == CurrentScheduledRosters.roster_group_id && c.is_delete_in_Roster==false);
                }
            }
            catch (Exception)
            {
            }
        }

        private void FilterEmployees()
        {
            try
            {
                RosterGroupEmployees = null;
                if (CurrentScheduledRosters != null && CurrentScheduledRosters.roster_group_id != 0)
                {
                    RosterGroupEmployees = listEmployees;
                    RosterGroupEmployees = RosterGroupEmployees.Where(c => c.roster_group_id == CurrentScheduledRosters.roster_group_id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
        }
        #endregion

        #region Search
        private void Search()
        {

        }
        #endregion

        #region Add Button
        private void Add()
        {
            if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                List<RosterCalenderDetailView_PROC_Result> listSchedule = new List<RosterCalenderDetailView_PROC_Result>();
                listSchedule = ScheduledRostersDetails.ToList();
                if (listSchedule.Count(c => c.employee_id == CurrentRosterGroupEmployee.employee_id) == 0)
                {
                    RosterCalenderDetailView_PROC_Result up = new RosterCalenderDetailView_PROC_Result();
                    up.employee_id = CurrentRosterGroupEmployee.employee_id;
                    up.emp_id = CurrentRosterGroupEmployee.emp_id;
                    up.first_name = CurrentRosterGroupEmployee.first_name;
                    up.second_name = CurrentRosterGroupEmployee.second_name;
                    up.surname = CurrentRosterGroupEmployee.surname;
                    listSchedule.Add(up);
                    ScheduledRostersDetails = null;
                    ScheduledRostersDetails = listSchedule;
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
            }
        }

        private bool AddCanExecute()
        {
            if (CurrentScheduledRosters == null)
                return false;
            if (CurrentRosterGroupEmployee == null)
                return false;
            return true;
        }

        public ICommand AddButton
        {
            get { return new RelayCommand(Add, AddCanExecute); }
        }
        #endregion

        #region Remove Button
        private void Remove()
        {
            List<RosterCalenderDetailView_PROC_Result> listSchedule = new List<RosterCalenderDetailView_PROC_Result>();
            listSchedule = ScheduledRostersDetails.ToList();
            if (listSchedule.Count(c => c.employee_id == CurrentScheduledRostersDetails.employee_id) != 0)
            {
                listSchedule.Remove(CurrentScheduledRostersDetails);
                ScheduledRostersDetails = null;
                ScheduledRostersDetails = listSchedule;
            }
        }

        private bool RemoveCanExecute()
        {
            if (CurrentScheduledRosters == null)
                return false;
            if (CurrentScheduledRostersDetails == null)
                return false;
            return true;
        }

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove, RemoveCanExecute); }
        }
        #endregion

        #region Modify Roster
        private void ModifyRoster()
        {
            if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Do you want to modify? ", "", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    z_RosterCalenderHeader rosterHeader = new z_RosterCalenderHeader();
                    rosterHeader.roster_header_id = CurrentScheduledRosters.roster_header_id;
                    rosterHeader.roster_group_id = CurrentScheduledRosters.roster_group_id;
                    rosterHeader.modified_datetime = DateTime.Now;
                    rosterHeader.modified_user_id = clsSecurity.loggedUser.user_id;

                    List<dtl_RosterCalenderDetail> listRosterDetail = new List<dtl_RosterCalenderDetail>();

                    foreach (var item in ScheduledRostersDetails.ToList())
                    {
                        dtl_RosterCalenderDetail dtlRoster = new dtl_RosterCalenderDetail();
                        dtlRoster.roster_header_id = rosterHeader.roster_header_id;
                        dtlRoster.employee_id = item.employee_id;
                        dtlRoster.is_active = true;
                        dtlRoster.is_delete = false;
                        listRosterDetail.Add(dtlRoster);
                    }

                    if (serviceClient.UpdateRosterCalenderHeader(rosterHeader, listRosterDetail.ToArray()))
                    {

                    }
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForUpdate);
            }
        }

        private bool ModifyRosterCanExecute()
        {
            if(CurrentScheduledRosters==null)
                return false;
            if(ScheduledRostersDetails==null)
                return false;
            if(ScheduledRostersDetails.Count() ==0)
                return false;
            return true;
        }

        public ICommand ModifyRosterButton
        {
            get { return new RelayCommand(ModifyRoster, ModifyRosterCanExecute); }
        }
        #endregion
    }
}