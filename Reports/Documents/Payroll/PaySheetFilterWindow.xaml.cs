using ERP.ERPService;
using ERP.HelperClass;
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

namespace ERP
{
	
	public partial class PaySheetFilterWindow : Window
	{
		private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();

        public PaySheetFilterWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            addFieldToList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            //GetCityList();
            //GetTownList();
            GetPeriodList();
            CompanyBranch.Items.Add("All");
            PaymetPeriod.Items.Add("All");
            GetCompanyBranchList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string reportQuary = "";
            if (PaymetPeriod.SelectedItem != null)
            {
                if (PaymetPeriod.SelectedItem.ToString() == "All")
                {
                    reportQuary = "";
                }
                else
                {
                    reportQuary = "" + databasefield[9] + "='" + PaymetPeriod.SelectedItem +"'";
                }

            }
            if (combo_Department.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "AND" + databasefield[0] + " = '" + combo_Department.SelectedItem + "'";
                }
                else
                {
                    reportQuary += "AND" + databasefield[0] + " = '" + combo_Department.SelectedItem + "'";
                }
            }
            if (combo_Designation.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[3] + " = '" + combo_Designation.SelectedItem + "'";
                }
                else
                {
                    reportQuary += " AND " + databasefield[3] + " = '" + combo_Designation.SelectedItem + "'";
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
            if (CompanyBranch.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    if (CompanyBranch.SelectedItem.ToString()=="All")
                    {
                        //reportQuary = "" + databasefield[4] + " = '" + CompanyBranch.SelectedItem + "'";
                    }
                    else
                    {
                        reportQuary = "" + databasefield[4] + " = '" + CompanyBranch.SelectedItem + "'";
                    }
                }
                else
                {
                    if (CompanyBranch.SelectedItem.ToString()== "All")
                    {
                    }
                    else
                    {
                        reportQuary += " AND " + databasefield[4] + " = '" + CompanyBranch.SelectedItem + "'";
                    }
                }
            }
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
            rep.ReportLoad("Payroll", "EmployeePaySheet", reportQuary,"");
            rep.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //combo_Town.SelectedItem = null;
            combo_Section.SelectedItem = null;
            combo_Grade.SelectedItem = null;
            combo_Designation.SelectedItem = null;
            combo_Department.SelectedItem = null;
           // combo_City.SelectedItem = null;
            PaymetPeriod.SelectedItem = null;

        }

        #region Add database FieldS to List
        public void addFieldToList()
        {
            databasefield.Add("{Employee_Salary_Deatails_View.department_name}");
            databasefield.Add("{Employee_Salary_Deatails_View.grade}");
            databasefield.Add("{Employee_Salary_Deatails_View.section_name}");
            databasefield.Add("{Employee_Salary_Deatails_View.designation}");
            databasefield.Add("{Employee_Salary_Deatails_View.companyBranch_Name}");
            databasefield.Add("{Employee_Salary_Deatails_View.town_name}");
            databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
            databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
            databasefield.Add("{Employee_Salary_Deatails_View.end_date}");
            databasefield.Add("{Employee_Salary_Deatails_View.period_name}");
            
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

        #region Designation List
        public void GetDesignationList()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                foreach (z_Designation itemdesig in e.Result)
                {
                    combo_Designation.Items.Add(itemdesig.designation.ToString());
                }
            };
            this.serviceClient.GetDesignationsAsync();

        }
        #endregion

        #region City List
        //public void GetCityList()
        //{
        //    this.serviceClient.GetCitiesCompleted += (s, e) =>
        //    {
        //        foreach (z_City itemcity in e.Result)
        //        {
        //            combo_City.Items.Add(itemcity.city.ToString());
        //        }
        //    };
        //    this.serviceClient.GetCitiesAsync();

        //}
        #endregion

        #region Town List
        //public void GetTownList()
        //{
        //    this.serviceClient.GetTownDetailsCompleted += (s, e) =>
        //    {
        //        foreach (z_Town itemtown in e.Result)
        //        {
        //            combo_Town.Items.Add(itemtown.town_name.ToString());
        //        }
        //    };
        //    this.serviceClient.GetCitiesAsync();

        //}

        #endregion

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

        private void paysheet_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void paysheet_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void paysheet_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}