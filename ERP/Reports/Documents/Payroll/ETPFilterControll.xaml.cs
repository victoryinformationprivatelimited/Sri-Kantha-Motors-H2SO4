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
    /// Interaction logic for ETPFilterControll.xaml
    /// </summary>
    public partial class ETPFilterControll : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();

        public ETPFilterControll()
        {
            InitializeComponent();
            GetPeriodList();
            addFieldToList();
            CompanyBranch.Items.Add("All");
            GetCompanyBranchList();
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
            if (CompanyBranch.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    if (CompanyBranch.SelectedItem.ToString() == "All")
                    {
                    }
                    else
                    {
                        reportQuary = "" + databasefield[1] + " = '" + CompanyBranch.SelectedItem + "'";
                    }
                }
                else
                {
                    if (CompanyBranch.SelectedItem.ToString() == "All")
                    {
                    }
                    else
                    {
                        reportQuary += "AND" + databasefield[1] + " = '" + CompanyBranch.SelectedItem + "'";
                    }
                }
            }

            ReportViewer rep = new ReportViewer();
            rep.ReportLoad("Payroll", "ETFReport", reportQuary, "");
            rep.Show();
        }

        private void button1_Copy1_Click_1(object sender, RoutedEventArgs e)
        {

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
        #region Company Branch
        public void GetCompanyBranchList()
        {
            this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                foreach (z_CompanyBranches itemcity in e.Result)
                {
                    CompanyBranch.Items.Add(itemcity.companyBranch_Name.ToString());
                }
            };
            this.serviceClient.GetCompanyBranchesAsync();

        }
        #endregion
        #region Add database FieldS to List
        public void addFieldToList()
        {
            databasefield.Add("{EmployeeFundViiew.period_name}");
            databasefield.Add("{EmployeeFundViiew.companyBranch_Name}");
            //databasefield.Add("{Employee_Salary_Deatails_View.section_name}");
            //databasefield.Add("{Employee_Salary_Deatails_View.designation}");
            //databasefield.Add("{Employee_Salary_Deatails_View.city}");
            //databasefield.Add("{Employee_Salary_Deatails_View.town_name}");
            //databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
            //databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
            //databasefield.Add("{Employee_Salary_Deatails_View.end_date}");
            //databasefield.Add("{Employee_Salary_Deatails_View.period_name}");

        }
        #endregion

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
