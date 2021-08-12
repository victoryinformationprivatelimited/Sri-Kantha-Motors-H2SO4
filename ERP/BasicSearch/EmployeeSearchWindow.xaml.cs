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
    /// Interaction logic for EmployeeSearchWindow.xaml
    /// </summary>
    public partial class EmployeeSearchWindow : Window
    {
        public EmployeeSearchViewModel viewModel;

        public EmployeeSearchWindow()
        {
            InitializeComponent();
            viewModel = new EmployeeSearchViewModel();
            this.Loaded += (s, e) =>
            {
                this.DataContext = this.viewModel;
            };
            comboSearch();
        }

        public EmployeeSearchWindow(List<Guid> EmployeeList)
        {
            InitializeComponent();
            viewModel = new EmployeeSearchViewModel(EmployeeList);
            this.Loaded += (s, e) =>
            {
                this.DataContext = this.viewModel;
            };
            comboSearch();
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

        private void btnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //Search.Focus();
        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
