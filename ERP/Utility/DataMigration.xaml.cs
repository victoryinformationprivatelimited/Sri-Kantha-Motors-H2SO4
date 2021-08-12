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

namespace ERP.Utility
{
    /// <summary>
    /// Interaction logic for DataMigration.xaml
    /// </summary>
    public partial class DataMigration : UserControl
    {
        DataMigrationViewModel viewModel = new DataMigrationViewModel();

        public DataMigration()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
