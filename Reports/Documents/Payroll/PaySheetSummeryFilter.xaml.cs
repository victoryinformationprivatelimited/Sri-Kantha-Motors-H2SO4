using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for PaySheetSummeryFilter.xaml
    /// </summary>
    public partial class PaySheetSummeryFilter : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();

        public PaySheetSummeryFilter()
        {
            InitializeComponent();
            GetPeriodList();
            addFieldToList();

        }

        private void button1_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            string reportQuary = "";
           
           
                if (PaymetPeriod.SelectedItem != null)
                {
                    if (reportQuary == "")
                    {
                        reportQuary = "" + databasefield[0] + " = '" + PaymetPeriod.SelectedItem + "'";
                    }
                    else
                    {
                        reportQuary += "AND" + databasefield[0] + " = '" + PaymetPeriod.SelectedItem + "'";
                    }
                }
            
                ReportViewer rep = new ReportViewer();
                rep.ReportLoad("Summery Reports", "PaySheet", reportQuary, "");
                rep.Show();
            

        }
        public void GetPeriodList()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                foreach (z_Period itemperiod in e.Result)
                {
                    PaymetPeriod.Items.Add(itemperiod.period_name.ToString());
                }
            };
            this.serviceClient.GetPeriodsAsync();

        }
        #region Add database FieldS to List
        public void addFieldToList()
        {
            
            databasefield.Add("{Employee_Deduction_Benifit_Sumarry_Union.period_name}");
          

        }
        #endregion

        private void button1_Copy1_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
