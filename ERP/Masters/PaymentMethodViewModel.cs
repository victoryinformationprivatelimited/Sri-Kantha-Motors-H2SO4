/************************************************************************************************************
 *   Author     :  Heshantha Lakshitha                                                                       *                    
 *   Date       :  08-05-2013                                                                                *             
 *   Purpose    :  Keep the list of Master Payment Method                                                            *                                    
 *   Company    :  Victory Information                                                                       *     
 *   Module     :  ERP System => Masters => Payroll                                                          *                                                                                              *     
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
    public class PaymentMethodViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public PaymentMethodViewModel()
        {
            this.refreshPaymentMethods();
            this.New();
        }
        #endregion

        #region Properties
        private IEnumerable<z_PaymentMethod> _PaymentMethods;
        public IEnumerable<z_PaymentMethod> PaymentMethods
        {
            get
            {
                return this._PaymentMethods;
            }
            set
            {
                this._PaymentMethods = value;
                this.OnPropertyChanged("PaymentMethods");
            }
        }

        private z_PaymentMethod _CurrentPaymentMethod;
        public z_PaymentMethod CurrentPaymentMethod
        {
            get
            {
                return this._CurrentPaymentMethod;
            }
            set
            {
                this._CurrentPaymentMethod = value;
                this.OnPropertyChanged("CurrentPaymentMethod");
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
                    refreshPaymentMethods();
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
        //private SaveButton _SavePaymentMethods;
        //public SaveButton SavePaymentMethods
        //{
        //    get
        //    {
        //        return _SavePaymentMethods;
        //    }
        //}

        //private NewButton _NewPaymentMethods;
        //public NewButton NewPaymentMethods
        //{
        //    get
        //    {
        //        return _NewPaymentMethods;
        //    }
        //}

        //private DeleteButton _DeletePaymentMethods;
        //public DeleteButton DeletePaymentMethods
        //{
        //    get
        //    {
        //        return _DeletePaymentMethods;
        //    }
        //}
        //#endregion


        //public class SaveButton : ICommand
        //{
        //    private PaymentMethodViewModel View;

        //    public SaveButton(PaymentMethodViewModel View)
        //    {
        //        this.View = View;
        //        this.View.PropertyChanged += (s, e) =>
        //            {
        //                if (this.CanExecuteChanged != null)
        //                {
        //                    this.CanExecuteChanged(this, new EventArgs());
        //                }
        //            };
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        if (this.View.CurrentPaymentMethod != null)
        //        {
        //            return this.View.CurrentPaymentMethod != null;
        //        }
        //        return false;
        //    }

        //    public event EventHandler CanExecuteChanged;

        //    public void Execute(object parameter)
        //    {
        //        this.View.Save();
        //        this.View.New();
        //    }
        //}

        //public class NewButton : ICommand
        //{
        //    private PaymentMethodViewModel View;

        //    public NewButton(PaymentMethodViewModel View)
        //    {
        //        this.View = View;
        //        this.View.PropertyChanged += (s, e) =>
        //            {
        //                if (this.CanExecuteChanged != null)
        //                {
        //                    this.CanExecuteChanged(this, new EventArgs());
        //                }
        //            };
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return true;
        //    }

        //    public event EventHandler CanExecuteChanged;

        //    public void Execute(object parameter)
        //    {
        //        this.View.New();
        //    }
        //}

        //public class DeleteButton : ICommand
        //{
        //    private PaymentMethodViewModel View;

        //    public DeleteButton(PaymentMethodViewModel View)
        //    {
        //        this.View = View;
        //        this.View.PropertyChanged += (s, e) =>
        //            {
        //                if (this.CanExecuteChanged != null)
        //                {
        //                    this.CanExecuteChanged(this, new EventArgs());
        //                }
        //            };
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return this.View.CurrentPaymentMethod != null;
        //    }

        //    public event EventHandler CanExecuteChanged;

        //    public void Execute(object parameter)
        //    {
        //        this.View.delete();
        //        this.View.New();
        //    }
        //} 
        #endregion
        #endregion

        #region New Method
        private void New()
        {
            this.CurrentPaymentMethod = null;
            CurrentPaymentMethod = new z_PaymentMethod();
            CurrentPaymentMethod.paymet_method_id = Guid.NewGuid();
            refreshPaymentMethods();
        }

        bool newCanExecute()
        {
            return true;
        }
        #endregion

        #region Save Method
        private void Save()
        {
            bool isUpdate = false;

            z_PaymentMethod newPaymentMethod = new z_PaymentMethod();
            newPaymentMethod.paymet_method_id = CurrentPaymentMethod.paymet_method_id;
            newPaymentMethod.payment_method = CurrentPaymentMethod.payment_method;

            IEnumerable<z_PaymentMethod> allPaymentMethods = this.serviceClient.GetPaymentMethods();

            if (allPaymentMethods != null)
            {
                foreach (z_PaymentMethod paymentmethod in allPaymentMethods)
                {
                    if (paymentmethod.paymet_method_id == CurrentPaymentMethod.paymet_method_id)
                    {
                        isUpdate = true;
                    }
                }
            }
            if (newPaymentMethod != null && newPaymentMethod.payment_method != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(213))
                    {
                        if (this.serviceClient.UpdatePaymentMethods(newPaymentMethod))
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
                    if (clsSecurity.GetSavePermission(213))
                    {
                        if (this.serviceClient.SavePaymentMethods(newPaymentMethod))
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

                refreshPaymentMethods();
            }
            else
            {
                clsMessages.setMessage("Please mension PaymentMethod  !");
            }
        }

        bool saveCanExecute()
        {
            if (CurrentPaymentMethod != null)
            {
                if (CurrentPaymentMethod.paymet_method_id == null || CurrentPaymentMethod.paymet_method_id == Guid.Empty)
                    return false;

                if (CurrentPaymentMethod.payment_method == null || CurrentPaymentMethod.payment_method == string.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Delete Method
        private void delete()
        {
            if (clsSecurity.GetDeletePermission(213))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this recod ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeletePaymentMethods(CurrentPaymentMethod))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    refreshPaymentMethods();
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentPaymentMethod == null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Refresh Method
        private void refreshPaymentMethods()
        {
            this.serviceClient.GetPaymentMethodsCompleted += (s, e) =>
                {
                    this.PaymentMethods = e.Result;
                };
            this.serviceClient.GetPaymentMethodsAsync();
        }
        #endregion

        #region Grid Data Refresh
        private void searchTextChanged()
        {
            PaymentMethods = PaymentMethods.Where(e => e.payment_method.ToUpper().Contains(Search.ToUpper())).ToList();

        }
        #endregion

        #region Search key
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
    }
}
