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
    /// Interaction logic for OTCatagoryUserControl.xaml
    /// </summary>
    public partial class OTCatagoryUserControl : UserControl
    {
        private OTCatagoryMasterViewModel viewModel = new OTCatagoryMasterViewModel();
        public OTCatagoryUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            SearchBox.Focus();
        }

      

        private void OT_Category_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
