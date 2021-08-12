using CustomBusyBox;
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

namespace ERP.Masters
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        EmployeeViewModel viewModel = new EmployeeViewModel();

        public EmployeeWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            Town.SelectedIndex = 0;
            BusyBox.InitializeThread(this);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //tabcontroll.SelectedItem = basictab;
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Emp_Award_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Emp_Award_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Family_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Family_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Social_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Social_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Prof_Que_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Prof_Que_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Skills_Type_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Skills_Type_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Helth_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Helth_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Activities_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Activities_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Employee_Academic_Add_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Employee_Academic_Remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Social_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Interst_Type_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Emp_Familynew_btn_Click(object sender, RoutedEventArgs e)
        {

        }
        
    }
}
