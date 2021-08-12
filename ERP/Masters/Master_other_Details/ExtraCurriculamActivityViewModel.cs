using ERP.AdditionalWindows;
using ERP.Attendance.Master_Details;
using ERP.ERPService;
using ERP.Masters;
using ERP.Masters.Master_other_Details;
using ERP.Medical;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ERP.Masters.Master_other_Details
{
    class ExtraCurriculamActivityViewModel:ViewModelBase
    {
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_ExtraCurricularActivities> AllExtraCurricularActivities;

        #endregion

        #region constructor

        public ExtraCurriculamActivityViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllExtraCurricularActivities = new List<z_ExtraCurricularActivities>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshExtraCurriculamActivity()
        {
            serviceClient.GetExtraCurricularActivitiesCompleted += (s, e) =>
            {
                try
                {
                    ExtraCurricularActivities = e.Result;
                    if (ExtraCurricularActivities != null)
                        AllExtraCurricularActivities = ExtraCurricularActivities.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetExtraCurricularActivitiesAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_ExtraCurricularActivities> _ExtraCurricularActivities;
        public IEnumerable<z_ExtraCurricularActivities> ExtraCurricularActivities
        {
            get { return _ExtraCurricularActivities; }
            set { _ExtraCurricularActivities = value; OnPropertyChanged("ExtraCurricularActivities"); }
        }

        private z_ExtraCurricularActivities _CurrentExtraCurricularActivities;
        public z_ExtraCurricularActivities CurrentExtraCurricularActivities
        {
            get { return _CurrentExtraCurricularActivities; }
            set { _CurrentExtraCurricularActivities = value; OnPropertyChanged("CurrentExtraCurricularActivities"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
           // _Universites = null;
            CurrentExtraCurricularActivities = null;
            CurrentExtraCurricularActivities = new z_ExtraCurricularActivities();
            RefreshExtraCurriculamActivity();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentExtraCurricularActivities.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentExtraCurricularActivities.save_datetime = DateTime.Now;
                _CurrentExtraCurricularActivities.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentExtraCurricularActivities.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateExtraCurricularActivites(_CurrentExtraCurricularActivities))
                    clsMessages.setMessage("Record Saved Successfully");
                else
                    clsMessages.setMessage("Record Save Failed");

                New();

            }
        }

        public ICommand DeleteBtn 
        {
            get { return new RelayCommand(Delete,DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (_CurrentExtraCurricularActivities == null || _CurrentExtraCurricularActivities.activities_category_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteExtraCurricularActivite(_CurrentExtraCurricularActivities))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentExtraCurricularActivities == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentExtraCurricularActivities.activities_category_type))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else
                return true;
        }

        #region Search TextBox

        private string _Search;
        public string Search
        {
            get
            {
                return _Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (_Search != null)
                    searchTextChanged();
            }
        }

        private void searchTextChanged()
        {
            ExtraCurricularActivities = null;
            ExtraCurricularActivities = AllExtraCurricularActivities.Where(e => e.activities_category_type.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
