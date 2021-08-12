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

namespace ERP.Loan_Module.Detail_Masters
{
    /// <summary>
    /// Interaction logic for EmployeeLoansWindow.xaml
    /// </summary>
    public partial class EmployeeLoansWindow : Window
    {
        private EmployeeLoansViewModel viewModel;
        public EmployeeLoansWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new EmployeeLoansViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };


        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}
