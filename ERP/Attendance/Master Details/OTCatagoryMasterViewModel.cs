using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Masters
{
    class OTCatagoryMasterViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public OTCatagoryMasterViewModel()
        {
            this.reafreshOTCatagories();
            reafreshUnits();
            reafreshDepartment();
            this.New();
        }
        #endregion

        #region Properties
        private IEnumerable<z_OTCategory> _OTCatagories;
        public IEnumerable<z_OTCategory> OTCatagories
        {
            get
            {
                return this._OTCatagories;
            }
            set
            {
                this._OTCatagories = value;
                this.OnPropertyChanged("OTCatagories");
            }
        }

        private z_OTCategory _CurrentOTCatagory;
        public z_OTCategory CurrentOTCatagory
        {
            get
            {
                return this._CurrentOTCatagory;
            }
            set
            {
                this._CurrentOTCatagory = value;
                this.OnPropertyChanged("CurrentOTCatagory");

            }
        }

        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (this._Search == "")
                {
                    this.reafreshOTCatagories();
                }
                else
                {
                    SearchTextChanged();
                }

            }
        }

        private IEnumerable<z_BasicUnit> _BasicUnits;
        public IEnumerable<z_BasicUnit> BasicUnits
        {
            get { return _BasicUnits; }
            set { _BasicUnits = value; this.OnPropertyChanged("BasicUnits"); }
        }

        private IEnumerable<z_Department> _Department;
        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { _Department = value; this.OnPropertyChanged("Department"); }
        }
        

        //private z_Company _CurrentCompany;
        //public z_Company CurretCompany
        //{
        //    get { return _CurrentCompany; }
        //    set { _CurrentCompany = value; this.OnPropertyChanged("CurretCompany"); }
        //}
        
        
        #endregion

        #region New Method
        void New()
        {
            try
            {
               
                    CurrentOTCatagory = null;
                    CurrentOTCatagory = new z_OTCategory();
                    CurrentOTCatagory.ot_id = Guid.NewGuid();
                    reafreshOTCatagories(); 
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }
        #endregion

        #region NewButton Class & Property
        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }
        #endregion

        #region Save Method
        void Save()
        {

            Guid UnitID = BasicUnits.FirstOrDefault().unit_id;
            Guid DepartmentID = Department.FirstOrDefault().department_id;
          
            bool IsUpdate = false;

            z_OTCategory newOTCatagory = new z_OTCategory();
            newOTCatagory.ot_id = CurrentOTCatagory.ot_id;
            newOTCatagory.name = CurrentOTCatagory.name;
            newOTCatagory.calc_start_time = CurrentOTCatagory.calc_start_time;
            newOTCatagory.calc_end_time = CurrentOTCatagory.calc_end_time;
            newOTCatagory.minit_per_unit = CurrentOTCatagory.minit_per_unit;
            //newOTCatagory.break_time = CurrentOTCatagory.break_time;
            newOTCatagory.amount_for_unit = CurrentOTCatagory.amount_for_unit;
            newOTCatagory.priority = CurrentOTCatagory.priority;
            newOTCatagory.is_active = CurrentOTCatagory.is_active;
            newOTCatagory.save_user_id = clsSecurity.loggedUser.user_id;
            newOTCatagory.save_datetime = System.DateTime.Now;
            newOTCatagory.modified_user_id = clsSecurity.loggedUser.user_id;
            newOTCatagory.modified_datetime = System.DateTime.Now;
            newOTCatagory.delete_user_id = clsSecurity.loggedUser.user_id;
            newOTCatagory.delete_datetime = System.DateTime.Now;
            newOTCatagory.isdelete = false;

            Guid newBenifit_id = Guid.NewGuid();
            mas_Benifit newBenifits = new mas_Benifit();
            newBenifits.benifit_id = newBenifit_id;
            newBenifits.benifit_name = CurrentOTCatagory.name;
            newBenifits.save_datetime = System.DateTime.Now;
            newBenifits.save_user_id = clsSecurity.loggedUser.user_id;
            newBenifits.modified_datetime = System.DateTime.Now;
            newBenifits.modified_user_id = clsSecurity.loggedUser.user_id;
            newBenifits.delete_datetime = System.DateTime.Now;
            newBenifits.delete_user_id = clsSecurity.loggedUser.user_id;
            newBenifits.isActive = CurrentOTCatagory.is_active;

            mas_CompanyRule newCompanyRule = new mas_CompanyRule();
            newCompanyRule.rule_id = CurrentOTCatagory.ot_id;
            newCompanyRule.benifit_id = newBenifit_id;
            newCompanyRule.deduction_id = Guid.Empty;
            newCompanyRule.department_id = DepartmentID;
            newCompanyRule.rate = CurrentOTCatagory.amount_for_unit;
            newCompanyRule.unit_id = UnitID;
            newCompanyRule.minimum_value =decimal.Parse("0.00");
            newCompanyRule.maximum_value = decimal.Parse("0.00");
            newCompanyRule.rule_name = CurrentOTCatagory.name;
            //newCompanyRule.isActive = CurrentOTCatagory.is_active;
            newCompanyRule.isEffecToTheCompanyVariable = true ;
            newCompanyRule.isdelete = false;
            newCompanyRule.save_user_id = clsSecurity.loggedUser.user_id;
            newCompanyRule.save_datetime = System.DateTime.Now;
            newCompanyRule.modified_datetime = System.DateTime.Now;
            newCompanyRule.modified_user_id = clsSecurity.loggedUser.user_id;
            newCompanyRule.delete_datetime = System.DateTime.Now;
            newCompanyRule.delete_user_id = clsSecurity.loggedUser.user_id;

         
            IEnumerable<z_OTCategory> allOTCatogaries = this.serviceClient.GetOTCatagories();

            if (allOTCatogaries != null)
            {
                foreach (var OT in allOTCatogaries)
                {
                    if (OT.ot_id == CurrentOTCatagory.ot_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newOTCatagory != null && CurrentOTCatagory.ot_id != null)
            {
                if (IsUpdate)
                {
                    
                        newOTCatagory.modified_user_id = clsSecurity.loggedUser.user_id;
                        newOTCatagory.modified_datetime = System.DateTime.Now;

                        if (this.serviceClient.UpdateOTCatagories(newOTCatagory))
                        {
                            MessageBox.Show("Record Update Successfully");
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                        else
                        {
                            MessageBox.Show("Record Update Failed");
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        } 
                    
                }
                else
                {
                   
                        newOTCatagory.save_user_id = clsSecurity.loggedUser.user_id;
                        newOTCatagory.save_datetime = System.DateTime.Now;

                        if (this.serviceClient.SaveOTCatagories(newOTCatagory))
                        {
                            if (serviceClient.SaveBenifits(newBenifits))
                            {
                                if (this.serviceClient.SaveCompanyRules(newCompanyRule))
                                {
                                    MessageBox.Show("Record Save Sucess");
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                }
                                else
                                {
                                    MessageBox.Show("Company Rule Create Failed");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Benifit Create Failed");
                            }
                        }
                       
                       else
                        {
                            MessageBox.Show("OT Catagory Create Failed");
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        } 
                    }
                
                reafreshOTCatagories();
            }
            clsMessages.setMessage("Please Mention the OT Name...");
        }
        #endregion

        #region SaveButton Class & Property
        bool saveCanExecute()
        {
            if (CurrentOTCatagory != null)
            {
                if (CurrentOTCatagory.ot_id == null || CurrentOTCatagory.ot_id == Guid.Empty)
                    return false;
                if (CurrentOTCatagory.name == null || CurrentOTCatagory.name == string.Empty)
                    return false;
                if (CurrentOTCatagory.calc_start_time == null )
                    return false;
                if (CurrentOTCatagory.calc_end_time == null)
                    return false;
                if (CurrentOTCatagory.minit_per_unit == null)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }
        #endregion

        #region Delete Method
        void Delete()
        {
            MessageBoxResult rs = new MessageBoxResult();
            rs = MessageBox.Show("Do You Want to Delete This Record", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.Yes)
            {
                z_OTCategory newOT = new z_OTCategory();
                newOT.ot_id = CurrentOTCatagory.ot_id;
                newOT.delete_user_id = clsSecurity.loggedUser.user_id;
                newOT.delete_datetime = System.DateTime.Now;

                if (this.serviceClient.DeleteOTCatagory(newOT))
                {
                    MessageBox.Show("Record Deleted");
                    reafreshOTCatagories();
                }
                else
                {
                    MessageBox.Show("Record Delete Failed");
                }
            }
            this.New();
        }
        #endregion

        #region DeleteButton Class & Property
        bool deleteCanexecute()
        {
            if (CurrentOTCatagory == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanexecute);
            }
        } 
        #endregion

        #region OT Catagory List
        private void reafreshOTCatagories()
        {
            this.serviceClient.GetOTCatagoriesCompleted += (s, e) =>
                {
                    this.OTCatagories = e.Result.Where(m => m.isdelete==false);
                };
            this.serviceClient.GetOTCatagoriesAsync();
        } 
        #endregion

        #region Get Basic Units
        private void reafreshUnits()
        {
            this.serviceClient.GetUnitsCompleted += (s, e) =>
            {
                this.BasicUnits = e.Result;
            };
            this.serviceClient.GetUnitsAsync();
        }  
        #endregion

        #region Get Department
        private void reafreshDepartment()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Department = e.Result;
            };
            this.serviceClient.GetDepartmentsAsync();
        }
        #endregion

        #region Search Class
        public class relayCommand : ICommand
        {
            readonly Action<object> _Execute;
            readonly Predicate<object> _CanExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public relayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _Execute = execute;
                _CanExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _CanExecute == null ? true : _CanExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _Execute(parameter);
            }


            public event EventHandler CanExecuteChanged;
        }
        #endregion

        #region Search Property
        relayCommand _OperationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_OperationCommand == null)
                {
                    _OperationCommand = new relayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }

                return this._OperationCommand;
            }
        }

        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region Search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }

        #endregion

        #region Search Method for all Properties
        public void SearchTextChanged()
        {
            OTCatagories = OTCatagories.Where(e => e.name.ToUpper().Contains(Search.ToUpper())).ToList();
        }
        #endregion
    }
}
