using ERP.Performance;
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

namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for ModifyLeavesUserControl.xaml
    /// </summary>
    public partial class ModifyLeavesUserControl : UserControl
    {
        ModifyLeavesViewModel ViewModel;

        public ModifyLeavesUserControl()
        {
            InitializeComponent();
            ViewModel = new ModifyLeavesViewModel();

            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
