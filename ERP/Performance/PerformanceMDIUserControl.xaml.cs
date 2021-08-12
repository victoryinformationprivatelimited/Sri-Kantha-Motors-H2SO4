using ERP.UI_UserControlers;
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

namespace ERP.Performance
{
    /// <summary>
    /// Interaction logic for PerformanceMDIUserControl.xaml
    /// </summary>
    public partial class PerformanceMDIUserControl : UserControl
    {
        public PerformanceMDIUserControl()
        {
            InitializeComponent();
        }

        private void PerformanceUserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            PerformanceButton performance = new PerformanceButton(this);
            Mdiwrappanel.Children.Add(performance);
        }
    }
}
