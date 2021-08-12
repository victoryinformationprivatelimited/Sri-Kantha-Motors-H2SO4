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

namespace ERP.MastersDetails
{
    /// <summary>
    /// Interaction logic for EmployeeLeaveDetailUserControl.xaml
    /// </summary>
    public partial class EmployeeLeaveDetailUserControl : UserControl
    {
        private EmployeeLeaveDetailViewModel viewModel = new EmployeeLeaveDetailViewModel();
        public EmployeeLeaveDetailUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Emp Id");
            SearchCombo.Items.Add("Emp Name");
            SearchCombo.Items.Add("Leave Detail");
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

    }
}
