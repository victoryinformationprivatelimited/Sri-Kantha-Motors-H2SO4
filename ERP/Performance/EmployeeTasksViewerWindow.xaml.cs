using ERP.ERPService;
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

namespace ERP.Performance
{
    /// <summary>
    /// Interaction logic for EmployeeTasksViewerWindow.xaml
    /// </summary>
    public partial class EmployeeTasksViewerWindow : Window
    {
        EmployeeTasksViewerViewmodel ViewModel;

        public EmployeeTasksViewerWindow()
        {
            InitializeComponent();

            ViewModel = new EmployeeTasksViewerViewmodel();
            ViewModel.MainWindow = this;
            this.Owner = clsWindow.Mainform;

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
                Closing += ViewModel.OnwindowClose;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button expandCollapseButton = (Button)sender;
            DataGridRow selectedRow = DataGridRow.GetRowContainingElement(expandCollapseButton);
            EmployeeSubTasksDetailForUserView CurrentItem = (expandCollapseButton.DataContext) as EmployeeSubTasksDetailForUserView;

            if (selectedRow.DetailsVisibility == System.Windows.Visibility.Collapsed)
            {
                ViewModel.CurrentAcceptedSubtask = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Visible;
                expandCollapseButton.Content = "-";
            }

            else
            {
                ViewModel.CurrentAcceptedSubtask = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Collapsed;
                expandCollapseButton.Content = "+";
            }

        }
    }
}
