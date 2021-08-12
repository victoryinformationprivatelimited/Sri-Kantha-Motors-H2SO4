using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Payroll
{
    public class EmployeeRulePerPersonViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient;
        bool RunMethod;
        public EmployeeRulePerPersonViewModel()
        {
            RunMethod = false;
            serviceClient = new ERPServiceClient();
        }

        private IEnumerable<EmployeeSearchView> _employees;
        public IEnumerable<EmployeeSearchView> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged("Employees"); }
        }


        private IEnumerable<mas_CompanyRule> _companyRules;
        public IEnumerable<mas_CompanyRule> CompanyRules
        {
            get { return _companyRules; }
            set { _companyRules = value; OnPropertyChanged("CompanyRules"); }
        }


        private mas_CompanyRule _currentCompanyRules;
        public mas_CompanyRule CurrentCompanyRules
        {
            get { return _currentCompanyRules; }
            set { _currentCompanyRules = value; OnPropertyChanged("CurrentCompanyRules"); }
        }

        private void reafreshCompanyRule()
        {
            this.serviceClient.GetCompanyRulesCompleted += (s, e) =>
            {
                this.CompanyRules = e.Result.Where(z => z.isActive == true && z.isdelete == false);
            };
            this.serviceClient.GetCompanyRulesAsync();
        }

        private double _specialAmont;

        public double SpecialAmont
        {
            get { return _specialAmont; }
            set { _specialAmont = value; OnPropertyChanged("SpecialAmont"); }
        }


        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("IsActive"); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }


        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged("Content"); }
        }


        private bool _isSepcialRate;
        public bool IsSpecialRate
        {
            get { return _isSepcialRate; }
            set { _isSepcialRate = value; OnPropertyChanged("IsSpecialRate"); }
        }


        void Save()
        {
            dtl_EmployeeRule saveEmployeeRule = new dtl_EmployeeRule();
            dtl_EmployeeRule updateEmployeeRule = new dtl_EmployeeRule();


            dtl_EmployeeRule tempEmployeeRules = new dtl_EmployeeRule();
            foreach (var emp in Employees)
            {
                bool isUpdate = false;

                IEnumerable<dtl_EmployeeRule> allemployeeRule = serviceClient.GetEmployeeRule();

                foreach (dtl_EmployeeRule newemployeeRule in allemployeeRule)
                {
                    if (emp.employee_id == newemployeeRule.employee_id && newemployeeRule.rule_id == CurrentCompanyRules.rule_id)
                    {
                        isUpdate = true;
                        break;
                    }

                }

                if (isUpdate)
                {
                    EmployeeRulePerPersonSpecialRateWindow window = new EmployeeRulePerPersonSpecialRateWindow(this);
                    window.Show();

                    if (RunMethod)
                    {
                        updateEmployeeRule.rule_id = CurrentCompanyRules.rule_id;
                        updateEmployeeRule.employee_id = (Guid)emp.employee_id;

                        if (IsSpecialRate)
                            updateEmployeeRule.special_amount = (Decimal)SpecialAmont;
                        else
                            updateEmployeeRule.special_amount = 0;

                        updateEmployeeRule.is_special = IsSpecialRate;
                        updateEmployeeRule.isactive = IsActive;
                        updateEmployeeRule.modified_user_id = clsSecurity.loggedUser.user_id;
                        updateEmployeeRule.modified_datetime = System.DateTime.Now; 
                    }
                }
                else
                {
                    if (RunMethod)
                    {
                        saveEmployeeRule.rule_id = CurrentCompanyRules.rule_id;
                        saveEmployeeRule.employee_id = (Guid)emp.employee_id;

                        if (IsSpecialRate)
                            saveEmployeeRule.special_amount = (Decimal)SpecialAmont;
                        else
                            saveEmployeeRule.special_amount = 0;
                        saveEmployeeRule.is_special = IsSpecialRate;
                        saveEmployeeRule.isactive = IsActive;
                        saveEmployeeRule.save_user_id = clsSecurity.loggedUser.user_id;
                        saveEmployeeRule.save_datetime = System.DateTime.Now; 
                    }
                }



            }

      
            if (saveEmployeeRule != null)
            {

            }

            if (updateEmployeeRule != null)
            {

            }


        }

        public ICommand OK
        {
            get { return new RelayCommand(OKmethod); }
        }
        void OKmethod()
        {
            RunMethod = true;
        }
        public ICommand Cancel 
        {
            get { return new RelayCommand(Cancelmethod);}
        }

        void Cancelmethod() 
        {
            RunMethod = false;
        }
        //public Ico
    }
}
