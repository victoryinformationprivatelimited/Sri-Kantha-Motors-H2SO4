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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    /// <summary>
    /// Interaction logic for EPFeReturn.xaml
    /// </summary>
    public partial class EPFeReturn : Window
    {
        EPFeReturnViewModel viewModel;

        public EPFeReturn()
        {
            InitializeComponent();

            this.viewModel = new EPFeReturnViewModel();
            Loaded += (s, e) =>
            {
                DataContext = viewModel;
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
