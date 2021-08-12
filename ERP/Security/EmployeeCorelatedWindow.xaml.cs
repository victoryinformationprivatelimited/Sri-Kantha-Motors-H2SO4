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

namespace ERP.Security
{
    /// <summary>
    /// Interaction logic for EmployeeCorelatedWindow.xaml
    /// </summary>
    public partial class EmployeeCorelatedWindow : Window
    {
        EmployeeCorelatedViewModel ViewModel;
        public EmployeeCorelatedWindow()
        {
            InitializeComponent();
            Owner = clsWindow.Mainform;
            ViewModel = new EmployeeCorelatedViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
