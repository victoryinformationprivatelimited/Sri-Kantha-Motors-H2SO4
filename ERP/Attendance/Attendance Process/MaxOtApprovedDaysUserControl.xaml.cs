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

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for MaxOtApprovedDaysUserControl.xaml
    /// </summary>
    public partial class MaxOtApprovedDaysUserControl : UserControl
    {
        MaxOtApprovedDaysViewModel maxOt = new MaxOtApprovedDaysViewModel();

        public MaxOtApprovedDaysUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.maxOt; };
        }
    }
}
