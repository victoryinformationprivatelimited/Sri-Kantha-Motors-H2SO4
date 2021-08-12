using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Payroll.AdminReversing
{
    class ReversBonusAndOtherPaymentsViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        #endregion

        #region Properties
        private IEnumerable<z_BonusPeriod> _Periods;

        public IEnumerable<z_BonusPeriod> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_BonusPeriod _CurrentPeriod;

        public z_BonusPeriod CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }
        
        #endregion

        #region Constructor
        public ReversBonusAndOtherPaymentsViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshBonusPeriods();
        }
        #endregion

        #region Refresh Methods
        private void RefreshBonusPeriods()
        {
            try
            {
                serviceClient.GetBonusPeriodCompleted += (s, e) =>
                    {
                        Periods = e.Result;
                    };
                serviceClient.GetBonusPeriodAsync();
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
                trnsReverse.reverse_category = CurrentPeriod.Bonus_Period_Name;
                trnsReverse.reverse_user_id = clsSecurity.loggedUser.user_id;
                trnsReverse.reverse_datetime = System.DateTime.Now;
                List<trns_EmployeeBonus> Exist = serviceClient.CheckToReverseBonusAndOtherPayments(CurrentPeriod.Bonus_Period_id).ToList();
                if (Exist.Count > 0)
                {
                    if (serviceClient.SaveReverseLog(trnsReverse))
                    {
                        if (serviceClient.ReverseBonusAndOtherPayments(CurrentPeriod.Bonus_Period_id))
                        {
                            clsMessages.setMessage(CurrentPeriod.Bonus_Period_Name + " Reverse Successfully Completed");
                        }
                        else
                        {
                            clsMessages.setMessage(CurrentPeriod.Bonus_Period_Name + " Reverse Failed");
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("Reverse Log File Saving Failed");
                    } 
                }
                else
                {
                    clsMessages.setMessage(CurrentPeriod.Bonus_Period_Name + " is Already Reversed or Yet to Process");
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
