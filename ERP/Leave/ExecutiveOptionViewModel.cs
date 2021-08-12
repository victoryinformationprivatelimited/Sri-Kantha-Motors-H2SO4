using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.AttendanceService;
using System.Windows.Input;
using ERP.HelperClass;
using System.Collections;

namespace ERP.Leave
{
    class ExecutiveOptionViewModel : ViewModelBase
    {
        #region Fields

        List<ExecutiveOptionView> EmployeeOptionViewAll;
        List<ExecutiveOptionView> ExecutiveList;

        #endregion

        #region Service Client

        AttendanceServiceClient serviceClient;    
    
        #endregion

        #region Constructor

        public ExecutiveOptionViewModel()
        {
            serviceClient = new AttendanceServiceClient();
            EmployeeOptionViewAll = new List<ExecutiveOptionView>();
            RefreshExecutives();
        }
        #endregion

        #region Properties  
    
        private IEnumerable<AttendEmployeeSummaryView> _EmployeeSummary;
        public IEnumerable<AttendEmployeeSummaryView> EmployeeSummary
        {
            get 
            {
                return _EmployeeSummary;
            }
            set 
            {
                _EmployeeSummary = value;
                OnPropertyChanged("EmployeeSummary");
            }
        }

        private AttendEmployeeSummaryView _CurrentEmployeeSummary;
        public AttendEmployeeSummaryView CurrentEmployeeSummary
        {
            get
            { 
                return _CurrentEmployeeSummary;
            }
            set 
            { 
                _CurrentEmployeeSummary = value; 
                OnPropertyChanged("CurrentEmployeeSummary");
                if (CurrentEmployeeSummary != null)
                    FilterEmployeeOptions(); 
                else
                { 
                    EmployeeOptionView = null;
                    AttGroup = null;
                } 
            }
        }


        private IEnumerable<ExecutiveOptionView> _EmployeeOptionView;
        public IEnumerable<ExecutiveOptionView> EmployeeOptionView
        {
            get
            {
                return _EmployeeOptionView;
            }
            set
            {
                _EmployeeOptionView = value;
                OnPropertyChanged("EmployeeOptionView");
            }
        }


        private ExecutiveOptionView _CurrentEmployeeOptionView;
        public ExecutiveOptionView CurrentEmployeeOptionView
        {
            get
            {
                return _CurrentEmployeeOptionView;
            }
            set 
            { 
                _CurrentEmployeeOptionView = value;
                OnPropertyChanged("CurrentEmployeeOptionView");
            }
        }

        private string _AttGroup;

        public string AttGroup
        {
            get
            { 
                return _AttGroup;
            }
            set
            { 
                _AttGroup = value;
                OnPropertyChanged("AttGroup"); 
            }
        }

        private IList _CurrentEmployeeOptionViewList = new ArrayList();
        public IList CurrentEmployeeOptionViewList
        {
            get { return _CurrentEmployeeOptionViewList; }
            set { _CurrentEmployeeOptionViewList = value; OnPropertyChanged("CurrentEmployeeOptionViewList"); }
        }
        #endregion

        #region Refresh
        void RefreshExecutives()
        {
            serviceClient.GetExecutiveDetailsFirstCompleted += (s, e) =>
            {
                try
                {
                    EmployeeSummary = e.Result.OrderBy(c=> Convert.ToInt32(c.emp_id));
                    if(EmployeeSummary != null && EmployeeSummary.Count()>0)
                        RefreshExecutivesDetails();
                }
                catch (Exception)
                {
                     clsMessages.setMessage("Employees refresh is failed");
                }
            };
            serviceClient.GetExecutiveDetailsFirstAsync();           
        }

        void RefreshExecutivesDetails()
        {
            serviceClient.GetExecutiveDetailsCompleted += (s, e) =>
            {
                try
                {
                    EmployeeOptionViewAll.Clear();
                    EmployeeOptionView = e.Result;
                    if (EmployeeOptionView != null && EmployeeOptionView.Count() > 0)
                        EmployeeOptionViewAll = EmployeeOptionView.ToList();
                    EmployeeOptionView = null;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Employees refresh is failed");
                }
            };
            serviceClient.GetExecutiveDetailsAsync();
        }

        #endregion

        #region Commands and Methods

        private void FilterEmployeeOptions()
        {
            EmployeeOptionView = null;
            EmployeeOptionView = EmployeeOptionViewAll.Where(c => c.employee_id == CurrentEmployeeSummary.employee_id);
            AttGroup = EmployeeOptionView.FirstOrDefault().attendance_group_name;
        }

        public ICommand LeiuLeave
        {
            get
            {
                return new RelayCommand(leiu);
            }
        }

        void leiu()
        {
            try
            {
                if (clsSecurity.GetSavePermission(412))
                {
                    if (CurrentEmployeeOptionViewList != null && CurrentEmployeeOptionViewList.Count > 0)
                    {
                        ExecutiveList = new List<ExecutiveOptionView>();
                        foreach (var item in CurrentEmployeeOptionViewList)
                        {
                            ExecutiveOptionView CurrentExecutive = new ExecutiveOptionView();
                            CurrentExecutive = item as ExecutiveOptionView;
                            CurrentExecutive.is_leiu_leave = true;
                            ExecutiveList.Add(CurrentExecutive);
                        }
                        DateTime dt = DateTime.Now;
                        Guid uid = clsSecurity.loggedUser.user_id;
                        if (ExecutiveList != null && ExecutiveList.Count > 0)
                        {
                            if (serviceClient.UpdateExecutiveDetails(ExecutiveList.ToArray(), false, dt, uid))
                            {
                                clsMessages.setMessage("Record(s) Added To Leiu Leave Successfully");
                                RefreshExecutivesDetails();
                                RefreshExecutives();
                            }
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("Please Select a Record");
                    } 
                }
                else
                    clsMessages.setMessage("You don't permission to add to leiu leave");

            }
            catch (Exception)
            {
                clsMessages.setMessage("Record(s) Added To Leiu Leave Failed");
            }
        }

        public ICommand Pay
        {
            get
            {
                return new RelayCommand(pay);
            }
        }

        void pay()
        {
            try
            {
                if (clsSecurity.GetSavePermission(412))
                {
                    if (CurrentEmployeeOptionViewList != null && CurrentEmployeeOptionViewList.Count > 0)
                    {
                        if (CurrentEmployeeOptionView.attendance_group_id == 7 || CurrentEmployeeOptionView.attendance_group_id == 11 || CurrentEmployeeOptionView.attendance_group_id == 15)
                        {
                            ExecutiveList = new List<ExecutiveOptionView>();
                            foreach (var item in CurrentEmployeeOptionViewList)
                            {
                                ExecutiveOptionView CurrentExecutive = new ExecutiveOptionView();
                                CurrentExecutive = item as ExecutiveOptionView;

                                CurrentExecutive.is_pay = true;
                                ExecutiveList.Add(CurrentExecutive);


                            }
                            DateTime dt = DateTime.Now;
                            Guid uid = clsSecurity.loggedUser.user_id;
                            if (ExecutiveList != null && ExecutiveList.Count > 0)
                            {
                                if (serviceClient.UpdateExecutiveDetails(ExecutiveList.ToArray(), true, dt, uid))
                                {
                                    clsMessages.setMessage("Record(s) Added To Pay Successfully");
                                    RefreshExecutivesDetails();
                                    RefreshExecutives();
                                }
                            }
                        }
                        else
                        {
                            clsMessages.setMessage("Non Executives Are Not Paid");
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("Please Select a Record");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to add to pay");

            }
            catch (Exception)
            {
                clsMessages.setMessage("Record(s) Added To Pay Failed");
            }
        }
        #endregion

    }
}
