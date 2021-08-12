using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.IO;
using System.Net;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using CustomBusyBox;

namespace ERP.Payroll.SendSMS
{
    class SendSMSViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        string DirectoryPath = @"C:\\H2SO4\\SMSLog\\";
        string fileName = "smslog.txt";
        #endregion

        #region Properties
        private IEnumerable<smssendview> _EmployeeSalary;

        public IEnumerable<smssendview> EmployeeSalary
        {
            get { return _EmployeeSalary; }
            set { _EmployeeSalary = value; OnPropertyChanged("EmployeeSalary"); }
        }

        private IEnumerable<z_Period> _Periods;

        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod.period_id != null) RefreshSalaryDetails(); }
        }

        #endregion

        #region Constructor
        public SendSMSViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshPeriods();
        }
        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                {
                    Periods = e.Result.OrderBy(c => c.start_date);
                };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Loading Failed");
            }
        }

        private void RefreshSalaryDetails()
        {
            try
            {
                serviceClient.GetEmployeeSalaryDetailsCompleted += (s, e) =>
                {
                    EmployeeSalary = e.Result.OrderBy(c => c.emp_id);
                };
                serviceClient.GetEmployeeSalaryDetailsAsync(CurrentPeriod.period_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Salary Details Loading Failed");
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand btnSend
        {
            get
            {
                return new RelayCommand(Send);
            }
        }

        private void Send()
        {
            try
            {
                BusyBox.ShowBusy("Please Wait Until SMS Excel Generate Completed...");
                if (clsSecurity.GetSavePermission(109) && clsSecurity.GetUpdatePermission(109) && clsSecurity.GetDeletePermission(109))
                {
                    int c = 0;
                    List<string> transactionLog = new List<string>();
                    //string ACCOUNT_SID = "ACf0abcee60b997a34dd508de1602a6031";
                    //string AUTH_TOKEN = "0a53bf51d61634f9bfaab847ffd54ebb";
                    //TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);
                    //foreach (var CurrentEmployeeSalary in EmployeeSalary)
                    //{
                    //if (CurrentEmployeeSalary.mobile_number1 != null && CurrentEmployeeSalary.total_salary != null)
                    //{
                    //MessageResource.Create(to: new PhoneNumber("+94" + CurrentEmployeeSalary.mobile_number1.ToString()), from: new PhoneNumber("+12722000630"), body: "Your Salary For The Month " + CurrentPeriod.start_date.Value.Month + " is " + CurrentEmployeeSalary.total_salary.ToString());
                    //WebRequest webRequest = WebRequest.Create("https://cpsolutions.dialog.lk/index.php/cbs/sms/send?destination=94" + CurrentEmployeeSalary.mobile_number1.ToString() + "&q=15112544514495&message=Test Message Your Salary For The Month " + CurrentPeriod.start_date.Value.Month.ToString() + " is " + CurrentEmployeeSalary.total_salary.ToString());
                    //WebResponse webResp = webRequest.GetResponse();
                    //c = c + 1;
                    DirectoryPath = @"C:\\H2SO4\\SMSExcelFiles\\" + CurrentPeriod.period_name + "\\";
                    CreatSubFolder();
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    Worksheet ws = wb.Worksheets[1];

                    int j = 1;
                    foreach (var CurrentEmployeeSalary in EmployeeSalary)
                    {
                        if (CurrentEmployeeSalary.mobile_number1 != null && CurrentEmployeeSalary.total_salary != null)
                        {
                            ws.Range["A" + j].Value = CurrentEmployeeSalary.mobile_number1.ToString();
                            ws.Range["B" + j].Value = "Rs." + Math.Round((decimal)CurrentEmployeeSalary.total_salary, 2).ToString()+ " worth salary for the month " + Convert.ToDateTime(CurrentPeriod.start_date).ToString("MMM") + " can be collected from your account within next working day.";
                            j = j + 1;
                            c = c + 1;
                        }
                    }
                    wb.SaveAs("C:\\H2SO4\\SMSExcelFiles\\" + CurrentPeriod.period_name + "\\SMSExcelFile.csv");
                    Marshal.ReleaseComObject(app);
                    //}
                    //}
                    DirectoryPath = @"C:\\H2SO4\\SMSLog\\";
                    CreatSubFolder();
                    transactionLog.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}", clsSecurity.loggedUser.user_name, "\t", System.DateTime.Now, "\t", c, "\t", "Salary " + CurrentPeriod.start_date.Value.Month.ToString() + "-" + CurrentPeriod.start_date.Value.Year.ToString()) + Environment.NewLine);
                    foreach (var item in transactionLog)
                    {
                        File.AppendAllText(DirectoryPath + @"\" + fileName, item);
                    }
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("SMS Excel Generated Successfully");
                }
                else
                {
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("You don't have permission to Generate SMS Excel");
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        void CreatSubFolder()
        {

            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }
        #endregion
    }
}
