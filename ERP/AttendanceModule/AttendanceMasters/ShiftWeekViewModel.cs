using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ERP.AttendanceService;
using System.Collections;
using System.Windows.Forms;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class ShiftWeekViewModel : ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendClient;

        #endregion

        #region Constructor

        public ShiftWeekViewModel()
        {
            attendClient = new AttendanceServiceClient();
            this.RefreshShiftWeekDetails();
            this.New();
        }

        #endregion

        #region Data Members

        ShiftMasterDetailsView selectedShift;
        public ShiftWeek shiftWeekUI;

        #endregion

        #region List Members

        List<ShiftWeekWithDaysView> allShiftWeekList = new List<ShiftWeekWithDaysView>();
        List<dtl_Shift_Master> allShiftList = new List<dtl_Shift_Master>();

        #region IList

        IList selectDayList = new ArrayList();
        IList selectEmpList = new ArrayList();

        #endregion

        #endregion

        #region Properties

        List<WeekDayShift> weekDays;
        public List<WeekDayShift> WeekDays
        {
            get { return weekDays; }
            set { weekDays = value; OnPropertyChanged("WeekDays"); }
        }

        public IList SelectDayList
        {
            get { return selectDayList; }
            set { selectDayList = value; }
        }

        string weekName;
        public string WeekName
        {
            get { return weekName; }
            set { weekName = value; OnPropertyChanged("WeekName"); }
        }

        string weekDescription;
        public string WeekDescription
        {
            get { return weekDescription; }
            set { weekDescription = value; OnPropertyChanged("WeekDescription"); }
        }

        IEnumerable<ShiftWeekWithDaysView> shiftWeekDays;
        public IEnumerable<ShiftWeekWithDaysView> ShiftWeekDays
        {
            get { return shiftWeekDays; }
            set { shiftWeekDays = value; OnPropertyChanged("ShiftWeekDays"); }
        }

        ShiftWeekWithDaysView currentShiftWeekDay;
        public ShiftWeekWithDaysView CurrentShiftWeekDay
        {
            get { return currentShiftWeekDay; }
            set { currentShiftWeekDay = value; OnPropertyChanged("CurrentShiftWeekDay"); }
        }

        IEnumerable<z_ShiftWeek> shiftWeeks;
        public IEnumerable<z_ShiftWeek> ShiftWeeks
        {
            get { return shiftWeeks; }
            set { shiftWeeks = value; OnPropertyChanged("ShiftWeeks"); }
        }

        z_ShiftWeek currentShiftWeek;
        public z_ShiftWeek CurrentShiftWeek
        {
            get { return currentShiftWeek; }
            set
            {
                currentShiftWeek = value; OnPropertyChanged("CurrentShiftWeek");
                this.SetCurrentShiftWeekDetails();
            }
        }

        IEnumerable<dtl_Shift_Master> shifts;
        public IEnumerable<dtl_Shift_Master> Shifts
        {
            get { return shifts; }
            set { shifts = value; OnPropertyChanged("Shifts"); }
        }

        dtl_Shift_Master currentShift;
        public dtl_Shift_Master CurrentShift
        {
            get { return currentShift; }
            set { currentShift = value; OnPropertyChanged("CurrentShift"); }
        }

        IEnumerable<z_Shift_Category> shiftCategories;
        public IEnumerable<z_Shift_Category> ShiftCategories
        {
            get { return shiftCategories; }
            set { shiftCategories = value; OnPropertyChanged("ShiftCategories"); }
        }

        z_Shift_Category currentShiftCategory;
        public z_Shift_Category CurrentShiftCategory
        {
            get { return currentShiftCategory; }
            set { currentShiftCategory = value; OnPropertyChanged("CurrentShiftCategory"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshShiftWeekDetails()
        {
            attendClient.GetShiftWeekWithDaysDetailsCompleted += (s, e) =>
            {
                try
                {
                    ShiftWeekDays = e.Result;
                    if (shiftWeekDays != null)
                    {
                        allShiftWeekList = shiftWeekDays.ToList();
                        this.FilterShiftWeeks();
                        this.SetDefaultDays();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shift week details refresh is failed");
                }
            };

            attendClient.GetShiftWeekWithDaysDetailsAsync();
        }

        void RefreshtShiftDetails()
        {
            try
            {
                attendClient.GetShiftWithCategoriesCompleted += (s, e) =>
                {
                    Shifts = e.Result;
                    if (shifts != null)
                    {
                        allShiftList = shifts.ToList();
                    }
                };
            }
            catch (Exception)
            {
                clsMessages.setMessage("Shifts refresh is failed");
            }
        }

        #endregion

        #region Button Methods

        #region Select Shift Button

        void SelectShift()
        {
            if (shiftWeekUI != null)
            {
                SearchShiftWindow shiftWindow = new SearchShiftWindow(shiftWeekUI);
                shiftWindow.ShowDialog();
                if (shiftWindow.viewModel.CurrentShiftDetail != null)
                {
                    selectedShift = shiftWindow.viewModel.CurrentShiftDetail;
                    dtl_Shift_Master current = new dtl_Shift_Master();
                    current.shift_detail_id = selectedShift.shift_detail_id;
                    current.shift_detail_name = selectedShift.shift_detail_name;
                    this.SetShiftDay(current);
                }
                shiftWindow.Close();
            }
        }

        public ICommand SelectShiftButton
        {
            get { return new RelayCommand(SelectShift); }
        }

        #endregion

        #region Save Button

        void Save()
        {
            if (IsValidShiftWeek())
            {
                z_ShiftWeek addingWeek = new z_ShiftWeek();
                addingWeek.week_name = weekName;
                addingWeek.week_description = weekDescription;

                List<dtl_ShiftWeek> weekDetailList = new List<dtl_ShiftWeek>();
                foreach (ShiftWeekWithDaysView item in shiftWeekDays.Where(c => c.shift_detail_id != null))
                {
                    dtl_ShiftWeek weekDay = new dtl_ShiftWeek();
                    weekDay.shift_detail_id = (int)item.shift_detail_id;
                    weekDay.day_id = item.day_id;
                    weekDay.trans_id = item.trans_id;
                    weekDetailList.Add(weekDay);
                }

                addingWeek.dtl_ShiftWeek = weekDetailList.ToArray();
                if (currentShiftWeek == null)
                {
                    if (clsSecurity.GetSavePermission(305))
                    {
                        if (attendClient.SaveShiftWeeksDetails(addingWeek))
                        {
                            clsMessages.setMessage("Shift week is saved successfully");
                            this.RefreshShiftWeekDetails();
                            this.New();
                        }
                        else
                        {
                            clsMessages.setMessage("Save is failed");
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                }
                else
                {
                    addingWeek.week_id = currentShiftWeek.week_id;
                    if (clsSecurity.GetUpdatePermission(305))
                    {
                        if (attendClient.UpdateShiftWeekDetails(addingWeek))
                        {
                            clsMessages.setMessage("Shift week is updated successfully");
                            int weekID = currentShiftWeek.week_id;
                            this.RefreshShiftWeekDetails();
                            if (shiftWeeks != null)
                                CurrentShiftWeek = shiftWeeks.FirstOrDefault(c => c.week_id == weekID);

                        }
                        else
                        {
                            clsMessages.setMessage("Save is failed");
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }
            }
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        #endregion

        #region Delete Button

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }

        private void Delete()
        {
            if (currentShiftWeek != null && selectDayList.Count > 0)
            {
                List<dtl_ShiftWeek> deletingList = new List<dtl_ShiftWeek>();
                foreach (ShiftWeekWithDaysView selectDay in selectDayList)
                {
                    dtl_ShiftWeek deletingDayShift = new dtl_ShiftWeek();
                    deletingDayShift.trans_id = selectDay.trans_id;
                    deletingDayShift.day_id = selectDay.day_id;
                    deletingList.Add(deletingDayShift);
                }

                z_ShiftWeek deleteShiftWeek = new z_ShiftWeek();
                deleteShiftWeek.week_id = currentShiftWeek.week_id;
                deleteShiftWeek.dtl_ShiftWeek = deletingList.ToArray();
                if (allShiftWeekList.Where(c => c.week_id == currentShiftWeek.week_id).Count(d => !deletingList.Any(c => c.trans_id == d.trans_id)) > 0) // At least one week day won't be deleted
                {
                    if (clsSecurity.GetDeletePermission(305))
                    {
                        if (attendClient.DeleteShiftWeekDetails(deleteShiftWeek, false))
                        {
                            clsMessages.setMessage("Week shift days are deleted successfully");
                            int weekID = currentShiftWeek.week_id;
                            this.RefreshShiftWeekDetails();
                            if (shiftWeeks != null)
                                CurrentShiftWeek = shiftWeeks.FirstOrDefault(c => c.week_id == weekID);
                        }

                        else
                            clsMessages.setMessage("Week shift days delete is failed");
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Delete this record(s)");
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("Whole Shift week is going to be deleted, Are you sure?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        if (attendClient.DeleteShiftWeekDetails(deleteShiftWeek, true))
                        {
                            clsMessages.setMessage("Week Shift and days are deleted successfully");
                            int weekID = currentShiftWeek.week_id;
                            this.RefreshShiftWeekDetails();
                            if (shiftWeeks != null)
                                CurrentShiftWeek = shiftWeeks.FirstOrDefault(c => c.week_id == weekID);
                        }
                        else
                            clsMessages.setMessage("Week shift and days delete is failed");
                    }
                }
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
            WeekName = null;
            WeekDescription = null;
            CurrentShiftWeek = null;
            this.SetDefaultDays();
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void SetShiftDay(dtl_Shift_Master shft)
        {
            if (selectDayList.Count > 0)
            {
                foreach (ShiftWeekWithDaysView item in selectDayList)
                {
                    ShiftWeekWithDaysView current = shiftWeekDays.FirstOrDefault(c => c.day_id == item.day_id);
                    current.shift_detail_id = shft.shift_detail_id;
                    current.shift_detail_name = shft.shift_detail_name;
                }

                selectDayList.Clear();
            }
        }

        void SetDefaultDays()
        {
            List<ShiftWeekWithDaysView> defaultDays = new List<ShiftWeekWithDaysView>
            {
                new ShiftWeekWithDaysView{day_id = Days.Monday},
                new ShiftWeekWithDaysView{day_id = Days.Tuesday},
                new ShiftWeekWithDaysView{day_id = Days.Wednesday},
                new ShiftWeekWithDaysView{day_id = Days.Thursday},
                new ShiftWeekWithDaysView{day_id = Days.Friday},
                new ShiftWeekWithDaysView{day_id = Days.Saturday},
                new ShiftWeekWithDaysView{day_id= Days.Sunday}
            };
            ShiftWeekDays = null;
            ShiftWeekDays = defaultDays;
        }

        void SetCurrentShiftWeekDetails()
        {
            if (currentShiftWeek != null)
            {
                WeekName = currentShiftWeek.week_name;
                WeekDescription = currentShiftWeek.week_description;
                this.SetDefaultDays();
                foreach (ShiftWeekWithDaysView item in allShiftWeekList.Where(c => c.week_id == currentShiftWeek.week_id))
                {
                    ShiftWeekWithDaysView current = shiftWeekDays.FirstOrDefault(c => c.day_id == item.day_id);
                    current.shift_detail_id = item.shift_detail_id;
                    current.shift_detail_name = item.shift_detail_name;
                    current.trans_id = item.trans_id;
                }
            }
        }

        #endregion

        #region Data Filtering Methods

        void FilterShiftWeeks()
        {
            if (allShiftWeekList.Count > 0)
            {
                ShiftWeeks = allShiftWeekList.Where(c => c.week_id != null).GroupBy(d => d.week_id).Select(grp => grp.First()).Select(c => new z_ShiftWeek { week_id = (int)c.week_id, week_name = c.week_name, week_description = c.week_description });
            }
        }

        #endregion

        #region Validate Methods

        bool IsValidShiftWeek()
        {
            if (weekName == null || weekName == string.Empty)
            {
                clsMessages.setMessage("Week name required to be filled");
                return false;
            }
            if (shiftWeekDays.Count(c => c.shift_detail_id != 0) == 0)
            {
                clsMessages.setMessage("At least one shift required in a week");
                return false;
            }

            return true;
        }

        #endregion
    }
}
