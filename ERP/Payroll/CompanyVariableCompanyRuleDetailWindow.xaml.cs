using ERP.HelperClass;
using ERP.MastersDetails;
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
    /// Interaction logic for CompanyVariableCompanyRuleDetailWindow.xaml
    /// </summary>
    public partial class CompanyVariableCompanyRuleDetailWindow : Window
    {
        CompanyVariableRulesViewModel viewModel = new CompanyVariableRulesViewModel();
        public CompanyVariableCompanyRuleDetailWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void CompanyVariableCompanyRuleDetail_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CompanyVariableCompanyRuleDetail_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CompanyVariableCompanyRuleDetail_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
