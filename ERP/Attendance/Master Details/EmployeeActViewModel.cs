using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Master_Details
{
    public class EmployeeActViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Global Variables
        Guid EmployeeID;
        decimal BasicSalary;
        public string SaveList = "";
       public string FailList = "";
        #endregion

        #region Constructor
        public EmployeeActViewModel(Guid EmployeeID, decimal BasicSalary)
        {
            this.EmployeeID = EmployeeID;
            this.BasicSalary = BasicSalary;

            Start();
        }
        #endregion

        #region Properties
        private IEnumerable<dtl_OtConfiguration> _OtConfiguration;
        public IEnumerable<dtl_OtConfiguration> OtConfiguration
        {
            get { return _OtConfiguration; }
            set { _OtConfiguration = value; OnPropertyChanged("OtConfiguration"); }
        }

        private List<dtl_OtConfiguration> _OtConfigurationList = new List<dtl_OtConfiguration>();
        public List<dtl_OtConfiguration> OtConfigurationList
        {
            get { return _OtConfigurationList; }
            set { _OtConfigurationList = value; OnPropertyChanged("OtConfigurationList"); }
        }

        private IEnumerable<dtl_NoPayConfiguration> _NoPayConfiguration;
        public IEnumerable<dtl_NoPayConfiguration> NoPayConfiguration
        {
            get { return _NoPayConfiguration; }
            set { _NoPayConfiguration = value; OnPropertyChanged("NoPayConfiguration"); }
        }

        private List<dtl_NoPayConfiguration> _NoPayConfigurationList = new List<dtl_NoPayConfiguration>();
        public List<dtl_NoPayConfiguration> NoPayConfigurationList
        {
            get { return _NoPayConfigurationList; }
            set { _NoPayConfigurationList = value; OnPropertyChanged("NoPayConfigurationList"); }
        }

        private IEnumerable<dtl_LateConfiguration> _LateConfiguration;
        public IEnumerable<dtl_LateConfiguration> LateConfiguration
        {
            get { return _LateConfiguration; }
            set { _LateConfiguration = value; OnPropertyChanged("LateConfiguration"); }
        }

        private List<dtl_LateConfiguration> _LateConfigurationList = new List<dtl_LateConfiguration>();
        public List<dtl_LateConfiguration> LateConfigurationList
        {
            get { return _LateConfigurationList; }
            set { _LateConfigurationList = value; OnPropertyChanged("LateConfigurationList"); }
        }

        private IEnumerable<z_EmployeeAct> _EmployeeAct;
        public IEnumerable<z_EmployeeAct> EmployeeAct
        {
            get { return _EmployeeAct; }
            set { _EmployeeAct = value; OnPropertyChanged("EmployeeAct"); }
        }

        private z_EmployeeAct _CurrentEmployeeAct;
        public z_EmployeeAct CurrentEmployeeAct
        {
            get { return _CurrentEmployeeAct; }
            set { _CurrentEmployeeAct = value; OnPropertyChanged("CurrentEmployeeAct"); if (CurrentEmployeeAct != null)Filter(); }
        }

        private IEnumerable<mas_CompanyRule> _CompanyRule;
        public IEnumerable<mas_CompanyRule> CompanyRule
        {
            get { return _CompanyRule; }
            set { _CompanyRule = value; OnPropertyChanged("CompanyRule"); }
        }

        private IEnumerable<dtl_EmployeeRule> _EmployeeRule;
        public IEnumerable<dtl_EmployeeRule> EmployeeRule
        {
            get { return _EmployeeRule; }
            set { _EmployeeRule = value; OnPropertyChanged("EmployeeRule"); }
        }
        #endregion

        #region Filter
        void Filter()
        {
            if (EmployeeRule != null)
            {
                foreach (var item in OtConfiguration)
                {
                    List<dtl_EmployeeRule> rule = new List<dtl_EmployeeRule>();
                    rule = EmployeeRule.Where(c => c.rule_id == item.otConfig_id).ToList();
                    if (rule != null && rule.Count == 1 && rule.FirstOrDefault().isdelete == false)
                        item.is_check = true;
                    else
                        item.is_check = null;
                }

                foreach (var item in NoPayConfiguration)
                {
                    List<dtl_EmployeeRule> rule = new List<dtl_EmployeeRule>();
                    rule = EmployeeRule.Where(c => c.rule_id == item.noPayConfiguration_id).ToList();
                    if (rule != null && rule.Count == 1 && rule.FirstOrDefault().isdelete == false)
                        item.is_check = true;
                    else
                        item.is_check = null;
                }

                foreach (var item in LateConfiguration)
                {
                    List<dtl_EmployeeRule> rule = new List<dtl_EmployeeRule>();
                    rule = EmployeeRule.Where(c => c.rule_id == item.lateConfig_id).ToList();
                    if (rule != null && rule.Count == 1 && rule.FirstOrDefault().isdelete == false)
                        item.is_check = true;
                    else
                        item.is_check = null;
                }
            }
            if (OtConfiguration != null)
            {
                OtConfigurationList = null;
                OtConfigurationList = OtConfiguration.Where(c => c.Act_id == CurrentEmployeeAct.Act_id).ToList();
            }
            if (NoPayConfiguration != null)
            {
                NoPayConfigurationList = null;
                NoPayConfigurationList = NoPayConfiguration.Where(c => c.Act_id == CurrentEmployeeAct.Act_id).ToList();
            }
            if (LateConfiguration != null)
            {
                LateConfigurationList = null;
                LateConfigurationList = LateConfiguration.Where(c => c.Act_id == CurrentEmployeeAct.Act_id).ToList();
            }


        }

        void Start()
        {
            refreshEmployeeRules();
            refreshOtConfig();
            refreshNoPayConfig();
            refershLateConfig();
            refreshAct();

        }
        #endregion

        #region Save Method And Buttons
        bool ActCheck()
        {
            foreach (var item in OtConfiguration)
                if (item.is_check == true)
                {
                    if (CurrentEmployeeAct.Act_id == item.Act_id == false)
                        return false;
                }
            foreach (var item in NoPayConfiguration)
                if (item.is_check == true)
                {
                    if (CurrentEmployeeAct.Act_id == item.Act_id == false)
                        return false;
                }
            foreach (var item in LateConfiguration)
                if (item.is_check == true)
                {
                    if (CurrentEmployeeAct.Act_id == item.Act_id == false)
                        return false;
                }
            return true;
        }

        void Save()
        {
            if (ActCheck())
            {
                IEnumerable<mas_CompanyRule> newCompanyRule = serviceClient.GetCompanyRules();
                if (CheckLateConfiguration(newCompanyRule) || CheckNoPayConfiguration(newCompanyRule) || CheckOtConfiguration(newCompanyRule))
                {
                    BenfitAdd();
                    DeductionAdd();
                    CompanyRuleAdd();
                }
                List<dtl_EmployeeRule> SaveEmployeeRule = new List<dtl_EmployeeRule>();
                List<dtl_EmployeeRule> UpdateEmployeeRule = new List<dtl_EmployeeRule>();
                List<dtl_EmployeeRule> DeleteEmployeeRule = new List<dtl_EmployeeRule>();
                foreach (var item in OtConfigurationList)
                {
                    if (item.is_check == true)
                    {
                        if (EmployeeRule != null && EmployeeRule.Where(c => c.rule_id == item.otConfig_id).ToList().Count() == 1 )//&& EmployeeRule.FirstOrDefault(c => c.rule_id == item.otConfig_id).isdelete == true)
                        {
                            dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                            newDetailEmployeeRule.employee_id = EmployeeID;
                            newDetailEmployeeRule.rule_id = item.otConfig_id;
                            newDetailEmployeeRule.special_amount = Math.Round(((decimal)BasicSalary / (decimal)item.dividing_value) * (decimal)item.multiplying_value, 2);
                            newDetailEmployeeRule.is_special = true;
                            newDetailEmployeeRule.isactive = true;
                            newDetailEmployeeRule.isdelete = false;
                            newDetailEmployeeRule.modified_user_id = clsSecurity.loggedUser.user_id;
                            newDetailEmployeeRule.modified_datetime = System.DateTime.Now;
                            UpdateEmployeeRule.Add(newDetailEmployeeRule);
                        }
                        else
                        {
                            dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                            newDetailEmployeeRule.employee_id = EmployeeID;
                            newDetailEmployeeRule.rule_id = item.otConfig_id;
                            newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value) * (decimal)item.multiplying_value, 2);
                            newDetailEmployeeRule.is_special = true;
                            newDetailEmployeeRule.isactive = true;
                            newDetailEmployeeRule.isdelete = false;
                            newDetailEmployeeRule.save_user_id = clsSecurity.loggedUser.user_id;
                            newDetailEmployeeRule.save_datetime = System.DateTime.Now;
                            SaveEmployeeRule.Add(newDetailEmployeeRule);
                        }
                    }
                    else if (EmployeeRule != null && EmployeeRule.Where(c => c.rule_id == item.otConfig_id).ToList().Count() == 1)
                    {
                        dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                        newDetailEmployeeRule.employee_id = EmployeeID;
                        newDetailEmployeeRule.rule_id = item.otConfig_id;
                        newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value) * (decimal)item.multiplying_value, 2);
                        newDetailEmployeeRule.is_special = true;
                        newDetailEmployeeRule.isactive = false;
                        newDetailEmployeeRule.isdelete = true;
                        newDetailEmployeeRule.delete_user_id = clsSecurity.loggedUser.user_id;
                        newDetailEmployeeRule.delete_datetime = System.DateTime.Now;
                        DeleteEmployeeRule.Add(newDetailEmployeeRule);
                    }
                }

                foreach (var item in LateConfiguration)
                {
                    if (item.is_check == true)
                    {
                        if (EmployeeRule != null && EmployeeRule.Where(c => c.rule_id == item.lateConfig_id).ToList().Count() == 1)// && EmployeeRule.FirstOrDefault(c => c.rule_id == item.lateConfig_id).isdelete == true)
                        {
                            dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                            newDetailEmployeeRule.employee_id = EmployeeID;
                            newDetailEmployeeRule.rule_id = item.lateConfig_id;
                            newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value), 2);
                            newDetailEmployeeRule.is_special = true;
                            newDetailEmployeeRule.isactive = true;
                            newDetailEmployeeRule.isdelete = false;
                            newDetailEmployeeRule.modified_user_id = clsSecurity.loggedUser.user_id;
                            newDetailEmployeeRule.modified_datetime = System.DateTime.Now;
                            UpdateEmployeeRule.Add(newDetailEmployeeRule);
                        }
                        else
                        {
                            dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                            newDetailEmployeeRule.employee_id = EmployeeID;
                            newDetailEmployeeRule.rule_id = item.lateConfig_id;
                            newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value), 2);
                            newDetailEmployeeRule.is_special = true;
                            newDetailEmployeeRule.isactive = true;
                            newDetailEmployeeRule.isdelete = false;
                            newDetailEmployeeRule.save_user_id = clsSecurity.loggedUser.user_id;
                            newDetailEmployeeRule.save_datetime = System.DateTime.Now;
                            SaveEmployeeRule.Add(newDetailEmployeeRule);
                        }
                    }
                    else if (EmployeeRule != null && EmployeeRule.Where(c => c.rule_id == item.lateConfig_id).ToList().Count() == 1)
                    {
                        dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                        newDetailEmployeeRule.employee_id = EmployeeID;
                        newDetailEmployeeRule.rule_id = item.lateConfig_id;
                        newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value), 2);
                        newDetailEmployeeRule.is_special = true;
                        newDetailEmployeeRule.isactive = false;
                        newDetailEmployeeRule.isdelete = true;
                        newDetailEmployeeRule.delete_user_id = clsSecurity.loggedUser.user_id;
                        newDetailEmployeeRule.delete_datetime = System.DateTime.Now;
                        DeleteEmployeeRule.Add(newDetailEmployeeRule);
                    }

                }

                foreach (var item in NoPayConfiguration)
                {
                    if (item.is_check == true)
                    {
                        if (EmployeeRule != null && EmployeeRule.Where(c => c.rule_id == item.noPayConfiguration_id).ToList().Count() == 1 )//&& EmployeeRule.FirstOrDefault(c => c.rule_id == item.noPayConfiguration_id).isdelete == true)
                        {
                            dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                            newDetailEmployeeRule.employee_id = EmployeeID;
                            newDetailEmployeeRule.rule_id = item.noPayConfiguration_id;
                            newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value), 2);
                            newDetailEmployeeRule.is_special = true;
                            newDetailEmployeeRule.isactive = true;
                            newDetailEmployeeRule.isdelete = false;
                            newDetailEmployeeRule.modified_user_id = clsSecurity.loggedUser.user_id;
                            newDetailEmployeeRule.modified_datetime = System.DateTime.Now;
                            UpdateEmployeeRule.Add(newDetailEmployeeRule);
                        }
                        else
                        {
                            dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                            newDetailEmployeeRule.employee_id = EmployeeID;
                            newDetailEmployeeRule.rule_id = item.noPayConfiguration_id;
                            newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value), 2);
                            newDetailEmployeeRule.is_special = true;
                            newDetailEmployeeRule.isactive = true;
                            newDetailEmployeeRule.isdelete = false;
                            newDetailEmployeeRule.save_user_id = clsSecurity.loggedUser.user_id;
                            newDetailEmployeeRule.save_datetime = System.DateTime.Now;
                            SaveEmployeeRule.Add(newDetailEmployeeRule);
                        }
                    }
                    else if (EmployeeRule != null && EmployeeRule.Where(c => c.rule_id == item.noPayConfiguration_id).ToList().Count() == 1)
                    {
                        dtl_EmployeeRule newDetailEmployeeRule = new dtl_EmployeeRule();
                        newDetailEmployeeRule.employee_id = EmployeeID;
                        newDetailEmployeeRule.rule_id = item.noPayConfiguration_id;
                        newDetailEmployeeRule.special_amount = Math.Round((BasicSalary / (decimal)item.dividing_value), 2);
                        newDetailEmployeeRule.is_special = true;
                        newDetailEmployeeRule.isactive = false;
                        newDetailEmployeeRule.isdelete = true;
                        newDetailEmployeeRule.delete_user_id = clsSecurity.loggedUser.user_id;
                        newDetailEmployeeRule.delete_datetime = System.DateTime.Now;
                        DeleteEmployeeRule.Add(newDetailEmployeeRule);
                    }
                }
               
                //______________________________________________________________________
                foreach (var item in SaveEmployeeRule)
                    if (serviceClient.SaveEmployeeRules(item))
                        SaveList = Resources.SaveSucess + " " + "\n";
                    else
                        FailList += Resources.SaveFail + item.rule_id + " " + "\n";
                //______________________________________________________________________
                foreach (var item in UpdateEmployeeRule)
                    if (serviceClient.UpdateEmployeeRulesForAct(item))
                        SaveList = Resources.SaveSucess + " " + "\n";
                    else
                        FailList += Resources.SaveFail + item.rule_id + " " + "\n";
                //______________________________________________________________________
                foreach (var item in DeleteEmployeeRule)
                    if (serviceClient.DeleteEmployeeRulesForAct(item))
                        SaveList = Resources.SaveSucess + " " + "\n";
                    else
                        FailList += Resources.SaveFail + item.rule_id + " " + "\n";
                //______________________________________________________________________
                Xceed.Wpf.Toolkit.MessageBox.Show(SaveList + "\n" + FailList, "Save List", MessageBoxButton.OK, MessageBoxImage.Information);
                refreshEmployeeRules();
            }
            else
            {
                clsMessages.setMessage("Already in a another Act");
                Filter();
            }



        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        #endregion

        #region Check Config
        bool CheckLateConfiguration(IEnumerable<mas_CompanyRule> newCompanyRule)
        {
            if (LateConfiguration != null || newCompanyRule != null)
            {
                foreach (var item in LateConfiguration)
                {
                    IEnumerable<mas_CompanyRule> newRule = newCompanyRule.Where(c => c.rule_id == item.lateConfig_id);
                    if (newRule == null || newRule.ToList().Count == 0)
                        return true;
                }
                return false;
            }
            else
                return false;

        }

        bool CheckNoPayConfiguration(IEnumerable<mas_CompanyRule> newCompanyRule)
        {
            if (NoPayConfiguration != null || newCompanyRule != null)
            {
                foreach (var item in NoPayConfiguration)
                {
                    IEnumerable<mas_CompanyRule> newRule = newCompanyRule.Where(c => c.rule_id == item.noPayConfiguration_id);
                    if (newRule == null || newRule.ToList().Count == 0)
                        return true;
                }
                return false;
            }
            else
                return false;
        }

        bool CheckOtConfiguration(IEnumerable<mas_CompanyRule> newCompanyRule)
        {
            if (OtConfiguration != null || newCompanyRule != null)
            {
                foreach (var item in OtConfiguration)
                {
                    IEnumerable<mas_CompanyRule> newRule = newCompanyRule.Where(c => c.rule_id == item.otConfig_id);
                    if (newRule == null || newRule.ToList().Count == 0)
                        return true;
                }
                return false;
            }
            else
                return false;
        }
        #endregion

        #region Add Rules To Tables
        void BenfitAdd()
        {
            if (EmployeeAct != null && OtConfiguration != null)
                foreach (var item in OtConfiguration)
                {
                    mas_Benifit newSave = new mas_Benifit();
                    newSave.benifit_name = item.ot_name + " " + "(" + EmployeeAct.FirstOrDefault(c => c.Act_id == item.Act_id).Act_Name + ")";
                    newSave.benifit_id = item.otConfig_id;
                    newSave.isActive = true;
                    newSave.isdelete = false;
                    try
                    {

                        serviceClient.SaveActBenifts(newSave);
                    }
                    catch (Exception ex)
                    {

                        clsMessages.setMessage(ex.Message.ToString());
                    }
                }
            else
                clsMessages.setMessage("z_EmployeeAct Null or dtl_OtConfiguration Null");
        }

        void DeductionAdd()
        {
            if (EmployeeAct != null && NoPayConfiguration != null)
                foreach (var item in NoPayConfiguration)
                {
                    mas_Deduction newSave = new mas_Deduction();
                    newSave.deduction_name = item.noPay_name + " " + "(" + EmployeeAct.FirstOrDefault(c => c.Act_id == item.Act_id).Act_Name + ")";
                    newSave.deduction_id = item.noPayConfiguration_id;
                    newSave.isActive = true;
                    newSave.isdelete = false;
                    try
                    {
                        serviceClient.SaveActDeductions(newSave);
                    }
                    catch (Exception ex)
                    {

                        clsMessages.setMessage(ex.Message.ToString());
                    }
                }
            else
                clsMessages.setMessage("z_EmployeeAct Null or dtl_NoPayConfiguration Null");

            if (EmployeeAct != null && LateConfiguration != null)
                foreach (var item in LateConfiguration)
                {
                    mas_Deduction newSave = new mas_Deduction();
                    newSave.deduction_name = item.late_name + " " + "(" + EmployeeAct.FirstOrDefault(c => c.Act_id == item.Act_id).Act_Name + ")";
                    newSave.deduction_id = item.lateConfig_id;
                    newSave.isActive = true;
                    newSave.isdelete = false;
                    serviceClient.SaveActDeductions(newSave);
                }
            else
                clsMessages.setMessage("z_EmployeeAct Null or dtl_LateConfiguration Null");
        }

        void CompanyRuleAdd()
        {
            foreach (var item in OtConfiguration)
            {
                mas_CompanyRule newSave = new mas_CompanyRule();
                newSave.unit_id = new Guid("ec4e94e8-0d6b-4ab7-a7d7-6da6e863de36");
                newSave.benifit_id = item.otConfig_id;
                newSave.deduction_id = Guid.Empty;
                newSave.rule_id = item.otConfig_id;
                newSave.rate = Math.Round((10000 / (decimal)item.dividing_value) * (decimal)item.multiplying_value, 2);
                newSave.rule_name = item.ot_name + " " + "(" + EmployeeAct.FirstOrDefault(c => c.Act_id == item.Act_id).Act_Name + ")";
                newSave.maximum_value = Math.Round(((100000 / (decimal)item.dividing_value) * (decimal)item.multiplying_value), 2);
                newSave.minimum_value = 0;
                newSave.isActive = true;
                newSave.isdelete = false;
                newSave.isEffecToTheCompanyVariable = true;
                newSave.unit_id = Guid.Empty;
                newSave.department_id = Guid.Empty;
                serviceClient.SaveCompanyRules(newSave);
            }

            foreach (var item in NoPayConfiguration)
            {
                mas_CompanyRule newSave = new mas_CompanyRule();
                newSave.unit_id = new Guid("ec4e94e8-0d6b-4ab7-a7d7-6da6e863de36");
                newSave.benifit_id = Guid.Empty;
                newSave.deduction_id = item.noPayConfiguration_id;
                newSave.rule_id = item.noPayConfiguration_id;
                newSave.rate = Math.Round((10000 / (decimal)item.dividing_value), 2);
                newSave.rule_name = item.noPay_name + " " + "(" + EmployeeAct.FirstOrDefault(c => c.Act_id == item.Act_id).Act_Name + ")";
                newSave.maximum_value = Math.Round((100000 / (decimal)item.dividing_value), 2);
                newSave.minimum_value = 0;
                newSave.isActive = true;
                newSave.isdelete = false;
                newSave.isEffecToTheCompanyVariable = true;
                newSave.unit_id = Guid.Empty;
                newSave.department_id = Guid.Empty;
                serviceClient.SaveCompanyRules(newSave);
            }

            foreach (var item in LateConfiguration)
            {
                mas_CompanyRule newSave = new mas_CompanyRule();
                newSave.unit_id = new Guid("ec4e94e8-0d6b-4ab7-a7d7-6da6e863de36");
                newSave.benifit_id = Guid.Empty;
                newSave.deduction_id = item.lateConfig_id;
                newSave.rule_id = item.lateConfig_id;
                newSave.rate = Math.Round((10000 / (decimal)item.dividing_value), 2);
                newSave.rule_name = item.late_name + " " + "(" + EmployeeAct.FirstOrDefault(c => c.Act_id == item.Act_id).Act_Name + ")";
                newSave.maximum_value = Math.Round((100000 / (decimal)item.dividing_value), 2);
                newSave.minimum_value = 0;
                newSave.isActive = true;
                newSave.isdelete = false;
                newSave.isEffecToTheCompanyVariable = true;
                newSave.unit_id = Guid.Empty;
                newSave.department_id = Guid.Empty;
                serviceClient.SaveCompanyRules(newSave);
            }
        }
        #endregion

        #region Refresh
        void refreshOtConfig()
        {
            try
            {
                serviceClient.GetOtConfigurationCompleted += (s, e) =>
                {
                    OtConfiguration = e.Result;
                };
                serviceClient.GetOtConfigurationAsync();
            }
            catch (Exception)
            { }
        }

        void refreshNoPayConfig()
        {
            try
            {
                serviceClient.GetNoPayConfigurationCompleted += (s, e) =>
                {
                    NoPayConfiguration = e.Result;
                };
                serviceClient.GetNoPayConfigurationAsync();
            }
            catch (Exception)
            { }
        }

        void refershLateConfig()
        {
            try
            {
                serviceClient.GetLateConfigurationCompleted += (s, e) =>
                {
                    LateConfiguration = e.Result;
                };
                serviceClient.GetLateConfigurationAsync();
            }
            catch (Exception)
            { }
        }

        void refreshAct()
        {
            try
            {
                serviceClient.GetEMployeeActCompleted += (s, e) =>
                {
                    EmployeeAct = e.Result;
                };
                serviceClient.GetEMployeeActAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        void refreshEmployeeRules()
        {
            serviceClient.GetEmployeeRuleForActCompleted += (s, e) =>
            {
                EmployeeRule = e.Result.Where(c => c.employee_id == EmployeeID);
            };
            serviceClient.GetEmployeeRuleForActAsync(EmployeeID);
        }
        #endregion

    }
}
