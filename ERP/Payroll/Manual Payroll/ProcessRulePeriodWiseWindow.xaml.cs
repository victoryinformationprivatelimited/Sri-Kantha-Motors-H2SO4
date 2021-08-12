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

namespace ERP.Payroll.ManualPayroll
{
    /// <summary>
    /// Interaction logic for ProcessRulePeriodWiseWindow.xaml
    /// </summary>
    public partial class ProcessRulePeriodWiseWindow : Window
    {
        ProcessRulePeriodWiseViewModel ViewModel;
        public ProcessRulePeriodWiseWindow()
        {
            InitializeComponent();
            ViewModel = new ProcessRulePeriodWiseViewModel();
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

       
    }
}
