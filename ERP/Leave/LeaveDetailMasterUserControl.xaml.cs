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
    /// Interaction logic for LeaveDetailMasterUserControl.xaml
    /// </summary>
    public partial class LeaveDetailMasterUserControl : UserControl
    {
        private LeaveDetailMasterViewModel viewModel = new LeaveDetailMasterViewModel();
        public LeaveDetailMasterUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Category");
            SearchCombo.Items.Add("Period");
            SearchCombo.Items.Add("Leave Name");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

    }
}
