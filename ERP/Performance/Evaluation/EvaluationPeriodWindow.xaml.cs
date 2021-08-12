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
    /// Interaction logic for EvaluationPeriodWindow.xaml
    /// </summary>
    public partial class EvaluationPeriodWindow : Window
    {
        EvaluationPeriodViewModel viewModel;
        public EvaluationPeriodWindow()
        {
            InitializeComponent();
            viewModel = new EvaluationPeriodViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
