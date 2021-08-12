using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Notification
{
    public  class clsNotificationList
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private String hedder;

        public String Hedder
        {
            get { return hedder; }
            set { hedder = value; }
        }

        private String employeeId;

        public String EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        private String employeeName;

        public String EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        private String applyeddate;

        public String Applyeddate
        {
            get { return applyeddate; }
            set { applyeddate = value; }
        }

        private String description;

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

   
        
    }
}
