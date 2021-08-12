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
    class TownMasterViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public TownMasterViewModel()
        {
            this.reafreshCities();
            this.reafreshTownMasterView();
            this.New();
        }
        #endregion

        #region Properties
        private IEnumerable<TownMasterView> _TownsMasterView;
        public IEnumerable<TownMasterView> TownMasterView
        {
            get
            {
                return this._TownsMasterView;
            }
            set
            {
                this._TownsMasterView = value;
                this.OnPropertyChanged("TownMasterView");
            }
        }

        private TownMasterView _CurrentTownView;
        public TownMasterView CurrentTownView
        {
            get
            {
                return this._CurrentTownView;
            }
            set
            {
                this._CurrentTownView = value;
                this.OnPropertyChanged("CurrentTownView");

            }
        }

        private IEnumerable<z_Town> _Towns;
        public IEnumerable<z_Town> Towns
        {
            get
            {
                return this._Towns;
            }
            set
            {
                this._Towns = value;
                this.OnPropertyChanged("Towns");
            }
        }

        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get
            {
                return this._CurrentTown;
            }
            set
            {
                this._CurrentTown = value;
                this.OnPropertyChanged("CurrentTown");
            }
        }

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
                this.OnPropertyChanged("CurrentCity");
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
                    this.reafreshTownMasterView();
                }
                else
                {
                    SearchTextChanged();
                }

            }
        }
        #endregion

        #region New Method
        private void New()
        {
                CurrentTown = null;
                CurrentTown = new z_Town();
                CurrentTown.town_id = Guid.NewGuid();
                CurrentTownView = null;
                CurrentTownView = new TownMasterView();
                CurrentTownView.town_id = Guid.NewGuid();
                CurrentCity = null;
                CurrentCity = new z_City();
        }

        #endregion

        #region NewButton Class & Property
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
            try
            {
                bool Update = false;
                z_Town newTown = new z_Town();
                newTown.town_id = CurrentTownView.town_id;
                newTown.city_id = CurrentTownView.city_id;
                newTown.town_name = CurrentTownView.town_name;
                newTown.save_user_id = clsSecurity.loggedUser.user_id;
                newTown.save_datetime = System.DateTime.Now;
                newTown.modified_user_id = clsSecurity.loggedUser.user_id;
                newTown.modified_datetime = System.DateTime.Now;
                newTown.isdelete = false;
                IEnumerable<z_Town> alltowns = this.serviceClient.GetTowns();

                if (alltowns != null)
                {
                    foreach (var Towns in alltowns)
                    {
                        if (Towns.town_id == CurrentTownView.town_id)
                        {
                            Update = true;
                        }
                    }
                }
                if (newTown != null && newTown.town_id != null)
                {
                    if (Update)
                    {
                        if (clsSecurity.GetUpdatePermission(202))
                        {
                            if (this.serviceClient.UpdateTown(newTown))
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
                        if (clsSecurity.GetSavePermission(202))
                        {
                            if (this.serviceClient.SaveTown(newTown))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
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
                }
                reafreshTownMasterView();
                this.New();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region SaveButton Class & Properties
        bool saveCanExecute()
        {
            if (CurrentTownView != null)
            {
                if (CurrentTownView.town_id == null || CurrentTownView.town_id == Guid.Empty)
                    return false;
                if (CurrentTownView.city_id == null || CurrentTownView.city_id == Guid.Empty)
                    return false;
                if (CurrentTownView.town_name == null || CurrentTownView.town_name == string.Empty)
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
            try
            {
                if (clsSecurity.GetDeletePermission(202))
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result = MessageBox.Show("Do You Want To Delete This Record...?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        z_Town town = new z_Town();
                        town.town_id = CurrentTownView.town_id;
                        town.city_id = CurrentTownView.city_id;
                        town.delete_datetime = System.DateTime.Now;
                        town.delete_user_id = clsSecurity.loggedUser.user_id;

                        if (this.serviceClient.DeleteTown(town))
                        {
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            reafreshTownMasterView();
                            this.New();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region DeleteButton & Property
        bool deleteCanExecutive()
        {
            if (CurrentTownView == null)
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

        #region List of Cities
        private void reafreshCities()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
                {
                    this.Cities = e.Result.Where(z => z.isdelete == false);
                };
            this.serviceClient.GetCitiesAsync();
        }
        #endregion

        #region List of Town View List
        private void reafreshTownMasterView()
        {
            this.serviceClient.GetTownMasterViewCompleted += (s, e) =>
                {
                    this.TownMasterView = e.Result;
                };
            this.serviceClient.GetTownMasterViewAsync();
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
            TownMasterView = TownMasterView.Where(e => e.town_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion

    }
}
