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
using System.Windows.Shapes;

namespace ERP.Payroll.AdminReversing
{
    /// <summary>
    /// Interaction logic for ReversBonusAndOtherPaymentsWindow.xaml
    /// </summary>
    public partial class ReversBonusAndOtherPaymentsWindow : Window
    {
        ReversBonusAndOtherPaymentsViewModel viewModel;
        public ReversBonusAndOtherPaymentsWindow()
        {
            viewModel = new ReversBonusAndOtherPaymentsViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
