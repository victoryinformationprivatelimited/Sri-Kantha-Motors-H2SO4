/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-04-24                                                                                                
*   Purpose    : Benifits View Model                                                                                                
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
    class BenifitsViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public BenifitsViewModel()
        {
            this.reafreshBenifits();
            this.New();
        }
        #endregion

        #region Properties {get,set}
        private IEnumerable<mas_Benifit> _Benifits;
        public IEnumerable<mas_Benifit> Benifits
        {
            get
            {
                return this._Benifits;
            }
            set
            {
                this._Benifits = value;
                this.OnPropertyChanged("Benifits");
            }
        }

        private mas_Benifit _CurrentBenifit;
        public mas_Benifit CurrentBenifit
        {
            get
            {
                return this._CurrentBenifit;
            }
            set
            {
                this._CurrentBenifit = value;
                this.OnPropertyChanged("CurrentBenifit");

            }
        }

        private IEnumerable<mas_Benifit> _IsActive;
        public IEnumerable<mas_Benifit> IsActive
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
                    ReafreshBenifitResults();
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
                this.CurrentBenifit = null;
                CurrentBenifit = new mas_Benifit();
                CurrentBenifit.benifit_id = Guid.NewGuid();
                reafreshBenifits();
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
            mas_Benifit newBenifits = new mas_Benifit();
            newBenifits.benifit_id = CurrentBenifit.benifit_id;
            newBenifits.benifit_name = CurrentBenifit.benifit_name;
            newBenifits.save_datetime = System.DateTime.Now;
            newBenifits.save_user_id = clsSecurity.loggedUser.user_id;
            newBenifits.modified_datetime = System.DateTime.Now;
            newBenifits.modified_user_id = clsSecurity.loggedUser.user_id;
            newBenifits.delete_datetime = System.DateTime.Now;
            newBenifits.delete_user_id = clsSecurity.loggedUser.user_id;
            newBenifits.isActive = CurrentBenifit.isActive;

            IEnumerable<mas_Benifit> allbenifits = this.serviceClient.GetBenifits();
            if (allbenifits != null)
            {
                foreach (mas_Benifit Benifits in allbenifits)
                {
                    if (Benifits.benifit_id == CurrentBenifit.benifit_id)
                    {
                        isUpdate = true;
                    }
                }
            }

            if (newBenifits != null && newBenifits.benifit_name != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(501))
                    {
                        this.serviceClient.UpdateBenifits(newBenifits);
                        clsMessages.setMessage(Properties.Resources.UpdateSucess);
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }
                else
                {
                    if (clsSecurity.GetSavePermission(501))
                    {
                        this.serviceClient.SaveBenifits(newBenifits);
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                }
            }
            else
            {
                MessageBox.Show("Please Mention the Benefit Name...!");
            }
            reafreshBenifits();
        }

        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentBenifit != null)
            {
                if (CurrentBenifit.benifit_id == null || CurrentBenifit.benifit_id == Guid.Empty)
                    return false;
                if (CurrentBenifit.benifit_name == null || CurrentBenifit.benifit_name == string.Empty)
                    return false;
                if (CurrentBenifit.isActive == null)
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
            if (clsSecurity.GetDeletePermission(501))
            {
                CurrentBenifit.delete_datetime = System.DateTime.Now;
                CurrentBenifit.delete_user_id = clsSecurity.loggedUser.user_id;
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do you want to delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteBenifits(CurrentBenifit))
                    {
                        reafreshBenifits();
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
        bool deleteCanExecutive()
        {
            if (CurrentBenifit == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecutive);
            }
        }
        #endregion

        #region User Define Methods
        private void reafreshBenifits()
        {
            this.serviceClient.GetBenifitsCompleted += (s, e) =>
                {
                    this.Benifits = e.Result.Where(c => c.isdelete == false);
                    Benifits = Benifits.Where(z => z.benifit_id != new Guid());
                };
            this.serviceClient.GetBenifitsAsync();
        }

        public void ReafreshBenifitResults()
        {
            this.serviceClient.GetSearchBenifitsCompleted += (s, e) =>
                {
                    this.Benifits = (IEnumerable<mas_Benifit>)this.serviceClient.GetSearchBenifits(Search).Where(c => c.isdelete == false);
                    Benifits = Benifits.Where(z => z.benifit_id != new Guid());
                };
            this.serviceClient.GetSearchBenifitsAsync(Search);
        }
        #endregion

        #region Search Method for all Properties
        public void SearchTextChanged()
        {
            Benifits = Benifits.Where(e => e.benifit_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        #region Is Active Validation Method
        private bool validation()
        {
            string Message = "ERP System says..! please mention \n";
            bool isValidate = true;

            if (CurrentBenifit.isActive == null)
            {
                Message += "Method\n";
                isValidate = false;
            }
            if (!isValidate)
            {
                MessageBox.Show(Message, "ERP", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return isValidate;

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
