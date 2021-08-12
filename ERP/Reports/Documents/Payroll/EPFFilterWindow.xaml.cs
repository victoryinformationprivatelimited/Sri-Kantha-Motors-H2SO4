using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for EPFFilterWindow.xaml
    /// </summary>
    public partial class EPFFilterWindow : Window
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        //public List<string> databasefield = new List<string>();
        EPFContributionViewModel viewModel;
        public EPFFilterWindow()
        {
            InitializeComponent();
            viewModel = new EPFContributionViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
            //comboReportType.Items.Add("C Form");
            //comboReportType.Items.Add("R 4");
            //comboReportType.Items.Add("R 1");
            //GetPeriodList();
            //addFieldToList();
            //GetCompanyBranchList();
            //CompanyBranch.Items.Add("All");
        }

        //private void clr(object sender, RoutedEventArgs e)
        //{

        //}

        //private void print_rep(object sender, RoutedEventArgs e)
        //{
        //    string reportQuary = "";
        //    //if (PaymetPeriod.SelectedItem != null)
        //    //{
        //    //    if (PaymetPeriod.SelectedItem !=null)
        //    //    {
        //    //        reportQuary = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        reportQuary = "" + databasefield[0] + "='" + PaymetPeriod.SelectedItem +"'";
        //    //    }

        //    //}
        //    if (PaymetPeriod.SelectedItem != null)
        //    {
        //        if (reportQuary == "")
        //        {
        //            reportQuary = "" + databasefield[0] + " = '" + PaymetPeriod.SelectedItem + "'";
        //        }
        //        else
        //        {
        //            reportQuary += "AND" + databasefield[0] + " = '" + PaymetPeriod.SelectedItem + "'";
        //        }
        //    }
        //    if (CompanyBranch.SelectedItem != null)
        //    {
        //        if (reportQuary == "")
        //        {
        //            if (CompanyBranch.SelectedItem.ToString() == "All")
        //            {
        //            }
        //            else
        //            {

        //                reportQuary = "" + databasefield[1] + " = '" + CompanyBranch.SelectedItem + "'";
        //            }
        //        }
        //        else
        //        {
        //            if (CompanyBranch.SelectedItem.ToString() == "All")
        //            {
        //            }
        //            else
        //            {
        //                reportQuary += " AND " + databasefield[1] + " = '" + CompanyBranch.SelectedItem + "'";
        //            }
        //        }
        //    }
        //    if (combo_Section.SelectedItem != null)
        //    {
        //        if (reportQuary == "")
        //        {
        //            reportQuary = "" + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
        //        }
        //        else
        //        {
        //            reportQuary += " AND " + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
        //        }
        //    }
        //    if (combo_Grade.SelectedItem != null)
        //    {
        //        if (reportQuary == "")
        //        {
        //            reportQuary = "" + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
        //        }
        //        else
        //        {
        //            reportQuary += " AND " + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
        //        }
        //    }
        //    //if (combo_City.SelectedItem != null)
        //    //{
        //    //    if (reportQuary == "")
        //    //    {
        //    //        reportQuary = "" + databasefield[4] + " = '" + combo_City.SelectedItem + "'";
        //    //    }
        //    //    else
        //    //    {
        //    //        reportQuary += " AND " + databasefield[4] + " = '" + combo_City.SelectedItem + "'";
        //    //    }
        //    //}
        //    //if (combo_Town.SelectedItem != null)
        //    //{
        //    //    if (reportQuary == "")
        //    //    {
        //    //        reportQuary = "" + databasefield[6] + " = '" + combo_Town.SelectedItem + "'";
        //    //    }
        //    //    else
        //    //    {
        //    //        reportQuary += " AND " + databasefield[6] + " = '" + combo_Town.SelectedItem + "'";
        //    //    }
        //    //}
        //    ReportViewer rep = new ReportViewer();
        //    if (GcForm.IsChecked == true)
        //    {
        //        string path = "";
        //        path = "\\Reports\\Documents\\Payroll\\EPFContributionSheet";
        //        //rep.ReportLoad("Payroll", "EPFContributionSheet", reportQuary, "");
        //        //rep.Show();
        //        ReportPrint print = new ReportPrint(path);
        //        print.setParameterValue("@companybranchid", CompanyBranch.SelectedItem == null ? string.Empty : CompanyBranch.SelectedItem.ToString());
        //        print.setParameterValue("@periodid", PaymetPeriod.SelectedItem == null ? string.Empty : PaymetPeriod.SelectedItem.ToString());
        //        print.setParameterValue("@departmentid", combo_Department.SelectedItem == null ? string.Empty : combo_Department.SelectedItem.ToString());
        //        //print.setParameterValue("@designationid", CurrentDesignation == null ? string.Empty : CurrentDesignation.designation_id.ToString());
        //        //print.setParameterValue("@sectionid", CurrentSection == null ? string.Empty : CurrentSection.section_id.ToString());
        //        //print.setParameterValue("@gradeid", CurrentGrade == null ? string.Empty : CurrentGrade.grade_id.ToString());
        //        print.LoadToReportViewer();
        //    }
        //    else if (GR4.IsChecked == true)
        //    {

        //        rep.ReportLoad("Payroll", "R4", reportQuary, "");
        //        rep.Show();

        //    }
        //    else if (GR2.IsChecked == true)
        //    {


        //        rep.ReportLoad("Payroll", "R1", reportQuary, "");
        //        rep.Show();
        //    }
        //    else
        //        clsMessages.setMessage("Select the Report Type");

        //}
        //public void GetPeriodList()
        //{
        //    this.serviceClient.GetPeriodsCompleted += (s, e) =>
        //    {
        //        foreach (z_Period itemperiod in e.Result)
        //        {
        //            PaymetPeriod.Items.Add(itemperiod.period_name.ToString());
        //        }
        //    };
        //    this.serviceClient.GetPeriodsAsync();

        //}
        //#region Company Branch
        //public void GetCompanyBranchList()
        //{
        //    this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
        //    {
        //        foreach (z_CompanyBranches itemcity in e.Result)
        //        {
        //            CompanyBranch.Items.Add(itemcity.companyBranch_Name.ToString());
        //        }
        //    };
        //    this.serviceClient.GetCompanyBranchesAsync();

        //}
        //#endregion
        //#region Add database FieldS to List
        //public void addFieldToList()
        //{
        //    databasefield.Add("{EmployeeFundViiew.period_name}");
        //    databasefield.Add("{EmployeeFundViiew.companyBranch_Name}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.section_name}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.designation}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.city}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.town_name}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.end_date}");
        //    //databasefield.Add("{Employee_Salary_Deatails_View.period_name}");

        //}
        //#endregion

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}