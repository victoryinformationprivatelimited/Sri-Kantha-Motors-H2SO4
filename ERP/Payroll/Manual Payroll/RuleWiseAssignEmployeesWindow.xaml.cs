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
    /// Interaction logic for RuleWiseAssignEmployeesWindow.xaml
    /// </summary>
    public partial class RuleWiseAssignEmployeesWindow : Window
    {
        private RuleWiseAssignEmployeesViewModel ViewModel;
        public RuleWiseAssignEmployeesWindow()
        {
            InitializeComponent();
            ViewModel = new RuleWiseAssignEmployeesViewModel();
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
