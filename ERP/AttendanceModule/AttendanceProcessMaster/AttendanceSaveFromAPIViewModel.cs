using CustomBusyBox;
using ERP.AttendanceService;
using ERP.ERPService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceProcessMaster
{
    class AttendanceSaveFromAPIViewModel : ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor
        public AttendanceSaveFromAPIViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshAttendancePeriods();
            EndDate = DateTime.Now;
            DateTime ld = attendServiceClient.getLastDataDownloadDate().Value;
            if (ld != null)
            {
                LastDate = "Recent : " + ld.ToShortDateString().ToString();
                StartDate = ld.AddDays(1);
            }
        }

        #endregion

        IEnumerable<AttendancePeriodView> attendancePeriods;
        public IEnumerable<AttendancePeriodView> AttendancePeriods
        {
            get { if (attendancePeriods != null) attendancePeriods = attendancePeriods.OrderBy(c => c.start_date.Value.Date); return attendancePeriods; }
            set { attendancePeriods = value; OnPropertyChanged("AttendancePeriods"); }
        }

        private string lastDate;

        public string LastDate
        {
            get { return lastDate; }
            set { lastDate = value; OnPropertyChanged("LastDate"); }
        }


        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }



        AttendancePeriodView currentPeriod;
        public AttendancePeriodView CurrentPeriod
        {
            get { return currentPeriod; }
            set
            {
                currentPeriod = value; OnPropertyChanged("CurrentPeriod");
            }
        }

        List<ERP.AttendanceService.dtl_AttendanceData> downloadedUserData;
        public List<ERP.AttendanceService.dtl_AttendanceData> DownloadedUserData
        {
            get { return downloadedUserData; }
            set { downloadedUserData = value; OnPropertyChanged("DownloadedUserData"); }
        }

        void RefreshAttendancePeriods()
        {
            attendServiceClient.GetAttendancePeriodDetailsCompleted += (s, e) =>
            {
                try
                {
                    AttendancePeriods = e.Result.OrderBy(c => c.start_date);
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance period details refresh is failed");
                }
            };
            attendServiceClient.GetAttendancePeriodDetailsAsync();
        }

        #region Process Button

        void Process()
        {
            if (clsSecurity.GetSavePermission(310))
            {
                BusyBox.ShowBusy("Downloading Attendance Data");
                List<AttendanceEmployeeDataView> empList = new List<AttendanceEmployeeDataView>();
                List<AttendanceService.dtl_AttendanceData> ListAttData = new List<AttendanceService.dtl_AttendanceData>();

                // List<AttendanceData> TempAtt = new List<AttendanceData>();

                //--- Get data from API---
                List<ERPService.dtl_AttendanceData> ListAttendance = new List<ERPService.dtl_AttendanceData>();
                //--- Get data from API--- 
                List<string> apiLinkList = new List<string>();
                apiLinkList.Add(String.Format("http://gnext-sfa.evisionmicro.com/SFAAPI/GetAttendanceRecords?startDate=" + startDate.Date + "&endDate=" + endDate.Date + "&epfNo=All"));
                apiLinkList.Add(String.Format("http://173.249.17.202:7010/APIService/GetAttendanceRecords?startDate=" + startDate.Date + "&endDate=" + endDate.Date + "&epfNo=All"));
                HttpWebResponse responseObject = null;
                string strresulttest = null;
                List<APIData> ListApiData = new List<APIData>();
                foreach (string x in apiLinkList)
                {
                    WebRequest requestObject = WebRequest.Create(x);
                    requestObject.Method = "GET";
                    responseObject = (HttpWebResponse)requestObject.GetResponse();

                    using (Stream stream = responseObject.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream);
                        strresulttest = sr.ReadToEnd();
                        sr.Close();

                        // Parse JSON into dynamic object, convenient!
                        JObject results = JObject.Parse(strresulttest);
                        // Process each attendance
                        if (results.Count != 0)
                        {

                            foreach (var result in results["AttendanceRecords"])
                            {
                                if ((string)result["EPFNo"] != null)
                                {
                                    APIData TempApiData = new APIData();
                                    TempApiData.EPFNo = (string)result["EPFNo"];
                                    TempApiData.Date = (DateTime)result["Date"];
                                    TempApiData.Activity = (string)result["Activity"];
                                    TempApiData.Time = (DateTime)result["Time"];
                                    ListApiData.Add(TempApiData);

                                    AttendanceService.dtl_AttendanceData TempAttData = new AttendanceService.dtl_AttendanceData();
                                    TempAttData.attendance_data_id = Guid.NewGuid();
                                    //virtual device id
                                    TempAttData.device_id = new Guid("00000000-0000-0000-0000-000000000099");
                                    TempAttData.emp_id = TempApiData.EPFNo;
                                    TempAttData.year = TempApiData.Date.Year;
                                    TempAttData.day = TempApiData.Date.Day;
                                    TempAttData.month = TempApiData.Date.Month;
                                    TempAttData.hour = TempApiData.Time.Hour;
                                    TempAttData.minute = TempApiData.Time.Minute;
                                    TempAttData.second = TempApiData.Time.Second;
                                    TempAttData.attend_datetime = (DateTime)result["Time"];
                                    TempAttData.attend_date = (DateTime)result["Date"];
                                    TempAttData.attend_time = TempApiData.Time.TimeOfDay;
                                    if (TempApiData.Activity == "D_ON")
                                        TempAttData.mode_id = new Guid("00000000-0000-0000-0000-000000000000");
                                    else
                                        TempAttData.mode_id = new Guid("00000000-0000-0000-0000-000000000001");
                                    //default verify mode
                                    TempAttData.verify_id = new Guid("00000000-0000-0000-0000-000000000020");
                                    TempAttData.save_user_id = new Guid();
                                    TempAttData.save_datetime = DateTime.Now;
                                    ListAttData.Add(TempAttData);
                                }
                            }
                        }
                        else
                        {
                            BusyBox.CloseBusy();
                            MessageBox.Show("Data set is Empty for the selected period.");
                        }
                    }
                }
                DownloadedUserData = ListAttData;
                //---close---     
                try
                {
                    if (attendServiceClient.AddAttendanceFromAPI(ListAttData.ToArray()))
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show("Attendance downloaded successfully");
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show("Unable to upload all data to server");
                    }
                }
                catch (Exception)
                {
                    BusyBox.CloseBusy();
                    MessageBox.Show("Something went wrong");
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Save this record(s)");
            }
        }

        #endregion

        public ICommand ProcessButton
        {
            get { return new RelayCommand(Process); }
        }

    }
}
