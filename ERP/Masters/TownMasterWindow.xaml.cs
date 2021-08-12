using ERP.HelperClass;
using ERP.Masters;
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
	/// Interaction logic for TownMasterWindow1.xaml
	/// </summary>
	public partial class TownMasterWindow : Window
	{
        private TownMasterViewModel viewModel;
        public TownMasterWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new TownMasterViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void town_master_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void town_master_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void town_master_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}