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
using ERP.Security;

namespace ERP.Security
{
    /// <summary>
    /// Interaction logic for ViewModelUserPermission.xaml
    /// </summary>
    public partial class ViewModelUserPermission : UserControl
    {
        ViewModelUserPermissionsViewModel viewModel = new ViewModelUserPermissionsViewModel();

        public ViewModelUserPermission()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
        private void searchbox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            //UserLevelSearch.Focus();
        }
    }
}
