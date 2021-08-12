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
using ERP.ERPService;

namespace ERP.Reports
{
    /// <summary>
    /// Interaction logic for EmployeeSumarryReportFilterControll.xaml
    /// </summary>
    public partial class EmployeeSumarryReportFilterControll : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public EmployeeSumarryReportFilterControll()
        {
            InitializeComponent();
            addFieldToList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            GetCityList();
            GetTownList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string fileterby = "Filter By : ";
            string reportQuary = "";
            if (combo_Department.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[0] + " = '" + combo_Department.SelectedItem + "'";
                    fileterby += " " + "Department";
                }
                else
                {
                    reportQuary += "AND" + databasefield[0] + " = '" + combo_Department.SelectedItem + "'";
                    fileterby += " , " + "Department";
                }
            }
            if (combo_Designation.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[3] + " = '" + combo_Designation.SelectedItem + "'";
                    fileterby += "" + "Designation";
                }
                else
                {
                    reportQuary += " AND " + databasefield[3] + " = '" + combo_Designation.SelectedItem + "'";
                    fileterby += " , " + "Designation";
                }
            }
            if (combo_Section.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
                    fileterby += "" + "Section";
                }
                else
                {
                    reportQuary += " AND " + databasefield[2] + " = '" + combo_Section.SelectedItem + "'";
                    fileterby += " , " + "Section";
                }
            }
            if (combo_Grade.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
                    fileterby += "" + "Grade";
                }
                else
                {
                    reportQuary += " AND " + databasefield[1] + " = '" + combo_Grade.SelectedItem + "'";
                    fileterby += " , " + "Grade";
                }
            }
            if (combo_City.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[4] + " = '" + combo_City.SelectedItem + "'";
                    fileterby += "" +"City";
                }
                else
                {
                    reportQuary += " AND " + databasefield[4] + " = '" + combo_City.SelectedItem + "'";
                    fileterby += " , " + "City";
                }
            }
            if (combo_Town.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[6] + " = '" + combo_Town.SelectedItem + "'";
                    fileterby += "" +"Town";
                }
                else
                {
                    reportQuary += " AND " + databasefield[6] + " = '" + combo_Town.SelectedItem + "'";
                    fileterby += " , " + "Town";
                }
            }


            ReportViewer rep = new ReportViewer();
            rep.ReportLoad("Summery Reports", "EmployeeSumarryReport", reportQuary, fileterby);
            rep.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            combo_Town.SelectedItem = null;
            combo_Section.SelectedItem = null;
            combo_Grade.SelectedItem = null;
            combo_Designation.SelectedItem = null;
            combo_Department.SelectedItem = null;
            combo_City.SelectedItem = null;
            from_Date.SelectedDate = null;
            to_Date.SelectedDate = null;
        }
        #region Add database FieldS to List
        public void addFieldToList()
        {
            databasefield.Add("{EmployeeSumarryView.department_name}");
            databasefield.Add("{EmployeeSumarryView.grade}");
            databasefield.Add("{EmployeeSumarryView.section_name}");
            databasefield.Add("{EmployeeSumarryView.designation}");
            databasefield.Add("{EmployeeSumarryView.city}");
            databasefield.Add("{EmployeeSumarryView.town_name}");
            databasefield.Add("{EmployeeSumarryView.join_date}");

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
        public void GetCityList()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
            {
                foreach (z_City itemcity in e.Result)
                {
                    combo_City.Items.Add(itemcity.city.ToString());
                }
            };
            this.serviceClient.GetCitiesAsync();

        }
        #endregion

        #region Town List
        public void GetTownList()
        {
            this.serviceClient.GetTownDetailsCompleted += (s, e) =>
            {
                foreach (z_Town itemtown in e.Result)
                {
                    combo_Town.Items.Add(itemtown.town_name.ToString());
                }
            };
            this.serviceClient.GetCitiesAsync();

        }
        #endregion

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((Grid)this.Parent).Children.Remove(this);
        }
    }
}
