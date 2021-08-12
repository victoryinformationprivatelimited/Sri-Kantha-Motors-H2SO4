using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ERP.AttendanceModule.AttendanceProcessMaster
{
    [DataContract]
    public class APIData
    {
        [DataMember(Name = "EPFNo")]
        public string EPFNo { get; set; }
        [DataMember(Name = "Date")]
        public DateTime Date { get; set; }
        [DataMember(Name = "Activity")]
        public string Activity { get; set; }
        [DataMember(Name = "Time")]
        public DateTime Time { get; set; }
    }
}
