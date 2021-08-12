using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ERP.Masters
{
    class LocationMasterViewModel : ViewModelBase
    {
        #region Service

        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public LocationMasterViewModel()
        {
            this.RefreshLocations();
            this.New();
        }

        #endregion

        #region Properties

        private IEnumerable<z_Location> _Locations;

        public IEnumerable<z_Location> Locations
        {
            get
            {
                return this._Locations;
            }
            set
            {
                this._Locations = value;
                this.OnPropertyChanged("Locations");
            }
        }


        private z_Location _CurrentLocation;
        public z_Location CurrentLocation
        {
            get
            {
                return this._CurrentLocation;
            }
            set
            {
                this._CurrentLocation = value;
                this.OnPropertyChanged("CurrentLocation");
            }
        }

        #endregion

        #region Button Commands
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
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }


        #endregion

        #region Refresh Methodes

        private void RefreshLocations()
        {
            this.serviceClient.GetLocationCompleted += (s, e) =>
            {
                this.Locations = e.Result.Where(c => c.is_delete == false);
            };
            this.serviceClient.GetLocationAsync();
        }
        #endregion

        #region Methods

        #region Save

        private void Save()
        {
            bool isUpdate = false;

            z_Location newLocation = new z_Location();
            newLocation.location_id = CurrentLocation.location_id;
            newLocation.loc_code = CurrentLocation.loc_code;
            newLocation.location_name = CurrentLocation.location_name;
            newLocation.address = CurrentLocation.address;
            newLocation.Tel = CurrentLocation.Tel;
            newLocation.is_active = true;
            newLocation.is_delete = false;
            newLocation.latitude = CurrentLocation.latitude;
            newLocation.longtitude = CurrentLocation.longtitude;
            newLocation.save_datetime = System.DateTime.Now;

            IEnumerable<z_Location> allLocations = this.serviceClient.GetLocation();

            if (allLocations != null)
            {
                foreach (var item in allLocations)
                {
                    if (item.location_id == CurrentLocation.location_id)
                        isUpdate = true;
                }
            }

            if (newLocation != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(215))
                    {
                        newLocation.modified_datetime = System.DateTime.Now;

                        if (this.serviceClient.UpdateLocation(newLocation))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            New();
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }

                    }
                }
                else
                {
                    if (clsSecurity.GetSavePermission(215))
                    {
                        if (Locations.FirstOrDefault(c => c.location_id == CurrentLocation.location_id) == null)
                        {
                            newLocation.save_datetime = System.DateTime.Now;
                            if (this.serviceClient.SaveLocation(newLocation))
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
                            clsMessages.setMessage("Location Already Exists, Please Enter Different location");
                    }
                    else
                    {
                        clsMessages.setMessage(" You don't have permission to save this record(s)");
                    }
                }
                RefreshLocations();
            }
            else
            {
                clsMessages.setMessage(" Please Enter Location !");
            }
        }

        bool saveCanExecute()
        {
            if (CurrentLocation != null)
            {
                if (CurrentLocation.location_id == null)
                    return false;
                if (CurrentLocation.location_name == null || CurrentLocation.location_name == string.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Delete

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(215))
            {
                clsMessages.setMessage("Do you want to Delete this record? ", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    z_Location loc = new z_Location();
                    loc.location_id = CurrentLocation.location_id;
                    loc.delete_datetime = System.DateTime.Now;

                    if (serviceClient.DeleteLocation(loc))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    RefreshLocations();
                }
            }
            else
            {
                clsMessages.setMessage("You dont have permission to Delete this records");
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentLocation == null)
                return false;

            return true;
        }
        #endregion

        #endregion

        #region New Methode
        private void New()
        {
            this.CurrentLocation = null;
            CurrentLocation = new z_Location();
            RefreshLocations();
        }

        bool newCanExecute()
        {
            return true;
        }

        #endregion


    }
}
