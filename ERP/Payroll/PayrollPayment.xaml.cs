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

namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for PayrollPayment.xaml
    /// </summary>
    public partial class PayrollPayment : UserControl
    {
        PayrollPaymentViewModel viewmodel = new PayrollPaymentViewModel();

        public PayrollPayment()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //ReportViewer newRep = new ReportViewer();
            //newRep.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void searchbox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
    }
}
