using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.Leave
{
  public class Day
    {

      public Day(string DayName,int WeekOfMonth)
      {
          this.DayName = DayName;
          this.WeekOfMonth = WeekOfMonth;
          this.OutLineColor = "#FF818181";
          this.PoyaProp = Visibility.Hidden;
          this.MercantileProp = Visibility.Hidden;
          this.PublicProp = Visibility.Hidden;
          this.BankProp = Visibility.Hidden;
      }

        private int _date;
        public int date
        {
            get { return _date; }
            set { _date = value; }
        }

        private int _Month;
        public int Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        private DateTime _DateProp;
        public DateTime DateProp
        {
            get { return _DateProp; }
            set { _DateProp = value;  }
        }
  
        private string _DayName;
        public string DayName
        {
            get { return _DayName; }
            set { _DayName = value; }
        }

        private int _WeekOfMonth;
        public int WeekOfMonth
        {
            get { return _WeekOfMonth; }
            set { _WeekOfMonth = value; }
        }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _BckColor01;
        public string BckColor
        {
            get { return _BckColor01; }
            set { _BckColor01 = value; }
        }

        private bool _isHoliday;
        public bool isHoliday
        {
            get { return _isHoliday; }
            set { _isHoliday = value; }
        }

        private bool _isPublicHoliday;
        public bool isPublicHoliday
        {
            get { return _isPublicHoliday; }
            set
            {
                _isPublicHoliday = value;
                 if (isPublicHoliday)
                    PublicProp = Visibility.Visible;
                
            }
        }

        private bool _isBankHoliday;
        public bool isBankHoliday
        {
            get { return _isBankHoliday; }
            set 
            {
                _isBankHoliday = value; 
                 if(isBankHoliday)
                   BankProp = Visibility.Visible;
                
            }
        }

        private bool _isMercantileHoliday;
        public bool isMercantileHoliday
        {
            get { return _isMercantileHoliday; }
            set
            {
                _isMercantileHoliday = value;

                    if (isMercantileHoliday)
                        MercantileProp = Visibility.Visible;
                
            }
        }

        private bool _isPoyaHoliday;
        public bool isPoyaHoliday
        {
            get { return _isPoyaHoliday; }
            set 
            { 
                _isPoyaHoliday = value;
                    if (isPoyaHoliday)
                        PoyaProp = Visibility.Visible;
                
            }
        }

        private string _OutLineColor;
        public string OutLineColor
        {
            get { return _OutLineColor; }
            set { _OutLineColor = value; }
        }

        private Visibility _PoyaProp;
        public Visibility PoyaProp
        {
            get { return _PoyaProp; }
            set { _PoyaProp = value; }
        }

        private Visibility _BankProp;
        public Visibility BankProp
        {
            get { return _BankProp; }
            set { _BankProp = value; }
        }

        private Visibility _MercantileProp;
        public Visibility MercantileProp
        {
            get { return _MercantileProp; }
            set { _MercantileProp = value; }
        }

        private Visibility _PublicProp;
        public Visibility PublicProp
        {
            get { return _PublicProp; }
            set { _PublicProp = value; }
        }      
      
    }
}
