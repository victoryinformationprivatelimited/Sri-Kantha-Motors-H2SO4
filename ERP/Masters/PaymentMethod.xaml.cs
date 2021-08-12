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

namespace ERP
{
    /// <summary>
    /// Interaction logic for PaymentMethod.xaml
    /// </summary>
    public partial class PaymentMethod : UserControl
    {
        public PaymentMethodViewModel viewModel = new PaymentMethodViewModel();
        public PaymentMethod()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void TextBox_TextChanged_5(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
