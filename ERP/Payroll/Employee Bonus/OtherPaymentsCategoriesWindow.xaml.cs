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
    /// Interaction logic for OtherPaymentsCategoriesWindow.xaml
    /// </summary>
    public partial class OtherPaymentsCategoriesWindow : Window
    {
        OtherPaymentsCategoriesViewModel viewModel;
        public OtherPaymentsCategoriesWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new OtherPaymentsCategoriesViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
    }
}
