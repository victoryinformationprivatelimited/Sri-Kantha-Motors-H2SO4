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

namespace ERP.Security
{
    /// <summary>
    /// Interaction logic for xxxxx.xaml
    /// </summary>
    public partial class UserEmployeeUserControl : UserControl
    {
        UserEmployeeViewModel viewModel;

        public UserEmployeeUserControl()
        {
            InitializeComponent();
            viewModel = new UserEmployeeViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
