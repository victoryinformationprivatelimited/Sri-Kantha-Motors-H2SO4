using ERP.HelperClass;
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
	/// Interaction logic for PaymentMethodWindow1.xaml
	/// </summary>
	public partial class PaymentMethodWindow : Window
	{
		public PaymentMethodViewModel viewModel;
        public PaymentMethodWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new PaymentMethodViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void paymethod_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void paymethod_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void paymethod_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}