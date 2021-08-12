using ERP.Masters;
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

namespace ERP.Attendence
{
    /// <summary>
    /// Interaction logic for BasicDayMasterUserControl.xaml
    /// </summary>
    public partial class BasicDayMasterUserControl : UserControl
    {
        private BasicDayMasterViewModel viewModel = new BasicDayMasterViewModel();
        public BasicDayMasterUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void Basic_Day_Master_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
