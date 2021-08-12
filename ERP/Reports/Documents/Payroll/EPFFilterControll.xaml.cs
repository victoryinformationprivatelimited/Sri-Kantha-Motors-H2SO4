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

namespace ERP.Reports.Documents.Summery_Reports
{
    /// <summary>
    /// Interaction logic for EPFFilterControll.xaml
    /// </summary>
    public partial class EPFFilterControll : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public EPFFilterControll()
        {
            InitializeComponent();
            comboReportType.Items.Add("C Form");
            comboReportType.Items.Add("R 4");
            comboReportType.Items.Add("R 2");
            GetPeriodList();
            addFieldToList();
            GetCompanyBranchList();
            CompanyBranch.Items.Add("All");

        }

        private void clr(object sender, RoutedEventArgs e)
        {

        }

        private void print_rep(object sender, RoutedEventArgs e)
        {
             string reportQuary = "";
            //if (PaymetPeriod.SelectedItem != null)
            //{
            //    if (PaymetPeriod.SelectedItem !=null)
            //    {
            //        reportQuary = "";
            //    }
            //    else
            //    {
            //        reportQuary = "" + databasefield[0] + "='" + PaymetPeriod.SelectedItem +"'";
            //    }

            //}
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
                         reportQuary += " AND " + databasefield[1] + " = '" + CompanyBranch.SelectedItem + "'";
                     }
                 }
             }
             if (combo_Section.SelectedItem != null)
             {
                 if (reportQuary == "")
                 {
                     reportQuary = "" + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
                 }
                 else
                 {
                     reportQuary += " AND " + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
                 }
             }
             if (combo_Grade.SelectedItem != null)
             {
                 if (reportQuary == "")
                 {
                     reportQuary = "" + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
                 }
                 else
                 {
                     reportQuary += " AND " + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
                 }
             }
             //if (combo_City.SelectedItem != null)
             //{
             //    if (reportQuary == "")
             //    {
             //        reportQuary = "" + databasefield[4] + " = '" + combo_City.SelectedItem + "'";
             //    }
             //    else
             //    {
             //        reportQuary += " AND " + databasefield[4] + " = '" + combo_City.SelectedItem + "'";
             //    }
             //}
             //if (combo_Town.SelectedItem != null)
             //{
             //    if (reportQuary == "")
             //    {
             //        reportQuary = "" + databasefield[6] + " = '" + combo_Town.SelectedItem + "'";
             //    }
             //    else
             //    {
             //        reportQuary += " AND " + databasefield[6] + " = '" + combo_Town.SelectedItem + "'";
             //    }
             //}
             ReportViewer rep = new ReportViewer();
             if (comboReportType.SelectedIndex == 0)
             {

                 rep.ReportLoad("Payroll", "EPFContributionSheet", reportQuary, "");
                 rep.Show();
             }
             else if (comboReportType.SelectedIndex == 1)
             {

                 rep.ReportLoad("Payroll", "R4", reportQuary, "");
                 rep.Show();

             }
             else if (comboReportType.SelectedIndex == 2)
             {


                 rep.ReportLoad("Payroll", "R1", reportQuary, "");
                 rep.Show();
             }
             else
                 clsMessages.setMessage("Select the Report Type");
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

         private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {
             
         }
    }
}
