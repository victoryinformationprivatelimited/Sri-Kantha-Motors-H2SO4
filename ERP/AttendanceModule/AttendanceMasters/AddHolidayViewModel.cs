using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class AddHolidayViewModel:ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public AddHolidayViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshHolidayTypes();
            this.RefreshHolidays();
            CurrentHoliday = null;
            CurrentHoliday = new z_HolidayData();
            HolidayName = null;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            StartTime = new TimeSpan(0);
            EndTime = new TimeSpan(0);
        }

        #endregion

        #region Data Members

        public AddHolidayWindow viewModelUI;

        #endregion

        #region List Members

        List<trns_HolidayData> currentHolidayTypesList = new List<trns_HolidayData>();
        List<z_HolidayType> selectedHolidayTypeList = new List<z_HolidayType>();
        
        #endregion

        #region Properties

        IEnumerable<z_HolidayData> holidays;
        public IEnumerable<z_HolidayData> Holidays
        {
            get { if (holidays != null) holidays = holidays.OrderByDescending(c => c.holiday_start); return holidays; }
            set { holidays = value; OnPropertyChanged("Holidays"); }
        }

        z_HolidayData currentHoliday;
        public z_HolidayData CurrentHoliday
        {
            get { return currentHoliday; }
            set 
            { 
                currentHoliday = value; OnPropertyChanged("CurrentHoliday");
                if (currentHoliday != null)
                {
                    this.PopulateCurrentHoliday();
                }
                    
            }
        }

        List<z_HolidayType> holidayTypes;
        public List<z_HolidayType> HolidayTypes
        {
            get { return holidayTypes; }
            set { holidayTypes = value; OnPropertyChanged("HolidayTypes"); }
        }

        z_HolidayType currentHolidayType;
        public z_HolidayType CurrentHolidayType
        {
            get { return currentHolidayType; }
            set { currentHolidayType = value; OnPropertyChanged("CurrentHolidayType"); }
        }

        DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            { 
                startDate = value; OnPropertyChanged("StartDate"); 
                if(startDate != null)
                {
                    EndDate = startDate;
                }
            }
        }

        DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); }
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

        string holidayName;
        public string HolidayName
        {
            get { return holidayName; }
            set { holidayName = value; OnPropertyChanged("HolidayName"); }
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshHolidays()
        {
            attendServiceClient.GetHolidayBasicDetailsCompleted += (s, e) =>
            {
                try
                {
                    Holidays = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Holiday details refres is failed");
                }
            };
            attendServiceClient.GetHolidayBasicDetailsAsync();
        }

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

        #region Save Button

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (currentHoliday != null)
            {
                if (this.SetCurrentData())
                {
                    if (currentHoliday.holiday_id > 0)       // update existing holiday 
                    {
                        if (clsSecurity.GetUpdatePermission(308))
                        {
                            currentHoliday.modified_datetime = DateTime.Now;
                            currentHoliday.modified_user_id = clsSecurity.loggedUser.user_id;

                            if (attendServiceClient.UpdateHolidayData(currentHoliday))
                            {
                                clsMessages.setMessage("Holiday is updated successfully");
                                this.RefreshHolidays();
                                this.New();
                            }
                            else
                            {
                                clsMessages.setMessage("Holiday update is failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                    else                                     // adding new holiday
                    {
                        if (clsSecurity.GetSavePermission(308))
                        {
                            currentHoliday.save_datetime = DateTime.Now;
                            currentHoliday.save_user_id = clsSecurity.loggedUser.user_id;
                            if (attendServiceClient.SaveHolidayData(currentHoliday))
                            {
                                clsMessages.setMessage("Holiday is saved successfully");
                                this.RefreshHolidays();
                                this.New();
                            }
                            else
                            {
                                clsMessages.setMessage("Holiday save is failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
                    } 
                }
            }
        }

        #endregion

        #region Delete Button

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }

        private void Delete()
        {
            if(currentHoliday != null && currentHoliday.holiday_id > 0)
            {
                if (clsSecurity.GetDeletePermission(308))
                {
                    currentHoliday.is_delete = true;
                    currentHoliday.delete_datetime = DateTime.Now;
                    currentHoliday.delete_user_id = clsSecurity.loggedUser.user_id;
                    if (attendServiceClient.DeleteHolidayData(currentHoliday))
                    {
                        clsMessages.setMessage("Holiday is deleted successfully");
                        this.RefreshHolidays();
                        this.New();
                    }
                    else
                    {
                        clsMessages.setMessage("Holiday delete is failed");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        #endregion

        #region New Button

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            CurrentHoliday = null;
            CurrentHoliday = new z_HolidayData();
            HolidayName = null;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            StartTime = new TimeSpan(0);
            EndTime = new TimeSpan(0);
            this.SetAllHolidayTypes();
        }

        #endregion

        #region Add Button

        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }

        private void Add()
        {
            
        }

        #endregion

        #region Bulk Holiday Button

        public ICommand BulkHolidayButton
        {
            get { return new RelayCommand(GetBulkHoliday); }
        }

        private void GetBulkHoliday()
        {
            AddAnyHolidayWindow anyHolidayWindow = new AddAnyHolidayWindow(viewModelUI);
            anyHolidayWindow.ShowDialog();
            Holidays = null;
            this.RefreshHolidays();
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void SetCurrentHolidayTypes()
        {
            if (currentHoliday.holiday_id > 0)
            {
                this.SetAllHolidayTypes();
                currentHolidayTypesList = currentHoliday.trns_HolidayData.ToList();
                foreach (var item in currentHolidayTypesList)
                {
                    var current = holidayTypes.FirstOrDefault(c => c.holiday_type_id == item.holiday_type_id);
                    if (current != null)
                        current.is_active = true;
                }
            }
        }

        void SetAllHolidayTypes()
        {
            if (holidayTypes != null && holidayTypes.Count > 0)
                holidayTypes.ForEach(c => c.is_active = false);
        }

        bool SetCurrentData()
        {
            if (IsValidHolidayName() && IsValidHolidayDate() && IsValidHolidayTime() && IsValidHoliDayType())
            {
                this.SetDateTimeValues();
                currentHoliday.holiday_name = this.holidayName;
                currentHoliday.holiday_start = startDate;
                currentHoliday.holiday_end = endDate;
                currentHoliday.is_active = isActive;
                List<trns_HolidayData> holidayTypeList = new List<trns_HolidayData>();
                foreach (var item in holidayTypes.Where(c=>c.is_active == true))
                {
                    trns_HolidayData addType = new trns_HolidayData();
                    if(currentHoliday.holiday_id > 0)
                    {
                        var selectType = currentHoliday.trns_HolidayData.FirstOrDefault(c => c.holiday_type_id == item.holiday_type_id);
                        if (selectType != null)
                            addType.holiday_trans_id = selectType.holiday_trans_id;
                    }
                    addType.holiday_type_id = item.holiday_type_id;
                    addType.holiday_id = currentHoliday.holiday_id;
                    holidayTypeList.Add(addType);
                }
                currentHoliday.trns_HolidayData = holidayTypeList.ToArray();
                return true;
            }
            return false;
        }

        void PopulateCurrentHoliday()
        {
            if (currentHoliday.holiday_id > 0)
            {
                HolidayName = currentHoliday.holiday_name;
                StartDate = (DateTime)currentHoliday.holiday_start;
                EndDate = (DateTime)currentHoliday.holiday_end;
                StartTime = StartDate.TimeOfDay;
                EndTime = EndDate.TimeOfDay;
                IsActive = currentHoliday.is_active;
                this.SetCurrentHolidayTypes();
            }
        }

        void SetDateTimeValues()
        {
            startDate = startDate.Date.Add(startTime);
            endDate = endDate.Date.Add(endTime);
        }
        
        #endregion

        #region Validation Methods

        bool IsValidHolidayName()
        {
            if(holidayName == null || holidayName == string.Empty)
            {
                clsMessages.setMessage("Holiday name is required");
                return false;
            }
            return true;
        }

        bool IsValidHolidayDate()
        {
            if(startDate == null || endDate == null)
            {
                clsMessages.setMessage("Start Date/ End Date is required");
                return false;
            }
            return true;
        }

        bool IsValidHolidayTime()
        {
            if(startTime == null || endTime == null)
            {
                clsMessages.setMessage("Start Time/ End Time");
                return false;
            }
            return true;
        }

        bool IsValidHoliDayType()
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
