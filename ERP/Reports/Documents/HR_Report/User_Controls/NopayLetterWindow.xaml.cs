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
using CustomBusyBox;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for NopayLetterWindow.xaml
    /// </summary>
    public partial class NopayLetterWindow : Window
    {
        NopayLetterViewModel viewModel;

        public NopayLetterWindow()
        {
            BusyBox.InitializeThread(this);
            InitializeComponent();

            viewModel = new NopayLetterViewModel();
            Loaded += (s, e) =>
            {
                DataContext = viewModel;
            };
        }

        private void close_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
