using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;


namespace ERP.Leave
{
    public class Calender : ViewModelBase
    {
        List<Day> Dates = new List<Day>();
        ERPServiceClient serviceClient  = new ERPServiceClient();
        public Calender()
        {
            initializingDates();
            AddDates();
  
        }

        public void initializingDates()
        {
            date_01 = new Day("Sunday", 01);
            date_02 = new Day("Monday", 01);
            date_03 = new Day("Tuesday", 01);
            date_04 = new Day("Wednesday", 01);
            date_05 = new Day("Thursday", 01);
            date_06 = new Day("Friday", 01);
            date_07 = new Day("Saturday", 01);

            date_08 = new Day("Sunday", 02);
            date_09 = new Day("Monday", 02);
            date_10 = new Day("Tuesday", 02);
            date_11 = new Day("Wednesday", 02);
            date_12 = new Day("Thursday", 02);
            date_13 = new Day("Friday", 02);
            date_14 = new Day("Saturday", 02);

            date_15 = new Day("Sunday", 03);
            date_16 = new Day("Monday", 03);
            date_17 = new Day("Tuesday", 03);
            date_18 = new Day("Wednesday", 03);
            date_19 = new Day("Thursday", 03);
            date_20 = new Day("Friday", 03);
            date_21 = new Day("Saturday", 03);

            date_22 = new Day("Sunday", 04);
            date_23 = new Day("Monday", 04);
            date_24 = new Day("Tuesday", 04);
            date_25 = new Day("Wednesday", 04);
            date_26 = new Day("Thursday", 04);
            date_27 = new Day("Friday", 04);
            date_28 = new Day("Saturday", 04);

            date_29 = new Day("Sunday", 05);
            date_30 = new Day("Monday", 05);
            date_31 = new Day("Tuesday", 05);
            date_32 = new Day("Wednesday", 05);
            date_33 = new Day("Thursday", 05);
            date_34 = new Day("Friday", 05);
            date_35 = new Day("Saturday", 05);

            date_36 = new Day("Sunday", 06);
            date_37 = new Day("Monday", 06);
            date_38 = new Day("Tuesday", 06);
            date_39 = new Day("Wednesday", 06);
            date_40 = new Day("Thursday", 06);
            date_41 = new Day("Friday", 06);
            date_42 = new Day("Saturday", 06);
           
        }

        private void AddDates()
        {
            Dates.Add(date_01);
            Dates.Add(date_02);
            Dates.Add(date_03);
            Dates.Add(date_04);
            Dates.Add(date_05);
            Dates.Add(date_06);
            Dates.Add(date_07);
            Dates.Add(date_08);
            Dates.Add(date_09);
            Dates.Add(date_10);
            Dates.Add(date_11);
            Dates.Add(date_12);
            Dates.Add(date_13);
            Dates.Add(date_14);
            Dates.Add(date_15);
            Dates.Add(date_16);
            Dates.Add(date_17);
            Dates.Add(date_18);
            Dates.Add(date_19);
            Dates.Add(date_20);
            Dates.Add(date_21);
            Dates.Add(date_22);
            Dates.Add(date_23);
            Dates.Add(date_24);
            Dates.Add(date_25);
            Dates.Add(date_26);
            Dates.Add(date_27);
            Dates.Add(date_28);
            Dates.Add(date_29);
            Dates.Add(date_30);
            Dates.Add(date_31);
            Dates.Add(date_32);
            Dates.Add(date_33);
            Dates.Add(date_34);
            Dates.Add(date_35);
            Dates.Add(date_36);
            Dates.Add(date_37);
            Dates.Add(date_38);
            Dates.Add(date_39);
            Dates.Add(date_40);
            Dates.Add(date_41);
            Dates.Add(date_42); 
        }

        #region Props
   
        private Day _date_01;
        public Day date_01
        {
            get { return _date_01; }
            set { _date_01 = value; OnPropertyChanged("date_01"); }
        }
        private Day _date_02;
        public Day date_02
        {
            get { return _date_02; }
            set { _date_02 = value; OnPropertyChanged("date_02"); }
        }
        private Day _date_03;
        public Day date_03
        {
            get { return _date_03; }
            set { _date_03 = value; OnPropertyChanged("date_03"); }

        }
        private Day _date_04;
        public Day date_04
        {
            get { return _date_04; }
            set { _date_04 = value; OnPropertyChanged("date_04"); }
        }
        private Day _date_05;
        public Day date_05
        {
            get { return _date_05; }
            set { _date_05 = value; OnPropertyChanged("date_05"); }
        }
        private Day _date_06;
        public Day date_06
        {
            get { return _date_06; }
            set { _date_06 = value; OnPropertyChanged("date_06"); }
        }
        private Day _date_07;
        public Day date_07
        {
            get { return _date_07; }
            set { _date_07 = value; OnPropertyChanged("date_07"); }
        }
        private Day _date_08;
        public Day date_08
        {
            get { return _date_08; }
            set { _date_08 = value; }
        }
        private Day _date_09;
        public Day date_09
        {
            get { return _date_09; }
            set { _date_09 = value; }
        }
        private Day _date_10;
        public Day date_10
        {
            get { return _date_10; }
            set { _date_10 = value; }
        }
        private Day _date_11;
        public Day date_11
        {
            get { return _date_11; }
            set { _date_11 = value; }
        }
        private Day _date_12;
        public Day date_12
        {
            get { return _date_12; }
            set { _date_12 = value; }
        }
        private Day _date_13;
        public Day date_13
        {
            get { return _date_13; }
            set { _date_13 = value; }
        }
        private Day _date_14;
        public Day date_14
        {
            get { return _date_14; }
            set { _date_14 = value; }
        }
        private Day _date_15;
        public Day date_15
        {
            get { return _date_15; }
            set { _date_15 = value; }
        }
        private Day _date_16;
        public Day date_16
        {
            get { return _date_16; }
            set { _date_16 = value; }
        }
        private Day _date_17;
        public Day date_17
        {
            get { return _date_17; }
            set { _date_17 = value; }
        }
        private Day _date_18;
        public Day date_18
        {
            get { return _date_18; }
            set { _date_18 = value; }
        }
        private Day _date_19;
        public Day date_19
        {
            get { return _date_19; }
            set { _date_19 = value; }
        }
        private Day _date_20;
        public Day date_20
        {
            get { return _date_20; }
            set { _date_20 = value; }
        }
        private Day _date_21;
        public Day date_21
        {
            get { return _date_21; }
            set { _date_21 = value; }
        }
        private Day _date_22;
        public Day date_22
        {
            get { return _date_22; }
            set { _date_22 = value; }
        }
        private Day _date_23;
        public Day date_23
        {
            get { return _date_23; }
            set { _date_23 = value; }
        }
        private Day _date_24;
        public Day date_24
        {
            get { return _date_24; }
            set { _date_24 = value; }
        }
        private Day _date_25;
        public Day date_25
        {
            get { return _date_25; }
            set { _date_25 = value; }
        }
        private Day _date_26;
        public Day date_26
        {
            get { return _date_26; }
            set { _date_26 = value; }
        }
        private Day _date_27;
        public Day date_27
        {
            get { return _date_27; }
            set { _date_27 = value; }
        }
        private Day _date_28;
        public Day date_28
        {
            get { return _date_28; }
            set { _date_28 = value; }
        }
        private Day _date_29;
        public Day date_29
        {
            get { return _date_29; }
            set { _date_29 = value; }
        }
        private Day _date_30;
        public Day date_30
        {
            get { return _date_30; }
            set { _date_30 = value; }
        }
        private Day _date_31;
        public Day date_31
        {
            get { return _date_31; }
            set { _date_31 = value; }
        }
        private Day _date_32;
        public Day date_32
        {
            get { return _date_32; }
            set { _date_32 = value; }
        }
        private Day _date_33;
        public Day date_33
        {
            get { return _date_33; }
            set { _date_33 = value; }
        }
        private Day _date_34;
        public Day date_34
        {
            get { return _date_34; }
            set { _date_34 = value; }
        }
        private Day _date_35;
        public Day date_35
        {
            get { return _date_35; }
            set { _date_35 = value; }
        }
        private Day _date_36;
        public Day date_36
        {
            get { return _date_36; }
            set { _date_36 = value; }
        }
        private Day _date_37;
        public Day date_37
        {
            get { return _date_37; }
            set { _date_37 = value; }
        }
        private Day _date_38;
        public Day date_38
        {
            get { return _date_38; }
            set { _date_38 = value; }
        }
        private Day _date_39;
        public Day date_39
        {
            get { return _date_39; }
            set { _date_39 = value; }
        }
        private Day _date_40;
        public Day date_40
        {
            get { return _date_40; }
            set { _date_40 = value; }
        }
        private Day _date_41;
        public Day date_41
        {
            get { return _date_41; }
            set { _date_41 = value; }
        }
        private Day _date_42;
        public Day date_42
        {
            get { return _date_42; }
            set { _date_42 = value; }
        }
 
        #endregion

        private DateTime _CalenderDateTime;
        public DateTime CalenderDateTime
        {
            get { return _CalenderDateTime; }
            set { _CalenderDateTime = value; OnPropertyChanged("CalenderDateTime");}
        }

        private IEnumerable<z_Holiday> _Holidays;
        public IEnumerable<z_Holiday> Holidays
        {
            get { return _Holidays; }
            set { _Holidays = value; }
        }

        private IEnumerable<trns_LeavePool> _Leaves;
        public IEnumerable<trns_LeavePool> Leaves
        {
            get { return _Leaves; }
            set { _Leaves = value; }
        }

        public void setHolidays()
        {
            try
            {
                if (CalenderDateTime != null)
                {
                    if (Holidays != null && Dates != null)
                    {
                        foreach (z_Holiday Holiday in Holidays)
                        {
                            if ((bool)Holiday.isActive)
                            {
                                if ((bool)Holiday.isBankHoliday)
                                    Dates.Where(e => e.date == Holiday.holiday_Date.Value.Day).First().isBankHoliday = (bool)Holiday.isBankHoliday;
                                if ((bool)Holiday.isMercantileHoliday)
                                    Dates.Where(e => e.date.Equals(Holiday.holiday_Date.Value.Day)).First().isMercantileHoliday = (bool)Holiday.isMercantileHoliday;
                                if ((bool)Holiday.isPublicHoliday)
                                    Dates.Where(e => e.date.Equals(Holiday.holiday_Date.Value.Day)).First().isPublicHoliday = (bool)Holiday.isPublicHoliday;
                                if ((bool)Holiday.isPoyaHoliday)
                                    Dates.Where(e => e.date.Equals(Holiday.holiday_Date.Value.Day)).First().isPoyaHoliday = (bool)Holiday.isPoyaHoliday;

                                Dates.Where(e => e.date.Equals(Holiday.holiday_Date.Value.Day)).First().Status = Holiday.holiday_Description;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }

        public void setDates()
        {
            try
            {
                if (this.CalenderDateTime != null)
                {
                    var ret = new List<DateTime>();
                    for (int i = 1; i <= DateTime.DaysInMonth(CalenderDateTime.Year, CalenderDateTime.Month); i++)
                    {
                        DateTime newDate = new DateTime(CalenderDateTime.Year, CalenderDateTime.Month, i);

                        foreach (Day dat in Dates)
                        {
                            if (dat.DayName == newDate.DayOfWeek.ToString() && dat.WeekOfMonth == clsDatetimeFormat.GetWeekOfMonth(newDate))
                            {
                                dat.DateProp = newDate.Date;
                                dat.date = newDate.Day;
                                dat.Month = newDate.Month;

                                if (dat.date == DateTime.Today.Day && dat.Month == DateTime.Today.Month)
                                {
                                    dat.OutLineColor = "#FF000000";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }

        public void setLeaves()
        {
            try
            {
                foreach (Day da in Dates)
                {
                    da.OutLineColor = "#FF818181";

                    if (da.date == DateTime.Today.Day && da.Month == DateTime.Today.Month)
                    {
                        da.OutLineColor = "#FF000000";
                    }
                }

                if (Leaves != null)
                {
                    foreach (trns_LeavePool leave in Leaves)
                    {

                        Dates.Where(e => e.DateProp.Equals(leave.leave_date)).First().OutLineColor = "#FF155FFF";
                        //Dates.Where(e => e.DateProp.Equals(leave.leave_date)).First().Status = clsGetNames.getLeaveCategoryByID((Guid)leave.leave_category_id).name;                   
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
