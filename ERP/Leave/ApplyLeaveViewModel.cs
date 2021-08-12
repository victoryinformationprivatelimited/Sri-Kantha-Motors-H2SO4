using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using AttendanceServiceNew = ERP.AttendanceService.AttendanceServiceClient;
using System.Collections;

namespace ERP.Leave
{
    public class ApplyLeaveViewModel : ViewModelBase
    {

        ERPServiceClient serviceClient = new ERPServiceClient();
        AttendanceServiceNew ServiceAttendance = new AttendanceServiceNew();
        List<ERP.AttendanceService.EmployeeLeaveShiftDetailsView> ListLeaveShiftDetails;
        List<ERP.AttendanceService.EmployeeLeiuLeaveView> EmployeeLeiuLeaveAll;
        decimal leiuamount = 0;
        List<ERP.AttendanceService.EmployeeLeiuLeaveView> SelectedLeiuLeaveList;
        List<string> leiuleavedates;
        string currentdate = "";

        public ApplyLeaveViewModel()
        {
            ListLeaveShiftDetails = new List<AttendanceService.EmployeeLeaveShiftDetailsView>();
            EmployeeLeiuLeaveAll = new List<AttendanceService.EmployeeLeiuLeaveView>();
            SelectedLeiuLeaveList = new List<AttendanceService.EmployeeLeiuLeaveView>();
            refreshLeavePeriod();
            refreshEmployees();
            refreshLeaveTypes();
            RefreshLeiuLeaveDetails();
            LeaveFromDate = DateTime.Now.Date;
            LeaveToDate = DateTime.Now.Date;
            NormalLeave = true;
        }

        #region Propertices

        private IEnumerable<ERP.AttendanceService.EmployeeLeaveShiftDetailsView> _LeaveShiftDetails;
        public IEnumerable<ERP.AttendanceService.EmployeeLeaveShiftDetailsView> LeaveShiftDetails
        {
            get { return _LeaveShiftDetails; }
            set { _LeaveShiftDetails = value; OnPropertyChanged("LeaveShiftDetails"); }
        }

        private ERP.AttendanceService.EmployeeLeaveShiftDetailsView _CurrentLeaveShiftDetails;
        public ERP.AttendanceService.EmployeeLeaveShiftDetailsView CurrentLeaveShiftDetails
        {
            get { return _CurrentLeaveShiftDetails; }
            set { _CurrentLeaveShiftDetails = value; OnPropertyChanged("CurrentLeaveShiftDetails"); }
        }

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<mas_Employee> _EmployeesAll;
        public IEnumerable<mas_Employee> EmployeesAll
        {
            get { return _EmployeesAll; }
            set { _EmployeesAll = value; OnPropertyChanged("EmployeesAll"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee");
                if (CurrentEmployee != null)
                {
                    FilterEmployeeLeaveData();
                    FilterEmployeeLeiuLeave();
                    refreshEmployeeCurrentLeaves();
                    RemainingDays = 0;
                    TotalLeavedays = 0;
                    PendingLeaves = 0;
                }
                else
                {
                    Leavedetails = null;
                    RemainingDays = 0;
                    TotalLeavedays = 0;
                    PendingLeaves = 0;
                }
                CurrentLeaveType = null;
            }
        }

        private IEnumerable<z_LeavePeriod> _LeavePeriods;
        public IEnumerable<z_LeavePeriod> LeavePeriods
        {
            get { return _LeavePeriods; }
            set { _LeavePeriods = value; OnPropertyChanged("LeavePeriods"); }
        }


        private z_LeavePeriod _CurrentLeavePeriod;
        public z_LeavePeriod CurrentLeavePeriod
        {
            get { return _CurrentLeavePeriod; }
            set { _CurrentLeavePeriod = value; OnPropertyChanged("CurrentLeavePeriod"); buttonNew(); Employees = null; Leavedetails = null; LeavedetailsAll = null; EmployeeLeaveDetails = null; EmployeeLeaveDetailsAll = null; }
        }

        private IEnumerable<mas_LeaveDetail> _Leavedetails;
        public IEnumerable<mas_LeaveDetail> Leavedetails
        {
            get { return _Leavedetails; }
            set { _Leavedetails = value; OnPropertyChanged("Leavedetails"); }
        }

        private IEnumerable<mas_LeaveDetail> _LeavedetailsAll;
        public IEnumerable<mas_LeaveDetail> LeavedetailsAll
        {
            get { return _LeavedetailsAll; }
            set { _LeavedetailsAll = value; OnPropertyChanged("LeavedetailsAll"); }
        }

        private mas_LeaveDetail _CurrentLeaveDetail;
        public mas_LeaveDetail CurrentLeaveDetail
        {
            get { return _CurrentLeaveDetail; }
            set
            {
                _CurrentLeaveDetail = value; OnPropertyChanged("CurrentLeaveDetail"); if (CurrentEmployee != null && CurrentLeavePeriod != null && CurrentLeaveDetail != null) { CheckLeaveDates(); } else { RemainingDays = 0; TotalLeavedays = 0; PendingLeaves = 0; };
            }
        }

        private DateTime? _LeaveFromDate;
        public DateTime? LeaveFromDate
        {
            get { return _LeaveFromDate; }
            set { _LeaveFromDate = value; OnPropertyChanged("LeaveFromDate"); if (LeaveFromDate != null) LeaveToDate = LeaveFromDate.Value; if (CurrentEmployee != null && CurrentLeavePeriod != null && CurrentLeaveDetail != null && CurrentLeaveType != null) { CheckTypeQuantity(); } }
        }

        private DateTime? _LeaveToDate;
        public DateTime? LeaveToDate
        {
            get { return _LeaveToDate; }
            set { _LeaveToDate = value; OnPropertyChanged("LeaveToDate"); if (CurrentEmployee != null && CurrentLeavePeriod != null && CurrentLeaveDetail != null && CurrentLeaveType != null) { CheckTypeQuantity(); } }
        }


        private IEnumerable<Employee_Leave_Detail_View> _EmployeeLeaveDetails;
        public IEnumerable<Employee_Leave_Detail_View> EmployeeLeaveDetails
        {
            get { return _EmployeeLeaveDetails; }
            set { _EmployeeLeaveDetails = value; OnPropertyChanged("EmployeeLeaveDetails"); }
        }

        private IEnumerable<Employee_Leave_Detail_View> _EmployeeLeaveDetailsAll;
        public IEnumerable<Employee_Leave_Detail_View> EmployeeLeaveDetailsAll
        {
            get { return _EmployeeLeaveDetailsAll; }
            set { _EmployeeLeaveDetailsAll = value; OnPropertyChanged("EmployeeLeaveDetailsAll"); }
        }

        private IEnumerable<Employee_Maximum_Leave_Details_View> _EmployeeMaxLeaveDetailsAll;
        public IEnumerable<Employee_Maximum_Leave_Details_View> EmployeeMaxLeaveDetailsAll
        {
            get { return _EmployeeMaxLeaveDetailsAll; }
            set { _EmployeeMaxLeaveDetailsAll = value; OnPropertyChanged("EmployeeMaxLeaveDetailsAll"); }
        }

        private IEnumerable<z_LeaveType> _Leave_types;
        public IEnumerable<z_LeaveType> Leave_types
        {
            get { return _Leave_types; }
            set { _Leave_types = value; OnPropertyChanged("Leave_types"); }
        }

        private z_LeaveType _CurrentLeaveType;
        public z_LeaveType CurrentLeaveType
        {
            get { return _CurrentLeaveType; }
            set 
            { 
                _CurrentLeaveType = value; OnPropertyChanged("CurrentLeaveType"); 
                if (CurrentLeaveType != null && CurrentEmployee != null && CurrentLeavePeriod != null && CurrentLeaveDetail != null && CurrentLeaveShiftDetails != null)
                { 
                    CheckTypeQuantity();
                }
                if (CurrentEmployeeLeiuLeaveList != null && CurrentEmployeeLeiuLeaveList.Count > 0)
                {
                    foreach (ERP.AttendanceService.EmployeeLeiuLeaveView item in CurrentEmployeeLeiuLeaveList)
                    {
                        SelectedLeiuLeaveList.Add(item);
                    }
                }
            }
        }

        private IEnumerable<trns_LeavePool> _EmployeeLeaves;
        public IEnumerable<trns_LeavePool> EmployeeLeaves
        {
            get { return _EmployeeLeaves; }
            set { _EmployeeLeaves = value; OnPropertyChanged("EmployeeLeaves"); }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; OnPropertyChanged("Remark"); }
        }

        private string _Reason;
        public string Reason
        {
            get { return _Reason; }
            set { _Reason = value; OnPropertyChanged("Reason"); }
        }

        private decimal _RemainingDays;
        public decimal RemainingDays
        {
            get { return _RemainingDays; }
            set { _RemainingDays = value; OnPropertyChanged("RemainingDays"); }
        }

        private decimal _TypeQantity;
        public decimal TypeQantity
        {
            get { return _TypeQantity; }
            set { _TypeQantity = value; OnPropertyChanged("TypeQantity"); }
        }

        private decimal _TotalLeavedays;
        public decimal TotalLeavedays
        {
            get { return _TotalLeavedays; }
            set { _TotalLeavedays = value; OnPropertyChanged("TotalLeavedays"); }
        }

        private int _PendingLeaves;
        public int PendingLeaves
        {
            get { return _PendingLeaves; }
            set { _PendingLeaves = value; OnPropertyChanged("PendingLeaves"); }
        }

        public bool _NormalLeave;

        public bool NormalLeave
        {
            get { return _NormalLeave; }
            set { _NormalLeave = value; OnPropertyChanged("NormalLeave"); }
        }

        public bool _LeiuLeave;

        public bool LeiuLeave
        {
            get { return _LeiuLeave; }
            set { _LeiuLeave = value; OnPropertyChanged("LeiuLeave"); }
        }

        private IEnumerable<ERP.AttendanceService.EmployeeLeiuLeaveView> _EmployeeLeiuLeave;
        public IEnumerable<ERP.AttendanceService.EmployeeLeiuLeaveView> EmployeeLeiuLeave
        {
            get { return _EmployeeLeiuLeave; }
            set { _EmployeeLeiuLeave = value; OnPropertyChanged("EmployeeLeiuLeave"); }
        }

        private ERP.AttendanceService.EmployeeLeiuLeaveView _CurrentEmployeeLeiuLeave;
        public ERP.AttendanceService.EmployeeLeiuLeaveView CurrentEmployeeLeiuLeave
        {
            get { return _CurrentEmployeeLeiuLeave; }
            set { _CurrentEmployeeLeiuLeave = value; OnPropertyChanged("CurrentEmployeeLeiuLeave"); }
        }

        private IList _CurrentEmployeeLeiuLeaveList = new ArrayList();
        public IList CurrentEmployeeLeiuLeaveList
        {
            get { return _CurrentEmployeeLeiuLeaveList; }
            set
            { _CurrentEmployeeLeiuLeaveList = value; OnPropertyChanged("CurrentEmployeeLeiuLeaveList"); }
        }

        #endregion

        public ICommand BtnGetLeavesForPeriod
        {
            get { return new RelayCommand(GetLeavesForPeriod, GetLeavesForPeriodCE); }
        }

        private bool GetLeavesForPeriodCE()
        {
            if (CurrentLeavePeriod != null)
                return true;
            else
                return false;
        }

        private void GetLeavesForPeriod()
        {
            refreshEmpoyeeLeaveDetails();
            refreshEmployeeMaximumLeaves();
            refreshLeaveDetiails();
        }

        public ICommand BtnApply
        {
            get { return new RelayCommand(applyLeave, applyCanExecute); }
        }

        public ICommand BtnNew
        {
            get { return new RelayCommand(buttonNew, newCanExecute); }
        }

        private bool newCanExecute()
        {
            return true;
        }

        private void buttonNew()
        {
            CurrentLeaveDetail = null;
            CurrentEmployee = null;
            CurrentLeaveType = null;
            CurrentLeaveShiftDetails = null;
            LeaveShiftDetails = null;
            ListLeaveShiftDetails.Clear();
            EmployeeLeiuLeave = null;

            LeaveFromDate = DateTime.Now.Date;
            LeaveToDate = DateTime.Now.Date;

            Remark = "";
            Reason = "";
            RemainingDays = 0;
            TotalLeavedays = 0;
            PendingLeaves = 0;
        }

        private bool applyCanExecute()
        {
            return true;
        }

        #region Refresh Mehods

        private void refreshLeavePeriod() //leave periods
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
            {
                LeavePeriods = e.Result;
            };
            this.serviceClient.GetLeavePeriodsAsync();
        }

        private void refreshEmployees() //employee Details
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                EmployeesAll = e.Result;
                FilterLeaveData();
            };
            this.serviceClient.GetEmployeesAsync();

        }

        private void refreshEmpoyeeLeaveDetails() //Leave Details
        {
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodCompleted += (s, e) =>
            {
                EmployeeLeaveDetailsAll = e.Result.Where(c => c.is_special == true);
                FilterLeaveData();
            };
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriod.period_id);

        }

        private void refreshEmployeeMaximumLeaves()
        {
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodCompleted += (s, e) =>
            {
                try
                {
                    EmployeeMaxLeaveDetailsAll = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriod.period_id);
        }

        private void refreshLeaveTypes() //z_leave Types
        {
            this.serviceClient.GetAllLeaveTypesCompleted += (s, e) =>
            {
                this.Leave_types = e.Result;
            };
            this.serviceClient.GetAllLeaveTypesAsync();
        }

        private void refreshEmployeeCurrentLeaves() //Data from Leave Pool
        {
            if (CurrentEmployee != null)
            {
                this.serviceClient.GetLeavesByEmpIdByPeriodCompleted += (s, e) =>
                {
                    this.EmployeeLeaves = e.Result.Where(c => c.is_rejected == false);
                };
                this.serviceClient.GetLeavesByEmpIdByPeriodAsync(CurrentEmployee.employee_id, CurrentLeavePeriod.period_id);
            }
        }

        private void refreshLeaveDetiails() // mas_leavedetails
        {
            this.serviceClient.GetMasLeaveDetailsByPeriodCompleted += (s, e) =>
            {
                LeavedetailsAll = e.Result;
            };

            this.serviceClient.GetMasLeaveDetailsByPeriodAsync(CurrentLeavePeriod.period_id);
        }

        private void RefreshShiftDetails() // Shift Details
        {
            ServiceAttendance.GetShiftDetailsByDateCompleted += (s, e) =>
            {
                LeaveShiftDetails = e.Result;
                if (LeaveShiftDetails != null && LeaveShiftDetails.Count() > 0)
                    ListLeaveShiftDetails = LeaveShiftDetails.ToList();
                else
                    //clsMessages.setMessage("No Shift Details Found!");
                    MessageBox.Show("No Shift Details Found!");
            };
            ServiceAttendance.GetShiftDetailsByDateAsync(CurrentEmployee.employee_id, (DateTime)LeaveFromDate, (DateTime)LeaveToDate);
        }

        private void RefreshLeiuLeaveDetails() //Leiu Leave Details
        {
            ServiceAttendance.GetEmployeeLeiuLeaveDetailsCompleted += (s, e) =>
            {
                try
                {
                    EmployeeLeiuLeaveAll.Clear();
                    EmployeeLeiuLeave = e.Result;
                    if (EmployeeLeiuLeave != null && EmployeeLeiuLeave.Count() > 0)
                        EmployeeLeiuLeaveAll = EmployeeLeiuLeave.ToList();
                    EmployeeLeiuLeave = null;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Employee Leiu Leave Refresh is Failed");
                }
            };
            ServiceAttendance.GetEmployeeLeiuLeaveDetailsAsync();
        }

        #endregion

        #region Filter Mewthods

        private void FilterLeaveCategories()
        {
            if (LeavedetailsAll != null || LeavedetailsAll.Count() > 0)
            {
                IEnumerable<mas_LeaveDetail> templeaveDetail;
                EmployeeLeaveDetails = EmployeeLeaveDetailsAll.Where(c => c.emp_id == CurrentEmployee.employee_id);
                templeaveDetail = LeavedetailsAll.Where(c => EmployeeLeaveDetails.Count(d => d.leave_detail_id == c.leave_detail_id) > 0);
                Leavedetails = null;
                Leavedetails = templeaveDetail.Where(c => c.leave_period_id == CurrentLeavePeriod.period_id);
            }
        }

        private void FilterEmployeeLeaveData()
        {
            if (EmployeeLeaveDetailsAll != null || EmployeeLeaveDetailsAll.Count() > 0)
            {
                EmployeeLeaveDetails = EmployeeLeaveDetailsAll.Where(c => c.emp_id == CurrentEmployee.employee_id);
                FilterLeaveCategories();
            }
        }

        private void FilterLeaveData()
        {
            if ((EmployeesAll != null && EmployeesAll.Count() > 0) && (EmployeeLeaveDetailsAll != null && EmployeeLeaveDetailsAll.Count() > 0))
            {
                Employees = EmployeesAll.Where(c => EmployeeLeaveDetailsAll.Count(d => c.employee_id == d.emp_id) > 0);
            }
        }

        private void FilterEmployeeLeiuLeave()
        {
            EmployeeLeiuLeave = null;
            EmployeeLeiuLeave = EmployeeLeiuLeaveAll.Where(c => c.employee_id == CurrentEmployee.employee_id);
        }

        #endregion

        #region User Defined Methods

        private void applyLeave()
        {
            if (clsSecurity.GetSavePermission(406))
            {
                if (NormalLeave == true)
                {
                    if (ValidateSave())
                    {
                        try
                        {
                            List<trns_LeavePool> Savelist = new List<trns_LeavePool>();
                            //int DateCount = LeaveToDate.Value.Subtract(LeaveFromDate.Value).Days + 1;

                            foreach (var Shift in ListLeaveShiftDetails)
                            {
                                trns_LeavePool newLeave = new trns_LeavePool();
                                newLeave.pool_id = Guid.NewGuid();
                                newLeave.emp_id = CurrentEmployee.employee_id;
                                newLeave.leave_category_id = CurrentLeaveDetail.leave_category_id;
                                newLeave.leave_date = Shift.date;
                                newLeave.reason = Reason;
                                newLeave.apply_date = DateTime.Now;
                                newLeave.is_approved = false;
                                newLeave.is_pending_for_approval = true;
                                newLeave.is_rejected = false;
                                newLeave.remarks = Remark;
                                newLeave.leave_type_id = Shift.leave_type_id;
                                newLeave.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                                newLeave.shift_detail_id = Shift.shift_detail_id;
                                Savelist.Add(newLeave);
                            }

                            if (this.serviceClient.SaveLeavePool(Savelist.ToArray()))
                                MessageBox.Show("Leave Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                                MessageBox.Show("Leave Save Failed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            buttonNew();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Request Cannot be Completed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
                else
                {
                    if (ValidLeiuLeaveSave())
                    {
                        try
                        {
                            mas_LeaveDetail result = LeavedetailsAll.FirstOrDefault(c => c.leave_category_id == new Guid("9B615C80-32D7-4951-BABC-04AD7193BC32"));
                            List<trns_LeavePool> Savelist = new List<trns_LeavePool>();
                            foreach (var item in SelectedLeiuLeaveList)
                            {
                                //ERP.AttendanceService.EmployeeLeiuLeaveView currentamount = new AttendanceService.EmployeeLeiuLeaveView();
                                //currentamount = item as ERP.AttendanceService.EmployeeLeiuLeaveView;
                                leiuamount += Convert.ToDecimal(item.amount);
                            }
                            if (CurrentLeaveType.value != leiuamount)
                            {
                                MessageBox.Show("Please Select a Suitable Leave Type For The Suitable Amount", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                foreach (var Shift in ListLeaveShiftDetails)
                                {
                                    trns_LeavePool newLeave = new trns_LeavePool();
                                    newLeave.pool_id = Guid.NewGuid();
                                    newLeave.emp_id = CurrentEmployee.employee_id;
                                    newLeave.leave_category_id = new Guid("9B615C80-32D7-4951-BABC-04AD7193BC32");
                                    newLeave.leave_date = Shift.date;
                                    newLeave.reason = Reason;
                                    newLeave.apply_date = DateTime.Now;
                                    newLeave.is_approved = false;
                                    newLeave.is_pending_for_approval = true;
                                    newLeave.is_rejected = false;
                                    newLeave.remarks = Remark;
                                    newLeave.leave_type_id = Shift.leave_type_id;
                                    newLeave.leave_detail_id = result.leave_detail_id;
                                    newLeave.shift_detail_id = Shift.shift_detail_id;
                                    leiuleavedates = new List<string>();
                                    foreach (var item in SelectedLeiuLeaveList)
                                    {
                                        currentdate = currentdate +"/"+item.attend_date.ToString();
                                    }
                                    newLeave.used_leiu_leave_date = currentdate.ToString();
                                    Savelist.Add(newLeave);
                                }
                            }
                            if (this.serviceClient.SaveLeavePool(Savelist.ToArray()) && ServiceAttendance.UpdateEmployeeLeiuLeaveDetails(SelectedLeiuLeaveList.ToArray()))
                            {
                                MessageBox.Show("Leiu Leave Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                buttonNew();
                            }
                            else
                                MessageBox.Show("Leiu Leave Save Failed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Request Cannot be Completed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to Save this record(s)");
        }

        private bool ValidateSave()
        {


            if (CurrentEmployee == null)
            {
                MessageBox.Show("Please Select an Employee and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (serviceClient.GetSupervisorCount(CurrentEmployee.employee_id, new Guid("EE0D8A55-5A31-4FDB-A36F-36643A26B1AA")) == 0)
            {
                MessageBox.Show("An employee should be at least under one supervisor", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (CurrentLeavePeriod == null)
            {
                MessageBox.Show("Please Select a Period and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (CurrentLeaveDetail == null)
            {
                MessageBox.Show("Please Select a Leave Category and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (ListLeaveShiftDetails == null || ListLeaveShiftDetails.Count == 0)
            {
                MessageBox.Show("Please Fetch Shift Details and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (ListLeaveShiftDetails.Count(c => c.leave_type_id == null || c.leave_type_id == Guid.Empty) > 0)
            {
                MessageBox.Show("Shift Details Without Leave Types Exist, Please Check and try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (ListLeaveShiftDetails.Count(c => c.date < CurrentLeavePeriod.from_date.Value || c.date > CurrentLeavePeriod.to_date.Value) > 0)
            {
                MessageBox.Show("Please Select Valid Leave Dates Within the Selected Period and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (RemainingDays < 0)
            {
                MessageBox.Show("This Employee has Consumed All Entitled Leaves", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            else if (ListLeaveShiftDetails != null && ListLeaveShiftDetails.Count > 0)
            {
                decimal? AppliedDays = 0;
                decimal? MaxLeaves = 0;
                decimal? MaxAvailable = 0;
                IEnumerable<Employee_Maximum_Leave_Details_View> EmployeeMaxLeaveDetails;

                MaxLeaves = EmployeeLeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id) == null ? 0 : EmployeeLeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id).maximum_leaves;
                EmployeeMaxLeaveDetails = (EmployeeMaxLeaveDetailsAll == null || EmployeeMaxLeaveDetailsAll.Count() == 0) ? null : EmployeeMaxLeaveDetailsAll.Where(c => c.emp_id == CurrentEmployee.employee_id && c.leave_detail_id == CurrentLeaveDetail.leave_detail_id);

                foreach (var item in ListLeaveShiftDetails)
                {
                    AppliedDays += Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value;
                    MaxAvailable += Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value;
                }

                decimal? AvailableDays = 0;
                decimal? PendingValues = 0;
                decimal? PendingValuesByday = 0;

                if (EmployeeLeaves != null && EmployeeLeaves.Count() > 0)
                {
                    bool exists = false;

                    foreach (var LeaveItem in EmployeeLeaves.Where(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id && c.leave_date.Value.Month == LeaveFromDate.Value.Month))
                    {
                        MaxAvailable += Leave_types.FirstOrDefault(c => c.leave_type_id == LeaveItem.leave_type_id) == null ? 0 : Leave_types.FirstOrDefault(c => c.leave_type_id == LeaveItem.leave_type_id).value;
                    }

                    foreach (var Shift in ListLeaveShiftDetails)
                    {
                        if (EmployeeLeaves.Count(c => c.leave_date == Shift.date) > 0)
                        {
                            IEnumerable<trns_LeavePool> temp = EmployeeLeaves.Where(c => c.leave_date == Shift.date);
                            foreach (var item in temp)
                            {
                                if (item.leave_detail_id != CurrentLeaveDetail.leave_detail_id)
                                {
                                    PendingValuesByday = PendingValuesByday + Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value;
                                }
                                else
                                {
                                    PendingValuesByday = 1;
                                    break;
                                }
                            }

                            if (PendingValuesByday + Leave_types.FirstOrDefault(c => c.leave_type_id == Shift.leave_type_id).value > 1)
                            {
                                MessageBox.Show("Leave Data for '" + Shift.date + "' Already Exists", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                exists = true;
                                break;
                            }
                        }

                        PendingValuesByday = 0;
                    }

                    if (exists == true)
                        return false;

                    //PendingValues = Leave_types.Where(c => TempLeaves.Count(d => d.leave_type_id == c.leave_type_id) > 0).Select(c => c.value).Sum();

                    foreach (var item in EmployeeLeaves.Where(c => c.is_pending_for_approval == true && c.leave_detail_id == CurrentLeaveDetail.leave_detail_id))
                    {
                        PendingValues = PendingValues + Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value;
                    }
                }

                AvailableDays = CurrentLeaveDetail.number_of_days - (EmployeeLeaveDetails.FirstOrDefault(e => e.emp_id == CurrentEmployee.employee_id && e.leave_detail_id == CurrentLeaveDetail.leave_detail_id).number_of_days == null ? 0 : EmployeeLeaveDetails.FirstOrDefault(e => e.emp_id == CurrentEmployee.employee_id && e.leave_detail_id == CurrentLeaveDetail.leave_detail_id).number_of_days);
                AvailableDays -= PendingValues;

                if (AppliedDays > AvailableDays)
                {
                    if (AvailableDays <= 0)
                    {
                        MessageBox.Show("Leave Entitlement has been Fully Consumed", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("This Employee Can Only Apply for " + AvailableDays.ToString() + " Days in total", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                }

                if (MaxLeaves > 0)
                {
                    if (MaxAvailable > MaxLeaves)
                    {

                        MessageBox.Show("Maximum applicable amount of this leave category for this month is exceeded by " + (MaxAvailable - MaxLeaves).ToString() + " leaves", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }

                    else if (EmployeeMaxLeaveDetails != null && EmployeeMaxLeaveDetails.Count() > 0)
                    {
                        Guid Ltype = Guid.Empty;

                        foreach (var MaxItem in EmployeeMaxLeaveDetails)
                        {
                            if (ListLeaveShiftDetails.Count(c => c.leave_type_id == MaxItem.leave_type_id) > 0)
                            {
                                //decimal? Applied = ListLeaveShiftDetails.Where(c=> c.leave_type_id == MaxItem.leave_type_id).Select(d=> Leave_types.FirstOrDefault(f=> d.leave_type_id == f.leave_type_id).value).Sum();

                                foreach (var shift in ListLeaveShiftDetails.Where(c => c.leave_type_id == MaxItem.leave_type_id))
                                {
                                    decimal? Applied = (EmployeeLeaves == null || EmployeeLeaves.Count() == 0) ? 0 : EmployeeLeaves.Where(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id && c.leave_type_id == MaxItem.leave_type_id && c.leave_date.Value.Month == shift.date.Month).Select(d => Leave_types.FirstOrDefault(f => d.leave_type_id == f.leave_type_id).value).Sum();
                                    decimal? Requested = ListLeaveShiftDetails.Where(c => c.leave_type_id == MaxItem.leave_type_id).Select(d => Leave_types.FirstOrDefault(f => d.leave_type_id == f.leave_type_id).value).Sum();

                                    if ((MaxItem.leave_type_qty * MaxItem.value) < (Applied + Requested))
                                    {
                                        Ltype = MaxItem.leave_type_id;
                                        break;
                                    }
                                }

                                if (Ltype != Guid.Empty)
                                    break;
                            }
                        }

                        if (Ltype != Guid.Empty)
                        {
                            MessageBox.Show("This Employee cannot apply any " + Leave_types.FirstOrDefault(c => c.leave_type_id == Ltype).name + " leave(s) for this month", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return false;
                        }
                    }
                }

                if (CurrentLeaveDetail.leave_category_id == new Guid("33EB2C34-1FEA-4E70-BCF9-7BAEFA7FA038"))
                {
                    DateTime leave_date = (DateTime)LeaveFromDate;
                    int NoOfDays = DateTime.DaysInMonth(leave_date.Year, leave_date.Month);
                    DateTime start_date = new DateTime(leave_date.Year, leave_date.Month, 1);
                    DateTime end_date = new DateTime(leave_date.Year, leave_date.Month, NoOfDays);
                    decimal count = 0;
                    count = serviceClient.GetEmployeeShortLeaveCount(CurrentEmployee.employee_id, (Guid)CurrentLeaveDetail.leave_category_id, start_date, end_date);
                    if(CurrentLeaveDetail.leave_detail_name == "Short Leave 1.5")
                    {
                        count += (decimal)0.75;
                    }
                    if (CurrentLeaveDetail.leave_detail_name == "Short Leave 01")
                    {
                        count += (decimal)0.50;
                    }
                    if (CurrentLeaveDetail.leave_detail_name == "Short Leave 0.5")
                    {
                        count += (decimal)0.25;
                    }

                    if(count > (decimal)1.5) {
                        MessageBox.Show("Monthly short leave amount has been exceeded");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidLeiuLeaveSave()
        {
            if (CurrentEmployee == null)
            {
                MessageBox.Show("Please Select an Employee and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (serviceClient.GetSupervisorCount(CurrentEmployee.employee_id, new Guid("EE0D8A55-5A31-4FDB-A36F-36643A26B1AA")) == 0)
            {
                MessageBox.Show("An employee should be at least under one supervisor", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (CurrentLeavePeriod == null)
            {
                MessageBox.Show("Please Select a Period and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (CurrentEmployeeLeiuLeaveList == null)
            {
                MessageBox.Show("Please Select an Earned Leiu Leave and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (ListLeaveShiftDetails == null || ListLeaveShiftDetails.Count == 0)
            {
                MessageBox.Show("Please Fetch Shift Details and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (CurrentLeaveShiftDetails == null)
            {
                MessageBox.Show("Please Select a Shift and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (CurrentLeaveType == null)
            {
                MessageBox.Show("Please Select a Leave Type and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            trns_LeavePool existleiuleave = EmployeeLeaves.FirstOrDefault(c => c.emp_id == CurrentEmployee.employee_id && c.leave_date == CurrentLeaveShiftDetails.date);
            if (existleiuleave != null)
            {
                MessageBox.Show("Leiu Leave Already Exist For The Selected Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        private void CheckLeaveDates()
        {
            RemainingDays = 0;
            TotalLeavedays = 0;
            PendingLeaves = 0;

            LeaveShiftDetails = null;
            ListLeaveShiftDetails.Clear();

            if (EmployeeLeaves != null && EmployeeLeaves.Count() > 0)
            {
                IEnumerable<trns_LeavePool> TempLeaves = EmployeeLeaves.Where(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id && c.leave_category_id == CurrentLeaveDetail.leave_category_id && c.is_pending_for_approval == true);
                if (TempLeaves != null)
                    PendingLeaves = TempLeaves.Count();
            }

            Employee_Leave_Detail_View dEmpLeave = EmployeeLeaveDetails.FirstOrDefault(e => e.emp_id == CurrentEmployee.employee_id && e.leave_detail_id == CurrentLeaveDetail.leave_detail_id);

            if (dEmpLeave != null)
            {
                RemainingDays = (decimal)dEmpLeave.remaining_days.Value;
                TotalLeavedays = (decimal)dEmpLeave.number_of_days.Value;
            }
        }

        private void CheckTypeQuantity()
        {
            Employee_Leave_Detail_View dEmpLeave = EmployeeLeaveDetails.FirstOrDefault(e => e.emp_id == CurrentEmployee.employee_id && e.leave_detail_id == CurrentLeaveDetail.leave_detail_id);

            if (CurrentLeaveShiftDetails != null)
            {
                if (CurrentLeaveType != null)
                    CurrentLeaveShiftDetails.leave_type_id = CurrentLeaveType.leave_type_id;

                if (dEmpLeave != null)
                {
                    RemainingDays = (decimal)dEmpLeave.remaining_days.Value;
                    TotalLeavedays = (decimal)dEmpLeave.number_of_days.Value;

                    foreach (var item in ListLeaveShiftDetails)
                    {
                        if (item.leave_type_id != null)
                        {
                            RemainingDays = RemainingDays - (decimal)(Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value);
                            TotalLeavedays = TotalLeavedays + (decimal)(Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value);
                        }
                    }
                }
            }
            else
            {
                RemainingDays = (decimal)dEmpLeave.remaining_days.Value;
                TotalLeavedays = (decimal)dEmpLeave.number_of_days.Value;
            }
        }

        #endregion

        public ICommand GetShifDetailsBtn
        {
            get { return new RelayCommand(GetShifts, GetShiftsCanExecute); }
        }

        private bool GetShiftsCanExecute()
        {
            if (CurrentEmployee != null && CurrentLeavePeriod != null)
                return true;
            else
                return false;
        }

        private void GetShifts()
        {
            RefreshShiftDetails();
        }

        public ICommand RemoveShiftDetailbtn
        {
            get { return new RelayCommand(RemoveShiftDetails, RemoveShiftDetailsCanExecute); }
        }

        private bool RemoveShiftDetailsCanExecute()
        {
            if (CurrentLeaveShiftDetails != null)
                return true;
            else
                return false;
        }

        private void RemoveShiftDetails()
        {
            if (ListLeaveShiftDetails != null && ListLeaveShiftDetails.Count > 0)
            {
                ListLeaveShiftDetails.Remove(CurrentLeaveShiftDetails);
                LeaveShiftDetails = null;
                LeaveShiftDetails = ListLeaveShiftDetails;
                CurrentLeaveShiftDetails = ListLeaveShiftDetails.FirstOrDefault();
                CheckTypeQuantity();
            }
        }
    }
}
