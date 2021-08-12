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
    /// Interaction logic for LeaveCatagoryUserControl.xaml
    /// </summary>
    public partial class LeaveCatagoryUserControl : UserControl
    {
        private LeaveCatergoryViewModel viewModel = new LeaveCatergoryViewModel();
        public LeaveCatagoryUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Leave Name");
            SearchCombo.Items.Add("Leave Code");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

    }
}
