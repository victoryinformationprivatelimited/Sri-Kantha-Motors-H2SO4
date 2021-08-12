using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Attendance_Process
{
    public class AuthorizedNoPayChangersModificaViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<trns_AuthorizedNoPays> ANopayList = new List<trns_AuthorizedNoPays>();

        public AuthorizedNoPayChangersModificaViewModel()
        {
            refrishEmployee();
            //refreshPeriod();
            CurrentDate = DateTime.Now.Date;
            IsFullday = true;
            refrishAuthorizedNoPay();
        }
        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }
        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee");
            if (CurrentEmployee != null && CurrentEmployee.employee_id != Guid.Empty)
            {
                if (AuthorizeNopay != null)
                {
                    SelectionItems = null;
                    AuthorizeNopay = AuthorizeNopay.Where(z => z.employee_id == CurrentEmployee.employee_id);
                    SelectionItems = AuthorizeNopay.ToList();
                }
            }
            }
        }
        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }

            set { _CurrentDate = value; this.OnPropertyChanged("CurrentDate"); }
        }

        //private bool _LeaveType;
        //public bool LeaveType
        //{
        //    get { return _LeaveType; }
        //    set { _LeaveType = value; this.OnPropertyChanged("LeaveType"); }
        //}

        private bool _ISHalfday;
        public bool ISHalfday
        {
            get { return _ISHalfday; }
            set { _ISHalfday = value; this.OnPropertyChanged("ISHalfday");
            if (ISHalfday == true)
            {
                IsFullday = false;
            }
            }
        }

        private bool _IsFullday;
        public bool IsFullday
        {
            get { return _IsFullday; }
            set { _IsFullday = value; this.OnPropertyChanged("IsFullday");
            if (IsFullday == true)
            {
                ISHalfday = false;
            }
            }
        }



        private List<trns_AuthorizedNoPays> _SelectionItems = new List<trns_AuthorizedNoPays>();
        public List<trns_AuthorizedNoPays> SelectionItems
        {
            get { return this._SelectionItems; }
            set
            {
                this._SelectionItems = value;
                this.OnPropertyChanged("SelectionItems");
            }
        }
        private IEnumerable<trns_AuthorizedNoPays> _AuthorizeNopay;
        public IEnumerable<trns_AuthorizedNoPays> AuthorizeNopay
        {
            get { return _AuthorizeNopay; }
            set { _AuthorizeNopay = value;this.OnPropertyChanged("AuthorizeNopay"); }
        }

        private trns_AuthorizedNoPays _CurrentAuthorizedNoPay;
        public trns_AuthorizedNoPays CurrentAuthorizedNoPay
        {
            get { return _CurrentAuthorizedNoPay; }

            set { _CurrentAuthorizedNoPay = value; this.OnPropertyChanged("CurrentAuthorizedNoPay"); }
        }

        private void refrishAuthorizedNoPay()
        {
            this.serviceClient.GetEmployeeAuthorizeNopayCompleted += (s, e) =>
            {
                this.AuthorizeNopay = e.Result;
            };
            this.serviceClient.GetEmployeeAuthorizeNopayAsync();
        }
        private void refrishEmployee()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetEmployeesAsync();
        }

        private void EmployeeSearchButton()
        {
            EmployeeSearchWindow searchWindow = new EmployeeSearchWindow();
            searchWindow.ShowDialog();
            if (searchWindow.viewModel.CurrentEmployeeSearchView.employee_id != null)
                this.CurrentEmployee = Employees.FirstOrDefault(z => z.employee_id == searchWindow.viewModel.CurrentEmployeeSearchView.employee_id);
                searchWindow.Close();
            
        }
        public ICommand Add
        {
            get { return new RelayCommand(AddTolist, AddCanExecute); }
        }

        public ICommand Save
        {
            get { return new RelayCommand(SaveData, SaveDataCanExecute); }
        }

        private void SaveData()
        {
            if (serviceClient.SaveAuthorizeNoPay(SelectionItems.ToArray()))
            {
                MessageBox.Show("Authorized No pay Sussfully Added");
                refrishAuthorizedNoPay();
                SelectionItems=null;
                ANopayList.Clear();
                SelectionItems = AuthorizeNopay.ToList();
            }
            else
            {
                MessageBox.Show("Authorized No pay Save Fail");
            }
           
        }

        private bool SaveDataCanExecute()
        {
            if (SelectionItems == null)
            {
                return false;
            }
            return true;
        }
        private bool AddCanExecute()
        {
            if (CurrentEmployee == null)
            {
                return false;
            }
            if (CurrentDate == null)
            {
                return false;
            }
          
            return true;
        }

        private void AddTolist()
        {
            //ANopayList.Clear();
            SelectionItems = null;
            trns_AuthorizedNoPays nopay = new trns_AuthorizedNoPays();
            nopay.employee_id = CurrentEmployee.employee_id;
            nopay.nopay_date = CurrentDate.Date;
            nopay.is_full_day = IsFullday;
            nopay.is_half_day = ISHalfday;
            nopay.is_active = true;
            ANopayList.Add(nopay);
            SelectionItems = ANopayList;
            
        }

        public ICommand EmployeeSearch
        {
            get { return new RelayCommand(EmployeeSearchButton); }
        }

        public string getDateString(DateTime date)
        {
            string date_string = "";
            // 6/18/2014 12:00:00 AM
            try
            {

                string[] words = date.ToString().Split(' ');
                date_string = words[0];
                string[] words2 = date_string.Split('/');
                string day = words2[1];
                string Month = words2[0];
                string year = words2[2].ToString();
                if (Month.Length == 1)
                {
                    Month = "0" + Month;
                }
                if (day.Length == 1)
                {
                    day = "0" + day;
                }
                date_string = year + "-" + Month + "-" + day;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message);
            }
            return date_string;
        }
        
    }
}
