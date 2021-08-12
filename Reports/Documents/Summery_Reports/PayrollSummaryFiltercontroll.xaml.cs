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
    /// Interaction logic for PayrollSummaryFiltercontroll.xaml
    /// </summary>
    public partial class PayrollSummaryFiltercontroll : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public PayrollSummaryFiltercontroll()
        {
            InitializeComponent();
            GetPeriodList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            addFieldToList();
            GetCompanyBranchList();
            CompanyBranch.Items.Add("All");
        }

        private void button1_Copy1_Click_1(object sender, RoutedEventArgs e)
        {
            PaymetPeriod.SelectedItem=null;
            combo_Department.SelectedItem=null;
            combo_Designation.SelectedItem = null;
            combo_Section.SelectedItem = null;
            combo_Grade.SelectedItem = null;
        }

        private void button1_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            //string fileterby = "Filter By : ";
            string reportQuary = "";
            if (PaymetPeriod.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[0] + " = '" + PaymetPeriod.SelectedItem + "'";
                    //fileterby += " " + "Department";
                }
                else
                {
                    reportQuary += "AND" + databasefield[0] + " = '" + PaymetPeriod.SelectedItem + "'";
                    //fileterby += " , " + "Department";
                }
            }
            //if (combo_Designation.SelectedItem != null)
            //{
            //    if (reportQuary == "")
            //    {
            //        reportQuary = "" + databasefield[3] + " = '" + combo_Designation.SelectedItem + "'";
            //        //fileterby += "" + "Designation";
            //    }
            //    else
            //    {
            //        reportQuary += " AND " + databasefield[3] + " = '" + combo_Designation.SelectedItem + "'";
            //        //fileterby += " , " + "Designation";
            //    }
            //}
            //if (combo_Section.SelectedItem != null)
            //{
            //    if (reportQuary == "")
            //    {
            //        reportQuary = "" + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
            //        //fileterby += "" + "Section";
            //    }
            //    else
            //    {
            //        reportQuary += " AND " + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
            //        //fileterby += " , " + "Section";
            //    }
            //}
            //if (combo_Grade.SelectedItem != null)
            //{
            //    if (reportQuary == "")
            //    {
            //        reportQuary = "" + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
            //        //fileterby += "" + "Grade";
            //    }
            //    else
            //    {
            //        reportQuary += " AND " + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
            //        //fileterby += " , " + "Grade";
            //    }
            //}
          
            ReportViewer rep = new ReportViewer();
            rep.ReportLoad("Summery Reports", "SummaryReport", reportQuary, "");
            rep.Show();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {

        }

        #region Add database FieldS to List
        public void addFieldToList()
        {
            databasefield.Add("{Employee_Deduction_Benifit_Sumarry_Union.period_name}");
            databasefield.Add("{EmployeeSumarryView.grade}");
            databasefield.Add("{EmployeeSumarryView.section_name}");
            databasefield.Add("{EmployeeSumarryView.designation}");
            databasefield.Add("{EmployeeSumarryView.city}");
            databasefield.Add("{EmployeeSumarryView.town_name}");
            databasefield.Add("{EmployeeSumarryView.join_date}");

        }
        #endregion


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

        #region Department List
        public void GetDepartmentList()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                foreach (z_Department itemdept in e.Result)
                {
                    combo_Department.Items.Add(itemdept.department_name.ToString());
                }
            };
            this.serviceClient.GetDepartmentsAsync();

        }
        #endregion

        #region Section List
        public void GetSectionList()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                foreach (z_Section itemsec in e.Result)
                {
                    combo_Section.Items.Add(itemsec.section_name.ToString());
                }
            };
            this.serviceClient.GetSectionsAsync();

        }
        #endregion

        #region Grade List
        public void GetGradeList()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                foreach (z_Grade itemgrade in e.Result)
                {
                    combo_Grade.Items.Add(itemgrade.grade.ToString());
                }
            };
            this.serviceClient.GetGradeAsync();

        }
        #endregion
    }
}
