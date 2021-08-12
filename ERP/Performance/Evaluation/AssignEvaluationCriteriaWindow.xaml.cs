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

namespace ERP.Performance.Evaluation
{
    /// <summary>
    /// Interaction logic for AssignEvaluationCriteriaWindow.xaml
    /// </summary>
    public partial class AssignEvaluationCriteriaWindow : Window
    {
        AssignEvaluationCriteriaViewModel viewModel;
        public AssignEvaluationCriteriaWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new AssignEvaluationCriteriaViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void assignEvalCloseBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.Close();
        }

        private void assignEvalTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	this.DragMove();
        }

        private void assignEvalTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	this.DragMove();
        }

        private void CustomDataGrid_LoadingRow_1(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void CustomDataGrid_LoadingRow_2(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
