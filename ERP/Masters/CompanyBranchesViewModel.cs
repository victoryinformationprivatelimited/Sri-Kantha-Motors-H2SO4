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
    class CompanyBranchesViewModel : ViewModelBase
    {
        #region Service

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion

        #region Constructor

        public CompanyBranchesViewModel()
        {

            this.reafreshCompanies();
            this.reafreshCities();
            this.reafreshCompanyBranches();
            this.reafreshTowns();
            this.reafreshCompanyBranch();
            this.reafreshBank();
            this.reafreshBankBranch();
            New();
        }

        #endregion

        #region Properties

        private IEnumerable<Company_Branch_View> _CompanyBranchViews;
        public IEnumerable<Company_Branch_View> CompanyBranchViews
        {
            get { return _CompanyBranchViews; }
            set { _CompanyBranchViews = value; this.OnPropertyChanged("CompanyBranchViews"); }
        }

        private Company_Branch_View _CurrentCompanyBranchView;
        public Company_Branch_View CurrentCompanyBranchView
        {
            get { return _CurrentCompanyBranchView; }
            set { _CurrentCompanyBranchView = value; this.OnPropertyChanged("CurrentCompanyBranchView"); }
        }

        private IEnumerable<z_CompanyBranches> _Branches;
        public IEnumerable<z_CompanyBranches> Branches
        {
            get
            {
                return this._Branches;
            }
            set
            {
                this._Branches = value;
                this.OnPropertyChanged("Branches");

            }
        }

        private z_CompanyBranches _CurrentBranch;
        public z_CompanyBranches CurrentBranch
        {
            get
            {
                return this._CurrentBranch;
            }
            set
            {
                this._CurrentBranch = value;
                this.OnPropertyChanged("Branches");

            }
        }

        private IEnumerable<z_Company> _Company;
        public IEnumerable<z_Company> Company
        {
            get
            {
                return this._Company;
            }
            set
            {
                this._Company = value;
                this.OnPropertyChanged("Company");
            }
        }

        private z_Company _CurrentCompany;
        public z_Company CurrentCompany
        {
            get
            {
                return this._CurrentCompany;
            }
            set
            {
                this._CurrentCompany = value;
                this.OnPropertyChanged("CurrentCompany");

            }
        }

        private IEnumerable<z_City> _Cities;
        public IEnumerable<z_City> Cities
        {
            get
            {
                return this._Cities;
            }
            set
            {
                this._Cities = value;
                this.OnPropertyChanged("Cities");
            }
        }

        private z_City _CurrenyCity;
        public z_City CurrentCity
        {
            get
            {
                return this._CurrenyCity;
            }
            set
            {
                this._CurrenyCity = value;
                this.OnPropertyChanged("CurrentCity");
                if (CurrentCity != null && Towns != null)
                {
                    Towns = Towns.Where(e => e.city_id == CurrentCity.city_id);
                }
                else
                {
                    clsMessages.setMessage("Please wait...");
                }
            }
        }

        private IEnumerable<z_Town> _Towns;
        public IEnumerable<z_Town> Towns
        {
            get
            {
                return this._Towns;
            }
            set
            {
                this._Towns = value;
                this.OnPropertyChanged("Towns");
            }
        }

        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get
            {
                return this._CurrentTown;
            }
            set
            {
                this._CurrentTown = value;
                this.OnPropertyChanged("CurrentTown");
            }
        }

        private IEnumerable<z_Bank> _Banks;
        public IEnumerable<z_Bank> Banks
        {
            get { return _Banks; }
            set { _Banks = value; this.OnPropertyChanged("Banks"); }
        }

        private z_Bank _CurrentBank;
        public z_Bank CurrentBank
        {
            get { return _CurrentBank; }
            set
            {
                _CurrentBank = value; this.OnPropertyChanged("CurrentBank");
                if (CurrentBank != null && BankBranch != null)
                {
                    BankBranch = BankBranch.Where(a => a.bank_id == CurrentBank.bank_id);
                }
                else
                {
                    clsMessages.setMessage("Plese wait...");
                }
            }
        }

        private IEnumerable<z_BankBranch> _BankBranch;
        public IEnumerable<z_BankBranch> BankBranch
        {
            get { return _BankBranch; }
            set { _BankBranch = value; this.OnPropertyChanged("BankBranch"); }
        }

        private z_BankBranch _CurrentBankBranch;
        public z_BankBranch CurrentBankBranch
        {
            get { return _CurrentBankBranch; }
            set { _CurrentBankBranch = value; this.OnPropertyChanged("CurrentBankBranch"); }
        }

        #endregion

        #region Button Methods

        #region New Button

        void New()
        {
            this.CurrentCompanyBranchView = null;
            CurrentCompanyBranchView = new Company_Branch_View();
            CurrentCompanyBranchView.companyBranch_id = Guid.NewGuid();
            reafreshCompanyBranch();
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New);
            }
        }

        #endregion

        #region Save Button

        void Save()
        {
            bool IsUpdate = false;
            z_CompanyBranches newCompanyBranch = new z_CompanyBranches();
            newCompanyBranch.companyBranch_id = CurrentCompanyBranchView.companyBranch_id;
            newCompanyBranch.company_id = CurrentCompanyBranchView.company_id;
            newCompanyBranch.city_id = CurrentCompanyBranchView.city_id;
            newCompanyBranch.town_id = CurrentCompanyBranchView.town_id;
            newCompanyBranch.bank_id = CurrentBank == null ? Guid.Empty : CurrentBank.bank_id;
            newCompanyBranch.bank_branch_id = CurrentBankBranch == null ? Guid.Empty : CurrentBankBranch.branch_id;
            newCompanyBranch.account_name = CurrentCompanyBranchView.account_name;
            newCompanyBranch.account_no = CurrentCompanyBranchView.account_no;
            newCompanyBranch.epf_registstion_no = CurrentCompanyBranchView.epf_registstion_no;
            newCompanyBranch.companyBranch_Name = CurrentCompanyBranchView.companyBranch_Name;
            newCompanyBranch.address_01 = CurrentCompanyBranchView.address_01;
            newCompanyBranch.address_02 = CurrentCompanyBranchView.address_02;
            newCompanyBranch.address_03 = CurrentCompanyBranchView.address_03;
            newCompanyBranch.telephone_01 = CurrentCompanyBranchView.telephone_01;
            newCompanyBranch.telephone_02 = CurrentCompanyBranchView.telephone_02;
            newCompanyBranch.Emp_Count = CurrentCompanyBranchView.Emp_Count;
            newCompanyBranch.Description = CurrentCompanyBranchView.Description;
            newCompanyBranch.save_user_id = Guid.Empty;
            newCompanyBranch.save_datetime = System.DateTime.Now;
            newCompanyBranch.modified_user_id = Guid.Empty;
            newCompanyBranch.modified_datetime = System.DateTime.Now;
            newCompanyBranch.delete_datetime = System.DateTime.Now;
            newCompanyBranch.delete_user_id = Guid.Empty;
            newCompanyBranch.isdelete = false;

            IEnumerable<z_CompanyBranches> allCompanyBranches = this.serviceClient.GetCompanyBranches();

            if (allCompanyBranches != null)
            {
                foreach (z_CompanyBranches CompanyBranch in allCompanyBranches)
                {
                    if (CompanyBranch.companyBranch_id == CurrentCompanyBranchView.companyBranch_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newCompanyBranch != null && newCompanyBranch.companyBranch_Name != null)
            {
                if (IsUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(204))
                    {
                        newCompanyBranch.modified_user_id = clsSecurity.loggedUser.user_id;
                        newCompanyBranch.modified_datetime = System.DateTime.Now;

                        if (ValidateEmpCount())
                        {
                            if (this.serviceClient.UpdateCompanyBranches(newCompanyBranch))
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                reafreshCompanyBranch();
                                New();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            } 
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
                else
                {
                    newCompanyBranch.delete_user_id = clsSecurity.loggedUser.user_id;
                    newCompanyBranch.delete_datetime = System.DateTime.Now;

                    if (clsSecurity.GetSavePermission(204))
                    {
                        if (ValidateEmpCount())
                        {
                            if (this.serviceClient.SaveCompanyBranches(newCompanyBranch))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                reafreshCompanyBranch();
                                New();
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            }
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Mention the Branch Name....!");
            }
            reafreshCompanyBranches();
        }

        bool saveCanExecute()
        {
            if (CurrentCompanyBranchView != null)
            {
                if (CurrentCompanyBranchView.companyBranch_id == null || CurrentCompanyBranchView.companyBranch_id == Guid.Empty)
                    return false;
                //if (CurrentCompanyBranchView.company_id == null || CurrentCompany.company_id == Guid.Empty)
                //    return false;
                if (CurrentCompanyBranchView.city_id == null || CurrentCompanyBranchView.city_id == Guid.Empty)
                    return false;
                if (CurrentCompanyBranchView.town_id == null || CurrentCompanyBranchView.town_id == Guid.Empty)
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

        #region Delete Button

        void Delete()
        {
            if (clsSecurity.GetDeletePermission(204))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do You Want to Delete This Record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                z_CompanyBranches oldbranch = new z_CompanyBranches();
                oldbranch.companyBranch_id = CurrentCompanyBranchView.companyBranch_id;
                oldbranch.delete_user_id = clsSecurity.loggedUser.user_id;
                oldbranch.delete_datetime = System.DateTime.Now;
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteCompanyBranches(oldbranch))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        reafreshCompanyBranch();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentCompanyBranchView == null)
            {
                return false;
            }
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

        #endregion

        #region Refresh Methods

        private void reafreshCompanyBranch()
        {
            this.serviceClient.GetCompanyBranchCompleted += (s, e) =>
            {
                this.CompanyBranchViews = e.Result;
            };
            this.serviceClient.GetCompanyBranchAsync();
        }

        private void reafreshCompanies()
        {
            this.serviceClient.GetCompaniesCompleted += (s, e) =>
            {
                this.Company = e.Result;
            };
            this.serviceClient.GetCompaniesAsync();
        }

        private void reafreshCities()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
            {
                this.Cities = e.Result;
            };
            this.serviceClient.GetCitiesAsync();
        }

        private void reafreshCompanyBranches()
        {
            this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                this.Branches = e.Result;
            };
            this.serviceClient.GetCompanyBranchesAsync();
        }

        private void reafreshTowns()
        {
            this.serviceClient.GetTownsCompleted += (s, e) =>
            {
                this.Towns = e.Result;
            };
            this.serviceClient.GetTownsAsync();
        }

        private void reafreshBank()
        {
            this.serviceClient.GetBanksCompleted += (s, e) =>
            {
                this.Banks = e.Result;
            };
            this.serviceClient.GetBanksAsync();
        }

        private void reafreshBankBranch()
        {
            this.serviceClient.GetBanckBranchCompleted += (s, e) =>
            {
                this.BankBranch = e.Result;
            };
            this.serviceClient.GetBanckBranchAsync();
        }

        #endregion

        #region Methods
        private int CalculateEmployeeCount()
        {
            int CompanyEmpCount = (int)(CurrentCompany.company_capacity == null ? 0 : CurrentCompany.company_capacity);
            int result = (int)(CompanyBranchViews == null ? 0 : CompanyBranchViews.Where(c=> c.companyBranch_id != CurrentCompanyBranchView.companyBranch_id).Sum(d => d.Emp_Count));
            int sum = 0;
            if (result == null || result == 0)
            {
                sum = CompanyEmpCount - 0;
            }
            else
            {
                sum = CompanyEmpCount - result;
            }
            return sum;
        }

        private bool ValidateEmpCount()
        {
            if (CurrentCompanyBranchView.Emp_Count > CalculateEmployeeCount())
            {
                clsMessages.setMessage("Employee count exceeds the total capacity by " + (CurrentCompanyBranchView.Emp_Count - Math.Abs(CalculateEmployeeCount())) + " please try again.");
                return false;
            }
            else
                return true;
        }
        #endregion
    }
}
