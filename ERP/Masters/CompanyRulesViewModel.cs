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
    class CompanyRulesViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public CompanyRulesViewModel()
        {
            refreshDepartment();
            refreshCompanyRule();
            CurrentCompanyRule = new mas_CompanyRule();
            CurrentDepartment = new z_Department();
        }
        #endregion

        #region Properties

        private bool _IsFix;

        public bool IsFix
        {
            get { return _IsFix; }
            set { _IsFix = value; OnPropertyChanged("IsFix"); }
        }


        private IEnumerable<z_Department> _Department;

        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { _Department = value; OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepartment;

        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; OnPropertyChanged("CurrentDepartment"); }
        }

        private IEnumerable<mas_CompanyRule> _CompanyRule;

        public IEnumerable<mas_CompanyRule> CompanyRule
        {
            get { return _CompanyRule; }
            set { _CompanyRule = value; OnPropertyChanged("CompanyRule"); }
        }

        private mas_CompanyRule _CurrentCompanyRule;

        public mas_CompanyRule CurrentCompanyRule
        {
            get { return _CurrentCompanyRule; }
            set { _CurrentCompanyRule = value; OnPropertyChanged("CurrentCompanyRule"); if (CurrentCompanyRule != null)  CheckBenifit(); if (IsFix != null) CheckFix(); }
        }

        private bool _CheckBen;

        public bool CheckBen
        {
            get { return _CheckBen; }
            set { _CheckBen = value; OnPropertyChanged("CheckBen"); }
        }

        private bool _CheckDed;

        public bool CheckDed
        {
            get { return _CheckDed; }
            set { _CheckDed = value; OnPropertyChanged("CheckDed"); }
        }

        private bool _Partial;

        public bool Partial
        {
            get { return _Partial; }
            set { _Partial = value; OnPropertyChanged("Partial"); }
        }

        private bool _Full;

        public bool Full
        {
            get { return _Full; }
            set { _Full = value; OnPropertyChanged("Full"); }
        }

        #endregion

        #region Refresh Methods

        public void refreshDepartment()
        {
            serviceClient.GetDepartmentCompleted += (s, e) =>
                {
                    Department = e.Result;
                };
            serviceClient.GetDepartmentAsync();
        }

        public void refreshCompanyRule()
        {
            serviceClient.GetCompanyCompleted += (s, e) =>
                {
                    CompanyRule = e.Result;
                };
            serviceClient.GetCompanyAsync();
        }

        #endregion

        #region Buttons & Methods

        #region SaveButton

        public ICommand Savebtn
        {
            get
            {
                return new RelayCommand(Save, SaveCE);
            }
        }
        private bool SaveCE()
        {
            if (CurrentCompanyRule != null)
                return true;
            else
                return false;
        }
        public void Save()
        {
            bool IsUpdate = false;
            z_Datamigration_Configuration fix = new z_Datamigration_Configuration();
            if (fix != null)
            {
                fix.migration_id = Guid.NewGuid();
                fix.rule_id = CurrentCompanyRule.rule_id;
                fix.default_qty = 1;
                fix.is_update = false;
                fix.is_active = IsFix;
            }
            IEnumerable<mas_CompanyRule> allrules = serviceClient.GetCompany();
            if (allrules != null)
            {
                foreach (mas_CompanyRule Rule in allrules)
                {
                    if (Rule.rule_id == CurrentCompanyRule.rule_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            mas_CompanyRule newCompanyRule = new mas_CompanyRule();
            newCompanyRule.rule_id = CurrentCompanyRule.rule_id;
            newCompanyRule.department_id = CurrentCompanyRule.department_id;
            newCompanyRule.benifit_id = CheckBen == true ? Guid.NewGuid() : Guid.Empty;
            newCompanyRule.deduction_id = CheckDed == true ? Guid.NewGuid() : Guid.Empty;
            newCompanyRule.unit_id = Guid.Empty;
            newCompanyRule.rate = 1;
            newCompanyRule.rule_name = CurrentCompanyRule.rule_name;
            newCompanyRule.minimum_value = 0;
            newCompanyRule.maximum_value = 1;
            newCompanyRule.isEffecToTheCompanyVariable = false;
            newCompanyRule.isActive = CurrentCompanyRule.isActive;
            newCompanyRule.ext_acc_no = CurrentCompanyRule.ext_acc_no;
            newCompanyRule.nonext_acc_no = CurrentCompanyRule.nonext_acc_no;
            if (Partial == true)
            {
                newCompanyRule.status = "P"; 
            }
            else if (Full == true)
            {
                newCompanyRule.status = "F";
            }

            if (IsUpdate)
            {
                if (clsSecurity.GetUpdatePermission(504))
                {
                    newCompanyRule.modified_datetime = DateTime.Now;
                    newCompanyRule.modified_user_id = clsSecurity.loggedUser.user_id;
                    if (serviceClient.UpdateCompanyRule(newCompanyRule))
                    {
                        if (fix != null)
                        {
                            if (serviceClient.SaveFixRule(fix))
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            }
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                        New();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateFail);
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to update this record");
            }
            else
            {
                if (clsSecurity.GetSavePermission(504))
                {
                    if (ValidateSave())
                    {

                        newCompanyRule.rule_id = Guid.NewGuid();
                        newCompanyRule.save_user_id = clsSecurity.loggedUser.user_id;
                        newCompanyRule.save_datetime = DateTime.Now;
                        newCompanyRule.isdelete = false;
                        if (serviceClient.SaveCompanyRule(newCompanyRule))
                        {
                            if (fix != null)
                            {
                                fix.rule_id = newCompanyRule.rule_id;
                                if (serviceClient.SaveFixRule(fix))
                                {
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                }
                                else
                                {
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                }
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                            }
                            New();
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to save this record");
            }

        }
        private bool ValidateSave()
        {
            IEnumerable<mas_CompanyRule> Rule = serviceClient.GetCompany();
            if (string.IsNullOrEmpty(CurrentCompanyRule.rule_name) == true)
            {
                clsMessages.setMessage("Enter a Rule Name");
                return false;
            }
            else if(CurrentCompanyRule.department_id == Guid.Empty)
            {
                if(Rule.FirstOrDefault(c => c.rule_name == CurrentCompanyRule.rule_name) != null)
                {
                    clsMessages.setMessage("This Rule is Already entered for Default Department");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (Rule.FirstOrDefault(c => c.department_id == CurrentCompanyRule.department_id && c.rule_name == CurrentCompanyRule.rule_name) != null)
            {
                clsMessages.setMessage("Rule is Already Entered this Department Exists");
                return false;
            }
            else if (CurrentCompanyRule.department_id == null)
            {
                clsMessages.setMessage("Select a Department");
                return false;
            }
            else if (!(CheckBen == true && CheckDed == false || CheckBen == false && CheckDed == true))
            {
                clsMessages.setMessage("Check Benifit or Deduction Status");
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region NewButton

        public ICommand Newbtn
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            refreshDepartment();
            refreshCompanyRule();
            CurrentCompanyRule = new mas_CompanyRule();
            CurrentDepartment = new z_Department();
        }

        #endregion

        #region DeleteButton

        public ICommand Deletebtn
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (CurrentCompanyRule != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(504))
            {
                if (CurrentCompanyRule != null)
                {
                    mas_CompanyRule DelRule = new mas_CompanyRule();
                    DelRule.rule_id = CurrentCompanyRule.rule_id;
                    DelRule.isActive = CurrentCompanyRule.isActive;
                    DelRule.isdelete = CurrentCompanyRule.isdelete;
                    DelRule.delete_user_id = clsSecurity.loggedUser.user_id;
                    DelRule.delete_datetime = DateTime.Now;
                    if (serviceClient.DeleteCompanyRule(DelRule))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    New();
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to delete");
        }

        #endregion

        #region Benifit & Deduction Check Method

        private void CheckBenifit()
        {
            if (CurrentCompanyRule.benifit_id == Guid.Empty)
            {
                CheckBen = false;
                CheckDed = true;
                if(CurrentCompanyRule.status == "P")
                {
                    Partial = true;
                    Full = false;
                }
                else if (CurrentCompanyRule.status == "F")
                {
                    Partial = false;
                    Full = true;
                }
            }
            if (CurrentCompanyRule.deduction_id == Guid.Empty)
            {
                CheckBen = true;
                CheckDed = false;
                if (CurrentCompanyRule.status == "P")
                {
                    Partial = true;
                    Full = false;
                }
                else if (CurrentCompanyRule.status == "F")
                {
                    Partial = false;
                    Full = true;
                }
            }
        }

        #endregion

        #region Check Fix

        private void CheckFix()
        {
            IEnumerable<z_Datamigration_Configuration> Fix = serviceClient.GetFix();
            if (Fix != null && CurrentCompanyRule != null)
            {
                if (Fix.FirstOrDefault(c => c.rule_id == CurrentCompanyRule.rule_id) != null)
                {
                    IsFix = true;
                }
                else
                {
                    IsFix = false;
                }
            }

        }


        #endregion

        #endregion
    }
}
