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

namespace ERP.Medical
{
    /// <summary>
    /// Interaction logic for LoanUserControl.xaml
    /// </summary>
    public partial class MedicalUserControl : UserControl
    {
        public MedicalUserControl()
        {
            InitializeComponent();

            //ScrollViewer scroll = new ScrollViewer();
            //scroll.Height = 450;

            //WrapPanel wrap = new WrapPanel();
            //wrap.Width = 160; 
        }

        private void Loan_Usercontrol(object sender, RoutedEventArgs e)
        {

        }

        private void Medical_Module_Loaded(object sender, RoutedEventArgs e)
        {

            MedicalButton masterbtn = new MedicalButton(this);
            Mdiwrappanel.Children.Add(masterbtn);  
        }
    }
}
