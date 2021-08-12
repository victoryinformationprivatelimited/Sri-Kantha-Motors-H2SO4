using ERP.Sales;
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

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for SalesButtons.xaml
    /// </summary>
    public partial class SalesButtons : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        SalesUserControl suc;
        public SalesButtons(SalesUserControl SalesUserControl)
        {
            InitializeComponent();
            suc = SalesUserControl;
            MDIWrip = SalesUserControl.Mdi;
        }

        private void Item_Masters_Checked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemMasterUserControl ItemMaster = new ItemMasterUserControl();
            //MDIWrip.Children.Add(ItemMaster);
        }

        private void Item_Category_Master1_Checked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemCatergoryMasterUserControl Catergory = new ItemCatergoryMasterUserControl();
            //MDIWrip.Children.Add(Catergory);
        }

        private void Item_sub_Category_Checked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemSubCatergoryMasterUserControl SubCatergory = new ItemSubCatergoryMasterUserControl();
            //MDIWrip.Children.Add(SubCatergory);
        }

        private void Item_Category1_Checked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemCatergorizeUserControl itemcategory = new ItemCatergorizeUserControl(suc);
            //MDIWrip.Children.Add(itemcategory);
        }

        private void Hr_Checkbox_two_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Hr_Checkbox_two_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Item_Masters_Unchecked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemMasterUserControl ItemMaster = new ItemMasterUserControl();
            //MDIWrip.Children.Add(ItemMaster);
        }

        private void Item_Category_Master1_Unchecked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemCatergoryMasterUserControl Catergory = new ItemCatergoryMasterUserControl();
            //MDIWrip.Children.Add(Catergory);
        }

        private void Item_sub_Category_Unchecked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemSubCatergoryMasterUserControl SubCatergory = new ItemSubCatergoryMasterUserControl();
            //MDIWrip.Children.Add(SubCatergory);
        }

        private void Item_Category1_Unchecked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            //ItemCatergorizeUserControl itemcategory = new ItemCatergorizeUserControl(suc);
            //MDIWrip.Children.Add(itemcategory);
        }
    }
}
