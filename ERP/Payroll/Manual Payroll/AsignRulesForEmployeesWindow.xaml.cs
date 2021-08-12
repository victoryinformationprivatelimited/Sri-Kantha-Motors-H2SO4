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

namespace ERP.Payroll.ManualPayroll
{
    /// <summary>
    /// Interaction logic for AsignRulesForEmployeesWindow.xaml
    /// </summary>
    public partial class AsignRulesForEmployeesWindow : Window
    {
        AsignRulesForEmployeesViewModel ViewModel;
        public AsignRulesForEmployeesWindow()
        {
            InitializeComponent();
            ViewModel = new AsignRulesForEmployeesViewModel();
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void employee_rule_detail_close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void employee_rule_detail_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
