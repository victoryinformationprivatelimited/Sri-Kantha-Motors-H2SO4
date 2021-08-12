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
    public class TraninigProgramViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public TraninigProgramViewModel()
        {
            GetAllTrainingPrograms();
            GetAllTrainingCatagory();
            this.New();

        }
        private IEnumerable<view_TrainingPrograms> _TrainingPrograms;
        public IEnumerable<view_TrainingPrograms> TrainingPrograms
        {
            get { return _TrainingPrograms; }
            set { _TrainingPrograms = value; this.OnPropertyChanged("TrainingPrograms"); }
        }
        private view_TrainingPrograms _CurrentTraningPrograms;
        public view_TrainingPrograms CurrentTraningPrograms
        {
            get { return _CurrentTraningPrograms; }
            set { _CurrentTraningPrograms = value; this.OnPropertyChanged("CurrentTraningPrograms");
            if (CurrentTraningPrograms != null && CurrentTraningPrograms.catagory_id !=Guid.Empty )
            {
                CurrentTrainingCatagory = TraningCatagory.FirstOrDefault(z => z.catagory_id == CurrentTraningPrograms.catagory_id);
            }
            }
        }

        private IEnumerable<z_TrainingCategory> _TraningCatagory;
        public IEnumerable<z_TrainingCategory> TraningCatagory
        {
            get { return _TraningCatagory; }
            set { _TraningCatagory = value; this.OnPropertyChanged("TraningCatagory"); }
        }

        private z_TrainingCategory _CurrentTrainingCatagory;
        public z_TrainingCategory CurrentTrainingCatagory
        {
            get { return _CurrentTrainingCatagory; }
            set { _CurrentTrainingCatagory = value; this.OnPropertyChanged("CurrentTrainingCatagory"); }
        }
        

        public ICommand NewBtn
        {
            get { return new RelayCommand(New, NewCanExecute); }
        }

        private void New()
        {
            CurrentTraningPrograms = null;
            CurrentTraningPrograms = new view_TrainingPrograms();
            CurrentTraningPrograms.program_id = Guid.NewGuid();
            GetAllTrainingPrograms();
            CurrentTrainingCatagory = null;

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
                mas_TrainingProgram newTraining = new mas_TrainingProgram();
                newTraining.program_id = CurrentTraningPrograms.program_id;
                newTraining.catagory_id = CurrentTrainingCatagory.catagory_id;
                newTraining.program_name = CurrentTraningPrograms.program_name;
                newTraining.duration = CurrentTraningPrograms.duration;
                newTraining.program_description = CurrentTraningPrograms.program_description;
                newTraining.is_active = CurrentTraningPrograms.is_active;
                newTraining.is_delete = true;

                try
                {
                    if (serviceClient.UpdateMasTrainingPrograms(newTraining))
                    {
                        System.Windows.MessageBox.Show("Training Programs Delete Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.New();

                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Training Programs Delete Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show("Training Programs Delete Fail" + ex.InnerException.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        private bool DeleteCanExecute()
        {
            if (CurrentTraningPrograms != null)
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
            if (CurrentTraningPrograms.program_id == Guid.Empty)
            {
                return false;
            }
            if (CurrentTraningPrograms.program_name == null)
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

            List<mas_TrainingProgram> oldTraining = new List<mas_TrainingProgram>();
            oldTraining = serviceClient.GetMasTraining().ToList();
            CurrentTraningPrograms.catagory_id = CurrentTrainingCatagory.catagory_id;

            mas_TrainingProgram newTraining = new mas_TrainingProgram();
            newTraining.program_id = CurrentTraningPrograms.program_id;
            newTraining.catagory_id = CurrentTrainingCatagory.catagory_id;
            newTraining.duration = CurrentTraningPrograms.duration;
            newTraining.program_name = CurrentTraningPrograms.program_name;
            newTraining.program_description = CurrentTraningPrograms.program_description;
            newTraining.is_active = CurrentTraningPrograms.is_active;
            newTraining.is_delete = false;

            mas_TrainingProgram trainingCatagory = oldTraining.FirstOrDefault(z => z.program_id.Equals(CurrentTraningPrograms.program_id));
            if (trainingCatagory == null)
            {
               

                if (serviceClient.SaveMasTrainingPrograms(newTraining))
                {
                    System.Windows.MessageBox.Show("Training Programs Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.New();
                }
                else
                {
                    System.Windows.MessageBox.Show("Training Programs Save Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                if (serviceClient.UpdateMasTrainingPrograms(newTraining))
                {
                    System.Windows.MessageBox.Show("Training Programs Update Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.New();
                }
                else
                {
                    System.Windows.MessageBox.Show("Training Programs Update Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        public void GetAllTrainingPrograms()
        {
            this.serviceClient.GetMasTrainingProgramsFromViewCompleted += (s, e) =>
            {
                this.TrainingPrograms = e.Result.Where(z => z.is_delete == false);
            };
            this.serviceClient.GetMasTrainingProgramsFromViewAsync();
        }

        public void GetAllTrainingCatagory()
        {
            this.serviceClient.GetAllTraningCatagoryCompleted += (s, e) =>
            {
                this.TraningCatagory = e.Result.Where(z => z.is_delete == false && z.is_active==true);
            };
            this.serviceClient.GetAllTraningCatagoryAsync();
        }

        public ICommand SearchBtn
        {
            get { return new RelayCommand(Search, SearchCanExecute); }
        }

        private void Search()
        {
            throw new NotImplementedException();
        }

        private bool SearchCanExecute()
        {
            //if (SearchItem == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return true;

        }
        
    }
}
