using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace ERP.Attendance.Rosters
{
    class NewRosterCalenderViewModel :ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<EmployeeRosterDetailView> listEmployees = new List<EmployeeRosterDetailView>();
        List<EmployeeRosterDetailView> listSelectedEmployees = new List<EmployeeRosterDetailView>();
        List<z_Holiday> listHolydays = new List<z_Holiday>();
        List<z_Holiday> listCurrentMonthHoliday = new List<z_Holiday>();
        List<RosterCalenderSummary_PROC_Result> lisRosterSummary = new List<RosterCalenderSummary_PROC_Result>();
        #endregion

        #region Constructor
        public NewRosterCalenderViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    serviceClient = new ERPServiceClient();
                    RefreshHoliydays();
                    New();
                    CalenderDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }
        #endregion

        #region Properites
        private string rosterDateString;
        public string RosterDateString
        {
            get { return rosterDateString; }
            set { rosterDateString = value; OnPropertyChanged("RosterDateString"); }
        }
        
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

        private IEnumerable<z_RosterGroup> rosterGroups;
        public IEnumerable<z_RosterGroup> RosterGroups
        {
            get { return rosterGroups; }
            set { rosterGroups = value; OnPropertyChanged("RosterGroups"); }
        }

        private z_RosterGroup currentRosterGroup;
        public z_RosterGroup CurrentRosterGroup
        {
            get { return currentRosterGroup; }
            set { currentRosterGroup = value; OnPropertyChanged("CurrentRosterGroup"); RemoveAll(); FilterEmployees(); }
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

        private IEnumerable<EmployeeRosterDetailView> selectedRosterGroupEmployees;
        public IEnumerable<EmployeeRosterDetailView> SelectedRosterGroupEmployees
        {
            get { return selectedRosterGroupEmployees; }
            set { selectedRosterGroupEmployees = value; OnPropertyChanged("SelectedRosterGroupEmployees"); }
        }

        private EmployeeRosterDetailView selectedCurrentRosterGroupEmployee;
        public EmployeeRosterDetailView SelectedCurrentRosterGroupEmployee
        {
            get { return selectedCurrentRosterGroupEmployee; }
            set { selectedCurrentRosterGroupEmployee = value; OnPropertyChanged("SelectedCurrentRosterGroupEmployee"); }
        }

        private DateTime? calenderDate;
        public DateTime? CalenderDate
        {
            get { return calenderDate; }
            set { calenderDate = value; OnPropertyChanged("CalenderDate"); if (CalenderDate != null) { FilterHolidays(); GetRosterSummary(); LoadCalender(); } }
        }

        private DateTime? rosterDate;
        public DateTime? RosterDate
        {
            get { return rosterDate; }
            set { rosterDate = value; if (RosterDate!=null) RosterDateString = RosterDate.Value.ToShortDateString(); }
        }
        
        //private DateTime? RosterDate;

        private IEnumerable<z_Calender> calender;
        public IEnumerable<z_Calender> Calender
        {
            get { return calender; }
            set { calender = value; OnPropertyChanged("Calender"); }
        }

        private z_Calender currentCalender;
        public z_Calender CurrentCalender
        {
            get { return currentCalender; }
            set { currentCalender = value; OnPropertyChanged("CurrentCalender"); }
        }

        private DataGridColumn currentCalenderColumn;

        private DataGridColumn calenderColumn;
        public DataGridColumn CalenderColumn
        {
            get { return calenderColumn; }
            set
            {
                calenderColumn = value;
                OnPropertyChanged("CalenderColumn");
                if (calenderColumn != null)
                {
                    currentCalenderColumn = calenderColumn;
                    SelectCurrentDate();
                }
            }
        }
        
        #endregion

        #region New Method
        private void New()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {

                RefreshRosterView();
                RefreshRosterGroups();
                RefreshRosterGroupEmployees();
                LoadCalender();
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

        }

        private bool DeleteCanExecute()
        {
            return true;
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }
        #endregion

        #region Refresh Methods
        public void RefreshRosterView()
        {
            try
            {
                serviceClient.GetRosterDetailViewCompleted += (s, e) =>
                {
                    RosterDetails = e.Result;
                };
                serviceClient.GetRosterDetailViewAsync();
            }
            catch (Exception)
            { }
        }

        private void RefreshRosterGroups()
        {
            try
            {
                serviceClient.GetRosterGroupCompleted += (s, e) =>
                {
                    RosterGroups = e.Result;
                };
                serviceClient.GetRosterGroupAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void RefreshHoliydays()
        {
            try
            {
                listHolydays.Clear();
                IEnumerable<z_Holiday> ie;
                ie = serviceClient.GetHolidaysForCalender();
                if (ie != null)
                    listHolydays = ie.ToList();
               /* serviceClient.GetHolidaysForCalenderCompleted += (s, e) =>
                    {
                        ie = e.Result;
                        if (ie != null)
                            listHolydays = ie.ToList();
                    };
                serviceClient.GetHolidaysForCalenderAsync();*/
            }
            catch (Exception)
            { }
        }

        private void GetRosterSummary()
        {
            try
            {
                DateTime d =(DateTime)CalenderDate;
                DateTime startDate = new DateTime(d.Year,d.Month,1);
                DateTime endDate = new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month));
                lisRosterSummary.Clear();
                lisRosterSummary = serviceClient.GetRosterCalenderSummaryw_PROC(startDate, endDate).ToList();
            }
            catch (Exception)
            { }
        }
        #endregion

        #region Search
        private void Search()
        {

        }
        #endregion

        #region Fliter Methods
        private void FilterEmployees()
        {
            try
            {
                RosterGroupEmployees = null;
                if (CurrentRosterGroup != null && CurrentRosterGroup.roster_group_id != 0)
                {
                    RosterGroupEmployees = listEmployees;
                    RosterGroupEmployees = RosterGroupEmployees.Where(c => c.roster_group_id == CurrentRosterGroup.roster_group_id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
        }
        #endregion

        #region Roster Calender
        private void LoadCalender()
        {
            List<z_Calender> listCalender = new List<z_Calender>();
            if (CalenderDate != null)
            {
                int year = ((DateTime)CalenderDate).Year;
                int month = ((DateTime)CalenderDate).Month;

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                DateTime runningDay = new DateTime();

                runningDay = startDate;

                int weekOfMonth = 1;//(runningDay.Day + ((int)runningDay.DayOfWeek)) / 7 + 1;

                System.Globalization.CultureInfo cult_info = System.Globalization.CultureInfo.CreateSpecificCulture("no");
                System.Globalization.Calendar cal = cult_info.Calendar;

                //int weekCount = cal.GetWeekOfYear(runningDay, cult_info.DateTimeFormat.CalendarWeekRule, cult_info.DateTimeFormat.FirstDayOfWeek);
            
                int i = 1;
                while ( i <= endDate.Day)
                {
                    z_Calender day = new z_Calender();
                    day.Week_No =" "+ weekOfMonth.ToString();//cal.GetWeekOfYear(runningDay, cult_info.DateTimeFormat.CalendarWeekRule, cult_info.DateTimeFormat.FirstDayOfWeek).ToString();// ((runningDay.Day + ((int)runningDay.DayOfWeek)) / 7 + 1).ToString();

                    while (i <= endDate.Day)
                    {
                        if (runningDay.DayOfWeek == DayOfWeek.Sunday)
                        {
                            day.Sunday_Date = runningDay;
                            day.Sunday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                        }
                        else if (runningDay.DayOfWeek == DayOfWeek.Monday)
                        {
                            day.Monday_Date = runningDay;
                            day.Monday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                        }
                        else if (runningDay.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            day.Tuesday_Date = runningDay;
                            day.Tuesday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                        }
                        else if (runningDay.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            day.Wednesday_Date = runningDay;
                            day.Wednesday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                        }
                        else if (runningDay.DayOfWeek == DayOfWeek.Thursday)
                        {
                            day.Thursday_Date = runningDay;
                            day.Thursday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                        }
                        else if (runningDay.DayOfWeek == DayOfWeek.Friday)
                        {
                            day.Friday_Date = runningDay;
                            day.Friday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                        }
                        else
                        {
                            day.Saturday_Date = runningDay;
                            day.Saturday = runningDay.Day.ToString() + "\n" + GetHolidays(runningDay) + " " + GetDayRosterSummary(runningDay);
                            runningDay = startDate.AddDays(i);
                            i++;
                            break;
                        }
                        runningDay = startDate.AddDays(i);
                        i++;

                    }
                    weekOfMonth++;
                    listCalender.Add(day);
                }
            }
            Calender = null;
            Calender = listCalender;
        }

        private string GetHolidays(DateTime holiday)
        {
            string text = "";
            try
            {
                List<z_Holiday> list = new List<z_Holiday>();
                list = listCurrentMonthHoliday.Where(c => c.holiday_Date == holiday).ToList();
                foreach (var item in list)
                {
                    text += item.holiday_Description+" ";
                }
                text += "";
                /*string hd = null;
                foreach (var item in list)
                {

                    if (item.isPoyaHoliday == true)
                        hd = "POH";
                    if (item.isPublicHoliday == true)
                    {
                        if (hd != null)
                            hd = "\\";
                        hd += "PH";
                    }
                    if (item.isBankHoliday == true)
                    {
                        if (hd != null)
                            hd = "\\";
                        hd += "BH";
                    }
                    if (item.isMercantileHoliday == true)
                    {
                        if (hd != null)
                            hd = "\\";
                        hd += "MH";
                    }
                    text +="\n"+ hd;
                }*/
            }
            catch (Exception)
            { }
            return text;
        }

        private string GetDayRosterSummary(DateTime runningDay)
        {
            string result = "";
            try
            {
                //List<RosterCalenderSummary_PROC_Result> list = new List<RosterCalenderSummary_PROC_Result>();
                int count = 0;
                count = lisRosterSummary.Where(c => c.roster_calender_date == runningDay).Count();
                if (count > 0)
                    result = "\n No of Rosters - " + count;
            }
            catch (Exception)
            { }
            return result;
        }

        private void FilterHolidays()
        {
            if (CalenderDate!=null)
            {
                listCurrentMonthHoliday = listHolydays.Where(c => c.holiday_Date!=null && ((DateTime)c.holiday_Date).Month == ((DateTime)CalenderDate).Month && ((DateTime)c.holiday_Date).Year == ((DateTime)CalenderDate).Year).ToList(); 
            }
        }
        #endregion

        #region Add Button
        private void Add()
        {
            if (listSelectedEmployees.Count(c=>c.emp_id==CurrentRosterGroupEmployee.emp_id) == 0)
            {
                listSelectedEmployees.Add(CurrentRosterGroupEmployee);
            }
            SelectedRosterGroupEmployees = null;
            SelectedRosterGroupEmployees = listSelectedEmployees;
        }

        private bool AddCanExecute()
        {
            if (CurrentRosterGroupEmployee == null)
                return false;
            return true;
        }

        public ICommand AddButton
        {
            get { return new RelayCommand(Add,AddCanExecute); }
        }
        #endregion

        #region Add All Button
        private void AddAll()
        {
            listSelectedEmployees.Clear();
            if (RosterGroupEmployees != null)
            {
                foreach (var item in RosterGroupEmployees)
                {
                    listSelectedEmployees.Add(item);
                }
            }
            SelectedRosterGroupEmployees = null;
            SelectedRosterGroupEmployees = listSelectedEmployees;
        }

        private bool AddAllCanExecute()
        {
            if (RosterGroupEmployees == null)
                return false;
            if (RosterGroupEmployees.Count() == 0)
                return true;
            return true;
        }

        public ICommand AddAllButton
        {
            get { return new RelayCommand(AddAll, AddAllCanExecute); }
        }
        #endregion

        #region Remove Button
        private void Remove()
        {
            if (listSelectedEmployees.Count(c => c.emp_id == SelectedCurrentRosterGroupEmployee.emp_id) != 0)
            {
                listSelectedEmployees.Remove(SelectedCurrentRosterGroupEmployee);
            }
            SelectedRosterGroupEmployees = null;
            SelectedRosterGroupEmployees = listSelectedEmployees;
        }

        private bool RemoveCanExecute()
        {
            if (SelectedCurrentRosterGroupEmployee == null)
                return false;
            return true;
        }

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove, RemoveCanExecute); }
        }
        #endregion

        #region Remove All Button
        private void RemoveAll()
        {
            listSelectedEmployees.Clear();
            SelectedRosterGroupEmployees = null;
            SelectedRosterGroupEmployees = listSelectedEmployees;
        }

        private bool RemoveAllCanExecute()
        {
            if (SelectedRosterGroupEmployees == null)
                return false;
            if (SelectedRosterGroupEmployees.Count() == 0)
                return false;
            return true;
        }

        public ICommand RemoveAllButton
        {
            get { return new RelayCommand(RemoveAll, RemoveAllCanExecute); }
        }
        #endregion

        #region Add Roster Button
        private void AddRoster()
        {
            if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.EmployeeRoster), clsSecurity.loggedUser.user_id))
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Do you want to save this Roster for " + ((DateTime)RosterDate).ToShortDateString() + "?", "", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    z_RosterCalenderHeader rosterHeader = new z_RosterCalenderHeader();
                    List<dtl_RosterCalenderDetail> listGDetails = new List<dtl_RosterCalenderDetail>();
                    rosterHeader.roster_calender_date = RosterDate;
                    rosterHeader.roster_calender_end_date = ((DateTime)RosterDate).AddDays((int)CurrentRosterDetail.roster_out_day_value);
                    rosterHeader.roster_group_id = CurrentRosterGroup.roster_group_id;
                    rosterHeader.roster_detail_id = CurrentRosterDetail.roster_detail_id;
                    rosterHeader.grace_in = CurrentRosterDetail.grace_in;
                    rosterHeader.grace_out = CurrentRosterDetail.grace_out;
                    rosterHeader.in_day_id = CurrentRosterDetail.in_day_id;
                    rosterHeader.in_time = CurrentRosterDetail.in_time;
                    rosterHeader.is_off = CurrentRosterDetail.is_off;
                    rosterHeader.out_day_id = CurrentRosterDetail.out_day_id;
                    rosterHeader.out_time = CurrentRosterDetail.out_time;
                    rosterHeader.roster_in_day_id = CurrentRosterDetail.roster_in_day_id;
                    rosterHeader.roster_off_time = CurrentRosterDetail.roster_off_time;
                    rosterHeader.roster_on_time = CurrentRosterDetail.roster_on_time;
                    rosterHeader.roster_out_day_id = CurrentRosterDetail.roster_out_day_id;
                    rosterHeader.is_active = true;
                    rosterHeader.is_delete = false;
                    rosterHeader.save_datetime = DateTime.Now;
                    rosterHeader.save_user_id = clsSecurity.loggedUser.user_id;

                    foreach (var item in listSelectedEmployees)
                    {
                        dtl_RosterCalenderDetail dtl = new dtl_RosterCalenderDetail();
                        dtl.is_active = true;
                        dtl.is_delete = false;
                        dtl.employee_id = item.employee_id;
                        listGDetails.Add(dtl);
                    }

                    try
                    {
                        if (serviceClient.SaveRosterCalenderHeader(rosterHeader, listGDetails.ToArray()))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                            // New();
                            if (CalenderDate != null)
                            {
                                FilterHolidays(); GetRosterSummary(); LoadCalender();
                            }
                        }
                        else
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
            }
        }

        private bool AddRosterCanExecute()
        {
            if (CurrentRosterDetail == null)
                return false;
            if (CurrentRosterGroup == null)
                return false;
            if (listSelectedEmployees.Count() == 0)
                return false;
            if (RosterDate == null)
                return false;
            return true;
        }

        public ICommand AddRosterButton
        {
            get { return new RelayCommand(AddRoster, AddRosterCanExecute); }
        }
        #endregion

        #region View Roster Button
        private void ViewRoster()
        {
           // DateTime? CalDate;// = new DateTime();
            if (CurrentCalender != null && currentCalenderColumn != null)
            {
                if (currentCalenderColumn.Header.ToString() == "Sunday")
                    RosterDate = CurrentCalender.Sunday_Date;
                else if (currentCalenderColumn.Header.ToString() == "Monday")
                    RosterDate = CurrentCalender.Monday_Date;
                else if (currentCalenderColumn.Header.ToString() == "Tuesday")
                    RosterDate = CurrentCalender.Tuesday_Date;
                else if (currentCalenderColumn.Header.ToString() == "Wednesday")
                    RosterDate = CurrentCalender.Wednesday_Date;
                else if (currentCalenderColumn.Header.ToString() == "Thursday")
                    RosterDate = CurrentCalender.Thursday_Date;
                else if (currentCalenderColumn.Header.ToString() == "Friday")
                    RosterDate = CurrentCalender.Friday_Date;
                else if (currentCalenderColumn.Header.ToString() == "Saturday")
                    RosterDate = CurrentCalender.Saturday_Date;
                else
                    RosterDate = null;
                if (RosterDate != null)
                {
                    RosterCalenderDateWindow f = new RosterCalenderDateWindow(RosterDate);
                    f.ShowDialog();
                }
            }
        }

        private bool ViewRosterCanExecute()
        {
            return true;
        }

        public ICommand ViewRosterButton
        {
            get { return new RelayCommand(ViewRoster, ViewRosterCanExecute); }
        }
        #endregion

        private void SelectCurrentDate()
        {
            if (CurrentCalender != null && currentCalenderColumn != null)
            {
                try
                {
                    if (currentCalenderColumn.Header.ToString() == "Sunday")
                        RosterDate = CurrentCalender.Sunday_Date;
                    else if (currentCalenderColumn.Header.ToString() == "Monday")
                        RosterDate = CurrentCalender.Monday_Date;
                    else if (currentCalenderColumn.Header.ToString() == "Tuesday")
                        RosterDate = CurrentCalender.Tuesday_Date;
                    else if (currentCalenderColumn.Header.ToString() == "Wednesday")
                        RosterDate = CurrentCalender.Wednesday_Date;
                    else if (currentCalenderColumn.Header.ToString() == "Thursday")
                        RosterDate = CurrentCalender.Thursday_Date;
                    else if (currentCalenderColumn.Header.ToString() == "Friday")
                        RosterDate = CurrentCalender.Friday_Date;
                    else if (currentCalenderColumn.Header.ToString() == "Saturday")
                        RosterDate = CurrentCalender.Saturday_Date;
                    else
                        RosterDate = null;
                }
                catch (Exception)
                { }
            }
        }
    }
}