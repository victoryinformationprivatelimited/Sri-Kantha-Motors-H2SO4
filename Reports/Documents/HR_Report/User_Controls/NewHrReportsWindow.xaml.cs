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
    /// Interaction logic for NewHrReportsWindow.xaml
    /// </summary>
    public partial class NewHrReportsWindow : Window
    {
        public NewHrReportsWindow(ViewModelBase ViewModel)
        {
            InitializeComponent();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
