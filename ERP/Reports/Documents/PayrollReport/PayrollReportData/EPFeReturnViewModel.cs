using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Globalization;


namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class EPFeReturnViewModel : ViewModelBase
    {
        #region ServiceClient

        ERPServiceClient serviceClient;

        #endregion

        #region List

        GetEVEMPData_Result EVEMP = new GetEVEMPData_Result();
        List<Get_EPF_Data_Result> EVEMC = new List<Get_EPF_Data_Result>();
        #endregion

        string path = @"c:\EPFeReturn";
        string fullPath = "";
        string fileName1 = "EVEMP.txt";
        string fileName2 = "EVEMC.txt";
        DateTime Date;
        DateTime Paydate;

        #region Constructor

        public EPFeReturnViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshPayPeriod();
            RefreshEmployee();
            RefreshPayMethod();
            RefreshZoneCode();
            RefreshDistrictOfficeCode();
            RefershCompanyBranch();
            PayDate = System.DateTime.Now.Date;

        }

        #endregion

        #region Properties

        #region DataSubmissionNO

        private string _DataSubmision;

        public string DataSubmision
        {
            get { return _DataSubmision; }
            set { _DataSubmision = value; OnPropertyChanged("DataSubmision"); }
        }


        #endregion

        #region PaymentReference

        private string _PayRef;

        public string PayRef
        {
            get { return _PayRef; }
            set { _PayRef = value; OnPropertyChanged("PayRef"); }
        }


        #endregion

        #region Paymentdate

        private DateTime _PayDate;

        public DateTime PayDate
        {
            get { return _PayDate; }
            set { _PayDate = value; OnPropertyChanged("PayDate"); }
        }


        #endregion

        #region PayPeriod

        private IEnumerable<z_Period> _PayPeriod;
        public IEnumerable<z_Period> PayPeriod
        {
            get { return _PayPeriod; }
            set { _PayPeriod = value; OnPropertyChanged("PayPeriod"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        #endregion

        #region CompanyBranch

        private IEnumerable<z_CompanyBranches> _CompanyBranch;
        public IEnumerable<z_CompanyBranches> CompanyBranch
        {
            get { return _CompanyBranch; }
            set { _CompanyBranch = value; OnPropertyChanged("CompanyBranch"); }
        }

        private z_CompanyBranches _CurrentCompanyBranch;
        public z_CompanyBranches CurrentcompanyBranch
        {
            get { return _CurrentCompanyBranch; }
            set { _CurrentCompanyBranch = value; OnPropertyChanged("CurrentcompanyBranch"); }
        }



        #endregion

        #region Employee

        private IEnumerable<mas_Employee> _Employee;
        public IEnumerable<mas_Employee> Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }



        #endregion

        #region PayMethod

        private IEnumerable<z_Payment_Mode> _PaymentMode;
        public IEnumerable<z_Payment_Mode> PaymentMode
        {
            get { return _PaymentMode; }
            set { _PaymentMode = value; OnPropertyChanged("PaymentMode"); }
        }

        private z_Payment_Mode _CurrentPaymentMode;
        public z_Payment_Mode CurrentPaymentMode
        {
            get { return _CurrentPaymentMode; }
            set { _CurrentPaymentMode = value; OnPropertyChanged("CurrentPaymentMode"); }
        }



        #endregion

        #region Zonecode

        private IEnumerable<z_Zone_Code> _ZoneCode;
        public IEnumerable<z_Zone_Code> ZoneCode
        {
            get { return _ZoneCode; }
            set { _ZoneCode = value; OnPropertyChanged("ZoneCode"); }
        }

        private z_Zone_Code _CurrentZoneCode;
        public z_Zone_Code CurrentZoneCode
        {
            get { return _CurrentZoneCode; }
            set { _CurrentZoneCode = value; OnPropertyChanged("CurrentZoneCode"); }
        }


        #endregion

        #region DistrictOfficeCode

        private IEnumerable<z_District_Office_Code> _DistrictOfficeCode;
        public IEnumerable<z_District_Office_Code> DistrictOfficeCode
        {
            get { return _DistrictOfficeCode; }
            set { _DistrictOfficeCode = value; OnPropertyChanged("DistrictOfficeCode"); }
        }

        private z_District_Office_Code _CurrentDistrictCode;
        public z_District_Office_Code CurrentDistrictCode
        {
            get { return _CurrentDistrictCode; }
            set { _CurrentDistrictCode = value; OnPropertyChanged("CurrentDistrictCode"); }
        }

        #endregion

        #endregion

        #region RefreshMethods

        private async Task<IEnumerable<Get_EPF_Data_Result>> GetEpfData_Refresh()
        {
            Task<IEnumerable<Get_EPF_Data_Result>> asyncTask = Task<IEnumerable<Get_EPF_Data_Result>>.Factory.FromAsync(serviceClient.BeginGET_EPF_DataRows, serviceClient.EndGET_EPF_DataRows, CurrentPeriod.period_id.ToString(), CurrentcompanyBranch.companyBranch_id.ToString(), null);
            return await asyncTask;
        }

        void RefreshPayPeriod()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                this.PayPeriod = e.Result;
            };

            this.serviceClient.GetPeriodsAsync();
        }

        void RefreshEmployee()
        {
            serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employee = e.Result;
            };

            this.serviceClient.GetEmployeesAsync();
        }

        void RefreshPayMethod()
        {
            serviceClient.GetPaymentModeCompleted += (s, e) =>
            {
                this.PaymentMode = e.Result;
            };

            this.serviceClient.GetPaymentModeAsync();

        }

        void RefreshZoneCode()
        {
            serviceClient.GetZoneCodeCompleted += (s, e) =>
            {
                this.ZoneCode = e.Result;
            };

            this.serviceClient.GetZoneCodeAsync();
        }

        void RefreshDistrictOfficeCode()
        {
            serviceClient.GetDistrictOfficeCodeCompleted += (s, e) =>
            {
                this.DistrictOfficeCode = e.Result;
            };

            this.serviceClient.GetDistrictOfficeCodeAsync();
        }

        void RefershCompanyBranch()
        {
            serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                this.CompanyBranch = e.Result;
            };

            this.serviceClient.GetCompanyBranchesAsync();
        }

        #endregion

        #region GenerateButton

        public ICommand GenerateButton
        {
            get { return new RelayCommand(CheckPath); }
        }

        async void Genarate()
        {
            try
            {
                #region EVEMP Process

                string selectedStringEVEMP = "";
                CreatSubFolder();

                if (CurrentPeriod.period_id != null && CurrentcompanyBranch.epf_registstion_no != null)
                {

                    EVEMP = serviceClient.GetEVEMPDataRows(CurrentPeriod.period_id.ToString(), CurrentcompanyBranch.epf_registstion_no.ToString());

                    string ZoneCode = "";
                    string emplyrID = CurrentcompanyBranch.epf_registstion_no;
                    Date = Convert.ToDateTime(CurrentPeriod.end_date);
                    string SubmisionID = DataSubmision.ToString().PadLeft(2);
                    int MemCount = 0;
                    double TotContribution = 0;

                    if (EVEMP != null)
                    {
                        MemCount = Convert.ToInt32(EVEMP.TotalMembers);
                        TotContribution = Convert.ToInt32(EVEMP.TotalContribution);
                    }

                    string PayMode = CurrentPaymentMode.paymentMode_id.ToString();
                    string PayRefer = PayRef.ToString().PadRight(20);
                    Paydate = PayDate.Date;
                    string DOCode = CurrentDistrictCode.district_office_code.ToString();

                    if (CurrentZoneCode != null)
                    {
                        ZoneCode = CurrentZoneCode.zone_code.ToString();
                        selectedStringEVEMP = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", ZoneCode.ToString(), emplyrID.ToString().Trim().PadLeft(6), Date.ToString("yyyyMM"), SubmisionID.ToString().PadLeft(2), TotContribution.ToString("F", CultureInfo.InvariantCulture).PadLeft(11), MemCount.ToString().PadLeft(5), PayMode.ToString(), PayRefer.ToString(), PayDate.ToString("yyyyMMdd"), DOCode.ToString());
                    }
                    else
                    {
                        selectedStringEVEMP = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", emplyrID.Substring(0, 1).PadRight(1), emplyrID.Substring(1, emplyrID.TrimEnd(' ').Length - 1).PadLeft(6), Date.ToString("yyyyMM"), SubmisionID.ToString().PadLeft(2), TotContribution.ToString("F", CultureInfo.InvariantCulture).PadLeft(11), MemCount.ToString().PadLeft(5), PayMode.ToString(), PayRefer.ToString(), PayDate.ToString("yyyyMMdd"), DOCode.ToString());
                    }
                    if (selectedStringEVEMP != null)
                    {
                        File.Delete(this.fullPath + @"\" + this.fileName1);
                        File.WriteAllText(this.fullPath + @"\" + this.fileName1, selectedStringEVEMP);
                        MessageBox.Show("EVEMP Generated Successfully");
                    }

                }

                else
                {
                    MessageBox.Show("Check Company EPF Registration number");
                }

                #endregion

                #region EVEMC Process


                CreatSubFolder();
                List<string> selectedList = new List<string>();
                if (CurrentPeriod.period_id != null && CurrentcompanyBranch.companyBranch_id != null)
                {
                    try
                    {

                        EVEMC = (await GetEpfData_Refresh()).ToList();
                        if (EVEMC != null)
                        {
                            foreach (var item in EVEMC)
                            {
                                
                                string NIC = item.nic;
                                
                                string LastName = item.lastName;
                                string Initials = item.Initials;
                                if (item.Initials != null)
                                {
                                     Initials = item.Initials;
                                }
                                else
                                {
                                     Initials = "Na";
                                }
                                string MemNumber = item.EmpEPF_No;
                                double EmplyrCont = Convert.ToDouble(item.EmplyrContribution);
                                double EmpCont = Convert.ToDouble(item.EmpContribution);
                                double TotCont = (EmplyrCont + EmpCont);
                                double TotEarning = Convert.ToDouble(item.TotErns);
                                string MemStatus = item.MemStatus;
                                string ZoneCode = "";
                                string EplyrNumber = item.EplyrEPF_No;
                                Date = Convert.ToDateTime(CurrentPeriod.end_date);
                                string SubmisionID = DataSubmision.ToString().PadLeft(2);
                                double DaysWork = Convert.ToDouble(item.DaysWork);
                                int Grade = 11;
                                if (NIC != null)
                                {
                                    if (CurrentZoneCode != null) 
                                    {
                                        ZoneCode = CurrentZoneCode.zone_code.ToString();
                                        selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", NIC.PadRight(20), LastName.PadRight(40) == null ? "Na" : LastName.PadRight(40), Initials == null ? "Na" : Initials.PadRight(20), MemNumber.PadRight(6), TotCont.ToString("F", CultureInfo.InvariantCulture).PadLeft(11), EmplyrCont.ToString("F", CultureInfo.InvariantCulture).PadLeft(9), EmpCont.ToString("F", CultureInfo.InvariantCulture).PadLeft(9), TotEarning.ToString("F", CultureInfo.InvariantCulture).PadLeft(11), MemStatus == null ? "E" : MemStatus.PadLeft(1), ZoneCode.PadLeft(1), EplyrNumber.Substring(0, 1).PadRight(1), EplyrNumber.Substring(1, EplyrNumber.TrimEnd(' ').Length - 1).PadLeft(6), Date.ToString("yyyyMM"), SubmisionID.PadLeft(2), DaysWork.ToString("F", CultureInfo.InvariantCulture).PadLeft(4), Grade.ToString("00").PadLeft(3)) + Environment.NewLine);
                                    }
                                    else
                                    {
                                    selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}", NIC.PadRight(20), LastName.PadRight(40), Initials.PadRight(20), MemNumber.PadRight(6), TotCont.ToString("F", CultureInfo.InvariantCulture).PadLeft(11), EmplyrCont.ToString("F", CultureInfo.InvariantCulture).PadLeft(9), EmpCont.ToString("F", CultureInfo.InvariantCulture).PadLeft(9), TotEarning.ToString("F", CultureInfo.InvariantCulture).PadLeft(11), MemStatus.PadLeft(1), EplyrNumber.Substring(0, 1).PadRight(1), EplyrNumber.Substring(1, EplyrNumber.TrimEnd(' ').Length - 1).PadLeft(6), Date.ToString("yyyyMM"), SubmisionID.PadLeft(2), DaysWork.ToString("F", CultureInfo.InvariantCulture).PadLeft(4), Grade.ToString("00").PadLeft(3)) + Environment.NewLine);
                                    }
                                
                                }
                            }

                            if (selectedList != null && selectedList.Count != 0)
                            {
                                MessageBox.Show("EVEMC Generated Successfully");
                                File.Delete(this.fullPath + @"\" + this.fileName2);
                                foreach (string item in selectedList)
                                {
                                    File.AppendAllText(this.fullPath + @"\" + this.fileName2, item);
                                }

                            }
                            else

                                MessageBox.Show("EVEMC File is not Generated");

                        }
                    }
                    catch (Exception e)
                    {

                        MessageBox.Show(e.ToString());
                    }

                }



                #endregion

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion

        #region ClearButton


        public ICommand ClearButton
        {
            get { return new RelayCommand(Clear); }
        }

        void Clear()
        {
            serviceClient = new ERPServiceClient();
            RefreshPayPeriod();
            RefreshEmployee();
            RefreshPayMethod();
            RefreshZoneCode();
            RefreshDistrictOfficeCode();
            RefershCompanyBranch();
            DataSubmision = "";
            PayDate = System.DateTime.Now.Date;


        }

        #endregion

        #region SubFolder

        void CreatSubFolder()
        {
            try
            {
                fullPath = this.path + @"\EVEMP" + @"\" + this.CurrentPeriod.period_name + @"\" + this.CurrentcompanyBranch.companyBranch_Name;
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(Path.Combine(fullPath));
                }
            }
            catch (Exception)
            {

                throw null;
            }
        }

        #endregion

        #region CheckFilePath

        void CheckPath()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    this.Genarate();
                }
                else
                {
                    this.Genarate();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());

            }

        }

        #endregion


    }
}
