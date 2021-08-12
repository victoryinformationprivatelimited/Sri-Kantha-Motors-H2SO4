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
using LiveCharts;
using LiveCharts.Wpf;

namespace ERP.Dashboard
{
    /// <summary>
    /// Interaction logic for EmployeeSection.xaml
    /// </summary>
    public partial class EmployeeSection : UserControl
    {
        #region Properties
        EmployeeSectionViewModel viewModel = new EmployeeSectionViewModel();
        #endregion

        #region Constructor
        public EmployeeSection()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            setDefaultDropdownSizes();
            loadDepartmentWiseEmployeesPieChart();
            loadResignCountBarChart();
            loadDesignationWiseEmployeesPieChart();
            loadCompanyGrowthBarChart();
        }

#endregion

        #region Dropdown Button Click Events

        private void icon1_Click(object sender, RoutedEventArgs e)
        {
            if (icon1DropDown.Height <= 10)
            {
                icon1DropDown.Height = 222;
                icon1Border.Margin = new Thickness(71, 162, 0, 0);
                icon2DropDown.Height = 10;
                icon2Border.Margin = new Thickness(446, 10, 0, 0);
                icon3DropDown.Height = 10;
                icon3Border.Margin = new Thickness(818, 10, 0, 0);
                icon4DropDown.Height = 10;
                icon4Border.Margin = new Thickness(1223, 10, 0, 0);
            }
            else
            {
                icon1DropDown.Height = 10;
                icon1Border.Margin = new Thickness(71, 10, 0, 0);
            }
        }

        private void icon2_Click(object sender, RoutedEventArgs e)
        {
            if (icon2DropDown.Height <= 10)
            {
                icon1DropDown.Height = 10;
                icon1Border.Margin = new Thickness(71, 10, 0, 0);
                icon2DropDown.Height = 222;
                icon2Border.Margin = new Thickness(446, 162, 0, 0);
                icon3DropDown.Height = 10;
                icon3Border.Margin = new Thickness(818, 10, 0, 0);
                icon4DropDown.Height = 10;
                icon4Border.Margin = new Thickness(1223, 10, 0, 0);
            }
            else
            {
                icon2DropDown.Height = 10;
                icon2Border.Margin = new Thickness(446, 10, 0, 0);
            }
        }

        private void icon3_Click(object sender, RoutedEventArgs e)
        {
            if (icon3DropDown.Height <= 10)
            {
                icon1DropDown.Height = 10;
                icon1Border.Margin = new Thickness(71, 10, 0, 0);
                icon2DropDown.Height = 10;
                icon2Border.Margin = new Thickness(446, 10, 0, 0);
                icon3DropDown.Height = 222;
                icon3Border.Margin = new Thickness(818, 162, 0, 0);
                icon4DropDown.Height = 10;
                icon4Border.Margin = new Thickness(1223, 10, 0, 0);
            }
            else
            {
                icon3DropDown.Height = 10;
                icon3Border.Margin = new Thickness(818, 10, 0, 0);
            }
        }

        private void icon4_Click(object sender, RoutedEventArgs e)
        {
            if (icon4DropDown.Height <= 10)
            {
                icon1DropDown.Height = 10;
                icon1Border.Margin = new Thickness(71, 10, 0, 0);
                icon2DropDown.Height = 10;
                icon2Border.Margin = new Thickness(446, 10, 0, 0);
                icon3DropDown.Height = 10;
                icon3Border.Margin = new Thickness(818, 10, 0, 0);
                icon4DropDown.Height = 222;
                icon4Border.Margin = new Thickness(1223, 162, 0, 0);
            }
            else
            {
                icon4DropDown.Height = 10;
                icon4Border.Margin = new Thickness(1223, 10, 0, 0);
            }
        }

        #endregion

        #region Mouse Hover Events

        private void upcomingBirthdaysCardView_MouseEnter(object sender, MouseEventArgs e)
        {
            upcomingBirthdaysTopic.Foreground = Brushes.Black;
            upcomingBirthdayCount.Foreground = Brushes.Black;
        }

        private void upcomingBirthdaysCardView_MouseLeave(object sender, MouseEventArgs e)
        {
            upcomingBirthdaysTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));
            upcomingBirthdayCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
        }

        private void newJoineesCardView_MouseEnter(object sender, MouseEventArgs e)
        {
            newJoineesTopic.Foreground = Brushes.Black;
            newJoineeCount.Foreground = Brushes.Black;
        }

        private void newJoineesCardView_MouseLeave(object sender, MouseEventArgs e)
        {
            newJoineesTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
            newJoineeCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
        }

        private void permanentCardView_MouseEnter(object sender, MouseEventArgs e)
        {
            permanentCount.Foreground = Brushes.Black;
            permanentTopic.Foreground = Brushes.Black;
        }

        private void permanentCardView_MouseLeave(object sender, MouseEventArgs e)
        {
            permanentCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
            permanentTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
        }

        private void upcomingBirthdayCount_MouseEnter(object sender, MouseEventArgs e)
        {
            upcomingBirthdaysTopic.Foreground = Brushes.Black;
            upcomingBirthdayCount.Foreground = Brushes.Black;
        }

        private void upcomingBirthdayCount_MouseLeave(object sender, MouseEventArgs e)
        {
            upcomingBirthdaysTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
            upcomingBirthdayCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
        }

        private void newJoineeCount_MouseEnter(object sender, MouseEventArgs e)
        {
            newJoineesTopic.Foreground = Brushes.Black;
            newJoineeCount.Foreground = Brushes.Black;
        }

        private void newJoineeCount_MouseLeave(object sender, MouseEventArgs e)
        {
            newJoineesTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
            newJoineeCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
        }

        private void permanentCount_MouseEnter(object sender, MouseEventArgs e)
        {
            permanentTopic.Foreground = Brushes.Black;
            permanentCount.Foreground = Brushes.Black;
        }

        private void permanentCount_MouseLeave(object sender, MouseEventArgs e)
        {
            permanentTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
            permanentCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545"));;
        }


        private void resignCardView_MouseEnter(object sender, MouseEventArgs e)
        {
            ResignTopic.Foreground = Brushes.Black;
            ResignCount.Foreground = Brushes.Black;
        }

        private void ResignCount_MouseEnter(object sender, MouseEventArgs e)
        {
            ResignTopic.Foreground = Brushes.Black;
            ResignCount.Foreground = Brushes.Black;
        }

        private void ResignCount_MouseLeave(object sender, MouseEventArgs e)
        {
            ResignTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545")); ;
            ResignCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545")); ;
        }

        private void resignCardView_MouseLeave(object sender, MouseEventArgs e)
        {
            ResignTopic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545")); ;
            ResignCount.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#454545")); ;
        }

        #endregion

        #region Defaults

        private void setDefaultDropdownSizes()
        {
            icon1DropDown.Height = 10;
            icon1Border.Margin = new Thickness(71, 10, 0, 0);
            icon2DropDown.Height = 10;
            icon2Border.Margin = new Thickness(446, 10, 0, 0);
            icon3DropDown.Height = 10;
            icon3Border.Margin = new Thickness(818, 10, 0, 0);
            icon4DropDown.Height = 10;
            icon4Border.Margin = new Thickness(1223, 10, 0, 0);
        }

        #endregion

        #region Chart Methods

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            EmployeeSectionViewModel.PieChart_DataClick(sender, chartPoint);
        }

        private void loadDepartmentWiseEmployeesPieChart()
        {
            EmployeeSectionViewModel.loadPieChart(departmentWiseEmployeesPieChart);
        }

        private void loadDesignationWiseEmployeesPieChart()
        {
            EmployeeSectionViewModel.loadDesignationWisePieChart(designationWiseEmployeesPieChart);
        }

        private void loadCompanyGrowthBarChart()
        {
            EmployeeSectionViewModel.loadBarChart(yearWiseEmployeeCountLineChart);
        }

        private void loadResignCountBarChart()
        {
            EmployeeSectionViewModel.loadBarChartResignCount(resignCountBarChart);
        }

        #endregion

    }
}
