using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
namespace ERP.MastersDetails
{
   public class CompanyVariableRulesViewModel : ViewModelBase
    {

       ERPServiceClient seviceClient = new ERPServiceClient();

       private List<CompanyVariableRulesView> ruleList = new List<CompanyVariableRulesView>();

       public CompanyVariableRulesViewModel()
       {
           refreshCompanyVariable();
           refreshCompanyRules();
           IsActive = true;
       }

       private IEnumerable<z_CompanyVariable> _CompanyVariables;
       public IEnumerable<z_CompanyVariable> CompanyVariables
       {
           get { return _CompanyVariables; }
           set { _CompanyVariables = value; OnPropertyChanged("CompanyVariables"); }
       }

       private z_CompanyVariable _CurrentCompanyVariable;
       public z_CompanyVariable CurrentCompanyVariable
       {
           get { return _CurrentCompanyVariable; }
           set { _CurrentCompanyVariable = value; OnPropertyChanged("CurrentCompanyVariable");

           refreshVariableRulesByVariableId();

           }
       }
       
       private IEnumerable<mas_CompanyRule> _CompanyRules;
       public IEnumerable<mas_CompanyRule> CompanyRules
       {
           get { return _CompanyRules; }
           set { _CompanyRules = value; OnPropertyChanged("CompanyRules"); }
       }

       private mas_CompanyRule _CurrentCompanyRule;
       public mas_CompanyRule CurrentCompanyRule
       {
           get { return _CurrentCompanyRule; }
           set { _CurrentCompanyRule = value; OnPropertyChanged("CurrentCompanyRule"); }
       }

       private IEnumerable<CompanyVariableRulesView> _VariableRules;
       public IEnumerable<CompanyVariableRulesView> VariableRules
       {
           get { return _VariableRules; }
           set { _VariableRules = value; OnPropertyChanged("VariableRules"); }
       }

       private CompanyVariableRulesView _CurrentVariableRule;
       public CompanyVariableRulesView CurrentVariableRule
       {
           get { return _CurrentVariableRule; }
           set { _CurrentVariableRule = value; OnPropertyChanged("CurrentVariableRule");

           if (CurrentVariableRule != null && CurrentVariableRule.company_Rule_id != new Guid())
               CurrentCompanyRule = CompanyRules.First(e => e.rule_id.Equals(CurrentVariableRule.company_Rule_id));
     
           }
       }

       private bool _IsActive;
       public bool IsActive
       {
           get { return _IsActive; }
           set { _IsActive = value; OnPropertyChanged("IsActive"); }
       }

       public ICommand Add
       {
           get { return new RelayCommand(add, addCanExecute); }
       }

       public ICommand Remove
       {
           get { return new RelayCommand(remove, removeCanExecute); } 
       }

       public ICommand Save
       {
           get { return new RelayCommand(save,canSaveExecute); }
       }

       public ICommand New
       {
           get { return new RelayCommand(newBtn,newCanExecute); }
       }

       private bool newCanExecute()
       {
           return true;
       }

       private void newBtn()
       {
           NewRec();
       }

       private bool canSaveExecute()
       {
           if (VariableRules == null)
           {
               return false;
           }
           return true;
       }

       private void save()
       {
           try
           {
               if (VariableRules != null)
               {
                   if (clsSecurity.GetSavePermission(510))
                   {
                       if (this.seviceClient.SaveCompanyVariableRules(CurrentCompanyVariable, VariableRules.ToArray()))
                           clsMessages.setMessage(Properties.Resources.SaveSucess);
                       else
                           clsMessages.setMessage(Properties.Resources.SaveFail); 
                   }
                   else
                       clsMessages.setMessage("You don't have permission to Save this record(s)");
               }
           }
           catch (Exception ex)
           {
               clsMessages.setMessage(Properties.Resources.SaveFail + " Technical Error Code - [VM-0000001] - (" + ex.Message + ")");
           }
           finally 
           {
               refreshVariableRulesByVariableId();
           }

       }

       private bool removeCanExecute()
       {
           if (CurrentCompanyRule != null && !CurrentCompanyRule.rule_id.Equals(Guid.Empty))
           {               
               foreach (CompanyVariableRulesView item in ruleList)
               {
                   if (item.company_Rule_id == CurrentCompanyRule.rule_id)
                   {
                       return true;
                   }
               }
           }
           else
           {
               return false;
           }
           return false;
       }

       private void remove()
       {
           try
           {
               if (CurrentCompanyRule != null)
               {
                   ruleList.RemoveAll(e => e.company_Rule_id.Equals(CurrentCompanyRule.rule_id));
                   VariableRules = null;
                   VariableRules = ruleList;
               }
           }
           catch (Exception ex)
           {
               clsMessages.setMessage(Properties.Resources.SaveFail + " Technical Error Code - [VM-0000003] - (" + ex.Message + ")");
           }        
       }

       private bool addCanExecute()
       {
           if (CurrentCompanyRule != null && !CurrentCompanyRule.rule_id.Equals(Guid.Empty))
           {
               foreach (CompanyVariableRulesView item in ruleList)
               {
                   if (item.company_Rule_id == CurrentCompanyRule.rule_id)
                   {
                       return false;
                   }
               }
           }
           else
           {
               return false;
           }
           return true;
       }

       private void add()
       {
           try
           {
               CompanyVariableRulesView newRule = new CompanyVariableRulesView();
               newRule.company_varible_id = CurrentCompanyVariable.company_variableID;
               newRule.variable_Name = CurrentCompanyVariable.variable_Name;
               newRule.company_Rule_id = CurrentCompanyRule.rule_id;
               newRule.rule_name = CurrentCompanyRule.rule_name;
               newRule.isActive = IsActive;
               newRule.save_datetime = System.DateTime.Now;
               newRule.save_user_id = clsSecurity.loggedUser.user_id;
               newRule.modified_datetime = System.DateTime.Now;
               newRule.modified_user_id = clsSecurity.loggedUser.user_id;
               newRule.delete_datetime = System.DateTime.Now;
               newRule.delete_user_id = clsSecurity.loggedUser.user_id;
               ruleList.Add(newRule);

               VariableRules = null;
               VariableRules = ruleList;
           }
           catch (Exception ex)
           {
               clsMessages.setMessage(Properties.Resources.SaveFail + " Technical Error Code - [VM-0000002] - (" + ex.Message + ")");
           }

       }

       private void refreshCompanyVariable()
       {
           this.seviceClient.GetCompanyVariablesCompleted += (s, e) =>
               {
                   this.CompanyVariables = e.Result.Where(c=>c.isdelete==false);
               };
           this.seviceClient.GetCompanyVariablesAsync();
       }

       private void refreshCompanyRules()
       {
           this.seviceClient.GetCompanyRulesCompleted += (s, e) => 
           {
               this.CompanyRules = e.Result.Where(c=>c.isdelete==false);
           };
           this.seviceClient.GetCompanyRulesAsync();
       }

       private void refreshVariableRulesByVariableId()
       {
           if (this.CurrentCompanyVariable != null && !this.CurrentCompanyVariable.company_variableID.Equals(new Guid()))
           {
               this.seviceClient.GetVariableRulesCompleted += (s, e) =>
               {
                   this.VariableRules = e.Result.Where(c=>c.isdelete==false);

                   ruleList.Clear();
                   foreach (CompanyVariableRulesView item in VariableRules)
                   {
                       ruleList.Add(item);
                   }                   
               };
               this.seviceClient.GetVariableRulesAsync(this.CurrentCompanyVariable.company_variableID);
           }
       }

       private void NewRec()
       {
           CurrentCompanyRule = null;
           CurrentCompanyVariable = null;
           CurrentVariableRule = null;
           VariableRules = null;
           ruleList.Clear();
       }
    }
}
