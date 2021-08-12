using ERP.HelperClass;
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

namespace ERP.Payroll.Employee_Bonus
{
    /// <summary>
    /// Interaction logic for BonusPeriodWindow.xaml
    /// </summary>
    public partial class BonusPeriodWindow : Window
    {
        BonusPeriodViewModel viewModel;
        public BonusPeriodWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new BonusPeriodViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
