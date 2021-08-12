using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Forms;
using System.Windows.Input;

namespace ERP.Payroll.RI_Allowance
{
    class AllowanceTransactionViewModel:ViewModelBase
    {
        #region Search List
        private List<MonthlyAllowanceTransactionView> listTrans = new List<MonthlyAllowanceTransactionView>();
        #endregion
        
        #region Service Client

        ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public AllowanceTransactionViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshAllowancePeriod();
            RefreshAllowanceType();
            RefreshAllowanceTransactions();
            LoadToCombo();
        }
        #endregion

        #region Properties
        private IEnumerable<z_Period> allowancePeriods;
        public IEnumerable<z_Period> AllowancePeriods
        {
            get { return allowancePeriods; }
            set { allowancePeriods = value; OnPropertyChanged("AllowancePeriods"); }
        }

        private z_Period selectedPeriod;
        public z_Period SelectedPeriod
        {
            get { return selectedPeriod; }
            set { selectedPeriod = value; OnPropertyChanged("SelectedPeriod"); }
        }

        private IEnumerable<mas_Allowance> allowanceTypes;
        public IEnumerable<mas_Allowance> AllowanceTypes
        {
            get { return allowanceTypes; }
            set { allowanceTypes = value; OnPropertyChanged("AllowanceTypes"); }
        }

        private mas_Allowance selectedAllowanceType;
        public mas_Allowance SelectedAllowanceType
        {
            get { return selectedAllowanceType; }
            set { selectedAllowanceType = value; OnPropertyChanged("SelectedAllowanceType"); this.SearchByAllowance(); }
        }

        private IEnumerable<MonthlyAllowanceTransactionView> currentEmployees;
        public IEnumerable<MonthlyAllowanceTransactionView> CurrentEmployees
        {
            get { return currentEmployees; }
            set { currentEmployees = value; OnPropertyChanged("CurrentEmployees"); }
        }

        #region datagrid 1
        private IEnumerable<MonthlyAllowanceTransactionView> currentTransactions;
        public IEnumerable<MonthlyAllowanceTransactionView> CurrentTransactions
        {
            get { return currentTransactions; }
            set
            {
                currentTransactions = value; OnPropertyChanged("CurrentTransactions");
            }
        }

        private MonthlyAllowanceTransactionView currentTransaction;
        public MonthlyAllowanceTransactionView CurrentTransaction
        {
            get { return currentTransaction; }
            set { currentTransaction = value; OnPropertyChanged("CurrentTransaction"); }
        }

        #endregion

        #region datagrid 2

        #region List to keep selected employees
        List<MonthlyAllowanceTransactionView> listSelected = new List<MonthlyAllowanceTransactionView>();
        #endregion

        private IEnumerable<MonthlyAllowanceTransactionView> selectTransactions;
        public IEnumerable<MonthlyAllowanceTransactionView> SelectTransactions
        {
            get { return selectTransactions; }
            set { selectTransactions = value; OnPropertyChanged("SelectTransactions"); }
        }

        private MonthlyAllowanceTransactionView currentSelectTransaction;
        public MonthlyAllowanceTransactionView CurrentSelectTransaction
        {
            get { return currentSelectTransaction; }
            set { currentSelectTransaction = value; OnPropertyChanged("CurrentSelectTransaction"); }
        }

        #endregion
        #endregion

        #region Refresh Methods

        private void RefreshAllowancePeriod()
        {
            try
            {
                serviceClient.GetAllowancePeriodCompleted += (s, e) => {
                    AllowancePeriods = e.Result;
                };
                serviceClient.GetAllowancePeriodAsync();
                
            }
            catch (Exception)
            {
                MessageBox.Show("Allowance period refresh failed", "Refresh Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshAllowanceType()
        {
            try
            {
                serviceClient.GetAllowanceTypesCompleted += (s, e) => {
                    AllowanceTypes = e.Result;
                };
                serviceClient.GetAllowanceTypesAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void RefreshAllowanceTransactions()
        {
            try
            {
                //serviceClient.GetEmployeeCurrentAllowancesCompleted += (s, e) =>
                //{
                   // CurrentTransactions = e.Result;
                //};
                //serviceClient.GetEmployeeCurrentAllowancesAsync();
                CurrentTransactions = serviceClient.GetEmployeeCurrentAllowances();
                if (CurrentTransactions != null)
                    listTrans = CurrentTransactions.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Transaction refresh failed");
            }
        }

        private void LoadToCombo()
        {
            CurrentEmployees = listTrans.GroupBy(c => c.employee_id).Select(d => d.FirstOrDefault());
        }
        #endregion

        #region Buttton Methods

        #region Process Method
        //private void Process()
        //{
        //    try
        //    {
        //        if (SelectedPeriod != null && SelectTransactions != null)
        //        {
        //            List<trn_EmployeeMonthlyAllowance> processedTrans = new List<trn_EmployeeMonthlyAllowance>();
        //            DateTime? start = SelectedPeriod.start_date;
        //            DateTime? end = SelectedPeriod.end_date;
        //            List<MonthlyAllowanceTransactionView> employeeAllowances = SelectTransactions.ToList();
                    
        //            if (employeeAllowances.Count > 0)
        //            {
        //                foreach (MonthlyAllowanceTransactionView tran in employeeAllowances)
        //                {
        //                    if (tran.PAID_FOR != null)
        //                    {
        //                        if (tran.PAID_FOR.Value.Month < start.Value.Month)
        //                        {
        //                            int difference = start.Value.Month - tran.PAID_FOR.Value.Month;
        //                            for (int i = difference; i > 0; i--)
        //                            {
        //                                trn_EmployeeMonthlyAllowance allowance = new trn_EmployeeMonthlyAllowance();
        //                                allowance.allowance_id = tran.allowance_id;
        //                                allowance.employee_id = tran.employee_id;
        //                                allowance.period_id = SelectedPeriod.period_id;
        //                                allowance.amount = tran.amount;
        //                                allowance.paid_for = start.Value.Date.AddMonths(-i + 1);
        //                                allowance.is_processed = true;
        //                                processedTrans.Add(allowance);
        //                            }

        //                        }
        //                    }
        //                    else
        //                    {
        //                        trn_EmployeeMonthlyAllowance allowance = new trn_EmployeeMonthlyAllowance();
        //                        allowance.allowance_id = tran.allowance_id;
        //                        allowance.employee_id = tran.employee_id;
        //                        allowance.period_id = SelectedPeriod.period_id;
        //                        allowance.amount = tran.amount;
        //                        allowance.paid_for = start;
        //                        allowance.is_processed = true;
        //                        processedTrans.Add(allowance);
        //                    }
        //                }

        //                if (serviceClient.SaveAllowanceTransactions(processedTrans.ToArray()))
        //                {
        //                    MessageBox.Show("Transactions are saved");
        //                    SelectTransactions = null;
        //                    RefreshAllowanceTransactions();
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Save failed");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        private void Process()
        {
            try
            {
                if (SelectedPeriod != null && SelectTransactions != null)
                {
                    List<trn_EmployeeMonthlyAllowance> processedTrans = new List<trn_EmployeeMonthlyAllowance>();
                    List<MonthlyAllowanceTransactionView> employeeAllowances = SelectTransactions.ToList();

                    if (employeeAllowances.Count > 0)
                    {
                        foreach (MonthlyAllowanceTransactionView tran in employeeAllowances)
                        {
                            trn_EmployeeMonthlyAllowance allowance = new trn_EmployeeMonthlyAllowance();
                            allowance.allowance_id = tran.allowance_id;
                            allowance.employee_id = tran.employee_id;
                            allowance.period_id = SelectedPeriod.period_id;
                            allowance.amount = tran.amount;
                            allowance.paid_for = DateTime.Today;
                            allowance.is_processed = true;
                            processedTrans.Add(allowance);
                        }
                    }

                    if (serviceClient.SaveAllowanceTransactions(processedTrans.ToArray()))
                    {
                        MessageBox.Show("Transactions are saved");
                        SelectTransactions = null;
                        RefreshAllowanceTransactions();
                    }
                    else
                    {
                        MessageBox.Show("Save failed");
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        private bool ProcessCanExecute()
        {
            if (SelectedPeriod == null)
                return false;
            if (SelectTransactions == null)
                return false;

            return true;
        }

        public ICommand ProcessButton
        {
            get { return new RelayCommand(Process, ProcessCanExecute); }
        }
        #endregion

        #region Add button

        private void Add()
        {
            try
            {
                if (CurrentTransaction != null)
                {
                    if (listSelected.Count(c => c.allowance_id == CurrentTransaction.allowance_id && c.emp_id == CurrentTransaction.emp_id) == 0)
                        listSelected.Add(CurrentTransaction);
                    SelectTransactions = null;
                    SelectTransactions = listSelected;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }
        }

        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }
        #endregion

        #region Add All button

        private void AddAll()
        {
            if (CurrentTransactions != null)
            {
                foreach (MonthlyAllowanceTransactionView current in CurrentTransactions)
                {
                    if (SelectTransactions != null)
                    {
                        if (SelectTransactions.Count(c => c.emp_id == current.emp_id && c.allowance_id == current.allowance_id) == 0)
                            listSelected.Add(current);
                    }
                    else
                    {
                        listSelected = CurrentTransactions.ToList();
                    }
                }
            }
            SelectTransactions = null;
            SelectTransactions = listSelected;
        }

        public ICommand AddAllButton
        {
            get{return new RelayCommand(AddAll);}
        }
        #endregion

        #region Remove Button

        private void Remove()
        {
            if (CurrentSelectTransaction != null)
            {
                listSelected.Remove(CurrentSelectTransaction);
                SelectTransactions = null;
                SelectTransactions = listSelected;
            }
        }

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove); }
        }
        #endregion

        #region Remove All
        private void RemoveAll()
        {
            listSelected.Clear();
            SelectTransactions = null;
        }

        public ICommand RemoveAllButton
        {
            get { return new RelayCommand(RemoveAll); }
        }
        #endregion
        #endregion

        #region Search Process

        private void SearchByAllowance()
        {
            CurrentTransactions = null;
            CurrentTransactions = listTrans;
            if (SelectedAllowanceType != null )
            {
                CurrentTransactions = CurrentTransactions.Where(c => c.allowance_id == SelectedAllowanceType.allowance_id);
            }
        }
        #endregion
    }
}
