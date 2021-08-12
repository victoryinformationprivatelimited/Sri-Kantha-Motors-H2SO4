using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ERP.Loan_Module.Reports;
using System.Windows;
using ERP.Properties;

namespace ERP.Loan_Module.New_LoanForms
{
    class EmployeeNewLoanViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<EmployeeSumarryView> AllEmployees = new List<EmployeeSumarryView>();
        List<InternalLoanMainView> AllGurantorsOfLoan = new List<InternalLoanMainView>();
        List<z_Loan> AllLoan = new List<z_Loan>();
        List<EmployeeSumarryView> SelectedGurantorList = new List<EmployeeSumarryView>();
        CheckEmployeeLoanViewModel CheckEmployeeLoanVM;
        #endregion

        #region Constrouctor
        public EmployeeNewLoanViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshInternalView();
            CurrentInternalView = new InternalLoanWithoutGurantorsView();
            RefreshLoan();
            CurrentLoan = new z_Loan();
            RefreshEmployees();
            CurrentEmployee = new EmployeeSumarryView();
            RefreshGurontors();
            RefreshLoansWithRules();
            //  CurrentInternalLoanWithGurantors = new InternalLoanMainView();
            CurrentSelectedGurantorGrid = new EmployeeSumarryView();
            CheckEmployeeLoanVM = new CheckEmployeeLoanViewModel();
            RefreshInternalLoans();
            isHold = false;
        }
        #endregion

        #region Properties

        private IEnumerable<InternalLoanWithoutGurantorsView> _InternalLoanView;
        public IEnumerable<InternalLoanWithoutGurantorsView> InternalLoanView
        {
            get { return _InternalLoanView; }
            set { _InternalLoanView = value; OnPropertyChanged("InternalLoanView"); }
        }

        private IEnumerable<InternalLoanWithoutGurantorsView> _SelectedInternalLoanView;
        public IEnumerable<InternalLoanWithoutGurantorsView> SelectedInternalLoanView
        {
            get { return _SelectedInternalLoanView; }
            set { _SelectedInternalLoanView = value; OnPropertyChanged("SelectedInternalLoanView"); }
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
                    SetLoanName();

                }
            }
        }

        private EmployeeSearchView _CurrentEmployeesForDialogBox;
        public EmployeeSearchView CurrentEmployeesForDialogBox
        {
            get { return _CurrentEmployeesForDialogBox; }
            set { _CurrentEmployeesForDialogBox = value; OnPropertyChanged("CurrentEmployeesForDialogBox"); if (CurrentEmployeesForDialogBox != null)SetEmployeeDetails(); }
        }

        private IEnumerable<z_Loan> _Loan;
        public IEnumerable<z_Loan> Loan
        {
            get { return _Loan; }
            set { _Loan = value; OnPropertyChanged("Loan"); }
        }

        private z_Loan _CurrentLoan;
        public z_Loan CurrentLoan
        {
            get { return _CurrentLoan; }
            set { _CurrentLoan = value; OnPropertyChanged("CurrentLoan"); if (CurrentLoan != null)SetLoanDetails(); }
        }

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private EmployeeSearchView _CurrentGuarantor1ForDialogBox;
        public EmployeeSearchView CurrentGuarantor1ForDialogBox
        {
            get { return _CurrentGuarantor1ForDialogBox; }
            set { _CurrentGuarantor1ForDialogBox = value; OnPropertyChanged("CurrentGuarantor1ForDialogBox"); if (CurrentGuarantor1ForDialogBox != null) AddGurantor(); }//if (CurrentGuarantor1ForDialogBox != null) SetGuarantor1Details();
        }

        private IEnumerable<InternalLoanMainView> _InternalLoanWithGurantors;
        public IEnumerable<InternalLoanMainView> InternalLoanWithGurantors
        {
            get { return _InternalLoanWithGurantors; }
            set { _InternalLoanWithGurantors = value; OnPropertyChanged("InternalLoanWithGurantors"); }
        }

        private InternalLoanMainView _CurrentInternalLoanWithGurantors;
        public InternalLoanMainView CurrentInternalLoanWithGurantors
        {
            get { return _CurrentInternalLoanWithGurantors; }
            set { _CurrentInternalLoanWithGurantors = value; OnPropertyChanged("CurrentInternalLoanWithGurantors"); }
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

        private IEnumerable<mas_InternalLoanDetails> _InternalLoanDetails;
        public IEnumerable<mas_InternalLoanDetails> InternalLoanDetails
        {
            get { return _InternalLoanDetails; }
            set { _InternalLoanDetails = value; OnPropertyChanged("InternalLoanDetails"); }
        }

        private mas_InternalLoanDetails _CurrentInternalLoanDetails;
        public mas_InternalLoanDetails CurrentInternalLoanDetails
        {
            get { return _CurrentInternalLoanDetails; }
            set { _CurrentInternalLoanDetails = value; OnPropertyChanged("CurrentInternalLoanDetails"); }
        }

        private IEnumerable<EmployeeSumarryView> _GurantorEmployee;
        public IEnumerable<EmployeeSumarryView> GurantorEmployee
        {
            get { return _GurantorEmployee; }
            set { _GurantorEmployee = value; OnPropertyChanged("GurantorEmployee"); }
        }

        private EmployeeSumarryView _CurrentGurantorEmployee;
        public EmployeeSumarryView CurrentGurantorEmployee
        {
            get { return _CurrentGurantorEmployee; }
            set { _CurrentGurantorEmployee = value; OnPropertyChanged("CurrentGurantorEmployee"); }
        }

        private IEnumerable<LoanWithRulesView> _LoansWithRules;
        public IEnumerable<LoanWithRulesView> LoansWithRules
        {
            get { return _LoansWithRules; }
            set { _LoansWithRules = value; OnPropertyChanged("LoansWithRules"); }
        }

        private LoanWithRulesView _CurrentLoansWithRules;
        public LoanWithRulesView CurrentLoansWithRules
        {
            get { return _CurrentLoansWithRules; }
            set { _CurrentLoansWithRules = value; OnPropertyChanged("CurrentLoansWithRules"); }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterEmployeeLoansByEPF(); }
        }

        private bool _isHold;

        public bool isHold
        {
            get { return _isHold; }
            set { _isHold = value; OnPropertyChanged("isHold"); }
        }


        #endregion

        #region Refresh Methods

        private void RefreshInternalView()
        {
            serviceClient.GetInternalBankLoanWithoutGurantorsForMainWindowCompleted += (s, e) =>
                {
                    InternalLoanView = e.Result;
                };
            serviceClient.GetInternalBankLoanWithoutGurantorsForMainWindowAsync();

        }

        private void RefreshInternalLoans()
        {
            serviceClient.GetInternalLoansCompleted += (s, e) =>
            {
                InternalLoanDetails = e.Result;
            };
            serviceClient.GetInternalLoansAsync();

        }

        private void RefreshLoan()
        {
            serviceClient.GetLoansCompleted += (s, e) =>
                {
                    Loan = e.Result;
                    if (Loan != null)
                        AllLoan = Loan.ToList();
                };
            serviceClient.GetLoansAsync();
        }
        private void RefreshEmployees()
        {
            AllEmployees.Clear();
            Employees = null;
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result;
                if (Employees != null)
                {
                    AllEmployees = Employees.ToList();
                }
            };
            serviceClient.GetAllEmployeeDetailAsync();
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
        private void RefreshLoansWithRules()
        {
            serviceClient.GetLoansWithRulesCompleted += (s, e) =>
            {
                LoansWithRules = e.Result;
            };
            serviceClient.GetLoansWithRulesAsync();
        }


        #endregion

        #region Button Commands
        public ICommand SearchEmpButton
        {
            get { return new RelayCommand(searchEmp); }
        }
        public ICommand SearchGuarantor1Button
        {
            get { return new RelayCommand(Guarantor1); }
        }
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        public ICommand DeleteGurantorButton
        {
            get { return new RelayCommand(DeleteGurantor); }
        }
        public ICommand CalInstallmentMntButton
        {
            get { return new RelayCommand(InstallmentMonths); }
        }
        public ICommand PrintPaymentMethodButton
        {
            get { return new RelayCommand(PrintPaymentMethod); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Methods

        #region SearchEmp Open
        void searchEmp()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentEmployeesForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        #endregion
        private void SetEmployeeDetails()
        {
            if (CurrentEmployeesForDialogBox != null)
            {
                CurrentEmployee = null;
                CurrentEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentEmployeesForDialogBox.employee_id);
                CurrentInternalView.employee_id = CurrentEmployee.employee_id;
                CurrentInternalView.epf_no = CurrentEmployee.epf_no;
                CurrentInternalView.first_name = CurrentEmployee.first_name;
                CurrentInternalView.nic = CurrentEmployee.nic;
                CurrentInternalView.surname = CurrentEmployee.surname;
                SelectedInternalLoanView = InternalLoanView.Where(c => c.employee_id == CurrentInternalView.employee_id);

            }
            else
            {
                CurrentEmployee = null;
            }
        }
        #region SearchGuarantor1 Open
        void Guarantor1()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentGuarantor1ForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        #endregion
        private void InstallmentMonths()
        {
            if (ValidateInstallmentDate())
            {
                int months = ((DateTime)CurrentInternalView.LoanEndDate).Year * 12 + ((DateTime)CurrentInternalView.LoanEndDate).Month - (((DateTime)CurrentInternalView.LoanStartDate).Year * 12 + ((DateTime)CurrentInternalView.LoanStartDate).Month);
                CurrentInternalView.InstallmentMonths = months;
            }
        }
        private bool ValidateInstallmentDate()
        {
            if (CurrentInternalView.LoanStartDate == null)
            {
                clsMessages.setMessage("Please Select Start Date To Calculate Loan Installment Months...");
                return false;
            }
            else if (CurrentInternalView.LoanEndDate == null)
            {
                clsMessages.setMessage("Please Select End Date To Calculate Loan Installment Months...");
                return false;
            }
            else if (CurrentInternalView.LoanStartDate > CurrentInternalView.LoanEndDate)
            {
                clsMessages.setMessage("Start Date Should Not Be Greater Than End Date To Calculate Loan Installment Months...");
                return false;
            }

            else
                return true;
        }
        private void FilterGurantorsForLoan()
        {
            IEnumerable<InternalLoanMainView> tempList = AllGurantorsOfLoan.Where(c => c.InternalLoanID == CurrentInternalView.InternalLoanID);
            if (tempList != null && tempList.Count() > 0)
            {
                List<EmployeeSumarryView> TempGuaran = new List<EmployeeSumarryView>();

                foreach (var item in tempList)
                {
                    EmployeeSumarryView temp = new EmployeeSumarryView();
                    temp.epf_no = item.G_epf_no;
                    temp.nic = item.G_nic;
                    temp.first_name = item.G_first_name;
                    temp.surname = item.G_surname;
                    TempGuaran.Add(temp);
                }

                SelectedGurantorGrid = null;
                SelectedGurantorGrid = TempGuaran;
            }
        }
        private void DeleteGurantor()
        {
            if (ValidateDeleteGurantor())
            {
                List<EmployeeSumarryView> Templist = new List<EmployeeSumarryView>();
                Templist = SelectedGurantorGrid.ToList();
                var item = Templist.FirstOrDefault(c => c.employee_id == CurrentSelectedGurantorGrid.employee_id);
                Templist.Remove(item);
                SelectedGurantorList = Templist;
                SelectedGurantorGrid = null;
                SelectedGurantorGrid = Templist;
            }
        }
        private bool ValidateDeleteGurantor()
        {
            if (CurrentSelectedGurantorGrid.employee_id == Guid.Empty)
            {
                clsMessages.setMessage("There Are No Gurantors Selected...");
                return false;
            }
            else
                return true;
        }
        private void Save()
        {
            mas_InternalLoanDetails existLoan = InternalLoanDetails.FirstOrDefault(c => c.InternalLoanID == CurrentInternalView.InternalLoanID);
            if (existLoan == null)
            {
                if (ValidateSave())
                {
                    if (clsSecurity.GetSavePermission(603))
                    {
                        clsMessages.setMessage("Do You Want To Save This Record?", Visibility.Visible);
                        if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                        {
                            CurrentInternalLoanDetails = new mas_InternalLoanDetails();
                            CurrentInternalLoanDetails.employee_id = CurrentInternalView.employee_id;
                            CurrentInternalLoanDetails.loan_id = CurrentLoan.loan_id;
                            CurrentInternalLoanDetails.LoanTotalAmount = CurrentInternalView.LoanTotalAmount;
                            CurrentInternalLoanDetails.LoanIntrestRate = CurrentInternalView.LoanIntrestRate;
                            CurrentInternalLoanDetails.InstallmentMonths = CurrentInternalView.InstallmentMonths;
                            CurrentInternalLoanDetails.LoanStartDate = CurrentInternalView.LoanStartDate;
                            CurrentInternalLoanDetails.LoanEndDate = CurrentInternalView.LoanEndDate;
                            CurrentInternalLoanDetails.is_Completed = false;
                            CurrentInternalLoanDetails.is_delete = false;
                            CurrentInternalLoanDetails.is_active = true;
                            CurrentInternalLoanDetails.save_datetime = DateTime.Now;
                            CurrentInternalLoanDetails.save_user_id = clsSecurity.loggedUser.user_id;
                            CurrentInternalLoanDetails.LoanRemainingAmount = CurrentInternalView.LoanTotalAmount;
                            CurrentInternalLoanDetails.is_Pending = true;
                            CurrentInternalLoanDetails.is_Hold = isHold;
                            List<trns_LoanGuarantorsDetails> GurantorTemp = new List<trns_LoanGuarantorsDetails>();
                            if (GetNoGurantorsInLoan() > 0)
                            {
                                foreach (var Item in SelectedGurantorGrid)
                                {
                                    trns_LoanGuarantorsDetails temp = new trns_LoanGuarantorsDetails();
                                    temp.GuarantorID = Item.employee_id;
                                    GurantorTemp.Add(temp);
                                }
                                CurrentInternalLoanDetails.trns_LoanGuarantorsDetails = GurantorTemp.ToArray();
                                if (serviceClient.SaveInternalLoan(CurrentInternalLoanDetails))
                                {
                                    New();
                                    clsMessages.setMessage("Employees Internal Loan Has Been Saved Successfully...");
                                }
                                else
                                {
                                    clsMessages.setMessage("Employees Internal Loan Save Process Failed...");
                                }
                            }
                            else
                            {
                                trns_LoanGuarantorsDetails temp = new trns_LoanGuarantorsDetails();
                                temp.GuarantorID = Guid.Empty;
                                GurantorTemp.Add(temp);
                                CurrentInternalLoanDetails.trns_LoanGuarantorsDetails = GurantorTemp.ToArray();
                                if (serviceClient.SaveInternalLoan(CurrentInternalLoanDetails))
                                {
                                    New();
                                    clsMessages.setMessage("Employees Internal Loan Has Been Saved Successfully...");
                                }
                                else
                                {
                                    clsMessages.setMessage("Employees Internal Loan Save Process Failed...");
                                }
                            }
                        } 
                    }
                    else
                        clsMessages.setMessage("You don't have permission to save this record");
                }
            }
            else
            {
                if (clsSecurity.GetUpdatePermission(603))
                {
                    existLoan.is_Hold = isHold;
                    existLoan.modified_user_id = clsSecurity.loggedUser.user_id;
                    existLoan.modified_datetime = DateTime.Now;
                    if (serviceClient.UpdateInternalLoan(existLoan))
                    {
                        New();
                        clsMessages.setMessage("Employees Internal Loan Has Been Updated Successfully...");
                    }
                    else
                    {
                        clsMessages.setMessage("Employees Internal Loan Update Process Failed...");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to update this record");
            }
        }
        private bool ValidateSave()
        {
            //if (CurrentInternalView.InternalLoanID != 0)
            //{
            //    clsMessages.setMessage("Employee Internal Loan Cannot Be Updated...");
            //    return false;
            //}
            if (serviceClient.GetSupervisorCount(CurrentInternalView.employee_id, new Guid("9A8922B9-BDFE-4198-904C-A19BCAAFB5EB")) == 0)
            {
                clsMessages.setMessage("Loan Applicant Should Be at least under one Supervisor to Proceed the Loan...");
                return false;
            }
            else if (CurrentInternalView.epf_no == null)
            {
                clsMessages.setMessage("Please Select A Employee From 'Search Employee' Button...");
                return false;
            }
            else if (CurrentLoan.loan_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Loan...");
                return false;
            }
            else if (GetpermenentEmployee() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Applicant is not Fullfilled All The Loan Rules Criteria(Applicant is Not Permenent)...");
                return false;
            }
            else if (GetApplicantServicePeriod() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Applicant is not Fullfilled All The Loan Rules Criteria(Service Period Is Not Greater Than Loan Rule Service Period)...");
                return false;
            }
            else if (GetNoLoansInProgress() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Applicant is not Fullfilled All The Loan Rules Criteria(Active Loans Of The Applicant is Greater Than Loan Rule Active Loans)...");
                return false;
            }
            else if (GetNoGuranteedLoansByApplicant() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Applicant is not Fullfilled All The Loan Rules Criteria(Guranteed Loans By The Applicant Is Greater Than The Loan Rule)...");
                return false;
            }
            // else if (CurrentInternalView.LoanIntrestRate == 0 || CurrentInternalView.LoanIntrestRate == null)
            else if (CurrentInternalView.LoanIntrestRate == null)
            {
                clsMessages.setMessage("Please Enter Loan Intrest Rate...");
                return false;
            }
            else if (CurrentInternalView.LoanTotalAmount == 0 || CurrentInternalView.LoanTotalAmount == null)
            {
                clsMessages.setMessage("Please Enter Loan Total Amount...");
                return false;
            }
            else if ((CurrentLoan.minimum_amount > CurrentInternalView.LoanTotalAmount) || (CurrentInternalView.LoanTotalAmount > CurrentLoan.maximum_amount))
            {
                clsMessages.setMessage("Employee Loan Amount is Not Between Loan Minimum and Maximum Amount... Please Enter Value Between " + CurrentLoan.minimum_amount + " And " + CurrentLoan.maximum_amount + "...");
                CurrentInternalView.LoanTotalAmount = 0;
                return false;
            }
            else if (CurrentInternalView.LoanStartDate == null)
            {
                clsMessages.setMessage("Please Select The Start Date...");
                return false;
            }
            else if (CurrentInternalView.LoanEndDate == null)
            {
                clsMessages.setMessage("Please Select The End Date...");
                return false;
            }
            else if (CurrentInternalView.InstallmentMonths == 0)
            {
                clsMessages.setMessage("Please Click The Calculate 'Installments Months Button' To Get the Installment Months...");
                return false;
            }
            else if ((calculateInstallmentsMinMonthsInLoan() > CurrentInternalView.InstallmentMonths) || (CurrentInternalView.InstallmentMonths > calculateInstallmentsMaxMonthsInLoan()))
            {
                clsMessages.setMessage("Employee Loan Installment Months is Not Equals or Between Loan Minimum and Maximum Amount... Please Enter Value Equals or Between " + calculateInstallmentsMinMonthsInLoan() + " And " + calculateInstallmentsMaxMonthsInLoan() + "...");
                CurrentInternalView.InstallmentMonths = 0;
                CurrentInternalView.LoanStartDate = null;
                CurrentInternalView.LoanEndDate = null;
                return false;
            }
            else if (GetNoGurantorsInLoan() > 0)
            {
                if (GetNoGurantorsInLoan() < SelectedGurantorGrid.Count())
                {
                    clsMessages.setMessage("Number of Gurantors Should Be Greater Than Or Equal To  " + GetNoGurantorsInLoan() + " ");
                    return false;
                }
                else if (SelectedGurantorGrid == null)
                {
                    clsMessages.setMessage("Please Select A Gurantor...");
                    return false;
                }
                else
                    return true;
            }

            else
                return true;

        }
        private void SetLoanName()
        {
            CurrentLoan = null;
            CurrentLoan = AllLoan.FirstOrDefault(c => c.loan_id == CurrentInternalView.loan_id);

        }
        private void AddGurantor()
        {
            if (ValidateAddGurantor())
            {
                CurrentGurantorEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentGuarantor1ForDialogBox.employee_id);
                SelectedGurantorList.Add(CurrentGurantorEmployee);
                SelectedGurantorGrid = null;
                SelectedGurantorGrid = SelectedGurantorList;
            }
        }
        private bool ValidateAddGurantor()
        {
            if (CurrentInternalView.employee_id == null || CurrentInternalView.employee_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select Loan Applicant Before Adding Loan Gurantors...");
                return false;
            }
            else if (CurrentGuarantor1ForDialogBox.employee_id == CurrentInternalView.employee_id)
            {
                clsMessages.setMessage("Loan Applicant And Gurantor are the Same Please Change the Loan Gurantor...");
                return false;
            }
            else if (GetNoGurantorsInLoan() == 0)
            {
                clsMessages.setMessage("This Loan does not need guarantors...");
                return false;
            }
            else if (GetPermenentGurantor() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Guarantor is not Fullfilled All The Loan Rules Criteria(Guarantor is Not Permenent)...");
                return false;
            }
            else if (GetGurantorServicePeriod() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Guarantor is not Fullfilled All The Loan Rules Criteria(Service Period Is Not Greater Than Loan Rule Service Period)...");
                return false;
            }
            else if (GetNoLoansInProgressGurantor() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Guarantor is not Fullfilled All The Loan Rules Criteria(Active Loans Of The Applicant is Greater Than Loan Rule Active Loans)...");
                return false;
            }
            else if (GetNoGuranteedLoansByGuarantor() == false)
            {
                clsMessages.setMessage("Please Check With Loan Eligibility Form... Applicant is not Fullfilled All The Loan Rules Criteria(Guranteed Loans By The Applicant Is Greater Than The Loan Rule)...");
                return false;
            }
            else
                return true;
        }

        #region Report
        private void PrintPaymentMethod()
        {
            int LoanInstallmentMonths = 0;
            decimal LoanRemaining = 0;
            List<LoanInstallment> x2 = new List<LoanInstallment>();
            for (int i = 0; i < CurrentInternalView.InstallmentMonths; i++)
            {

                LoanInstallment temp1 = new LoanInstallment();
                temp1.installmentNo = LoanInstallmentMonths + 1;
                LoanInstallmentMonths = temp1.installmentNo;
                if (CurrentInternalView.InstallmentMonths == temp1.installmentNo)
                {
                    temp1.installment = LoanRemaining / 2;
                }
                else
                {
                    temp1.installment = CurrentInternalView.LoanTotalAmount / CurrentInternalView.InstallmentMonths;
                }
                if (LoanInstallmentMonths == 1)
                {
                    temp1.loanAmount = CurrentInternalView.LoanTotalAmount;
                    LoanRemaining = temp1.loanAmount;
                }
                else
                {
                    temp1.loanAmount = LoanRemaining - temp1.installment;
                    LoanRemaining = temp1.loanAmount;
                }
                temp1.interest = ((LoanRemaining * (CurrentInternalView.LoanIntrestRate / 100)) / 12);
                temp1.EmployeeName = CurrentInternalView.first_name + " " + CurrentInternalView.surname;
                temp1.Epf = CurrentInternalView.epf_no;
                x2.Add(temp1);
            }
            LoanInstallmentDescription report = new LoanInstallmentDescription();
            report.SetDataSource(x2);
            ReportViewer viewer = new ReportViewer(report, false);
            viewer.Show();
        }
        #endregion
        private void New()
        {
            RefreshInternalLoans();
            RefreshInternalView();
            InternalLoanView = null;
            CurrentInternalView = new InternalLoanWithoutGurantorsView();
            RefreshLoan();
            CurrentLoan = new z_Loan();
            RefreshEmployees();
            CurrentEmployee = new EmployeeSumarryView();
            RefreshGurontors();
            SelectedGurantorGrid = null;
            CurrentSelectedGurantorGrid = new EmployeeSumarryView();
            SelectedGurantorList = new List<EmployeeSumarryView>();
            isHold = false;
        }
        private void SetLoanDetails()
        {
            if (CurrentInternalView.InternalLoanID == 0)
                CurrentInternalView.LoanIntrestRate = CurrentLoan.default_rate;
            if (CurrentInternalView.InternalLoanID > 0)
                isHold = (bool)InternalLoanView.FirstOrDefault(c => c.InternalLoanID == CurrentInternalView.InternalLoanID).is_Hold;

        }
        private int calculateInstallmentsMinMonthsInLoan()
        {
            int MinMonth = 0;
            if (CurrentLoan.minimum_time_duration_type == "Y")
            {
                MinMonth = CurrentLoan.minimum_time_duration_period * 12;
                return MinMonth;
            }
            else
            {
                MinMonth = CurrentLoan.minimum_time_duration_period;
                return MinMonth;
            }

        }
        private int calculateInstallmentsMaxMonthsInLoan()
        {
            int MaxMonth = 0;
            if (CurrentLoan.maximum_time_duration_type == "Y")
            {
                MaxMonth = CurrentLoan.maximum_time_duration_period * 12;
                return MaxMonth;
            }
            else
            {
                MaxMonth = CurrentLoan.maximum_time_duration_period;
                return MaxMonth;
            }
        }

        #region Applicant
        private bool GetpermenentEmployee()
        {
            CheckEmployeeLoanVM.CurrentEmployee = CurrentEmployee;
            bool empperment = (bool)CheckEmployeeLoanVM.GetPermenentApplicant();
            bool per = (bool)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Emp_Permanent;
            if (empperment == per || empperment == true)
                return true;
            else
                return false;
        }
        private bool GetApplicantServicePeriod()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentEmployee = CurrentEmployee;
                CheckEmployeeLoanVM.CurrentLoan = LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);
                double ServicePeriodActual = CheckEmployeeLoanVM.GetServicePeriodApplicant();
                double ServicePeriodRule = (double)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Emp_MinServicePeriod;
                if (ServicePeriodActual < ServicePeriodRule)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private bool GetNoLoansInProgress()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentEmployee = CurrentEmployee;
                CheckEmployeeLoanVM.CurrentLoan = LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);
                int LoanCountActual = CheckEmployeeLoanVM.GetActiveLoanCountOFApplicant();
                int LoanCountRule = (int)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Emp_NoLoansInProgress;
                if (LoanCountActual > LoanCountRule)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool GetNoGuranteedLoansByApplicant()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentEmployee = CurrentEmployee;
                CheckEmployeeLoanVM.CurrentLoan = LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);
                int CountActual = CheckEmployeeLoanVM.GetTotalGuranteedLoansByApplicant();
                int CountRule = (int)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Emp_MaxguaranteedLoans;
                if (CountActual > CountRule)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private int GetNoGurantorsInLoan()
        {
            try
            {
                int GurantorCount = (int)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Emp_NoOfGurantors;
                return GurantorCount;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        #endregion

        #region Guarantor
        private bool GetPermenentGurantor()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentGurantor = CurrentGurantorEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentGuarantor1ForDialogBox.employee_id);
                bool Gurantor = (bool)CheckEmployeeLoanVM.GetPermenentGurantor();
                bool PermenentGurantorRule = (bool)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Guarantor_Permanent;
                if (Gurantor == PermenentGurantorRule || Gurantor == true)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool GetGurantorServicePeriod()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentGurantor = CheckEmployeeLoanVM.CurrentGurantor = CurrentGurantorEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentGuarantor1ForDialogBox.employee_id);
                CheckEmployeeLoanVM.CurrentGurontorLoan = LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);
                double ServicePeriodActual = (double)CheckEmployeeLoanVM.GetServicePeriodGurantor();
                double ServicePeriodRule = (double)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Guarantor_MinServicePeriod;
                if (ServicePeriodActual < ServicePeriodRule)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private bool GetNoLoansInProgressGurantor()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentGurantor = CheckEmployeeLoanVM.CurrentGurantor = CurrentGurantorEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentGuarantor1ForDialogBox.employee_id);
                CheckEmployeeLoanVM.CurrentGurontorLoan = LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);
                int LoanCountActual = CheckEmployeeLoanVM.GetActiveLoanCountOFGurantor();
                int LoanCountRule = (int)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Guarantor_NoLoansInProgress;
                if (LoanCountRule >= LoanCountActual)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private bool GetNoGuranteedLoansByGuarantor()
        {
            try
            {
                CheckEmployeeLoanVM.CurrentGurantor = CheckEmployeeLoanVM.CurrentGurantor = CurrentGurantorEmployee = AllEmployees.FirstOrDefault(e => e.employee_id == CurrentGuarantor1ForDialogBox.employee_id);
                CheckEmployeeLoanVM.CurrentGurontorLoan = LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);
                int CountActual = CheckEmployeeLoanVM.GetTotalGuranteedLoansByGurantor();
                int CountRule = (int)LoansWithRules.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id).Guarantor_MaxguaranteedLoans;
                if (CountRule >= CountActual)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        private void FilterEmployeeLoansByEPF()
        {
            InternalLoanView = InternalLoanView.Where(c => c.epf_no != null && c.epf_no.ToUpper().Contains(Search.ToUpper()));
        }

        #endregion
    }


}
