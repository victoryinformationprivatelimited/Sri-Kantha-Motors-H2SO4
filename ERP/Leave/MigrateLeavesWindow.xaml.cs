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

namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for MigrateLeavesWindow.xaml
    /// </summary>
    public partial class MigrateLeavesWindow : Window
    {

        MigrateLeavesViewModel ViewModel;
        ToolTip ControllerToolTip;


        public MigrateLeavesWindow()
        {
            InitializeComponent();
            CmbSearch.Items.Add("Emp ID");
            CmbSearch.Items.Add("First Name");
            CmbSearch.Items.Add("Surname");
            CmbSearch.Items.Add("Department");
            CmbSearch.Items.Add("Months Since Joined");

            ControllerToolTip = (ToolTip)CmbSearch.ToolTip;
            ControllerToolTip = new System.Windows.Controls.ToolTip();
            ControllerToolTip.PlacementTarget = CmbSearch;
            ControllerToolTip.Content = "Will filter the employees according to the entered \n number of months since the join date \n\n* entering a single value will filter all the employees \n less than or equal to the entered number of months \n\n* entering two values saperated by a comma (,) will filter all the employees if the number of worked months \n lies between the two values";

            ViewModel = new MigrateLeavesViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void CmbSearch_MouseEnter_1(object sender, MouseEventArgs e)
        {
            try
            {
                if (CmbSearch.SelectedIndex == 4) 
                {
                    ControllerToolTip.IsOpen = true;
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void CmbSearch_MouseLeave_1(object sender, MouseEventArgs e)
        {
            try
            {
                ControllerToolTip.IsOpen = false;
            }
            catch (Exception)
            {

            }

        }
    }
}
