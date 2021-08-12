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

namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for EmployeePayRuleDetailsWindow.xaml
    /// </summary>
    public partial class EmployeePayRuleDetailsWindow : Window
    {
        EmployeePayRuleDetailsViewModel ViewModel;

        public EmployeePayRuleDetailsWindow()
        {
            InitializeComponent();
            ViewModel = new EmployeePayRuleDetailsViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel; 
            };
        }

        private void employee_payrule_close_btn_Click_1(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void employee_payrule_titlebar_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

       
    }
}
