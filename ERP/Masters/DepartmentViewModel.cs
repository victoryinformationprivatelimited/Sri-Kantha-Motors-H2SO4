/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-04-19                                                                                                
*   Purpose    : Department View Model                                                                                                
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Masters => Payroll                                                        
*                                                                                                                
************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;

namespace ERP
{
    class DepartmentViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public DepartmentViewModel()
        {
            this.RefreshDepartment();
            this.refreshCompanies();
            RefreshCompanyBranch();
            this.New();
        }
        #endregion

        #region Properties {get,set}

        private IEnumerable<z_Department> _Dept;
        public IEnumerable<z_Department> Dept
        {
            get
            {
                return this._Dept;
            }
            set
            {
                this._Dept = value;
                this.OnPropertyChanged("Dept");
            }
        }
        private z_Department _CurrentDept;
        public z_Department CurrentDept
        {
            get
            {
                return this._CurrentDept;
            }
            set
            {
                this._CurrentDept = value;
                this.OnPropertyChanged("CurrentDept");

            }
        }
        private IEnumerable<z_Company> _Companies;
        public IEnumerable<z_Company> Companies
        {
            get
            {
                return this._Companies; ;
            }
            set
            {
                this._Companies = value;
                this.OnPropertyChanged("Companies;");
            }
        }
        private z_Company _CurrentCompany;
        public z_Company CurrentCompany
        {
            get
            {
                return this._CurrentCompany;
            }
            set
            {
                this._CurrentCompany = value;
                this.OnPropertyChanged("CurrentCompany");

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
                if (this._Search == "")
                {
                    ReafreshDepartmentResult();
                }
                else
                {
                    SearchTextChanged();
                }

            }
        }
        private IEnumerable<z_CompanyBranches> _CompanyBranches;
        public IEnumerable<z_CompanyBranches> CompanyBranches
        {
            get { return _CompanyBranches; }
            set { _CompanyBranches = value; this.OnPropertyChanged("CompanyBranches"); }
        }


        #endregion

        #region Search Class
        public class relayCommand : ICommand
        {
            readonly Action<object> _Execute;
            readonly Predicate<object> _CanExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public relayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _Execute = execute;
                _CanExecute = canExecute;
            }


            public bool CanExecute(object parameter)
            {
                return _CanExecute == null ? true : _CanExecute(parameter);
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _Execute(parameter);
            }
        }
        #endregion

        #region Search Property
        relayCommand _OperationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_OperationCommand == null)
                {
                    _OperationCommand = new relayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }

                return this._OperationCommand;
            }
        }

        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region Search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }
        #endregion

        #region New Method
        void New()
        {
                this.CurrentDept = null;
            
                CurrentDept = new z_Department();
                CurrentDept.department_id = Guid.NewGuid();
                RefreshDepartment();          
        }
        #endregion

        #region New Button Class & Property
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
            bool isUpdate = false;
            z_Department newDept = new z_Department();
            newDept.department_id = CurrentDept.department_id;
            newDept.department_name = CurrentDept.department_name;
            newDept.branch_id = CurrentDept.branch_id;
            newDept.save_datetime = System.DateTime.Now;
            newDept.save_user_id = clsSecurity.loggedUser.user_id;
            newDept.modified_user_id = Guid.Empty;
            newDept.modified_datetime = System.DateTime.Now;
            newDept.delete_datetime = System.DateTime.Now;
            newDept.delete_user_id = Guid.Empty;
            newDept.telephone_01 = CurrentDept.telephone_01;
            newDept.telephone_02 = CurrentDept.telephone_02;
            newDept.fax = CurrentDept.fax;
            newDept.company_id = CurrentCompany.company_id;
            newDept.Emp_Count = CurrentDept.Emp_Count;
            newDept.Description = CurrentDept.Description;
            newDept.isdelete = false;

            IEnumerable<z_Department> alldeparments = this.serviceClient.GetDepartments();
            if (alldeparments != null)
            {
                foreach (z_Department Dept in alldeparments)
                {
                    if (Dept.department_id == CurrentDept.department_id)
                    {
                        isUpdate = true;
                    }
                }
            }
            if (newDept != null && newDept.department_name != null)
            {

                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(205))
                    {
                        newDept.modified_user_id = clsSecurity.loggedUser.user_id;
                        newDept.modified_datetime = System.DateTime.Now;
                        if (ValidateEmpCount())
                        {

                            if (this.serviceClient.UpdateDepartment(newDept))
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                New();
                                //RefreshDepartment();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            } 
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
                else
                {
                    if (clsSecurity.GetSavePermission(205))
                    {
                        if (ValidateEmpCount())
                        {
                            if (this.serviceClient.SaveDepartment(newDept))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                New();
                                //RefreshDepartment();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            } 
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }

            }
        }

        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentDept != null)
            {
                if (CurrentDept.department_id == null || CurrentDept.department_id == Guid.Empty)
                    return false;
                if (CurrentDept.department_name == null || CurrentDept.department_name == string.Empty)
                    return false;


            }
            else
            {
                return false;
            }
            return true;
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
            if (clsSecurity.GetDeletePermission(205))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do you want to delete this record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    z_Department depat = new z_Department();
                    depat.department_id = CurrentDept.department_id;
                    depat.modified_datetime = System.DateTime.Now;
                    depat.delete_user_id = clsSecurity.loggedUser.user_id;
                    if (serviceClient.DeleteDepartment(depat))
                    {
                        //RefreshDepartment();
                        New();
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        #endregion

        #region Delete Button Class & Property
        bool deleteCanExecute()
        {
            if (CurrentDept == null)
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

        #region User Define Methods
        private void RefreshDepartment()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
                {
                    this.Dept = e.Result.Where(a => a.isdelete == false);

                };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void refreshCompanies()
        {
            this.Companies = this.serviceClient.GetCompanies();
            setCompany();
        }


        public void ReafreshDepartmentResult()
        {
            this.serviceClient.GetSearchDepartmentCompleted += (s, e) =>
                {
                    this.Dept = (IEnumerable<z_Department>)this.serviceClient.GetSearchDepartment(Search);
                };
            this.serviceClient.GetSearchDepartmentAsync(Search);
        }

        private void setCompany()
        {
            foreach (z_Company item in Companies)
            {
                CurrentCompany = item;
            }
        }
        #endregion

        private void RefreshCompanyBranch()
        {
            this.serviceClient.GetAllCompanyBranchCompleted += (s, e) =>
            {
                this.CompanyBranches = e.Result;
            };
            this.serviceClient.GetAllCompanyBranchAsync();
        }

        #region Search Method for all Properties
        public void SearchTextChanged()
        {
            Dept = Dept.Where(e => e.department_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        private double _scaleSize;
        public double ScaleSize
        {
            get { return _scaleSize; }
            set { _scaleSize = value; OnPropertyChanged("ScaleSize"); }
        }
        public void scale()
        {
            ScaleSize = 1;
            double h = System.Windows.SystemParameters.PrimaryScreenHeight;
            double w = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (h * w == 1366 * 768)
                ScaleSize = 0.90;
            if (w * h == 1280 * 768)
                ScaleSize = 0.90;
            if (w * h == 1024 * 768)
                ScaleSize = 1.5;
        }

        #region Methods
        private int CalculateEmployeeCount()
        {
            int BrunchEmpCount = (int)(CompanyBranches == null ? 0 : CompanyBranches.Where(c => c.companyBranch_id == CurrentDept.branch_id).Sum(c => c.Emp_Count));
            //int CompanyEmpCount = (int).company_capacity;
            int result = (int)(Dept == null ? 0 : Dept.Where(c => c.branch_id == CurrentDept.branch_id && c.department_id != CurrentDept.department_id).Sum(d => d.Emp_Count));
            int sum = 0;
            if (result == null || result == 0)
            {
                sum = BrunchEmpCount - 0;
            }
            else
            {
                sum = BrunchEmpCount - result;
            }
            return sum;
        }

        private bool ValidateEmpCount()
        {
            if (CurrentDept.Emp_Count > CalculateEmployeeCount())
            {
                clsMessages.setMessage("Employee count exceeds the total capacity by " +(CurrentDept.Emp_Count - Math.Abs(CalculateEmployeeCount())) + " please try again.");
                return false;
            }
            else
                return true;
        }
        #endregion
    }
}
