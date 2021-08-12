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

namespace ERP.Reports.Documents.Payroll
{
    /// <summary>
    /// Interaction logic for SalarySheetFilterUserControll.xaml
    /// </summary>
    public partial class SalarySheetFilterUserControll : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();

        public SalarySheetFilterUserControll()
        {
            InitializeComponent();

            addFieldToList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            GetPeriodList();
            PaymetPeriod.Items.Add("All");
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
                    reportQuary = "" + databasefield[9] + "='" + PaymetPeriod.SelectedItem + "'";
                }

            }
            if (combo_Department.SelectedItem != null)
            {
                if (reportQuary == "")
                {
                    reportQuary = "" + databasefield[0] + " = '" + combo_Department.SelectedItem + "'";
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
           
            ReportViewer rep = new ReportViewer();
            rep.ReportLoad("Payroll", "EmployeeSalaryReport", reportQuary,"");
            rep.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            combo_Section.SelectedItem = null;
            combo_Grade.SelectedItem = null;
            combo_Designation.SelectedItem = null;
            combo_Department.SelectedItem = null;
            PaymetPeriod.SelectedItem = null;
        }

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

        #region Add database FieldS to List
        public void addFieldToList()
        {
            databasefield.Add("{Employee_Salary_Deatails_View.department_name}");
            databasefield.Add("{Employee_Salary_Deatails_View.grade}");
            databasefield.Add("{Employee_Salary_Deatails_View.section_name}");
            databasefield.Add("{Employee_Salary_Deatails_View.designation}");
            databasefield.Add("{Employee_Salary_Deatails_View.city}");
            databasefield.Add("{Employee_Salary_Deatails_View.town_name}");
            databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
            databasefield.Add("{Employee_Salary_Deatails_View.start_date}");
            databasefield.Add("{Employee_Salary_Deatails_View.end_date}");
            databasefield.Add("{Employee_Salary_Deatails_View.period_name}");

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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((Grid)this.Parent).Children.Remove(this);
        }

    }
}
