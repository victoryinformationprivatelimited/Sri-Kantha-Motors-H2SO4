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
    /// Interaction logic for ApproveLeavesWindow.xaml
    /// </summary>
    public partial class ApproveLeavesWindow : Window
    {
        ApproveLeaveViewModel viewModel = new ApproveLeaveViewModel();

        public ApproveLeavesWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
            SearchCombo.Items.Add("Emp ID");
            SearchCombo.Items.Add("Emp Name");
            SearchCombo.Items.Add("Leave Category");
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
