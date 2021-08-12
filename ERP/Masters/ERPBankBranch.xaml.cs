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
using ERP.Masters;

namespace ERP
{
    /// <summary>
    /// Interaction logic for ERPBankBranch.xaml
    /// </summary>
    public partial class ERPBankBranch : UserControl
    {
        ERPBankBranchViewModel viewModel = new ERPBankBranchViewModel();
        public ERPBankBranch()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
