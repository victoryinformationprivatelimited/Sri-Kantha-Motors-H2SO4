using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Leave.Holiday_Detail
{
    public class BulkLeaveApplyViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient;

        public BulkLeaveApplyViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
            RefreshEmployeeSearch();
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
            set { _CurrentLeavePeriod = value; OnPropertyChanged("CurrentLeavePeriod"); if (CurrentLeavePeriod != null && Leavedetails != null && Leavedetails.Count() > 0) { FilterLeaveCategories(); } CurrentCategory = null; CurrentLeaveType = null; EmployeeSearch = null; }
        }

        private IEnumerable<mas_LeaveDetail> _Leavedetails;
        public IEnumerable<mas_LeaveDetail> Leavedetails
        {
            get { return _Leavedetails; }
            set { _Leavedetails = value; OnPropertyChanged("Leavedetails"); }
        }

        private mas_LeaveDetail _CurrentLeaveDetail;
        public mas_LeaveDetail CurrentLeaveDetail
        {
            get { return _CurrentLeaveDetail; }
            set { _CurrentLeaveDetail = value; OnPropertyChanged("CurrentLeaveDetail"); }
        }

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged("SelectedDate"); }
        }

        private IEnumerable<dtl_EmployeeLeave> _EmployeeLeaveDetails;
        public IEnumerable<dtl_EmployeeLeave> EmployeeLeaveDetails
        {
            get { return _EmployeeLeaveDetails; }
            set { _EmployeeLeaveDetails = value; OnPropertyChanged("EmployeeLeaveDetails"); }
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
            set { _CurrentLeaveType = value; OnPropertyChanged("CurrentLeaveType"); }
        }

        private IEnumerable<trns_LeavePool> _EmployeeLeaves;
        public IEnumerable<trns_LeavePool> EmployeeLeaves
        {
            get { return _EmployeeLeaves; }
            set { _EmployeeLeaves = value; OnPropertyChanged("EmployeeLeaves"); }
        }

        private IEnumerable<z_LeaveCategory> _LeaveCategoriesAll;
        public IEnumerable<z_LeaveCategory> LeaveCategoriesAll
        {
            get { return _LeaveCategoriesAll; }
            set { _LeaveCategoriesAll = value; OnPropertyChanged("LeaveCategoriesAll"); }
        }

        private IEnumerable<z_LeaveCategory> _LeaveCategories;
        public IEnumerable<z_LeaveCategory> LeaveCategories
        {
            get { return _LeaveCategories; }
            set { _LeaveCategories = value; OnPropertyChanged("LeaveCategories"); }
        }

        private z_LeaveCategory _CurrentCategory;
        public z_LeaveCategory CurrentCategory
        {
            get { return _CurrentCategory; }
            set { _CurrentCategory = value; OnPropertyChanged("CurrentCategory"); if (CurrentCategory != null) { CurrentLeaveType = null; EmployeeSearch = null; } if (CurrentLeavePeriod != null && CurrentCategory != null) CurrentLeaveDetail = Leavedetails.Count(c => c.leave_period_id == CurrentLeavePeriod.period_id && c.leave_category_id == CurrentCategory.leave_id) == 0 ? null : Leavedetails.FirstOrDefault(c => c.leave_period_id == CurrentLeavePeriod.period_id && c.leave_category_id == CurrentCategory.leave_id); }
        }

        private IEnumerable<LeavePoolView> _LeavePoolDetails;
        public IEnumerable<LeavePoolView> LeavePoolDetails
        {
            get { return _LeavePoolDetails; }
            set { _LeavePoolDetails = value; OnPropertyChanged("LeavePoolDetails"); }
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

        private string _RemainingDays;
        public string RemainingDays
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

        private IEnumerable<EmployeeSearchView> _employeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _employeeSearch; }
            set { _employeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        #region Select Button

        public ICommand SelectButton
        {
            get { return new RelayCommand(Select, SelectCanexcute); }
        }

        private bool SelectCanexcute()
        {
            if (CurrentLeavePeriod != null && CurrentCategory != null && CurrentLeaveType != null)
                return true;
            else
                return false;
        }

        void Select()
        {
            if (EmployeeLeaveDetails != null && EmployeeLeaveDetails.Count() > 0)
            {
                IEnumerable<dtl_EmployeeLeave> temp = EmployeeLeaveDetails.Where(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id && c.is_special == true && c.remaining_days > CurrentLeaveType.value);
                if (temp != null && temp.Count() > 0)
                {
                    try
                    {
                        EmployeeSearchViews = EmployeeSearchViews.Where(c => temp.Count(z => z.emp_id == c.employee_id) > 0);
                        EmployeeMultipleSearchWindow window = null;// new EmployeeMultipleSearchWindow(EmployeeSearchViews);
                        window.ShowDialog();
                        EmployeeSearch = null;
                        if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                            EmployeeSearch = window.viewModel.selectEmployeeList;
                        window.Close();
                        window = null;
                    }
                    catch (Exception)
                    {
                        
                        clsMessages.setMessage("No Employees Found under this Leave Period Or LeaveCategory");
                    }
                }

                else
                    clsMessages.setMessage("No Employees Found under this Leave Period Or LeaveCategory");
            }

            else
                clsMessages.setMessage("No Employees Found under this Leave Period Or LeaveCategory");

        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }

        private void Delete()
        {

        }

        private bool DeleteCanExecute()
        {
            return false;
        }

        private void Save()
        {
            try
            {
                if (SelectedDate != null && SelectedDate >= CurrentLeavePeriod.from_date && SelectedDate <= CurrentLeavePeriod.to_date)
                {
                    string DayExist = "";
                    string Pendingexist = "";

                    List<trns_LeavePool> LeavePoolSaveList = new List<trns_LeavePool>();

                    foreach (var item_leave in EmployeeSearch.ToList())
                    {
                        bool isExist = false;

                        if (LeavePoolDetails.Count(c => c.emp_id == item_leave.employee_id && c.leave_category_id == CurrentCategory.leave_id) > 0)
                        {
                            if (LeavePoolDetails.Count(c => c.emp_id == item_leave.employee_id && c.leave_category_id == CurrentCategory.leave_id && Convert.ToDateTime(c.leave_date).Date == SelectedDate.Date) >0)
                            {
                                isExist = true;
                                DayExist += item_leave.emp_id.ToString() + ", ";
                            }
                            else
                            {
                                decimal? PendingLeaves = 0;

                                IEnumerable<LeavePoolView> tempLeavepool = LeavePoolDetails.Where(c => c.emp_id == item_leave.employee_id && c.leave_category_id == CurrentCategory.leave_id && c.is_pending_for_approval == true);

                                foreach (var item in tempLeavepool)
                                {
                                    PendingLeaves += Leave_types.FirstOrDefault(c => c.leave_type_id == item.leave_type_id).value;
                                }

                                if (EmployeeLeaveDetails.FirstOrDefault(c => c.emp_id == item_leave.employee_id && c.leave_detail_id == CurrentLeaveDetail.leave_detail_id).number_of_days + PendingLeaves + CurrentLeaveType.value <= (CurrentLeaveDetail.number_of_days))
                                {
                                    trns_LeavePool newLeave = new trns_LeavePool();
                                    newLeave.pool_id = Guid.NewGuid();
                                    newLeave.emp_id = item_leave.employee_id;
                                    newLeave.leave_category_id = CurrentCategory.leave_id;
                                    newLeave.leave_date = SelectedDate.Date;
                                    newLeave.reason = Reason;
                                    newLeave.apply_date = DateTime.Now;
                                    newLeave.is_approved = false;
                                    newLeave.is_pending_for_approval = true;
                                    newLeave.is_rejected = false;
                                    newLeave.remarks = Remark;
                                    newLeave.leave_type_id = CurrentLeaveType.leave_type_id;
                                    newLeave.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                                    LeavePoolSaveList.Add(newLeave);
                                    isExist = true;
                                }

                                else
                                {
                                    isExist = true;
                                    Pendingexist += item_leave.emp_id.ToString() + ", ";
                                }
                            }
                        }

                        if (!isExist)
                        {
                            trns_LeavePool newLeave = new trns_LeavePool();
                            newLeave.pool_id = Guid.NewGuid();
                            newLeave.emp_id = item_leave.employee_id;
                            newLeave.leave_category_id = CurrentCategory.leave_id;
                            newLeave.leave_date = SelectedDate.Date;
                            newLeave.reason = Reason;
                            newLeave.apply_date = DateTime.Now;
                            newLeave.is_approved = false;
                            newLeave.is_pending_for_approval = true;
                            newLeave.is_rejected = false;
                            newLeave.remarks = Remark;
                            newLeave.leave_type_id = CurrentLeaveType.leave_type_id;
                            newLeave.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                            LeavePoolSaveList.Add(newLeave);
                        }
                    }

                    if (LeavePoolSaveList.Count > 0)
                    {
                        if (serviceClient.SaveBulkApplyLeave(LeavePoolSaveList.ToArray()))
                        {
                            if (DayExist != string.Empty && Pendingexist != string.Empty)
                                clsMessages.setMessage("Bulk Leave for " + SelectedDate.Date.ToString() + " Saved Successfully Except," + "\n\n" + DayExist + " Who have already applied for this day" + "\n" + Pendingexist + " Who have too many pending leaves.");
                            if (DayExist != string.Empty && Pendingexist == string.Empty)
                                clsMessages.setMessage("Bulk Leave for " + SelectedDate.Date.ToString() + " Saved Successfully Except" + "\n\n" + DayExist + "Who have already applied for this day.");
                            if (DayExist == string.Empty && Pendingexist != string.Empty)
                                clsMessages.setMessage("Bulk Leave for " + SelectedDate.Date.ToString() + " Saved Successfully Except" + "\n\n" + Pendingexist + "Who have too many pending leaves.");
                            if (DayExist == string.Empty && Pendingexist == string.Empty)
                                clsMessages.setMessage("Bulk Leave for " + SelectedDate.Date.ToString() + " Saved Successfully");
                            New();
                        }

                    }
                    else
                    {
                        if (DayExist != string.Empty && Pendingexist != string.Empty)
                            clsMessages.setMessage(DayExist + " Have already applied for this day" + "\n" + Pendingexist + " Have too many pending leaves.");
                        if (DayExist != string.Empty && Pendingexist == string.Empty)
                            clsMessages.setMessage(DayExist + " Have already applied for this day.");
                        if (DayExist == string.Empty && Pendingexist != string.Empty)
                            clsMessages.setMessage(Pendingexist + " have too many pending leaves.");
                    }
                }

                else
                    clsMessages.setMessage("Please Select a Valid Date within the Period");
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }

        private bool SaveCanExecute()
        {
            if (CurrentLeavePeriod != null && CurrentCategory != null && CurrentLeaveType != null && EmployeeSearch != null && EmployeeSearch.Count() > 0 && SelectedDate != null)
                return true;
            else
                return false;
        }

        public ICommand NewBtn
        {
            get { return new RelayCommand(New, NewCanExecute); }
        }

        private void New()
        {
            CurrentLeavePeriod = null;
            CurrentCategory = null;
            CurrentLeaveType = null;
            EmployeeSearch = null;
            CurrentLeaveDetail = null;

            refreshLeavePeriod();
            refreshLeaveCategories();
            refreshLeaveTypes();
            refreshEmpoyeeLeaveDetails();
            refreshLeaveDetiails();
            SelectedDate = DateTime.Now;
        }

        private bool NewCanExecute()
        {
            return true;

        }

        #region Refresh Method

        private void refreshLeavePeriod() //Leave Periods
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
            {
                this.LeavePeriods = e.Result;
            };
            this.serviceClient.GetLeavePeriodsAsync();
        }

        private void refreshEmpoyeeLeaveDetails()
        {

            this.serviceClient.GetEmployeeLeaveDetilCompleted += (s, e) =>
                {
                    this.EmployeeLeaveDetails = e.Result;
                };
            this.serviceClient.GetEmployeeLeaveDetilAsync();

        }

        private void refreshLeaveTypes()
        {
            this.serviceClient.GetAllLeaveTypesCompleted += (s, e) =>
            {
                this.Leave_types = e.Result;
            };
            this.serviceClient.GetAllLeaveTypesAsync();
        }

        private void refreshLeaveDetiails()
        {
            this.serviceClient.GetMasLeaveDetailsCompleted += (s, e) =>
            {
                this.Leavedetails = e.Result;
            };

            this.serviceClient.GetMasLeaveDetailsAsync();
        }

        private void refreshLeaveCategories()
        {
            try
            {
                serviceClient.GetLeaveCatergoriesCompleted += (s, e) =>
                {
                    LeaveCategoriesAll = e.Result;
                };
                serviceClient.GetLeaveCatergoriesAsync();
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        private void refreshLeavePoolData()
        {
            serviceClient.GetLaevePoolDataByPeriodCompleted += (s, e) =>
            {
                LeavePoolDetails = e.Result.Where(c=> c.is_rejected == false);
            };
            serviceClient.GetLaevePoolDataByPeriodAsync(CurrentLeavePeriod.period_id);
        }

        IEnumerable<EmployeeSearchView> EmployeeSearchViews;
        private void RefreshEmployeeSearch()
        {
            try
            {
                serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
                         {
                             try
                             {
                                 EmployeeSearchViews = e.Result.OrderBy(c => c.emp_id);
                             }
                             catch (Exception)
                             {
                                 clsMessages.setMessage("RefreshEmployeeSearch Error");
                             }

                         };
                serviceClient.GetEmloyeeSearchAsync();

            }
            catch (Exception ex)
            { System.Windows.MessageBox.Show(ex.Message.ToString()); }
        }


        #region Filters

        private void FilterLeaveCategories()
        {
            if (LeaveCategoriesAll != null)
            {
                LeaveCategories = LeaveCategoriesAll.Where(c => Leavedetails.Count(d => d.leave_category_id == c.leave_id && d.leave_period_id == CurrentLeavePeriod.period_id) > 0);
                refreshLeavePoolData();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
