using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ERP.Training
{
    public class TrainerViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public TrainerViewModel()
        {
            GetAllTrainer();
            GetAllEmployee();
            New();

        }
        private IEnumerable<z_Trainer> _Trainer;
        public IEnumerable<z_Trainer> Trainer
        {
            get { return _Trainer; }
            set { _Trainer = value; this.OnPropertyChanged("Trainer"); }
        }


        private z_Trainer _CurrentTrainer;
        public z_Trainer CurrentTrainer
        {
            get { return _CurrentTrainer; }
            set { _CurrentTrainer = value; this.OnPropertyChanged("CurrentTrainer");
            if (CurrentTrainer != null)
            {
                if (Employee != null)
                {
                    CurrentEmployee = Employee.FirstOrDefault(z => z.employee_id == CurrentTrainer.employee_id);
                }
                if (CurrentTrainer.is_visiting == true)
                {
                    IsVisiting = true;

                }
                else
                {
                    IsPermenent = true;
                }
            }

            }
        }

        private bool _IsPermenent;
        public bool IsPermenent
        {
            get { return _IsPermenent; }
            set { _IsPermenent = value; this.OnPropertyChanged("IsPermenent"); }
        }

        private bool _IsVisiting;
        public bool IsVisiting
        {
            get { return _IsVisiting; }
            set { _IsVisiting = value; this.OnPropertyChanged("IsVisiting"); }
        }
        

        private IEnumerable<mas_Employee> _Employee;
        public IEnumerable<mas_Employee> Employee
        {
            get { return _Employee; }
            set { _Employee = value; this.OnPropertyChanged("Employee"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        }
        
        

        public void GetAllTrainer()
        {
            this.serviceClient.GetAllTrainerCompleted += (s,e) =>
            {
                this.Trainer=e.Result.Where(z=>z.is_delete==false);
            };
            this.serviceClient.GetAllTrainerAsync();
        }

        public void GetAllEmployee()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employee = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetEmployeesAsync();
        }
        public ICommand NewBtn
        {
            get { return new RelayCommand(New, NewCanExecute); }
        }

        private void New()
        {
            CurrentTrainer = null;
            CurrentTrainer = new z_Trainer();
            CurrentTrainer.trainer_id = Guid.NewGuid();
            //CurrentEmployee = null;
            GetAllTrainer();

        }

        private bool NewCanExecute()
        {
            return true;
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }
        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }

        private void Delete()
        {
            DialogResult result = System.Windows.Forms.MessageBox.Show("Are you sure for Delete this Recode ?", "ERP Ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                CurrentTrainer.is_delete = true;

                try
                {
                    if (serviceClient.UpdateTrainer(CurrentTrainer))
                    {
                        System.Windows.MessageBox.Show("Trainer Delete Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.New();

                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Trainer Delete Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show("Trainer Delete Fail" + ex.InnerException.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        private bool DeleteCanExecute()
        {
            if (CurrentTrainer != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool SaveCanExecute()
        {
            if (CurrentTrainer.trainer_id == Guid.Empty)
            {
                return false;
            }
            if (CurrentTrainer.trainer_name == null)
            {
                return false;
            }
            else
            {
                return true;
            }



        }

        private void Save()
        {

            List<z_Trainer> oldTraining = new List<z_Trainer>();
            oldTraining = serviceClient.GetAllTrainer().ToList();
            z_Trainer trainer = oldTraining.FirstOrDefault(z => z.trainer_id.Equals(CurrentTrainer.trainer_id));
            CurrentTrainer.is_visiting = IsVisiting;
            if (IsVisiting == true)
            {
                CurrentTrainer.employee_id = Guid.Empty;
            }
            else
            {
                CurrentTrainer.employee_id = CurrentEmployee.employee_id;
            }
            if (trainer == null)
            {
                CurrentTrainer.is_delete = false;

                if (serviceClient.SaveTrainer(CurrentTrainer))
                {
                    System.Windows.MessageBox.Show("Trainer Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.New();
                }
                else
                {
                    System.Windows.MessageBox.Show("Trainer Save Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                if (serviceClient.UpdateTrainer(CurrentTrainer))
                {
                    System.Windows.MessageBox.Show("Trainer Update Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.New();
                }
                else
                {
                    System.Windows.MessageBox.Show("Trainer Update Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }
       
    }
}
