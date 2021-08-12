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
    public class TrainingCatagoryViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public TrainingCatagoryViewModel()
        {
            GetAllTrainingCatagory();
            New();
        }

        private IEnumerable<z_TrainingCategory> _TrainingCatagory;
        public IEnumerable<z_TrainingCategory> TrainingCatagory
        {
            get { return _TrainingCatagory; }
            set { _TrainingCatagory = value; this.OnPropertyChanged("TrainingCatagory"); }
        }

        private z_TrainingCategory _CurrentTrainingCatagory;
        public z_TrainingCategory CurrentTrainingCatagory
        {
            get { return _CurrentTrainingCatagory; }
            set { _CurrentTrainingCatagory = value; this.OnPropertyChanged("CurrentTrainingCatagory"); }
        }

        private String _SearchItem;
        public String SearchItem
        {
            get { return _SearchItem; }
            set { _SearchItem = value; this.OnPropertyChanged("SearchItem"); }
        }
        

        public ICommand NewBtn
        {
            get { return new RelayCommand(New, NewCanExecute); }
        }

        private void New()
        {
            CurrentTrainingCatagory = null;
            CurrentTrainingCatagory = new z_TrainingCategory();
            CurrentTrainingCatagory.catagory_id = Guid.NewGuid();
            GetAllTrainingCatagory();

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
                CurrentTrainingCatagory.is_delete = true;

                try
                {
                    if (serviceClient.UpdateTraningCatagory(CurrentTrainingCatagory))
                    {
                        System.Windows.MessageBox.Show("Training Category Delete Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.New();
                       
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Training Category Delete Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    
                   System.Windows.MessageBox.Show("Training Category Delete Fail" +ex.InnerException.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        private bool DeleteCanExecute()
        {
            if (CurrentTrainingCatagory != null)
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
            if (CurrentTrainingCatagory.catagory_id == Guid.Empty)
            {
                return false;
            }
            if (CurrentTrainingCatagory.catagory_name == null)
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
          
            List<z_TrainingCategory> oldTraining = new List<z_TrainingCategory>();
            oldTraining = serviceClient.GetAllTraningCatagory().ToList();
            z_TrainingCategory trainingCatagory = oldTraining.FirstOrDefault(z => z.catagory_id.Equals(CurrentTrainingCatagory.catagory_id));
            if (trainingCatagory == null)
            {
                CurrentTrainingCatagory.is_delete = false;

                if (serviceClient.SaveTraningCatagory(CurrentTrainingCatagory))
                {
                    System.Windows.MessageBox.Show("Training Category Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.New();
                }
                else
                {
                    System.Windows.MessageBox.Show("Training Category Save Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                if (serviceClient.UpdateTraningCatagory(CurrentTrainingCatagory))
                {
                    System.Windows.MessageBox.Show("Training Category Update Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.New();
                }
                else
                {
                    System.Windows.MessageBox.Show("Training Category Update Fail", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        public void GetAllTrainingCatagory()
        {
            this.serviceClient.GetAllTraningCatagoryCompleted += (s, e) =>
            {
                this.TrainingCatagory = e.Result.Where(z => z.is_delete == false);
            };
            this.serviceClient.GetAllTraningCatagoryAsync();
        }

        public ICommand SearchBtn
        {
            get { return new RelayCommand(Search, SearchCanExecute); }
        }

        private bool SearchCanExecute()
        {
            if (SearchItem == null)
            {
                return false;
            }
            else
            {
                return true;
            }

      
        }

        private void Search()
        {
            List<z_TrainingCategory> SerchItemList = new List<z_TrainingCategory>();
            SerchItemList = TrainingCatagory.ToList();

            if (SearchItem != null)
            {
                TrainingCatagory = SerchItemList.Where(z => z.catagory_name.ToUpper().Contains(SearchItem.ToUpper().ToString()));
            }

        }
    }
}
