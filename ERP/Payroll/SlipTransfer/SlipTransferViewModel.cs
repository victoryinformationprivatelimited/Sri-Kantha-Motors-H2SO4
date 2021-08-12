using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using ERP.Reports;
using System.Configuration;
using System.Collections;

namespace ERP.Payroll.SlipTransfer
{
    class SlipTransferViewModel : ViewModelBase
    {
        #region Service Client

        ERPServiceClient serviceClient;

        #endregion

        #region selected list
        List<SlipTransferView> EmployeeList = new List<SlipTransferView>();
        List<SlipTransferView> listSelected = new List<SlipTransferView>();
        // h 2020-09-08
        IList selectedSlipTransferGenerate = new ArrayList();
        IList selectedSlipTransferRemove = new ArrayList();
        List<SlipTransferView> tempToAddEmps;
        List<SlipTransferView> tempToGenerateEmps;
        #endregion

        #region Constructor

        public SlipTransferViewModel()
        {
            isEnable = false;
            //if (ConfigurationManager.AppSettings["SlipEnable"] == "True")
            //    isEnable = true;

            serviceClient = new ERPServiceClient();
            RefreshBankName();
            RefreshCompanyBranches();
            SelectDate = DateTime.Now.Date;
        }

        void New()
        {
            EmployeeList.Clear();
            listSelected.Clear();
            serviceClient = new ERPServiceClient();
            RefreshBankName();
            RefreshCompanyBranches();
            RefreshPayPeriod();
            SelectDate = DateTime.Now.Date;
            SlipTransfer = null;
            SelectedSlip = null;

        }

        #endregion

        #region Properties

        private bool _isEnable;

        public bool isEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; OnPropertyChanged("isEnable"); }
        }


        #region All Selected

        private bool _AllSelect;
        public bool AllSelect
        {
            get { return _AllSelect; }
            set { _AllSelect = value; OnPropertyChanged("AllSelect"); }
        }

        #endregion

        #region Date
        private DateTime _SelectDate;
        public DateTime SelectDate
        {
            get { return _SelectDate; }
            set { _SelectDate = value; OnPropertyChanged("SelectDate"); }
        }
        #endregion

        #region Progress Bar
        private string _Color;

        public string Color
        {
            get { return _Color; }
            set { _Color = value; OnPropertyChanged("Color"); }
        }


        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged("Progress"); }
        }
        #endregion

        #region Pay Period
        private z_Period _CurrentPayperiod;
        public z_Period CurrentPayperiod
        {
            get { return _CurrentPayperiod; }
            set { _CurrentPayperiod = value; OnPropertyChanged("CurrentPayperiod"); if (CurrentPayperiod != null && isEnable == true) this.RefreshSlipTransfer(); }
        }

        private IEnumerable<z_Period> _Payperiod;
        public IEnumerable<z_Period> Payperiod
        {
            get { return _Payperiod; }
            set { _Payperiod = value; OnPropertyChanged("Payperiod"); }
        }

        #endregion

        #region Bank
        private IEnumerable<z_Bank> _BankName;
        public IEnumerable<z_Bank> BankName
        {
            get { return _BankName; }
            set { _BankName = value; OnPropertyChanged("BankName"); }
        }

        private z_Bank _CurrentBankName;
        public z_Bank CurrentBankName
        {
            get { return _CurrentBankName; }
            set
            {
                _CurrentBankName = value; OnPropertyChanged("CurrentBankName");

                if (CurrentBankName != null)
                {
                    FilterEmployees();
                }
            }


        }

        void FilterEmployees()
        {
            SlipTransfer = null;
            SlipTransfer = EmployeeList.Where(i => i.bank_id == CurrentBankName.bank_id);
        }
        #endregion

        #region Company Branches
        private IEnumerable<z_CompanyBranches> _CompanyBranches;
        public IEnumerable<z_CompanyBranches> CompanyBranches
        {
            get { return _CompanyBranches; }
            set { _CompanyBranches = value; OnPropertyChanged("CompanyBranches"); }
        }

        private z_CompanyBranches _CurrentCompanyBranches;
        public z_CompanyBranches CurrentCompanyBranches
        {
            get { return _CurrentCompanyBranches; }
            set { _CurrentCompanyBranches = value; OnPropertyChanged("CurrentCompanyBranches"); if (CurrentCompanyBranches != null) RefreshPayPeriod(); }
        }


        #endregion

        #region Filter
        private IEnumerable<SlipTransferView> _Filter;
        public IEnumerable<SlipTransferView> Filter
        {
            get { return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); }
        }
        #endregion

        #region datagrid 1

        private IEnumerable<SlipTransferView> _SlipTransfer;
        public IEnumerable<SlipTransferView> SlipTransfer
        {
            get { return _SlipTransfer; }

            set
            {
                _SlipTransfer = value; OnPropertyChanged("SlipTransfer");
            }
        }

        // h 2020-09-08
        //private SlipTransferView _CurrentSlipTransfer;
        //public SlipTransferView CurrentSlipTransfer
        //{
        //    get { return _CurrentSlipTransfer; }

        //    set
        //    {
        //        _CurrentSlipTransfer = value; OnPropertyChanged("CurrentSlipTransfer");
        //    }
        //}

        public IList SelectedSlipTransferGenerate
        {
            get { return selectedSlipTransferGenerate; }
            set { selectedSlipTransferGenerate = value; OnPropertyChanged("SelectedSlipTransferGenerate"); }
        }

        #endregion

        #region datagrid 2


        private IEnumerable<SlipTransferView> _SelectedSlip;
        public IEnumerable<SlipTransferView> SelectedSlip
        {
            get { return _SelectedSlip; }
            set { _SelectedSlip = value; OnPropertyChanged("SelectedSlip"); }
        }

        // h 2020-09-08
        //private SlipTransferView _CurrentSelectedSlip;
        //public SlipTransferView CurrentSelectedSlip
        //{
        //    get { return _CurrentSelectedSlip; }
        //    set { _CurrentSelectedSlip = value; OnPropertyChanged("CurrentSelectedSlip"); }
        //}

        public IList SelectedSlipTransferRemove
        {
            get { return selectedSlipTransferRemove; }
            set { selectedSlipTransferRemove = value; OnPropertyChanged("SelectedSlipTransferRemove"); }
        }

        #endregion

        // h 2020-08-13 mail
        private bool _notif_salarydeposited;

        public bool notif_salarydeposited
        {
            get { return _notif_salarydeposited; }
            set { _notif_salarydeposited = value; OnPropertyChanged("notif_salarydeposited"); }
        }

        private bool _notif_salaryslip;

        public bool notif_salaryslip
        {
            get { return _notif_salaryslip; }
            set { _notif_salaryslip = value; OnPropertyChanged("notif_salaryslip"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshBankName()
        {
            try
            {
                serviceClient.GetBanksCompleted += (s, e) =>
                {
                    this.BankName = e.Result.Where(z => z.isdelete == false);
                };
                this.serviceClient.GetBanksAsync();
            }
            catch (Exception)
            {

                throw null;
            }
        }

        void RefreshSlipTransfer()
        {

            try
            {

                if (CurrentCompanyBranches != null && CurrentPayperiod != null)
                {

                    serviceClient.GetSlipTransferViewCompleted += (s, e) =>
                    {
                        Filter = e.Result.OrderBy(c => c.emp_id);
                        if (Filter != null)
                        {
                            SlipTransfer = Filter;
                            EmployeeList = Filter.ToList();
                        }
                    };
                    serviceClient.GetSlipTransferViewAsync(CurrentPayperiod.period_id, CurrentCompanyBranches.companyBranch_id);
                }
                else
                {
                    clsMessages.setMessage("you have to select a pay period");
                }

            }
            catch (Exception)
            {
            }
        }

        void RefreshCompanyBranches()
        {
            try
            {
                serviceClient.GetCompanyBranchesCompleted += (s, e) =>
                {
                    this.CompanyBranches = e.Result.Where(c => c.isdelete == false);//.Where(c => c.companyBranch_id == new Guid("FD18CFB6-EC4C-4098-B255-29A4C1AA8BF0"));
                };
                serviceClient.GetCompanyBranchesAsync();
            }
            catch (Exception)
            {

                throw null;
            }
        }

        private void RefreshPayPeriod()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                {
                    this.Payperiod = e.Result;
                };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {

                throw null;
            }
        }

        #endregion

        #region Add Button

        // h 2020-09-08
        //private void Add()
        //{
        //    try
        //    {
        //        if (CurrentBankName != null && CurrentSlipTransfer != null)
        //        {
        //            EmployeeList.Remove(CurrentSlipTransfer);
        //            listSelected.Add(CurrentSlipTransfer);
        //            AllSetToDefault();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //void AddAll()
        //{
        //    try
        //    {
        //        if (EmployeeList != null)
        //        {
        //            listSelected.AddRange(SlipTransfer.ToList());
        //            foreach (var item in SlipTransfer.ToList())
        //            {
        //                EmployeeList.Remove(item);
        //            }
        //            AllSetToDefault();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //public ICommand AddAllButton
        //{
        //    get { return new RelayCommand(AddAll); }
        //}

        public ICommand AddButton
        {
            get { return new RelayCommand(Add, AddCE); }
        }

        private bool AddCE()
        {
            if (SelectedSlipTransferGenerate == null || SelectedSlipTransferGenerate.Count <= 0)
                return false;
            return true;
        }

        private void Add()
        {
            tempToAddEmps = new List<SlipTransferView>();
            tempToGenerateEmps = new List<SlipTransferView>();
            if(SlipTransfer != null)
            {
                tempToAddEmps = SlipTransfer.ToList();
            }
            if(SelectedSlip != null)
            {
                tempToGenerateEmps = SelectedSlip.ToList();
            }
            foreach(SlipTransferView emp in SelectedSlipTransferGenerate)
            {
                tempToGenerateEmps.Add(emp);
                tempToAddEmps.Remove(emp);
            }
            SlipTransfer = null;
            SelectedSlip = null;
            SlipTransfer = tempToAddEmps.OrderBy(c => c.emp_id);
            SelectedSlip = tempToGenerateEmps.OrderBy(c => c.emp_id);
        }

        #endregion

        #region ClearButton

        public ICommand ClearButton
        {
            get { return new RelayCommand(Clear); }
        }

        void Clear()
        {
            New();
        }

        #endregion

        #region Remove Buttons

        // h 2020-09-08
        //void Remove()
        //{
        //    try
        //    {
        //        if (CurrentBankName != null && CurrentSelectedSlip != null)
        //        {
        //            if (CurrentSelectedSlip.bank_id == CurrentBankName.bank_id)
        //            {
        //                listSelected.Remove(CurrentSelectedSlip);
        //                EmployeeList.Add(CurrentSelectedSlip);
        //                AllSetToDefault();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //void RemoveAll()
        //{
        //    try
        //    {
        //        if (CurrentBankName != null)
        //        {

        //            EmployeeList.AddRange(listSelected.Where(i => i.bank_id == CurrentBankName.bank_id));
        //            int Count = listSelected.Count();
        //            for (int i = 0; i < Count; i++)
        //            {
        //                if (listSelected == null)
        //                    break;
        //                if (listSelected.Where(c => c.bank_id == CurrentBankName.bank_id).Count() == 0)
        //                    break;
        //                listSelected.Remove(listSelected.Where(c => c.bank_id == CurrentBankName.bank_id).FirstOrDefault());
        //            }
        //            AllSetToDefault();
        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}

        //public ICommand RemoveAllButton
        //{
        //    get { return new RelayCommand(RemoveAll); }
        //}

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove, RemoveCE); }
        }

        private bool RemoveCE()
        {
            if (SelectedSlipTransferRemove == null || SelectedSlipTransferRemove.Count <= 0)
                return false;
            return true;
        }

        private void Remove()
        {
            tempToAddEmps = new List<SlipTransferView>();
            tempToGenerateEmps = new List<SlipTransferView>();
            if (SlipTransfer != null)
            {
                tempToAddEmps = SlipTransfer.ToList();
            }
            if (SelectedSlip != null)
            {
                tempToGenerateEmps = SelectedSlip.ToList();
            }
            foreach (SlipTransferView emp in SelectedSlipTransferRemove)
            {
                tempToGenerateEmps.Remove(emp);
                tempToAddEmps.Add(emp);
            }
            SlipTransfer = null;
            SelectedSlip = null;
            SlipTransfer = tempToAddEmps.OrderBy(c => c.emp_id);
            SelectedSlip = tempToGenerateEmps.OrderBy(c => c.emp_id);
        }

        #endregion

        #region List Refresh
        void ListRefresh()
        {
            EmployeeList.Clear();
            SlipTransferToDefault();
        }

        void AllSetToDefault()
        {
            SlipTransfer = null;
            SelectedSlip = null;
            SlipTransfer = EmployeeList.Where(c => c.bank_id == CurrentBankName.bank_id);
            SelectedSlip = listSelected;
        }
        void SlipTransferToDefault()
        {
            SlipTransfer = null;
            SlipTransfer = EmployeeList;
        }

        #endregion

        #region GenerateButton
        public ICommand GenerateButton
        {
            get { return new RelayCommand(Genarate); }
        }
        void Genarate()
        {
            z_Bank Bank = new z_Bank();
            Bank = serviceClient.GetBanks().Where(i => i.bank_id == CurrentCompanyBranches.bank_id).FirstOrDefault();

            if (isEnable)
            {
                try
                {
                    Progress = 0;
                    DispatcherTimer dis = new DispatcherTimer();

                    int BANKCODE = 0;
                    int BANKBRANCH = 0;
                    string ACCOUNTNO = "";
                    string COMPANYBRANCHACCOUNTNAME = "";
                    string PAYPERIOD = "";
                    string COMPANYBRANCH = "";

                    string COMPANYBRANCHID = "";
                    string PERIODID = "";

                    //__________________________________________________________________________________________________________


                    //____________________________________________________________________________________________________________________
                    Color = "#FF0E44A2";
                    dis.Tick += (s, e) =>
                    {
                        Progress++;
                        if (Progress == 50)
                        {
                            dis.Stop();

                            BANKCODE = Convert.ToInt16(Bank.bank_code);
                            BANKBRANCH = Convert.ToInt16(serviceClient.GetBanckBranch().Where(i => i.branch_id == CurrentCompanyBranches.bank_branch_id).FirstOrDefault().bank_branch_code);
                            ACCOUNTNO = CurrentCompanyBranches.account_no.ToString();
                            COMPANYBRANCHACCOUNTNAME = CurrentCompanyBranches.account_name;
                            PAYPERIOD = CurrentPayperiod.period_name;
                            COMPANYBRANCH = CurrentCompanyBranches.companyBranch_Name;
                            COMPANYBRANCHID = CurrentCompanyBranches.companyBranch_id.ToString();
                            PERIODID = CurrentPayperiod.period_id.ToString();


                            dis.Start();
                        }

                        if (Progress == 90)
                        {

                            dis.Stop();
                            // h 2020-09-08
                            //if (BANKCODE >= 0 && BANKBRANCH >= 0 && listSelected.Count() > 0)
                            if (BANKCODE >= 0 && BANKBRANCH >= 0 && SelectedSlip.Count() > 0)
                            {
                                //BankSelector select = new BankSelector(Bank.bank_name, listSelected, SelectDate, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, PAYPERIOD, COMPANYBRANCH, COMPANYBRANCHID, PERIODID);
                                BankSelector select = new BankSelector(Bank.bank_name, SelectedSlip.ToList(), SelectDate, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, PAYPERIOD, COMPANYBRANCH, COMPANYBRANCHID, PERIODID);


                                if (select.CheckPath())
                                {
                                    // h 2020-08-13 mail
                                    if (notif_salarydeposited || notif_salaryslip)
                                    {
                                        //if (serviceClient.notif_setEmployeeSalaryDepositDetail(listSelected.Select(c => (Guid)c.employee_id).ToArray(), SelectDate, notif_salaryslip, notif_salarydeposited))
                                        if (serviceClient.notif_setEmployeeSalaryDepositDetail(SelectedSlip.Select(c => (Guid)c.employee_id).ToArray(), SelectDate, notif_salaryslip, notif_salarydeposited))
                                        {
                                            MessageBox.Show("Notification generated successfully.");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Notification generate error.");
                                        }
                                    }
                                    New();
                                }

                            }

                            else
                            {
                                Color = "#FFFF0808";
                                MessageBox.Show("There are no records related to this details");
                            }
                            dis.Start();

                        }

                        if (Progress == 100)
                        {
                            dis.Stop();
                        }
                    };
                    dis.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    dis.Start();
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
            }
            else
            {
                if (Bank.bank_code == "7162")
                {
                    string path1 = "\\Reports\\Documents\\PayrollReport\\SlipTransferReport-NTB";
                    ReportPrint print1 = new ReportPrint(path1);
                    print1.setParameterValue("@Branch_id", CurrentCompanyBranches.companyBranch_id.ToString());
                    print1.setParameterValue("@PayPeriod_id", CurrentPayperiod.period_id.ToString());
                    print1.setParameterValue("Paydate", SelectDate.Date);
                    print1.LoadToReportViewer();

                    string path2 = "\\Reports\\Documents\\PayrollReport\\SlipTransferReport-Letter-NTB";
                    ReportPrint print2 = new ReportPrint(path2);
                    print2.setParameterValue("@Branch_id", CurrentCompanyBranches.companyBranch_id.ToString());
                    print2.setParameterValue("@PayPeriod_id", CurrentPayperiod.period_id.ToString());
                    print2.setParameterValue("Paydate", SelectDate.Date);
                    print2.LoadToReportViewer();

                    //string path = "\\Reports\\Documents\\PayrollReport\\SlipTransferReport-NTB";
                    //ReportPrint print = new ReportPrint(path);
                    //print.setParameterValue("@Branch_id", CurrentCompanyBranches.companyBranch_id.ToString());
                    //print.setParameterValue("@PayPeriod_id", CurrentPayperiod.period_id.ToString());
                    //print.setParameterValue("Paydate", SelectDate.Date);
                    //print.LoadToReportViewer();

                }
                else if (Bank.bank_code == "7162")
                {


                }
                else
                {
                    string path1 = "\\Reports\\Documents\\PayrollReport\\SlipTransferReport";
                    ReportPrint print1 = new ReportPrint(path1);
                    print1.setParameterValue("@Branch_id", CurrentCompanyBranches.companyBranch_id.ToString());
                    print1.setParameterValue("@PayPeriod_id", CurrentPayperiod.period_id.ToString());
                    print1.setParameterValue("Paydate", SelectDate.Date);
                    print1.LoadToReportViewer();

                    //string path2 = "\\Reports\\Documents\\PayrollReport\\SlipTransferReport-Letter";
                    //ReportPrint print2 = new ReportPrint(path2);
                    //print2.setParameterValue("@Branch_id", CurrentCompanyBranches.companyBranch_id.ToString());
                    //print2.setParameterValue("@PayPeriod_id", CurrentPayperiod.period_id.ToString());
                    //print2.setParameterValue("Paydate", SelectDate.Date);
                    //print2.LoadToReportViewer();

                }
            }
        }

        #endregion


    }

}
