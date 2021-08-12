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
    class SkillTypeViewModel:ViewModelBase
    {
        
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_SkillType> AllSkillType;

        #endregion

        #region constructor

        public SkillTypeViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllSkillType = new List<z_SkillType>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshSkill()
        {
            serviceClient.GetSkillTypeCompleted += (s, e) =>
            {
                try
                {
                    SkillType = e.Result;
                    if (SkillType != null)
                        AllSkillType = SkillType.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetSkillTypeAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_SkillType> _SkillType;
        public IEnumerable<z_SkillType> SkillType
        {
            get { return _SkillType; }
            set { _SkillType = value; OnPropertyChanged("SkillType"); }
        }

        private z_SkillType _CurrentSkillType;
        public z_SkillType CurrentSkillType
        {
            get { return _CurrentSkillType; }
            set { _CurrentSkillType = value; OnPropertyChanged("CurrentSkillType"); }
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
            CurrentSkillType = null;
            CurrentSkillType = new z_SkillType();
            RefreshSkill();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentSkillType.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentSkillType.save_datetime = DateTime.Now;
                _CurrentSkillType.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentSkillType.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateSkill(_CurrentSkillType))
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
            if (_CurrentSkillType == null || _CurrentSkillType.skill_type_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteSkill(_CurrentSkillType))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentSkillType == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentSkillType.skill_type))
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
            set { _Search = value; OnPropertyChanged("Search"); if (SkillType != null) searchTextChanged(); }
        }
        private void searchTextChanged()
        {
            SkillType = null;
            SkillType = AllSkillType.Where(e => e.skill_type.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
