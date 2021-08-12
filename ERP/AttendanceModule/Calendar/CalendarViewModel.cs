using ERP.AttendanceModule.Calendar.Calendar_Items;
using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ERP.AttendanceModule.Calendar
{
    public class CalendarViewModel : ViewModelBase
    {

        #region Service Client

        AttendanceServiceClient attendClient;

        #endregion
        
        #region Constructors

        public CalendarViewModel(CalendarWindow MWindow)
        {
            this.MWindow = MWindow;
            WrapCalender = MWindow.WrapCalender;
            DateSelect = DateTime.Now;
        }

        public CalendarViewModel(CalendarWindow MWindow, Guid? employeeID, int OverLoadControl)
        {
            attendClient = new AttendanceServiceClient();
            this.MWindow = MWindow;
            this.empID = employeeID;
            this.OverLoadControl = OverLoadControl;
            WrapCalender = MWindow.WrapCalender;
            DateSelect = DateTime.Now;
        }

        #endregion

        #region Data Members

        int OverLoadControl = 0;
        WrapPanel WrapCalender;
        CalendarWindow MWindow;
        DateTime firstDate;
        DateTime endDate;
        Guid? empID;

        #endregion

        #region List Members

        List<EmployeeShiftCalendarDetailView> employeeShiftsList = new List<EmployeeShiftCalendarDetailView>();

        #endregion

        #region Properties

        private DateTime _DateSelect;
        public DateTime DateSelect
        {
            get { return _DateSelect; }
            set
            { 
                _DateSelect = value; OnPropertyChanged("DateSelect"); 
                this.SetCurrentDateRange(); 
                OverLoadControlMethod(); 
            }
        }

        private void OverLoadControlMethod()
        {
            if (OverLoadControl == 0)
                CalenderGenerator();
            else if (OverLoadControl == 1)
                RefershShift();
        }

        private string _ColorDate;
        public string ColorDate
        {
            get { return _ColorDate; }
            set { _ColorDate = value; OnPropertyChanged("ColorDate"); }
        } 

        #endregion

        #region Refresh Methods

        private void RefershShift()
        {
            attendClient.GetEmployeeShiftCalendarDetailsCompleted += (s, e) =>
            {
                try
                {
                    this.employeeShiftsList.Clear();
                    this.employeeShiftsList = e.Result.ToList();
                    if(employeeShiftsList.Count > 0)
                    {
                        CalenderGenerator();
                    }
                    
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shifts refresh is failed");
                }
            };

            attendClient.GetEmployeeShiftCalendarDetailsAsync(firstDate, endDate, empID);

        }

        #endregion

        public void CalenderGenerator()
        {
            WrapCalender.Children.Clear();

            #region Calendar Initialize
            DateTime FirstDay = new DateTime(DateSelect.Year, DateSelect.Month, 1);
            DateTime LastDay = FirstDay.AddMonths(1).AddDays(-1);
            int DayOfWeekCount = DayOfWeekCheck(FirstDay.DayOfWeek);

            for (int i = 0; i < 7; i++)
            {
                DayOfWeekUserControl DayOfWeekControl = new DayOfWeekUserControl();
                if (i == 0)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Monday;
                    WrapCalender.Children.Add(DayOfWeekControl);
                }
                else if (i == 1)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Tuesday;
                    WrapCalender.Children.Add(DayOfWeekControl);
                }
                else if (i == 2)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Wednesday;
                    WrapCalender.Children.Add(DayOfWeekControl);
                }
                else if (i == 3)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Thursday;
                    WrapCalender.Children.Add(DayOfWeekControl);
                }
                else if (i == 4)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Friday;
                    WrapCalender.Children.Add(DayOfWeekControl);
                }
                else if (i == 5)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Saturday;
                    DayOfWeekControl.DayOfWeekLable.Foreground = new SolidColorBrush(Colors.SlateGray);
                    WrapCalender.Children.Add(DayOfWeekControl);
                }
                else if (i == 6)
                {
                    DayOfWeekControl.DayOfWeekLable.Content = DayOfWeek.Sunday;
                    DayOfWeekControl.DayOfWeekLable.Foreground = new SolidColorBrush(Colors.Salmon);
                    WrapCalender.Children.Add(DayOfWeekControl);
                }

            }

            for (int i = 1; i <= DayOfWeekCount; i++)
            {
                DateUserControl blank = new DateUserControl(this);
                blank.Visibility = Visibility.Hidden;
                WrapCalender.Children.Add(blank);
            } 
            #endregion

            for (int i = FirstDay.Day; i <= LastDay.Day; i++)
            {
                DateUserControl DateButton = new DateUserControl(this);
                DateButton.Dateselected = i;
                DateButton.DateLabel.Content = i.ToString();
                DateTime currentDate = firstDate.AddDays(i - 1);
                EmployeeShiftCalendarDetailView current = employeeShiftsList.FirstOrDefault(c => c.date == currentDate.Date);
                if (current != null)
                    DateButton.DateContent.Text = current.shift_detail_name + " in " + current.shift_category_name;
                //DateButton.DateContent.Text = FirstDay.AddDays(i - 1).ToLongDateString();
                if (FirstDay.AddDays(i - 1).Date == DateTime.Now.Date)
                    DateButton.CalenderCell.Fill = new SolidColorBrush(Colors.Aqua);
                if (FirstDay.AddDays(i - 1).DayOfWeek == DayOfWeek.Saturday)
                    DateButton.CalenderCell.Fill = new SolidColorBrush(Colors.SlateGray);
                if (FirstDay.AddDays(i - 1).DayOfWeek == DayOfWeek.Sunday)
                    DateButton.CalenderCell.Fill = new SolidColorBrush(Colors.Salmon);
                WrapCalender.Children.Add(DateButton);
            }
        }

        public void SelectedDate(string DateSelected)
        {
            MessageBox.Show(DateSelected.ToString());
        }

        int DayOfWeekCheck(DayOfWeek DayType)
        {
            switch (DayType)
            {
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                default:
                    return 0;
            }
        }

        void SetCurrentDateRange()
        {
            firstDate = new DateTime(DateSelect.Year, DateSelect.Month, 1);
            endDate = firstDate.AddMonths(1).AddDays(-1);
        }
    }
}
