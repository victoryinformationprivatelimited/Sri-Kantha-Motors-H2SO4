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

namespace ERP.Loan_Module.Basic_Masters
{
    /// <summary>
    /// Interaction logic for LoanProcessWindow.xaml
    /// </summary>
    public partial class LoanProcessWindow : Window
    {
        LoanProcessViewModel viewModel = new LoanProcessViewModel();
        public LoanProcessWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void loan_process_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loan_process_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
