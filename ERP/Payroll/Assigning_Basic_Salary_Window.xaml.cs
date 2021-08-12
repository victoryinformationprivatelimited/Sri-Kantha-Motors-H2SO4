using ERP.Payroll;
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
    /// Interaction logic for Assigning_Basic_Salary_Window.xaml
    /// </summary>
    public partial class Assigning_Basic_Salary_Window : Window
    {
        Assigning_Basic_SalaryViewModel viewModel = new Assigning_Basic_SalaryViewModel();
        public Assigning_Basic_Salary_Window()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };

            // Insert code required on object creation below this point.
        }

        private void assignDailyShiftTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}