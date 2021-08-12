/************************************************************************************************************
 *   Author     :  Heshantha Lakshitha                                                                       *                    
 *   Date       :  07-05-2013                                                                                *             
 *   Purpose    :  Keep the list of Master Bank                                                           *                                    
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
    class BankViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public BankViewModel()
        {

            this.RefreshBanks();
            New();
        }
        #endregion

        #region Properties
        private IEnumerable<z_Bank> _Banks;
        public IEnumerable<z_Bank> Banks
        {
            get
            {
                return this._Banks;
            }
            set
            {
                this._Banks = value;
                this.OnPropertyChanged("Banks");
            }
        }

        private z_Bank _CurrentBank;
        public z_Bank CurrentBank
        {
            get
            {
                return this._CurrentBank;
            }
            set
            {
                this._CurrentBank = value;
                this.OnPropertyChanged("CurrentBank");
                if (CurrentBank != null)
                    BankName = CurrentBank.bank_name;

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
                    RefreshBanks();
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
        #endregion

        #region UserDefined Method

        #region Refresh Method
        private void RefreshBanks()
        {
            this.serviceClient.GetBanksCompleted += (s, e) =>
                {
                    this.Banks = e.Result.Where(a => a.isdelete == false);
                };
            this.serviceClient.GetBanksAsync();
        }
        #endregion

        #region save Method
        void Save()
        {
            bool isUpdate = false;

            z_Bank newbank = new z_Bank();
            newbank.bank_id = CurrentBank.bank_id;
            newbank.bank_code = CurrentBank.bank_code;
            newbank.bank_name = CurrentBank.bank_name;
            newbank.save_datetime = System.DateTime.Now;
            newbank.save_user_id = clsSecurity.loggedUser.user_id;
            newbank.modified_datetime = System.DateTime.Now;
            newbank.modified_user_id = Guid.Empty;
            newbank.delete_datetime = System.DateTime.Now;
            newbank.delete_user_id = Guid.Empty;

            IEnumerable<z_Bank> allbanks = this.serviceClient.GetBanks();

            if (allbanks != null)
            {
                foreach (z_Bank bank in allbanks)
                {
                    if (bank.bank_id == CurrentBank.bank_id)
                    {
                        isUpdate = true;
                    }
                }
            }
            if (newbank != null && newbank != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(210))
                    {
                        newbank.modified_datetime = System.DateTime.Now;
                        newbank.modified_user_id = clsSecurity.loggedUser.user_id;

                        if (this.serviceClient.UpdateBanks(newbank))
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }
                    }
                    else
                    {

                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
                else
                {
                    if (clsSecurity.GetSavePermission(210))
                    {
                        if (this.serviceClient.Savebanks(newbank))
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
                RefreshBanks();
            }
            else
            {
                clsMessages.setMessage("Please mention Destination Name  !");
            }
        }

        private string _Color;
        public string Color
        {
            get { return _Color; }
            set { _Color = value; OnPropertyChanged("Color"); }
        }


        bool saveCanExecute()
        {


            if (CurrentBank != null)
            {
                if (CurrentBank.bank_id == null || CurrentBank.bank_id == Guid.Empty)
                    return false;

                if (CurrentBank.bank_name == null || CurrentBank.bank_name == string.Empty)
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
        void New()
        {
                this.CurrentBank = null;
                CurrentBank = new z_Bank();
                CurrentBank.bank_id = Guid.NewGuid();
                RefreshBanks();
        }

        bool newCanExecute()
        {
            return true;
        }


        #endregion

        #region Delete Method
        void delete()
        {
            if (clsSecurity.GetDeletePermission(210))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    z_Bank bank = new z_Bank();
                    bank.bank_id = CurrentBank.bank_id;
                    bank.delete_user_id = clsSecurity.loggedUser.user_id;
                    bank.delete_datetime = System.DateTime.Now;
                    bank.isdelete = true;

                    if (serviceClient.DeleteBanks(bank))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    RefreshBanks();
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentBank == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #endregion

        #region Grid Data Refresh
        private void searchTextChanged()
        {
            Banks = Banks.Where(e => e.bank_name.ToUpper().Contains(Search.ToUpper())).ToList();

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

        #region search Comand Exicute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }
        #endregion

        #region Error
        #region Validation Properties
        private String _BankName;
        public String BankName
        {
            get { return _BankName; }
            set
            {
                _BankName = value; OnPropertyChanged("BankName");
                CurrentBank.bank_name = BankName;
            }
        }
        #endregion

        #region Error Interface
        //public string Error
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //string IDataErrorInfo.this[string PropertyName]
        //{
        //    get
        //    {
        //        return getValidationError(PropertyName);
        //    }
        //}
        #endregion

        public override string getValidationError(string PropertyName)
        {

            String error = null;
            switch (PropertyName)
            {
                case "BankName":
                    error = validateBankName();
                    break;
            }
            return error;
        }
        //_______________________________________________________________________
        private string validateBankName()
        {
            if (String.IsNullOrWhiteSpace(BankName))
            {
                return "Cannot Be Empty";
            }
            return null;
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


