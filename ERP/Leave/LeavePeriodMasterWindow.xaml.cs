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
using System.Windows.Shapes;

namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for LeavePeriodMasterWindow.xaml
    /// </summary>
    public partial class LeavePeriodMasterWindow : Window
    {
        private LeavePeriodMasterViewModel viewModel = new LeavePeriodMasterViewModel();

        public LeavePeriodMasterWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Period Name");
            SearchCombo.Items.Add("From Date");
            SearchCombo.Items.Add("To Date");
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
