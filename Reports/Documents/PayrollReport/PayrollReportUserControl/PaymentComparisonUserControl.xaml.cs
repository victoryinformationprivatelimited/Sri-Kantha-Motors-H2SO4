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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    /// <summary>
    /// Interaction logic for PaymentComparisonUserControl.xaml
    /// </summary>
    public partial class PaymentComparisonUserControl : UserControl
    {
        PaymentComparisonViewModel viewModel;
        public PaymentComparisonUserControl()
        {
            this.viewModel = new PaymentComparisonViewModel();
            Loaded += (s, e) =>
            {
                DataContext = viewModel;
            };
            InitializeComponent();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
