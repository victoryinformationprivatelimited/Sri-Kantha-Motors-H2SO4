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
    class BonusOtherSendSMSViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        string DirectoryPath = @"C:\\H2SO4\\SMSLog\\";
        string fileName = "smslog.txt";
        #endregion

        #region Properties
        private IEnumerable<sendbonusotherpaymentsview> _EmployeeOtherPayments;

        public IEnumerable<sendbonusotherpaymentsview> EmployeeOtherPayments
        {
            get { return _EmployeeOtherPayments; }
            set { _EmployeeOtherPayments = value; OnPropertyChanged("EmployeeOtherPayments"); }
        }

        private IEnumerable<z_BonusPeriod> _Periods;

        public IEnumerable<z_BonusPeriod> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_BonusPeriod _CurrentPeriod;

        public z_BonusPeriod CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod.Bonus_Period_id != null) RefreshOtherPaymentsDetails(); }
        }

        #endregion

        #region Constructor
        public BonusOtherSendSMSViewModel()
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
                serviceClient.GetBonusPeriodCompleted += (s, e) =>
                {
                    Periods = e.Result.OrderBy(c => c.Bonus_Period_Start_Date);
                };
                serviceClient.GetBonusPeriodAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Loading Failed");
            }
        }

        private void RefreshOtherPaymentsDetails()
        {
            try
            {
                serviceClient.GetEmployeeOtherPaymentsDetailsCompleted += (s, e) =>
                {
                    EmployeeOtherPayments = e.Result.OrderBy(c => c.emp_id);
                };
                serviceClient.GetEmployeeOtherPaymentsDetailsAsync(CurrentPeriod.Bonus_Period_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Other Payments Details Loading Failed");
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
                if (clsSecurity.GetSavePermission(110) && clsSecurity.GetUpdatePermission(110) && clsSecurity.GetDeletePermission(110))
                {
                    int c = 0;
                    List<string> transactionLog = new List<string>();
                    //string ACCOUNT_SID = "ACf0abcee60b997a34dd508de1602a6031";
                    //string AUTH_TOKEN = "0a53bf51d61634f9bfaab847ffd54ebb";
                    //TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);
                    //foreach (var CurrentEmployeeOtherPayment in EmployeeOtherPayments)
                    //{
                    //if (CurrentEmployeeOtherPayment.mobile_number1 != null && CurrentEmployeeOtherPayment.PayedBonusAmount != null)
                    //{
                    //MessageResource.Create(to: new PhoneNumber("+94" + CurrentEmployeeOtherPayment.mobile_number1.ToString()), from: new PhoneNumber("+12722000630"), body: "Your" + CurrentEmployeeOtherPayment.Bonus_Period_Name + " is " + CurrentEmployeeOtherPayment.PayedBonusAmount.ToString());
                    //WebRequest webRequest = WebRequest.Create("https://cpsolutions.dialog.lk/index.php/cbs/sms/send?destination=94" + CurrentEmployeeOtherPayment.mobile_number1.ToString() + "&q=15112544514495&message=Your" + CurrentEmployeeOtherPayment.Bonus_Period_Name + " is " + CurrentEmployeeOtherPayment.PayedBonusAmount.ToString());
                    //WebResponse webResp = webRequest.GetResponse();
                    //c = c + 1;
                    DirectoryPath = @"C:\\H2SO4\\SMSExcelFiles\\" + CurrentPeriod.Bonus_Period_Name + "\\";
                    CreatSubFolder();
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    Worksheet ws = wb.Worksheets[1];

                    int j = 1;
                    foreach (var CurrentEmployeeOtherPayment in EmployeeOtherPayments)
                    {
                        if (CurrentEmployeeOtherPayment.mobile_number1 != null && CurrentEmployeeOtherPayment.PayedBonusAmount != null)
                        {
                            ws.Range["A" + j].Value = CurrentEmployeeOtherPayment.mobile_number1.ToString();
                            ws.Range["B" + j].Value = "Rs." + CurrentEmployeeOtherPayment.PayedBonusAmount.ToString() + " worth " + CurrentEmployeeOtherPayment.Bonus_Period_Name + " can be collected from your account within next working day.";
                            j = j + 1;
                            c = c + 1;
                        }
                    }
                    wb.SaveAs("C:\\H2SO4\\SMSExcelFiles\\" + CurrentPeriod.Bonus_Period_Name + "\\SMSExcelFile.csv");
                    Marshal.ReleaseComObject(app);
                    //}
                    //}
                    DirectoryPath = @"C:\\H2SO4\\SMSLog\\";
                    CreatSubFolder();
                    transactionLog.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}", clsSecurity.loggedUser.user_name, "\t", System.DateTime.Now, "\t", c, "\t", "BonusOtherPayments " + CurrentPeriod.Bonus_Period_Name.ToString()) + Environment.NewLine);
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
