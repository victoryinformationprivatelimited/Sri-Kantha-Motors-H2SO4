/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-04-24                                                                                                
*   Purpose    : Deductions View Model                                                                                                
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Masters => Payroll                                                        
*                                                                                                                
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
    class DeductionViewModel : ViewModelBase
    {
        #region Sevice Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public DeductionViewModel()
        {
            this.reafreshDeduction();
            New();
        }
        #endregion

        #region Properties {get,set}
        private IEnumerable<mas_Deduction> _Deductions;
        public IEnumerable<mas_Deduction> Deductions
        {
            get
            {
                return this._Deductions;
            }
            set
            {
                this._Deductions = value;
                this.OnPropertyChanged("Deductions");

            }
        }

        private mas_Deduction _CurrentDeduction;
        public mas_Deduction CurrentDeduction
        {
            get
            {
                return this._CurrentDeduction;
            }
            set
            {
                this._CurrentDeduction = value;
                this.OnPropertyChanged("CurrentDeduction");

            }
        }

        private IEnumerable<mas_Deduction> _IsActive;
        public IEnumerable<mas_Deduction> IsActive
        {
            get
            {
                return this._IsActive;
            }
            set
            {
                this._IsActive = value;
                this.OnPropertyChanged("IsActive");
            }
        }

        private mas_Benifit _CurrentIsActive;
        public mas_Benifit CurrentIsActive
        {
            get
            {
                return this._CurrentIsActive;
            }
            set
            {
                this._CurrentIsActive = value;
                this.OnPropertyChanged("CurrentIsActive");

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
                    ReafreshDeductionResult();
                }
                else
                {
                    SearchTextChanged();
                }

            }
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

        #region New Method
        void New()
        {
                this.Deductions = null;
                CurrentDeduction = new mas_Deduction();
                CurrentDeduction.deduction_id = Guid.NewGuid();
                reafreshDeduction();
        }
        #endregion

        #region New Button Class & Property
        bool newCanExecutive()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecutive);
            }
        }
        #endregion

        #region Save Method
        void Save()
        {
            bool isUpdate = false;
            mas_Deduction newDeduction = new mas_Deduction();
            newDeduction.deduction_id = CurrentDeduction.deduction_id;
            newDeduction.deduction_name = CurrentDeduction.deduction_name;
            newDeduction.isActive = CurrentDeduction.isActive;
            newDeduction.save_datetime = System.DateTime.Now;
            newDeduction.save_user_id = clsSecurity.loggedUser.user_id;
            newDeduction.modified_datetime = System.DateTime.Now;
            newDeduction.modified_user_id = clsSecurity.loggedUser.user_id;
            newDeduction.delete_datetime = System.DateTime.Now;
            newDeduction.delete_user_id = clsSecurity.loggedUser.user_id;

            IEnumerable<mas_Deduction> alldeductions = this.serviceClient.GetDeductions();
            if (alldeductions != null)
            {
                foreach (mas_Deduction Deduction in alldeductions)
                {
                    if (Deduction.deduction_id == CurrentDeduction.deduction_id)
                    {
                        isUpdate = true;
                    }
                }
            }

            if (newDeduction != null && newDeduction.deduction_name != null)
            {

                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(502))
                    {
                        newDeduction.modified_datetime = System.DateTime.Now;
                        newDeduction.modified_user_id = clsSecurity.loggedUser.user_id;
                        this.serviceClient.UpdateDeductions(newDeduction);
                        clsMessages.setMessage(Properties.Resources.UpdateSucess);
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }
                else
                {

                    if (clsSecurity.GetSavePermission(502))
                    {
                        this.serviceClient.SaveDeductions(newDeduction);
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                }
            }
            else
            {
                MessageBox.Show("Please Meantion the Deduction Name...!");
            }
            reafreshDeduction();
        }

        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentDeduction != null)
            {
                if (CurrentDeduction.deduction_id == null || CurrentDeduction.deduction_id == Guid.Empty)
                    return false;
                if (CurrentDeduction.deduction_name == null || CurrentDeduction.deduction_name == string.Empty)
                    return false;
                if (CurrentDeduction.isActive == null)
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
            if (clsSecurity.GetDeletePermission(502))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do you want to delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    CurrentDeduction.delete_datetime = System.DateTime.Now;
                    CurrentDeduction.delete_user_id = clsSecurity.loggedUser.user_id;
                    if (serviceClient.DeleteDeductions(CurrentDeduction))
                    {
                        reafreshDeduction();
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
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
            if (CurrentDeduction == null)
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

        public void reafreshDeduction()
        {
            this.serviceClient.GetDeductionsCompleted += (s, e) =>
                {
                    this.Deductions = e.Result.Where(c => c.isdelete == false);
                    Deductions = Deductions.Where(z => z.deduction_id != new Guid() && z.isdelete == false);
                };
            this.serviceClient.GetDeductionsAsync();

        }

        public void ReafreshDeductionResult()
        {
            this.serviceClient.GetSearchDeductionsCompleted += (s, e) =>
                {
                    this.Deductions = (IEnumerable<mas_Deduction>)this.serviceClient.GetSearchDeductions(Search).Where(c => c.isdelete == false);
                    Deductions = Deductions.Where(z => z.deduction_id != new Guid() && z.isdelete == false);
                };
            this.serviceClient.GetSearchDeductionsAsync(Search);
        }
        #endregion

        #region Search Method for all Properties
        public void SearchTextChanged()
        {

            Deductions = Deductions.Where(e => e.deduction_name.ToUpper().Contains(Search.ToUpper())).ToList();

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
