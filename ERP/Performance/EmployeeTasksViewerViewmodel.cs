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
using System.Windows.Media.Effects;
using System.Windows.Threading;
namespace ERP.Performance
{
    public class EmployeeTasksViewerViewmodel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<EmployeeSubTasksDetailForUserView> ListEmployeeSubtasks;
        List<EmployeeSubTasksDetailForUserView> ListAcceptedSubtasks;
        List<EmployeeSubTasksDetailForUserView> ExipredTasks;
        ActiveTasksWindow ActiveTasksW;
        DispatcherTimer timerTask = new DispatcherTimer();
        DispatcherTimer MainTimer = new DispatcherTimer();

        #endregion

        #region Constructor

        public EmployeeTasksViewerViewmodel()
        {
            serviceClient = new ERPServiceClient();
            ExipredTasks = new List<EmployeeSubTasksDetailForUserView>();
            SearchIndex = 0;
            timerTask.Interval = TimeSpan.FromSeconds(1);
            timerTask.Tick += timer_Tick;
            MainTimer.Interval = TimeSpan.FromSeconds(5);
            MainTimer.Tick += MainTimer_Tick;
            MainTimer.Start();
            New();

        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (AllSubtasks != null && AllSubtasks.Count() > 0)
            {
                ExipredTasks.Clear();

                foreach (var item in AllSubtasks)
                {
                    if (item.is_expired != true)
                    {
                        DateTime CurrentSubtaskEndDatetime = item.SubTask_EndDate.Value.Date.AddTicks(item.SubTask_EndTime.Value.Ticks);

                        if (CurrentSubtaskEndDatetime < DateTime.Now)
                        {
                            item.save_datetime = DateTime.Now;
                            item.save_user_id = clsSecurity.loggedUser.user_id;
                            ExipredTasks.Add(item);
                        }
                    }
                }

                if (ExipredTasks != null && ExipredTasks.Count > 0)
                {
                    if (serviceClient.ExpireTasks(ExipredTasks.ToArray()))
                        New();
                }
            }
        }
        #endregion

        #region Properties

        private string _AcceptRejectRemark;
        public string AcceptRejectRemark
        {
            get { return _AcceptRejectRemark; }
            set { _AcceptRejectRemark = value; OnPropertyChanged("AcceptRejectRemark"); }
        }

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private string _AssignedTasks;
        public string AssignedTasks
        {
            get { return _AssignedTasks; }
            set { _AssignedTasks = value; OnPropertyChanged("AssignedTasks"); }
        }

        private EmployeeTasksViewerWindow _MainWindow;
        public EmployeeTasksViewerWindow MainWindow
        {
            get { return _MainWindow; }
            set { _MainWindow = value; OnPropertyChanged("MainWindow"); }
        }


        private IEnumerable<EmployeeTasksView> _Tasks;
        public IEnumerable<EmployeeTasksView> Tasks
        {
            get { return _Tasks; }
            set { _Tasks = value; OnPropertyChanged("Tasks"); }
        }

        private EmployeeTasksView _CurrentTask;
        public EmployeeTasksView CurrentTask
        {
            get { return _CurrentTask; }
            set { _CurrentTask = value; OnPropertyChanged("CurrentTask"); if (CurrentTask != null) { EmployeeSubTasks = null; EmployeeSubTasks = ListEmployeeSubtasks.Where(c => c.Task_ID == CurrentTask.Task_ID); } }
        }

        private IEnumerable<EmployeeSubTasksDetailForUserView> _EmployeeSubTasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> EmployeeSubTasks
        {
            get { return _EmployeeSubTasks; }
            set { _EmployeeSubTasks = value; OnPropertyChanged("EmployeeSubTasks"); }
        }

        private IEnumerable<EmployeeSubTasksDetailForUserView> _AcceptedSubtasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> AcceptedSubtasks
        {
            get { return _AcceptedSubtasks; }
            set { _AcceptedSubtasks = value; OnPropertyChanged("AcceptedSubtasks"); }
        }

        private EmployeeSubTasksDetailForUserView _CurrentAcceptedSubtask;
        public EmployeeSubTasksDetailForUserView CurrentAcceptedSubtask
        {
            get { return _CurrentAcceptedSubtask; }
            set { _CurrentAcceptedSubtask = value; OnPropertyChanged("CurrentAcceptedSubtask"); if (CurrentAcceptedSubtask != null) { setTime(); if (TaskProgress != null && TaskProgress.Count() > 0) { filterTaskProgress(); } } }
        }

        private IEnumerable<EmployeeSubtasksProgressView> _TaskProgress;
        public IEnumerable<EmployeeSubtasksProgressView> TaskProgress
        {
            get { return _TaskProgress; }
            set { _TaskProgress = value; OnPropertyChanged("TaskProgress"); }
        }

        private IEnumerable<EmployeeSubtasksProgressView> _CurrentTaskProgress;
        public IEnumerable<EmployeeSubtasksProgressView> CurrentTaskProgress
        {
            get { return _CurrentTaskProgress; }
            set { _CurrentTaskProgress = value; OnPropertyChanged("CurrentTaskProgress"); }
        }

        private IEnumerable<EmployeeSubTasksDetailForUserView> _AllSubtasks;
        public IEnumerable<EmployeeSubTasksDetailForUserView> AllSubtasks
        {
            get { return _AllSubtasks; }
            set { _AllSubtasks = value; OnPropertyChanged("AllSubtasks"); }
        }



        private IList _SelectedActiveTasks = new ArrayList();
        public IList SelectedActiveTasks
        {
            get { return _SelectedActiveTasks; }
            set { _SelectedActiveTasks = value; OnPropertyChanged("SelectedActiveTasks"); }
        }

        private string _TimeRemaining;

        public string TimeRemaining
        {
            get { return _TimeRemaining; }
            set { _TimeRemaining = value; OnPropertyChanged("TimeRemaining"); }
        }

        private DateTime _TimerDate;
        public DateTime TimerDate
        {
            get { return _TimerDate; }
            set { _TimerDate = value; OnPropertyChanged("TimerDate"); }
        }

        private string _TasksRemarks;
        public string TasksRemarks
        {
            get { return _TasksRemarks; }
            set { _TasksRemarks = value; OnPropertyChanged("TasksRemarks"); }
        }

        #endregion

        #region RefreshMethods

        private void RefreshEmployeeSubtasks()
        {
            serviceClient.GetEmployeeSubtaskDetailsForUserCompleted += (s, e) =>
            {
                try
                {
                    AllSubtasks = e.Result;

                    if (AllSubtasks != null && AllSubtasks.Count() > 0)
                    {
                        ListEmployeeSubtasks.Clear();
                        ListEmployeeSubtasks = AllSubtasks.Where(c => c.is_accepted == false && c.is_rejected == false).ToList();
                        AcceptedSubtasks = AllSubtasks.Where(c => c.is_accepted == true || (c.is_accepted == true && c.is_expired == true)).ToList();
                        ListAcceptedSubtasks = AcceptedSubtasks.ToList();

                        if (ListEmployeeSubtasks != null && ListEmployeeSubtasks.Count > 0)
                            AssignedTasks = "You have been Assigned to " + ListEmployeeSubtasks.Select(c => c.Task_ID).Distinct().Count().ToString() + " Task(s) having " + ListEmployeeSubtasks.Count.ToString() + " Subtasks under them";
                        else
                            AssignedTasks = "Currently you haven't been assigned to any new Task(s)";

                        if (ListEmployeeSubtasks != null && ListEmployeeSubtasks.Count > 0)
                            RefreshTasks();

                        if (ListAcceptedSubtasks != null && ListAcceptedSubtasks.Count > 0)
                            RefreshTaskProgress();
                    }
                    else
                    {
                        AssignedTasks = "Currently you haven't been assigned to any new Task(s)";
                        AcceptedSubtasks = null;
                    }

                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEmployeeSubtasks()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeSubtaskDetailsForUserAsync(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id, true);
        }

        private void RefreshTasks()
        {
            serviceClient.GetEmployeeTasksCompleted += (s, e) =>
            {
                try
                {
                    Tasks = e.Result.Where(c => ListEmployeeSubtasks.Count(d => d.Task_ID == c.Task_ID) > 0);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshTasks()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeeTasksAsync(Guid.Empty);
        }

        private void RefreshTaskProgress()
        {
            serviceClient.GetTaskProgressByUserCompleted += (s, e) =>
            {
                try
                {
                    TaskProgress = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshTaskProgress()\n\n" + ex.Message);
                }
            };
            serviceClient.GetTaskProgressByUserAsync(clsSecurity.loggedUser.user_id);
        }

        #endregion

        #region Commands & Methods

        private void Search()
        {
            try
            {

            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }

        }

        private void New()
        {
            ListEmployeeSubtasks = new List<EmployeeSubTasksDetailForUserView>();
            ListAcceptedSubtasks = new List<EmployeeSubTasksDetailForUserView>();
            AcceptRejectRemark = "";
            TasksRemarks = "";
            timerTask.Stop();
            TimeRemaining = "";
            RefreshEmployeeSubtasks();
        }

        public ICommand GetTasksBtn
        {
            get { return new RelayCommand(GetTasks, GetTasksCanExecute); }
        }

        private bool GetTasksCanExecute()
        {
            if (ListEmployeeSubtasks != null && ListEmployeeSubtasks.Count > 0)
                return true;
            else
                return false;
        }

        private void GetTasks()
        {
            EmployeeSubTasks = null;
            ActiveTasksW = new ActiveTasksWindow(this);
            ActiveTasksW.Show();
        }

        public ICommand btnAccept
        {
            get { return new RelayCommand(acceptTask, acceptRejectTaskCanExecute); }
        }

        public ICommand btnReject
        {
            get { return new RelayCommand(rejectTask, acceptRejectTaskCanExecute); }
        }

        private void rejectTask()
        {
            List<dtl_EmployeeSubTasks> Rejectlist = new List<dtl_EmployeeSubTasks>();
            string expiredList = "";

            foreach (var item in SelectedActiveTasks)
            {
                EmployeeSubTasksDetailForUserView Subtask = item as EmployeeSubTasksDetailForUserView;

                dtl_EmployeeSubTasks RejecObj = new dtl_EmployeeSubTasks();
                RejecObj.Employee_ID = Subtask.Employee_ID;
                RejecObj.SubTask_ID = (int)Subtask.SubTask_ID;
                RejecObj.save_datetime = DateTime.Now;
                RejecObj.save_user_id = clsSecurity.loggedUser.user_id;

                Rejectlist.Add(RejecObj);
            }

            if (Rejectlist != null && Rejectlist.Count > 0)
            {
                clsMessages.setMessage("You're going to Reject the task(s), do you want to proceed?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (ActiveTasksW != null)
                    {
                        ActiveTasksW.Close();
                    }

                    if (serviceClient.AcceptRejectSubtasks(Rejectlist.ToArray(), false, AcceptRejectRemark == string.Empty ? "Employee => Employee rejected the Task assigend by the supervisor" : "Employee => "+AcceptRejectRemark))
                    {
                        New();
                        clsMessages.setMessage("Task(s) Rejected Successfully");
                    }
                }
            }
        }

        private bool acceptRejectTaskCanExecute()
        {
            if (SelectedActiveTasks != null && SelectedActiveTasks.Count > 0)
                return true;
            else
                return false;
        }

        private void acceptTask()
        {
            List<dtl_EmployeeSubTasks> Acceptlist = new List<dtl_EmployeeSubTasks>();

            foreach (var item in SelectedActiveTasks)
            {
                EmployeeSubTasksDetailForUserView Subtask = item as EmployeeSubTasksDetailForUserView;
                //DateTime CurrentSubtaskEndDatetime = Subtask.SubTask_EndDate.Value.Date.AddTicks(Subtask.SubTask_EndTime.Value.Ticks);

                dtl_EmployeeSubTasks AcceptObj = new dtl_EmployeeSubTasks();
                AcceptObj.Employee_ID = Subtask.Employee_ID;
                AcceptObj.SubTask_ID = (int)Subtask.SubTask_ID;
                AcceptObj.save_user_id = clsSecurity.loggedUser.user_id;
                AcceptObj.save_datetime = DateTime.Now;

                Acceptlist.Add(AcceptObj);
            }

            if (Acceptlist != null && Acceptlist.Count > 0)
            {
                clsMessages.setMessage("Once you accepted a task there's no way you can revoke, do you want to proceed?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (ActiveTasksW != null)
                    {
                        ActiveTasksW.Close();
                    }

                    if (serviceClient.AcceptRejectSubtasks(Acceptlist.ToArray(), true, AcceptRejectRemark == string.Empty ? "Employee => Employee accepted the Task assigend by the supervisor" : "Employee => "+AcceptRejectRemark))
                    {
                        New();
                        clsMessages.setMessage("Task(s) Accepted Successfully");
                    }
                }
            }
        }

        private void setTime()
        {
            TimerDate = CurrentAcceptedSubtask.SubTask_EndDate.Value.Date.AddTicks(CurrentAcceptedSubtask.SubTask_EndTime.Value.Ticks);

            if (DateTime.Now > TimerDate || CurrentAcceptedSubtask.is_expired)
            {
                timerTask.Stop();
                TimeRemaining = "Expired";
            }
            else
            {
                timerTask.Start();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > TimerDate)
            {
                timerTask.Stop();
                TimeRemaining = "Expired";
            }
            else
            {
                if (TimerDate.Subtract(DateTime.Now).Duration().ToString().Trim().Length == 18)
                {
                    TimeRemaining = "0" + TimerDate.Subtract(DateTime.Now).Duration().ToString().Remove(TimerDate.Subtract(DateTime.Now).Duration().ToString().Length - 8, 8);
                }
                else
                    TimeRemaining = TimerDate.Subtract(DateTime.Now).Duration().ToString().Remove(TimerDate.Subtract(DateTime.Now).Duration().ToString().Length - 8, 8);

            }
        }

        public ICommand Updatebtn
        {
            get { return new RelayCommand(UpdateTask, UpdateTaskCanExecute); }
        }

        private bool UpdateTaskCanExecute()
        {
            if (CurrentAcceptedSubtask != null && TasksRemarks != null && TasksRemarks != string.Empty)
                return true;
            else
                return false;
        }

        private void UpdateTask()
        {

            trns_EmployeeTaskProgress TaskProgress = new trns_EmployeeTaskProgress();
            TaskProgress.SubTask_ID = (int)CurrentAcceptedSubtask.SubTask_ID;
            TaskProgress.Employee_ID = CurrentAcceptedSubtask.Employee_ID;
            TaskProgress.Remarks = "Employee => "+TasksRemarks;
            TaskProgress.Is_active = true;
            TaskProgress.save_user_id = clsSecurity.loggedUser.user_id;
            TaskProgress.save_datetime = DateTime.Now;

            if (CurrentAcceptedSubtask.is_pending == true && CurrentAcceptedSubtask.is_expired == false)
            {
                clsMessages.setMessage("This Task will be Submitted to the Supervisor for Review, do you want to proceed?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (serviceClient.UpdateTaskProgress(TaskProgress, true))
                    {
                        New();
                        clsMessages.setMessage("Task Updated Successfully");
                    }
                    else
                        clsMessages.setMessage("Task Update Failed");
                }
            }

            else if (CurrentAcceptedSubtask.is_expired == true && (CurrentAcceptedSubtask.is_pending == true || CurrentAcceptedSubtask.is_pending == false))
            {
                clsMessages.setMessage("Exipred tasks cannot be sent to the supervisor, however you can update the task or ask the supervisor for a date extension", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (serviceClient.UpdateTaskProgress(TaskProgress, false))
                    {
                        New();
                        clsMessages.setMessage("Task Updated Successfully");
                    }
                    else
                        clsMessages.setMessage("Task Update Failed");
                }
            }
            else
            {
                if (serviceClient.UpdateTaskProgress(TaskProgress, false))
                {
                    New();
                    clsMessages.setMessage("Task Updated Successfully");
                }
                else
                    clsMessages.setMessage("Task Update Failed");
            }

        }

        private void filterTaskProgress()
        {
            CurrentTaskProgress = null;
            CurrentTaskProgress = TaskProgress.Where(c => c.SubTask_ID == CurrentAcceptedSubtask.SubTask_ID);
        }

        public ICommand RefreshReassignBtn
        {
            get { return new RelayCommand(RefreshReassign); }
        }

        private void RefreshReassign()
        {
            if (CurrentAcceptedSubtask != null)
            {
                if (clsSecurity.loggedEmployee.employee_id != null)
                {
                    if (CurrentAcceptedSubtask.Is_Reassignable == true)
                    {
                        if (CurrentAcceptedSubtask.Is_Reassigned != true)
                        {
                            if (CurrentAcceptedSubtask.started_datetime != null)
                                clsMessages.setMessage("You cannot Reassign Tasks which you have already started.");

                            else
                            {
                                EmployeeTasksWindow TasksWindow = new EmployeeTasksWindow(CurrentAcceptedSubtask, MainWindow);
                                TasksWindow.ShowDialog();
                                New();
                            } 
                        }
                        else
                            clsMessages.setMessage("You have already reassigned this task");

                    }
                    else
                        clsMessages.setMessage("This Task is not Resassignable.");
                }
                else
                    clsMessages.setMessage("Current User is not an Employee");

            }

            else
            {
                New();
            }
        }

        public void OnwindowClose(object sender, CancelEventArgs e)
        {
            timerTask.Stop();
            MainTimer.Stop();
        }

        #endregion
    }
}
