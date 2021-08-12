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

namespace ERP.Leave.Holiday_Detail
{
    /// <summary>
    /// Interaction logic for BulkLeaveApplyWindow.xaml
    /// </summary>
    public partial class BulkLeaveApplyWindow : Window
    {
        BulkLeaveApplyViewModel viewModel = new BulkLeaveApplyViewModel();

        public BulkLeaveApplyWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
