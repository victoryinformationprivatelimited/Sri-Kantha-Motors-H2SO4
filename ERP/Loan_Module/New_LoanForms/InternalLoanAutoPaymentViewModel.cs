using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ERP.Properties;

namespace ERP.Loan_Module.New_LoanForms
{
    class InternalLoanAutoPaymentViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<InternalLoanWithoutGurantorsView> AllLoans = new List<InternalLoanWithoutGurantorsView>();
        List<InternalLoanWithoutGurantorsView> Templist = new List<InternalLoanWithoutGurantorsView>();
        List<InternalLoanWithoutGurantorsView> RemovedList = new List<InternalLoanWithoutGurantorsView>();
        List<InternalLoanProcessForView> AllInternalLoanProcessDetails = new List<InternalLoanProcessForView>();
        #endregion

        #region Constrouctor
        public InternalLoanAutoPaymentViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshLoans();
            RefreshPeriod();
            CurrentPeriod = new z_Period();
            RefreshInternalLoanProcessDetails();
            CurrentLoans = new InternalLoanWithoutGurantorsView();
            Department = null;
            RefreshDepartments();
            //ToBeProcessed = true;
            // NotToBeProcessed = false;
        }
        #endregion

        #region Properties

        private IEnumerable<InternalLoanWithoutGurantorsView> _Loans;
        public IEnumerable<InternalLoanWithoutGurantorsView> Loans
        {
            get { return _Loans; }
            set { _Loans = value; OnPropertyChanged("Loans"); }
        }

        private InternalLoanWithoutGurantorsView _CurrentLoans;
        public InternalLoanWithoutGurantorsView CurrentLoans
        {
            get { return _CurrentLoans; }
            set { _CurrentLoans = value; OnPropertyChanged("CurrentLoans"); }
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod != null)GetLoansWithoutPeroiod(); }
        }

        private IEnumerable<InternalLoanWithoutGurantorsView> _RemovedLoans;
        public IEnumerable<InternalLoanWithoutGurantorsView> RemovedLoans
        {
            get { return _RemovedLoans; }
            set { _RemovedLoans = value; OnPropertyChanged("RemovedLoans"); }
        }

        private InternalLoanWithoutGurantorsView _CurrentRemovedLoan;
        public InternalLoanWithoutGurantorsView CurrentRemovedLoan
        {
            get { return _CurrentRemovedLoan; }
            set { _CurrentRemovedLoan = value; OnPropertyChanged("CurrentRemovedLoan"); }
        }

        private IEnumerable<InternalLoanProcessForView> _InternalLoanProcessDetails;
        public IEnumerable<InternalLoanProcessForView> InternalLoanProcessDetails
        {
            get { return _InternalLoanProcessDetails; }
            set { _InternalLoanProcessDetails = value; OnPropertyChanged("InternalLoanProcessDetails"); }
        }

        private InternalLoanProcessForView _CurrentInternalLoanProcessDetails;
        public InternalLoanProcessForView CurrentInternalLoanProcessDetails
        {
            get { return _CurrentInternalLoanProcessDetails; }
            set { _CurrentInternalLoanProcessDetails = value; OnPropertyChanged("CurrentInternalLoanProcessDetails"); }
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
            set { _CurrentDepartment = value; OnPropertyChanged("CurrentDepartment"); if (CurrentDepartment != null)FilterEmployeesByDepartment(); }
        }

        #endregion

        #region RefreshMethods

        private void RefreshLoans()
        {
            serviceClient.GetInternalBankLoanWithoutGurantorsCompleted += (s, e) =>
            {
                Loans = e.Result;
                if (Loans != null && Loans.Count() > 0)
                {
                    AllLoans = Loans.ToList();
                }
            };
            serviceClient.GetInternalBankLoanWithoutGurantorsAsync();
        }
        private void RefreshPeriod()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.OrderBy(c => c.start_date);
            };
            serviceClient.GetPeriodsAsync();
        }
        private void RefreshInternalLoanProcessDetails()
        {
            //serviceClient.GetAllLoanPaymentDetailsCompleted += (s, e) =>
            //    {
            //        InternalLoanProcessDetails = e.Result;
            //        if (InternalLoanProcessDetails != null && InternalLoanProcessDetails.Count() > 0)
            //            AllInternalLoanProcessDetails = InternalLoanProcessDetails.ToList();
            //    };
            //serviceClient.GetAllLoanPaymentDetailsAsync();
            InternalLoanProcessDetails = serviceClient.GetAllLoanPaymentDetails();
            if (InternalLoanProcessDetails != null && InternalLoanProcessDetails.Count() > 0)
                AllInternalLoanProcessDetails = InternalLoanProcessDetails.ToList();
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

        #region ButtonCommands

        public ICommand AddEmpButton
        {
            get { return new RelayCommand(AddEmploee); }
        }
        public ICommand RemoveEmpButton
        {
            get { return new RelayCommand(RemoveEmployee); }
        }
        public ICommand ProcessButton
        {
            get { return new RelayCommand(Process); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Methods
        private void AddEmploee()
        {
            if (RemovedLoansList.Count > 0)
            {
                foreach (InternalLoanWithoutGurantorsView item in RemovedLoansList)
                {
                    RemovedList.Remove(item);
                    Templist.Add(item);
                }
                Loans = null;
                RemovedLoans = null;
                Loans = Templist;
                RemovedLoans = RemovedList;
            }

        }
        private void RemoveEmployee()
        {
            if (SelectedLoans.Count > 0)
            {
                Templist = Loans.ToList();
                foreach (InternalLoanWithoutGurantorsView item in SelectedLoans)
                {
                    Templist.Remove(item);
                    RemovedList.Add(item);
                }
                Loans = null;
                Loans = Templist;
                RemovedLoans = null;
                RemovedLoans = RemovedList;
            }
        }
        private void Process()
        {
            if (clsSecurity.GetSavePermission(606))
            {
                if (ValidateProcess())
                {
                    clsMessages.setMessage("Are You Sure You Want To Process Internal Bank Loans?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        List<InternalLoanProcessForView> SetValues = new List<InternalLoanProcessForView>();
                        foreach (var item in Loans)
                        {
                            InternalLoanProcessForView InternalLoanPayment = new InternalLoanProcessForView();

                            InternalLoanPayment.EmployeeID = item.employee_id;
                            InternalLoanPayment.loan_id = item.loan_id;
                            InternalLoanPayment.PaymentDate = DateTime.Now;
                            InternalLoanPayment.period_id = CurrentPeriod.period_id;
                            InternalLoanPayment.is_Completed = false;
                            InternalLoanPayment.InternalLoanID = item.InternalLoanID;

                            if (item.LoanRemainingAmount == item.LoanTotalAmount)
                            {
                                InternalLoanPayment.Installment_No = 1;
                            }
                            else
                            {
                                var result = InternalLoanProcessDetails.Where(c => c.EmployeeID == item.employee_id && c.loan_id == item.loan_id && c.int_loan_id == item.InternalLoanID).Max(d => d.Installment_No);
                                InternalLoanPayment.Installment_No = result + 1;
                            }

                            if (item.InstallmentMonths == InternalLoanPayment.Installment_No)
                            {
                                InternalLoanPayment.PaidAmount_WithoutIntrest = item.LoanRemainingAmount;
                                InternalLoanPayment.Paid_IntrestAmount = ((item.LoanRemainingAmount * (item.LoanIntrestRate / 100)) / 12);
                                InternalLoanPayment.Payment_Amount = item.LoanRemainingAmount + ((item.LoanRemainingAmount * (item.LoanIntrestRate / 100)) / 12);
                                InternalLoanPayment.LoanRemainingAmount = 0;
                                InternalLoanPayment.is_Completed = true;
                                InternalLoanPayment.is_active = false;
                            }
                            else
                            {
                                InternalLoanPayment.PaidAmount_WithoutIntrest = (item.LoanTotalAmount / item.InstallmentMonths);
                                InternalLoanPayment.Paid_IntrestAmount = ((item.LoanRemainingAmount * (item.LoanIntrestRate / 100)) / 12);
                                InternalLoanPayment.Payment_Amount = (item.LoanTotalAmount / item.InstallmentMonths) + ((item.LoanRemainingAmount * (item.LoanIntrestRate / 100)) / 12);
                                InternalLoanPayment.LoanRemainingAmount = item.LoanRemainingAmount - (item.LoanTotalAmount / item.InstallmentMonths);
                                InternalLoanPayment.is_active = true;
                            }

                            //InternalLoanPayment.LoanRemainingAmount = item.LoanRemainingAmount - (item.LoanTotalAmount / item.InstallmentMonths);
                            SetValues.Add(InternalLoanPayment);
                        }

                        if (serviceClient.SaveInternalLoanProcess(SetValues.ToArray()))
                        {
                            New();
                            clsMessages.setMessage("Employee Internal Loan Process Successfully Saved...");
                        }
                        else
                        {
                            clsMessages.setMessage("Employee Internal Loan Process Failed...");
                        }
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to process");
        }
        private bool ValidateProcess()
        {
            if (CurrentPeriod.period_id == null || CurrentPeriod.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Period To Process Loan Payment...");
                return false;
            }
            else if (Loans == null || Loans.Count() == 0)
            {
                clsMessages.setMessage("There Aren't Any Loans To Process Loan Payment...");
                return false;
            }
            else
                return true;
        }
        private void New()
        {
            RefreshLoans();
            RefreshPeriod();
            Periods = null;
            CurrentPeriod = new z_Period();
            RefreshInternalLoanProcessDetails();
            CurrentLoans = new InternalLoanWithoutGurantorsView();
            RemovedLoans = null;
            CurrentRemovedLoan = new InternalLoanWithoutGurantorsView();
            Department = null;
            RefreshDepartments();
            //ToBeProcessed = true;
            //    NotToBeProcessed = false;
        }
        private void GetLoansWithoutPeroiod()
        {

            Loans = null;
            Loans = AllLoans.Where(c => !AllInternalLoanProcessDetails.Any(d => c.employee_id == d.EmployeeID && c.loan_id == d.loan_id && d.period_id == CurrentPeriod.period_id)).ToList();
            // }
        }
        // private void FilterLoans()
        //{

        //     if (ToBeProcessed == true)
        //     {
        //         if (SearchIndex == 0)
        //         {
        //             //GetLoansWithoutPeroiod();
        //             IEnumerable<InternalLoanWithoutGurantorsView> temp = Loans;
        //             Loans = temp.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        //         }
        //         if (SearchIndex == 1)
        //         {
        //            // GetLoansWithoutPeroiod();
        //             IEnumerable<InternalLoanWithoutGurantorsView> temp = Loans;
        //             Loans = temp.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));

        //         }
        //         if (SearchIndex == 2)
        //         {
        //            // GetLoansWithoutPeroiod();
        //             Loans = Loans.Where(c => c.surname != null && c.surname.ToUpper().Contains(Search.ToUpper()));
        //         }
        //         if (SearchIndex == 3)
        //         {
        //            // GetLoansWithoutPeroiod();
        //             Loans = Loans.Where(c => c.loan_name != null && c.loan_name.ToUpper().Contains(Search.ToUpper()));
        //         }
        //     }
        //     else if (NotToBeProcessed == true)
        //     {
        //         if (SearchIndex == 0)
        //         {
        //             RemovedLoans = RemovedLoans.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        //         }
        //         if (SearchIndex == 1)
        //         {
        //             RemovedLoans = RemovedLoans.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
        //         }
        //         if (SearchIndex == 2)
        //         {
        //             RemovedLoans = RemovedLoans.Where(c => c.surname != null && c.surname.ToUpper().Contains(Search.ToUpper()));
        //         }
        //         if (SearchIndex == 3)
        //         {
        //             RemovedLoans = RemovedLoans.Where(c => c.loan_name != null && c.loan_name.ToUpper().Contains(Search.ToUpper()));
        //         }
        //     }
        // }
        private void FilterEmployeesByDepartment()
        {
            Loans = Loans.Where(c => c.department_id == CurrentDepartment.department_id);
        }
        #endregion
    }
}
