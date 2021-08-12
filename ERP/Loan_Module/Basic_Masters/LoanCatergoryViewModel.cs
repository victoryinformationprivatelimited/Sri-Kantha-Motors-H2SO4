using ERP.ERPService;
using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Basic_Masters
{
    class LoanCatergoryViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<z_LoanCatergories> list = new List<z_LoanCatergories>();
        #endregion

        #region Constructor
        public LoanCatergoryViewModel()
        {
            serviceClient = new ERPServiceClient();
            refreshLoanCatergories();
            New();
        }
        #endregion

        #region Properties
        private IEnumerable<z_LoanCatergories> loanCatergories;
        public IEnumerable<z_LoanCatergories> LoanCatergories
        {
            get { return loanCatergories; }
            set { loanCatergories = value; OnPropertyChanged("LoanCatergories"); }
        }

        private z_LoanCatergories cuurentLoanCatergory;
        public z_LoanCatergories CurrentLoanCatergory
        {
            get { return cuurentLoanCatergory; }
            set { cuurentLoanCatergory = value; OnPropertyChanged("CurrentLoanCatergory"); }
        }

        private string search;
        public string Search
        {
            get { return search; }
            set
            {
                search = value; OnPropertyChanged("Search");
                if (Search == null)
                    refreshLoanCatergories();
                else
                    searchTextChanged();
            }
        }

        #endregion

        #region New Method
        public void New()
        {
            CurrentLoanCatergory = null;
            CurrentLoanCatergory = new z_LoanCatergories();
            try
            {
                CurrentLoanCatergory.loan_no = this.serviceClient.GetLastLoanCatergoryNo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region New Button Class & Property
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
        public void Save()
        {
            MessageBoxResult Result = new MessageBoxResult();
            Result = MessageBox.Show("Do You Want To Save This Record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                if (CurrentLoanCatergory.loan_Catergory_id == null || CurrentLoanCatergory.loan_Catergory_id == Guid.Empty)
                {
                    CurrentLoanCatergory.loan_Catergory_id = Guid.NewGuid();
                }
                try
                {
                    bool IsUpdate = false;
                    z_LoanCatergories newLoanCatergory = new z_LoanCatergories();
                    newLoanCatergory.loan_Catergory_id = CurrentLoanCatergory.loan_Catergory_id;
                    newLoanCatergory.loan_no = CurrentLoanCatergory.loan_no;
                    newLoanCatergory.loan_catergory_name = CurrentLoanCatergory.loan_catergory_name;
                    newLoanCatergory.loan_description = CurrentLoanCatergory.loan_description;
                    newLoanCatergory.is_active = CurrentLoanCatergory.is_active;
                    newLoanCatergory.is_delete = false;

                    IEnumerable<z_LoanCatergories> allLoanCatergories = serviceClient.GetLoanCatergories();
                    if (allLoanCatergories != null)
                    {
                        foreach (var item in allLoanCatergories)
                        {
                            if (item.loan_Catergory_id == CurrentLoanCatergory.loan_Catergory_id)
                            {
                                IsUpdate = true;
                                break;
                            }
                        }
                    }
                    if (newLoanCatergory != null && newLoanCatergory.loan_Catergory_id != null)
                    {
                        if (IsUpdate)
                        {
                            newLoanCatergory.modified_user_id = clsSecurity.loggedUser.user_id;
                            newLoanCatergory.modified_datetime = System.DateTime.Now;
                            if (clsSecurity.GetUpdatePermission(601))
                            {
                                if (serviceClient.UpdateLoanCatergories(newLoanCatergory))
                                {
                                    // MessageBox.Show("Record Updated Successfully...");
                                    clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                    refreshLoanCatergories();
                                }
                                else
                                {
                                    //  MessageBox.Show("Record Update Failed...");
                                    clsMessages.setMessage(Properties.Resources.UpdateFail);
                                }
                            }
                            else
                                clsMessages.setMessage("You Don't Have Permission to Update in this form...");
                        }
                        else
                        {
                            newLoanCatergory.save_user_id = clsSecurity.loggedUser.user_id;
                            newLoanCatergory.save_datetime = System.DateTime.Now;
                            if (clsSecurity.GetSavePermission(601))
                            {
                                if (serviceClient.SaveLoanCatergories(newLoanCatergory))
                                {

                                    // MessageBox.Show("Record Saved Successfully...");
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                    refreshLoanCatergories();
                                    New();
                                }
                                else
                                {
                                    // MessageBox.Show("Record Save Failed...");
                                    clsMessages.setMessage(Properties.Resources.SaveFail);
                                }
                            }
                            else
                                clsMessages.setMessage("You Don't Have Permission to Save in this form...");
                        }
                    }
                }

                catch (Exception ex)
                {
                    clsMessages.setMessage(ex.Message);
                }
            }

        }
        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentLoanCatergory == null || CurrentLoanCatergory.loan_catergory_name == null || CurrentLoanCatergory.loan_catergory_name == String.Empty)
                return false;
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
        public void Delete()
        {
            try
            {
                if (clsSecurity.GetDeletePermission(601))
                {
                    MessageBoxResult Result = new MessageBoxResult();
                    Result = MessageBox.Show("Do You Want To Delete This Record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Result == MessageBoxResult.Yes)
                    {
                        CurrentLoanCatergory.delete_user_id = clsSecurity.loggedUser.user_id;
                        CurrentLoanCatergory.delete_datetime = System.DateTime.Now;

                        if (serviceClient.DeleteLoanCatergory(CurrentLoanCatergory))
                        {
                            // MessageBox.Show("Record Deleted Successfully...");
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            refreshLoanCatergories();
                        }
                        else
                        {
                            MessageBox.Show("Record Delete Failed...", "Loan Module Says", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            clsMessages.setMessage(Properties.Resources.DeleteFail);
                        }
                    }
                }
                else
                    clsMessages.setMessage("You Don't Have Permission to Delete in this form...");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Delete Button Class & Property
        bool deleteCanExecute()
        {
            if (CurrentLoanCatergory == null || CurrentLoanCatergory.loan_Catergory_id == null || CurrentLoanCatergory.loan_Catergory_id == Guid.Empty)
                return false;
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }
        #endregion

        #region Refresh Methods
        private void refreshLoanCatergories()
        {
            list.Clear();
            serviceClient.GetLoanCatergoriesCompleted += (s, e) =>
                {
                    LoanCatergories = e.Result;
                    if (LoanCatergories != null)
                        list = LoanCatergories.ToList();
                };
            serviceClient.GetLoanCatergoriesAsync();
        }
        #endregion

        #region Search Method
        private void searchTextChanged()
        {
            LoanCatergories = null;
            LoanCatergories = list;
            LoanCatergories = LoanCatergories.Where(p => p.loan_catergory_name.ToUpper().Contains(Search.ToUpper()));
        }
        #endregion
    }
}
