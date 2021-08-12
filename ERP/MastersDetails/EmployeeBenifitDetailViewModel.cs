using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.ComponentModel;
using System.Resources;
using System.Windows.Input;

namespace ERP
{
    public class EmployeeBenifitDetailViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient = new ERPServiceClient();

        public EmployeeBenifitDetailViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.Period), clsSecurity.loggedUser.user_id))
            {
                this.refreshEmployee();
                this.refreshbenifits();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return this._Employees; }
            set { this._Employees = value; OnPropertyChanged("Employees"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee 
        {
            get { return this._CurrentEmployee; }
            set { this._CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); refreshEmployeeBenifits();}
        }

        private IEnumerable<mas_Benifit> _Benifits;
        public IEnumerable<mas_Benifit> Benifits
        {
            get { return this._Benifits; }
            set { this._Benifits = value; }
        }

        private mas_Benifit _CurrentBenifits;
        public mas_Benifit CurrentBenifits
        {
            get { return this._CurrentBenifits; }
            set { this._CurrentBenifits = value; OnPropertyChanged("CurrentBenifits");}
        }
        
        private IEnumerable<EmployeeBinifitView> _EmployeeBenifits;
        public IEnumerable<EmployeeBinifitView> EmployeeBenifits
        {
            get { return this._EmployeeBenifits; }
            set { this._EmployeeBenifits = value; OnPropertyChanged("EmployeeBenifits"); }
        }

        private EmployeeBinifitView _CurrentEmployeeBenifit;
        public EmployeeBinifitView CurrentEmployeeBenifit
        {
            get { return this._CurrentEmployeeBenifit; }
            set { this._CurrentEmployeeBenifit = value; OnPropertyChanged("CurrentEmployeeBenifit");  }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(save, saveCanExecute);
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
                    return new RelayCommand(delete, deleteCanexecute);
                }          
        }

        void save()
        {
            try
            {
                CurrentEmployeeBenifit.save_datetime = System.DateTime.Now;
                CurrentEmployeeBenifit.save_user_id = clsSecurity.loggedUser.user_id;
                CurrentEmployeeBenifit.modified_datetime = System.DateTime.Now;
                CurrentEmployeeBenifit.modified_user_id = clsSecurity.loggedUser.user_id;
                CurrentEmployeeBenifit.delete_datetime = System.DateTime.Now;
                CurrentEmployeeBenifit.delete_user_id = clsSecurity.loggedUser.user_id;
                
                if (this.CurrentEmployeeBenifit != null)
                {
                    if (this.serviceClient.SaveEmployeeBenifits(CurrentEmployeeBenifit))
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                    else
                        clsMessages.setMessage(Properties.Resources.SaveFail);
                }
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
            finally
            {
                this.refreshEmployeeBenifits();
                this.refreshbenifits();
            }
        }

        bool saveCanExecute()
        {
            if (CurrentEmployeeBenifit != null)
            {
                if (CurrentEmployeeBenifit.benifit_id == null || CurrentEmployeeBenifit.benifit_id == Guid.Empty)
                    return false;
                if (CurrentEmployeeBenifit.employee_id == null || CurrentEmployeeBenifit.employee_id == Guid.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        void newRecord()
        {
            this.CurrentEmployee = null;
            this.CurrentEmployeeBenifit = new EmployeeBinifitView();    
        
        }

        bool newCanExecute()
        {
            return true;
        }

        void delete()
        {
            try
            {
                if (this.CurrentEmployeeBenifit != null)
                {
                    if (this.serviceClient.DeleteEmplyeeBenifits(CurrentEmployeeBenifit))
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    else
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                }

            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
            finally 
            {
                this.refreshEmployeeBenifits();
            }
        }

        bool deleteCanexecute()
        {
            if (CurrentEmployeeBenifit == null)
            {
                return false;
            }
            return true;
        }

        private void refreshEmployee()
       {
           this.serviceClient.GetEmployeesCompleted += (s, e) =>
               {
                   this.Employees = e.Result;
               };
           this.serviceClient.GetEmployeesAsync();
       }

        private void refreshbenifits()
        {
            this.serviceClient.GetBenifitsCompleted += (s, e) =>
                {
                    this.Benifits = e.Result;
                };
            this.serviceClient.GetBenifitsAsync();
        }

        private void refreshEmployeeBenifits()
        {
            this.serviceClient.GetEmployeeBenifitByEmployeeCompleted += (s, e) =>
                {
                    this.EmployeeBenifits = e.Result;
                };
            this.serviceClient.GetEmployeeBenifitByEmployeeAsync(CurrentEmployee);
        }
    }
}
