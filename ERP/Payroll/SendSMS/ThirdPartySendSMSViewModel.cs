using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomBusyBox;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;

namespace ERP.Payroll.SendSMS
{
    class ThirdPartySendSMSViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        string DirectoryPath = @"C:\\H2SO4\\SMSLog\\";
        string fileName = "smslog.txt";
        #endregion

        #region Properties
        private IEnumerable<sendthirdpartypayment> _EmployeeThirdPartyPayment;

        public IEnumerable<sendthirdpartypayment> EmployeeThirdPartyPayment
        {
            get { return _EmployeeThirdPartyPayment; }
            set { _EmployeeThirdPartyPayment = value; OnPropertyChanged("EmployeeThirdPartyPayment"); }
        }

        private IEnumerable<z_EmployeeThirdPartyPayments> _Periods;

        public IEnumerable<z_EmployeeThirdPartyPayments> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_EmployeeThirdPartyPayments _CurrentPeriod;

        public z_EmployeeThirdPartyPayments CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod.category_id != null) RefreshThirdPartyPaymentsDetails(); }
        }

        private DateTime _SelectedDate;

        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged("SelectedDate"); }
        }
        
        #endregion

        #region Constructor
        public ThirdPartySendSMSViewModel()
        {
            serviceClient = new ERPServiceClient();
            SelectedDate = DateTime.Now.Date;
            RefreshPeriods();
        }
        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            try
            {
                serviceClient.GetThirdPartyCategoriesCompleted += (s, e) =>
                {
                    Periods = e.Result.OrderBy(c => c.category_id);
                };
                serviceClient.GetThirdPartyCategoriesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Third Party Payments Loading Failed");
            }
        }

        private void RefreshThirdPartyPaymentsDetails()
        {
            try
            {
                serviceClient.GetEmployeeThirdPartyPaymentsDetailsCompleted += (s, e) =>
                {
                    EmployeeThirdPartyPayment = e.Result.OrderBy(c => c.emp_id);
                };
                serviceClient.GetEmployeeThirdPartyPaymentsDetailsAsync(CurrentPeriod.category_id, SelectedDate);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Third Party Payments Details Loading Failed");
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
                if (clsSecurity.GetSavePermission(111) && clsSecurity.GetUpdatePermission(111) && clsSecurity.GetDeletePermission(111))
                {
                    int c = 0;
                    List<string> transactionLog = new List<string>();
                    //string ACCOUNT_SID = "ACf0abcee60b997a34dd508de1602a6031";
                    //string AUTH_TOKEN = "0a53bf51d61634f9bfaab847ffd54ebb";
                    //TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);
                    //foreach (var CurrentEmployeeThirdPartyPayment in EmployeeThirdPartyPayment)
                    //{
                    //if (CurrentEmployeeThirdPartyPayment.mobile_number1 != null && CurrentEmployeeThirdPartyPayment.amount != null)
                    //{
                    //MessageResource.Create(to: new PhoneNumber("+94" + CurrentEmployeeThirdPartyPayment.mobile_number1.ToString()), from: new PhoneNumber("+12722000630"), body: "Your" + CurrentEmployeeThirdPartyPayment.category_name + " is " + CurrentEmployeeThirdPartyPayment.amount.ToString());
                    //WebRequest webRequest = WebRequest.Create("https://cpsolutions.dialog.lk/index.php/cbs/sms/send?destination=94" + CurrentEmployeeThirdPartyPayment.mobile_number1.ToString() + "&q=15112544514495&message=Your" + CurrentEmployeeThirdPartyPayment.category_name + " is " + CurrentEmployeeThirdPartyPayment.amount.ToString());
                    //WebResponse webResp = webRequest.GetResponse();
                    //c = c + 1;
                    DirectoryPath = @"C:\\H2SO4\\SMSExcelFiles\\" + CurrentPeriod.category_name + "\\";
                    CreatSubFolder();
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    Worksheet ws = wb.Worksheets[1];

                    int j = 1;
                    foreach (var CurrentEmployeeThirdPartyPayment in EmployeeThirdPartyPayment)
                    {
                        if (CurrentEmployeeThirdPartyPayment.mobile_number1 != null && CurrentEmployeeThirdPartyPayment.amount != null)
                        {
                            ws.Range["A" + j].Value = CurrentEmployeeThirdPartyPayment.mobile_number1.ToString();
                            ws.Range["B" + j].Value = "Rs." + Math.Round((decimal)CurrentEmployeeThirdPartyPayment.amount, 2).ToString() + " worth " + CurrentEmployeeThirdPartyPayment.category_name + " can be collected from your account within next working day.";
                            j = j + 1;
                            c = c + 1;
                        }
                    }
                    wb.SaveAs("C:\\H2SO4\\SMSExcelFiles\\" + CurrentPeriod.category_name + "\\SMSExcelFile.csv");
                    Marshal.ReleaseComObject(app);
                    //}
                    //}
                    DirectoryPath = @"C:\\H2SO4\\SMSLog\\";
                    CreatSubFolder();
                    transactionLog.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}", clsSecurity.loggedUser.user_name, "\t", System.DateTime.Now, "\t", c, "\t", "BonusOtherPayments " + CurrentPeriod.category_name.ToString()) + Environment.NewLine);
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
