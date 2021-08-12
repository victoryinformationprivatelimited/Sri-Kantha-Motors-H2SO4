using ERP.HelperClass;
using ERP.MastersDetails;
using ERP.Performance;
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

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for PerformanceButton.xaml
    /// </summary>
    public partial class PerformanceButton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        TaskCategoriesWindow TaskCategoriesW;
        TaskSubCategoriesWindow TaskSubCategoriesW;
        EmployeeTasksWindow EmployeeTasksW;
        EmployeeSubTasksWindow EmployeeSubTasksW;
        EmployeeTasksViewerWindow EmployeeTasksViewerW;
        TaskApprovalsWindow TaskApprovalsW;

        public PerformanceButton(Performance.PerformanceMDIUserControl Performance)
        {
            InitializeComponent();
            MDIWrip = Performance.Mdi;
        }

        private void Hr_Checkbox_two_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Hr_Checkbox_two_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Task_Category_Click_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            TaskCategoriesW = new TaskCategoriesWindow();
            TaskCategoriesW.Show();
        }

        private void Task_SubCategory_Click_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            TaskSubCategoriesW = new TaskSubCategoriesWindow();
            TaskSubCategoriesW.Show();
        }

        private void Employee_Tasks_Click_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            EmployeeTasksW = new EmployeeTasksWindow();
            EmployeeTasksW.Show();
        }

        private void Employee_SubTasks_Click_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            EmployeeSubTasksW = new EmployeeSubTasksWindow();
            EmployeeSubTasksW.Show();
        }


        private void Employee_Dashboard_Click_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            EmployeeTasksViewerW = new EmployeeTasksViewerWindow();
            EmployeeTasksViewerW.Show();
        }


        private void Employee_TaskApprovals_Click_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            TaskApprovalsW = new TaskApprovalsWindow();
            TaskApprovalsW.Show();
        }

        private void WindowClose()
        {
            if (EmployeeSubTasksW != null)
                EmployeeSubTasksW.Close();
            if (EmployeeTasksW != null)
                EmployeeTasksW.Close();
            if (TaskCategoriesW != null)
                TaskCategoriesW.Close();
            if (TaskSubCategoriesW != null)
                TaskSubCategoriesW.Close();
            if (EmployeeTasksViewerW != null)
                EmployeeTasksViewerW.Close();
            if (TaskApprovalsW != null)
                TaskApprovalsW.Close();
        }
    }
}
