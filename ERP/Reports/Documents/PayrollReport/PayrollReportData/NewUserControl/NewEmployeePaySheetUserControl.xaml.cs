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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportData.NewUserControl
{
    /// <summary>
    /// Interaction logic for NewEmployeePaySheetUserControl.xaml
    /// </summary>
    public partial class NewEmployeePaySheetUserControl : UserControl
    {
        WrapPanel masterwp;
        NewEmployeePaySheetViewModel viewmodel = new NewEmployeePaySheetViewModel();
        public NewEmployeePaySheetUserControl(WrapPanel wp)
        {
            masterwp = wp;
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };            
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            masterwp.Children.Clear();
        }        
    }
}
