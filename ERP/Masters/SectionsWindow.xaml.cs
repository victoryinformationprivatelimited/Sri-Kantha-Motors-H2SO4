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
	/// Interaction logic for SectionsWindow1.xaml
	/// </summary>
	public partial class SectionsWindow : Window
	{
		private SectionViewModel viewModel;
        public SectionsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new SectionViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void section_master_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void section_master_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void section_master_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}