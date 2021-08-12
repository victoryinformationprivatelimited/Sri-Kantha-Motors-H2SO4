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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    /// <summary>
    /// Interaction logic for EPFETFGenerateExcelWindow.xaml
    /// </summary>
    public partial class EPFETFGenerateExcelWindow : Window
    {
        EPFETFGenerateExcelViewModel ViewModel;
        public EPFETFGenerateExcelWindow()
        {
            BusyBox.InitializeThread(this);
            InitializeComponent();
            ViewModel = new EPFETFGenerateExcelViewModel();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }
    }
}
