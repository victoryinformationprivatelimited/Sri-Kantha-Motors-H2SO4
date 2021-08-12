using ERP.ERPService;
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
	/// Interaction logic for MedicalDetailsWindow1.xaml
	/// </summary>
	public partial class MedicalDetailsWindow : Window
	{
		private MedicalDetailsViewModel viewModel;
        
        public MedicalDetailsWindow()
        {
            viewModel = new MedicalDetailsViewModel();
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
        public MedicalDetailsWindow(EmployeeSumarryView CurrentEmployee)
        {
            viewModel = new MedicalDetailsViewModel(CurrentEmployee);
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
        
        private void emp_medic_details_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_medic_details_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_medic_details_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}