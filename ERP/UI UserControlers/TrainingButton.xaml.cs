using ERP.Training;
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
    /// Interaction logic for TrainingButton.xaml
    /// </summary>
    public partial class TrainingButton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        public TrainingButton(TrainingUserControl trainingUserControl)
        {
            InitializeComponent();
            MDIWrip = trainingUserControl.Mdi;
        }

        private void Training_catagory_Checked(object sender, RoutedEventArgs e)
        {
          
        }

        private void Training_catagory_Unchecked(object sender, RoutedEventArgs e)
        {
          
        }

        private void Training_Checkbox_two_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }

        }

        private void Training_Checkbox_two_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }

        }

        private void Trainingcatagory_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void Trainingcatagory_Unchecked(object sender, RoutedEventArgs e)
        {
           
        }

        private void Training_Checked(object sender, RoutedEventArgs e)
        {
            //Hr_Checkbox_two.IsChecked = false;
            TrainingaCatagoryUserControl tc = new TrainingaCatagoryUserControl();
            MDIWrip.Children.Add(tc);
        }

        private void Training_Unchecked(object sender, RoutedEventArgs e)
        {
            TrainingaCatagoryUserControl tc = new TrainingaCatagoryUserControl();
            MDIWrip.Children.Add(tc);
        }

        private void Trainer_Checked(object sender, RoutedEventArgs e)
        {
            TrainerUserControl tr = new TrainerUserControl();
            MDIWrip.Children.Add(tr);
        }

        private void Trainer_Unchecked(object sender, RoutedEventArgs e)
        {
            TrainerUserControl tr = new TrainerUserControl();
            MDIWrip.Children.Add(tr);
        }

        private void training_programs_Checked(object sender, RoutedEventArgs e)
        {
            TrainingProgramUserControl tp = new TrainingProgramUserControl();
            MDIWrip.Children.Add(tp);
        }

        private void traning_programs_Unchecked(object sender, RoutedEventArgs e)
        {
            TrainingProgramUserControl tp = new TrainingProgramUserControl();
            MDIWrip.Children.Add(tp);
        }

        private void Training_Detail_Checked(object sender, RoutedEventArgs e)
        {
            TrainingDetailUserControl td = new TrainingDetailUserControl();
            MDIWrip.Children.Add(td);
        }

        private void Training_Detail_Unchecked(object sender, RoutedEventArgs e)
        {
            TrainingDetailUserControl td = new TrainingDetailUserControl();
            MDIWrip.Children.Add(td);
        }
    }
}
