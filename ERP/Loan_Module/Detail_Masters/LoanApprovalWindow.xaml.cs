using ERP.HelperClass;
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

namespace ERP.Loan_Module.Detail_Masters
{
    /// <summary>
    /// Interaction logic for LoanApprovalWindow.xaml
    /// </summary>
    public partial class LoanApprovalWindow : Window
    {
        LoanApprovalViewModel viewModel;
        public LoanApprovalWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new LoanApprovalViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
     
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
