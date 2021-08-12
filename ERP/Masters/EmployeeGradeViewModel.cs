/*************************************************************************************************************
 *   Author     :  Harshana Madusanka                                                                       *                    
 *   Date       :  13-05-2013                                                                                *             
 *   Purpose    :  Add Grade To Employee                                                    *                                    
 *   Company    :  Victory Information                                                                       *     
 *   Module     :  ERP System => Masters => Payroll                                                          * 
 *                                                                                                           *     
 ************************************************************************************************************/


using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP
{
    class EmployeeGradeViewModel : INotifyPropertyChanged
    {
        private ERPServiceClient serviceClient = new ERPServiceClient(); 
        public EmployeeGradeViewModel()
        {
                this.RefreshGrade();
                this._NewGrade = new NewButton(this);
                this._GradeSave = new SaveButton(this);
                this._GradeDelete = new DeleteButton(this);
                this.newRecode();
        }
        #region Properties
        private z_Grade _CurrentGrade;
        public z_Grade Currentgrade
        {
            get
            {
                return this._CurrentGrade;
            }
            set
            {
                this._CurrentGrade = value;
                //  SearchTextChanged();
                this.OnPropertyChanged("Currentgrade");

            }
        }

        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get
            {
                return this._Grades;
            }
            set
            {
                this._Grades = value;

                this.OnPropertyChanged("Grades");
            }
        }


        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;

            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (this._Search == "")
                {
                    this.RefreshGrade();
                }
                else
                {
                    SearchTextChanged();
                }
                    
               
            }

        }

        #endregion

        #region Event Propeties
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {

            var pargs = new PropertyChangedEventArgs(propertyName);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RefreshGrade()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                this.Grades = e.Result.Where(a=> a.isdelete==false);
            };
            this.serviceClient.GetGradeAsync();

        }
        #endregion

        #region Button Command

        #region Button Properties

        private NewButton _NewGrade;
        public NewButton NewGrade
        {
            get { return _NewGrade; }
        }

        private SaveButton _GradeSave;
        public SaveButton GradeSave
        {
            get { return _GradeSave; }
        }

        private DeleteButton _GradeDelete;
        public DeleteButton GradeDelete
        {
            get { return _GradeDelete; }
        }

        #endregion

        #region Button Class

        public class NewButton : ICommand
        {
            private EmployeeGradeViewModel View;
            public NewButton(EmployeeGradeViewModel View)
            {
                this.View = View;
                this.View.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };

            }
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

                this.View.newRecode();

            }
        }

        public class DeleteButton : ICommand
        {
            private EmployeeGradeViewModel View;
            public DeleteButton(EmployeeGradeViewModel View)
            {
                this.View = View;
                this.View.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }
            public bool CanExecute(object parameter)
            {
                if (this.View._CurrentGrade != null)
                {
                    return this.View._CurrentGrade != null;
                }
                return false;

            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.View.Delete();
                this.View.newRecode();
            }
        }

        public class SaveButton : ICommand
        {
            private EmployeeGradeViewModel View;

            public SaveButton(EmployeeGradeViewModel View)
            {
                this.View = View;
                this.View.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }
            public bool CanExecute(object parameter)
            {
                if (this.View._CurrentGrade != null)
                {
                    return this.View._CurrentGrade != null;
                }
                return false;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.View.Save();
                this.View.newRecode();

            }
        }

        #endregion

        #endregion

        #region User Define Methods

        #region Save Methord
        public void Save()
        {
            bool isUpdate = false;

            z_Grade newGrade = new z_Grade();
            newGrade.grade_id = Currentgrade.grade_id;
            newGrade.grade = Currentgrade.grade;
            newGrade.save_datetime = System.DateTime.Now;
            newGrade.save_user_id = clsSecurity.loggedUser.user_id;
            newGrade.modified_datetime = System.DateTime.Now;
            newGrade.modified_user_id = Guid.Empty;
            newGrade.delete_datetime = System.DateTime.Now;
            newGrade.delete_user_id = Guid.Empty;
            newGrade.isdelete = false;

            List<z_Grade> allgrade = serviceClient.GetGrade().ToList();

            foreach (z_Grade emp in allgrade)
            {
                if (emp.grade_id == _CurrentGrade.grade_id)
                {
                    isUpdate = true;
                }
            }

            if (newGrade != null && newGrade.grade != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(208))
                    {
                        newGrade.modified_datetime = System.DateTime.Now;
                        newGrade.modified_user_id = clsSecurity.loggedUser.user_id;

                        if (serviceClient.UpdateGrade(newGrade))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }

                }
                else
                {
                    if (clsSecurity.GetSavePermission(208))
                    {
                        if (serviceClient.SaveGrade(newGrade))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }

                RefreshGrade();

            }
            else
            {
                clsMessages.setMessage("Please mension emplpyee empty fileds");
            }
        }
        #endregion

        #region Delete Methord

        public void Delete()
        {
            if (clsSecurity.GetDeletePermission(208))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this record ?", "Quesion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    z_Grade grd = new z_Grade();
                    grd.grade_id = Currentgrade.grade_id;
                    grd.delete_user_id = clsSecurity.loggedUser.user_id;
                    grd.delete_datetime = System.DateTime.Now;
                    if (serviceClient.deleteGrade(grd))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    RefreshGrade();
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }
        #endregion

        #region New Recode
        public void newRecode()
        {
                Currentgrade = new z_Grade();
                Currentgrade.grade_id = Guid.NewGuid();
                RefreshGrade();
        }
        #endregion


        #endregion

        #region Grid Data Refresh
        public void SearchTextChanged()
        {
            Grades = Grades.Where(e => e.grade.ToUpper().Contains(Search.ToUpper())).ToList();

          
        }

     
        #endregion

        #region Search Key
        public class RelayCommand : ICommand
        {
            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }


            public bool CanExecute(object parameter)
            {
                return _canExecute == null ? true : _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }


        }
        #endregion

        #region Search Propaty

        RelayCommand _operationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_operationCommand == null)
                {
                    _operationCommand = new RelayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }
                return _operationCommand;
            }
        }

        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region search Comand Exicute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";

        }
        #endregion

        private double _scaleSize;
        public double ScaleSize
        {
            get { return _scaleSize; }
            set { _scaleSize = value; OnPropertyChanged("ScaleSize"); }
        }


        public void scale()
        {
            ScaleSize = 1;
            double h = System.Windows.SystemParameters.PrimaryScreenHeight;
            double w = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (h * w == 1366 * 768)
                ScaleSize = 0.90;
            if (w * h == 1280 * 768)
                ScaleSize = 0.90;
            if (w * h == 1024 * 768)
                ScaleSize = 1.5;
        }

    }
}
