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

namespace ERP.Payroll.RI_Allowance
{
    /// <summary>
    /// Interaction logic for EmployeeAllowanceWindow.xaml
    /// </summary>
    public partial class EmployeeAllowanceWindow : Window
    {
        EmployeeAllowanceViewModel viewModel;
        public EmployeeAllowanceWindow()
        {
            InitializeComponent();
            viewModel = new EmployeeAllowanceViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void emp_allowance_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_allowance_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_allowance_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
