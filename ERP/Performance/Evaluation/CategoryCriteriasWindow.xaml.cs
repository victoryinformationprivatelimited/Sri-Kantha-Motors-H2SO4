using ERP.HelperClass;
using ERP.Performance.Evaluation;
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

namespace ERP.Performance.Evaluation
{
    /// <summary>
    /// Interaction logic for CategoryCriteriasWindow.xaml
    /// </summary>
    public partial class CategoryCriteriasWindow : Window
    {
        CategoryCriteriasViewModel ViewModel;
        public CategoryCriteriasWindow()
        {
            InitializeComponent();
            Owner = clsWindow.Mainform;
            ViewModel = new CategoryCriteriasViewModel();
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
