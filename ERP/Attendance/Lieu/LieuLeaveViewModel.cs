using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Lieu
{
    class LieuLeaveViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public LieuLeaveViewModel()
        {
            FromDate = null;
            ToDate = null;
            refreshShift();
            RefreshLieuLeaves();
            refreshPeriod();
            IsEnabled = true;
        }
        #endregion

        #region Properties

        private IEnumerable<trns_EmployeeAttendance> _EmployeeAttendance;
        public IEnumerable<trns_EmployeeAttendance> EmployeeAttendance
        {
            get { return _EmployeeAttendance; }
            set { _EmployeeAttendance = value; OnPropertyChanged("EmployeeAttendance"); }
        }

        private IEnumerable<LieuLeavesView> _LieuLeaves;
        public IEnumerable<LieuLeavesView> LieuLeaves
        {
            get { return _LieuLeaves; }
            set { _LieuLeaves = value; OnPropertyChanged("LieuLeaves"); }
        }

        private LieuLeavesView _CurrentLieuLeaves;
        public LieuLeavesView CurrentLieuLeaves
        {
            get { return _CurrentLieuLeaves; }
            set { _CurrentLieuLeaves = value; OnPropertyChanged("CurrentLieuLeaves"); if (CurrentLieuLeaves != null) { LieuDate = CurrentLieuLeaves.lieu_date; IsEnabled = false; CurrentEmployeeAttendance = null; } }
        }

        private IEnumerable<EmployeeShiftDetailView> _Shift;
        public IEnumerable<EmployeeShiftDetailView> Shift
        {
            get { return _Shift; }
            set { _Shift = value; OnPropertyChanged("Shift"); }
        }

        private EmployeeShiftDetailView _CurrentShift;
        public EmployeeShiftDetailView CurrentShift
        {
            get { return _CurrentShift; }
            set { _CurrentShift = value; OnPropertyChanged("CurrentShift"); }
        }

        private IEnumerable<EmployeeSearchView> _EmployeeSearchViews;
        public IEnumerable<EmployeeSearchView> EmployeeSearchViews
        {
            get { return _EmployeeSearchViews; }
            set { _EmployeeSearchViews = value; OnPropertyChanged("EmployeeSearchViews"); }
        }

        private EmployeeSearchView _CurrentEmployeeSearchView;
        public EmployeeSearchView CurrentEmployeeSearchView
        {
            get { return _CurrentEmployeeSearchView; }
            set { _CurrentEmployeeSearchView = value; OnPropertyChanged("CurrentEmployeeSearchView"); if (CurrentEmployeeSearchView != null && FromDate != null && ToDate != null)RefreshEmployeesAttendance(); }
        }

        private trns_EmployeeAttendance _CurrentEmployeeAttendance;
        public trns_EmployeeAttendance CurrentEmployeeAttendance
        {
            get { return _CurrentEmployeeAttendance; }
            set
            {
                _CurrentEmployeeAttendance = value; OnPropertyChanged("CurrentEmployeeAttendance");
                if (_CurrentEmployeeAttendance != null && _CurrentEmployeeAttendance.period_id == Guid.Empty)
                {
                    clsMessages.setMessage("This Employee didn't work on any holiday between  " + _FromDate.Value.ToShortDateString() +
                        " and " + _ToDate.Value.ToShortDateString() + ", Do you still want to continue?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result != Resources.MessageBoxOK)
                        EmployeeAttendance = null;
                }
            }
        }

        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; this.OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set
            {
                _CurrentPeriod = value; this.OnPropertyChanged("CurrentPeriod");

            }
        }

        private bool _IsEnabled;

        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }


        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
        }

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime? _LieuDate;
        public DateTime? LieuDate
        {
            get { return _LieuDate; }
            set { _LieuDate = value; OnPropertyChanged("LieuDate"); }
        }
        #endregion

        #region New
        void New()
        {
            IsEnabled = true;
            CurrentLieuLeaves = null;
            CurrentEmployeeAttendance = null;
            LieuDate = null;
            refreshShift();
            RefreshLieuLeaves();
            refreshPeriod();

        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        #endregion

        #region Holiday Search

        public ICommand HolidaySearchButton
        {
            get { return new RelayCommand(HolidaySearch, ExecuteHolidaySearch); }
        }
        bool ExecuteHolidaySearch()
        {
            if (FromDate == null || ToDate == null)
                return false;
            return true;
        }
        void HolidaySearch()
        {
            RefreshHolidayEmployees();
        }
        #endregion

        #region Save Button & Method
        void Save()
        {
            bool CanSave = true;
            trns_LieuLeaves newLieuLeave = new trns_LieuLeaves();
            if (CurrentEmployeeAttendance == null)
            {
                if (CurrentLieuLeaves == null)
                {
                    clsMessages.setMessage("Select an Emloyee and a Attend Date");
                    CanSave = false;
                }
                else
                {
                    newLieuLeave.employee_id = CurrentLieuLeaves.employee_id;
                    newLieuLeave.holiday_work_day_out = CurrentLieuLeaves.holiday_work_day_out;
                    newLieuLeave.holiday_work_day_in = CurrentLieuLeaves.holiday_work_day_in;
                }
            }
            else
            {
                newLieuLeave.employee_id = CurrentEmployeeAttendance.employee_id;
                newLieuLeave.holiday_work_day_out = CurrentEmployeeAttendance.out_datetime;
                newLieuLeave.holiday_work_day_in = CurrentEmployeeAttendance.in_datetime;
            }
            newLieuLeave.is_processed = false;
            newLieuLeave.shift_id = CurrentShift.shift_detail_id;
            newLieuLeave.save_user_id = clsSecurity.loggedUser.user_id;
            newLieuLeave.save_datetime = System.DateTime.Now;
            newLieuLeave.lieu_date = LieuDate.Value;
            newLieuLeave.period_id = CurrentPeriod.period_id;

            if (CanSave) 
                if (serviceClient.SaveLieuLeave(newLieuLeave))
                {
                    New();
                    clsMessages.setMessage(Resources.SaveSucess);
                }
                else
                    clsMessages.setMessage(Resources.SaveFail);

        }
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, saveCanExecote); }
        }

        private bool saveCanExecote()
        {
            if (CurrentShift == null || CurrentPeriod == null || LieuDate == null)
                return false;
            else
                return true;
        }
        #endregion

        #region LieuProcessButton
        public ICommand LieuProcessButton
        {
            get { return new RelayCommand(LieuProcess); }
        }

        private void LieuProcess()
        {
            if (serviceClient.LieuProcess())
            {
                clsMessages.setMessage(Resources.UpdateSucess);
                RefreshLieuLeaves();
            }
            else
                clsMessages.setMessage(Resources.UpdateFail);
        }
        #endregion

        #region Delete Button
        void Delete()
        {
            trns_LieuLeaves newLieuLeave = new trns_LieuLeaves();

            newLieuLeave.employee_id = CurrentLieuLeaves.employee_id;
            newLieuLeave.lieu_date = CurrentLieuLeaves.lieu_date;

            if (serviceClient.DeleteLieuLeave(newLieuLeave))
            {
                clsMessages.setMessage(Resources.DeleteSucess);
                New();
            }
            else
                clsMessages.setMessage(Resources.DeleteFail);
        }
        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, deleteCanExecute); }
        }

        private bool deleteCanExecute()
        {
            if (CurrentLieuLeaves == null)
                return false;
            else return true;
        }
        #endregion

        #region Search
        public ICommand SearchButton
        {
            get { return new RelayCommand(EmployeeSearch, ExecuteHolidaySearch); }
        }
        void EmployeeSearch()
        {
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
            {
                EmployeeSearchViews = null;
                EmployeeSearchViews = window.viewModel.selectEmployeeList;
            }
            window.Close();
            window = null;
        }
        #endregion

        #region Refresh
        void RefreshLieuLeaves()
        {
            serviceClient.GetLieuDataCompleted += (s, e) =>
            {
                try
                {
                    LieuLeaves = e.Result.OrderBy(c => c.emp_id);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshLieuLeaves()" + ex);
                }
            };
            serviceClient.GetLieuDataAsync();
        }

        void refreshShift()
        {

            serviceClient.GetShiftDetailsViewListCompleted += (s, e) =>
            {
                try
                {
                    this.Shift = e.Result.OrderBy(c => c.shift_name);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("refreshShift()" + ex);
                }
            };
            this.serviceClient.GetShiftDetailsViewListAsync();

        }

        private void RefreshEmployeesAttendance()
        {
            serviceClient.GetDateRangeEmployeeAttendancesCompleted += (s, e) =>
            {
                try
                {
                    if (e.Result.Count() == 0)
                    {
                        List<trns_EmployeeAttendance> ListEmp = new List<trns_EmployeeAttendance>();
                        trns_EmployeeAttendance trnsEmp = new trns_EmployeeAttendance();
                        trnsEmp.employee_id = CurrentEmployeeSearchView.employee_id;
                        ListEmp.Add(trnsEmp);
                        EmployeeAttendance = ListEmp;
                    }
                    else
                        EmployeeAttendance = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEmployeesAttendance()" + ex);
                }
            };
            serviceClient.GetDateRangeEmployeeAttendancesAsync(FromDate.Value, ToDate.Value, CurrentEmployeeSearchView.employee_id);
        }

        private void refreshPeriod()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                this.Periods = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetPeriodsAsync();
        }

        void RefreshHolidayEmployees()
        {
            serviceClient.GetHolidayEmployeesCompleted += (s, e) =>
            {
                try
                {
                    EmployeeSearchViews = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshHolidayEmployees()" + ex);
                }
            };
            serviceClient.GetHolidayEmployeesAsync(FromDate.Value, ToDate.Value);
        }
        #endregion
    }
}
