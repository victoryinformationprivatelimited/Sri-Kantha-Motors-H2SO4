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
    /// Interaction logic for TaskApprovalsWindow.xaml
    /// </summary>
    public partial class TaskApprovalsWindow : Window
    {
        TaskApprovalsViewModel ViewModel;

        public TaskApprovalsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new TaskApprovalsViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button expandCollapseButton = (Button)sender;
            DataGridRow selectedRow = DataGridRow.GetRowContainingElement(expandCollapseButton);
            EmployeeSubTasksDetailForUserView CurrentItem = (expandCollapseButton.DataContext) as EmployeeSubTasksDetailForUserView;

            if (selectedRow.DetailsVisibility == System.Windows.Visibility.Collapsed)
            {
                ViewModel.CurrentPendingApprovalTasks = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Visible;
                expandCollapseButton.Content = "-";
            }

            else
            {
                ViewModel.CurrentPendingApprovalTasks = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Collapsed;
                expandCollapseButton.Content = "+";
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Button expandCollapseButton = (Button)sender;
            DataGridRow selectedRow = DataGridRow.GetRowContainingElement(expandCollapseButton);
            EmployeeSubTasksDetailForUserView CurrentItem = (expandCollapseButton.DataContext) as EmployeeSubTasksDetailForUserView;

            if (selectedRow.DetailsVisibility == System.Windows.Visibility.Collapsed)
            {
                ViewModel.CurrentRejectedTasks = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Visible;
                expandCollapseButton.Content = "-";
            }

            else
            {
                ViewModel.CurrentRejectedTasks = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Collapsed;
                expandCollapseButton.Content = "+";
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Button expandCollapseButton = (Button)sender;
            DataGridRow selectedRow = DataGridRow.GetRowContainingElement(expandCollapseButton);
            EmployeeSubTasksDetailForUserView CurrentItem = (expandCollapseButton.DataContext) as EmployeeSubTasksDetailForUserView;

            if (selectedRow.DetailsVisibility == System.Windows.Visibility.Collapsed)
            {
                ViewModel.CurrentExpiredTask = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Visible;
                expandCollapseButton.Content = "-";
            }

            else
            {
                ViewModel.CurrentExpiredTask = CurrentItem;
                selectedRow.DetailsVisibility = Visibility.Collapsed;
                expandCollapseButton.Content = "+";
            }
        }

    }
}
