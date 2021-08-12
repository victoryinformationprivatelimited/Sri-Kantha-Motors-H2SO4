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
    /// Interaction logic for UserMasterUserControl1.xaml
    /// </summary>
    public partial class UserMasterUserControl1 : UserControl
    {
        UserMasterViewModel viewModel = new UserMasterViewModel();
        public UserMasterUserControl1()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Password.Clear();
        }
    }
}
