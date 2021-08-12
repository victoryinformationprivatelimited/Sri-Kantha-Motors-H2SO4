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
    /// Interaction logic for TaskCategoriesWindow.xaml
    /// </summary>
    public partial class TaskCategoriesWindow : Window
    {
        TaskCategoriesViewModel ViewModel;

        public TaskCategoriesWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new TaskCategoriesViewModel();
            Loaded += (s, e) => { DataContext = ViewModel; };
            SearchCombo.Items.Add("Task Name");
            SearchCombo.Items.Add("Task Description");
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
