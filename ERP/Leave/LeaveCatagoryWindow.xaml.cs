﻿using ERP.Masters;
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

namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for LeaveCatagoryWindow.xaml
    /// </summary>
    public partial class LeaveCatagoryWindow : Window
    {
        private LeaveCatergoryViewModel viewModel = new LeaveCatergoryViewModel();

        public LeaveCatagoryWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("Leave Name");
            SearchCombo.Items.Add("Leave Code");
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
