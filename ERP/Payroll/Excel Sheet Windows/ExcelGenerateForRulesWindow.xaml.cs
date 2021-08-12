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
    /// Interaction logic for ExcelGenerateForRulesWindow.xaml
    /// </summary>
    public partial class ExcelGenerateForRulesWindow : Window
    {
        ExcelGenerateForRulesViewModel ViewModel;
        public ExcelGenerateForRulesWindow()
        {
            BusyBox.InitializeThread(this);
            InitializeComponent();
            ViewModel = new ExcelGenerateForRulesViewModel();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }
    }
}
