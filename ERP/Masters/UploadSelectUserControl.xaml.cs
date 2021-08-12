using ERP.UI_UserControlers;
using ERP.Utility;
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

namespace ERP.Masters
{
    /// <summary>
    /// Interaction logic for UploadSelectUserControl.xaml
    /// </summary>
    public partial class UploadSelectUserControl : UserControl
    {
        MasterButtons mast;
        public UploadSelectUserControl(MasterButtons mast)
        {
            this.mast = mast;
            InitializeComponent();
        }

        private void Employee_Date_upload_Checked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Download Excel Format ", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
            }
            if (result == MessageBoxResult.No)
            {
                ((WrapPanel)this.Parent).Children.Remove(this);
                EmployeeDataMigration datamigration = new EmployeeDataMigration();

                
                mast.MDIWrip.Children.Add(datamigration);
            }

 
        }

        private void Attendance_upload_Checked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Download Excel Format ", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
            }
            if (result == MessageBoxResult.No)
            {
                LeaveDataMigration leave = new LeaveDataMigration();
                mast.MDIWrip.Children.Clear();
                mast.MDIWrip.Children.Add(leave);
            }
        }
    }
}
