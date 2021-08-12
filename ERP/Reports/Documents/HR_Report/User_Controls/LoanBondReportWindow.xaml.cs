﻿using ERP.HelperClass;
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

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for LoanBondReportWindow.xaml
    /// </summary>
    public partial class LoanBondReportWindow : Window
    {
        LoanBondReportViewModel viewModel;
        public LoanBondReportWindow()
        {
            viewModel = new LoanBondReportViewModel();
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
