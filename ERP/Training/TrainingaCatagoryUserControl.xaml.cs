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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Training
{
    /// <summary>
    /// Interaction logic for TrainingaCatagoryUserControl.xaml
    /// </summary>
    public partial class TrainingaCatagoryUserControl : UserControl
    {
        TrainingCatagoryViewModel viewmodel = new TrainingCatagoryViewModel();
        public TrainingaCatagoryUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
