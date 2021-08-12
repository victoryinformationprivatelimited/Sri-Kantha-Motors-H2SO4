using ERP.Medical;
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
    /// Interaction logic for MedicalButton.xaml
    /// </summary>
    public partial class MedicalButton : UserControl
    {
        public WrapPanel MDIWrip = new WrapPanel();

        #region Window Members

        MedicalPeriodMasterWindow medicPeriodWindow;
        MedicalCategoryWindow medicCategoryWindow;
        MedicalDetailsWindow medicDetailsWindow;
        EmployeeMedicalWindow empMedicWindow;

        #endregion

        public MedicalButton(Medical.MedicalUserControl medUserControl)
        {
            InitializeComponent();
            MDIWrip = medUserControl.Mdi;
        }


        #region Form Close Methods

        void closeWindows()
        {
            if (medicPeriodWindow != null)
                medicPeriodWindow.Close();
            if (medicCategoryWindow != null)
                medicCategoryWindow.Close();
            if (medicDetailsWindow != null)
                medicDetailsWindow.Close();
            if (empMedicWindow != null)
                empMedicWindow.Close();
        }

        #endregion

        private void Medical_Period_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            MedicalPeriodMasterUserControl periodMaster = new MedicalPeriodMasterUserControl();
            MDIWrip.Children.Add(periodMaster);
        }

        private void Medical_Category_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            MedicalCategoryUserControl catMaster = new MedicalCategoryUserControl();
            MDIWrip.Children.Add(catMaster);
        }

        private void Medical_Reimbursement_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            EmployeeMedicalUserControl empMedical = new EmployeeMedicalUserControl();
            MDIWrip.Children.Add(empMedical);
        }

        private void Medical_Period_Unchecked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            MedicalPeriodMasterUserControl periodMaster = new MedicalPeriodMasterUserControl();
            MDIWrip.Children.Add(periodMaster);
        }

        private void Medical_Category_Unchecked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            MedicalCategoryUserControl catMaster = new MedicalCategoryUserControl();
            MDIWrip.Children.Add(catMaster);
        }

        private void Medical_Reimbursement_Unchecked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            EmployeeMedicalUserControl empMedical = new EmployeeMedicalUserControl();
            MDIWrip.Children.Add(empMedical);
        }

        private void Medical_Detail_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            MedicalDetailsUserControl details = new MedicalDetailsUserControl();
            MDIWrip.Children.Add(details);
        }

        private void Medical_Detail_Unchecked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;
            MedicalDetailsUserControl details = new MedicalDetailsUserControl();
          MDIWrip.Children.Add(details);
        }

        private void Hr_Checkbox_two_Click(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
        }

        private void Medical_Period_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(701))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                medicPeriodWindow = new MedicalPeriodMasterWindow();
                medicPeriodWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Medical_Category_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(702))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                medicCategoryWindow = new MedicalCategoryWindow();
                medicCategoryWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Medical_Detail_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(703))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                medicDetailsWindow = new MedicalDetailsWindow();
                medicDetailsWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Medical_Reimbursement_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(704))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeWindows();
                empMedicWindow = new EmployeeMedicalWindow();
                empMedicWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

      

    }
}
