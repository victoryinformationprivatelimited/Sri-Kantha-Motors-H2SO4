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

namespace ERP.Reports.Documents.NewReportFilters
{
    /// <summary>
    /// Interaction logic for PayrollSumarryUserControl.xaml
    /// </summary>
    public partial class PayrollSumarryUserControl : UserControl
    {
        //PayrollSumarryViewModle viewModel = new PayrollSumarryViewModle();
        public PayrollSumarryUserControl()
        {
            InitializeComponent();
         //   this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }
    }
}
