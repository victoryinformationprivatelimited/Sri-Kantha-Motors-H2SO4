using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Leave;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Master_Details
{
    public class EmployeeAutoLeaveViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Global Variables
        Guid EmployeeID;
        #endregion

        #region Fields
        List<Employee_Leave_Detail_View> ListEmployeeLeaveDetails;
        List<Employee_Maximum_Leave_Details_View> ListEmployeeMaxLeaveDetails;

        #endregion

        public EmployeeAutoLeaveViewModel(Guid EmployeeID)
        {
            ListEmployeeLeaveDetails = new List<Employee_Leave_Detail_View>();
            this.EmployeeID = EmployeeID;
            refreshLeavePeriods();
        }

        #region Properties

        private bool isProbation;

        public bool IsProbation
        {
            get { return isProbation; }
            set
            {
                isProbation = value;
                this.OnPropertyChanged("IsProbation");
            }
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
                    //ListEmployeeLeaveDetails = new List<Employee_Leave_Detail_View>();
                    // ListEmployeeMaxLeaveDetails.Clear();
                    LeaveDetails = null;
                    CurrentEmployeeLeaveDetailView = null;
                    CurrentEmployeeLeaveDetailView = new Employee_Leave_Detail_View();
                }
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
            }
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

                /* if (CurrentEmployeeLeaveDetailView.is_probation != null)
                     this.IsProbation = (bool)CurrentEmployeeLeaveDetailView.is_probation;                
                 else
                     this.IsProbation = false;*/
            }
        }

        #endregion

        #region Refresh Methods

        private void refreshLeavePeriods()
        {
            this.serviceClient.GetLeavePeriodsCompleted += (s, e) =>
            {
                this.LeavePeriods = e.Result;
            };
            this.serviceClient.GetLeavePeriodsAsync();
        }

        private void reafreshLeaveDetails()
        {
            this.serviceClient.GetMasLeaveDetailsByPeriodCompleted += (s, e) =>
            {
                this.LeaveDetails = null;
                this.LeaveDetails = e.Result.Where(c => c.is_automate == true);
            };
            this.serviceClient.GetMasLeaveDetailsByPeriodAsync(CurrentLeavePeriods == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        /*private void reafreshEmployeeLeaveDetailsViewList()
        {
            this.serviceClient.GetEmployeeAutoLeaveDetailsViewCompleted += (s, e) =>
            {
                ListEmployeeLeaveDetails.Clear();

                this.EmployeeLeaveDetailView = e.Result.Where(c => c.is_detail_automate == true);
                if (EmployeeLeaveDetailView != null)
                    ListEmployeeLeaveDetails = EmployeeLeaveDetailView.ToList();
            };
            this.serviceClient.GetEmployeeAutoLeaveDetailsViewAsync(CurrentLeavePeriods == null ? Guid.Empty : CurrentLeavePeriods.period_id, EmployeeID);
        }*/

        private void reafreshEmployeeLeaveDetailsViewList()
        {
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodCompleted += (s, e) =>
            {
                ListEmployeeLeaveDetails.Clear();
                this.EmployeeLeaveDetailView = e.Result.Where(c => c.is_detail_automate == true && c.emp_id == EmployeeID);
                if (EmployeeLeaveDetailView != null)
                {
                    ListEmployeeLeaveDetails = EmployeeLeaveDetailView.ToList();
                    /*if(ListEmployeeLeaveDetails != null)
                    this.IsProbation = (bool)ListEmployeeLeaveDetails.FirstOrDefault().is_probation;*/
                }
            };
            this.serviceClient.GetEmployeeLeaveDetailsViewListByPeriodAsync(CurrentLeavePeriods == null ? Guid.Empty : CurrentLeavePeriods.period_id);
        }

        #endregion

        #region Commands

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


        bool saveCanExecute()
        {
            if (CurrentEmployeeLeaveDetailView != null || CurrentLeaveDetail != null)
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


        void New()
        {
            try
            {
                CurrentEmployeeLeaveDetailView = null;
                CurrentEmployeeLeaveDetailView = new Employee_Leave_Detail_View();
                CurrentLeaveDetail = null;
                CurrentEmployeeLeaveDetailView.leave_detail_id = Guid.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void GetLeaveData()
        {
            reafreshLeaveDetails();
            reafreshEmployeeLeaveDetailsViewList();
        }

        void Save()
        {
            if (clsSecurity.GetSavePermission(405))
            {
                List<dtl_EmployeeLeave> SaveList = new List<dtl_EmployeeLeave>();

                if (EmployeeID != null)
                {

                    dtl_EmployeeLeave SaveObj = new dtl_EmployeeLeave();
                    SaveObj.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                    SaveObj.emp_id = EmployeeID;
                    SaveObj.number_of_days = CurrentLeaveDetail.number_of_days;
                    SaveObj.remaining_days = 0;
                    SaveObj.maximum_leaves = 0;
                    SaveObj.is_special = true;
                    SaveObj.is_automate = CurrentLeaveDetail.is_automate;
                    SaveObj.is_probation = true;

                    // m 2020-08-07 only save

                    //if (ListEmployeeLeaveDetails != null && ListEmployeeLeaveDetails.Count() > 0)
                    //{
                    //    SaveObj.leave_detail_id = ListEmployeeLeaveDetails[0].leave_detail_id;
                    //    SaveObj.emp_id = EmployeeID;
                    //    SaveObj.number_of_days = ListEmployeeLeaveDetails[0].number_of_days;
                    //    SaveObj.remaining_days = ListEmployeeLeaveDetails[0].remaining_days;
                    //    SaveObj.maximum_leaves = ListEmployeeLeaveDetails[0].maximum_leaves;
                    //    SaveObj.is_special = ListEmployeeLeaveDetails[0].is_special;
                    //    SaveObj.is_automate = true;//ListEmployeeLeaveDetails[0].is_automate;                            
                    //                               // bool var2 = (bool)CurrentEmployeeLeaveDetailView.is_probation;
                    //    SaveObj.is_probation = ListEmployeeLeaveDetails[0].is_probation;
                    //}
                    //else
                    //{
                    //    SaveObj.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                    //    SaveObj.emp_id = EmployeeID;
                    //    SaveObj.number_of_days = 0;
                    //    SaveObj.remaining_days = 0;
                    //    SaveObj.maximum_leaves = 0;
                    //    SaveObj.is_special = true;
                    //    SaveObj.is_automate = true;
                    //    SaveObj.is_probation = CurrentEmployeeLeaveDetailView.is_probation;
                    //}

                    if (this.serviceClient.SaveEmployeeMasterLeaveDetails(SaveObj))
                    {
                        EmployeeLeaveDetailView = null;
                        reafreshEmployeeLeaveDetailsViewList();
                        clsMessages.setMessage("Record Saved/Updated Successfully");
                    }
                    else
                    {
                        clsMessages.setMessage("Record Save/Update Failed");
                    }

                }
                else
                    clsMessages.setMessage("Please Select Employee..");

            }
            else
                clsMessages.setMessage("You don't have permission to Save/Update this record(s)");
        }

        void set()
        {
            if (CurrentEmployeeLeaveDetailView == null)
            {
                CurrentEmployeeLeaveDetailView.leave_detail_id = CurrentLeaveDetail.leave_detail_id;
                CurrentEmployeeLeaveDetailView.number_of_days = 0;
                CurrentEmployeeLeaveDetailView.remaining_days = 0;
                CurrentEmployeeLeaveDetailView.maximum_leaves = 0;
                CurrentEmployeeLeaveDetailView.is_special = true;
                CurrentEmployeeLeaveDetailView.is_automate = true;
                CurrentEmployeeLeaveDetailView.is_probation = true;
            }
        }

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


    }
}
