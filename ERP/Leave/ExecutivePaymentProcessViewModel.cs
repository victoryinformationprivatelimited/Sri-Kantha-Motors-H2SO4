using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using ERP.AttendanceService;
using System.Windows.Input;
using System.Collections;
using System.Windows.Documents;
namespace ERP.Leave
{
    class ExecutivePaymentProcessViewModel : ViewModelBase
    {
        #region Fields

        AttendanceServiceClient AttSvcClient;
        ERPServiceClient ErpSvcClient;
        IList selectEmpList = new ArrayList();
        List<EmployeeLeiuLeaveView> allEmployeeList = new List<EmployeeLeiuLeaveView>();
        List<EmployeeLeiuLeaveView> unselectedEmployeeList = new List<EmployeeLeiuLeaveView>();
        List<EmployeeLeiuLeaveView> selectedEmployeeList = new List<EmployeeLeiuLeaveView>();
        List<trns_LeiuLeavePayment> trnspaylist;

        #endregion

        #region Constructor

        public ExecutivePaymentProcessViewModel()
        {
            AttSvcClient = new AttendanceServiceClient();
            ErpSvcClient = new ERPServiceClient();
            RefreshExecutivePaymentsDetails();
            RefreshPeriod();
            RefreshBasicSalaries();
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeLeiuLeaveView> _SelectExecutivePayments;
        public IEnumerable<EmployeeLeiuLeaveView> SelectExecutivePayments
        {
            get { return _SelectExecutivePayments; }
            set { _SelectExecutivePayments = value; OnPropertyChanged("SelectExecutivePayments"); }
        }

        private IEnumerable<EmployeeLeiuLeaveView> _UnselectExecutivePayments;
        public IEnumerable<EmployeeLeiuLeaveView> UnselectExecutivePayments
        {
            get { return _UnselectExecutivePayments; }
            set { _UnselectExecutivePayments = value; OnPropertyChanged("UnselectExecutivePayments"); }
        }

        private IEnumerable<EmployeeLeiuLeaveView> _ExecutivePayments;
        public IEnumerable<EmployeeLeiuLeaveView> ExecutivePayments
        {
            get { return _ExecutivePayments; }
            set { _ExecutivePayments = value; OnPropertyChanged("ExecutivePayments"); }
        }

        private EmployeeLeiuLeaveView _CurrentExecutivePayments;
        public EmployeeLeiuLeaveView CurrentExecutivePayments
        {
            get { return _CurrentExecutivePayments; }
            set { _CurrentExecutivePayments = value; OnPropertyChanged("CurrentExecutivePayments"); }
        }

        private IEnumerable<AttendancePeriodView> _Period;
        public IEnumerable<AttendancePeriodView> Period
        {
            get { return _Period; }
            set { _Period = value; OnPropertyChanged("Period"); }
        }

        private AttendancePeriodView _CurrentPeriod;
        public AttendancePeriodView CurrentPeriod
        {
            get
            {
                return _CurrentPeriod;
            }
            set
            {
                _CurrentPeriod = value;
                OnPropertyChanged("CurrentPeriod");
                if (CurrentPeriod != null)
                    FilterEmployees();
                else
                    ExecutivePayments = null;
            }
        }

        public IList SelectEmpList
        {
            get { return selectEmpList; }
            set { selectEmpList = value; OnPropertyChanged("SelectEmpList"); }
        }

        private IEnumerable<ViewBasicSalary> _EmployeeBasicSalary;
        public IEnumerable<ViewBasicSalary> EmployeeBasicSalary
        {
            get { return _EmployeeBasicSalary; }
            set { _EmployeeBasicSalary = value; OnPropertyChanged("EmployeeBasicSalary"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshExecutivePaymentsDetails()
        {
            AttSvcClient.GetExecutivePaymentDetailsCompleted += (s, e) =>
            {
                try
                {
                    allEmployeeList.Clear();
                    ExecutivePayments = e.Result;
                    if (ExecutivePayments != null && ExecutivePayments.Count() > 0)
                    {
                        allEmployeeList = ExecutivePayments.ToList();
                    }
                    ExecutivePayments = null;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Executive Payments Load is Failed");
                }
            };
            AttSvcClient.GetExecutivePaymentDetailsAsync();
        }

        void RefreshPeriod()
        {
            AttSvcClient.GetAttendancePeriodDetailsCompleted += (s, e) =>
            {
                try
                {
                    Period = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance period details refresh is failed");
                }
            };
            AttSvcClient.GetAttendancePeriodDetailsAsync();
        }

        void RefreshBasicSalaries()
        {
            ErpSvcClient.GetAllBasicSalaryCompleted += (s, e) =>
            {
                try
                {
                    EmployeeBasicSalary = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Basic Salaries Refresh Failed");
                }
            };
            ErpSvcClient.GetAllBasicSalaryAsync();
        }

        #endregion

        #region Commands & Methods

        private void FilterEmployees()
        {
            SelectExecutivePayments = null;
            SelectExecutivePayments = allEmployeeList.Where(c => c.pay_period_id == CurrentPeriod.period_id);
        }

        void Add()
        {
            if (selectEmpList.Count > 0)
            {
                foreach (EmployeeLeiuLeaveView addEmp in selectEmpList)
                {
                    selectedEmployeeList.Add(addEmp);
                    unselectedEmployeeList.Remove(addEmp);
                }

                SelectExecutivePayments = null;
                SelectExecutivePayments = selectedEmployeeList;
                UnselectExecutivePayments = null;
                UnselectExecutivePayments = unselectedEmployeeList;
            }
        }

        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }

        void Remove()
        {
            if (selectEmpList.Count > 0)
            {
                foreach (EmployeeLeiuLeaveView removeEmp in selectEmpList)
                {
                    selectedEmployeeList = SelectExecutivePayments.ToList();
                    unselectedEmployeeList.Add(removeEmp);
                    selectedEmployeeList.Remove(removeEmp);
                }

                SelectExecutivePayments = null;
                SelectExecutivePayments = selectedEmployeeList;
                UnselectExecutivePayments = null;
                UnselectExecutivePayments = unselectedEmployeeList;
            }
        }

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove); }
        }

        void StartProcess()
        {
            try
            {
                if (clsSecurity.GetSavePermission(413))
                {
                    trnspaylist = new List<trns_LeiuLeavePayment>();

                    foreach (var executive in SelectExecutivePayments)
                    {
                        trns_LeiuLeavePayment trnspay = new trns_LeiuLeavePayment();
                        decimal basic_salary = Convert.ToDecimal(EmployeeBasicSalary.FirstOrDefault(c => c.employee_id == executive.employee_id).amount);
                        decimal pay_amount = (basic_salary / 100) * 5;
                        trnspay.employee_id = executive.employee_id;
                        trnspay.attend_date = executive.attend_date;
                        trnspay.amount = pay_amount;
                        trnspay.period_id = executive.pay_period_id;
                        trnspay.save_user_id = clsSecurity.loggedUser.user_id;
                        trnspay.save_datetime = DateTime.Now;
                        trnspaylist.Add(trnspay);


                    }
                    if (AttSvcClient.SaveLeiuLeavePayments(trnspaylist.ToArray()))
                    {
                        clsMessages.setMessage("Executive Payment Process Successfully Completed");
                        SelectExecutivePayments = null;
                        UnselectExecutivePayments = null;
                        RefreshPeriod();
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to process");
            }
            catch (Exception)
            {
                clsMessages.setMessage("Executive Payment Process Failed");
            }
        }

        public ICommand Process
        {
            get { return new RelayCommand(StartProcess); }
        }

        #endregion
    }
}
