using ERP.Reports;
using ERP.Reports.Documents.BasicReports;
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
    /// Interaction logic for Report_Main_btn.xaml
    /// </summary>
    public partial class Report_Main_btn : UserControl
    {
        
         WrapPanel SubWrap;
        public Report_Main_btn(ReportMaster Reportmster)
        {
            SubWrap = Reportmster.MdiMainReport;
            InitializeComponent();
        }
    
        private void Admin_check_box_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportControllManager rcm = new ReportControllManager();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(rcm);
            }
            catch (Exception)
            {

            }

        }

        private void Admin_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportControllManager rcm = new ReportControllManager();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(rcm);
            }
            catch (Exception)
            {

            }
        }

        private void HR_check_box_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportControllManager rcm = new ReportControllManager();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(rcm);
            }
            catch (Exception)
            {

            }
                   
           
        }
           

        private void HR_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
           
        }

        private void Inventory_check_box_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Inventory_check_box_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Sales_Check_box_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Sales_Check_box_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Masters_check_box_Checked(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Report_Masters_btn rm = new Report_Masters_btn();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(rm);
            }
            catch (Exception)
            {

            }
        
        }

        private void Masters_check_box_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
