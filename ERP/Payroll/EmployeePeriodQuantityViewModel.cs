using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Resources;
using System.Windows;

namespace ERP.Payroll
{
    public class EmployeePeriodQuantityViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient = new ERPServiceClient();
        List<EmployeeSumarryView> TempEmployees = new List<EmployeeSumarryView>();
        List<EmployeeSumarryView> vw = new List<EmployeeSumarryView>();
        public EmployeePeriodQuantityViewModel()
        {

            refreshEmployees();
            refreshperiods();
            refreshDepatments();
            refreshSections();
            newRecord();
        }

        #region Properties

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return this._Employees; }
            set { this._Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmplpyee;
        public EmployeeSumarryView CurrentEmplopyee
        {
            get { return _CurrentEmplpyee; }
            set
            {
                _CurrentEmplpyee = value; OnPropertyChanged("CurrentEmplopyee");
                if (CurrentEmplopyee != null)
                {
                    refreshEmployeeRulesByEmployeeId();

                }
                if (CurrentEmplopyee != null && CurrentPeriod != null)
                {
                    fillDataForGrid();
                }
            }


        }

        private IEnumerable<z_Period> _Priods;
        public IEnumerable<z_Period> Priods
        {
            get { return _Priods; }
            set { _Priods = value; OnPropertyChanged("Priods"); }
        }

        private IEnumerable<z_Department> _Department;
        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { this._Department = value; OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepatment;
        public z_Department CurrentDepatment
        {
            get { return _CurrentDepatment; }
            set { _CurrentDepatment = value; OnPropertyChanged("CurrentDepatment"); refreshSections(); filter(); }
        }

        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get { return _Sections; }
            set { _Sections = value; OnPropertyChanged("Sections"); }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; OnPropertyChanged("CurrentSection"); filter(); }
        }

        private IEnumerable<EmployeePeriodQantityView> _QuantityPeriods;
        public IEnumerable<EmployeePeriodQantityView> QuantityPeriods
        {
            get { return _QuantityPeriods; }
            set { _QuantityPeriods = value; OnPropertyChanged("QuantityPeriods"); }
        }

        private EmployeePeriodQantityView _CurrentQuantityPeriod;
        public EmployeePeriodQantityView CurrentQuantityPeriod
        {
            get { return _CurrentQuantityPeriod; }
            set { _CurrentQuantityPeriod = value; OnPropertyChanged("CurrentQuantityPeriod"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set
            {
                _CurrentPeriod = value;
                OnPropertyChanged("CurrentPeriod");
                fillDataForGrid();
            }
        }

        private EmployeePeriodQantityView _CurrentQuntity;
        public EmployeePeriodQantityView CurrentQuntity
        {
            get { return _CurrentQuntity; }
            set { _CurrentQuntity = value; this.OnPropertyChanged("CurrentQuntity"); }
        }


        #endregion

        #region Button Command

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(saveRecode, saveCanExecute);
            }
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(newRecord, newCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(deleteRecode, deleteCanExecute);
            }
        }

        public ICommand NextButton
        {
            get { return new RelayCommand(nextEmployee, nextEmployeeCanExecute); }
        }

        public ICommand PriviosButton
        {
            get { return new RelayCommand(priviosEmployee, priviosEmployeeCanExecute); }
        }

        public ICommand UpdateButton
        {
            get
            {
                return new RelayCommand(updateRecode, updateCanExecute);
            }
        }

        private void updateRecode()
        {
            if (CurrentQuantityPeriod != null && CurrentEmplopyee != null && CurrentQuantityPeriod.rule_id != Guid.Empty && CurrentEmplopyee.employee_id != Guid.Empty)
            {
                if (clsSecurity.GetUpdatePermission(511))
                {
                    trns_EmployeePeriodQunatity p_quntity = serviceClient.checkEmployeePeriodQuntityExist(CurrentQuantityPeriod.rule_id, CurrentPeriod.period_id, CurrentEmplopyee.employee_id).FirstOrDefault();
                    dtl_EmployeeRule d_rule = serviceClient.checkEmployeePeriodQuntitySpecialAmount(CurrentEmplopyee.employee_id, CurrentQuantityPeriod.rule_id).FirstOrDefault();
                    if (p_quntity != null && d_rule != null)
                    {

                        //if (CurrentQuantityPeriod.Quantity != null)
                        {
                            bool specialamountaUpdate = false;
                            bool periodQuntityUpdate = false;

                            trns_EmployeePeriodQunatity new_qty = new trns_EmployeePeriodQunatity();
                            new_qty.rule_id = CurrentQuantityPeriod.rule_id;
                            new_qty.employee_id = CurrentQuantityPeriod.employee_id;
                            new_qty.period_id = CurrentPeriod.period_id;
                            new_qty.quantity = CurrentQuantityPeriod.Quantity;
                            new_qty.modified_datetime = System.DateTime.Now.Date;
                            new_qty.modified_user_id = clsSecurity.loggedUser.user_id;

                            dtl_EmployeeRule new_rule = new dtl_EmployeeRule();
                            new_rule.rule_id = CurrentQuantityPeriod.rule_id;
                            new_rule.employee_id = CurrentQuantityPeriod.employee_id;
                            new_rule.special_amount = CurrentQuantityPeriod.special_amount;
                            new_rule.modified_datetime = System.DateTime.Now.Date;
                            new_rule.modified_user_id = clsSecurity.loggedUser.user_id;

                            if (serviceClient.UpdateSpecialAmount(new_rule))
                            {
                                specialamountaUpdate = true;
                            }
                            if (serviceClient.UpdateSinglePeriodQuntity(new_qty))
                            {
                                periodQuntityUpdate = true;
                            }
                            if (specialamountaUpdate == true && periodQuntityUpdate == true)
                            {
                                MessageBox.Show("Employee Period Quantity and Special amount update Successfully ");
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Please check the employee rule and try again");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to Update this record(s)");

            }

        }

        private bool updateCanExecute()
        {
            if (CurrentQuantityPeriod == null)
            {
                return false;
            }
            return true;
        }

        private bool priviosEmployeeCanExecute()
        {
            if (Employees == null)
                return false;
            else if (CurrentPeriod == null)
                return false;
            else
                return true;
        }

        private void priviosEmployee()
        {
            vw.Clear();
            vw = Employees.ToList();
            if (Employees != null)
            {
                int ElementNo = 0;
                if (CurrentEmplopyee == null)
                    ElementNo = -1;
                else
                {
                    ElementNo = vw.IndexOf(CurrentEmplopyee);
                    ElementNo--;
                }

                try
                {
                    CurrentEmplopyee = Employees.ElementAt(ElementNo);
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("This Element is Empty");
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Sorry..No more elements available");
                }
            }
        }

        private bool nextEmployeeCanExecute()
        {
            if (Employees == null)
                return false;
            else if (CurrentPeriod == null)
                return false;
            else
                return true;
        }

        private void nextEmployee()
        {
            if (Employees != null)
            {
                vw.Clear();
                vw = Employees.ToList();
                int ElementNo = 0;
                if (CurrentEmplopyee == null)
                    ElementNo = 0;
                else
                {
                    ElementNo = vw.IndexOf(CurrentEmplopyee);
                    ElementNo++;
                }

                try
                {
                    CurrentEmplopyee = Employees.ElementAt(ElementNo);
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("This Element is Empty");
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Sorry..No more elements available");
                }
            }
        }

        private bool deleteCanExecute()
        {
            if (CurrentEmplopyee != null && CurrentPeriod != null)
            {
                if (CurrentEmplopyee.employee_id == null || CurrentEmplopyee.employee_id == Guid.Empty)
                    return false;
                if (CurrentPeriod.period_id == null || CurrentPeriod.period_id == Guid.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        private void deleteRecode()
        {
            if (clsSecurity.GetDeletePermission(511))
            {

                foreach (EmployeePeriodQantityView item in QuantityPeriods)
                {
                    List<trns_EmployeePeriodQunatity> temp = this.serviceClient.GetTrnsPeriodQuanity(CurrentEmplopyee.employee_id, item.rule_id, CurrentPeriod.period_id).ToList();
                    foreach (trns_EmployeePeriodQunatity trnitem in temp)
                    {
                        if (trnitem.is_proceed != true)
                        {
                            if (temp.Count != 0)
                            {

                                trns_EmployeePeriodQunatity newEmpquantity = new trns_EmployeePeriodQunatity();
                                newEmpquantity.employee_id = item.employee_id;
                                newEmpquantity.rule_id = item.rule_id;
                                newEmpquantity.period_id = CurrentPeriod.period_id;
                                newEmpquantity.quantity = item.Quantity;



                                if (this.serviceClient.deleteTransPeriodQuantity(newEmpquantity))
                                {
                                    trns_alter_EmployeePeriodQuantity newalter = new trns_alter_EmployeePeriodQuantity();
                                    newalter.employee_id = newEmpquantity.employee_id;
                                    newalter.rule_id = newEmpquantity.rule_id;
                                    newalter.period_id = newEmpquantity.period_id;
                                    newalter.quantity = newEmpquantity.quantity;
                                    this.serviceClient.addTrnalterEmplyeePeriodQuntityCompleted += (s, e) =>
                                        {
                                            bool a = e.Result;
                                        };
                                    this.serviceClient.addTrnalterEmplyeePeriodQuntityAsync(newalter);
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                }
                                else
                                    clsMessages.setMessage(Properties.Resources.SaveFail);
                            }
                            else
                            {
                                MessageBox.Show(item.rule_name + " not existing for the selected time period");
                            }
                        }
                        else
                        {
                            MessageBox.Show(item.rule_name + "rule is already run the pay process for selected time period.");
                        }
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
        }

        private bool newCanExecute()
        {
            return true;
        }

        private void newRecord()
        {
            CurrentEmplopyee = null;
            CurrentPeriod = null;
            //CurrentQuantityPeriod = null;
            CurrentDepatment = null;
            CurrentSection = null;
            QuantityPeriods = null;
            Employees = TempEmployees;
        }

        private bool saveCanExecute()
        {
            if (CurrentEmplopyee != null && CurrentPeriod != null)
            {
                if (CurrentEmplopyee.employee_id == null || CurrentEmplopyee.employee_id == Guid.Empty)
                    return false;
                if (CurrentPeriod.period_id == null || CurrentPeriod.period_id == Guid.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        private void saveRecode()
        {
            //bool isUpdate = false;

            if (CurrentEmplopyee != null && CurrentPeriod != null)
            {
                foreach (EmployeePeriodQantityView Epqv in QuantityPeriods)
                {
                    List<trns_EmployeePeriodQunatity> temp = this.serviceClient.GetTrnsPeriodQuanity(CurrentEmplopyee.employee_id, Epqv.rule_id, CurrentPeriod.period_id).ToList();
                    if (temp.Count == 0)
                    {
                        try
                        {
                            if (Epqv != null)
                            {
                                if (clsSecurity.GetSavePermission(511))
                                {
                                    trns_EmployeePeriodQunatity newEmpquantity = new trns_EmployeePeriodQunatity();
                                    newEmpquantity.employee_id = Epqv.employee_id;
                                    newEmpquantity.rule_id = Epqv.rule_id;
                                    newEmpquantity.period_id = CurrentPeriod.period_id;
                                    newEmpquantity.quantity = Epqv.Quantity;
                                    newEmpquantity.is_proceed = false;
                                    newEmpquantity.save_datetime = System.DateTime.Now;
                                    newEmpquantity.save_user_id = clsSecurity.loggedUser.user_id;
                                    newEmpquantity.modified_datetime = System.DateTime.Now;
                                    newEmpquantity.modified_user_id = clsSecurity.loggedUser.user_id;
                                    newEmpquantity.delete_datetime = System.DateTime.Now;
                                    newEmpquantity.delete_user_id = clsSecurity.loggedUser.user_id;
                                    newEmpquantity.isdelete = false;

                                    if (this.serviceClient.saveTransPeriodQuantity(newEmpquantity))
                                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                                    else
                                        clsMessages.setMessage(Properties.Resources.SaveFail); 
                                }
                                else
                                    clsMessages.setMessage("You don't have permission to Save this record(s)");
                            }
                        }

                        catch (Exception)
                        {

                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    }
                    else
                    {
                        MessageBox.Show(Epqv.rule_name.ToString() + " already exist for selected time period.");
                    }
                }
            }
        }

        #endregion

        #region Refresh Methods

        private void refreshEmployees()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(z => z.isdelete == false && z.isActive == true);
                foreach (EmployeeSumarryView emp in Employees)
                {
                    TempEmployees.Add(emp);
                }
            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }

        private void refreshperiods()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
                {
                    this.Priods = e.Result.Where(z => z.isdelete == false);
                };
            this.serviceClient.GetPeriodsAsync();
        }

        private void refreshEmployeeRulesByEmployeeId()
        {
            this.serviceClient.GetEmnployeePeriodQuantityCompleted += (s, e) =>
                {
                    this.QuantityPeriods = e.Result;
                };
            this.serviceClient.GetEmnployeePeriodQuantityAsync(CurrentEmplopyee.employee_id);
        }

        private void refreshDepatments()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Department = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void refreshSections()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
                {
                    if (CurrentDepatment != null)
                        this.Sections = e.Result.Where(z => z.department_id.Equals(CurrentDepatment.department_id) && z.isdelete == false);
                    else
                        this.Sections = e.Result;
                };
            this.serviceClient.GetSectionsAsync();
        }

        #endregion

        private void fillDataForGrid()
        {
            List<trns_EmployeePeriodQunatity> oldTrnce = new List<trns_EmployeePeriodQunatity>();

            if (CurrentPeriod != null && CurrentEmplopyee != null)
            {
                try
                {
                    this.serviceClient.GetTrnsEmployeePeriodQuntityByEmployeeIDAndPeriodCompleted += (s, e) =>
                               {
                                   oldTrnce = e.Result.ToList();

                                   if (oldTrnce.Count != 0)
                                   {
                                       foreach (trns_EmployeePeriodQunatity trnItem in oldTrnce)
                                       {
                                           try
                                           {
                                               foreach (EmployeePeriodQantityView item in QuantityPeriods)
                                               {
                                                   if (item.employee_id == trnItem.employee_id && item.rule_id == trnItem.rule_id)
                                                   {
                                                       item.Quantity = (Decimal)trnItem.quantity;
                                                   }
                                               }
                                           }
                                           catch (Exception)
                                           {

                                               // throw;
                                           }
                                       }
                                   }
                               };
                    this.serviceClient.GetTrnsEmployeePeriodQuntityByEmployeeIDAndPeriodAsync(CurrentEmplopyee.employee_id, CurrentPeriod.period_id);
                }
                catch (Exception)
                {


                }
            }
        }

        private void filter()
        {
            Employees = TempEmployees;

            if (CurrentDepatment != null)
                Employees = Employees.Where(e => e.department_id.Equals(CurrentDepatment.department_id));
            if (CurrentSection != null)
                Employees = Employees.Where(e => e.section_id.Equals(CurrentSection.section_id));
        }
    }
}
