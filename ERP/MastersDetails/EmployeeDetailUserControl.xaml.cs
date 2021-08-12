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
using ERP.MastersDetails;


namespace ERP
{
    /// <summary>
    /// Interaction logic for EmployeeDetailUserControl.xaml
    /// </summary>
    public partial class EmployeeDetailUserControl : UserControl
    {
        EmployeeDetailsViewModel viweModel = new EmployeeDetailsViewModel(clsConfig.CurrentEmployee);
        public EmployeeDetailUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viweModel; };
        }
    }
}
