﻿using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP
{
	/// <summary>
	/// Interaction logic for UserLevelMasterWindow.xaml
	/// </summary>
	public partial class UserLevelMasterWindow : Window
	{
		UserLevelMasterViewModel viewModel = new UserLevelMasterViewModel();

        public UserLevelMasterWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
		}
	
		private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

	}
}