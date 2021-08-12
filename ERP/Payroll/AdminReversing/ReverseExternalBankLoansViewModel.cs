using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Payroll.AdminReversing
{
    class ReverseExternalBankLoansViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        #endregion

        #region Properties
        private IEnumerable<z_Period> _Periods;

        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }
        
        #endregion

        #region Constructor
        public ReverseExternalBankLoansViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshPeriods();
        }
        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                    {
                        Periods = e.Result.OrderBy(c => c.start_date);
                    };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand btnReverse
        {
            get
            {
                return new RelayCommand(Reverse);
            }
        }

        private void Reverse()
        {
            try
            {
                trns_ReverseLog trnsReverse = new trns_ReverseLog();
                trnsReverse.reverse_category = CurrentPeriod.period_name + " External";
                trnsReverse.reverse_user_id = clsSecurity.loggedUser.user_id;
                trnsReverse.reverse_datetime = System.DateTime.Now;
                List<trns_ExtBankLoan> Exist = serviceClient.CheckToReverseExternalBankLoans(CurrentPeriod.period_id).ToList();
                if (Exist.Count > 0)
                {
                    if (serviceClient.SaveReverseLog(trnsReverse))
                    {
                        if (serviceClient.ReverseExternalBankLoans(CurrentPeriod.period_id))
                        {
                            clsMessages.setMessage(CurrentPeriod.period_name + " Reverse Successfully Completed");
                        }
                        else
                        {
                            clsMessages.setMessage(CurrentPeriod.period_name + " Reverse Failed");
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("Reverse Log File Saving Failed");
                    } 
                }
                else
                {
                    clsMessages.setMessage(CurrentPeriod.period_name + " is Already Reversed or Yet to Process");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error in Reversing");
            }
        }
        #endregion
    }
}
