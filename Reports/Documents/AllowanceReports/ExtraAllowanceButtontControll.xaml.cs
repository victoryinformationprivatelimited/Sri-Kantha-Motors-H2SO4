using ERP.Reports.Documents.AllowanceReports.AllowanceUserControls;
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

namespace ERP.Reports.Documents.AllowanceReports
{
    /// <summary>
    /// Interaction logic for ExtraAllowanceButtontControll.xaml
    /// </summary>
    public partial class ExtraAllowanceButtontControll : UserControl
    {
        public ExtraAllowanceButtontControll()
        {
            InitializeComponent();
        }

        private void _check_box_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void extra_allowance_check_box_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                AllowanceBasicUserControl extraallownce = new AllowanceBasicUserControl();
                DetailFilteringMDI.Children.Clear();
                DetailFilteringMDI.Children.Add(extraallownce);
            }
            catch (Exception)
            {

            }
        }

        private void extra_allowance_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                AllowanceBasicUserControl extraallownce = new AllowanceBasicUserControl();
                DetailFilteringMDI.Children.Clear();
                DetailFilteringMDI.Children.Add(extraallownce);
            }
            catch (Exception)
            {

            }
        }
    }
}
