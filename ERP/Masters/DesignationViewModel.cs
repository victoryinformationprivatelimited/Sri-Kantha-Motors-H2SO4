/************************************************************************************************************
 *   Author     :  Heshantha Lakshitha                                                                       *                    
 *   Date       :  23-04-2013                                                                                *             
 *   Purpose    :  Keep the list of Master Designation                                                       *                                    
 *   Company    :  Victory Information                                                                       *     
 *   Module     :  ERP System => Masters => Payroll                                                          * 
 *                                                                                                           *     
 ************************************************************************************************************/
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP
{

    class DesignationViewModel : ViewModelBase
    {

        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public DesignationViewModel()
        {

            reafreshCompanies();
            this.RefreshDesignations();
            this.New();
        }


        #endregion

        #region Properties
        private IEnumerable<z_Designation> _Designations;

        public IEnumerable<z_Designation> Designations
        {
            get
            {
                return this._Designations;
            }
            set
            {
                this._Designations = value;
                this.OnPropertyChanged("Designations");
            }
        }

        private IEnumerable<z_Company> _Company;
        public IEnumerable<z_Company> Company
        {
            get
            {
                return this._Company;
            }
            set
            {
                this._Company = value;
                this.OnPropertyChanged("Company");
            }
        }


        private z_Designation _CurrentDesignation;

        public z_Designation CurrentDesignation
        {
            get
            {
                return this._CurrentDesignation;
            }
            set
            {
                this._CurrentDesignation = value;
                this.OnPropertyChanged("Currentdesignation");
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
                    RefreshDesignations();
                }
                else
                {
                    searchTextChanged();
                }

            }
        }
        #endregion

        #region Button Command

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(delete, deleteCanExecute);
            }
        }

        #region Button Properties
        //private SaveButton _SaveDesignations;

        //public SaveButton SaveDesignations
        //{
        //    get { return _SaveDesignations; }

        //}

        //private NewButton _NewDesignations;

        //public NewButton NewDesignations
        //{
        //    get { return _NewDesignations; }

        //}

        //private DeleteButton _DeleteDesignations;

        //public DeleteButton DeleteDesignations
        //{
        //    get { return _DeleteDesignations; }

        //}



        #endregion
        #endregion

        #region UserDefined Method
        #region Save Method
        private void Save()
        {
            bool isUpdate = false;

            z_Designation NewDesignations = new z_Designation();
            NewDesignations.designation_id = CurrentDesignation.designation_id;
            NewDesignations.designation = CurrentDesignation.designation;
            NewDesignations.Emp_Count = CurrentDesignation.Emp_Count;
            NewDesignations.Description = CurrentDesignation.Description;
            NewDesignations.save_datetime = System.DateTime.Now;
            NewDesignations.save_user_id = Guid.Empty;
            NewDesignations.modified_user_id = Guid.Empty;
            NewDesignations.modified_datetime = System.DateTime.Now;
            NewDesignations.delete_datetime = System.DateTime.Now;
            NewDesignations.delete_user_id = Guid.Empty;
            NewDesignations.isdelete = false;

            IEnumerable<z_Designation> alldestinations = this.serviceClient.GetDesignations();

            if (alldestinations != null)
            {
                foreach (z_Designation des in alldestinations)
                {
                    if (des.designation_id == CurrentDesignation.designation_id)
                    {
                        isUpdate = true;
                    }
                }
            }
            if (NewDesignations != null && NewDesignations != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(207))
                    {
                        if (ValidateEmpCount())
                        {
                            NewDesignations.modified_datetime = System.DateTime.Now;
                            NewDesignations.modified_user_id = clsSecurity.loggedUser.user_id;

                            if (this.serviceClient.UpdateDesignation(NewDesignations))
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
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
                    if (clsSecurity.GetSavePermission(207))
                    {
                        if (ValidateEmpCount())
                        {
                            NewDesignations.save_datetime = System.DateTime.Now;
                            NewDesignations.save_user_id = clsSecurity.loggedUser.user_id;

                            if (this.serviceClient.SaveDesignation(NewDesignations))

                                clsMessages.setMessage(Properties.Resources.SaveSucess);
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
                RefreshDesignations();
            }
            else
            {
                clsMessages.setMessage("Please mension Desination Name  !");
            }
        }

        bool saveCanExecute()
        {
            if (CurrentDesignation != null)
            {
                if (CurrentDesignation.designation_id == null || CurrentDesignation.designation_id == Guid.Empty)
                    return false;

                if (CurrentDesignation.designation == null || CurrentDesignation.designation == string.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }
        #endregion

        #region New Method
        private void New()
        {
                this.CurrentDesignation = null;
                CurrentDesignation = new z_Designation();
                CurrentDesignation.designation_id = Guid.NewGuid();
                RefreshDesignations();
        }
        bool newCanExecute()
        {
            return true;
        }
        #endregion

        #region DeleteMethod
        private void delete()
        {
            if (clsSecurity.GetDeletePermission(207))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (Result == MessageBoxResult.Yes)
                {
                    z_Designation dese = new z_Designation();
                    dese.designation_id = CurrentDesignation.designation_id;
                    dese.delete_datetime = System.DateTime.Now;
                    dese.delete_user_id = clsSecurity.loggedUser.user_id;

                    if (serviceClient.DeletetDesignations(dese))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    RefreshDesignations();

                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentDesignation == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Refresh Method
        private void RefreshDesignations()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                this.Designations = e.Result.Where(a => a.isdelete == false);
            };
            this.serviceClient.GetDesignationsAsync();

        }

        private void reafreshCompanies()
        {
            this.serviceClient.GetCompaniesCompleted += (s, e) =>
            {
                this.Company = e.Result;
            };
            this.serviceClient.GetCompaniesAsync();
        }
        #endregion

        #endregion

        #region Grid Data Refresh
        private void searchTextChanged()
        {
            Designations = Designations.Where(e => e.designation.ToUpper().Contains(Search.ToUpper())).ToList();

        }

        #endregion

        #region Search Key
        public class relayCommand : ICommand
        {
            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {

            }
            public relayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null ? true : _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    CommandManager.RequerySuggested += value;
                }
                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }



            public void Execute(object parameter)
            {
                _execute(parameter);
            }
        }
        #endregion

        #region Search Property
        relayCommand _operationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_operationCommand == null)
                {
                    _operationCommand = new relayCommand(param => this.ExecuteCommand(),
                        Param => this.CanExecuteCommand);
                }
                return _operationCommand;
            }
        }
        bool CanExecuteCommand
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
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

        #region Methods For Calcutale Employee Count

        private int CalculateEmployeeCount()
        {
            int CompEmpCount = (int)(Company == null ? 0 : (Company.FirstOrDefault().company_capacity == null ? 0 : Company.FirstOrDefault().company_capacity));
            int result = (int)(Designations == null ? 0 : Designations.Where(c=> c.designation_id != CurrentDesignation.designation_id).Sum(d=> d.Emp_Count));
            int sum = 0;
            if (result == null || result == 0)
            {
                sum = CompEmpCount - 0;
            }
            else
            {
                sum = CompEmpCount - result;
            }
            return sum;
        }

        private bool ValidateEmpCount()
        {
            if (CurrentDesignation.Emp_Count > CalculateEmployeeCount())
            {
                clsMessages.setMessage("Employee count exceeds the total capacity by " + (CurrentDesignation.Emp_Count - Math.Abs(CalculateEmployeeCount())) + " please try again.");
                return false;
            }
            else
                return true;
        }

        #endregion
    }
}



