using ERP.BasicSearch;
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

namespace ERP.Leave
{
    class MigrateLeavesViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<EmployeeSearchView> ListEmployees;
        List<Employee_Leave_Detail_Extended_View> ListEmployeeLeaveDetails;
        List<NewLeaveDetails> ListNewLeaveDetails;

        #endregion

        #region Constructor
        public MigrateLeavesViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListEmployees = new List<EmployeeSearchView>();
            ListEmployeeLeaveDetails = new List<Employee_Leave_Detail_Extended_View>();
            ListNewLeaveDetails = new List<NewLeaveDetails>();
            ControllerState = false;
            SearchText = "";
            SearchIndex = 0;

            RefreshEmployees();
            RefreshLeavePeriods();
        }

        #endregion

        #region Properties

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); if (SearchText != null && ListEmployees != null && ListEmployees.Count > 0) FilterEmployees(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private bool _IsTransfer;
        public bool IsTransfer
        {
            get { return _IsTransfer; }
            set { _IsTransfer = value; OnPropertyChanged("IsTransfer"); if (IsTransfer != false)SetMigrationState(); }
        }

        private bool _IsPay;
        public bool IsPay
        {
            get { return _IsPay; }
            set { _IsPay = value; OnPropertyChanged("IsPay"); if (IsPay != false)SetMigrationState(); }
        }

        private bool _None;
        public bool None
        {
            get { return _None; }
            set { _None = value; OnPropertyChanged("None"); if (None != false)SetMigrationState(); }
        }


        private decimal? _Remaining;
        public decimal? Remaining
        {
            get { return _Remaining; }
            set { _Remaining = value; OnPropertyChanged("Remaining"); if (Remaining != null) SetRemainingValues(); }
        }

        private bool _ControllerState;
        public bool ControllerState
        {
            get { return _ControllerState; }
            set { _ControllerState = value; OnPropertyChanged("ControllerState"); }
        }

        private IEnumerable<LeaveDetailMasterView> _LeaveDetail;
        public IEnumerable<LeaveDetailMasterView> LeaveDetail
        {
            get { return _LeaveDetail; }
            set { _LeaveDetail = value; OnPropertyChanged("LeaveDetail"); }
        }

        private LeaveDetailMasterView _CurrentLeaveDetail;
        public LeaveDetailMasterView CurrentLeaveDetail
        {
            get { return _CurrentLeaveDetail; }
            set { _CurrentLeaveDetail = value; OnPropertyChanged("CurrentLeaveDetail"); }
        }


        private IEnumerable<z_LeavePeriod> _LeavePeriods;
        public IEnumerable<z_LeavePeriod> LeavePeriods
        {
            get
            {
                return _LeavePeriods;
            }
            set
            {
                _LeavePeriods = value;
                OnPropertyChanged("LeavePeriods");
            }
        }

        private z_LeavePeriod _CurrentFromLeavePeriod;
        public z_LeavePeriod CurrentFromLeavePeriod
        {
            get
            {
                return _CurrentFromLeavePeriod;
            }
            set
            {
                _CurrentFromLeavePeriod = value;
                OnPropertyChanged("CurrentFromLeavePeriod");
                ControllerState = false;
                Employees = null;
                EmployeeMaxLeaveDetails = null;
                EmployeeLeaveDetails = null;
                ListEmployeeLeaveDetails.Clear();
                LeaveDetail = null;
                IsPay = false;
                IsTransfer = false;
                None = false;
                Remaining = null;
            }
        }

        private z_LeavePeriod _CurrentToLeavePeriod;
        public z_LeavePeriod CurrentToLeavePeriod
        {
            get
            {
                return this._CurrentToLeavePeriod;
            }
            set
            {
                _CurrentToLeavePeriod = value;
                OnPropertyChanged("CurrentToLeavePeriod");
                ControllerState = false;
                Employees = null;
                EmployeeMaxLeaveDetails = null;
                EmployeeLeaveDetails = null;
                ListEmployeeLeaveDetails.Clear();
                LeaveDetail = null;
                IsPay = false;
                IsTransfer = false;
                None = false;
                Remaining = null;
            }
        }

        private IEnumerable<EmployeeSearchView> _Employees;
        public IEnumerable<EmployeeSearchView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IList _CurrentEmployees = new ArrayList();
        public IList CurrentEmployees
        {
            get { return _CurrentEmployees; }
            set { _CurrentEmployees = value; OnPropertyChanged("CurrentEmployees"); if (CurrentEmployees != null && CurrentEmployees.Count > 0) FilterEmployeeLeaves(); }
        }

        private IEnumerable<Employee_Leave_Detail_Extended_View> _EmployeeLeaveDetails;
        public IEnumerable<Employee_Leave_Detail_Extended_View> EmployeeLeaveDetails
        {
            get { return _EmployeeLeaveDetails; }
            set { _EmployeeLeaveDetails = value; OnPropertyChanged("EmployeeLeaveDetails"); }
        }

        private IList _CurrentEmployeeLeaveDetails = new ArrayList();
        public IList CurrentEmployeeLeaveDetails
        {
            get { return _CurrentEmployeeLeaveDetails; }
            set { _CurrentEmployeeLeaveDetails = value; OnPropertyChanged("CurrentEmployeeLeaveDetails"); }
        }

        private IEnumerable<Employee_Maximum_Leave_Details_View> _EmployeeMaxLeaveDetails;
        public IEnumerable<Employee_Maximum_Leave_Details_View> EmployeeMaxLeaveDetails
        {
            get { return _EmployeeMaxLeaveDetails; }
            set { _EmployeeMaxLeaveDetails = value; OnPropertyChanged("EmployeeMaxLeaveDetails"); }
        }

        #endregion

        #region RefreshMethods

        private void RefreshEmployees()
        {
            serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
            {
                try
                {
                    ListEmployees.Clear();
                    ListEmployees = e.Result.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmloyeeSearchAsync();

        }

        private void RefreshLeavePeriods()
        {
            serviceClient.GetLeavePeriodsCompleted += (s, e) =>
            {
                try
                {
                    LeavePeriods = e.Result;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetLeavePeriodsAsync();
        }

        private void RefreshLeaveEntitlements()
        {
            this.serviceClient.GetEmployeeLeaveDetailsExtendedViewListByPeriodCompleted += (s, e) =>
            {
                try
                {
                    EmployeeLeaveDetails = e.Result;
                    if (EmployeeLeaveDetails != null && EmployeeLeaveDetails.Count() > 0)
                    {
                        ListEmployeeLeaveDetails = EmployeeLeaveDetails.ToList();
                        ListEmployeeLeaveDetails.ForEach(c => c.is_payed = c.is_transferred = false);
                        ListEmployeeLeaveDetails.ForEach(c => c.number_of_days = c.remaining_days.Value);
                    }
                    EmployeeLeaveDetails = null;
                    FilterLeaveData();
                }
                catch (Exception)
                {

                }
            };
            this.serviceClient.GetEmployeeLeaveDetailsExtendedViewListByPeriodAsync(CurrentFromLeavePeriod.period_id);
        }

        private void RefreshLeaveDetails()
        {
            serviceClient.GetLeaveMasterDetailViewByPeriodCompleted += (s, e) =>
            {
                try
                {
                    ListNewLeaveDetails.Clear();
                    LeaveDetail = e.Result;
                    if (LeaveDetail != null && LeaveDetail.Count() > 0)
                    {
                        foreach (var item in LeaveDetail)
                        {
                            NewLeaveDetails Temp = new NewLeaveDetails();
                            Temp.Period = CurrentToLeavePeriod.period_id;
                            Temp.Category = (Guid)item.leave_category_id;
                            Temp.OldDetail = item.leave_detail_id;
                            Temp.NewDetail = Guid.NewGuid();
                            Temp.DetailName = item.leave_detail_name;
                            Temp.Days = (decimal)item.number_of_days;

                            ListNewLeaveDetails.Add(Temp);
                        }
                    }
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetLeaveMasterDetailViewByPeriodAsync(CurrentFromLeavePeriod.period_id);
        }

        private void RefreshMaxLeaveDetails()
        {
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodCompleted += (s, e) =>
            {
                try
                {
                    EmployeeMaxLeaveDetails = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodAsync(CurrentFromLeavePeriod.period_id);
        }

        #endregion

        #region Commands & Methods

        private void FilterEmployees()
        {
            if (SearchIndex == 0)
            {
                Employees = null;
                EmployeeLeaveDetails = null;
                Employees = ListEmployees.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(SearchText.ToUpper()));
            }

            if (SearchIndex == 1)
            {
                Employees = null;
                EmployeeLeaveDetails = null;
                Employees = ListEmployees.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(SearchText.ToUpper()));
            }

            if (SearchIndex == 2)
            {
                Employees = null;
                EmployeeLeaveDetails = null;
                Employees = ListEmployees.Where(c => c.surname != null && c.surname.ToUpper().Contains(SearchText.ToUpper()));
            }

            if (SearchIndex == 3)
            {
                Employees = null;
                EmployeeLeaveDetails = null;
                Employees = ListEmployees.Where(c => c.department_name != null && c.department_name.ToUpper().Contains(SearchText.ToUpper()));
            }

            if (SearchIndex == 4)
            {
                double searchValue1 = 0;
                double searchValue2 = 0;
                string[] Values = SearchText.Split(',');

                if (Values != null)
                {

                    if (double.TryParse(Values[0], out searchValue1))
                    {
                        if (Values.Count() > 1)
                        {
                            if (double.TryParse(Values[1], out searchValue2))
                            {
                                Employees = null;
                                EmployeeLeaveDetails = null;
                                Employees = ListEmployees.Where(c => c.join_date != null && CurrentToLeavePeriod.from_date.Value.Date.Subtract(c.join_date.Value.Date).TotalDays / 30 >= searchValue1 && CurrentToLeavePeriod.from_date.Value.Date.Subtract(c.join_date.Value.Date).TotalDays / 30 <= searchValue2);
                            }

                            else
                            {
                                Employees = null;
                                EmployeeLeaveDetails = null;
                                Employees = ListEmployees.Where(c => c.join_date != null && CurrentToLeavePeriod.from_date.Value.Date.Subtract(c.join_date.Value.Date).TotalDays / 30 <= searchValue1);
                            }
                        }

                        else
                        {
                            Employees = null;
                            EmployeeLeaveDetails = null;
                            Employees = ListEmployees.Where(c => c.join_date != null && CurrentToLeavePeriod.from_date.Value.Date.Subtract(c.join_date.Value.Date).TotalDays / 30 <= searchValue1);
                        }
                    }

                    else
                    {
                        Employees = null;
                        EmployeeLeaveDetails = null;
                        Employees = ListEmployees;
                    }
                }
            }

            else { }
        }

        public ICommand KeyRemoveEntitlement
        {
            get { return new RelayCommand(RemoveEntitlement); }
        }

        private void RemoveEntitlement()
        {
            if (CurrentEmployees != null && CurrentEmployees.Count > 0 && CurrentEmployeeLeaveDetails != null && CurrentEmployeeLeaveDetails.Count > 0)
            {
                foreach (EmployeeSearchView employee in CurrentEmployees)
                {
                    foreach (Employee_Leave_Detail_Extended_View leave in CurrentEmployeeLeaveDetails)
                    {
                        if (ListEmployeeLeaveDetails.Count(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id) > 0)
                        {
                            Employee_Leave_Detail_Extended_View tempLeave = ListEmployeeLeaveDetails.FirstOrDefault(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id);
                            ListEmployeeLeaveDetails.Remove(tempLeave);
                        }
                    }
                }

                List<Employee_Leave_Detail_Extended_View> TempList = new List<Employee_Leave_Detail_Extended_View>();

                foreach (EmployeeSearchView employee in CurrentEmployees)
                {
                    foreach (var leave in ListEmployeeLeaveDetails.Where(c => c.emp_id == employee.employee_id))
                    {
                        if (TempList.Count(c => c.leave_detail_id == leave.leave_detail_id) == 0)
                        {
                            Employee_Leave_Detail_Extended_View TempLeave = ListEmployeeLeaveDetails.FirstOrDefault(c => c.leave_detail_id == leave.leave_detail_id);
                            TempList.Add(TempLeave);
                        }
                    }
                }

                EmployeeLeaveDetails = null;
                EmployeeLeaveDetails = TempList;
            }
        }

        private void SetRemainingValues()
        {
            try
            {
                if (CurrentEmployees != null && CurrentEmployees.Count > 0 && CurrentEmployeeLeaveDetails != null && CurrentEmployeeLeaveDetails.Count > 0)
                {
                    foreach (EmployeeSearchView employee in CurrentEmployees)
                    {
                        foreach (Employee_Leave_Detail_Extended_View leave in CurrentEmployeeLeaveDetails)
                        {
                            if (ListEmployeeLeaveDetails.Count(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id) > 0)
                            {
                                Employee_Leave_Detail_Extended_View tempLeave = ListEmployeeLeaveDetails.FirstOrDefault(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id);
                                tempLeave.remaining_days = Remaining >= tempLeave.number_of_days ? tempLeave.number_of_days : (Remaining <= 0 ? 0 : Remaining.Value);
                            }
                        }
                    }
                }

                else
                    Remaining = null;
            }
            catch (Exception)
            {

            }
        }

        private void SetMigrationState()
        {
            try
            {
                if (CurrentEmployees != null && CurrentEmployees.Count > 0 && CurrentEmployeeLeaveDetails != null && CurrentEmployeeLeaveDetails.Count > 0)
                {
                    foreach (EmployeeSearchView employee in CurrentEmployees)
                    {
                        foreach (Employee_Leave_Detail_Extended_View leave in CurrentEmployeeLeaveDetails)
                        {
                            if (ListEmployeeLeaveDetails.Count(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id && c.remaining_days > 0) > 0)
                            {
                                Employee_Leave_Detail_Extended_View tempLeave = ListEmployeeLeaveDetails.FirstOrDefault(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id);
                                tempLeave.is_payed = IsPay;
                                tempLeave.is_transferred = IsTransfer;

                                if (None)
                                {
                                    tempLeave.is_payed = false;
                                    tempLeave.is_transferred = false;
                                }
                            }
                        }
                    }

                    IsTransfer = false;
                    IsPay = false;
                    None = false;
                }
                else
                {
                    IsTransfer = false;
                    IsPay = false;
                    None = false;
                }
            }
            catch (Exception)
            {

            }
        }

        public ICommand BtnAddCategory
        {
            get { return new RelayCommand(AddCategory, AddCategoryCE); }
        }

        private bool AddCategoryCE()
        {
            if (CurrentLeaveDetail != null && CurrentEmployees != null && CurrentEmployees.Count > 0)
                return true;
            else
                return false;
        }

        private void AddCategory()
        {
            foreach (EmployeeSearchView Employee in CurrentEmployees)
            {
                try
                {
                    if (ListEmployeeLeaveDetails.Count(c => c.emp_id == Employee.employee_id && c.leave_detail_id == CurrentLeaveDetail.leave_detail_id) == 0)
                    {
                        Employee_Leave_Detail_Extended_View Temp = new Employee_Leave_Detail_Extended_View();
                        Temp.number_of_days = 0;
                        Temp.remaining_days = 0;
                        Temp.emp_id = Employee.employee_id;
                        Temp.leave_detail_name = CurrentLeaveDetail.leave_detail_name;
                        Temp.first_name = Employee.first_name;
                        Temp.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                        Temp.EMPLOYEE_ID = Employee.emp_id;
                        Temp.leave_period_id = CurrentLeaveDetail.leave_period_id;
                        Temp.leave_category_id = CurrentLeaveDetail.leave_category_id;
                        Temp.cat_number_of_days = CurrentLeaveDetail.number_of_days;
                        Temp.is_payed = false;
                        Temp.is_transferred = false;

                        if (ListEmployeeLeaveDetails.Count(c => c.leave_category_id == CurrentLeaveDetail.leave_category_id && c.emp_id == Employee.employee_id) > 0)
                        {
                            Employee_Leave_Detail_Extended_View delObj = ListEmployeeLeaveDetails.FirstOrDefault(c => c.leave_category_id == CurrentLeaveDetail.leave_category_id && c.emp_id == Employee.employee_id);
                            Temp.number_of_days = delObj.number_of_days.Value;
                            Temp.remaining_days = delObj.number_of_days.Value;

                            ListEmployeeLeaveDetails.Remove(delObj);
                        }

                        ListEmployeeLeaveDetails.Add(Temp);
                    }
                }
                catch (Exception)
                {

                }
            }

            List<Employee_Leave_Detail_Extended_View> TempList = new List<Employee_Leave_Detail_Extended_View>();

            foreach (EmployeeSearchView employee in CurrentEmployees)
            {
                foreach (var leave in ListEmployeeLeaveDetails.Where(c => c.emp_id == employee.employee_id))
                {
                    if (TempList.Count(c => c.leave_detail_id == leave.leave_detail_id) == 0)
                    {
                        Employee_Leave_Detail_Extended_View TempLeave = ListEmployeeLeaveDetails.FirstOrDefault(c => c.leave_detail_id == leave.leave_detail_id && c.emp_id == employee.employee_id);
                        TempList.Add(TempLeave);
                    }
                }
            }

            EmployeeLeaveDetails = null;
            EmployeeLeaveDetails = TempList;
        }

        private void FilterEmployeeLeaves()
        {
            List<Employee_Leave_Detail_Extended_View> TempList = new List<Employee_Leave_Detail_Extended_View>();

            foreach (EmployeeSearchView employee in CurrentEmployees)
            {
                foreach (var leave in ListEmployeeLeaveDetails.Where(c => c.emp_id == employee.employee_id))
                {
                    if (TempList.Count(c => c.leave_detail_id == leave.leave_detail_id) == 0)
                    {
                        Employee_Leave_Detail_Extended_View TempLeave = ListEmployeeLeaveDetails.FirstOrDefault(c => c.emp_id == employee.employee_id && c.leave_detail_id == leave.leave_detail_id);
                        TempList.Add(TempLeave);
                    }
                }
            }

            EmployeeLeaveDetails = null;
            EmployeeLeaveDetails = TempList;
        }

        private void FilterLeaveData()
        {
            if (ListEmployeeLeaveDetails != null && ListEmployeeLeaveDetails.Count() > 0 && ListEmployees != null && ListEmployees.Count > 0)
            {
                ControllerState = true;
                ListEmployees = ListEmployees.Where(c => ListEmployeeLeaveDetails.Count(d => c.employee_id == d.emp_id) > 0).ToList();
                Employees = null;
                Employees = ListEmployees;
            }
        }

        public ICommand BtnCheckLeaves
        {
            get { return new RelayCommand(CheckLeaves, CheckLeavesCE); }
        }

        private bool CheckLeavesCE()
        {
            if (CurrentFromLeavePeriod != null && CurrentToLeavePeriod != null && (CurrentFromLeavePeriod.period_id != CurrentToLeavePeriod.period_id))
                return true;
            else
                return false;
        }

        private void CheckLeaves()
        {
            int[] TempResult = serviceClient.CheckExistingEntitlements(CurrentFromLeavePeriod.period_id, CurrentToLeavePeriod.period_id);

            if (TempResult == null)
                clsMessages.setMessage("Error");
            else if (TempResult[0] == 0)
                clsMessages.setMessage("'Old leave period' doesn't have any existing entitlements,\n select a different leave period and try again");
            else if (TempResult[1] == 1)
                clsMessages.setMessage("Leave entitlements found under the 'New leave period',\n please create a new leave period and try again");
            else
            {
                RefreshLeaveEntitlements();
                RefreshMaxLeaveDetails();
                RefreshLeaveDetails();
            }

        }

        public ICommand BtnSave
        {
            get { return new RelayCommand(SaveEntitlements); }
        }

        private void SaveEntitlements()
        {
            if (ListEmployees != null && ListEmployees.Count > 0 && ListEmployeeLeaveDetails != null && ListEmployeeLeaveDetails.Count > 0 && ListNewLeaveDetails != null && ListNewLeaveDetails.Count > 0)
            {
                if (clsSecurity.GetSavePermission(411))
                {
                    List<mas_LeaveDetail> MasLeaveList = new List<mas_LeaveDetail>();
                    List<Employee_Leave_Detail_Extended_View> EmployeeLeavesList = new List<Employee_Leave_Detail_Extended_View>();
                    List<Employee_Maximum_Leave_Details_View> EmployeeMaxLeavesList = new List<Employee_Maximum_Leave_Details_View>();

                    foreach (var Leave in ListNewLeaveDetails)
                    {
                        mas_LeaveDetail Temp = new mas_LeaveDetail();
                        Temp.leave_category_id = Leave.Category;
                        Temp.leave_period_id = Leave.Period;
                        Temp.leave_detail_id = Leave.NewDetail;
                        Temp.number_of_days = Leave.Days;
                        Temp.leave_detail_name = Leave.DetailName;

                        MasLeaveList.Add(Temp);

                        foreach (var EmpLeave in ListEmployeeLeaveDetails.Where(c => c.leave_detail_id == Leave.OldDetail))
                        {
                            Employee_Leave_Detail_Extended_View EmpTemp = new Employee_Leave_Detail_Extended_View();
                            EmpTemp.emp_id = EmpLeave.emp_id;
                            EmpTemp.leave_detail_id = Leave.NewDetail;
                            EmpTemp.number_of_days = 0;
                            EmpTemp.remaining_days = EmpLeave.cat_number_of_days;
                            EmpTemp.maximum_leaves = EmpLeave.maximum_leaves;
                            EmpTemp.is_special = true;
                            EmpTemp.is_payed = EmpLeave.is_payed;
                            EmpTemp.is_transferred = EmpLeave.is_transferred;
                            EmpTemp.value = EmpLeave.remaining_days;

                            EmployeeLeavesList.Add(EmpTemp);

                            if (EmployeeMaxLeaveDetails != null && EmployeeMaxLeaveDetails.Count() > 0)
                            {
                                foreach (var MaxItem in EmployeeMaxLeaveDetails.Where(c => c.emp_id == EmpLeave.emp_id && c.leave_detail_id == EmpLeave.leave_detail_id))
                                {
                                    Employee_Maximum_Leave_Details_View TempMax = new Employee_Maximum_Leave_Details_View();
                                    TempMax.emp_id = MaxItem.emp_id;
                                    TempMax.leave_detail_id = Leave.NewDetail;
                                    TempMax.leave_type_id = MaxItem.leave_type_id;
                                    TempMax.leave_type_qty = MaxItem.leave_type_qty;

                                    EmployeeMaxLeavesList.Add(TempMax);
                                }
                            }
                        }
                    }

                    if (serviceClient.SaveMigratedEntitlement(MasLeaveList.ToArray(), EmployeeLeavesList.ToArray(), EmployeeMaxLeavesList.ToArray()))
                    {
                        ControllerState = false;
                        Employees = null;
                        EmployeeLeaveDetails = null;
                        ListEmployeeLeaveDetails.Clear();
                        LeaveDetail = null;
                        IsPay = false;
                        IsTransfer = false;
                        None = false;
                        Remaining = null;

                        clsMessages.setMessage("Leave data was migrated successfully!");
                    }
                    else
                        clsMessages.setMessage("Leave data  migration failed!"); 
                }
                else
                    clsMessages.setMessage("You don't have permission to migrate");
            }
        }

        #endregion
    }

    class NewLeaveDetails
    {
        private Guid _Period;
        public Guid Period
        {
            get { return _Period; }
            set { _Period = value; }
        }


        private Guid _Category;
        public Guid Category
        {
            get { return _Category; }
            set { _Category = value; }
        }


        private Guid _OldDetail;
        public Guid OldDetail
        {
            get { return _OldDetail; }
            set { _OldDetail = value; }
        }

        private Guid _NewDetail;
        public Guid NewDetail
        {
            get { return _NewDetail; }
            set { _NewDetail = value; }
        }

        private string _DetailName;
        public string DetailName
        {
            get { return _DetailName; }
            set { _DetailName = value; }
        }

        private decimal _Days;
        public decimal Days
        {
            get { return _Days; }
            set { _Days = value; }
        }



    }
}
