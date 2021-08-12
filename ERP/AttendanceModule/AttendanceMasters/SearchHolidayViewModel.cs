using ERP.AttendanceService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    public class SearchHolidayViewModel:ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendClient;

        #endregion

        #region Constructor

        public SearchHolidayViewModel()
        {
            attendClient = new AttendanceServiceClient();
            this.RefreshYears();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        #endregion

        #region Data Members

        public List<z_HolidayData> selectedHolidayList =  new List<z_HolidayData>();
        public SearchHolidayWindow ownerWindow;

        #endregion

        #region List Members

        List<z_HolidayData> allHolidayList = new List<z_HolidayData>();
        #region IList

        IList selectHolidayList = new ArrayList();

        #endregion

        #endregion

        #region Properties

        IEnumerable<string> holidayYears;
        public IEnumerable<string> HolidayYears
        {
            get { return holidayYears; }
            set { holidayYears = value; OnPropertyChanged("HolidayYears"); }
        }

        string currentHolidayYear;
        public string CurrentHolidayYear
        {
            get { return currentHolidayYear; }
            set 
            { 
                currentHolidayYear = value; OnPropertyChanged("CurrentHolidayYear");
                if (currentHolidayYear != null && currentHolidayYear != string.Empty)
                    this.RefreshHolidaysByYear();
            }
        }

        DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged("StartDate"); }
        }

        DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); }
        }

        IEnumerable<z_HolidayData> holidays;
        public IEnumerable<z_HolidayData> Holidays
        {
            get { return holidays; }
            set { holidays = value; OnPropertyChanged("Holidays"); }
        }

        z_HolidayData currentHoliday;
        public z_HolidayData CurrentHoliday
        {
            get { return currentHoliday; }
            set
            { currentHoliday = value; OnPropertyChanged("CurrentHoliday"); }
        }
        
        public IList SelectHolidayList
        {
            get { return selectHolidayList; }
            set { selectHolidayList = value; }
        }

        bool isSearchAll;
        public bool IsSearchAll
        {
            get { return isSearchAll; }
            set { isSearchAll = value; OnPropertyChanged("IsSearchAll"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshYears()
        {
            attendClient.GetHolidayYearsCompleted += (s, e) =>
            {
                HolidayYears = e.Result;
            };
            attendClient.GetHolidayYearsAsync();
        }

        void RefreshHolidaysByYear()
        {
            attendClient.GetHolidaysBasicDetailsByYearCompleted += (s, e) =>
            {
                allHolidayList.Clear();
                Holidays = e.Result;
                if (holidays != null)
                    allHolidayList = holidays.ToList();
            };

            attendClient.GetHolidaysBasicDetailsByYearAsync(Convert.ToInt32(currentHolidayYear));
        }

        #endregion

        #region Button Methods

        #region Search Button

        public ICommand SearchButton
        {
            get{return new RelayCommand(Search);}
        }

        private void Search()
        {
            this.SearchHolidayByDate();
        }

        #endregion

        #region Select Button

        public ICommand SelectButton
        {
            get { return new RelayCommand(Select); }
        }

        private void Select()
        {
            if(selectHolidayList.Count > 0)
            {
                selectedHolidayList.Clear();
                foreach(z_HolidayData holiday in selectHolidayList)
                {
                    z_HolidayData selectHoliday = new z_HolidayData();
                    selectHoliday.holiday_id = holiday.holiday_id;
                    selectHoliday.holiday_name = holiday.holiday_name;
                    selectHoliday.holiday_start = holiday.holiday_start;
                    selectHoliday.holiday_end = holiday.holiday_end;
                    selectHoliday.is_active = holiday.is_active;
                    selectedHolidayList.Add(selectHoliday);
                }
            }

            if(ownerWindow != null)
            {
                ownerWindow.Close();
            }
        }

        #endregion

        #endregion

        #region Holiday Search Methods

        void SearchHolidayByDate()
        {
            Holidays = null;
            if(isSearchAll)
            {
                Holidays = allHolidayList;
            }
            else
            {
                Holidays = allHolidayList.Where(c => c.holiday_start.Value.Date >= startDate.Date && c.holiday_end.Value.Date <= endDate.Date);
            }
        }

        #endregion
    }
}
