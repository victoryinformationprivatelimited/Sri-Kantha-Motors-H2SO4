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

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for SalaryIncrementWindow.xaml
    /// </summary>
    public partial class SalaryIncrementWindow : Window
    {
        SalaryIncrementViewModel viewmodel = new SalaryIncrementViewModel();
        public SalaryIncrementWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void salary_inc_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void salary_inc_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void salary_inc_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
