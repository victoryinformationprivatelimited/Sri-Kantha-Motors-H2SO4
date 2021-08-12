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

namespace ERP.MastersDetails
{
    /// <summary>
    /// Interaction logic for BankMasterDetail.xaml
    /// </summary>
    public partial class BankMasterDetail : UserControl
    {
        private BankMasterDetailViewModel viewModel = new BankMasterDetailViewModel();
        public BankMasterDetail()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
       
        private void searchbox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void Bank_Master_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void bank_master_detail_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void bank_master_detail_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void bank_master_detail_close_btn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
