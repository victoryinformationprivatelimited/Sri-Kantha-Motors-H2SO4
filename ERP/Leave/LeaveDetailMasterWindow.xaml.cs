using ERP.MastersDetails;
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
    /// Interaction logic for LeaveDetailMasterWindow.xaml
    /// </summary>
    public partial class LeaveDetailMasterWindow : Window
    {
        private LeaveDetailMasterViewModel viewModel = new LeaveDetailMasterViewModel();

        public LeaveDetailMasterWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Category");
            SearchCombo.Items.Add("Period");
            SearchCombo.Items.Add("Leave Name");
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
