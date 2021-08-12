using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using ERP.AttendanceService;
using System.Windows.Input;
using System.Windows;
using System.Collections;

namespace ERP.Leave
{
    public class ApplyBulkLeaveViewModel : ViewModelBase
    {
        #region Service
        ERPServiceClient serviceClient = new ERPServiceClient();
        AttendanceServiceClient serviceAttendance = new AttendanceServiceClient();
        #endregion
        List<ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView> ListLeaveShiftDetails;
        List<ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView> ListLeaveShiftDetailsSave;
        public ApplyBulkLeaveViewModel()
        {
            ListLeaveShiftDetails = new List<AttendanceService.EmployeeLeaveShiftDetailsNewView>();
            ListLeaveShiftDetailsSave = new List<AttendanceService.EmployeeLeaveShiftDetailsNewView>();
            this.serviceClient.GetLeavePeriodsCompleted += serviceClient_GetLeavePeriodsCompleted;
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodCompleted += serviceClient_GetEmployeeLeaveDetailsViewListByPeriodCompleted;
            this.serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodCompleted += serviceClient_GetEmployeeMaximumLeaveDetailsViewListByPeriodCompleted;
            this.serviceClient.GetMasLeaveDetailsByPeriodCompleted += serviceClient_GetMasLeaveDetailsByPeriodCompleted;
            this.serviceClient.GetEmployeesCompleted += serviceClient_GetEmployeesCompleted;
            this.serviceClient.GetAllLeaveTypesCompleted += serviceClient_GetAllLeaveTypesCompleted;
            this.serviceAttendance.GetShiftDetailsByDateNewCompleted += serviceAttendance_GetShiftDetailsByDateNewCompleted;
            this.serviceClient.GetLeavesByEmpIdByPeriodCompleted += serviceClient_GetLeavesByEmpIdByPeriodCompleted;
            LeavePeriods = null;
            RefreshLeavePeriod();
            RefreshEmployees();
            RefreshLeaveTypes();
            
            LeaveFromDate = DateTime.Now.Date;
            LeaveToDate = DateTime.Now.Date;

            IsSave = false;
            IsMaternity = false;
            IsBoolConvert = true;
        }         

        #region Propertices

        private IEnumerable<z_LeavePeriod> leavePeriods;

        public IEnumerable<z_LeavePeriod> LeavePeriods
        {
            get { return leavePeriods; }
            set { leavePeriods = value; OnPropertyChanged("LeavePeriods"); }
        }

        private z_LeavePeriod currentLeavePeriod;

        public z_LeavePeriod CurrentLeavePeriod
        {
            get { return currentLeavePeriod; }
            set { currentLeavePeriod = value; OnPropertyChanged("CurrentLeavePeriod"); }
        }

        private IEnumerable<ERP.ERPService.mas_Employee> employees;

        public IEnumerable<ERP.ERPService.mas_Employee> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged("Employees"); }
        }

        private ERP.ERPService.mas_Employee currentEmployee;

        public ERP.ERPService.mas_Employee CurrentEmployee
        {
            get { return currentEmployee; }
            set
            {
                currentEmployee = value; OnPropertyChanged("CurrentEmployee");

                if (CurrentEmployee != null)
                {
                    FilterEmployeeLeaveData();
                    RefreshEmpLeave();
                }
            }
        }

        private IEnumerable<ERP.ERPService.mas_Employee> employeesAll;

        public IEnumerable<ERP.ERPService.mas_Employee> EmployeesAll
        {
            get { return employeesAll; }
            set { employeesAll = value; OnPropertyChanged("EmployeesAll"); }
        }
        
        private IEnumerable<Employee_Leave_Detail_View> employeeLeaveDetailsAll;

        public IEnumerable<Employee_Leave_Detail_View> EmployeeLeaveDetailsAll
        {
            get { return employeeLeaveDetailsAll; }
            set { employeeLeaveDetailsAll = value; OnPropertyChanged("EmployeeLeaveDetailsAll"); }
        }

        private IEnumerable<Employee_Leave_Detail_View> employeeLeaveDetails;

        public IEnumerable<Employee_Leave_Detail_View> EmployeeLeaveDetails
        {
            get { return employeeLeaveDetails; }
            set { employeeLeaveDetails = value; OnPropertyChanged("EmployeeLeaveDetails"); }
        }
        
        private IEnumerable<Employee_Maximum_Leave_Details_View> employeeMaxLeaveDetailsAll;

        public IEnumerable<Employee_Maximum_Leave_Details_View> EmployeeMaxLeaveDetailsAll
        {
            get { return employeeMaxLeaveDetailsAll; }
            set { employeeMaxLeaveDetailsAll = value; OnPropertyChanged("EmployeeMaxLeaveDetailsAll"); }
        }

        private IEnumerable<mas_LeaveDetail> leavedetailsAll;

        public IEnumerable<mas_LeaveDetail> LeavedetailsAll
        {
            get { return leavedetailsAll; }
            set { leavedetailsAll = value; OnPropertyChanged("LeavedetailsAll"); }
        }

        private IEnumerable<mas_LeaveDetail> leavedetails;
        public IEnumerable<mas_LeaveDetail> Leavedetails
        {
            get { return leavedetails; }
            set { leavedetails = value; OnPropertyChanged("Leavedetails");  }
        }

        private mas_LeaveDetail currentLeaveDetail;
        public mas_LeaveDetail CurrentLeaveDetail
        {
            get { return currentLeaveDetail; }
            set
            {
                currentLeaveDetail = value; OnPropertyChanged("CurrentLeaveDetail"); 
            }
        }

        private IEnumerable<ERP.ERPService.z_LeaveType> leave_types;
        public IEnumerable<ERP.ERPService.z_LeaveType> Leave_types
        {
            get { return leave_types; }
            set { leave_types = value; OnPropertyChanged("Leave_types"); }
        }

        private ERP.ERPService.z_LeaveType currentLeaveType;
        public ERP.ERPService.z_LeaveType CurrentLeaveType
        {
            get { return currentLeaveType; }
            set { currentLeaveType = value; OnPropertyChanged("CurrentLeaveType");  }
        }

        private DateTime? leaveFromDate;
        public DateTime? LeaveFromDate
        {
            get { return leaveFromDate; }
            set
            {
                leaveFromDate = value; OnPropertyChanged("LeaveFromDate");
                if (IsMaternity && MaternityLeave != null)
                {
                    LeaveToDate = LeaveFromDate.Value.AddDays(MaternityLeave);
                }
            }
        }

        private DateTime? leaveToDate;
        public DateTime? LeaveToDate
        {
            get { return leaveToDate; }
            set { leaveToDate = value; OnPropertyChanged("LeaveToDate");  }
        }

        private IEnumerable<ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView> leaveShiftDetails;
        public IEnumerable<ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView> LeaveShiftDetails
        {
            get { return leaveShiftDetails; }
            set { leaveShiftDetails = value; OnPropertyChanged("LeaveShiftDetails");  }
        }

        private ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView currentLeaveShiftDetails;
        public ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView CurrentLeaveShiftDetails
        {
            get { return currentLeaveShiftDetails; }
            set { currentLeaveShiftDetails = value; OnPropertyChanged("CurrentLeaveShiftDetails"); }
        }

        private IList _CurrentLeaveShiftDetailslist = new ArrayList();
        public IList CurrentLeaveShiftDetailslist
        {
            get { return _CurrentLeaveShiftDetailslist; }
            set { _CurrentLeaveShiftDetailslist = value; OnPropertyChanged("CurrentLeaveShiftDetailslist"); }
        }

        private IEnumerable<ERP.ERPService.trns_LeavePool> _EmployeeLeaves;
        public IEnumerable<ERP.ERPService.trns_LeavePool> EmployeeLeaves
        {
            get { return _EmployeeLeaves; }
            set { _EmployeeLeaves = value; OnPropertyChanged("EmployeeLeaves"); }
        }

        private bool isSave;

        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; OnPropertyChanged("IsSave"); }
        }

        private IList listLeaveCategory;

        public IList ListLeaveCategory
        {
            get { return listLeaveCategory; }
            set { listLeaveCategory = value; OnPropertyChanged("ListLeaveCategory"); }
        }

        private bool isMaternity;

        public bool IsMaternity
        {
            get { return isMaternity; }
            set
            {
                isMaternity = value; OnPropertyChanged("IsMaternity");
                if (IsMaternity)
                {
                    IsBoolConvert = false;
                }
                else
                {
                    IsBoolConvert = true;
                }
            }
        }

        private int maternityLeave;

        public int MaternityLeave
        {
            get { return maternityLeave; }
            set
            {
                maternityLeave = value; OnPropertyChanged("MaternityLeave");
                if (IsMaternity && MaternityLeave != null)
                {
                    LeaveToDate = LeaveFromDate.Value.AddDays(MaternityLeave);
                }
                else if (!IsMaternity)
                    MaternityLeave = 0;
            }
        }
        

        private bool isBoolConvert;

        public bool IsBoolConvert
        {
            get { return isBoolConvert; }
            set { isBoolConvert = value; OnPropertyChanged("IsBoolConvert"); }
        }
        
        
        #endregion

        #region Refresh and void Mehods

        private void buttonNew()
        {
            CurrentLeaveDetail = null;
            CurrentEmployee = null;
            CurrentLeaveType = null;
            CurrentLeaveShiftDetails = null;
            LeaveShiftDetails = null;
            Employees = null;
            ListLeaveShiftDetails.Clear();
            ListLeaveShiftDetailsSave.Clear();
            LeavePeriods = null;
            RefreshLeavePeriod();
            RefreshEmployees();
            RefreshLeaveTypes();
            LeaveFromDate = DateTime.Now.Date;
            LeaveToDate = DateTime.Now.Date;

            GetLeavesForPeriod();
            IsSave = false;
            IsMaternity = false;
            IsBoolConvert = true;
        }

        private void GetLeavesForPeriod()
        {
            if (BoolLeavesForPeriod())
            {                
                RefreshEmpoyeeLeaveDetails();
                RefreshEmployeeMaximumLeaves();
                RefreshLeaveDetiails(); 
            }
        }

        void serviceClient_GetLeavePeriodsCompleted(object sender, GetLeavePeriodsCompletedEventArgs e)
        {
            try
            {
                LeavePeriods = e.Result;
            }
            catch (Exception)
            {

                clsMessages.setMessage("Leave Period Failed To Refresh...");
            }
        }

        private void RefreshLeavePeriod()
        {
            serviceClient.GetLeavePeriodsAsync();
        }

        void serviceClient_GetEmployeeLeaveDetailsViewListByPeriodCompleted(object sender, GetEmployeeLeaveDetailsViewListByPeriodCompletedEventArgs e)
        {
            try
            {
                EmployeeLeaveDetailsAll = e.Result.Where(c => c.is_special == true);
                FilterLeaveData();
            }
            catch (Exception)
            {
                
                 clsMessages.setMessage("Leave Details Failed To Refresh...");
            }
        }

        private void RefreshEmpoyeeLeaveDetails()
        {
            serviceClient.GetEmployeeLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriod.period_id);
        }

        void serviceClient_GetEmployeeMaximumLeaveDetailsViewListByPeriodCompleted(object sender, GetEmployeeMaximumLeaveDetailsViewListByPeriodCompletedEventArgs e)
        {
            try
            {
                EmployeeMaxLeaveDetailsAll = e.Result;
            }
            catch (Exception)
            {
                
                clsMessages.setMessage("Employee Maximum Leave Details Failed To Refresh...");
            }
        }             

        private void RefreshEmployeeMaximumLeaves()
        {
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriod.period_id);
        }

        void serviceClient_GetMasLeaveDetailsByPeriodCompleted(object sender, GetMasLeaveDetailsByPeriodCompletedEventArgs e)
        {
            try
            {
                LeavedetailsAll = e.Result;
            }
            catch (Exception)
            {
                clsMessages.setMessage("Leave Details Failed To Refresh...");
            }
        }

        private void RefreshLeaveDetiails()
        {
            serviceClient.GetMasLeaveDetailsByPeriodAsync(CurrentLeavePeriod.period_id);
        }

        void serviceClient_GetEmployeesCompleted(object sender, GetEmployeesCompletedEventArgs e)
        {
            try
            {
                EmployeesAll = e.Result.OrderBy(c => c.emp_id);
                FilterLeaveData();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Details Failed To Refresh...");
            }
        }

        private void RefreshEmployees()
        {
            serviceClient.GetEmployeesAsync();
        }

        void serviceClient_GetAllLeaveTypesCompleted(object sender, GetAllLeaveTypesCompletedEventArgs e)
        {
            try
            {
                Leave_types = e.Result;
            }
            catch (Exception)
            {
                
                clsMessages.setMessage("Leave Types Failed To Refresh...");
            }
        }

        private void RefreshLeaveTypes()
        {
            serviceClient.GetAllLeaveTypesAsync();
        }

        void serviceClient_GetLeavesByEmpIdByPeriodCompleted(object sender, GetLeavesByEmpIdByPeriodCompletedEventArgs e)
        {
            try
            {
                EmployeeLeaves = e.Result.Where(c => c.is_rejected == false);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Leaves Failed To Refresh...");
            }
        }

        private void RefreshEmpLeave()
        {
            serviceClient.GetLeavesByEmpIdByPeriodAsync(CurrentEmployee.employee_id, CurrentLeavePeriod.period_id);
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
            if (GetShiftsCanExecute())
            {
                RefreshShiftDetails();
            }
           
        }
        void serviceAttendance_GetShiftDetailsByDateNewCompleted(object sender, GetShiftDetailsByDateNewCompletedEventArgs e)
        {
            try
            {
                LeaveShiftDetails = e.Result;
                if (LeaveShiftDetails != null && LeaveShiftDetails.Count() > 0)
                    ListLeaveShiftDetails = LeaveShiftDetails.ToList();
                else
                    clsMessages.setMessage("No Shift Details Found!");
            }
            catch (Exception)
            {

                clsMessages.setMessage("Shift Details Failed To Refresh...");
            }
        }

        private void RefreshShiftDetails()
        {
            this.serviceAttendance.GetShiftDetailsByDateNewAsync(CurrentEmployee.employee_id, (DateTime)LeaveFromDate, (DateTime)LeaveToDate);
        }

        private void applyLeave()
        {
            if (ValidateSave())
            {
                if (ListLeaveShiftDetails != null && ListLeaveShiftDetails.Any())
                {
                    decimal? MaxLeaves = 0;
                    decimal? RemainingDays = 0;

                    IEnumerable<Employee_Maximum_Leave_Details_View> EmployeeMaxLeaveDetails;

                    MaxLeaves = EmployeeLeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id) == null ? 0 : EmployeeLeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id).maximum_leaves;
                    EmployeeMaxLeaveDetails = (EmployeeMaxLeaveDetailsAll == null || EmployeeMaxLeaveDetailsAll.Count() == 0) ? null : EmployeeMaxLeaveDetailsAll.Where(c => c.emp_id == CurrentEmployee.employee_id && c.leave_detail_id == CurrentLeaveDetail.leave_detail_id);

                    Employee_Leave_Detail_View dEmpLeave = EmployeeLeaveDetails.FirstOrDefault(e => e.emp_id == CurrentEmployee.employee_id && e.leave_detail_id == CurrentLeaveDetail.leave_detail_id);

                    if (dEmpLeave != null)
                    {
                        RemainingDays = (decimal)dEmpLeave.remaining_days.Value;
                    }

                    if (CurrentLeaveShiftDetails != null)
                    {

                        foreach (var item in ListLeaveShiftDetails)
                        {
                            if (item.leave_type_id != null && item.leave_detail_id != null )
                            {
                                //if (item.leave_type_id ==  CurrentLeaveType.leave_type_id && item.leave_detail_id == CurrentLeaveDetail.leave_detail_id )
                                //{
                                    RemainingDays = RemainingDays - (decimal)(Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value); 
                                //}
                            }
                        }

                        List<ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView> temp = new List<EmployeeLeaveShiftDetailsNewView>();
                        List<ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView> temp1 = new List<EmployeeLeaveShiftDetailsNewView>();
                        // LeaveShiftDetails
                        foreach (ERP.AttendanceService.EmployeeLeaveShiftDetailsNewView item in CurrentLeaveShiftDetailslist)
                        {
                            RemainingDays = RemainingDays - (decimal)(Leave_types.FirstOrDefault(c => c.leave_type_id == CurrentLeaveType.leave_type_id).value);
                            if (RemainingDays >= 0)
                            {
                                item.leave_type_id = CurrentLeaveType.leave_type_id;
                                item.LeaveTypeName = CurrentLeaveType.name;
                                item.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                                item.leave_detail_name = CurrentLeaveDetail.leave_detail_name;
                                item.reason = CurrentLeaveShiftDetails.reason;
                                item.remarks = "";
                                item.leave_category_id = CurrentLeaveDetail.leave_category_id;
                                temp.Add(item);
                            }
                        }

                        temp1 = LeaveShiftDetails.Where(c => !temp.Any(d => d.shift_detail_id == c.shift_detail_id && d.date == c.date)).ToList();
                        foreach (var item in temp1)
                        {
                            temp.Add(item);
                        }

                        LeaveShiftDetails = null;
                        LeaveShiftDetails = temp.OrderBy(c => c.date);

                        ListLeaveShiftDetailsSave = null;
                        ListLeaveShiftDetailsSave = LeaveShiftDetails.Where(c => c.leave_detail_id != null && c.leave_type_id != null).ToList();
                        bool exists = false;

                        if (ListLeaveShiftDetails != null && ListLeaveShiftDetails.Count > 0)
                        {
                            decimal? PendingValuesByday = 0;
                            if (EmployeeLeaves != null && EmployeeLeaves.Count() > 0)
                            {                               

                                foreach (var Shift in ListLeaveShiftDetails)
                                {
                                    if (EmployeeLeaves.Count(c => c.leave_date == Shift.date) > 0)
                                    {
                                        IEnumerable<ERP.ERPService.trns_LeavePool> tempLeave = EmployeeLeaves.Where(c => c.leave_date == Shift.date);
                                        foreach (var item in tempLeave)
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

                                        if (Shift.leave_type_id != null)
                                        {
                                            if (PendingValuesByday + Leave_types.FirstOrDefault(c => c.leave_type_id == Shift.leave_type_id).value > 1)
                                            {
                                                MessageBox.Show("Leave Data for '" + Shift.date + "' Already Exists", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                                exists = true;
                                                break;
                                            }
                                        }
                                    }

                                    PendingValuesByday = 0;
                                }

                            }
                            else
                                exists = false;
                        }

                        if (exists)
                        {
                            IsSave = false;
                        }
                        else
                            IsSave = true;
                    }                    
                }
            }      
              
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
            else if (CurrentLeaveType == null)
            {
                MessageBox.Show("Please Select a Leave Type and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if (ListLeaveShiftDetails == null || ListLeaveShiftDetails.Count == 0)
            {
                MessageBox.Show("Please Fetch Shift Details and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }                  
            else if (ListLeaveShiftDetails.Any(c => c.date < CurrentLeavePeriod.from_date.Value || c.date > CurrentLeavePeriod.to_date.Value))
            {
                MessageBox.Show("Please Select Valid Leave Dates Within the Selected Period and Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            
            else
                return true;
        }

        private void buttonSave()
        {
            try
            {
                if (ListLeaveShiftDetailsSave.Any())
                {
                    List<ERP.ERPService.trns_LeavePool> Savelist = new List<ERP.ERPService.trns_LeavePool>();
                    //int DateCount = LeaveToDate.Value.Subtract(LeaveFromDate.Value).Days + 1;

                    foreach (var Shift in ListLeaveShiftDetailsSave)
                    {
                        ERP.ERPService.trns_LeavePool newLeave = new ERP.ERPService.trns_LeavePool();
                        newLeave.pool_id = Guid.NewGuid();
                        newLeave.emp_id = Shift.employee_id;
                        newLeave.leave_category_id = Shift.leave_category_id;
                        newLeave.leave_date = Shift.date;
                        newLeave.reason = Shift.reason;
                        newLeave.apply_date = DateTime.Now.Date;
                        newLeave.is_approved = false;
                        newLeave.is_pending_for_approval = true;
                        newLeave.is_rejected = false;
                        newLeave.remarks = Shift.remarks;
                        newLeave.leave_type_id = Shift.leave_type_id;
                        newLeave.leave_detail_id = Shift.leave_detail_id;
                        newLeave.shift_detail_id = Shift.shift_detail_id;
                        Savelist.Add(newLeave);
                    }

                    if (this.serviceClient.SaveLeavePool(Savelist.ToArray()))
                    {
                        MessageBox.Show("Leave Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        buttonNew();
                    }
                    else
                        MessageBox.Show("Leave Save Failed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
                else
                    MessageBox.Show("Request Cannot be Completed", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Request Cannot be Completed", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool ValidateRemove()
        {
            if (CurrentLeaveShiftDetails != null)
                return true;
            else
                return false;
        }

        private void RemoveShiftDetails()
        {
            if (ValidateRemove())
            {
                if (ListLeaveShiftDetails != null && ListLeaveShiftDetails.Count > 0)
                {
                    ListLeaveShiftDetails.Remove(CurrentLeaveShiftDetails);
                    ListLeaveShiftDetailsSave.Remove(CurrentLeaveShiftDetails);
                    LeaveShiftDetails = null;
                    LeaveShiftDetails = ListLeaveShiftDetails;
                    CurrentLeaveShiftDetails = ListLeaveShiftDetails.FirstOrDefault();
                }
            }
        }

        private bool BoolLeavesForPeriod()
        {
            if (CurrentLeavePeriod != null)
                return true;
            else
                return false;
        } 

        #endregion

        #region Filter
        private void FilterLeaveData()
        {
            if ((EmployeesAll != null && EmployeesAll.Any()) && (EmployeeLeaveDetailsAll != null && EmployeeLeaveDetailsAll.Any()))
            {
                Employees = EmployeesAll.Where(c => EmployeeLeaveDetailsAll.Count(d => c.employee_id == d.emp_id) > 0);
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

        #endregion
        public ICommand BtnGetLeavesForPeriod
        {
            get { return new RelayCommand(GetLeavesForPeriod ); }
        }

        public ICommand GetShifDetailsBtn
        {
            get { return new RelayCommand(GetShifts ); }
        }

        public ICommand ApplyBtn
        {
            get { return new RelayCommand(applyLeave); }
        }

        public ICommand BtnNew
        {
            get { return new RelayCommand(buttonNew); }
        }

        public ICommand BtnSave
        {
            get { return new RelayCommand(buttonSave); }
        }

        public ICommand RemoveShiftDetailbtn
        {
            get { return new RelayCommand(RemoveShiftDetails); }
        }
    }
}
