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

namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for ApproveLeaves.xaml
    /// </summary>
    public partial class ApproveLeaves : UserControl
    {
        ApproveLeaveViewModel viewModel = new ApproveLeaveViewModel();

        public ApproveLeaves()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
            SearchCombo.Items.Add("Emp ID");
            SearchCombo.Items.Add("Emp Name");
            SearchCombo.Items.Add("Leave Category");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
