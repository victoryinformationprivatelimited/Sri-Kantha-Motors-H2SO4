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
    /// Interaction logic for TaskSubCategoriesWindow.xaml
    /// </summary>
    public partial class TaskSubCategoriesWindow : Window
    {
        TaskSubCategoriesViewModel ViewModel;

        public TaskSubCategoriesWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            SearchCombo.Items.Add("Sub Category Name");
            SearchCombo.Items.Add("Sub Category Desc");
            SearchCombo.Items.Add("Category Name");
            ViewModel = new TaskSubCategoriesViewModel();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
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
