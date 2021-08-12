using ERP.Performance.Evaluation;
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
    /// Interaction logic for AppraisalButton.xaml
    /// </summary>
    public partial class AppraisalButton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        EvaluationCategoryWindow EvaluationCategoryW;
        EvaluationPeriodWindow EvaluationPeriodW;
        CategoryCriteriasWindow CategoryCriteriasW;
        AssignEvaluationCriteriaWindow AssignEvaluationCriteriaW;
        public AppraisalButton(EvaluationMDIUserControl Evaluation)
        {
            InitializeComponent();
            MDIWrip = Evaluation.Mdi;
        }

        private void Employee_TaskApprovals_Copy_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            EvaluationCategoryW = new EvaluationCategoryWindow();
            EvaluationCategoryW.Show();
            
        }

        private void Employee_TaskApprovals_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            EvaluationPeriodW = new EvaluationPeriodWindow();
            EvaluationPeriodW.Show();
        }

        private void WindowClose()
        {
           
            if (EvaluationCategoryW != null)
                EvaluationCategoryW.Close();
            if (EvaluationPeriodW != null)
                EvaluationPeriodW.Close();
            if (CategoryCriteriasW != null)
                CategoryCriteriasW.Close();
            if (AssignEvaluationCriteriaW != null)
                AssignEvaluationCriteriaW.Close();
        }

        private void Employee_TaskApprovals_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            CategoryCriteriasW = new CategoryCriteriasWindow();
            CategoryCriteriasW.Show();

        }

        private void Employee_TaskApprovals_Copy3_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            AssignEvaluationCriteriaW = new AssignEvaluationCriteriaWindow();
            AssignEvaluationCriteriaW.Show();
        }

        private void Employee_TaskApprovals_Copy4_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
        }
        
    }
}
