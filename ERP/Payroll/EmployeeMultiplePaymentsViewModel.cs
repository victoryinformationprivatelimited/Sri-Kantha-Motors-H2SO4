using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace ERP.Payroll
{
    class EmployeeMultiplePaymentsViewModel : ViewModelBase
    {
        #region Service Object
        //private ERPServiceClient serviceClient;
        #endregion

        #region Global List
      //  private List<EmployeeMulitiplePaymentView> TempEmployeeMulitiplePaymentViews = new List<EmployeeMulitiplePaymentView>();
        #endregion

        #region Constructor
        public EmployeeMultiplePaymentsViewModel()
        {
            //serviceClient = new ERPServiceClient();
            //RefreshMultiplePaymentsView();
            //RefreshPeriod();
            //RefereshEmployeeMultiplePayment();
        }
        #endregion

        #region Properties
        //private string _FullName;
        //public string FullName
        //{
        //    get { return _FullName; }
        //    set { _FullName = value; OnPropertyChanged("FullName"); }
        //}

        //private decimal? _TotalSalary;
        //public decimal? TotalSalary
        //{
        //    get { return _TotalSalary; }
        //    set { _TotalSalary = value; OnPropertyChanged("TotalSalary"); }
        //}

        //private decimal? _Balance;
        //public decimal? Balance
        //{
        //    get { return _Balance; }
        //    set { _Balance = value; OnPropertyChanged("Balance"); }
        //}

        //private IEnumerable<trns_EmployeePayment> _EmployeePayment;
        //public IEnumerable<trns_EmployeePayment> EmployeePayment
        //{
        //    get { return _EmployeePayment; }
        //    set { _EmployeePayment = value; OnPropertyChanged("EmployeePayment"); }
        //}

        //private z_Period _CurrentPeriod;
        //public z_Period CurrentPeriod
        //{
        //    get { return _CurrentPeriod; }
        //    set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); ReferesshEmployeePayments(); }
        //}

        //private IEnumerable<z_Period> _Periods;
        //public IEnumerable<z_Period> Periods
        //{
        //    get { return _Periods; }
        //    set { _Periods = value; OnPropertyChanged("Periods"); }
        //}

        //private IEnumerable<EmployeeSearchView> _employeeSearch;
        //public IEnumerable<EmployeeSearchView> EmployeeSearch
        //{
        //    get { return _employeeSearch; }
        //    set { _employeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        //}

        //private EmployeeMulitiplePaymentView _CurrentEmployeeMulitiplePaymentView;
        //public EmployeeMulitiplePaymentView CurrentEmployeeMulitiplePaymentView
        //{
        //    get { return _CurrentEmployeeMulitiplePaymentView; }
        //    set { _CurrentEmployeeMulitiplePaymentView = value; OnPropertyChanged("CurrentEmployeeMulitiplePaymentView"); }
        //}

        //private IEnumerable<trns_EmployeeMultiplePayments> _EmployeeMultiplePayments;
        //public IEnumerable<trns_EmployeeMultiplePayments> EmployeeMultiplePayments
        //{
        //    get { return _EmployeeMultiplePayments; }
        //    set { _EmployeeMultiplePayments = value; OnPropertyChanged("EmployeeMultiplePayments"); }
        //}

        //private EmployeeSearchView _currentEmployeeSearch;
        //public EmployeeSearchView CurrentEmployeeSearch
        //{
        //    get { return _currentEmployeeSearch; }
        //    set { _currentEmployeeSearch = value; OnPropertyChanged("CurrentEmployeeSearch"); if (CurrentEmployeeSearch != null) { EmployeeFilter(); FullName = CurrentEmployeeSearch.first_name + " " + CurrentEmployeeSearch.surname; } }

        //}

        //private List<EmployeeMulitiplePaymentView> _EmployeeMulitiplePaymentViews = new List<EmployeeMulitiplePaymentView>();
        //public List<EmployeeMulitiplePaymentView> EmployeeMulitiplePaymentViews
        //{
        //    get { return _EmployeeMulitiplePaymentViews; }
        //    set { _EmployeeMulitiplePaymentViews = value; OnPropertyChanged("EmployeeMulitiplePaymentViews"); }
        //}
        //#endregion

        //#region Filter Method
        //void EmployeeFilter()
        //{
        //    TotalSalary = 0;
        //    Balance = 0;
        //    EmployeeMulitiplePaymentViews = null;
        //    EmployeeMulitiplePaymentViews = TempEmployeeMulitiplePaymentViews.Where(c => c.employee_id == CurrentEmployeeSearch.employee_id).ToList();
        //    if (EmployeeMulitiplePaymentViews.Count == 0)
        //        clsMessages.setMessage("This Employee dose not belong to a bank");
        //    else
        //        if (EmployeePayment != null && EmployeePayment.ToList().Count != 0)
        //        {
        //            trns_EmployeePayment tempCurrentEmployeePayment;
        //            tempCurrentEmployeePayment = EmployeePayment.FirstOrDefault(c => c.employee_id == CurrentEmployeeSearch.employee_id);
        //            if (tempCurrentEmployeePayment != null)
        //            {
        //                TotalSalary = tempCurrentEmployeePayment.total_salary;
        //                List<trns_EmployeeMultiplePayments> TempMultiplePayment = new List<trns_EmployeeMultiplePayments>();
        //                Balance = TotalSalary;
        //              if (EmployeeMultiplePayments != null)
        //                {
        //                    foreach (var item in EmployeeMulitiplePaymentViews)
        //                    {
        //                       trns_EmployeeMultiplePayments TempEmployeeMultiplePayments = new trns_EmployeeMultiplePayments();
        //                        TempEmployeeMultiplePayments = EmployeeMultiplePayments.Where(c => c.account_id == item.account_id && c.emloyee_id == item.employee_id && c.period_id == CurrentPeriod.period_id).FirstOrDefault();
        //                        if (TempEmployeeMultiplePayments != null && TempEmployeeMultiplePayments.amuont != null)
        //                        item.Value = (Decimal)TempEmployeeMultiplePayments.amuont;
        //                    }
        //                }
        //                BalanceMethod();
        //            }
        //        }
        //        else
        //        {
        //            EmployeeMulitiplePaymentViews = null;
        //            clsMessages.setMessage("No payments for this period");
        //        }
        //}
        //#endregion

        //#region Balace Button and Method
        //void BalanceMethod()
        //{
        //    Balance = TotalSalary;
        //    foreach (var item in EmployeeMulitiplePaymentViews)
        //        Balance = Balance - item.Value;
        //}
        //public ICommand BalanceButton
        //{
        //    get { return new RelayCommand(BalanceMethod); }
        //}
        //#endregion

        //#region Save Button and Method
        //void Save()
        //{
        //    string SaveList = "";
        //    BalanceMethod();
        //    if (Balance >= 0)
        //    {
        //        if (CurrentPeriod != null && CurrentPeriod.period_id != null)
        //            foreach (var item in EmployeeMulitiplePaymentViews)
        //            {
        //                if (item != null && item.account_id != null && item.employee_id != null && item.Value != null && item.Value != 0)
        //                {
        //                    trns_EmployeeMultiplePayments MultiplePaymentSave = new trns_EmployeeMultiplePayments();
        //                    MultiplePaymentSave.account_id = (Guid)item.account_id;
        //                    MultiplePaymentSave.emloyee_id = (Guid)item.employee_id;
        //                    MultiplePaymentSave.period_id = (Guid)CurrentPeriod.period_id;
        //                    MultiplePaymentSave.amuont = item.Value;

        //                    if (serviceClient.SaveMultiplePayments(MultiplePaymentSave))
        //                        SaveList += Resources.SaveSucess + " " + item.bank_name + "\n";
        //                    else
        //                        SaveList += Resources.SaveFail + " " + item.bank_name + "\n";
        //                }
        //            }
        //        MessageBox.Show(SaveList, "", MessageBoxButton.OK, MessageBoxImage.Information);
        //        RefreshMultiplePaymentsView();
        //        RefereshEmployeeMultiplePayment(true);
        //        BalanceMethod();
        //      }
        //    else
        //        clsMessages.setMessage("Not A Valid Balance");
        //}

        //public ICommand SaveButton
        //{
        //    get { return new RelayCommand(Save); }
        //}
        //#endregion

        //#region Next Button and Method
        //void Next()
        //{
        //    List<EmployeeSearchView> emp = new List<EmployeeSearchView>();
        //    emp = EmployeeSearch.ToList();

        //    emp.IndexOf(CurrentEmployeeSearch);


        //    try
        //    {
        //        CurrentEmployeeSearch = EmployeeSearch.ElementAt(emp.IndexOf(CurrentEmployeeSearch) + 1);
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        clsMessages.setMessage("This Element is Empty");
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        clsMessages.setMessage("Sorry..No more elements avalable");
        //    }
        //}

        //public ICommand NextButton
        //{
        //    get { return new RelayCommand(Next); }
        //}
        //#endregion

        //#region Back Button and Method
        //void Back()
        //{
        //    List<EmployeeSearchView> emp = new List<EmployeeSearchView>();
        //    emp = EmployeeSearch.ToList();

        //    emp.IndexOf(CurrentEmployeeSearch);


        //    try
        //    {
        //        CurrentEmployeeSearch = EmployeeSearch.ElementAt(emp.IndexOf(CurrentEmployeeSearch) - 1);
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        clsMessages.setMessage("This Element is Empty");
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        clsMessages.setMessage("Sorry..No more elements avalable");
        //    }
        //}

        //public ICommand BackButton
        //{
        //    get { return new RelayCommand(Back); }
        //}
        //#endregion

        //#region Search and Method
        //void search()
        //{
        //    if (CurrentPeriod != null)
        //    {
        //        Guid PaymentID = new Guid("00000000-0000-0000-0000-000000000001");
        //        EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow(PaymentID);
        //        window.ShowDialog();
        //        if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
        //        {
        //            EmployeeSearch = window.viewModel.selectEmployeeList;
        //            CurrentEmployeeSearch = EmployeeSearch.FirstOrDefault();
        //        }
        //    }
        //    else
        //    {
        //        clsMessages.setMessage("Please Select a Period");
        //    }
        //}

        //public ICommand SearchButton
        //{
        //    get { return new RelayCommand(search); }
        //}
        //#endregion

        //#region Refresh Methods
        //void RefereshEmployeeMultiplePayment()
        //{
        //    try
        //    {
        //        serviceClient.GetEmployeeMultiplePaymentsCompleted += (s, e) =>
        //        {
        //            EmployeeMultiplePayments = e.Result;
        //        };
        //        serviceClient.GetEmployeeMultiplePaymentsAsync();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //void RefereshEmployeeMultiplePayment(bool save)
        //{
        //    try
        //    {
        //        serviceClient.GetEmployeeMultiplePaymentsCompleted += (s, e) =>
        //        {
        //            EmployeeMultiplePayments = null;
        //            EmployeeMultiplePayments = e.Result;
        //            if (EmployeeMultiplePayments != null)
        //                EmployeeFilter();
        //        };
        //        serviceClient.GetEmployeeMultiplePaymentsAsync();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //void ReferesshEmployeePayments()
        //{
        //    try
        //    {
        //        serviceClient.GetEmployeePaymentByPeriodCompleted += (s, e) =>
        //        {
        //            EmployeePayment = e.Result.Where(c => c.period_id == CurrentPeriod.period_id);
        //        };
        //        serviceClient.GetEmployeePaymentByPeriodAsync(CurrentPeriod.period_id);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //void RefreshMultiplePaymentsView()
        //{
        //    try
        //    {
        //        serviceClient.GetMultiplePaymentViewCompleted += (s, e) =>
        //           {
        //               try
        //               {
        //                   TempEmployeeMulitiplePaymentViews.Clear();
        //                   TempEmployeeMulitiplePaymentViews = e.Result.ToList();
        //               }
        //               catch (Exception)
        //               {
        //                   clsMessages.setMessage("No Banks assigned for employees");
        //               }
        //           };
        //        serviceClient.GetMultiplePaymentViewAsync();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //void RefreshPeriod()
        //{
        //    try
        //    {
        //        Periods = serviceClient.GetPeriods();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        #endregion
    }
}
