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

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for EmployeeResignDetails.xaml
    /// </summary>
    public partial class EmployeeResignDetails : UserControl
    {
        EmployeeResignDetailsViewModel viewModel = new EmployeeResignDetailsViewModel();

        public EmployeeResignDetails()
        {
            InitializeComponent();

            resign.Items.Add("3");
            resign.Items.Add("6");
            resign.Items.Add("9");
            resign.Items.Add("12");



            this.Loaded += (s, e) =>
                {
                    DataContext = viewModel;
                }; 



        }

        private void closeClick(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

       

        


    }
}
