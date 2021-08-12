using ERP.ERPService;
using ERP.HelperClass;
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

namespace ERP.Reports.Documents.Advance_Report
{
    /// <summary>
    /// Interaction logic for DeductionUserControll.xaml
    /// </summary>
    public partial class DeductionUserControll : UserControl

    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();

        public DeductionUserControll()
        {
            InitializeComponent();
            GetPeriodList();
            GetGradeList();
            GetDepartmentList();
            GetSectionList();
            GetDesignationList();
            GetBenifitReport();
            addFieldToList();
        }
        public Guid GetRuleID(reportname rptname)
        {
            return HelperClass.clsReportData.GetReportID(rptname);
        }

        private void clr(object sender, RoutedEventArgs e)
        {

        }

        private void print_rep(object sender, RoutedEventArgs e)
        {
             if (combo_benifit.SelectedItem != null && PaymetPeriod.SelectedItem != null)
            {
                string Benifitreportname = combo_benifit.SelectedItem.ToString();
                string banifitrule = Benifitreportname.Replace(" ", "");
                reportname myname = (reportname)Enum.Parse(typeof(reportname), banifitrule, true);
                Guid ruleid = GetRuleID(myname);

                string reportQuary = "";
                reportQuary = "" + databasefield[0] + "='" + ruleid + "'" + "AND" + databasefield[1] + " = '" + PaymetPeriod.SelectedItem + "'";

                if (combo_Department.SelectedItem != null)
                {
                    if (departmentcheck.IsChecked == true)
                    {
                        reportQuary += "AND" + databasefield[2] + " = ''";
                    }
                    else
                    {

                        reportQuary += "AND" + databasefield[2] + " = '" + combo_Department.SelectedItem + "'";

                    }
                }
                if (combo_Designation.SelectedItem != null)
                {
                    if (designationcheck.IsChecked == true)
                    {
                        reportQuary += "AND" + databasefield[4] + " = ''";
                    }

                    else
                    {
                        reportQuary += " AND " + databasefield[4] + " = '" + combo_Designation.SelectedItem + "'";
                    }
                }
                if (combo_Section.SelectedItem != null)
                {
                    if (sectioncheck.IsChecked == true)
                    {
                        reportQuary += "AND" + databasefield[3] + " = ''";
                    }

                    else
                    {
                        reportQuary += " AND " + databasefield[3] + " = '" + combo_Section.SelectedItem + "'";
                    }
                }

                if (combo_Grade.SelectedItem != null)
                {
                    if (gradecheck.IsChecked == true)
                    {
                        reportQuary += "AND" + databasefield[5] + " = ''";
                    }
                    else
                    {
                        reportQuary += " AND " + databasefield[5] + " = '" + combo_Grade.SelectedItem + "'";
                    }
                }

                ReportViewer rep = new ReportViewer();
                rep.ReportLoad("Advance Report", banifitrule, reportQuary, "");
                rep.Show();
            }
            else
            {
                clsMessages.setMessage("Please Select The payment Period ");
            }
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

         public void GetBenifitReport()
        {
            this.serviceClient.GetReportDataForBenifitUserControllCompleted += (s, e) =>
            {
                foreach (Report_Details_View itembenifit in e.Result.Where(c=> c.isDeduct==true))
                {
                    combo_benifit.Items.Add(itembenifit.rpt_name.ToString());
                }
            };
            this.serviceClient.GetReportDataForBenifitUserControllAsync();

        }

       
         public void addFieldToList()
         {
             databasefield.Add("{Rpt_DeductionForSingleReport.rule_id}");
             databasefield.Add("{Rpt_DeductionForSingleReport.period_name}");
             databasefield.Add("{Rpt_DeductionForSingleReport.department_name}");
             databasefield.Add("{Rpt_DeductionForSingleReport.second_name}");
             databasefield.Add("{Rpt_DeductionForSingleReport.designation}");
             databasefield.Add("{Rpt_DeductionForSingleReport.grade}");
           

         }
    
    }
}
