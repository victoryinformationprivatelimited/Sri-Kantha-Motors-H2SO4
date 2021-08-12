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
    /// Interaction logic for UserLevelMaster.xaml
    /// </summary>
    public partial class UserLevelMaster : UserControl
    {

        UserLevelMasterViewModel viewModel = new UserLevelMasterViewModel();

        public UserLevelMaster()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };

        }

        private void Searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //UserLevelSearch.Focus();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
