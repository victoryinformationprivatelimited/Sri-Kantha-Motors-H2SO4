/************************************************************************************************************
 *   Author     :  Heshantha Lakshitha                                                                       *                    
 *   Date       :  19-04-2013                                                                                *             
 *   Purpose    :  Keep the list of Master cities                                                            *                                    
 *   Company    :  Victory Information                                                                       *     
 *   Module     :  ERP System => Masters => Payroll                                                          * 
 *                                                                                                           *     
 ************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ERP.ERPService;
using System.Windows.Input;




namespace ERP
{
    class CityViewModel : ViewModelBase
    {

        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public CityViewModel()
        {

            this.refreshCities();
            New();

        }
        #endregion

        #region Properties
        private IEnumerable<z_City> _Cities;
        public IEnumerable<z_City> Cities
        {
            get
            {
                return this._Cities;
            }
            set
            {
                this._Cities = value;
                this.OnPropertyChanged("Cities");
            }
        }

        private z_City _CurrentCity;
        public z_City CurrentCity
        {
            get
            {
                return this._CurrentCity;
            }
            set
            {
                this._CurrentCity = value;
                this.OnPropertyChanged("Currentcity");
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
                    refreshCities();
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
                return new RelayCommand(save, savCanExecute);
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

        #region New Method
        private void New()
        {
                this.CurrentCity = null;
                CurrentCity = new z_City();
                CurrentCity.city_id = Guid.NewGuid();
                refreshCities();
        }

        bool newCanExecute()
        {
            return true;
        }
        #endregion

        #region Save Method
        private void save()
        {
            bool isUpdate = false;

            z_City newCity = new z_City();
            newCity.city_id = CurrentCity.city_id;
            newCity.city = CurrentCity.city;
            newCity.save_datetime = System.DateTime.Now;
            newCity.save_user_id = clsSecurity.loggedUser.user_id;
            newCity.modified_datetime = System.DateTime.Now;
            newCity.modified_user_id = Guid.Empty;
            newCity.delete_datetime = System.DateTime.Now;
            newCity.delete_user_id = Guid.Empty;
            newCity.isdelete = false;


            IEnumerable<z_City> allcities = this.serviceClient.GetCities();

            if (allcities != null)
            {
                foreach (z_City city in allcities)
                {
                    if (city.city_id == CurrentCity.city_id)
                    {
                        isUpdate = true;
                    }
                }

            }
            if (newCity != null && newCity.city != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(201))
                    {
                        newCity.modified_datetime = System.DateTime.Now;
                        newCity.modified_user_id = clsSecurity.loggedUser.user_id;

                        if (this.serviceClient.UpdateCities(newCity))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
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
                    if (clsSecurity.GetSavePermission(201))
                    {
                        if (this.serviceClient.SaveCites(newCity))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                            New();
                        }
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
                refreshCities();
            }
            else
            {
                clsMessages.setMessage("Please mension city Name  !");
            }
        }

        bool savCanExecute()
        {
            if (CurrentCity != null)
            {
                //   if (CurrentCity.city_id == null || CurrentCity.city_id == Guid.Empty)
                //     return false;
                if (CurrentCity.city == null || CurrentCity.city == string.Empty)
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
            if (clsSecurity.GetDeletePermission(201))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    z_City cit = new z_City();
                    cit.city_id = CurrentCity.city_id;
                    cit.delete_user_id = clsSecurity.loggedUser.user_id;
                    cit.delete_datetime = System.DateTime.Now;
                    if (serviceClient.DeleteCities(cit))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    refreshCities();
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentCity == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Refresh Method
        private void refreshCities()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
            {
                this.Cities = e.Result.Where(a => a.isdelete == false);
            };
            this.serviceClient.GetCitiesAsync();

        }
        #endregion

        #endregion

        #region Grid Data Refresh
        private void searchTextChanged()
        {
            Cities = Cities.Where(e => e.city.ToUpper().Contains(Search.ToUpper())).ToList();
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


        public ICommand NewError
        {
            get
            {
                return new RelayCommand(newerror, newerrorCanExecute);
            }
        }

        private bool newerrorCanExecute()
        {
            return true;
        }

        private void newerror()
        {
            //void IDialogManager.CreateMessageDialog()
            //this.IDialogManager.CreateMessageDialog("Test", "I'm a dialog", DialogMode.YesNo).Show();

        }

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