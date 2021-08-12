using ERP.Base;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP
{
    class ERPSplashViewModel : ViewModelBase
    {
        public ERPSplashViewModel()
        {
            Name();
            Rdays();
            GoogleDriveModel.GetInstance.getAuth();
        }

        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; OnPropertyChanged("CompanyName"); }
        }

        private void Name()
        {
            CompanyName = "Licensed to:" + " " + ConfigurationManager.AppSettings["Company_Name"];
        }
        private string remainigdays;
        public string Remainingdays
        {
            get { return remainigdays; }
            set { remainigdays = value; OnPropertyChanged("remainigdays"); }
        }

        public void Rdays()
        {
            string Decryptcode_new = StringCipher.Decrypt(ConfigurationManager.AppSettings["EXP_Key"], "12");
            DateTime expdate = DateTime.Parse(Decryptcode_new);

            // static string reamining = expdate - DateTime.Now;
            TimeSpan aa = expdate - DateTime.Now;
            if (aa.Days <= 31) {
                remainingDaysCheck(aa.Days);
            }
            //"Licensed to:" + " " + ConfigurationManager.AppSettings["Company_Name"];
        }

        public void remainingDaysCheck(int days) {
            if (Convert.ToInt32(days) < 0)
            {
                Remainingdays = "System Expires in " + 0.ToString() + " Days";
            }
            else
            {
                Remainingdays = "System Expires in " + days.ToString() + " Days";
            }
        }


    }
}
