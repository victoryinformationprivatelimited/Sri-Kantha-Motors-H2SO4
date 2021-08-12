using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Collections;

namespace ERP.Leave
{
    public class ApproveLeaveViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        #region Fields

        List<PendingLeavesForSupervisorApproval> ListLeavePool;

        #endregion

        public ApproveLeaveViewModel()
        {
            ListLeavePool = new List<PendingLeavesForSupervisorApproval>();
            refreshLeavePool();
            refreshSupervisors();
            refreshUserEmployee();
            SearchIndex = 0;
            Search = "";
        }

        private IEnumerable<PendingLeavesForSupervisorApproval> _LeavePool;
        public IEnumerable<PendingLeavesForSupervisorApproval> LeavePool
        {
            get { return _LeavePool; }
            set { _LeavePool = value; OnPropertyChanged("LeavePool"); }
        }

        private PendingLeavesForSupervisorApproval _CurrentLeavePool;
        public PendingLeavesForSupervisorApproval CurrentLeavePool
        {
            get { return _CurrentLeavePool; }
            set { _CurrentLeavePool = value; OnPropertyChanged("CurrentLeavePool"); }
        }

        private IList _CurrentLeavePoolList = new ArrayList();
        public IList CurrentLeavePoolList
        {
            get { return _CurrentLeavePoolList; }
            set { _CurrentLeavePoolList = value; OnPropertyChanged("CurrentLeavePoolList"); }
        }

        private IEnumerable<EmployeeSupervisorsView> _Supervisors;
        public IEnumerable<EmployeeSupervisorsView> Supervisors
        {
            get { return _Supervisors; }
            set { _Supervisors = value; OnPropertyChanged("Supervisors"); }
        }

        private EmployeeSupervisorsView _CurrentSupervisor;
        public EmployeeSupervisorsView CurrentSupervisor
        {
            get { return _CurrentSupervisor; }
            set { _CurrentSupervisor = value; OnPropertyChanged("CurrentSupervisor"); }

        }

        private IEnumerable<usr_UserEmployee> _Users;
        public IEnumerable<usr_UserEmployee> Users
        {
            get { return _Users; }
            set { _Users = value; OnPropertyChanged("Users"); }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null && CurrentSupervisor != null) SearchLeavePool(); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        public ICommand BtnApprove
        {
            get { return new RelayCommand(ApproveLeave); }
        }

        public ICommand BtnCancelApprove
        {
            get { return new RelayCommand(CancelLeave); }
        }

        public ICommand BtnTransferReponse
        {
            get { return new RelayCommand(TransferReponse); }
        }

        private void CancelLeave()
        {
            if (CurrentLeavePoolList == null || CurrentLeavePoolList.Count == 0)
                clsMessages.setMessage("Please Select an Employee(s) and Try Again");
            else if (CurrentSupervisor == null)
                clsMessages.setMessage("Please Select an Employee Supervisor");
            else
            {
                if (clsSecurity.GetDeletePermission(407))
                {
                    try
                    {
                        Guid? CurrentUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;

                        if (CurrentUserEmployee != null)
                        {
                            if (CurrentSupervisor.supervisor_employee_id == CurrentUserEmployee)
                            {
                                if (CurrentLeavePoolList != null && CurrentLeavePoolList.Count > 0)
                                {
                                    List<PendingLeavesForSupervisorApproval> LeavePoolList = new List<PendingLeavesForSupervisorApproval>();

                                    foreach (var item in CurrentLeavePoolList)
                                    {
                                        PendingLeavesForSupervisorApproval CancelLeavePool = new PendingLeavesForSupervisorApproval();

                                        CancelLeavePool = item as PendingLeavesForSupervisorApproval;
                                        if (CancelLeavePool.leave_category_id != new Guid("9B615C80-32D7-4951-BABC-04AD7193BC32"))
                                        {
                                            CancelLeavePool.is_pending_for_approval = false;
                                            CancelLeavePool.rejected = true;
                                            CancelLeavePool.is_active = false;
                                            LeavePoolList.Add(CancelLeavePool);
                                        }
                                        else
                                            clsMessages.setMessage("Leiu Leaves Cannot Be Rejected");
                                    }

                                    if (LeavePoolList != null && LeavePoolList.Count>0)
	                                {
		                                if (serviceClient.CancelEmplyeeLeavesBySupervisor(LeavePoolList.ToArray()))
                                        {
                                            refreshLeavePool();
                                            clsMessages.setMessage("Leave(s) Cancelled Sucessfully");
                                        }

                                        else
                                            clsMessages.setMessage("Leave(s) Cancellation Failed"); 
	                                }

                                }


                                SearchIndex = 0;
                                Search = "";
                            }
                            else
                                clsMessages.setMessage("Current User Does not have Permission to perform this Task");

                        }

                        else
                            clsMessages.setMessage("Current User Is not an Employee to perform this Task");
                    }
                    catch (Exception ex)
                    {
                        clsMessages.setMessage(ex.ToString());
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to reject");
            }
        }

        private void ApproveLeave()
        {
            if (CurrentLeavePoolList == null || CurrentLeavePoolList.Count == 0)
                clsMessages.setMessage("Please Select an Employee(s) and Try Again");
            else if (CurrentSupervisor == null)
                clsMessages.setMessage("Please Select an Employee Supervisor");
            else
            {
                if (clsSecurity.GetSavePermission(407))
                {
                    try
                    {
                        Guid? CurrentUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;

                        if (CurrentUserEmployee != null)
                        {
                            if (CurrentSupervisor.supervisor_employee_id == CurrentUserEmployee)
                            {
                                if (CurrentLeavePoolList != null && CurrentLeavePoolList.Count > 0)
                                {
                                    List<PendingLeavesForSupervisorApproval> LeavePoolList = new List<PendingLeavesForSupervisorApproval>();

                                    foreach (var item in CurrentLeavePoolList)
                                    {
                                        PendingLeavesForSupervisorApproval SaveLeavePool = new PendingLeavesForSupervisorApproval();

                                        SaveLeavePool = item as PendingLeavesForSupervisorApproval;
                                        SaveLeavePool.approved = true;
                                        SaveLeavePool.is_pending_for_approval = false;
                                        SaveLeavePool.is_active = false;
                                        SaveLeavePool.approved_user_id = clsSecurity.loggedUser.user_id;
                                        LeavePoolList.Add(SaveLeavePool);

                                    }

                                    if (serviceClient.ApproveEmployeeLeavesBySupervisor(LeavePoolList.ToArray()))
                                    {
                                        refreshLeavePool();
                                        clsMessages.setMessage("Leave(s) Approved Sucessfully");
                                    }

                                    else
                                        clsMessages.setMessage("Leave(s) Approval Failed");

                                }


                                SearchIndex = 0;
                                Search = "";
                            }
                            else
                                clsMessages.setMessage("Current User Does not have Permission to perform this Task");

                        }

                        else
                            clsMessages.setMessage("Current User Is not an Employee to perform this Task");
                    }
                    catch (Exception ex)
                    {
                        clsMessages.setMessage(ex.ToString());
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Approve");
            }
        }

        private void TransferReponse()
        {
            if (CurrentLeavePoolList == null || CurrentLeavePoolList.Count == 0)
                clsMessages.setMessage("Please Select an Employee(s) and Try Again");
            else if (CurrentSupervisor == null)
                clsMessages.setMessage("Please Select an Employee Supervisor");
            else
            {
                if (clsSecurity.GetSavePermission(407))
                {
                    try
                    {
                        Guid? CurrentUserEmployee = Users.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;

                        if (CurrentUserEmployee != null)
                        {
                            if (CurrentSupervisor.supervisor_employee_id == CurrentUserEmployee)
                            {
                                if (CurrentLeavePoolList != null && CurrentLeavePoolList.Count > 0)
                                {
                                    List<PendingLeavesForSupervisorApproval> LeavePoolList = new List<PendingLeavesForSupervisorApproval>();

                                    foreach (var item in CurrentLeavePoolList)
                                    {
                                        PendingLeavesForSupervisorApproval TransferLeavePool = new PendingLeavesForSupervisorApproval();

                                        TransferLeavePool = item as PendingLeavesForSupervisorApproval;
                                        TransferLeavePool.transferred = true;
                                        TransferLeavePool.is_active = false;
                                        LeavePoolList.Add(TransferLeavePool);
                                    }

                                    if (serviceClient.TransferEmplyeeLeavesBySupervisor(LeavePoolList.ToArray()))
                                    {
                                        refreshLeavePool();
                                        clsMessages.setMessage("Leave(s) Response Transfered Sucessfully");
                                    }

                                    else
                                        clsMessages.setMessage("Leave(s) Transfer Failed");

                                }


                                SearchIndex = 0;
                                Search = "";
                            }
                            else
                                clsMessages.setMessage("Current User Does not have Permission to perform this Task");

                        }

                        else
                            clsMessages.setMessage("Current User Is not an Employee to perform this Task");
                    }
                    catch (Exception ex)
                    {
                        clsMessages.setMessage(ex.ToString());
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to transfer");
            }
        }

        private void refreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                Users = e.Result;
            };
            serviceClient.GetUserEmployeesAsync();
        }

        private void refreshSupervisors()
        {
            this.serviceClient.GetAllEmployeeSupervisorByModuleCompleted += (s, e) =>
            {
                this.Supervisors = e.Result;
                if (clsSecurity.loggedEmployee != null && Supervisors != null && Supervisors.Count(c => c.supervisor_employee_id == clsSecurity.loggedEmployee.employee_id) > 0)
                {
                    CurrentSupervisor = Supervisors.FirstOrDefault(c => c.supervisor_employee_id == clsSecurity.loggedEmployee.employee_id);
                }
            };
            this.serviceClient.GetAllEmployeeSupervisorByModuleAsync(new Guid("EE0D8A55-5A31-4FDB-A36F-36643A26B1AA"));
        }

        private void refreshLeavePool()
        {
            this.serviceClient.GetAllPendingLeavesForSupervisorApprovalCompleted += (s, e) =>
            {
                this.LeavePool = e.Result;
                if (LeavePool != null)
                {
                    ListLeavePool = LeavePool.ToList();
                }
            };
            this.serviceClient.GetAllPendingLeavesForSupervisorApprovalAsync(clsSecurity.loggedUser.user_id);
        }

        private void SearchLeavePool()
        {
            LeavePool = null;
            LeavePool = ListLeavePool;

            try
            {
                if (SearchIndex == 0)
                    LeavePool = LeavePool.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(Search.ToUpper()) && c.supervisor_employee_id == CurrentSupervisor.supervisor_employee_id);
                if (SearchIndex == 1)
                    LeavePool = LeavePool.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()) && c.supervisor_employee_id == CurrentSupervisor.supervisor_employee_id);
                if (SearchIndex == 2)
                    LeavePool = LeavePool.Where(c => c.name != null && c.name.ToUpper().Contains(Search.ToUpper()) && c.supervisor_employee_id == CurrentSupervisor.supervisor_employee_id);
            }
            catch (Exception)
            {
            }
        }
    }
}
