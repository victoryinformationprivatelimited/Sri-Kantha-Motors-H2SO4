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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for EmployeePayrollProcess.xaml
    /// </summary>
    public partial class EmployeePayrollProcess : UserControl
    {
        int Step = 1;
        EmployeePayrollProcessViewModel viewModel = new EmployeePayrollProcessViewModel();
        public EmployeePayrollProcess()
        {
            InitializeComponent();            
            this.Loaded += (s, e) => { this.DataContext = viewModel;
                EmployeeSelectionUserControl suc= new EmployeeSelectionUserControl(viewModel);
            this.Mdi.Children.Clear();
            this.Mdi.Children.Add(suc);
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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void Payaroll_process_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
