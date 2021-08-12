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

namespace ERP.Payroll.RI_Allowance
{
    /// <summary>
    /// Interaction logic for AllowanceTransactionUserControl.xaml
    /// </summary>
    public partial class AllowanceTransactionUserControl : UserControl
    {
        AllowanceTransactionViewModel viewModel;
        public AllowanceTransactionUserControl()
        {
            InitializeComponent();
            viewModel = new AllowanceTransactionViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)Parent).Children.Clear();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

    }
}
