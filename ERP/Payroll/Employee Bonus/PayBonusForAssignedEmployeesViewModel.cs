using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Payroll.Employee_Bonus
{
    class PayBonusForAssignedEmployeesViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<AsignedEmployeesForBonusByBonusPeriodView> AllAlreadyAsignedEmployeesForBonus;
        List<EmployeeBonusProcessView> AllSelectedBonusEmployees;
        List<EmployeeBonusProcessView> AllRemovedBonusEmployees;
        #endregion

        #region Constructor

        public PayBonusForAssignedEmployeesViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Properties

        private IEnumerable<z_BonusPeriod> _BonusPeriod;
        public IEnumerable<z_BonusPeriod> BonusPeriod
        {
            get { return _BonusPeriod; }
            set { _BonusPeriod = value; OnPropertyChanged("BonusPeriod"); }
        }

        private z_BonusPeriod _CurrentBonusPeriod;
        public z_BonusPeriod CurrentBonusPeriod
        {
            get { return _CurrentBonusPeriod; }
            set { _CurrentBonusPeriod = value; OnPropertyChanged("CurrentBonusPeriod"); if (CurrentBonusPeriod != null)FilterBonusDetailsByPeriod(); }
        }

        private IEnumerable<AsignedEmployeesForBonusByBonusPeriodView> _AlreadyAsignedEmployeesForBonus;
        public IEnumerable<AsignedEmployeesForBonusByBonusPeriodView> AlreadyAsignedEmployeesForBonus
        {
            get { return _AlreadyAsignedEmployeesForBonus; }
            set { _AlreadyAsignedEmployeesForBonus = value; OnPropertyChanged("AlreadyAsignedEmployeesForBonus"); }
        }

        private AsignedEmployeesForBonusByBonusPeriodView _CurrentAlreadyAsignedEmployeeForBonus;
        public AsignedEmployeesForBonusByBonusPeriodView CurrentAlreadyAsignedEmployeeForBonus
        {
            get { return _CurrentAlreadyAsignedEmployeeForBonus; }
            set { _CurrentAlreadyAsignedEmployeeForBonus = value; OnPropertyChanged("CurrentAlreadyAsignedEmployeeForBonus"); if (CurrentAlreadyAsignedEmployeeForBonus != null)FilterBonusDetailsByPeriod(); }
        }

        private IEnumerable<GetEmployeePaymentPeriodDateAndEmployeeWiseView> _SelectedEmployeesSalaryDetails;
        public IEnumerable<GetEmployeePaymentPeriodDateAndEmployeeWiseView> SelectedEmployeesSalaryDetails
        {
            get { return _SelectedEmployeesSalaryDetails; }
            set { _SelectedEmployeesSalaryDetails = value; OnPropertyChanged("SelectedEmployeesSalaryDetails"); }
        }

        private IEnumerable<EmployeeBonusProcessView> _EmployeeBonusWithAverageGrossSalary;
        public IEnumerable<EmployeeBonusProcessView> EmployeeBonusWithAverageGrossSalary
        {
            get { return _EmployeeBonusWithAverageGrossSalary; }
            set { _EmployeeBonusWithAverageGrossSalary = value; OnPropertyChanged("EmployeeBonusWithAverageGrossSalary"); }
        }

        private EmployeeBonusProcessView _CurrentEmployeeBonusWithAverageGrossSalary;
        public EmployeeBonusProcessView CurrentEmployeeBonusWithAverageGrossSalary
        {
            get { return _CurrentEmployeeBonusWithAverageGrossSalary; }
            set { _CurrentEmployeeBonusWithAverageGrossSalary = value; OnPropertyChanged("CurrentEmployeeBonusWithAverageGrossSalary"); if (CurrentEmployeeBonusWithAverageGrossSalary != null)ChangeAverageOFGrossAndPayrollCount(); }
        }

        private IList _CurrentEmployeeBonusWithAverageGrossSalaryList = new ArrayList();
        public IList CurrentEmployeeBonusWithAverageGrossSalaryList
        {
            get { return _CurrentEmployeeBonusWithAverageGrossSalaryList; }
            set { _CurrentEmployeeBonusWithAverageGrossSalaryList = value; OnPropertyChanged("CurrentEmployeeBonusWithAverageGrossSalaryList"); }
        }

        private IEnumerable<EmployeeBonusProcessView> _RemovedEmployeeBonusWithAverageGrossSalary;
        public IEnumerable<EmployeeBonusProcessView> RemovedEmployeeBonusWithAverageGrossSalary
        {
            get { return _RemovedEmployeeBonusWithAverageGrossSalary; }
            set { _RemovedEmployeeBonusWithAverageGrossSalary = value; OnPropertyChanged("RemovedEmployeeBonusWithAverageGrossSalary"); }
        }

        private IList _RemovedEmployeeBonusWithAverageGrossSalaryList = new ArrayList();
        public IList RemovedEmployeeBonusWithAverageGrossSalaryList
        {
            get { return _RemovedEmployeeBonusWithAverageGrossSalaryList; }
            set { _RemovedEmployeeBonusWithAverageGrossSalaryList = value; OnPropertyChanged("RemovedEmployeeBonusWithAverageGrossSalaryList"); }
        }

        private decimal _GrossSalary;
        public decimal GrossSalary
        {
            get { return _GrossSalary; }
            set { _GrossSalary = value; OnPropertyChanged("GrossSalary"); }
        }

        private int _PayrollRunCount;
        public int PayrollRunCount
        {
            get { return _PayrollRunCount; }
            set { _PayrollRunCount = value; OnPropertyChanged("PayrollRunCount"); }
        }

        private bool _IsPayeeDeduction;
        public bool IsPayeeDeduction
        {
            get { return _IsPayeeDeduction; }
            set
            {
                _IsPayeeDeduction = value;
                OnPropertyChanged("IsPayeeDeduction");
                if (IsPayeeDeduction == true)
                {
                    IsDisabled = Visibility.Visible;
                }
                else
                    IsDisabled = Visibility.Hidden;
            }
        }

        private Visibility _IsDisabled;
        public Visibility IsDisabled
        {
            get { return _IsDisabled; }
            set { _IsDisabled = value; OnPropertyChanged("IsDisabled"); }
        }


        #endregion

        #region Refresh
        private void RefreshBonusPeriod()
        {
            serviceClient.GetBonusPeriodCompleted += (s, e) =>
            {
                BonusPeriod = e.Result;
            };
            serviceClient.GetBonusPeriodAsync();
        }
        private void RefreshAlreadyAsigedEmployeesForBonus()
        {
            serviceClient.GetAsignedEmployeesForBounsDetailsCompleted += (s, e) =>
            {
                if (e.Result != null && e.Result.Count() > 0)
                {
                    AllAlreadyAsignedEmployeesForBonus = e.Result.Where(c => c.Is_Processed == false).ToList();
                }
            };
            serviceClient.GetAsignedEmployeesForBounsDetailsAsync();
        }

        #endregion

        #region Button Commands
        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }
        public ICommand PayButton
        {
            get { return new RelayCommand(Save); }
        }
        public ICommand AddBonusButton
        {
            get { return new RelayCommand(AddBonus); }
        }
        public ICommand DeleteBonusButton
        {
            get { return new RelayCommand(DeleteBonus); }
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }


        #endregion

        #region Methods
        private void FilterBonusDetailsByPeriod()
        {
            AlreadyAsignedEmployeesForBonus = AllAlreadyAsignedEmployeesForBonus.Where(c => c.Bonus_Period_id == CurrentBonusPeriod.Bonus_Period_id);
            List<EmployeeBonusProcessView> AddAsignedDetailsOfEmployees = new List<EmployeeBonusProcessView>();
            foreach (var item in AlreadyAsignedEmployeesForBonus)
            {
                EmployeeBonusProcessView temp = new EmployeeBonusProcessView();
                temp.employee_id = item.employee_id;
                temp.Employee_Bonus_id = item.Employee_Bonus_id;
                temp.emp_id = item.emp_id;
                temp.first_name = item.first_name;
                temp.surname = item.surname;
                //temp.GrossSalary = FilterEmployeesGrossSalary((Guid)temp.employee_id);
                temp.GrossSalary = FilterEmployeesGrossSalary((Guid)temp.employee_id, Convert.ToInt32(item.Bonus_Period_id));
                temp.PayrollMonths = FilterEmployeesPayrollCount((Guid)temp.employee_id);
                temp.epf_no = item.epf_no;
                temp.nic = item.nic;
                temp.Bonus_Period_id = item.Bonus_Period_id;
                temp.save_datetime = DateTime.Now;
                temp.save_user_id = clsSecurity.loggedUser.user_id;
                temp.PayedBonusAmount = item.BonusAmount;
                AddAsignedDetailsOfEmployees.Add(temp);
            }
            EmployeeBonusWithAverageGrossSalary = AddAsignedDetailsOfEmployees;

        }
        private decimal FilterEmployeesGrossSalary(Guid EmployeeID, int period_id)
        {
            decimal TotGrossSalary1 = 0;
            decimal GrossSalary1 = 0;
            //int PayrollCount = 0;
            //SelectedEmployeesSalaryDetails = serviceClient.GetEmployeePaymentPeriodDateAndEmployeeWise(EmployeeID, (DateTime)CurrentBonusPeriod.Bonus_Period_Start_Date, (DateTime)CurrentBonusPeriod.Bonus_Period_End_Date);
            //PayrollCount = SelectedEmployeesSalaryDetails.Count();
            //if (PayrollCount > 0)
            //{
            //GrossSalary1 = (((decimal)SelectedEmployeesSalaryDetails.Sum(c => c.grossSalary)) / PayrollCount);
            TotGrossSalary1 = (decimal)AllAlreadyAsignedEmployeesForBonus.FirstOrDefault(c => c.employee_id == EmployeeID && c.Bonus_Period_id == period_id).BonusAmount;
            GrossSalary1 = TotGrossSalary1 * 12;
            //}
            return GrossSalary1;

        }
        private int FilterEmployeesPayrollCount(Guid EmployeeID)
        {
            int PayrollCount = 0;
            //SelectedEmployeesSalaryDetails = serviceClient.GetEmployeePaymentPeriodDateAndEmployeeWise(EmployeeID, (DateTime)CurrentBonusPeriod.Bonus_Period_Start_Date, (DateTime)CurrentBonusPeriod.Bonus_Period_End_Date);
            //PayrollCount = SelectedEmployeesSalaryDetails.Count();
            PayrollCount = 12;
            return PayrollCount;
        }
        private void ChangeAverageOFGrossAndPayrollCount()
        {

            GrossSalary = (decimal)CurrentEmployeeBonusWithAverageGrossSalary.GrossSalary;
            PayrollRunCount = (int)CurrentEmployeeBonusWithAverageGrossSalary.PayrollMonths;

        }
        private void Add()
        {
            if (CurrentEmployeeBonusWithAverageGrossSalary == null || CurrentEmployeeBonusWithAverageGrossSalary.Employee_Bonus_id == 0)
            {
                clsMessages.setMessage("Please Select A Employee Before You Click Add Button...");
            }
            else
            {
                foreach (var item in EmployeeBonusWithAverageGrossSalary.Where(c => c.employee_id == CurrentEmployeeBonusWithAverageGrossSalary.employee_id && c.Employee_Bonus_id == CurrentEmployeeBonusWithAverageGrossSalary.Employee_Bonus_id))
                {
                    item.GrossSalary = GrossSalary;
                    item.PayrollMonths = PayrollRunCount;
                }
                clsMessages.setMessage("Value Has Been Changed...");
                GrossSalary = 0;
                PayrollRunCount = 0;
            }
        }
        private void Save()
        {
            if (clsSecurity.GetSavePermission(519))
            {
                List<EmployeeBonusProcessView> tempSaveObjList = new List<EmployeeBonusProcessView>();
                tempSaveObjList = EmployeeBonusWithAverageGrossSalary.ToList();
                if (serviceClient.SaveEmployeeBonusProcess(tempSaveObjList.ToArray(), IsPayeeDeduction))
                {
                    New();
                    clsMessages.setMessage("Employee Bonus Process Completed Successfully...");
                }
                else
                    clsMessages.setMessage("Employee Bonus Process Failed..."); 
            }
            else
                clsMessages.setMessage("You don't have permission to process"); 
        }
        private void AddBonus()
        {
            if (RemovedEmployeeBonusWithAverageGrossSalaryList.Count > 0)
            {
                foreach (EmployeeBonusProcessView item in RemovedEmployeeBonusWithAverageGrossSalaryList)
                {
                    AllRemovedBonusEmployees.Remove(item);
                    AllSelectedBonusEmployees.Add(item);
                }
                EmployeeBonusWithAverageGrossSalary = null;
                RemovedEmployeeBonusWithAverageGrossSalary = null;
                EmployeeBonusWithAverageGrossSalary = AllSelectedBonusEmployees;
                RemovedEmployeeBonusWithAverageGrossSalary = AllRemovedBonusEmployees;
            }
        }
        private void DeleteBonus()
        {
            if (clsSecurity.GetDeletePermission(519))
            {
                if (CurrentEmployeeBonusWithAverageGrossSalaryList.Count > 0)
                {
                    AllSelectedBonusEmployees = EmployeeBonusWithAverageGrossSalary.ToList();
                    foreach (EmployeeBonusProcessView item in CurrentEmployeeBonusWithAverageGrossSalaryList)
                    {
                        AllSelectedBonusEmployees.Remove(item);
                        AllRemovedBonusEmployees.Add(item);
                    }
                    EmployeeBonusWithAverageGrossSalary = null;
                    RemovedEmployeeBonusWithAverageGrossSalary = null;
                    EmployeeBonusWithAverageGrossSalary = AllSelectedBonusEmployees;
                    RemovedEmployeeBonusWithAverageGrossSalary = AllRemovedBonusEmployees;

                } 
            }
            else
                clsMessages.setMessage("You don't have permisiion to delete this record"); 
        }
        private void New()
        {
            RemovedEmployeeBonusWithAverageGrossSalary = null;
            EmployeeBonusWithAverageGrossSalary = null;
            RefreshBonusPeriod();
            AllAlreadyAsignedEmployeesForBonus = new List<AsignedEmployeesForBonusByBonusPeriodView>();
            AllSelectedBonusEmployees = new List<EmployeeBonusProcessView>();
            AllRemovedBonusEmployees = new List<EmployeeBonusProcessView>();
            RefreshAlreadyAsigedEmployeesForBonus();
            IsPayeeDeduction = true;
        }

        #endregion
    }
}
