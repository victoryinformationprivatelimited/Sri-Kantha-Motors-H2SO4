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

namespace ERP.BasicSearch
{
    /// <summary>
    /// Interaction logic for EmployeeMultipleSearchWindow.xaml
    /// </summary>
    public partial class EmployeeMultipleSearchWindow : Window
    {
        
        public EmployeeSearchViewModel viewModel;
        public EmployeeMultipleSearchWindow()
        {
            this.Owner = clsWindow.Mainform;
            viewModel = new EmployeeSearchViewModel(true);
            InitializeComponent();
            comboSearch();
            Loaded += (s, e) => {DataContext = viewModel;};
        }

        public EmployeeMultipleSearchWindow(Guid PaymentMethod) 
        {
            this.Owner = clsWindow.Mainform;
            viewModel = new EmployeeSearchViewModel(PaymentMethod);
            InitializeComponent();
            comboSearch();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        public EmployeeMultipleSearchWindow(List<Guid> empList)
        {
            this.Owner = clsWindow.Mainform;
            viewModel = new EmployeeSearchViewModel(empList);
            InitializeComponent();
            comboSearch();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        public EmployeeMultipleSearchWindow(IEnumerable<EmployeeSearchView> SelectedEmployees)
        {

            viewModel = new EmployeeSearchViewModel(SelectedEmployees);
            InitializeComponent();
            comboSearch();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void comboSearch()
        {
            Search.Items.Add("First Name");//0
            Search.Items.Add("Last Name");//1
            Search.Items.Add("Middle Name");//2
            Search.Items.Add("Employee ID");//3
            Search.Items.Add("Mobile");//4
            Search.Items.Add("Branch");//5
            Search.Items.Add("Department");//6
            Search.Items.Add("Section");//7
            Search.Items.Add("Designation");//8
            Search.Items.Add("Grade");//9

            Search.SelectedIndex = 0;
        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
          
        }
    }
}
