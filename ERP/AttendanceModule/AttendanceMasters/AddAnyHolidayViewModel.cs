using ERP.AttendanceService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class AddAnyHolidayViewModel:ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public AddAnyHolidayViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshHolidayTypes();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            StartTime = TimeSpan.FromTicks(0);
            EndTime = TimeSpan.FromTicks(0);
        }

        #endregion

        #region Data Members

        static int holidayID = 1;
        public AddAnyHolidayWindow ownerWindow;

        #endregion

        #region List Members

        List<z_HolidayData> selectedHolidayList = new List<z_HolidayData>();
        IList multipleHolidayList = new ArrayList();

        #endregion

        #region Properties

        IEnumerable<z_HolidayData> holidays;
        public IEnumerable<z_HolidayData> Holidays
        {
            get { return holidays; }
            set { holidays = value; OnPropertyChanged("Holidays"); }
        }

        List<z_HolidayType> holidayTypes;
        public List<z_HolidayType> HolidayTypes
        {
            get { return holidayTypes; }
            set { holidayTypes = value; OnPropertyChanged("HolidayTypes"); }
        }

        public IList MultipleHolidayList
        {
            get { return multipleHolidayList; }
            set { multipleHolidayList = value; OnPropertyChanged("MultipleHolidayList"); }
        }

        DayOfWeek currentSelectedDay;
        public DayOfWeek CurrentSelectedDay
        {
            get { return currentSelectedDay; }
            set { currentSelectedDay = value; }
        }

        TimeSpan startTime;
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged("StartTime"); }
        }

        TimeSpan endTime;
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; OnPropertyChanged("EndTime"); }
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

        #endregion

        #region Refresh Methods

        void RefreshHolidayTypes()
        {
            attendServiceClient.GetHolidayTypesBasicDetailsCompleted += (s, e) =>
            {
                try
                {
                    HolidayTypes = e.Result.ToList();
                    if (holidayTypes != null)
                        this.SetAllHolidayTypes();
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Holiday types refresh is failed");
                }
            };

            attendServiceClient.GetHolidayTypesBasicDetailsAsync();
        }

        #endregion

        #region Button Methods

        #region Add Button

        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }

        private void Add()
        {
            if (IsValidDateRange() && IsValidTimeRange())
            {
                this.SetBulkHolidayData();
            }
        }

        #endregion

        #region Clear Button

        public ICommand ClearButton
        {
            get { return new RelayCommand(Clear); }
        }

        private void Clear()
        {
            if(multipleHolidayList != null && multipleHolidayList.Count > 0)
            {
                foreach (z_HolidayData item in multipleHolidayList)
                {
                    var current = selectedHolidayList.FirstOrDefault(c => c.holiday_id == item.holiday_id);
                    selectedHolidayList.Remove(current);
                }

                Holidays = null;
                Holidays = selectedHolidayList;
            }
        }

        #endregion

        #region Save Button

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if(SetAddingHolidayData())
            {
                if(attendServiceClient.SaveBulkHolidayData(selectedHolidayList.ToArray()))
                {
                    clsMessages.setMessage("Bulk holiday data are saved successfully");
                    if (ownerWindow != null)
                    {
                        ownerWindow.Close(); 
                    }
                }
            }
            else
            {
                clsMessages.setMessage("Saving data assign is failed");
            }
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void SetBulkHolidayData()
        {
            DateTimeFormatInfo cul = DateTimeFormatInfo.CurrentInfo;
            System.Globalization.Calendar cal = cul.Calendar;
            DateTime changeDate = startDate.Date;
            // Setting holidays for selected date range giving random names
            for (int i = 1; endDate.Date >= changeDate.Date; i++)
            {
                if (changeDate.DayOfWeek == currentSelectedDay)
                {
                    z_HolidayData addingHoliday = new z_HolidayData();
                    addingHoliday.holiday_id = holidayID;
                    addingHoliday.holiday_name = currentSelectedDay.ToString() + cal.GetWeekOfYear(changeDate, cul.CalendarWeekRule, cul.FirstDayOfWeek).ToString();
                    addingHoliday.holiday_start = changeDate.Add(startTime);
                    addingHoliday.holiday_end = changeDate.Add(endTime);
                    addingHoliday.is_active = true;

                    if (selectedHolidayList.Count(c => c.holiday_start >= addingHoliday.holiday_start && c.holiday_end <= addingHoliday.holiday_end) == 0)
                    {
                        selectedHolidayList.Add(addingHoliday);
                        holidayID++;
                    }
                }
                changeDate = startDate.AddDays(i);
            }

            Holidays = null;
            Holidays = selectedHolidayList;
        }

        void SetAllHolidayTypes()
        {
            if (holidayTypes != null && holidayTypes.Count > 0)
                holidayTypes.ForEach(c => c.is_active = false);
        }

        bool SetAddingHolidayData()
        {
            if(IsValidHolidayType() && selectedHolidayList.Count > 0)
            {
                foreach(z_HolidayData selectHoliday in selectedHolidayList)
                {
                    List<trns_HolidayData> holidayTypeList = new List<trns_HolidayData>();
                    foreach(var item in holidayTypes.Where(c=>c.is_active == true))
                    {
                        trns_HolidayData addType = new trns_HolidayData();
                        addType.holiday_type_id = item.holiday_type_id;
                        holidayTypeList.Add(addType);
                    }
                    selectHoliday.trns_HolidayData = holidayTypeList.ToArray();
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Validation Methods

        bool IsValidDateRange()
        {
            if(startDate == null || endDate == null)
            {
                clsMessages.setMessage("Start/End dates should be filled");
                return false;
            }
            else if(startDate > endDate)
            {
                clsMessages.setMessage("Invalid date range");
                return false;
            }
            return true;
        }

        bool IsValidTimeRange()
        {
            if(startTime == null || endTime == null)
            {
                clsMessages.setMessage("Start/End times should be filled");
                return false;
            }
            else if(startTime > endTime)
            {
                clsMessages.setMessage("Invalid time range");
                return false;
            }

            return true;
        }

        bool IsValidHolidayType()
        {
            if(holidayTypes.Count(c=>c.is_active == true) == 0)
            {
                clsMessages.setMessage("At least one holiday type should be selected");
                return false;
            }
            return true;
        }

        #endregion

    }
}
