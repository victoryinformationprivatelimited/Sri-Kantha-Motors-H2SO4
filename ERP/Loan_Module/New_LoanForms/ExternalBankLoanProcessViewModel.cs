using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.New_LoanForms
{
    class ExternalBankLoanProcessViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<ExternalBankLoanView> AllExternalBankLoanViewList = new List<ExternalBankLoanView>();
        List<ExternalBankLoanView> RemovedExternalBankLoanList = new List<ExternalBankLoanView>();
        List<ExternalBankLoanView> AddExternalBankLoanList;
        List<ProcessDetailsForExternalLoanView> AllProcessDetailsForExternalLoanView = new List<ProcessDetailsForExternalLoanView>();

        #endregion

        #region Constrouctor
        public ExternalBankLoanProcessViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshExternalBankLoanView();
            RefreshPeriods();
            CurrentPeriod = new z_Period();
            CurrentRemovedExternalbankLoanView = new ExternalBankLoanView();
            RefreshProcessDetailsForexternalLoan();
            AddExternalBankLoanList = new List<ExternalBankLoanView>();
            Department = null;
            RefreshDepartments();
        }

        #endregion

        #region Properties

        private IEnumerable<ExternalBankLoanView> _ExternalBankLoanView;
        public IEnumerable<ExternalBankLoanView> ExternalBankLoanView
        {
            get { return _ExternalBankLoanView; }
            set { _ExternalBankLoanView = value; OnPropertyChanged("ExternalBankLoanView"); }
        }

        private ExternalBankLoanView _CurrentExternalBankLoanView;
        public ExternalBankLoanView CurrentExternalBankLoanView
        {
            get { return _CurrentExternalBankLoanView; }
            set { _CurrentExternalBankLoanView = value; OnPropertyChanged("CurrentExternalBankLoanView"); }
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod != null)GetLoanDetailsWithoutPeriod(); }
        }

        private IEnumerable<ExternalBankLoanView> _RemovedExternalbankLoanView;
        public IEnumerable<ExternalBankLoanView> RemovedExternalbankLoanView
        {
            get { return _RemovedExternalbankLoanView; }
            set { _RemovedExternalbankLoanView = value; OnPropertyChanged("RemovedExternalbankLoanView"); }
        }

        private ExternalBankLoanView _CurrentRemovedExternalbankLoanView;
        public ExternalBankLoanView CurrentRemovedExternalbankLoanView
        {
            get { return _CurrentRemovedExternalbankLoanView; }
            set { _CurrentRemovedExternalbankLoanView = value; OnPropertyChanged("CurrentRemovedExternalbankLoanView"); }
        }

        private IEnumerable<ProcessDetailsForExternalLoanView> _ProcessDetailsForExternalLoan;
        public IEnumerable<ProcessDetailsForExternalLoanView> ProcessDetailsForExternalLoan
        {
            get { return _ProcessDetailsForExternalLoan; }
            set { _ProcessDetailsForExternalLoan = value; OnPropertyChanged("ProcessDetailsForExternalLoan"); }
        }

        private ProcessDetailsForExternalLoanView _CurrentProcessDetailsForExternalLoan;
        public ProcessDetailsForExternalLoanView CurrentProcessDetailsForExternalLoan
        {
            get { return _CurrentProcessDetailsForExternalLoan; }
            set { _CurrentProcessDetailsForExternalLoan = value; OnPropertyChanged("CurrentProcessDetailsForExternalLoan"); }
        }

        private IList _SelectedLoans = new ArrayList();
        public IList SelectedLoans
        {
            get { return _SelectedLoans; }
            set { _SelectedLoans = value; OnPropertyChanged("SelectedLoans"); }
        }

        private IList _RemovedLoansList = new ArrayList();
        public IList RemovedLoansList
        {
            get { return _RemovedLoansList; }
            set { _RemovedLoansList = value; OnPropertyChanged("RemovedLoansList"); }
        }

        //private string _Search;
        //public string Search
        //{
        //    get { return _Search; }
        //    set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterLoans(); }
        //}

        //private int _SearchIndex;
        //public int SearchIndex
        //{
        //    get { return _SearchIndex; }
        //    set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        //}

        //private bool _ToBeProcessed;
        //public bool ToBeProcessed
        //{
        //    get { return _ToBeProcessed; }
        //    set { _ToBeProcessed = value; OnPropertyChanged("ToBeProcessed"); }
        //}

        //private bool _NotToBeProcessed;
        //public bool NotToBeProcessed
        //{
        //    get { return _NotToBeProcessed; }
        //    set { _NotToBeProcessed = value; OnPropertyChanged("NotToBeProcessed"); }
        //}

        private IEnumerable<z_Department> _Department;
        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { _Department = value; OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; OnPropertyChanged("CurrentDepartment"); if (CurrentDepartment != null)FilterLoansDepartmentWise(); }
        }
        
        

        #endregion

        #region Refresh Methods
        private void RefreshExternalBankLoanView()
        {
            serviceClient.GetExternalLoanViewCompleted += (s, e) =>
                {
                    ExternalBankLoanView = e.Result;
                    if (ExternalBankLoanView != null && ExternalBankLoanView.Count() > 0)
                        AllExternalBankLoanViewList = ExternalBankLoanView.ToList();
                };
            serviceClient.GetExternalLoanViewAsync();
        }
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
                {
                    Periods = e.Result;
                };
            serviceClient.GetPeriodsAsync();
        }
        private void RefreshProcessDetailsForexternalLoan()
        {
            serviceClient.GetProcessDetailsForExternalLoanViewCompleted += (s, e) =>
                {
                    ProcessDetailsForExternalLoan = e.Result;
                    if (ProcessDetailsForExternalLoan != null && ProcessDetailsForExternalLoan.Count() > 0)
                        AllProcessDetailsForExternalLoanView = ProcessDetailsForExternalLoan.ToList();
                };
            serviceClient.GetProcessDetailsForExternalLoanViewAsync();
        }
        private void RefreshDepartments()
        {
            serviceClient.GetDepartmentCompleted += (s, e) =>
            {
                Department = e.Result;
            };
            serviceClient.GetDepartmentAsync();
        }

        #endregion

        #region Button Commands
        public ICommand AddEmployeeButton
        {
            get { return new RelayCommand(AddEmployee); }
        }
        public ICommand RemoveEmployeeButton
        {
            get { return new RelayCommand(RemoveEmployee); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        public ICommand ProcessButton
        {
            get { return new RelayCommand(Process); }
        }

        #endregion

        #region Methods
        private void AddEmployee()
        {
            if (RemovedLoansList.Count > 0)
            {
                foreach (ExternalBankLoanView item in RemovedLoansList)
                {
                    RemovedExternalBankLoanList.Remove(item);
                    AddExternalBankLoanList.Add(item);
                }
                ExternalBankLoanView = null;
                RemovedExternalbankLoanView = null;
                ExternalBankLoanView = AddExternalBankLoanList;
                RemovedExternalbankLoanView = RemovedExternalBankLoanList;                
            }
        }
        private void RemoveEmployee()
        {
            if (SelectedLoans.Count > 0)
            {
                AddExternalBankLoanList = ExternalBankLoanView.ToList();
                foreach (ExternalBankLoanView item in SelectedLoans)
                {
                    AddExternalBankLoanList.Remove(item);
                    RemovedExternalBankLoanList.Add(item);
                }
                ExternalBankLoanView = null;
                RemovedExternalbankLoanView = null;
                ExternalBankLoanView = AddExternalBankLoanList;
                RemovedExternalbankLoanView = RemovedExternalBankLoanList;

            }
        }
        private void GetLoanDetailsWithoutPeriod()
        {
            //if (AllProcessDetailsForExternalLoanView == null || AllProcessDetailsForExternalLoanView.Count == 0)
            //{
            //    ExternalBankLoanView = AllExternalBankLoanViewList;
            //}
            //else
            //{
            ExternalBankLoanView = null;
            ExternalBankLoanView = AllExternalBankLoanViewList.Where(c => !AllProcessDetailsForExternalLoanView.Any(d => c.employee_id == d.employee_id && d.period_id == CurrentPeriod.period_id));

            //}
        }
        private void Process()
        {
            if (clsSecurity.GetSavePermission(609))
            {
                if (ValidateProcess())
                {
                    clsMessages.setMessage("Are You Sure You Want To Process External Bank Loans?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        List<ProcessDetailsForExternalLoanView> SetValues = new List<ProcessDetailsForExternalLoanView>();
                        foreach (var item in ExternalBankLoanView)
                        {
                            ProcessDetailsForExternalLoanView ExternalLoanPayment = new ProcessDetailsForExternalLoanView();
                            ExternalLoanPayment.BankLoanID = item.BankLoanID;
                            ExternalLoanPayment.period_id = CurrentPeriod.period_id;
                            ExternalLoanPayment.employee_id = item.employee_id;
                            ExternalLoanPayment.isComplete = false;
                            var result = ProcessDetailsForExternalLoan.Where(c => c.BankLoanID == item.BankLoanID && c.employee_id == item.employee_id).Max(d => d.Installment_No);
                            if (result == 0 || result == null)
                            {
                                ExternalLoanPayment.Installment_No = 1;
                            }
                            else
                            {
                                ExternalLoanPayment.Installment_No = result + 1;
                            }

                            if (item.LoanInstallmentMonths == 0)
                            {
                                ExternalLoanPayment.InstallmentAmount = item.LoanAmountPerMonth;
                            }
                            else if (item.LoanInstallmentMonths == ExternalLoanPayment.Installment_No)
                            {
                                ExternalLoanPayment.InstallmentAmount = item.LoanAmountPerMonth;
                                ExternalLoanPayment.isComplete = true;
                            }
                            else
                            {
                                ExternalLoanPayment.InstallmentAmount = item.LoanAmountPerMonth;
                            }

                            ExternalLoanPayment.save_user_id = clsSecurity.loggedUser.user_id; ;
                            ExternalLoanPayment.save_datetime = DateTime.Now;
                            SetValues.Add(ExternalLoanPayment);
                        }
                        if (serviceClient.SaveExternalBankLoanProcess(SetValues.ToArray()))
                        {
                            New();
                            clsMessages.setMessage("External Bank Loan Process Have Been Successfully Saved...");
                        }
                        else
                        {
                            clsMessages.setMessage("External Bank Loan Process Save Failed...");
                        }

                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to process");
        }
        private bool ValidateProcess()
        {
            if (CurrentPeriod.period_id == null)
            {
                clsMessages.setMessage("Please Select a Period to process");
                return false;
            }
            else if (ExternalBankLoanView == null)
            {
                clsMessages.setMessage("There Is No external lLoan to Process...");
                return false;
            }
            else
                return true;
        }
        private void New()
        {
            RefreshExternalBankLoanView();
            RefreshPeriods();
            CurrentPeriod = new z_Period();
            CurrentRemovedExternalbankLoanView = new ExternalBankLoanView();
            RefreshProcessDetailsForexternalLoan();
            AddExternalBankLoanList = new List<ExternalBankLoanView>();
          //  ToBeProcessed = true;
           // NotToBeProcessed = false;
            Department = null;
            RefreshDepartments();
        }
        //private void FilterLoans()
        //{
        //    if (ToBeProcessed == true)
        //    {
        //        if (SearchIndex == 0)
        //        {
        //            ExternalBankLoanView = ExternalBankLoanView.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        //        }
        //        if (SearchIndex == 1)
        //        {
        //            ExternalBankLoanView = ExternalBankLoanView.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
        //        }
        //        if (SearchIndex == 2)
        //        {
        //            ExternalBankLoanView = ExternalBankLoanView.Where(c => c.surname != null && c.surname.ToUpper().Contains(Search.ToUpper()));
        //        }
        //        if (SearchIndex == 3)
        //        {
        //            ExternalBankLoanView = ExternalBankLoanView.Where(c => c.BankLoanName != null && c.BankLoanName.ToUpper().Contains(Search.ToUpper()));
        //        }
        //    }
        //    else if(NotToBeProcessed == true)
        //    {
        //        if (SearchIndex == 0)
        //        {
        //            RemovedExternalbankLoanView = RemovedExternalbankLoanView.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        //        }
        //        if (SearchIndex == 1)
        //        {
        //            RemovedExternalbankLoanView = RemovedExternalbankLoanView.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
        //        }
        //        if (SearchIndex == 2)
        //        {
        //            RemovedExternalbankLoanView = RemovedExternalbankLoanView.Where(c => c.surname != null && c.surname.ToUpper().Contains(Search.ToUpper()));
        //        }
        //        if (SearchIndex == 3)
        //        {
        //            RemovedExternalbankLoanView = RemovedExternalbankLoanView.Where(c => c.BankLoanName != null && c.BankLoanName.ToUpper().Contains(Search.ToUpper()));
        //        }
        //    }
        //}

        private void FilterLoansDepartmentWise()
        {
            ExternalBankLoanView = ExternalBankLoanView.Where(c => c.department_id == CurrentDepartment.department_id);         
        }
        #endregion
    }
}
