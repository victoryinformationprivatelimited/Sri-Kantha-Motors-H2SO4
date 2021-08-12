using ERP.UI_UserControlers;
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

namespace ERP.Performance.Evaluation
{
    /// <summary>
    /// Interaction logic for EvaluationMDIUserControl.xaml
    /// </summary>
    public partial class EvaluationMDIUserControl : UserControl
    {
        public EvaluationMDIUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AppraisalButton appraisal = new AppraisalButton(this);
            Mdiwrappanel.Children.Add(appraisal);
        }
    }
}
