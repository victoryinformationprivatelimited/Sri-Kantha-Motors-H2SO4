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

namespace ERP
{
    /// <summary>
    /// Interaction logic for MasterDetailUserControl.xaml
    /// </summary>
    public partial class MasterDetailUserControl : UserControl
    {
        public MasterDetailUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmployeeBenifitDetail EMD  = new EmployeeBenifitDetail();
            DetailMdi.Children.Clear();
            DetailMdi.Children.Add(EMD);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           // EmployeeDeduction ED = new EmployeeDeduction();
           // MasterDetailMDI.Children.Clear();
            //MasterDetailMDI.Children.Add(ED);
        }
    }
}
