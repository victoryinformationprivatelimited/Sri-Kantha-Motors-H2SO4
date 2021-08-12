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

namespace ERP.Masters
{
    /// <summary>
    /// Interaction logic for LeavePeriodMasterUserControl.xaml
    /// </summary>
    public partial class LeavePeriodMasterUserControl : UserControl
    {
        private LeavePeriodMasterViewModel viewModel = new LeavePeriodMasterViewModel();
        public LeavePeriodMasterUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Period Name");
            SearchCombo.Items.Add("From Date");
            SearchCombo.Items.Add("To Date");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void Searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
    }
}
