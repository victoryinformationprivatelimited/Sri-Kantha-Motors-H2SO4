/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-06-06                                                                                               
*   Purpose    : Company Variables View Model                                                                                                
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Masters => Payroll                                                        
*                                                                                                                
************************************************************************************************************/
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Masters
{
    class CompanyVariablesViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public CompanyVariablesViewModel()
        {
            this.reafreshCompanyVariables();
            this.reafreshCompanies();
            this.New();

        }
        #endregion

        #region Properties
        private IEnumerable<z_CompanyVariable> _CompanyVariables;
        public IEnumerable<z_CompanyVariable> CompanyVariables
        {
            get
            {
                return this._CompanyVariables;
            }
            set
            {
                this._CompanyVariables = value;
                this.OnPropertyChanged("CompanyVariables");
            }
        }

        private z_CompanyVariable _CurrentCompanyVariable;
        public z_CompanyVariable CurrentCompanyVariable
        {
            get
            {
                return this._CurrentCompanyVariable;
            }
            set
            {
                this._CurrentCompanyVariable = value;
                this.OnPropertyChanged("CurrentCompanyVariable");

            }
        }

        public void SetCompanyName()
        {
            _CompanyName = "Victory";
        }
        public static string _CompanyName { get; private set; }

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
        private bool _IsPracentage;
        public bool IsPracentage
        {
            get
            {
                return this._IsPracentage;
            }
            set
            {
                this._IsPracentage = value;
                this.OnPropertyChanged("IsPracentage");
                if (_IsPracentage == true)
                {
                    this.IsVisible = Visibility.Visible;
                }
                else
                {
                    this.IsVisible = Visibility.Hidden;
                }

            }
        }

        private Visibility _IsVisible;
        public Visibility IsVisible
        {
            get
            {
                return this._IsVisible;
            }
            set
            {
                this._IsVisible = value;
                this.OnPropertyChanged("IsVisible");
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
                    reafreshCompanyVariables();
                }
                else
                {
                    SearchTextChanged();
                }
            }
        }
        #endregion

        #region New Method
        void New()
        {
            this.CurrentCompanyVariable = null;
            this.IsPracentage = false;
            CurrentCompanyVariable = new z_CompanyVariable();
            CurrentCompany = new z_Company();
            CurrentCompanyVariable.company_variableID = Guid.NewGuid();
            reafreshCompanyVariables();
        }
        #endregion

        #region New Button Classs & Property
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
            Guid CompanyID = new Guid();
            foreach (var item in Company)
            {
                CompanyID = item.company_id;
            }

            bool IsUpdate = false;

            z_CompanyVariable newCompanyVariable = new z_CompanyVariable();
            newCompanyVariable.company_variableID = CurrentCompanyVariable.company_variableID;
            newCompanyVariable.company_id = CompanyID;
            newCompanyVariable.variable_Name = CurrentCompanyVariable.variable_Name;
            newCompanyVariable.variable_Value = CurrentCompanyVariable.variable_Value;
            newCompanyVariable.Is_Pracentage = CurrentCompanyVariable.Is_Pracentage;
            newCompanyVariable.Is_Active = CurrentCompanyVariable.Is_Active;
            newCompanyVariable.isSlapCalcution = CurrentCompanyVariable.isSlapCalcution;
            newCompanyVariable.isFlatSlapCalculation = CurrentCompanyVariable.isSlapCalcution;
            newCompanyVariable.isCalculateForBasicSalary = CurrentCompanyVariable.isCalculateForBasicSalary;
            newCompanyVariable.isCalculatewithBasicPlusCompanyRules = CurrentCompanyVariable.isCalculatewithBasicPlusCompanyRules;
            newCompanyVariable.save_user_id = Guid.Empty;
            newCompanyVariable.save_datetime = System.DateTime.Now;
            newCompanyVariable.modified_datetime = System.DateTime.Now;
            newCompanyVariable.modified_user_id = Guid.Empty;
            newCompanyVariable.delete_datetime = System.DateTime.Now;
            newCompanyVariable.delete_user_id = System.Guid.Empty;
            newCompanyVariable.isdelete = false;


            IEnumerable<z_CompanyVariable> allVariables = this.serviceClient.GetCompanyVariables();
            if (allVariables != null)
            {
                foreach (z_CompanyVariable Variable in allVariables)
                {
                    if (Variable.company_variableID == CurrentCompanyVariable.company_variableID)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newCompanyVariable != null && newCompanyVariable.variable_Value != null)
            {
                if (IsUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(509))
                    {
                        newCompanyVariable.modified_user_id = clsSecurity.loggedUser.user_id;
                        newCompanyVariable.modified_datetime = System.DateTime.Now;
                        if (this.serviceClient.UpdateCompanyVariables(newCompanyVariable))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                        reafreshCompanyVariables();
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }
                else
                {
                    try
                    {
                        if (clsSecurity.GetSavePermission(509))
                        {
                            newCompanyVariable.save_user_id = clsSecurity.loggedUser.user_id;
                            newCompanyVariable.save_datetime = System.DateTime.Now;
                            this.serviceClient.SaveCompanyVaribles(newCompanyVariable);
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                            reafreshCompanyVariables();
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Meantion the Variable Name....");
            }
            reafreshCompanyVariables();
            reafreshCompanies();
        }

        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentCompanyVariable != null)
            {
                if (CurrentCompanyVariable.variable_Name == null || CurrentCompanyVariable.variable_Name == String.Empty)
                    return false;
                if (CurrentCompanyVariable.variable_Value == null || CurrentCompanyVariable.variable_Value == 0)
                    return false;
                if (CurrentCompanyVariable.Is_Benifit == null)
                    return false;
                if (CurrentCompanyVariable.Is_Deduct == null)
                    return false;
                if (CurrentCompanyVariable.isCalculateForBasicSalary == null)
                    return false;
                if (CurrentCompanyVariable.isCalculatewithBasicPlusCompanyRules == null)
                    return false;
                if (CurrentCompanyVariable.isSlapCalcution == null)
                    return false;
                if (CurrentCompanyVariable.isFlatSlapCalculation == null)
                    return false;
                //if (CurrentCompanyVariable.Is_Pracentage == null || CurrentCompanyVariable.Is_Pracentage == false)
                //    return false;
                //if (CurrentCompanyVariable.Is_Active == null || CurrentCompanyVariable.Is_Active == false)
                //    return false;
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
            if (clsSecurity.GetDeletePermission(509))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do You Want to Delete This Record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    z_CompanyVariable variable = new z_CompanyVariable();
                    variable.delete_user_id = clsSecurity.loggedUser.user_id;
                    variable.delete_datetime = System.DateTime.Now;
                    variable.company_variableID = CurrentCompanyVariable.company_variableID;

                    if (serviceClient.DeleteCompanyVariabls(variable))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        reafreshCompanyVariables();
                    }
                    else
                    {
                        MessageBox.Show("You Cannot Delete This Rule,Because This Variable to another Operations...");
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
        }
        #endregion

        #region Delete Button Class & Property
        bool deleteCanExecute()
        {
            if (CurrentCompanyVariable == null)
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
        private void reafreshCompanyVariables()
        {
            this.serviceClient.GetCompanyVariablesCompleted += (s, e) =>
                {
                    this.CompanyVariables = e.Result.Where(a => a.isdelete == false);
                };
            this.serviceClient.GetCompanyVariablesAsync();
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

            public void Execute(object parameter)
            {
                _Execute(parameter);
            }


            public event EventHandler CanExecuteChanged;
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

        #region Search Method for all Properties
        public void SearchTextChanged()
        {
            CompanyVariables = CompanyVariables.Where(e => e.variable_Name.ToUpper().Contains(Search.ToUpper()) && e.isdelete == false).ToList();
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
    }
}
