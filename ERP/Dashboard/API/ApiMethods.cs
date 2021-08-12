using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace ERP.Dashboard.API
{
    static class ApiMethods
    {

        #region Properties

        public static JArray upComingBirthdayList;
        public static JArray newJoineeList;
        public static JArray permanentList;
        public static JArray resignList;
        public static JArray companyGrowth;
        public static JArray employeesDepartmentWiseList;
        public static JArray employeesDesignationtWiseList;

        #endregion

        #region Methods

        public static async Task<JArray> getupComingBirthdayListAsync()
        {
            string j = null;
            DateTime startDate;
            DateTime endDate;

            startDate = DateTime.Now;
            endDate = DateTime.Now.AddDays(31);
            string startDateText = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
            string endDateText = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;

            HttpClient client = new HttpClient();
            string path = "http://localhost:50523/Api/ERP/birthdays/" + startDateText + "/" + endDateText;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                j = await response.Content.ReadAsStringAsync();
            }
            upComingBirthdayList = JArray.Parse(j);
            return upComingBirthdayList;
        }

        public static async Task<JArray> getNewJoineeListAsync()
        {

            string j = null;

            HttpClient client = new HttpClient();
            string path = "http://localhost:50523/Api/ERP/newJoinees";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                j = await response.Content.ReadAsStringAsync();
            }
            newJoineeList = JArray.Parse(j);

            return newJoineeList;
        }

        public static async Task<JArray> getPermanentListAsync()
        {

            string j = null;

            HttpClient client = new HttpClient();
            string path = "http://localhost:50523/Api/ERP/permanentList";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                j = await response.Content.ReadAsStringAsync();
            }
            permanentList = JArray.Parse(j);

            return permanentList;
        }

        public static async Task<JArray> getResignListAsync()
        {
            string j = null;

            HttpClient client = new HttpClient();
            string path = "http://localhost:50523/Api/ERP/resignList";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                j = await response.Content.ReadAsStringAsync();
            }
            resignList = JArray.Parse(j);

            return resignList;
        }

        public static async Task<JArray> getCompanyGrowthDetailsAsync()
            {

                string j = null;

                HttpClient client = new HttpClient();
                string path = "http://localhost:50523/Api/ERP/employeesCountByYear";
                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    j = await response.Content.ReadAsStringAsync();
                }
                companyGrowth = JArray.Parse(j);

                return companyGrowth;

            }

        public static async Task<JArray> getEmployeesDepartmentWiseAsync()
        {

            string j = null;

            HttpClient client = new HttpClient();
            string path = "http://localhost:50523/Api/ERP/employeesByDept";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                j = await response.Content.ReadAsStringAsync();
            }
            employeesDepartmentWiseList = JArray.Parse(j);

            return employeesDepartmentWiseList;
        }

        public static async Task<JArray> getEmployeesDesignationWiseAsync()
        {

            string j = null;

            HttpClient client = new HttpClient();
            string path = "http://localhost:50523/Api/ERP/employeesByDesignation";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                j = await response.Content.ReadAsStringAsync();
            }
            employeesDepartmentWiseList = JArray.Parse(j);

            return employeesDepartmentWiseList;
        }

        #endregion Methods

    }
}
