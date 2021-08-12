using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKTClassLibrary.HelperClass
{
    public class DownloadDataType
    {
        private Guid _Attendance_data_id;
        public Guid Attendance_data_id
        {
            get { return _Attendance_data_id; }
            set { _Attendance_data_id = value; }
        }
        private int _InOutMode;
        public int InOutMode
        {
            get { return _InOutMode; }
            set { _InOutMode = value; }
        }
        private int _VeryfyMode;
        public int VeryfyMode
        {
            get { return _VeryfyMode; }
            set { _VeryfyMode = value; }
        }
        
        
        private Guid _Device_id;
        public Guid Device_id
        {
            get { return _Device_id; }
            set { _Device_id = value; }
        }
        private string _Emp_id;
        public string Emp_id
        {
            get { return _Emp_id; }
            set { _Emp_id = value; }
        }

        private string _Year;
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _Day;
        public string Day
        {
            get { return _Day; }
            set { _Day = value; }
        }
        private string _Month;
        public string Month
        {
            get { return _Month; }
            set { _Month = value; }
        }
        private string _Hour;
        public string Hour
        {
            get { return _Hour; }
            set { _Hour = value; }
        }
        private string _Minute;
        public string Minute
        {
            get { return _Minute; }
            set { _Minute = value; }
        }

        private string _Second;
        public string Second
        {
            get { return _Second; }
            set { _Second = value; }
        }
        
    }
}
