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
    class InstituteGradeViewModel:ViewModelBase
    {

            #region Fileds

        ERPServiceClient serviceClient;

        #endregion

        #region constructor

        public InstituteGradeViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshUniversitesGrade()
        {
            serviceClient.GetUnivercityGradeTypeCompleted += (s, e) =>
            {
                try
                {
                    UniversityGrade = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetUnivercityGradeTypeAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_UniversityGrade> _UniversityGrade;
        public IEnumerable<z_UniversityGrade> UniversityGrade
        {
            get { return _UniversityGrade; }
            set { _UniversityGrade = value; OnPropertyChanged("UniversityGrade"); }
        }

        private z_UniversityGrade _CurrentUniversityGrade;
        public z_UniversityGrade CurrentUniversityGrade
        {
            get { return _CurrentUniversityGrade; }
            set { _CurrentUniversityGrade = value; OnPropertyChanged("CurrentUniversityGrade"); }
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
            CurrentUniversityGrade = null;
            CurrentUniversityGrade = new z_UniversityGrade();
            RefreshUniversitesGrade();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentUniversityGrade.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentUniversityGrade.save_datetime = DateTime.Now;
                _CurrentUniversityGrade.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentUniversityGrade.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateUnivercityDegreeGrade(_CurrentUniversityGrade))
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
            if (_CurrentUniversityGrade == null || _CurrentUniversityGrade.uni_grade_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteUnivercityDegreeGrade(_CurrentUniversityGrade))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentUniversityGrade == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentUniversityGrade.grade))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else
                return true;
        }

        
        #endregion


    }
}
