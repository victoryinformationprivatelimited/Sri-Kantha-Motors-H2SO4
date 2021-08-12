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
    public class ExtraOtViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<trns_ExtraOt> ExtraOtList = new List<trns_ExtraOt>();

        public ExtraOtViewModel()
        {
            refrishEmployee();
            CurrentDate = DateTime.Now.Date;
            refrishEmployeeExtraOt();
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; this.OnPropertyChanged("CurrentDate"); }
        }

        private TimeSpan _ExtraOtTime;
        public TimeSpan ExtraOtTime
        {
            get { return _ExtraOtTime; }
            set { _ExtraOtTime = value; this.OnPropertyChanged("ExtraOtTime"); }
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
            set
            {
                _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee");
                if (CurrentEmployee != null && CurrentEmployee.employee_id != Guid.Empty)
                {
                    if (ExtraOt != null)
                    {
                        SelectionItems = null;
                        ExtraOt = ExtraOt.Where(z => z.employee_id == CurrentEmployee.employee_id);
                        SelectionItems = ExtraOt.ToList();
                    }
                }
            }
        }
        private List<trns_ExtraOt> _SelectionItems = new List<trns_ExtraOt>();
        public List<trns_ExtraOt> SelectionItems
        {
            get { return this._SelectionItems; }
            set
            {
                this._SelectionItems = value;
                this.OnPropertyChanged("SelectionItems");
            }
        }

        private IEnumerable<trns_ExtraOt> _ExtraOt;
        public IEnumerable<trns_ExtraOt> ExtraOt
        {
            get { return _ExtraOt; }
            set { _ExtraOt = value; this.OnPropertyChanged("ExtraOt"); }
        }

        private trns_ExtraOt _CurrentExtraOt;
        public trns_ExtraOt CurrentExtraOt
        {
            get { return _CurrentExtraOt; }
            set { _CurrentExtraOt = value; this.OnPropertyChanged("CurrentExtraOt"); }
        }
        

        private void refrishEmployee()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetEmployeesAsync();
        }

        private void refrishEmployeeExtraOt()
        {
            this.serviceClient.GetEmployeeExtraOtCompleted += (s, e) =>
            {
                this.ExtraOt = e.Result;
            };
            this.serviceClient.GetEmployeeExtraOtAsync();
        }

        public ICommand EmployeeSearch
        {
            get { return new RelayCommand(EmployeeSearchButton); }
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
            SelectionItems = null;
            trns_ExtraOt ot = new trns_ExtraOt();
            ot.employee_id = CurrentEmployee.employee_id;
            ot.ot_date = CurrentDate.Date;
            ot.extra_ot_time = ExtraOtTime;
            ExtraOtList.Add(ot);
            SelectionItems = ExtraOtList;
        }

        public ICommand Save
        {
            get { return new RelayCommand(SaveData, SaveDataCanExecute); }
        }

        private bool SaveDataCanExecute()
        {
            if (SelectionItems == null)
            {
                return false;
            }
            return true;
        }

        private void SaveData()
        {
            if (serviceClient.SaveExtraOt(SelectionItems.ToArray()))
            {
                MessageBox.Show("Extra OT Sussfully Added");
                refrishEmployeeExtraOt();
                SelectionItems = null;
                ExtraOtList.Clear();
                SelectionItems = ExtraOt.ToList();
            }
            else
            {
                MessageBox.Show("Extra OT Save Fail");
            }
           
        }
    }
}
