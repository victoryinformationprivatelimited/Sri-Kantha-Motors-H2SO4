using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.AttendanceService;
using ERP.ERPService;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Data;
using System.Windows;

namespace ERP.AttendanceModule.SMS
{
    class SMSAttendanceViewModel : ViewModelBase
    {
        #region Fields
        AttendanceServiceClient serviceClient;
        ERPServiceClient serviceClientERP;
        string[] smstexts;
        string currentsmstext = null;
        string DirectoryPath = null;
        List<SMS_IN> SelectedSMSData;
        List<ERP.ERPService.dtl_AttendanceData> ToSaveSMSData;
        int j = 2;
        Microsoft.Office.Interop.Excel.Application app;
        Microsoft.Office.Interop.Excel.Workbook wb;
        Microsoft.Office.Interop.Excel.Worksheet ws;
        string flag = "";
        #endregion

        #region Properties
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

        private IEnumerable<SMS_IN> _SMSData;

        public IEnumerable<SMS_IN> SMSData
        {
            get { return _SMSData; }
            set { _SMSData = value; OnPropertyChanged("SMSData"); }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
        }

        private NotifyingProperty cursor = new NotifyingProperty("Cursor", typeof(System.Windows.Input.Cursor), System.Windows.Input.Cursors.Arrow);
        public System.Windows.Input.Cursor Cursor
        {
            get { return (System.Windows.Input.Cursor)GetValue(cursor); }
            set { SetValue(cursor, value); }
        }
        #endregion

        #region Constructor
        public SMSAttendanceViewModel()
        {
            SelectedSMSData = new List<SMS_IN>();
            ToSaveSMSData = new List<ERP.ERPService.dtl_AttendanceData>();
            smstexts = new string[9];
            serviceClient = new AttendanceServiceClient();
            serviceClientERP = new ERPServiceClient();
            FromDate = System.DateTime.Now.Date;
            ToDate = System.DateTime.Now.Date;
        }
        #endregion

        #region Refresh Methods
        private void RefreshSMSData()
        {
            SMSData = serviceClient.GetSMSAttendance();
            SelectedSMSData = SMSData.Where(c => c.SENT_DT.Value.Date >= FromDate.Date && c.SENT_DT.Value.Date <= ToDate.Date).ToList();
        }
        #endregion

        #region Commands And Methods
        public ICommand GenerateBTN
        {
            get
            {
                return new RelayCommand(Generate);
            }
        }

        public ICommand BrowseBTN
        {
            get
            {
                return new RelayCommand(Browse);
            }
        }

        public ICommand UploadBTN
        {
            get { return new RelayCommand(Upload); }
        }

        private void Generate()
        {
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                wb = app.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                ws = wb.Worksheets[1];
                RefreshSMSData();
                serviceClient.UpdateSMSAttendance(SelectedSMSData.ToArray());
                if (SelectedSMSData != null && SelectedSMSData.Count() > 0)
                {
                    foreach (SMS_IN CurrentSMSData in SelectedSMSData)
                    {
                        currentsmstext = CurrentSMSData.SMS_TEXT.Replace(' ', ',');
                        currentsmstext = currentsmstext.Replace('-', ',');
                        smstexts = currentsmstext.Split(',');
                        if (smstexts.Length > 9)
                        {
                            try
                            {
                                if (smstexts[9].Equals("") && smstexts[10].Equals(""))
                                    smstexts = smstexts.Take(smstexts.Count() - (smstexts.Count() - 9)).ToArray();
                            }
                            catch (Exception)
                            {
                                GenerateExcel(CurrentSMSData);
                            }
                        }
                        if (smstexts.Length == 9)
                        {
                            InsertAttendance(CurrentSMSData);
                        }
                        else
                        {
                            GenerateExcel(CurrentSMSData);
                        }
                    }
                    if (flag.Equals("Excel"))
                    {
                        wb.SaveAs("C:\\H2SO4\\ExcelFiles\\" + FromDate.ToString("yyyMMdd") + ' ' + ToDate.ToString("yyyMMdd") + "\\" + "SMSData.xlsx");
                        Marshal.ReleaseComObject(app);
                        New();
                        clsMessages.setMessage("SMS Attendance Data Excel Generated Successfully");
                    }
                }
                else
                {
                    clsMessages.setMessage("No Data For The Selected Date Range");
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }

        void CreatSubFolder()
        {
            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        private void New()
        {
            j = 2;
            flag = "";
        }

        private void InsertAttendance(SMS_IN CurrentSMSData)
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    ERP.ERPService.dtl_AttendanceData new_rule = new ERP.ERPService.dtl_AttendanceData();
                    new_rule.attendance_data_id = Guid.NewGuid();
                    new_rule.device_id = new Guid("00000000-0000-0000-0000-000000000001");
                    new_rule.emp_id = smstexts[0].TrimStart('0');
                    if (i == 0)
                    {
                        new_rule.year = Convert.ToInt32(smstexts[3]);
                        new_rule.day = Convert.ToInt32(smstexts[1]);
                        new_rule.month = Convert.ToInt32(smstexts[2]);
                        new_rule.hour = Convert.ToInt32(smstexts[4].Substring(0, 2));
                        new_rule.minute = Convert.ToInt32(smstexts[4].Substring(3));
                        new_rule.second = 00;
                        new_rule.attend_datetime = Convert.ToDateTime(smstexts[3] + '-' + smstexts[2] + '-' + smstexts[1] + ' ' + smstexts[4].Substring(0, 2) + ':' + smstexts[4].Substring(3) + ':' + "00");
                        new_rule.attend_date = Convert.ToDateTime(smstexts[3] + '-' + smstexts[2] + '-' + smstexts[1]);
                    }
                    else
                    {
                        new_rule.year = Convert.ToInt32(smstexts[7]);
                        new_rule.day = Convert.ToInt32(smstexts[5]);
                        new_rule.month = Convert.ToInt32(smstexts[6]);
                        new_rule.hour = Convert.ToInt32(smstexts[8].Substring(0, 2));
                        new_rule.minute = Convert.ToInt32(smstexts[8].Substring(3));
                        new_rule.second = 00;
                        new_rule.attend_datetime = Convert.ToDateTime(smstexts[7] + '-' + smstexts[6] + '-' + smstexts[5] + ' ' + smstexts[8].Substring(0, 2) + ':' + smstexts[8].Substring(3) + ':' + "00");
                        new_rule.attend_date = Convert.ToDateTime(smstexts[7] + '-' + smstexts[6] + '-' + smstexts[5]);

                    }
                    //datetime = (smstexts[3] + '-' + smstexts[2] + '-' + smstexts[1] + ' ' + smstexts[4].Substring(0, 2) + ':' + smstexts[4].Substring(3) + ':' + "00").ToString();
                    //new_rule.attend_datetime = Convert.ToDateTime(smstexts[3] + '-' + smstexts[2] + '-' + smstexts[1] + ' ' + smstexts[4].Substring(0, 2) + ':' + smstexts[4].Substring(3) + ':' + "00");
                    new_rule.attend_time = new TimeSpan(new_rule.hour, new_rule.minute, new_rule.second);
                    new_rule.mode_id = new Guid("00000000-0000-0000-0000-000000000000");
                    new_rule.verify_id = new Guid("00000000-0000-0000-0000-000000000000");
                    new_rule.save_user_id = clsSecurity.loggedUser.user_id;
                    new_rule.save_datetime = System.DateTime.Now;
                    new_rule.isdelete = false;
                    new_rule.is_manual = false;
                    ToSaveSMSData.Add(new_rule);
                }
                serviceClientERP.SaveManualAttendanceUpload(ToSaveSMSData.ToArray());
                ToSaveSMSData = null;
                ToSaveSMSData = new List<ERPService.dtl_AttendanceData>();
            }
            catch (Exception)
            {
                GenerateExcel(CurrentSMSData);
            }
        }

        private void GenerateExcel(SMS_IN CurrentSMSData)
        {
            flag = "Excel";
            DirectoryPath = @"C:\\H2SO4\\ExcelFiles\\" + FromDate.ToString("yyyMMdd") + ' ' + ToDate.ToString("yyyMMdd") + "\\";
            CreatSubFolder();
            ws.Range["A1"].Value = "SMS Text";
            ws.Range["B1"].Value = "Phone Number";
            ws.Range["C1"].Value = "Date";

            ws.Range["A" + j].Value = CurrentSMSData.SMS_TEXT;
            ws.Range["B" + j].Value = CurrentSMSData.SENDER_NUMBER;
            ws.Range["C" + j].Value = CurrentSMSData.SENT_DT;
            j++;
        }

        private void Browse()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".csv";

            Nullable<bool> result = fd.ShowDialog();

            if (result == true)
            {
                Path = fd.FileName;
            }

        }

        private DataTable ReadExcelFile()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties=Excel 12.0;";
            string query = @"Select * From [Sheet1$]";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connStr);

            conn.Open();
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(query, conn);
            System.Data.OleDb.OleDbDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            conn.Close();

            return dt;

        }

        private void Upload()
        {
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                wb = app.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                ws = wb.Worksheets[1];
                Cursor = System.Windows.Input.Cursors.Wait;
                DataTable dt = ReadExcelFile();

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        SMS_IN CurrentSMSData = new SMS_IN(); ;
                        currentsmstext = dr["SMS Text"].ToString().Replace(' ', ',');
                        currentsmstext = currentsmstext.Replace('-', ',');
                        smstexts = currentsmstext.Split(',');
                        if (smstexts.Length > 9)
                        {
                            if (smstexts[9].Equals("") && smstexts[10].Equals(""))
                                smstexts = smstexts.Take(smstexts.Count() - (smstexts.Count() - 9)).ToArray();
                        }
                        CurrentSMSData.SMS_TEXT = dr["SMS Text"].ToString();
                        CurrentSMSData.SENDER_NUMBER = dr["Phone Number"].ToString();
                        CurrentSMSData.SENT_DT = Convert.ToDateTime(dr["Date"]);
                        if (smstexts.Length == 9)
                        {
                            InsertAttendance(CurrentSMSData);
                        }
                        else
                        {
                            GenerateExcel(CurrentSMSData);
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
                if (flag.Equals("Excel"))
                {
                    wb.SaveAs("C:\\H2SO4\\ExcelFiles\\" + FromDate.ToString("yyyMMdd") + ' ' + ToDate.ToString("yyyMMdd") + "\\" + "SMSData.xlsx");
                    Marshal.ReleaseComObject(app);
                    New();
                    clsMessages.setMessage("There Were Some Invalid SMS In The Uploaded Excel. Invalids Were Replaced On The Same Excel");
                }
                else
                    clsMessages.setMessage("Attendance Data Uploaded Successfully.");

            }
            catch (Exception)
            {
                clsMessages.setMessage("Error In Attendance Data Uploading.");
            }
        }
        #endregion
    }
}
