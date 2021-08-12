using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Leave;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.MastersDetails
{
    public class EmployeeLeaveDetailViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Fields

        List<Employee_Leave_Detail_View> ListEmployeeLeaveDetails;
        List<Employee_Maximum_Leave_Details_View> ListEmployeeMaxLeaveDetails;
        MaximumLeavesWindow window;

        #endregion

        #region Constructor

        public EmployeeLeaveDetailViewModel()
        {
            ListEmployeeLeaveDetails = new List<Employee_Leave_Detail_View>();
            ListEmployeeMaxLeaveDetails = new List<Employee_Maximum_Leave_Details_View>();
            refreshLeavePeriods();
            refreshLeaveTypes();
            this.New();
            IsRefreshed = false;
        }

        #endregion

        #region Properties

        private bool _IsRefreshed;
        public bool IsRefreshed
        {
            get { return _IsRefreshed; }
            set { _IsRefreshed = value; OnPropertyChanged("IsRefreshed"); }
        }

        private bool _EnableTransfer;
        public bool EnableTransfer
        {
            get { return _EnableTransfer; }
            set { _EnableTransfer = value; OnPropertyChanged("EnableTransfer"); }
        }
        

        private decimal _LeaveTypeRemaining;
        public decimal LeaveTypeRemaining
        {
            get { return _LeaveTypeRemaining; }
            set { _LeaveTypeRemaining = value; OnPropertyChanged("LeaveTypeRemaining"); }
        }

        private IEnumerable<z_LeaveType> _LeaveTypes;
        public IEnumerable<z_LeaveType> LeaveTypes
        {
            get { return _LeaveTypes; }
            set { _LeaveTypes = value; OnPropertyChanged("LeaveTypes"); }
        }

        private z_LeaveType _CurrentLeaveType;
        public z_LeaveType CurrentLeaveType
        {
            get { return _CurrentLeaveType; }
            set { _CurrentLeaveType = value; OnPropertyChanged("CurrentLeaveType"); }
        }

        private IEnumerable<Employee_Maximum_Leave_Details_View> _EmployeeMaxLeaveDetails;
        public IEnumerable<Employee_Maximum_Leave_Details_View> EmployeeMaxLeaveDetails
        {
            get { return _EmployeeMaxLeaveDetails; }
            set { _EmployeeMaxLeaveDetails = value; OnPropertyChanged("EmployeeMaxLeaveDetails"); }
        }

        private Employee_Maximum_Leave_Details_View _CurrentEmployeeMaxLeaveDetail;
        public Employee_Maximum_Leave_Details_View CurrentEmployeeMaxLeaveDetail
        {
            get { return _CurrentEmployeeMaxLeaveDetail; }
            set { _CurrentEmployeeMaxLeaveDetail = value; OnPropertyChanged("CurrentEmployeeMaxLeaveDetail"); }
        }

        private IEnumerable<z_LeavePeriod> _LeavePeriods;
        public IEnumerable<z_LeavePeriod> LeavePeriods
        {
            get
            {
                return this._LeavePeriods;
            }
            set
            {
                this._LeavePeriods = value;
                this.OnPropertyChanged("LeavePeriods");
            }
        }

        private z_LeavePeriod _CurrentLeavePeriods;
        public z_LeavePeriod CurrentLeavePeriods
        {
            get
            {
                return this._CurrentLeavePeriods;
            }
            set
            {
                this._CurrentLeavePeriods = value;
                this.OnPropertyChanged("CurrentLeavePeriods");
                if (CurrentLeavePeriods != null)
                {
                    EmployeeLeaveDetailView = null;
                    ListEmployeeLeaveDetails.Clear();
                    EmployeeMaxLeaveDetails = null;
                    ListEmployeeMaxLeaveDetails.Clear();
                    LeaveDetails = null;
                    CurrentEmployeeLeaveDetailView = null;
                    CurrentEmployeeLeaveDetailView = new Employee_Leave_Detail_View();
                }
            }
        }

        private IEnumerable<EmployeeSearchView> _EmployeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _EmployeeSearch; }
            set { _EmployeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        private IEnumerable<Employee_Leave_Detail_View> _EmployeeLeaveDetailView;
        public IEnumerable<Employee_Leave_Detail_View> EmployeeLeaveDetailView
        {
            get
            {
                return this._EmployeeLeaveDetailView;
            }
            set
            {
                this._EmployeeLeaveDetailView = value;
                this.OnPropertyChanged("EmployeeLeaveDetailView");
            }
        }

        private Employee_Leave_Detail_View _CurrentEmployeeLeaveDetailView;
        public Employee_Leave_Detail_View CurrentEmployeeLeaveDetailView
        {
            get
            {
                return this._CurrentEmployeeLeaveDetailView;
            }
            set
            {
                this._CurrentEmployeeLeaveDetailView = value;
                this.OnPropertyChanged("CurrentEmployeeLeaveDetailView");
                if (CurrentEmployeeLeaveDetailView == null || CurrentEmployeeLeaveDetailView.emp_id == Guid.Empty)
                    EnableTransfer = true;
                else
                    EnableTransfer = false;
            }
        }

        private IEnumerable<mas_LeaveDetail> _LeaveDetails;
        public IEnumerable<mas_LeaveDetail> LeaveDetails
        {
            get
            {
                return this._LeaveDetails;
            }
            set
            {
                this._LeaveDetails = value;
                this.OnPropertyChanged("LeaveDetails");
            }
        }

        private mas_LeaveDetail _CurrentLeaveDetail;
        public mas_LeaveDetail CurrentLeaveDetail
        {
            get
            {
                return this._CurrentLeaveDetail;
            }
            set
            {
                this._CurrentLeaveDetail = value;
                this.OnPropertyChanged("CurrentLeaveDetail");
                if (CurrentLeaveDetail != null && ListEmployeeLeaveDetails != null && ListEmployeeLeaveDetails.Count > 0)
                {
                    EmployeeLeaveDetailView = null;
                    EmployeeLeaveDetailView = ListEmployeeLeaveDetails.Where(c => c.leave_detail_id == CurrentLeaveDetail.leave_detail_id);
                }
            }
        }

        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (this.Search != null)
                    SearchLeaveDetails();

            }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        #endregion

        #region Buttons and Commands

        #region Get Employees

        public ICommand BtnSelectEmployees
        {
            get { return new RelayCommand(SelectEmployees, SelectEmployeesCE); }
        }

        private bool SelectEmployeesCE()
        {
            if (CurrentEmployeeLeaveDetailView != null && IsRefreshed == true)
                return true;
            else
                return false;

        }

        private void SelectEmployees()
        {
            if (ValidateGetEmployees())
            {
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();// (EmployeeSearch);
                window.ShowDialog();
                if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                {
                    foreach (EmployeeSearchView employee in window.viewModel.selectEmployeeList)
                    {
                        //if (ListEmployeeLeaveDetails.Count(c => c.emp_id == employee.employee_id && c.leave_category_id == CurrentLeaveDetail.leave_category_id) == 0)
                        //{
                            Employee_Leave_Detail_View TempObj = new Employee_Leave_Detail_View();
                            TempObj.emp_id = employee.employee_id;
                            TempObj.EMPLOYEE_ID = employee.emp_id;
                            TempObj.first_name = employee.first_name;
                            TempObj.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                            TempObj.leave_detail_name = CurrentLeaveDetail.leave_detail_name;
                            TempObj.number_of_days = CurrentEmployeeLeaveDetailView.number_of_days;
                            TempObj.remaining_days = CurrentEmployeeLeaveDetailView.remaining_days + (CurrentEmployeeLeaveDetailView.trns_value == null ? 0 : CurrentEmployeeLeaveDetailView.trns_value);
                            TempObj.maximum_leaves = CurrentEmployeeLeaveDetailView.maximum_leaves;
                            TempObj.trns_value = CurrentEmployeeLeaveDetailView.trns_value == null ? 0 : CurrentEmployeeLeaveDetailView.trns_value;
                            TempObj.is_special = true;

                            ListEmployeeLeaveDetails.Add(TempObj);
                        //}

                        if (ListEmployeeMaxLeaveDetails != null && ListEmployeeMaxLeaveDetails.Count() > 0)
                        {
                            foreach (var LeaveItem in ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == Guid.Empty).ToList())
                            {
                                Employee_Maximum_Leave_Details_View Temp = new Employee_Maximum_Leave_Details_View();
                                Temp.leave_type_id = LeaveItem.leave_type_id;
                                Temp.leave_type_qty = LeaveItem.leave_type_qty;
                                Temp.emp_id = employee.employee_id;
                                Temp.leave_detail_id = LeaveItem.leave_detail_id;
                                Temp.name = LeaveItem.name;
                                Temp.value = LeaveItem.value;

                                ListEmployeeMaxLeaveDetails.Add(Temp);
                            }
                        }
                    }

                    EmployeeLeaveDetailView = null;
                    EmployeeLeaveDetailView = CurrentLeaveDetail == null ? ListEmployeeLeaveDetails : ListEmployeeLeaveDetails.Where(c=> c.leave_detail_id == CurrentLeaveDetail.leave_detail_id);
                }

            }
        }

        private bool ValidateGetEmployees()
        {
            if (CurrentEmployeeLeaveDetailView.leave_detail_id == null || CurrentEmployeeLeaveDetailView.leave_detail_id == Guid.Empty)
            {
                clsMessages.setMessage("Please select a leave type from 'Leave Detail Names'");
                return false;
            }

            else if (CurrentEmployeeLeaveDetailView.number_of_days == null || CurrentEmployeeLeaveDetailView.number_of_days < 0)
            {
                clsMessages.setMessage("Please Enter Number Of Days Used Up");
                return false;
            }

            else if (LeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id).number_of_days < CurrentEmployeeLeaveDetailView.number_of_days)
            {
                clsMessages.setMessage("Invalid Number Of Used Up Days");
                return false;
            }

            else if (CurrentEmployeeLeaveDetailView.remaining_days == null || CurrentEmployeeLeaveDetailView.remaining_days < 0)
            {
                clsMessages.setMessage("Please Enter Remaining Number Of Days");
                return false;
            }

            else if (LeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id).number_of_days < CurrentEmployeeLeaveDetailView.remaining_days)
            {
                clsMessages.setMessage("Invalid Remaining Number Of Days");
                return false;
            }

            else if (LeaveDetails.FirstOrDefault(c => c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id).number_of_days != CurrentEmployeeLeaveDetailView.number_of_days + CurrentEmployeeLeaveDetailView.remaining_days)
            {
                clsMessages.setMessage("Either 'Number Of Used Up Days' Or 'Remaining Number Of Days' is Invalid");
                return false;
            }

            else
                return true;

        }

        #endregion

        #region Get Leave Data

        public ICommand BtnGetLeaveData
        {
            get { return new RelayCommand(GetLeaveData, GetLeaveDataCE); }
        }

        private bool GetLeaveDataCE()
        {
            if (CurrentLeavePeriods != null)
                return true;
            else
                return false;
        }

        private void GetLeaveData()
        {
            reafreshLeaveDetails();
            reafreshEmployeeLeaveDetailsViewList();
            refreshEmployeeMaxLeaveDetailsViewList();
        }


        #endregion

        #region New Method

        void New()
        {
            try
            {
                SearchIndex = 0;
                Search = "";
                CurrentEmployeeLeaveDetailView = null;
                CurrentEmployeeLeaveDetailView = new Employee_Leave_Detail_View();
                CurrentLeaveDetail = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region NewButton Class & Property
        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        #endregion

        #region Save Method

        void Save()
        {
            if (ValidateSave())
            {
                if (clsSecurity.GetSavePermission(405))
                {
                    List<dtl_EmployeeLeave> SaveList = new List<dtl_EmployeeLeave>();
                    List<dtl_EmployeeMaximumLeaves> SaveMaxList = new List<dtl_EmployeeMaximumLeaves>();
                    List<trns_MigratedLeaveData> MigratedSaveList = new List<trns_MigratedLeaveData>();

                    foreach (var employee in ListEmployeeLeaveDetails)
                    {

                        dtl_EmployeeLeave SaveObj = new dtl_EmployeeLeave();
                        trns_MigratedLeaveData SaveMig = new trns_MigratedLeaveData();

                        SaveObj.leave_detail_id = employee.leave_detail_id;
                        SaveObj.emp_id = employee.emp_id;
                        SaveObj.number_of_days = employee.number_of_days;
                        SaveObj.remaining_days = employee.remaining_days;
                        SaveObj.maximum_leaves = (employee.maximum_leaves == null || employee.maximum_leaves < 0) ? 0 : employee.maximum_leaves;
                        SaveObj.is_special = employee.is_special;

                        SaveMig.leave_detail_id = employee.leave_detail_id;
                        SaveMig.employee_id = employee.emp_id;
                        SaveMig.is_payed = false;
                        SaveMig.is_transferred = true;
                        SaveMig.value = employee.trns_value == null ? 0 : (decimal)employee.trns_value;
                        SaveMig.save_user_id = clsSecurity.loggedUser.user_id;
                        SaveMig.save_datetime = DateTime.Now;

                        SaveList.Add(SaveObj);
                        MigratedSaveList.Add(SaveMig);
                    }

                    if (ListEmployeeMaxLeaveDetails != null && ListEmployeeMaxLeaveDetails.Count > 0)
                    {

                        foreach (var item in ListEmployeeMaxLeaveDetails.Where(c => c.emp_id != Guid.Empty))
                        {
                            dtl_EmployeeMaximumLeaves MaxSaveObj = new dtl_EmployeeMaximumLeaves();

                            MaxSaveObj.emp_id = item.emp_id;
                            MaxSaveObj.leave_detail_id = item.leave_detail_id;
                            MaxSaveObj.leave_type_id = item.leave_type_id;
                            MaxSaveObj.leave_type_qty = item.leave_type_qty;

                            SaveMaxList.Add(MaxSaveObj);
                        }
                    }

                    if (serviceClient.SaveUpdateEmployeeLeaveDetails(SaveList.ToArray(), MigratedSaveList.ToArray(), SaveMaxList.ToArray()))
                    {
                        reafreshEmployeeLeaveDetailsViewList();
                        refreshEmployeeMaxLeaveDetailsViewList();
                        clsMessages.setMessage("Records Saved/Updated Successfully");
                        New();
                    }
                    else
                        clsMessages.setMessage("Records Save/Update failed"); 
                }
                else
                    clsMessages.setMessage("You don't have permission to Save/Update this record(s)");
            }
        }

        private bool ValidateSave()
        {
            bool status = true;

            foreach (var employee in ListEmployeeLeaveDetails)
            {
                if (employee.leave_detail_id == null || employee.leave_detail_id == Guid.Empty)
                {
                    clsMessages.setMessage("Please Select a Leave Detail Name");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }
                else if (employee.number_of_days == null || employee.number_of_days < 0)
                {
                    clsMessages.setMessage("Please Enter Number Of Days Used Up");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }

                else if (LeaveDetails.FirstOrDefault(c => c.leave_detail_id == employee.leave_detail_id).number_of_days < employee.number_of_days -(employee.trns_value == null ? 0 : employee.trns_value))
                {
                    clsMessages.setMessage("Invalid Number Of Used Up Days");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }

                else if (employee.remaining_days == null || employee.remaining_days < 0)
                {
                    clsMessages.setMessage("Please Enter Remaining Number Of Days");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }

                else if (employee.maximum_leaves != null && employee.maximum_leaves > (employee.number_of_days + employee.remaining_days))
                {
                    clsMessages.setMessage("Maximum leaves value cannont exceed Remaining value");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }

                else if (LeaveDetails.FirstOrDefault(c => c.leave_detail_id == employee.leave_detail_id).number_of_days < employee.remaining_days - (employee.trns_value == null ? 0 : employee.trns_value))
                {
                    clsMessages.setMessage("Invalid Remaining Number Of Days");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }

                else if (LeaveDetails.FirstOrDefault(c => c.leave_detail_id == employee.leave_detail_id).number_of_days != employee.number_of_days + employee.remaining_days - (employee.trns_value == null ? 0 : employee.trns_value))
                {
                    clsMessages.setMessage("Either 'Number Of Used Up Days' Or 'Remaining Number Of Days' is Invalid");
                    CurrentEmployeeLeaveDetailView = employee;
                    status = false;
                    break;
                }

                else if (ListEmployeeMaxLeaveDetails != null && ListEmployeeMaxLeaveDetails.Count > 0 && ListEmployeeMaxLeaveDetails.Count(c => c.emp_id == employee.emp_id && c.leave_detail_id == employee.leave_detail_id) > 0)
                {
                    decimal? DefinedMax = 0;

                    if (employee.maximum_leaves == null || employee.maximum_leaves == 0)
                    {
                        clsMessages.setMessage("invalid maximum number of leaves");
                        CurrentEmployeeLeaveDetailView = employee;
                        status = false;
                        break;
                    }

                    else
                        DefinedMax = ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == employee.emp_id && c.leave_detail_id == employee.leave_detail_id).Select(d => d.value * d.leave_type_qty).Sum();

                    if (employee.maximum_leaves < DefinedMax || employee.maximum_leaves > DefinedMax)
                    {
                        clsMessages.setMessage("Maximumm number of leaves and  its definitions does not match, please check the leave definition");
                        CurrentEmployeeLeaveDetailView = employee;
                        status = false;
                        break;
                    }
                }
            }

            return status;
        }

        #endregion

        #region SaveButton Class & Property

        bool saveCanExecute()
        {
            if (ListEmployeeLeaveDetails != null && ListEmployeeLeaveDetails.Count > 0)
                return true;
            else
                return false;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }
        #endregion

        #region Delete Method
        void Delete()
        {
            try
            {
                if (EmployeeLeaveDetailView.Count(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id) > 0)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (clsSecurity.GetDeletePermission(405))
                        {
                            if (this.serviceClient.DeleteEmployeeLeaveDetails(CurrentEmployeeLeaveDetailView))
                            {
                                EmployeeLeaveDetailView = null;
                                reafreshEmployeeLeaveDetailsViewList();
                                clsMessages.setMessage("Record Deleted");
                                New();

                            }
                            else
                            {
                                clsMessages.setMessage("Record Delete Failed");
                            } 
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Delete this record(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }
        #endregion

        #region Delete Button Class & Property
        bool deleteCanExecute()
        {
            if (CurrentEmployeeLeaveDetailView == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }
        #endregion

        #region Refresh Methods

        private void refreshLeaveTypes()
        {
            this.serviceClient.GetAllLeaveTypesCompleted += (s, e) =>

            {
                try
                {
                    LeaveTypes = e.Result;
                }
                catch (Exception)
                {

                }
            };
            this.serviceClient.GetAllLeaveTypesAsync();
        }

        private void refreshEmployeeMaxLeaveDetailsViewList()
        {
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeMaxLeaveDetails.Clear();
                    EmployeeMaxLeaveDetails = e.Result;
                    if (EmployeeMaxLeaveDetails != null)
                        ListEmployeeMaxLeaveDetails = EmployeeMaxLeaveDetails.ToList();
                    EmployeeMaxLeaveDetails = null;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeMaximumLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriods == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        private void reafreshEmployeeLeaveDetailsViewList()
        {
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodCompleted += (s, e) =>
                {
                    ListEmployeeLeaveDetails.Clear();

                    IsRefreshed = true;
                    this.EmployeeLeaveDetailView = e.Result;
                    if (EmployeeLeaveDetailView != null)
                        ListEmployeeLeaveDetails = EmployeeLeaveDetailView.ToList();
                };
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriods == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        private void reafreshLeaveDetails()
        {
            this.serviceClient.GetMasLeaveDetailsByPeriodCompleted += (s, e) =>
            {
                this.LeaveDetails = null;
                this.LeaveDetails = e.Result;
            };
            this.serviceClient.GetMasLeaveDetailsByPeriodAsync(CurrentLeavePeriods == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        private void refreshLeavePeriods()
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
            {
                this.LeavePeriods = e.Result;
            };
            this.serviceClient.GetLeavePeriodsAsync();
        }

        #endregion

        #region Search


        private void SearchLeaveDetails()
        {
            EmployeeLeaveDetailView = null;
            EmployeeLeaveDetailView = ListEmployeeLeaveDetails;

            try
            {
                if (SearchIndex == 0)
                    EmployeeLeaveDetailView = EmployeeLeaveDetailView.Where(c => c.EMPLOYEE_ID != null && c.EMPLOYEE_ID.Trim().ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 1)
                    EmployeeLeaveDetailView = EmployeeLeaveDetailView.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
                if (SearchIndex == 2)
                    EmployeeLeaveDetailView = EmployeeLeaveDetailView.Where(c => c.leave_detail_name != null && c.leave_detail_name.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.ToString());
            }
        }

        #endregion

        #region KeyBindings

        public ICommand CalculateDays
        {
            get { return new RelayCommand(RemainingDays, RemainingDaysCanexecute); }
        }

        private bool RemainingDaysCanexecute()
        {
            if (CurrentLeaveDetail != null && CurrentEmployeeLeaveDetailView != null)
                return true;
            else
                return false;
        }

        private void RemainingDays()
        {
            CurrentEmployeeLeaveDetailView.remaining_days = (CurrentLeaveDetail.number_of_days - CurrentEmployeeLeaveDetailView.number_of_days) + (CurrentEmployeeLeaveDetailView.trns_value == null ? 0 : CurrentEmployeeLeaveDetailView.trns_value);
        }

        public ICommand KeyReomveLeaveType
        {
            get { return new RelayCommand(ReomveLeaveType, ReomveLeaveTypeCE); }
        }

        private bool ReomveLeaveTypeCE()
        {
            if (CurrentEmployeeMaxLeaveDetail != null && ListEmployeeMaxLeaveDetails != null && ListEmployeeMaxLeaveDetails.Count > 0)
                return true;
            else
                return false;
        }

        private void ReomveLeaveType()
        {
            Employee_Maximum_Leave_Details_View temp = ListEmployeeMaxLeaveDetails.FirstOrDefault(c => c.emp_id == CurrentEmployeeMaxLeaveDetail.emp_id && c.leave_detail_id == CurrentEmployeeMaxLeaveDetail.leave_detail_id && c.leave_type_id == CurrentEmployeeMaxLeaveDetail.leave_type_id);

            if (temp != null)
            {
                ListEmployeeMaxLeaveDetails.Remove(temp);
                EmployeeMaxLeaveDetails = null;
                EmployeeMaxLeaveDetails = ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id);

                CurrentEmployeeMaxLeaveDetail = new Employee_Maximum_Leave_Details_View();
                CurrentEmployeeMaxLeaveDetail.leave_detail_id = CurrentEmployeeLeaveDetailView.leave_detail_id;
                CurrentEmployeeMaxLeaveDetail.emp_id = CurrentEmployeeLeaveDetailView.emp_id;

                if (EmployeeMaxLeaveDetails != null && EmployeeMaxLeaveDetails.Count() > 0)
                    LeaveTypeRemaining = (decimal)(CurrentEmployeeLeaveDetailView.maximum_leaves - EmployeeMaxLeaveDetails.Select(c => LeaveTypes.FirstOrDefault(d => c.leave_type_id == d.leave_type_id).value * c.leave_type_qty).Sum());
                else
                    LeaveTypeRemaining = 0;
            }
        }

        #endregion

        #region Define Max Leaves

        public ICommand BtnDefineMaxLeaves
        {
            get { return new RelayCommand(DefineMaxLeaves, DefineMaxLeavesCE); }
        }

        private bool DefineMaxLeavesCE()
        {
            if (CurrentEmployeeLeaveDetailView != null && CurrentEmployeeLeaveDetailView.leave_detail_id != null && CurrentEmployeeLeaveDetailView.remaining_days != null && CurrentEmployeeLeaveDetailView.maximum_leaves != null && CurrentEmployeeLeaveDetailView.maximum_leaves <= CurrentEmployeeLeaveDetailView.remaining_days)
                return true;
            else
                return false;
        }

        private void DefineMaxLeaves()
        {

            EmployeeMaxLeaveDetails = null;
            EmployeeMaxLeaveDetails = ListEmployeeMaxLeaveDetails.Where(c => c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id && c.emp_id == CurrentEmployeeLeaveDetailView.emp_id);

            CurrentEmployeeMaxLeaveDetail = null;
            CurrentEmployeeMaxLeaveDetail = new Employee_Maximum_Leave_Details_View();

            if (EmployeeMaxLeaveDetails != null && EmployeeMaxLeaveDetails.Count() > 0)
                LeaveTypeRemaining = (decimal)(CurrentEmployeeLeaveDetailView.maximum_leaves - EmployeeMaxLeaveDetails.Select(c => LeaveTypes.FirstOrDefault(d => c.leave_type_id == d.leave_type_id).value * c.leave_type_qty).Sum());
            else
                LeaveTypeRemaining = CurrentEmployeeLeaveDetailView.maximum_leaves.Value;

            window = new MaximumLeavesWindow(this);
            window.ShowDialog();
        }

        public ICommand BtnAddLeaveType
        {
            get { return new RelayCommand(AddLEaveType, AddLEaveTypeCE); }
        }

        private bool AddLEaveTypeCE()
        {
            if (CurrentLeaveType != null && CurrentEmployeeMaxLeaveDetail != null && CurrentEmployeeMaxLeaveDetail.leave_type_id != null && CurrentEmployeeMaxLeaveDetail.leave_type_qty > 0)
                return true;
            else
                return false;
        }

        private void AddLEaveType()
        {
            if (CurrentEmployeeLeaveDetailView.emp_id == Guid.Empty)
            {
                if (ListEmployeeMaxLeaveDetails.Count(c => c.leave_type_id == CurrentEmployeeMaxLeaveDetail.leave_type_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id && c.emp_id == Guid.Empty) == 0)
                {
                    if (ListEmployeeMaxLeaveDetails.Count(c => c.emp_id == Guid.Empty && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id) > 0)
                    {
                        if (ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == Guid.Empty && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id).Select(c => c.value).Sum() * CurrentEmployeeMaxLeaveDetail.leave_type_qty <= CurrentEmployeeLeaveDetailView.maximum_leaves)
                        {
                            Employee_Maximum_Leave_Details_View Temp = new Employee_Maximum_Leave_Details_View();
                            Temp.leave_type_id = CurrentEmployeeMaxLeaveDetail.leave_type_id;
                            Temp.value = CurrentLeaveType.value;
                            Temp.leave_type_qty = CurrentEmployeeMaxLeaveDetail.leave_type_qty;
                            Temp.emp_id = Guid.Empty;
                            Temp.leave_detail_id = CurrentEmployeeLeaveDetailView.leave_detail_id;
                            Temp.name = CurrentLeaveType.name;

                            ListEmployeeMaxLeaveDetails.Add(Temp);

                            EmployeeMaxLeaveDetails = null;
                            EmployeeMaxLeaveDetails = ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id);
                        }
                        else
                            clsMessages.setMessage("Sum of the leave types should not exceed thye maximum value");
                    }

                    else
                    {
                        if ((CurrentEmployeeMaxLeaveDetail.leave_type_qty * CurrentLeaveType.value) <= CurrentEmployeeLeaveDetailView.maximum_leaves)
                        {
                            Employee_Maximum_Leave_Details_View Temp = new Employee_Maximum_Leave_Details_View();
                            Temp.leave_type_id = CurrentEmployeeMaxLeaveDetail.leave_type_id;
                            Temp.value = CurrentLeaveType.value;
                            Temp.leave_type_qty = CurrentEmployeeMaxLeaveDetail.leave_type_qty;
                            Temp.emp_id = Guid.Empty;
                            Temp.leave_detail_id = CurrentEmployeeLeaveDetailView.leave_detail_id;
                            Temp.name = CurrentLeaveType.name;

                            ListEmployeeMaxLeaveDetails.Add(Temp);

                            EmployeeMaxLeaveDetails = null;
                            EmployeeMaxLeaveDetails = ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id);
                        }
                        else
                            clsMessages.setMessage("Sum of the leave types should not exceed thye maximum value");
                    }
                }

            }
            else
            {
                if (ListEmployeeMaxLeaveDetails.Count(c => c.leave_type_id == CurrentEmployeeMaxLeaveDetail.leave_type_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id && c.emp_id == CurrentEmployeeLeaveDetailView.emp_id) == 0)
                {
                    if (ListEmployeeMaxLeaveDetails.Count(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id) > 0)
                    {
                        if (ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id).Select(c => c.value).Sum() * CurrentEmployeeMaxLeaveDetail.leave_type_qty <= CurrentEmployeeLeaveDetailView.maximum_leaves)
                        {
                            Employee_Maximum_Leave_Details_View Temp = new Employee_Maximum_Leave_Details_View();
                            Temp.leave_type_id = CurrentEmployeeMaxLeaveDetail.leave_type_id;
                            Temp.value = CurrentLeaveType.value;
                            Temp.leave_type_qty = CurrentEmployeeMaxLeaveDetail.leave_type_qty;
                            Temp.emp_id = CurrentEmployeeLeaveDetailView.emp_id;
                            Temp.leave_detail_id = CurrentEmployeeLeaveDetailView.leave_detail_id;
                            Temp.name = CurrentLeaveType.name;

                            ListEmployeeMaxLeaveDetails.Add(Temp);

                            EmployeeMaxLeaveDetails = null;
                            EmployeeMaxLeaveDetails = ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id);
                        }

                        else
                            clsMessages.setMessage("Sum of the leave types should not exceed thye maximum value");
                    }

                    else
                    {
                        if ((CurrentEmployeeMaxLeaveDetail.leave_type_qty * CurrentLeaveType.value) <= CurrentEmployeeLeaveDetailView.maximum_leaves)
                        {
                            Employee_Maximum_Leave_Details_View Temp = new Employee_Maximum_Leave_Details_View();
                            Temp.leave_type_id = CurrentEmployeeMaxLeaveDetail.leave_type_id;
                            Temp.value = CurrentLeaveType.value;
                            Temp.leave_type_qty = CurrentEmployeeMaxLeaveDetail.leave_type_qty;
                            Temp.emp_id = CurrentEmployeeLeaveDetailView.emp_id;
                            Temp.leave_detail_id = CurrentEmployeeLeaveDetailView.leave_detail_id;
                            Temp.name = CurrentLeaveType.name;

                            ListEmployeeMaxLeaveDetails.Add(Temp);

                            EmployeeMaxLeaveDetails = null;
                            EmployeeMaxLeaveDetails = ListEmployeeMaxLeaveDetails.Where(c => c.emp_id == CurrentEmployeeLeaveDetailView.emp_id && c.leave_detail_id == CurrentEmployeeLeaveDetailView.leave_detail_id);
                        }
                        else
                            clsMessages.setMessage("Sum of the leave types should not exceed thye maximum value");
                    }
                }
            }

            if (EmployeeMaxLeaveDetails != null && EmployeeMaxLeaveDetails.Count() > 0)
                LeaveTypeRemaining = (decimal)(CurrentEmployeeLeaveDetailView.maximum_leaves - EmployeeMaxLeaveDetails.Select(c => LeaveTypes.FirstOrDefault(d => c.leave_type_id == d.leave_type_id).value * c.leave_type_qty).Sum());

            CurrentEmployeeMaxLeaveDetail = null;
            CurrentEmployeeMaxLeaveDetail = new Employee_Maximum_Leave_Details_View();

        }

        public ICommand BtnExitLeaveTypes
        {
            get { return new RelayCommand(ExitLeaveTypes); }
        }

        private void ExitLeaveTypes()
        {
            if (EmployeeMaxLeaveDetails == null || EmployeeMaxLeaveDetails.Count() == 0)
            {
                window.Close();
            }

            else
            {
                if (LeaveTypeRemaining != 0)
                    clsMessages.setMessage("Please define the maximum leaves distribution, until the 'Remaining' value is zero");
                else
                    window.Close();
            }
        }

        #endregion


        #endregion
    }
}
