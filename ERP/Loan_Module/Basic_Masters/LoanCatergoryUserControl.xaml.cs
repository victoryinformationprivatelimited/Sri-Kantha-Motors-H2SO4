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

namespace ERP.Loan_Module.Basic_Masters
{
    /// <summary>
    /// Interaction logic for LoanCatergoryUserControl.xaml
    /// </summary>
    public partial class LoanCatergoryUserControl : UserControl
    {
        private LoanCatergoryViewModel viewModel;
        public LoanCatergoryUserControl()
        {
            InitializeComponent();
            viewModel = new LoanCatergoryViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
