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

namespace ERP.Loan_Module.Basic_Masters
{
    /// <summary>
    /// Interaction logic for LoansWindow.xaml
    /// </summary>
    public partial class LoansWindow : Window
    {
       private LoansViewModel viewModel;
       public LoansWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new LoansViewModel();
            //___________________________________________________________________________
            CMBMaxDurationType.Items.Add("Years");
            CMBMaxDurationType.Items.Add("Months");
            CMBMaxDurationType.SelectedIndex = 0;

            CmbEmpserviceperiod.Items.Add("Years");
            CmbEmpserviceperiod.Items.Add("Months");
            CmbEmpserviceperiod.SelectedIndex = 0;

            CMBMinDurationType.Items.Add("Years");
            CMBMinDurationType.Items.Add("Months");
            CMBMinDurationType.SelectedIndex = 0;

            CMBGurantorServicePeriodType.Items.Add("Years");
            CMBGurantorServicePeriodType.Items.Add("Months");
            CMBGurantorServicePeriodType.SelectedIndex = 0;
            //___________________________________________________________________________


            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
