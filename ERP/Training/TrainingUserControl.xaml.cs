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

namespace ERP.Training
{
    /// <summary>
    /// Interaction logic for TrainingUserControl.xaml
    /// </summary>
    public partial class TrainingUserControl : UserControl
    {
        public TrainingUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            TrainingButton trningbtn = new TrainingButton(this);
            Mdiwrappanel.Children.Add(trningbtn);

        }
    }
}
