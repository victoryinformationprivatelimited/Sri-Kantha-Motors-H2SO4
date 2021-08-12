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
using ERP.ERPService;

namespace ERP.Performance
{
    /// <summary>
    /// Interaction logic for EmployeeTasksWindow.xaml
    /// </summary>
    public partial class EmployeeTasksWindow : Window
    {
        EmployeeTasksViewModel ViewModel;

        public EmployeeTasksWindow(EmployeeSubTasksDetailForUserView Subtask, EmployeeTasksViewerWindow Owner)
        {
            InitializeComponent();
            this.Owner = Owner;
            ViewModel = new EmployeeTasksViewModel(Subtask);
            ViewModel.Window = this;
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };

            SearchCombo.Items.Add("Name");
            SearchCombo.Items.Add("Description");
            SearchCombo.Items.Add("Start Date");
            SearchCombo.Items.Add("End Date");
        }

        public EmployeeTasksWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new EmployeeTasksViewModel();
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };

            SearchCombo.Items.Add("Name");
            SearchCombo.Items.Add("Description");
            SearchCombo.Items.Add("Start Date");
            SearchCombo.Items.Add("End Date");
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
