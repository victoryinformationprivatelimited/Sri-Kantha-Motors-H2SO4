using CustomBusyBox;
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

namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for EmployeePayrollProcessWindow.xaml
    /// </summary>
    public partial class EmployeePayrollProcessWindow : Window
    {
        int Step = 1;
        EmployeePayrollProcessViewModel viewModel = new EmployeePayrollProcessViewModel();
        public EmployeePayrollProcessWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            this.Loaded += (s, e) =>
            {
                this.DataContext = viewModel;
                EmployeeSelectionUserControl suc = new EmployeeSelectionUserControl(viewModel);
                /*this.Mdi.Children.Clear();
                this.Mdi.Children.Add(suc);*/
            };
        }

        /*private void Payaroll_process_Next_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (Step)
            {
                case 1:
                    EmployeePayrollViertification suc = new EmployeePayrollViertification(viewModel);
                    this.Mdi.Children.Clear();
                    this.Mdi.Children.Add(suc);
                    Step = 2;
                    break;
                case 2:
                    EmployeeFinalPayrollProcess pfp = new EmployeeFinalPayrollProcess(viewModel);
                    this.Mdi.Children.Clear();
                    this.Mdi.Children.Add(pfp);
                    break;
                default:
                    break;
            }
        }*/

        private void payroll_process_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void payroll_process_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void payroll_process_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Payaroll_process_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchbox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void searchbox_TextChanged_5(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
    }
}
