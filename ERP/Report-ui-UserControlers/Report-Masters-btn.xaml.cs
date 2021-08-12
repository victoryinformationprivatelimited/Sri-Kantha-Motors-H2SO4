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

namespace ERP.Report_ui_UserControlers
{
    /// <summary>
    /// Interaction logic for Report_Masters_btn.xaml
    /// </summary>
    public partial class Report_Masters_btn : UserControl
    {
        public Report_Masters_btn()
        {
            InitializeComponent();
        }

        private void Section_Check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1401))
            {
                ReportViewer rptviewersec = new ReportViewer();
                rptviewersec.ReportLoad("BasicReports", "SectionReport", "", "");
                rptviewersec.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Section_Check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1401))
            {
                ReportViewer rptviewersec = new ReportViewer();
                rptviewersec.ReportLoad("BasicReports", "SectionReport", "", "");
                rptviewersec.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Town_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1402))
            {
                ReportViewer rptviewertown = new ReportViewer();
                rptviewertown.ReportLoad("BasicReports", "Town", "", "");
                rptviewertown.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Town_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1402))
            {
                ReportViewer rptviewertown = new ReportViewer();
                rptviewertown.ReportLoad("BasicReports", "Town", "", "");
                rptviewertown.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Grade_Check_Box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1403))
            {
                ReportViewer rptviewer = new ReportViewer();
                rptviewer.ReportLoad("BasicReports", "GradeReport", "", "");
                rptviewer.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Grade_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1403))
            {
                ReportViewer rptviewer = new ReportViewer();
                rptviewer.ReportLoad("BasicReports", "GradeReport", "", "");
                rptviewer.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Designation_Check_Box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1404))
            {
                ReportViewer rptviewerdesi = new ReportViewer();
                rptviewerdesi.ReportLoad("BasicReports", "DesignationReport", "", "");
                rptviewerdesi.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Designation_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1404))
            {
                ReportViewer rptviewerdesi = new ReportViewer();
                rptviewerdesi.ReportLoad("BasicReports", "DesignationReport", "", "");
                rptviewerdesi.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Departments_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1405))
            {
                ReportViewer rptviewerdept = new ReportViewer();
                rptviewerdept.ReportLoad("BasicReports", "EmployeeDepartmentReport", "", "");
                rptviewerdept.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Departments_Check_Box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1405))
            {
                ReportViewer rptviewerdept = new ReportViewer();
                rptviewerdept.ReportLoad("BasicReports", "EmployeeDepartmentReport", "", "");
                rptviewerdept.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void City_Check_Box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1406))
            {
                ReportViewer rptviewerbranch = new ReportViewer();
                rptviewerbranch.ReportLoad("BasicReports", "City", "", "");
                rptviewerbranch.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void City_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1406))
            {
                ReportViewer rptviewerbranch = new ReportViewer();
                rptviewerbranch.ReportLoad("BasicReports", "City", "", "");
                rptviewerbranch.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Hr_Checkbox_two_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Hr_Checkbox_two_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Basic_Unit_Box_Copy_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1407))
            {
                ReportViewer rptviewerbranch = new ReportViewer();
                rptviewerbranch.ReportLoad("BasicReports", "BasicUnit", "", "");
                rptviewerbranch.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Basic_Unit_Box_Copy_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1407))
            {
                ReportViewer rptviewerbranch = new ReportViewer();
                rptviewerbranch.ReportLoad("BasicReports", "BasicUnit", "", "");
                rptviewerbranch.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }
    }
}
