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
    class SchoolGradeViewModel :ViewModelBase
    {

        #region Fileds

        ERPServiceClient serviceClient;
        List<z_SchoolGrade> AllSchoolGrade;

        #endregion

        #region constructor

        public SchoolGradeViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllSchoolGrade = new List<z_SchoolGrade>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshSchoolGradeViewModel()
        {
            serviceClient.GetSchoolGradesCompleted += (s, e) =>
            {
                try
                {
                    SchoolGrade = e.Result;
                    if (SchoolGrade != null)
                        AllSchoolGrade = SchoolGrade.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetSchoolGradesAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_SchoolGrade> _SchoolGrade;
        public IEnumerable<z_SchoolGrade> SchoolGrade
        {
            get { return _SchoolGrade; }
            set { _SchoolGrade = value; OnPropertyChanged("SchoolGrade"); }
        }

        private z_SchoolGrade _CurrentSchoolGrade;
        public z_SchoolGrade CurrentSchoolGrade
        {
            get { return _CurrentSchoolGrade; }
            set { _CurrentSchoolGrade = value; OnPropertyChanged("CurrentSchoolGrade"); }
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
            CurrentSchoolGrade = null;
            CurrentSchoolGrade = new z_SchoolGrade();
            RefreshSchoolGradeViewModel();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentSchoolGrade.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentSchoolGrade.save_datetime = DateTime.Now;
                _CurrentSchoolGrade.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentSchoolGrade.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateSchoolGrade(_CurrentSchoolGrade))
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
            if (_CurrentSchoolGrade == null || _CurrentSchoolGrade.school_grade_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteSchoolGrade(_CurrentSchoolGrade))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentSchoolGrade == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentSchoolGrade.school_grade_type))
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
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (SchoolGrade != null)searchTextChanged(); }
        }

        private void searchTextChanged()
        {
            SchoolGrade = null;
            SchoolGrade = AllSchoolGrade.Where(e => e.school_grade_type.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
