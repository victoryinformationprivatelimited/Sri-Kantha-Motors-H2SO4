using CustomBusyBox;
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

namespace ERP.Payroll.Excel_Sheet_Windows
{
    /// <summary>
    /// Interaction logic for ExcelUploadedSlipTransferWindow.xaml
    /// </summary>
    public partial class ExcelUploadedSlipTransferWindow : Window
    {
        ExcelUploadedSlipTransferViewModel viewModel;
        public ExcelUploadedSlipTransferWindow()
        {
            BusyBox.InitializeThread(this);
            viewModel = new ExcelUploadedSlipTransferViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
