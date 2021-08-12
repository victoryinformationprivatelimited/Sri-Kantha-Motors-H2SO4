using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using ERP.Properties;

namespace ERP.Payroll.Employee_Bonus
{
    class OtherPaymentsCategoriesViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region constrouctor
        public OtherPaymentsCategoriesViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Properties

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
        #endregion

        #region Methods
        private void New()
        {
            RefreshEmployeeOtherPaymentsCategories();
            CurrentEmployeeOtherPayment = new z_EmployeeOtherPayments();
        }
        private void Save()
        {
            if (ValidateSave())
            {
                if (CurrentEmployeeOtherPayment.other_payment_id == 0)
                {
                    clsMessages.setMessage("Are You Sure You Want to Save Employee Other Payments Categories?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (serviceClient.SaveEmployeeOtherPaymentsCategories(CurrentEmployeeOtherPayment))
                        {
                            clsMessages.setMessage("Employee Other Payments Categories Saved Successfully...");
                            New();
                        }
                        else
                            clsMessages.setMessage("Employee Other Payments Categories Saved Failed...");
                    }
                }
                else
                {
                    clsMessages.setMessage("Are You Sure You Want to Update Employee Other Payments Categories?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (serviceClient.UpdateEmployeeOtherPaymentsCategories(CurrentEmployeeOtherPayment))
                        {
                            clsMessages.setMessage("Employee Other Payments Categories Update Successfully...");
                            New();
                        }
                        else
                            clsMessages.setMessage("Employee Other Payments Categories Update Failed...");
                    }
                }
            }
        }
        private bool ValidateSave()
        {
            if (CurrentEmployeeOtherPayment == null)
            {
                clsMessages.setMessage("Please Enter Category Name Before You Click The Save Button...");
                return false;
            }
            else if (CurrentEmployeeOtherPayment.other_payment_name == null || CurrentEmployeeOtherPayment.other_payment_name == string.Empty)
            {
                clsMessages.setMessage("Please Enter Category Name Before You Click The Save Button...");
                return false;
            }
            else
                return true;
        }

        #endregion
    }
}
