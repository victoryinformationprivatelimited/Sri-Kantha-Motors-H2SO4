﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.MastersDetails
{
    /// <summary>
    /// Interaction logic for CompanyVariableCompanyRuleDetail.xaml
    /// </summary>
    public partial class CompanyVariableCompanyRuleDetail : UserControl
    {
        CompanyVariableRulesViewModel viewModel = new CompanyVariableRulesViewModel();

        public CompanyVariableCompanyRuleDetail()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    
    }
}
