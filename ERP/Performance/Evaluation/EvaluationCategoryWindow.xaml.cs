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
    /// Interaction logic for EvaluationCategoryWindow.xaml
    /// </summary>
    public partial class EvaluationCategoryWindow : Window
    {

        EvaluationCategoryViewModel viewModel;
     
        public EvaluationCategoryWindow()
        {
            InitializeComponent();
            viewModel = new EvaluationCategoryViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Shift_Categories_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
