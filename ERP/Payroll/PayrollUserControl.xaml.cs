using ERP.MastersDetails;
using ERP.Payroll;
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
using ERP.UI_UserControlers;


namespace ERP
{
    /// <summary>
    /// Interaction logic for PayrollUserControl.xaml
    /// </summary>
    public partial class PayrollUserControl : UserControl
    {
        public PayrollUserControl()
        {
            InitializeComponent();
        }

        private void HR_Payaroll_Button_Loaded(object sender, RoutedEventArgs e)
        {
            PayrollButton Payrollbtn = new PayrollButton(this);
            Mdiwrappanel.Children.Add(Payrollbtn);
        }

       
    }
}
