using ERP.Attendence;
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
    /// Interaction logic for ShiftCatergoryMasterUserControl.xaml
    /// </summary>
    public partial class ShiftCatergoryMasterUserControl : UserControl
    {
        ShiftCatergoryViewModel viewModel = new ShiftCatergoryViewModel();
        public ShiftCatergoryMasterUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            SearchBox.Focus();
        }

        private void Shift_Categories_close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
