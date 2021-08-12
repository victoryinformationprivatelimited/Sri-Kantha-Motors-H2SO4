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

namespace ERP.Masters.Master_other_Details
{
    /// <summary>
    /// Interaction logic for EmployeeDetailsEffectiveDates.xaml
    /// </summary>
    public partial class EmployeeDetailsEffectiveDates : Window
    {
        public EmployeeDetailsEffectiveDates(EmployeeViewModel ViewModel)
        {
            InitializeComponent();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
