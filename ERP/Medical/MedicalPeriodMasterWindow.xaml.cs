using ERP.HelperClass;
using ERP.Medical;
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
	/// Interaction logic for MedicalPeriodMasterWindow1.xaml
	/// </summary>
	public partial class MedicalPeriodMasterWindow : Window
	{
        private MedicalPeriodMasterViewModel viewModel;
        public MedicalPeriodMasterWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new MedicalPeriodMasterViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void mediacal_period_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void mediacal_period_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void mediacal_period_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}