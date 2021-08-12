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
    class ProfessionalQualificationCatagoryTypeViewModel : ViewModelBase
    {



        #region Fileds

        ERPServiceClient serviceClient;
        List<z_UnivercityDigreeType> AllUnivercityDigreeType;

        #endregion

        #region constructor

        public ProfessionalQualificationCatagoryTypeViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllUnivercityDigreeType = new List<z_UnivercityDigreeType>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshProfessionalQualificationCatagoryType()
        {
            serviceClient.GetUnivercityDegreeTypeCompleted += (s, e) =>
            {
                try
                {
                    UnivercityDigreeType = e.Result;
                    if (UnivercityDigreeType != null)
                        AllUnivercityDigreeType = UnivercityDigreeType.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetUnivercityDegreeTypeAsync();
        }

        private void RefreshUnivercity()
        {
            serviceClient.GetUnivercityNameCompleted += (s, e) =>
            {
                try
                {
                    Universities = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetUnivercityNameAsync();

        }

        #endregion

        #region Properties

        private IEnumerable<z_UnivercityName> _Universities;
        public IEnumerable<z_UnivercityName> Universities
        {
            get { return _Universities; }
            set { _Universities = value; OnPropertyChanged("Universities"); }
        }


        private IEnumerable<z_UnivercityDigreeType> _UnivercityDigreeType;
        public IEnumerable<z_UnivercityDigreeType> UnivercityDigreeType
        {
            get { return _UnivercityDigreeType; }
            set { _UnivercityDigreeType = value; OnPropertyChanged("UnivercityDigreeType"); }
        }

        private z_UnivercityDigreeType _CurrentLifeUnivercityDigreeType;
        public z_UnivercityDigreeType CurrentLifeUnivercityDigreeType
        {
            get { return _CurrentLifeUnivercityDigreeType; }
            set { _CurrentLifeUnivercityDigreeType = value; OnPropertyChanged("CurrentLifeUnivercityDigreeType"); }
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
            CurrentLifeUnivercityDigreeType = null;
            CurrentLifeUnivercityDigreeType = new z_UnivercityDigreeType();
            RefreshUnivercity();
            RefreshProfessionalQualificationCatagoryType();
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave())
            {
                _CurrentLifeUnivercityDigreeType.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentLifeUnivercityDigreeType.save_datetime = DateTime.Now;
                _CurrentLifeUnivercityDigreeType.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentLifeUnivercityDigreeType.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateUnivercityDegreeType(_CurrentLifeUnivercityDigreeType))
                    clsMessages.setMessage("Record Saved Successfully");
                else
                    clsMessages.setMessage("Record Save Failed");

                New();

            }
        }

        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (_CurrentLifeUnivercityDigreeType == null || _CurrentLifeUnivercityDigreeType.univercity_Course_type_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteUnivercityDegreeType(_CurrentLifeUnivercityDigreeType))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentLifeUnivercityDigreeType == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentLifeUnivercityDigreeType.univercity_Course_type))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else if (_CurrentLifeUnivercityDigreeType.univercity_id == null || _CurrentLifeUnivercityDigreeType.univercity_id == 0)
            {

                clsMessages.setMessage("Please Select an Institute name!");
                return false;

            }

            else
                return true;
        }

        #region Search TextBox

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (UnivercityDigreeType != null) searchTextChanged(); }
        }
        private void searchTextChanged()
        {
            UnivercityDigreeType = null;
            UnivercityDigreeType = AllUnivercityDigreeType.Where(e => e.univercity_Course_type.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        #endregion

    }
}
