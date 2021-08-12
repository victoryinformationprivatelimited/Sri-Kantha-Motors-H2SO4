using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ERP.Performance
{
    class TaskApprovalsViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<dtl_EvaluationRates> ListEvaluatioRates;

        #endregion

        #region Constructor

        public TaskApprovalsViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListEvaluatioRates = new List<dtl_EvaluationRates>();
            New();
            SearchIndex = 0;
        }

        #endregion

        #region Properties

        private IEnumerable<dtl_EvaluationRates> _EvaluationRates;
        public IEnumerable<dtl_EvaluationRates> EvaluationRates
        {
            get { return _EvaluationRates; }
            set { _EvaluationRates = value; OnPropertyChanged("EvaluationRates"); }
        }
        

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText");}
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private string  _PendingTabHeader;
        public string  PendingTabHeader
        {
            get { return _PendingTabHeader; }
            set { _PendingTabHeader = value; OnPropertyChanged("PendingTabHeader"); }
        }

        private string _RejectTabHeader;
        public string RejectTabHeader
        {
            get { return _RejectTabHeader; }
            set { _RejectTabHeader = value; OnPropertyChanged("RejectTabHeader"); }
        }

        private string _ExpiredTabHeader;
        public string ExpiredTabHeader
        {
            get { return _ExpiredTabHeader; }
            set { _ExpiredTabHeader = value; OnPropertyChanged("ExpiredTabHeader"); }
        }        

        private IEnumerable<EmployeeSubTasksDetailForUserView> _PendingRejectTasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> PendingRejectTasks
        {
            get { return _PendingRejectTasks; }
            set { _PendingRejectTasks = value; OnPropertyChanged("PendingRejectTasks"); }
        }   

        private IEnumerable<EmployeeSubTasksDetailForUserView> _PendingApprovalTasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> PendingApprovalTasks
        {
            get { return _PendingApprovalTasks; }
            set { _PendingApprovalTasks = value; OnPropertyChanged("PendingApprovalTasks"); }
        }

        private IEnumerable<EmployeeSubTasksDetailForUserView> _RejectedTasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> RejectedTasks
        {
            get { return _RejectedTasks; }
            set { _RejectedTasks = value; OnPropertyChanged("RejectedTasks"); }
        }

        private IEnumerable<EmployeeSubTasksDetailForUserView> _ExpiredTasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> ExpiredTasks
        {
            get { return _ExpiredTasks; }
            set { _ExpiredTasks = value; OnPropertyChanged("ExpiredTasks"); }
        }

        private EmployeeSubTasksDetailForUserView _CurrentExpiredTask;
        public EmployeeSubTasksDetailForUserView CurrentExpiredTask
        {
            get { return _CurrentExpiredTask; }
            set { _CurrentExpiredTask = value; OnPropertyChanged("CurrentExpiredTask"); if (CurrentExpiredTask != null) { if (TaskProgress != null && TaskProgress.Count() > 0) filterTasksProgressExpired(); }; }
        }
        
        private EmployeeSubTasksDetailForUserView _CurrentPendingApprovalTasks;
        public EmployeeSubTasksDetailForUserView CurrentPendingApprovalTasks
        {
            get { return _CurrentPendingApprovalTasks; }
            set { _CurrentPendingApprovalTasks = value; OnPropertyChanged("CurrentPendingApprovalTasks"); if (CurrentPendingApprovalTasks != null) { if (TaskProgress != null && TaskProgress.Count() > 0) filterTaskProgressPending(); if (ListEvaluatioRates != null && ListEvaluatioRates.Count > 0) FilterEvaluationRates(); }; }
        }

        private EmployeeSubTasksDetailForUserView _CurrentRejectedTasks;
        public EmployeeSubTasksDetailForUserView CurrentRejectedTasks
        {
            get { return _CurrentRejectedTasks; }
            set { _CurrentRejectedTasks = value; OnPropertyChanged("CurrentRejectedTasks"); if (CurrentRejectedTasks != null) { if (TaskProgress != null && TaskProgress.Count() > 0) filterTaskProgressRejected();};}
        }

        private IEnumerable<EmployeeSubtasksProgressView> _TaskProgress;
        public IEnumerable<EmployeeSubtasksProgressView> TaskProgress
        {
            get { return _TaskProgress; }
            set { _TaskProgress = value; OnPropertyChanged("TaskProgress"); }
        }

        private IEnumerable<EmployeeSubtasksProgressView> _CurrentTaskProgressPending;
        public IEnumerable<EmployeeSubtasksProgressView> CurrentTaskProgressPending
        {
            get { return _CurrentTaskProgressPending; }
            set { _CurrentTaskProgressPending = value; OnPropertyChanged("CurrentTaskProgressPending"); }
        }

        private IEnumerable<EmployeeSubtasksProgressView> _CurrentTaskProgressRejected;
        public IEnumerable<EmployeeSubtasksProgressView> CurrentTaskProgressRejected
        {
            get { return _CurrentTaskProgressRejected; }
            set { _CurrentTaskProgressRejected = value; OnPropertyChanged("CurrentTaskProgressRejected"); }
        }

        private IEnumerable<EmployeeSubtasksProgressView> _CurrentTaskProgressExpired;
        public IEnumerable<EmployeeSubtasksProgressView> CurrentTaskProgressExpired
        {
            get { return _CurrentTaskProgressExpired; }
            set { _CurrentTaskProgressExpired = value; OnPropertyChanged("CurrentTaskProgressExpired"); }
        }
       
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; OnPropertyChanged("Remark"); }
        }

        private string _RejectRemark;
        public string RejectRemark
        {
            get { return _RejectRemark; }
            set { _RejectRemark = value; OnPropertyChanged("RejectRemark"); }
        }

        private string  _ExpiredRemark;
        public string  ExpiredRemark
        {
            get { return _ExpiredRemark; }
            set { _ExpiredRemark = value; OnPropertyChanged("ExpiredRemark"); }
        }
        
        
        
        #endregion

        #region RefreshMethods

        private void RefreshEmployeeSubtasks()
        {
            serviceClient.GetEmployeeSubtaskDetailsForUserCompleted += (s, e) =>
            {
                try
                {
                    PendingRejectTasks = e.Result;
                    PendingApprovalTasks = null;
                    RejectedTasks = null;
                    ExpiredTasks = null;

                    if(PendingRejectTasks != null && PendingRejectTasks.Count()>0)
                    {
                        PendingApprovalTasks = PendingRejectTasks.Where(c => c.is_pending == true);
                        RejectedTasks = PendingRejectTasks.Where(c => c.is_rejected == true);
                        ExpiredTasks = PendingRejectTasks.Where(c => c.is_expired == true);
                    }

                    if (PendingApprovalTasks != null && PendingApprovalTasks.Count() > 0) 
                    {
                        PendingTabHeader = "Tasks Pending For Approval (" + PendingApprovalTasks.Count().ToString() + ")";
                    }

                    else if(PendingApprovalTasks == null || PendingApprovalTasks.Count() == 0)
                        PendingTabHeader = "Tasks Pending For Approval (0)";

                    if (RejectedTasks != null && RejectedTasks.Count() > 0) 
                    {
                        RejectTabHeader = "Rejected Tasks (" + RejectedTasks.Count().ToString() + ")";
                    }

                    else if(RejectedTasks == null || RejectedTasks.Count() == 0)
                        RejectTabHeader = "Rejected Tasks (0)";

                    if (ExpiredTasks != null && ExpiredTasks.Count() > 0)
                    {
                        ExpiredTabHeader = "Expired Tasks (" + ExpiredTasks.Count().ToString() + ")";
                    }

                    else if (ExpiredTasks == null || ExpiredTasks.Count() == 0)
                        ExpiredTabHeader = "Expired Tasks (0)";

                    if ((PendingApprovalTasks != null && PendingApprovalTasks.Count() > 0) || (RejectedTasks != null && RejectedTasks.Count() > 0) || (ExpiredTasks != null && ExpiredTasks.Count()>0))
                        ResfreshTaskProgress();

                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEmployeeSubtasks()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeSubtaskDetailsForUserAsync(clsSecurity.loggedUser.user_id,false);
        }

        private void ResfreshTaskProgress()
        {
            serviceClient.GetTaskProgressByPendingSubtasksCompleted += (s, e) =>
            {
                try
                {
                    TaskProgress = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("ResfreshTaskProgress()\n\n" + ex.Message);
                }
            };
            serviceClient.GetTaskProgressByPendingSubtasksAsync(PendingRejectTasks.ToArray());
        }

        private void RefreshEvaluationRates() 
        {
            serviceClient.GetEvaluationRatesCompleted += (s, e) => 
            {
                try
                {
                    EvaluationRates = e.Result;
                    if (EvaluationRates != null && EvaluationRates.Count() > 0) 
                    {
                        ListEvaluatioRates = EvaluationRates.ToList();
                        EvaluationRates = null;
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEvaluationRates()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEvaluationRatesAsync();
        }

        #endregion

        #region Commands & Methods

        private void New()
        {
            PendingTabHeader = "Tasks Pending For Approval";
            RejectTabHeader = "Rejected Tasks";
            ExpiredTabHeader = "Expired Tasks";
            ListEvaluatioRates.Clear();
            RefreshEmployeeSubtasks();
            RefreshEvaluationRates();
        }
        
        private void SearchTaskCategory()
        {
            try
            {

            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }

        }

        private void filterTaskProgressPending()
        {
            CurrentTaskProgressPending = null;
            CurrentTaskProgressPending = TaskProgress.Where(c => c.SubTask_ID == CurrentPendingApprovalTasks.SubTask_ID && c.Employee_ID == CurrentPendingApprovalTasks.Employee_ID);
        }

        private void filterTaskProgressRejected()
        {
            CurrentTaskProgressRejected = null;
            CurrentTaskProgressRejected = TaskProgress.Where(c => c.SubTask_ID == CurrentRejectedTasks.SubTask_ID && c.Employee_ID == CurrentRejectedTasks.Employee_ID);
        }

        private void filterTasksProgressExpired()
        {
            CurrentTaskProgressExpired = null;
            CurrentTaskProgressExpired = TaskProgress.Where(c => c.SubTask_ID == CurrentExpiredTask.SubTask_ID && c.Employee_ID == CurrentExpiredTask.Employee_ID);
        }


        private void FilterEvaluationRates()
        {
            EvaluationRates = null;
            EvaluationRates = ListEvaluatioRates.Where(c => c.evaluation_rate_group_id == CurrentPendingApprovalTasks.evaluation_rate_group_id);
        }
        
        public ICommand Approvebtn 
        {
            get { return new RelayCommand(Approve, ApproveRejectCanExecute); }
        }

        public ICommand Rejectbtn 
        {
            get { return new RelayCommand(Reject, ApproveRejectCanExecute); }
        }

        private bool ApproveRejectCanExecute()
        {
            if (CurrentPendingApprovalTasks != null && Remark != null && Remark != string.Empty)
                return true;
            else
                return false;
        }

        public ICommand Resetbtn 
        {
            get { return new RelayCommand(Reset, ResetRemoveCanExecute); }
        }

        public ICommand Removebtn 
        {
            get { return new RelayCommand(Remove, ResetRemoveCanExecute); }
        }

        private bool ResetRemoveCanExecute()
        {
            if (CurrentRejectedTasks != null && RejectRemark != null && RejectRemark != string.Empty)
                return true;
            else
                return false;
        }

        public ICommand ResetExpiredbtn 
        {
            get { return new RelayCommand(ResetExpired, ResetExpiredCanexecute); }
        }

        public ICommand RemoveExpiredbtn
        {
            get { return new RelayCommand(RemoveExpired, ResetExpiredCanexecute); }
        }

        private void RemoveExpired()
        {
            if (!serviceClient.CheckUnderlyingTasks(CurrentExpiredTask))
            {
                CurrentExpiredTask.save_datetime = DateTime.Now;
                CurrentExpiredTask.save_user_id = clsSecurity.loggedUser.user_id;

                if (ExpiredRemark == null || ExpiredRemark == string.Empty)
                    ExpiredRemark = "Supervisor removed this task from the employee";

                clsMessages.setMessage("You're going to Remove this task from the Empoyee, do you want to proceed?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (serviceClient.RemoveResetTasks(CurrentExpiredTask, false, "Supervisor => " + ExpiredRemark))
                    {
                        New();
                        clsMessages.setMessage("Task Removal was successfull");
                    }
                    else
                        clsMessages.setMessage("Task Removal failed");
                }
            }
            else
                clsMessages.setMessage("This task has an underlying task that needs to be completed for this approval");
        }

        private bool ResetExpiredCanexecute()
        {
            if (CurrentExpiredTask != null)
                return true;
            else
                return false;
        }

        private void ResetExpired()
        {
            DateTime CurrentSubtaskEndDatetime = CurrentExpiredTask.SubTask_EndDate.Value.Date.AddTicks(CurrentExpiredTask.SubTask_EndTime.Value.Ticks);

            if (CurrentSubtaskEndDatetime < DateTime.Now)
                clsMessages.setMessage("Please go to Tasks/Subtasks window and extend dates and try again");
            else 
            {
                CurrentExpiredTask.save_datetime = DateTime.Now;
                CurrentExpiredTask.save_user_id = clsSecurity.loggedUser.user_id;

                if (ExpiredRemark == null || ExpiredRemark == string.Empty)
                    ExpiredRemark = "Supervisor extended time for this task";

                clsMessages.setMessage("You're going to reset this task to the Empoyee, do you want to proceed?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    serviceClient.ResetExpiredTasks(CurrentExpiredTask, "Supervisor => " + ExpiredRemark);
                    New();
                }
            }
        }

        private void Remove()
        {
            if (!serviceClient.CheckUnderlyingTasks(CurrentRejectedTasks))
            {
                CurrentRejectedTasks.save_datetime = DateTime.Now;
                CurrentRejectedTasks.save_user_id = clsSecurity.loggedUser.user_id;

                clsMessages.setMessage("You're going to Remove this task from the Empoyee, do you want to proceed?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (serviceClient.RemoveResetTasks(CurrentRejectedTasks, false, "Supervisor => " + RejectRemark))
                    {
                        New();
                        clsMessages.setMessage("Task Removal was successfull");
                    }
                    else
                        clsMessages.setMessage("Task Removal was failed");
                }
            }
            else
                clsMessages.setMessage("This task has an underlying task that needs to be completed for this approval");
        }

        private void Reset()
        {
            CurrentRejectedTasks.save_datetime = DateTime.Now;
            CurrentRejectedTasks.save_user_id = clsSecurity.loggedUser.user_id;

            clsMessages.setMessage("You're going Reset this task to the Empoyee, do you want to proceed?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                if (serviceClient.RemoveResetTasks(CurrentRejectedTasks, true, "Supervisor => " + RejectRemark))
                {
                    New();
                    clsMessages.setMessage("Task Reset was successful");
                }
                else
                    clsMessages.setMessage("Task Reset was failed");
            }
        }

        private void Approve()
        {
            if (!serviceClient.CheckUnderlyingFinishedTasks(CurrentPendingApprovalTasks))
            {
                CurrentPendingApprovalTasks.save_datetime = DateTime.Now;
                CurrentPendingApprovalTasks.save_user_id = clsSecurity.loggedUser.user_id;

                if (CurrentPendingApprovalTasks.evaluation_rate_id != 0)
                {
                    clsMessages.setMessage("You're going to save this task as finalized, do you want to proceed?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (serviceClient.ApproveRejectTasks(CurrentPendingApprovalTasks, true, "Supervisor => " + Remark))
                        {
                            New();
                            clsMessages.setMessage("Task finalized successfully");
                        }
                        else
                            clsMessages.setMessage("Task finalization failed");
                    } 
                }
                else
                    clsMessages.setMessage("Please Rate this task and proceed");
            }
            else
                clsMessages.setMessage("This task has an underlying task that needs to be completed for this approval");

        }

        private void Reject()
        {
            CurrentPendingApprovalTasks.save_datetime = DateTime.Now;
            CurrentPendingApprovalTasks.save_user_id = clsSecurity.loggedUser.user_id;

            if (RejectRemark == null || RejectRemark == string.Empty)
                RejectRemark = "Supervisor rejected your request to finalize this task";

            clsMessages.setMessage("You're going to reject this task and send back to the Employee, do you want to proceed?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                if (serviceClient.ApproveRejectTasks(CurrentPendingApprovalTasks, false, "Supervisor => " + Remark))
                {
                    New();
                    clsMessages.setMessage("Task rejected successfully");
                }
                else
                    clsMessages.setMessage("Task rejection failed");
            }
        }

        #endregion
    }
}
