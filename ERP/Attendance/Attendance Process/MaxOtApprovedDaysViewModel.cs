using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using ERP.BasicSearch;
using System.Windows.Input;
using System.Windows.Forms;

namespace ERP.Attendance.Basic_Masters
{
    class MaxOtApprovedDaysViewModel : ViewModelBase
    {
        #region Service Client
        ERPServiceClient serviceClient;
        #endregion

        TimeSpan DefaultOt = new TimeSpan(01, 00, 00);
        List<dtl_MaxOtApprovedEmployees> listMaxOtApprovedEmp = new List<dtl_MaxOtApprovedEmployees>();

        #region Constructor
        public MaxOtApprovedDaysViewModel()
        {
            serviceClient = new ERPServiceClient();
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
            Sunday = false;
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            TimePeriod = true;
            RefreshBasicDays();
            RefreshMaxOtApprovedEmployees();
            RefreshDefaultMaxOtData();
            RefreshRandomMaxOtData();
        }

        #endregion

        #region Properties

        #region List Properties

        private IEnumerable<trns_RandomMaxOT> _RandomMaxOT;
        public IEnumerable<trns_RandomMaxOT> RandomMaxOT
        {
            get { return _RandomMaxOT; }
            set { _RandomMaxOT = value; OnPropertyChanged("RandomMaxOT"); }
        }


        private IEnumerable<dtl_DefaultMaxOT> _DefaultMaxOT;
        public IEnumerable<dtl_DefaultMaxOT> DefaultMaxOT
        {
            get { return _DefaultMaxOT; }
            set { _DefaultMaxOT = value; OnPropertyChanged("DefaultMaxOT"); }
        }

        private IEnumerable<EmployeeSearchView> _EmployeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _EmployeeSearch; }
            set { _EmployeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        #endregion

        #region Table Properties

        private IEnumerable<z_BasicDay> _BasicDay;
        public IEnumerable<z_BasicDay> BasicDay
        {
            get { return _BasicDay; }
            set { _BasicDay = value; OnPropertyChanged("BasicDay"); }
        }

        private IEnumerable<dtl_MaxOtApprovedEmployees> _MaxOtEmployees;
        public IEnumerable<dtl_MaxOtApprovedEmployees> MaxOtEmployees
        {
            get { return _MaxOtEmployees; }
            set { _MaxOtEmployees = value; OnPropertyChanged("MaxOtEmployees"); }
        }

        private IEnumerable<dtl_DefaultMaxOT> _DefaulMaxOtData;
        public IEnumerable<dtl_DefaultMaxOT> DefaulMaxOtData
        {
            get { return _DefaulMaxOtData; }
            set { _DefaulMaxOtData = value; OnPropertyChanged("DefaulMaxOtData"); }
        }

        private IEnumerable<trns_RandomMaxOT> _RandomMaxOtdata;
        public IEnumerable<trns_RandomMaxOT> RandomMaxOtdata
        {
            get { return _RandomMaxOtdata; }
            set { _RandomMaxOtdata = value; OnPropertyChanged("RandomMaxOtdata"); }
        }
        #endregion

        #region CheckBoxPrpperties

        private bool _TimePeriod;
        public bool TimePeriod
        {
            get { return _TimePeriod; }
            set { _TimePeriod = value; OnPropertyChanged("TimePeriod"); }
        }

        private bool _Default;
        public bool Default
        {
            get { return _Default; }
            set { _Default = value; OnPropertyChanged("Default"); isDefaultSelect(); }
        }

        private bool _MaxOtApproved;
        public bool MaxOtApproved
        {
            get { return _MaxOtApproved; }
            set { _MaxOtApproved = value; OnPropertyChanged("MaxOtApproved"); }
        }

        private DateTime _FromDate;
        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime _ToDate;
        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
        }

        private bool _Sunday;
        public bool Sunday
        {
            get { return _Sunday; }
            set { _Sunday = value; OnPropertyChanged("Sunday"); }
        }

        private bool _Monday;
        public bool Monday
        {
            get { return _Monday; }
            set { _Monday = value; OnPropertyChanged("Monday"); }
        }

        private bool _Tuesday;
        public bool Tuesday
        {
            get { return _Tuesday; }
            set { _Tuesday = value; OnPropertyChanged("Tuesday"); }
        }

        private bool _Wednesday;
        public bool Wednesday
        {
            get { return _Wednesday; }
            set { _Wednesday = value; OnPropertyChanged("Wednesday"); }
        }

        private bool _Thursday;
        public bool Thursday
        {
            get { return _Thursday; }
            set { _Thursday = value; OnPropertyChanged("Thursday"); }
        }

        private bool _Friday;
        public bool Friday
        {
            get { return _Friday; }
            set { _Friday = value; OnPropertyChanged("Friday"); }
        }

        private bool _Saturday;
        public bool Saturday
        {
            get { return _Saturday; }
            set { _Saturday = value; OnPropertyChanged("Saturday"); }
        }

        #endregion

        #region MorningOtTextBoxProperties

        private TimeSpan _SundayMorningOT;
        public TimeSpan SundayMorningOT
        {
            get { return _SundayMorningOT; }
            set { _SundayMorningOT = value; OnPropertyChanged("SundayMorningOT"); }
        }

        private TimeSpan _MondayMorningOT;
        public TimeSpan MondayMorningOT
        {
            get { return _MondayMorningOT; }
            set { _MondayMorningOT = value; OnPropertyChanged("MondayMorningOT"); }
        }

        private TimeSpan _TuesdayMorningOT;
        public TimeSpan TuesdayMorningOT
        {
            get { return _TuesdayMorningOT; }
            set { _TuesdayMorningOT = value; OnPropertyChanged("TuesdayMorningOT"); }
        }

        private TimeSpan _WednesdayMorningOT;
        public TimeSpan WednesdayMorningOT
        {
            get { return _WednesdayMorningOT; }
            set { _WednesdayMorningOT = value; OnPropertyChanged("WednesdayMorningOT"); }
        }

        private TimeSpan _ThursdayMorningOT;
        public TimeSpan ThursdayMorningOT
        {
            get { return _ThursdayMorningOT; }
            set { _ThursdayMorningOT = value; OnPropertyChanged("ThursdayMorningOT"); }
        }

        private TimeSpan _FridayMorningOT;
        public TimeSpan FridayMorningOT
        {
            get { return _FridayMorningOT; }
            set { _FridayMorningOT = value; OnPropertyChanged("FridayMorningOT"); }
        }

        private TimeSpan _SaturdayMorningOT;
        public TimeSpan SaturdayMorningOT
        {
            get { return _SaturdayMorningOT; }
            set { _SaturdayMorningOT = value; OnPropertyChanged("SaturdayMorningOT"); }
        }


        #endregion

        #region EveningOtTextBoxProperties

        private TimeSpan _SundayEveningOT;
        public TimeSpan SundayEveningOT
        {
            get { return _SundayEveningOT; }
            set { _SundayEveningOT = value; OnPropertyChanged("SundayEveningOT"); }
        }

        private TimeSpan _MondayEveningOT;
        public TimeSpan MondayEveningOT
        {
            get { return _MondayEveningOT; }
            set { _MondayEveningOT = value; OnPropertyChanged("MondayEveningOT"); }
        }

        private TimeSpan _TuesdayEveningOT;
        public TimeSpan TuesdayEveningOT
        {
            get { return _TuesdayEveningOT; }
            set { _TuesdayEveningOT = value; OnPropertyChanged("TuesdayEveningOT"); }
        }

        private TimeSpan _WednesdayEveningOT;
        public TimeSpan WednesdayEveningOT
        {
            get { return _WednesdayEveningOT; }
            set { _WednesdayEveningOT = value; OnPropertyChanged("WednesdayEveningOT"); }
        }

        private TimeSpan _ThursdayEveningOT;
        public TimeSpan ThursdayEveningOT
        {
            get { return _ThursdayEveningOT; }
            set { _ThursdayEveningOT = value; OnPropertyChanged("ThursdayEveningOT"); }
        }

        private TimeSpan _FridayEveningOT;
        public TimeSpan FridayEveningOT
        {
            get { return _FridayEveningOT; }
            set { _FridayEveningOT = value; OnPropertyChanged("FridayEveningOT"); }
        }

        private TimeSpan _SaturdayEveningOT;
        public TimeSpan SaturdayEveningOT
        {
            get { return _SaturdayEveningOT; }
            set { _SaturdayEveningOT = value; OnPropertyChanged("SaturdayEveningOT"); }
        }

        #endregion

        #endregion

        #region Select Button

        public ICommand SelectButton
        {
            get { return new RelayCommand(Select); }
        }

        void Select()
        {
            List<EmployeeSearchView> tempEmployeeSearch = new List<EmployeeSearchView>();
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            EmployeeSearch = null;
            if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                EmployeeSearch = window.viewModel.selectEmployeeList;
            window.Close();
            window = null;

        }

        #endregion

        #region Save Button

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        void Save()
       {
            List<dtl_DefaultMaxOT> SaveDefaultList = new List<dtl_DefaultMaxOT>();
            List<dtl_DefaultMaxOT> UpdateDefaultList = new List<dtl_DefaultMaxOT>();
            List<trns_RandomMaxOT> SaveRandomList = new List<trns_RandomMaxOT>();
            List<trns_RandomMaxOT> UpdateRandomList = new List<trns_RandomMaxOT>();


            if (EmployeeSearch != null)

                foreach (var item in EmployeeSearch)
                {
                    dtl_MaxOtApprovedEmployees maxOtEmployees = new dtl_MaxOtApprovedEmployees();
                    maxOtEmployees.empID = item.emp_id;
                    maxOtEmployees.isMaxOtApproved = MaxOtApproved;
                    maxOtEmployees.isDefault = Default;

                    if (MaxOtEmployees != null && MaxOtEmployees.Where(c => c.empID.Contains(item.emp_id)).Count() >= 1)
                    {
                        if (serviceClient.MaxOtApprovedEmployeesUpdate(maxOtEmployees))
                        {
                            MessageBox.Show("Max Overtime employees updated");
                        }
                        else
                        {
                            MessageBox.Show("Max Overtime employees update faild");
                        }
                    }
                    else
                    {
                        if (serviceClient.MaxOtApprovedEmployeesSave(maxOtEmployees))
                        {
                            MessageBox.Show("Max Overtime employees saved");
                        }

                        else
                        {
                            MessageBox.Show("Max Overtime employees save faild");
                        }
                    }



                    if (true)
                    {
                        #region Default
                        if (Default)
                        {
                            foreach (var Day in BasicDay)
                            {
                                dtl_DefaultMaxOT defaultOT = new dtl_DefaultMaxOT();
                                defaultOT.empID = item.emp_id;
                                defaultOT.dayID = Day.day_id;
                                defaultOT.day_of_week = Day.day_of_week;
                                defaultOT.maxMoOT = GetMrTimeSpan(Day.day_of_week);
                                defaultOT.maxEvOT = GetEvTimeSpan(Day.day_of_week);
                                defaultOT.isActive = true;
                                List<dtl_DefaultMaxOT> CheckEmp = new List<dtl_DefaultMaxOT>();
                                try
                                {
                                    CheckEmp = DefaulMaxOtData.Where(c => c.dayID == Day.day_id && c.empID == item.emp_id).ToList();

                                }
                                catch (Exception)
                                {
                                    CheckEmp = new List<dtl_DefaultMaxOT>();
                                }
                                if (CheckEmp != null && CheckEmp.Count == 1)
                                    UpdateDefaultList.Add(defaultOT);

                                else
                                    SaveDefaultList.Add(defaultOT);

                            }
                        }
                        #endregion

                        else
                        {
                            int NoOfDays = (ToDate - FromDate).Days;
                            DateTime StartingDate;
                            for (int i = 0; i <= NoOfDays; i++)
                            {
                                StartingDate = FromDate.AddDays(i);
                                trns_RandomMaxOT RandomMaxOt = new trns_RandomMaxOT();
                                RandomMaxOt.empId = item.emp_id;
                                RandomMaxOt.date = StartingDate.Date;
                                RandomMaxOt.dayId = GetDayOfWeek(StartingDate.DayOfWeek.ToString());
                                RandomMaxOt.maxMoOT = GetMrTimeSpan(StartingDate.DayOfWeek.ToString());
                                RandomMaxOt.maxEvOT = GetEvTimeSpan(StartingDate.DayOfWeek.ToString());
                                RandomMaxOt.isActive = true;

                                List<trns_RandomMaxOT> CheckEmp = new List<trns_RandomMaxOT>();
                                try
                                {
                                    if (RandomMaxOtdata != null)
                                        CheckEmp = RandomMaxOtdata.Where(c => c.date == StartingDate.Date && c.empId == item.emp_id).ToList();

                                }
                                catch (NullReferenceException)
                                {
                                    CheckEmp = new List<trns_RandomMaxOT>();
                                }
                                catch (Exception)
                                {
                                    CheckEmp = new List<trns_RandomMaxOT>();
                                }
                                if (CheckEmp != null && CheckEmp.Count == 1)
                                {
                                    UpdateRandomList.Add(RandomMaxOt);
                                }
                                else
                                {
                                    SaveRandomList.Add(RandomMaxOt);
                                }

                            }

                        }
                    }



                    if (SaveRandomList.Count != 0)
                    {

                        if (serviceClient.RandomMaxOtSave(SaveRandomList.ToArray()))
                        {
                            MessageBox.Show("Random Max Overtime Saved");
                            SaveRandomList.Clear();

                        }
                        else
                        {
                            MessageBox.Show("Random Max Overtime Save fail");
                            SaveRandomList.Clear();
                        }
                    }

                    if (UpdateRandomList.Count != 0)
                    {

                        if (serviceClient.RandomMaxOtUpdate(UpdateRandomList.ToArray()))
                        {
                            MessageBox.Show("Random Max Overtime Updated");
                            UpdateRandomList.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Random Max Overtime Update fail");
                            UpdateRandomList.Clear();
                        }

                    }

                    if (SaveDefaultList.Count != 0)
                    {
                        if (serviceClient.DefaultMaxOtSave(SaveDefaultList.ToArray()))
                        {
                            MessageBox.Show("Default Max Overtime Save");
                            SaveDefaultList.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Default Max Overtime save fail");
                            SaveDefaultList.Clear();
                        }
                    }
                    if (UpdateDefaultList.Count != 0)
                    {

                        if (serviceClient.DefaultMaxOtUpdate(UpdateDefaultList.ToArray()))
                        {
                            MessageBox.Show("Default Max Overtime updated");
                            UpdateDefaultList.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Default Max Overtime update fail");
                            UpdateDefaultList.Clear();
                        }

                    }
                }

        }

        TimeSpan GetEvTimeSpan(string DayOfWeekSwitch)
        {

            switch (DayOfWeekSwitch)
            {
                case "Sunday":
                    if (Sunday)
                        return SundayEveningOT;
                    else
                        return new TimeSpan();
                case "Monday":
                    if (Monday)
                        return MondayEveningOT;
                    else
                        return new TimeSpan();
                case "Tuesday":
                    if (Tuesday)
                        return TuesdayEveningOT;
                    else
                        return new TimeSpan();
                case "Wednesday":
                    if (Wednesday)
                        return WednesdayEveningOT;
                    else
                        return new TimeSpan();
                case "Thursday":
                    if (Thursday)
                        return ThursdayEveningOT;
                    else
                        return new TimeSpan();
                case "Friday":
                    if (Friday)
                        return FridayEveningOT;
                    else
                        return new TimeSpan();
                case "Saturday":
                    if (Saturday)
                        return SaturdayEveningOT;
                    else
                        return new TimeSpan();


            }
            return new TimeSpan();
        }
        TimeSpan GetMrTimeSpan(string DayOfWeekSwitch)
        {

            switch (DayOfWeekSwitch)
            {
                case "Sunday":
                    if (Sunday)
                        return SundayMorningOT;
                    else
                        return new TimeSpan();
                case "Monday":
                    if (Monday)
                        return MondayMorningOT;
                    else
                        return new TimeSpan();
                case "Tuesday":
                    if (Tuesday)
                        return TuesdayMorningOT;
                    else
                        return new TimeSpan();
                case "Wednesday":
                    if (Wednesday)
                        return WednesdayMorningOT;
                    else
                        return new TimeSpan();
                case "Thursday":
                    if (Thursday)
                        return ThursdayMorningOT;
                    else
                        return new TimeSpan();
                case "Friday":
                    if (Friday)
                        return FridayMorningOT;
                    else
                        return new TimeSpan();
                case "Saturday":
                    if (Saturday)
                        return SaturdayMorningOT;
                    else
                        return new TimeSpan();
            }
            return new TimeSpan();
        }
        #endregion

        #region Clear Button

        public ICommand CleartButton
        {
            get { return new RelayCommand(Clear); }
        }

        void Clear()
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
            Sunday = false;
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            MondayMorningOT = new TimeSpan();
            SundayEveningOT = new TimeSpan();
            MondayMorningOT = new TimeSpan();
            MondayEveningOT = new TimeSpan();
            TuesdayMorningOT = new TimeSpan();
            TuesdayEveningOT = new TimeSpan();
            WednesdayMorningOT = new TimeSpan();
            WednesdayEveningOT = new TimeSpan();
            ThursdayMorningOT = new TimeSpan();
            ThursdayEveningOT = new TimeSpan();
            FridayMorningOT = new TimeSpan();
            FridayEveningOT = new TimeSpan();
            SaturdayMorningOT = new TimeSpan();
            SaturdayEveningOT = new TimeSpan();
        }

        #endregion

        #region Refresh Methods
        void RefreshBasicDays()
        {
            serviceClient.GetBasicDaysCompleted += (s, e) =>
            {
                BasicDay = e.Result;
            };
            serviceClient.GetBasicDaysAsync();
        }

        void RefreshMaxOtApprovedEmployees()
        {
            serviceClient.GetMaxOtApprovedEmployeesCompleted += (s, e) =>
            {
                MaxOtEmployees = e.Result;
            };
            serviceClient.GetMaxOtApprovedEmployeesAsync();

        }

        void RefreshDefaultMaxOtData()
        {
            serviceClient.GetDeafaultMaxOTCompleted += (s, e) =>
            {
                DefaulMaxOtData = e.Result;
            };
            serviceClient.GetDeafaultMaxOTAsync();
        }

        void RefreshRandomMaxOtData()
        {
            serviceClient.GetRandomMaxOtCompleted += (s, e) =>
            {
                RandomMaxOtdata = e.Result;
            };
            serviceClient.GetRandomMaxOtAsync();
        }
        #endregion



        private void isDefaultSelect()
        {
            if (Default)
            {
                Sunday = true;
                Monday = true;
                Tuesday = true;
                Wednesday = true;
                Thursday = true;
                Friday = true;
                Saturday = true;
                SundayMorningOT = DefaultOt;
                SundayEveningOT = DefaultOt;
                MondayMorningOT = DefaultOt;
                MondayEveningOT = DefaultOt;
                TuesdayMorningOT = DefaultOt;
                TuesdayEveningOT = DefaultOt;
                WednesdayMorningOT = DefaultOt;
                WednesdayEveningOT = DefaultOt;
                ThursdayMorningOT = DefaultOt;
                ThursdayEveningOT = DefaultOt;
                FridayMorningOT = DefaultOt;
                FridayEveningOT = DefaultOt;
                SaturdayMorningOT = DefaultOt;
                SaturdayEveningOT = DefaultOt;
                TimePeriod = false;
            }
            else
            {
                Sunday = false;
                Monday = false;
                Tuesday = false;
                Wednesday = false;
                Thursday = false;
                Friday = false;
                Saturday = false;
                SundayMorningOT = new TimeSpan();
                SundayEveningOT = new TimeSpan();
                MondayMorningOT = new TimeSpan();
                MondayEveningOT = new TimeSpan();
                TuesdayMorningOT = new TimeSpan();
                TuesdayEveningOT = new TimeSpan();
                WednesdayMorningOT = new TimeSpan();
                WednesdayEveningOT = new TimeSpan();
                ThursdayMorningOT = new TimeSpan();
                ThursdayEveningOT = new TimeSpan();
                FridayMorningOT = new TimeSpan();
                FridayEveningOT = new TimeSpan();
                SaturdayMorningOT = new TimeSpan();
                SaturdayEveningOT = new TimeSpan();
                TimePeriod = true;
            }
        }

        Guid GetDayOfWeek(String DayOfWeek)
        {
            z_BasicDay tempBasicDay = BasicDay.Where(c => c.day_of_week.ToUpper().Contains(DayOfWeek.ToUpper())).FirstOrDefault();
            return tempBasicDay.day_id;
        }
    }
}
