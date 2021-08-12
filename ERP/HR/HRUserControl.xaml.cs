using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP.Payroll;
using ERP.MastersDetails;


namespace ERP
{
	/// <summary>
	/// Interaction logic for HRUserControl.xaml
	/// </summary>
	public partial class HRUserControl : UserControl
	{
		public HRUserControl()
		{
			this.InitializeComponent();
		}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UI_UserControlers.HR_Buttons  a = new UI_UserControlers.HR_Buttons(this);
            Mdi.Children.Clear();
            Mdi.Children.Add(a);
        }

  
       

      

        

	}
}