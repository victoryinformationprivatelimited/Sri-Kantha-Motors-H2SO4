using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Payroll.Employee_Bonus
{
    class BonusPeriodViewModel: ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region constrouctor
        public BonusPeriodViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_BonusPeriod> _BonusPeriod;
        public IEnumerable<z_BonusPeriod> BonusPeriod
        {
            get { return _BonusPeriod; }
            set { _BonusPeriod = value; OnPropertyChanged("BonusPeriod"); }
        }

        private z_BonusPeriod _CurrentBonusPeriod;
        public z_BonusPeriod CurrentBonusPeriod
        {
            get { return _CurrentBonusPeriod; }
            set { _CurrentBonusPeriod = value; OnPropertyChanged("CurrentBonusPeriod"); if (CurrentBonusPeriod.Bonus_Period_id > 0) setData(); }
        }

        private IEnumerable<z_EmployeeOtherPayments> _OtherPayments;
        public IEnumerable<z_EmployeeOtherPayments> OtherPayments
        {
            get { return _OtherPayments; }
            set { _OtherPayments = value; OnPropertyChanged("OtherPayments"); }
        }

        private z_EmployeeOtherPayments _CurrentEmployeeOtherPayment;
        public z_EmployeeOtherPayments CurrentEmployeeOtherPayment
        {
            get { return _CurrentEmployeeOtherPayment; }
            set { _CurrentEmployeeOtherPayment = value; OnPropertyChanged("CurrentEmployeeOtherPayment"); }
        }
        #endregion

        #region Refresh Methods

        private void RefreshBonusPeriod()
        {
            serviceClient.GetBonusPeriodCompleted += (s, e) =>
                {
                    BonusPeriod = e.Result;
                };
            serviceClient.GetBonusPeriodAsync();
        }

        private void RefreshEmployeeOtherPaymentsCategories()
        {
            serviceClient.GetEmployeeOtherPaymentsCategoriesCompleted += (s, e) =>
            {
                OtherPayments = e.Result;
            };
            serviceClient.GetEmployeeOtherPaymentsCategoriesAsync();
        }
        #endregion

        #region Button Commands
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }


        #endregion

        #region Methods
        private void New()
        {
            RefreshBonusPeriod();
            RefreshEmployeeOtherPaymentsCategories();
            CurrentBonusPeriod = new z_BonusPeriod();
        }

        private void setData()
        {
            CurrentEmployeeOtherPayment = OtherPayments.FirstOrDefault(c => c.other_payment_id == CurrentBonusPeriod.other_payment_id);
        }

        private void Save()
        {
            if (ValidateSave())
            {
                if (CurrentBonusPeriod.Bonus_Period_id == 0)
                {
                    if (clsSecurity.GetSavePermission(517))
                    {
                        clsMessages.setMessage("Are You Sure You Want to Save Bonus Period?", Visibility.Visible);
                        if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                        {
                            CurrentBonusPeriod.save_datetime = DateTime.Now;
                            CurrentBonusPeriod.save_user_id = clsSecurity.loggedUser.user_id;
                            CurrentBonusPeriod.isactive = true;
                            CurrentBonusPeriod.isdelete = false;
                            if (serviceClient.SaveBounsPeriod(CurrentBonusPeriod,CurrentEmployeeOtherPayment))
                            {
                                clsMessages.setMessage("Bonus Period Saved Successfully...");
                                New();
                            }
                            else
                                clsMessages.setMessage("Bonus Period Saved Failed...");
                        } 
                    }
                    else
                        clsMessages.setMessage("You don't have permission to save this record");
                }
                else
                {
                    if (clsSecurity.GetUpdatePermission(517))
                    {
                        clsMessages.setMessage("Are You Sure You Want to Update Bonus Period Description?", Visibility.Visible);
                        if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                        {
                            CurrentBonusPeriod.modified_datetime = DateTime.Now;
                            CurrentBonusPeriod.modified_user_id = clsSecurity.loggedUser.user_id;
                            if (serviceClient.UpdateBonusPeriod(CurrentBonusPeriod,CurrentEmployeeOtherPayment))
                            {
                                clsMessages.setMessage("Bonus Period Update Successfully...");
                                New();
                            }
                            else
                                clsMessages.setMessage("Bonus Period Update Failed...");
                        } 
                    }
                    else
                        clsMessages.setMessage("You don't have permission to update this record");
                } 
            }
        }        
        private bool ValidateSave()
        {
            if (CurrentBonusPeriod == null)
            {
                clsMessages.setMessage("Please Enter Period Name Before You Click The Save Button...");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_Name == null || CurrentBonusPeriod.Bonus_Period_Name == string.Empty)
            {
                clsMessages.setMessage("Please Enter Period Name Before You Click The Save Button...");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_Start_Date == null)
            {
                clsMessages.setMessage("Please Select Period Start Date Before You Click The Save Button...");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_End_Date == null)
            {
                clsMessages.setMessage("Please Select Period End Date Before You Click The Save Button...");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_Start_Date > CurrentBonusPeriod.Bonus_Period_End_Date)
            {
                clsMessages.setMessage("End Date Cannot Be Less than Start Date...");
                return false;
            }
            else if (CurrentEmployeeOtherPayment == null)
            {
                clsMessages.setMessage("Please Select a Bonus Payment Type...");
                return false;
            }
            else
                return true;
        }
        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(517))
            {
                if (ValidateDelete())
                {
                    clsMessages.setMessage("Are You Sure You Want to Delete Bonus Period?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        CurrentBonusPeriod.delete_datetime = DateTime.Now;
                        CurrentBonusPeriod.delete_user_id = clsSecurity.loggedUser.user_id;
                        CurrentBonusPeriod.isdelete = true;
                        CurrentBonusPeriod.isactive = false;
                        if (serviceClient.DeleteBonusPeriod(CurrentBonusPeriod))
                        {
                            clsMessages.setMessage("Bonus Period Successfully Deleted...");
                            New();
                        }
                        else
                            clsMessages.setMessage("Bonus Period Deleted Failed...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");
        }
        private bool ValidateDelete()
        {
            if (CurrentBonusPeriod == null)
            {
                clsMessages.setMessage("Please Select Bonus Period Before Clicking Delete Button...");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_id == 0)
            {
                clsMessages.setMessage("Please Select Bonus Period Before Clicking Delete Button...");
                return false;
            }
            else
                return true;


        }

        #endregion
    }
}
