using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.New_LoanForms
{
    class ManualInternalLoanPaymentViewModel : ViewModelBase
    {
        #region Fields

        private ERPServiceClient serviceClient;
        List<InternalLoanMainView> AllGurantorsOfLoan;

        #endregion

        #region Counstrouctor
        public ManualInternalLoanPaymentViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllGurantorsOfLoan = new List<InternalLoanMainView>();
            New();
        }

        #endregion

        #region Properties

        private IEnumerable<InternalLoanWithoutGurantorsView> _InternalLoanView;
        public IEnumerable<InternalLoanWithoutGurantorsView> InternalLoanView
        {
            get { return _InternalLoanView; }
            set { _InternalLoanView = value; OnPropertyChanged("InternalLoanView"); }
        }

        private InternalLoanWithoutGurantorsView _CurrentInternalView;
        public InternalLoanWithoutGurantorsView CurrentInternalView
        {
            get { return _CurrentInternalView; }
            set
            {
                _CurrentInternalView = value;
                OnPropertyChanged("CurrentInternalView");
                if (CurrentInternalView != null)
                {
                    FilterGurantorsForLoan();
                    SetPayableAmount();

                    //SetEmployeeToPay();
                }
            }
        }

        private IEnumerable<EmployeeSumarryView> _SelectedGurantorGrid;
        public IEnumerable<EmployeeSumarryView> SelectedGurantorGrid
        {
            get { return _SelectedGurantorGrid; }
            set { _SelectedGurantorGrid = value; OnPropertyChanged("SelectedGurantorGrid"); }
        }

        private EmployeeSumarryView _CurrentSelectedGurantorGrid;
        public EmployeeSumarryView CurrentSelectedGurantorGrid
        {
            get { return _CurrentSelectedGurantorGrid; }
            set { _CurrentSelectedGurantorGrid = value; OnPropertyChanged("CurrentSelectedGurantorGrid"); }
        }

        private bool _IsGuarantor;
        public bool IsGuarantor
        {
            get { return _IsGuarantor; }
            set { _IsGuarantor = value; OnPropertyChanged("IsGuarantor"); if (IsGuarantor)GPayThroughNewLoan = false; }
        }

        private bool _GPayThroughNewLoan;
        public bool GPayThroughNewLoan
        {
            get { return _GPayThroughNewLoan; }
            set { _GPayThroughNewLoan = value; OnPropertyChanged("GPayThroughNewLoan"); if (GPayThroughNewLoan) IsGuarantor = false; }
        }

        private decimal _PaidAmount;
        public decimal PaidAmount
        {
            get { return _PaidAmount; }
            set { _PaidAmount = value; OnPropertyChanged("PaidAmount"); }
        }

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; OnPropertyChanged("Comments"); }
        }

        //private string _Name;
        //public string Name
        //{
        //    get { return _Name; }
        //    set { _Name = value; OnPropertyChanged("Name"); }
        //}

        //private string _EPF;
        //public string EPF
        //{
        //    get { return _EPF; }
        //    set { _EPF = value; OnPropertyChanged("EPF"); }
        //}

        //private string _NIC;
        //public string NIC
        //{
        //    get { return _NIC; }
        //    set { _NIC = value; OnPropertyChanged("NIC"); }
        //}

        private decimal _PayingAmount;
        public decimal PayingAmount
        {
            get { return _PayingAmount; }
            set { _PayingAmount = value; OnPropertyChanged("PayingAmount"); }
        }

        private bool _PayAmtValidate;
        public bool PayAmtValidate
        {
            get { return _PayAmtValidate; }
            set { _PayAmtValidate = value; OnPropertyChanged("PayAmtValidate"); }
        }

        private IEnumerable<usr_UserEmployee> _Users;
        public IEnumerable<usr_UserEmployee> Users
        {
            get { return _Users; }
            set { _Users = value; OnPropertyChanged("Users"); }
        }

        private IEnumerable<dtl_EmployeeSupervisor> _Supervisors;
        public IEnumerable<dtl_EmployeeSupervisor> Supervisors
        {
            get { return _Supervisors; }
            set { _Supervisors = value; OnPropertyChanged("Supervisors"); }
        }

        private dtl_EmployeeSupervisor _CurrentSupervisors;
        public dtl_EmployeeSupervisor CurrentSupervisors
        {
            get { return _CurrentSupervisors; }
            set { _CurrentSupervisors = value; OnPropertyChanged("CurrentSupervisors"); }
        }

        private Visibility _DisableGuarantor;
        public Visibility DisableGuarantor
        {
            get { return _DisableGuarantor; }
            set { _DisableGuarantor = value; OnPropertyChanged("DisableGuarantor"); }
        }

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterSearch(); }
        }



        #endregion

        #region RefreshMethods
        private void RefreshInternalView()
        {
            serviceClient.GetInternalBankLoanWithoutGurantorsCompleted += (s, e) =>
                {
                    InternalLoanView = e.Result;
                };
            serviceClient.GetInternalBankLoanWithoutGurantorsAsync();
        }
        private void RefreshGurontors()
        {
            serviceClient.GetInternalBankLoanCompleted += (s, e) =>
            {
                IEnumerable<InternalLoanMainView> ie = e.Result;
                if (ie != null && ie.Count() > 0)
                    AllGurantorsOfLoan = ie.ToList();
            };
            serviceClient.GetInternalBankLoanAsync();
        }
        private void RefreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                Users = e.Result;
            };
            serviceClient.GetUserEmployeesAsync();
        }
        private void RefreshSupervisors()
        {
            serviceClient.GetEmployeeManagerCompleted += (s, e) =>
            {
                Supervisors = e.Result;
            };
            serviceClient.GetEmployeeManagerAsync();
        }

        #endregion

        #region Button Commands
        public ICommand BTNCalcutatePayableAmount
        {
            get { return new RelayCommand(SetPayableAmount); }
        }
        public ICommand BTNPAY
        {
            get { return new RelayCommand(PayProcess); }
        }

        public ICommand BTNNew
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Methods
        private void FilterGurantorsForLoan()
        {
            IEnumerable<InternalLoanMainView> tempList = AllGurantorsOfLoan.Where(c => c.InternalLoanID == CurrentInternalView.InternalLoanID);
            if (tempList != null && tempList.Count() > 0)
            {
                List<EmployeeSumarryView> TempGuaran = new List<EmployeeSumarryView>();

                foreach (var item in tempList)
                {
                    if (item.GuarantorID == new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        DisableGuarantor = Visibility.Hidden;
                    }
                    else
                    {
                        DisableGuarantor = Visibility.Visible;
                        EmployeeSumarryView temp = new EmployeeSumarryView();
                        temp.epf_no = item.G_epf_no;
                        temp.nic = item.G_nic;
                        temp.first_name = item.G_first_name;
                        temp.surname = item.G_surname;
                        TempGuaran.Add(temp);
                    }
                }

                SelectedGurantorGrid = null;
                SelectedGurantorGrid = TempGuaran;
            }
        }
        private void New()
        {
            RefreshInternalView();
            SelectedGurantorGrid = null;
            RefreshGurontors();
            RefreshUserEmployee();
            RefreshSupervisors();
            PayingAmount = 0;
            PaidAmount = 0;
            IsGuarantor = false;
            GPayThroughNewLoan = false;
            PayAmtValidate = false;
            DisableGuarantor = Visibility.Visible;

        }
        private void SetPayableAmount()
        {
            PayingAmount = CurrentInternalView.LoanRemainingAmount;
            PaidAmount = CalculatePayableAmount();
        }
        private decimal CalculatePayableAmount()
        {
            decimal Total = 0;
            decimal IntrestAmount = (PayingAmount * CurrentInternalView.LoanIntrestRate) / 100;
            Total = PayingAmount + IntrestAmount;
            return Math.Round(Total, 2);
        }

        //private void SetGuarontorToPay()
        //{
        //    Name = null;
        //    EPF = null;
        //    NIC = null;
        //    PaidAmount = 0;
        //    Name = CurrentSelectedGurantorGrid.first_name;
        //    EPF = CurrentSelectedGurantorGrid.epf_no;
        //    NIC= CurrentSelectedGurantorGrid.nic;
        //    IsGuarantor = true;
        //    PayAmtValidate = true;
        //}

        //private void SetEmployeeToPay()
        //{
        //    Name = null;
        //    EPF = null;
        //    NIC = null;
        //    PaidAmount = 0;
        //    Name = CurrentInternalView.first_name;
        //    EPF = CurrentInternalView.epf_no;
        //    NIC = CurrentInternalView.nic;

        //    IsGuarantor = false;
        //    PayAmtValidate = false;
        //    PayingAmount = CurrentInternalView.LoanRemainingAmount;
        //    SetPayableAmount();
        //}
        private void PayProcess()
        {

            if (clsSecurity.GetSavePermission(605))
            {
                if (ValidatePayProcess())
                {
                    clsMessages.setMessage("Do You Want To Save This Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        List<trns_ManualLoanPayment> AddManualPayment = new List<trns_ManualLoanPayment>();
                        if (IsGuarantor)
                        {
                            int GuarantorCount = AllGurantorsOfLoan.Where(c => c.InternalLoanID == CurrentInternalView.InternalLoanID).Count();
                            decimal RemainingAmount = Math.Round(CurrentInternalView.LoanRemainingAmount, 2);
                            decimal PerGuarantorAmount = Math.Round((RemainingAmount / GuarantorCount), 2);
                            IEnumerable<InternalLoanMainView> GuarantorList = AllGurantorsOfLoan.Where(c => c.InternalLoanID == CurrentInternalView.InternalLoanID);
                            foreach (var item in GuarantorList)
                            {
                                trns_ManualLoanPayment temp = new trns_ManualLoanPayment();
                                temp.Employee_ID = item.employee_id;
                                temp.InternalLoanID = CurrentInternalView.InternalLoanID;
                                temp.IntrestRate = CurrentInternalView.LoanIntrestRate;
                                temp.Is_Guarantor = IsGuarantor;
                                temp.GuarantorPayThroughNewLoan = GPayThroughNewLoan;
                                temp.payingAmountWithoutIntrest = PerGuarantorAmount;
                                temp.PayedAmountWithIntrest = Math.Round((PerGuarantorAmount * CurrentInternalView.LoanIntrestRate) / 100, 2) + PerGuarantorAmount;
                                temp.save_datetime = DateTime.Now;
                                temp.Comments = Comments;
                                temp.PayedDate = DateTime.Now;
                                temp.save_user_id = clsSecurity.loggedUser.user_id;
                                // temp.mas_InternalLoanDetails.InternalLoanID = CurrentInternalView.InternalLoanID;
                                AddManualPayment.Add(temp);
                            }
                        }
                        else if (GPayThroughNewLoan)
                        {
                            trns_ManualLoanPayment temp = new trns_ManualLoanPayment();
                            temp.Employee_ID = CurrentInternalView.employee_id;
                            temp.InternalLoanID = CurrentInternalView.InternalLoanID;
                            temp.IntrestRate = CurrentInternalView.LoanIntrestRate;
                            temp.Is_Guarantor = IsGuarantor;
                            temp.payingAmountWithoutIntrest = PayingAmount;
                            temp.GuarantorPayThroughNewLoan = GPayThroughNewLoan;
                            temp.PayedAmountWithIntrest = PaidAmount;
                            temp.save_datetime = DateTime.Now;
                            temp.Comments = Comments;
                            temp.PayedDate = DateTime.Now;
                            temp.save_user_id = clsSecurity.loggedUser.user_id;
                            AddManualPayment.Add(temp);
                        }
                        else if (IsGuarantor == false && GPayThroughNewLoan == false)
                        {
                            trns_ManualLoanPayment temp = new trns_ManualLoanPayment();
                            temp.Employee_ID = CurrentInternalView.employee_id;
                            temp.InternalLoanID = CurrentInternalView.InternalLoanID;
                            temp.IntrestRate = CurrentInternalView.LoanIntrestRate;
                            temp.Is_Guarantor = IsGuarantor;
                            temp.payingAmountWithoutIntrest = PayingAmount;
                            temp.GuarantorPayThroughNewLoan = GPayThroughNewLoan;
                            temp.PayedAmountWithIntrest = PaidAmount;
                            temp.save_datetime = DateTime.Now;
                            temp.Comments = Comments;
                            temp.PayedDate = DateTime.Now;
                            temp.save_user_id = clsSecurity.loggedUser.user_id;
                            AddManualPayment.Add(temp);
                        }
                        if (serviceClient.SaveManualPayment(AddManualPayment.ToArray()))
                        {
                            New();
                            clsMessages.setMessage("Manual Payment Saved Successfully....");

                        }
                        else
                            clsMessages.setMessage("Manual Payment Failed....");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to pay");
        }
        private dtl_EmployeeSupervisor ValidateSupervisor()
        {
            Guid? CurrentLoggedUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
            CurrentSupervisors = Supervisors.FirstOrDefault(c => c.supervisor_employee_id == CurrentLoggedUserEmployee && c.employee_id == CurrentInternalView.employee_id && c.module_id == new Guid("9A8922B9-BDFE-4198-904C-A19BCAAFB5EB"));
            return CurrentSupervisors;
        }
        private bool ValidatePayProcess()
        {
            if (CurrentInternalView == null || CurrentInternalView.InternalLoanID == 0 || CurrentInternalView.InternalLoanID == null)
            {
                clsMessages.setMessage("Please Select A Loan to Process The Task...");
                return false;
            }
            else if (ValidateSupervisor() == null)
            {
                clsMessages.setMessage("Your Not a Supervisor to This Employee To Complete The Process...");
                return false;
            }
            else if (IsGuarantor == false && GPayThroughNewLoan == false)
            {
                //if (GetPaidLoanAmountPercentage() < 75)
                //{
                //    clsMessages.setMessage("Employee Cannot Manually settle The Loan Because The Employee Has Not Paid 75% Of The Loan...");
                //    return false;
                //}
                return true;
            }
            else
                return true;
        }
        private decimal GetPaidLoanAmountPercentage()
        {
            decimal PayedAmount = Math.Round(CurrentInternalView.LoanTotalAmount - CurrentInternalView.LoanRemainingAmount, 2);
            decimal Value = Math.Round((PayedAmount / CurrentInternalView.LoanTotalAmount) * 100, 2);
            return Value;
        }

        private void FilterSearch()
        {
            InternalLoanView = InternalLoanView.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        }

        #endregion
    }
}
