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

namespace ERP.MastersDetails
{
    /// <summary>
    /// Interaction logic for ShiftDetailUserControl.xaml
    /// </summary>
    public partial class ShiftDetailUserControl : UserControl
    {
         ShiftDetailViewModel viewModel = new ShiftDetailViewModel();
        public ShiftDetailUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBox.Focus();
        }

        private void Shift_Detail_Close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        #region DragMove
        Window LoadedWindow = new Window();
        public void Windows(Window LoadedWindow)
        {
            this.LoadedWindow = LoadedWindow;
        }
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadedWindow.DragMove();
        }
        #endregion
    }
}
