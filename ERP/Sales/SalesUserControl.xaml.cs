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
using ERP.UI_UserControlers;

namespace ERP.Sales
{
    /// <summary>
    /// Interaction logic for SalesUserControl.xaml
    /// </summary>
    public partial class SalesUserControl : UserControl
    {
        public SalesUserControl()
        {
            InitializeComponent();
        }

        private void Item_Master_Click(object sender, RoutedEventArgs e)
        {
            //ItemMasterUserControl ItemMaster = new ItemMasterUserControl();
           
            //Mdi.Children.Add(ItemMaster);
        }
       
        private void Item_Category_Master_Click(object sender, RoutedEventArgs e)
        {
            //ItemCatergoryMasterUserControl Catergory = new ItemCatergoryMasterUserControl();
       
            //Mdi.Children.Add(Catergory);
        }

        private void Item_Sub_Category_Click(object sender, RoutedEventArgs e)
        {
            //ItemSubCatergoryMasterUserControl SubCatergory = new ItemSubCatergoryMasterUserControl();
            
            //Mdi.Children.Add(SubCatergory);
        }

        private void sales_usercontrol_Loaded(object sender, RoutedEventArgs e)
        {
            SalesButtons salesbtn = new SalesButtons(this);
            Mdiwrappanel.Children.Add(salesbtn);
        }

        
    }
}
