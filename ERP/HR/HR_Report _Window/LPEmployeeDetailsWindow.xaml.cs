using CustomBusyBox;
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

namespace ERP.HR.HR_Report_Window
{
    /// <summary>
    /// Interaction logic for LPEmployeeDetailsWindow.xaml
    /// </summary>
    public partial class LPEmployeeDetailsWindow : Window
    {
        LPEmployeeDetailsViewModel viewmodel;
        public LPEmployeeDetailsWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            this.Owner = clsWindow.Mainform;
            viewmodel = new LPEmployeeDetailsViewModel();
            Loaded += (s, e) => { DataContext = viewmodel; };
        }
    }
}
