/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-04-23                                                                                                
*   Purpose    : Section View Model                                                                                                
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Masters => Payroll                                                        
*                                                                                                                
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
    class SectionViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public SectionViewModel()
        {

            this.reafreshDepartments();
            this.reafreshSections();
            this.New();
        }
        #endregion

        #region Properties {get,set}
        private IEnumerable<z_Section> _SectionName;
        public IEnumerable<z_Section> SectionName
        {
            get
            {
                return this._SectionName;
            }
            set
            {
                this._SectionName = value;
                this.OnPropertyChanged("SectionName");
            }
        }

        private z_Section _CurrentSectionName;
        public z_Section CurrentSectionName
        {
            get
            {
                return this._CurrentSectionName;
            }
            set
            {
                this._CurrentSectionName = value;
                this.OnPropertyChanged("CurrentSectionName");

            }
        }

        private IEnumerable<z_Department> _DepartmentName;
        public IEnumerable<z_Department> DepartmentName
        {
            get
            {
                return this._DepartmentName;
            }
            set
            {
                this._DepartmentName = value;
                this.OnPropertyChanged("DepartmentName");
            }
        }

        private z_Section _CurrentDepartmentName;
        public z_Section CurrentDepatmentName
        {
            get
            {
                return this._CurrentDepartmentName;
            }
            set
            {
                this._CurrentDepartmentName = value;
                this.OnPropertyChanged("CurrentDepatmentName");
            }
        }

        private List<string> _SelectionItems = new List<string>();
        public List<string> SelectionItems
        {
            get
            {
                return this._SelectionItems;
            }
            set
            {
                this._SelectionItems = value;
                this.OnPropertyChanged("SelectionItems");
            }
        }

        private string _SearchSelectionItem;
        public string SearchSelectionItem
        {
            get
            {
                return this._SearchSelectionItem;
            }
            set
            {
                this._SearchSelectionItem = value;
                this.OnPropertyChanged("SearchSelectionItem");
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
                    reafreshSections();
                }
                else
                {
                    SearchTextChanged();
                }

            }
        }

        #endregion

        #region Search Class
        public class relayCommand : ICommand
        {
            readonly Action<object> _Execute;
            readonly Predicate<object> _CanExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public relayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _Execute = execute;
                _CanExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _CanExecute == null ? true : _CanExecute(parameter);
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _Execute(parameter);
            }
        }

        #endregion

        #region Search Property
        relayCommand _OperationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_OperationCommand == null)
                {
                    _OperationCommand = new relayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }

                return this._OperationCommand;
            }
        }

        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region Search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }

        #endregion

        #region New Method
        void New()
        {
                this.CurrentSectionName = null;
                CurrentSectionName = new z_Section();
                CurrentSectionName.section_id = Guid.NewGuid();
                reafreshSections();
        }
        #endregion

        #region New Button Class & Property
        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        #endregion

        #region Save Method
        void Save()
        {
            bool isUpdate = false;
            z_Section newSection = new z_Section();
            newSection.section_id = CurrentSectionName.section_id;
            newSection.section_name = CurrentSectionName.section_name;
            newSection.department_id = CurrentSectionName.department_id;
            newSection.Emp_Count = CurrentSectionName.Emp_Count;
            newSection.Description = CurrentSectionName.Description;
            newSection.save_datetime = System.DateTime.Now;
            newSection.save_user_id = Guid.Empty;
            newSection.modified_datetime = System.DateTime.Now;
            newSection.modified_user_id = Guid.Empty;
            newSection.delete_user_id = Guid.Empty;
            newSection.delete_datetime = System.DateTime.Now;
            newSection.isdelete = false;

            IEnumerable<z_Section> allsections = this.serviceClient.GetSections();
            if (allsections != null)
            {
                foreach (z_Section Section in allsections)
                {
                    if (Section.section_id == CurrentSectionName.section_id)
                    {
                        isUpdate = true;
                    }
                }
            }
            if (newSection != null && newSection.section_name != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(206))
                    {
                        newSection.modified_user_id = clsSecurity.loggedUser.user_id;
                        newSection.modified_datetime = System.DateTime.Now;

                        if (ValidateEmpCount())
                        {
                            if (this.serviceClient.UpdateSection(newSection))
                            {
                                New();
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            }
                        } 
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
                else
                {
                    if (clsSecurity.GetSavePermission(206))
                    {
                        newSection.save_datetime = System.DateTime.Now;
                        newSection.save_user_id = clsSecurity.loggedUser.user_id;

                        if (ValidateEmpCount())
                        {
                            if (this.serviceClient.SaveSection(newSection))
                            {
                                New();
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            } 
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
            }

            else
            {
                clsMessages.setMessage("Please mention the Section Name...!");
            }
            reafreshSections();
        }

        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentSectionName != null)
            {
                if (CurrentSectionName.section_id == null || CurrentSectionName.section_id == Guid.Empty)
                    return false;
                if (CurrentSectionName.department_id == null || CurrentSectionName.department_id == Guid.Empty)
                    return false;
                if (CurrentSectionName.section_name == null || CurrentSectionName.section_name == string.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }
        #endregion

        #region Delete Method
        void Delete()
        {
            if (clsSecurity.GetDeletePermission(206))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do you want to delete this record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                z_Section sec = new z_Section();
                sec.section_id = CurrentSectionName.section_id;
                sec.delete_user_id = clsSecurity.loggedUser.user_id;
                sec.delete_datetime = System.DateTime.Now;


                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteSection(sec))
                    {
                        reafreshSections();
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        #endregion

        #region Delete Button Class & Property
        bool deleteCanExecute()
        {
            if (CurrentSectionName == null)
            {
                return false;
            }
            return true;
        }


        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }
        #endregion

        #region User Define Methods
        private void reafreshDepartments()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
                {
                    this.DepartmentName = e.Result.Where(a => a.isdelete == false);
                };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void reafreshSections()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
                {
                    this.SectionName = e.Result.Where(a => a.isdelete == false);
                };
            this.serviceClient.GetSectionsAsync();
        }


        //public void ReafreshSectionResults()
        //{
        //    this.serviceClient.GetSearchSectionsCompleted += (s, e) =>
        //        {
        //            this.SectionName = (IEnumerable<z_Section>)this.serviceClient.GetSearchSections(Search).Where(a=> a.isdelete==false);

        //        };
        //    this.serviceClient.GetSearchSectionsAsync(Search);
        //}
        #endregion

        #region Search Method for all Properties
        public void SearchTextChanged()
        {

            SectionName = SectionName.Where(e => e.section_name.ToUpper().Contains(Search.ToUpper()) && e.isdelete == false).ToList();
            DepartmentName = DepartmentName.Where(e => e.department_name.ToUpper().Contains(Search.ToUpper()) && e.isdelete == false).ToList();
        }

        #endregion

        #region Methods For Calcutale Employee Count

        private int CalculateEmployeeCount()
        {
            int DepEmpCount = (int)(DepartmentName == null ? 0 : DepartmentName.Where(c => c.department_id == CurrentSectionName.department_id).Sum(c => c.Emp_Count));
            int result = (int)(SectionName == null ? 0 : SectionName.Where(c => c.department_id == CurrentSectionName.department_id && c.section_id != CurrentSectionName.section_id).Sum(d => d.Emp_Count));
            int sum = 0;
            if (result == null || result == 0)
            {
                sum = DepEmpCount - 0;
            }
            else
            {
                sum = DepEmpCount - result;
            }
            return sum;
        }

        private bool ValidateEmpCount()
        {
            if (CurrentSectionName.Emp_Count > CalculateEmployeeCount())
            {
                clsMessages.setMessage("Employee count exceeds the total capacity by " + (CurrentSectionName.Emp_Count - Math.Abs(CalculateEmployeeCount())) + " please try again.");
                return false;
            }
            else
                return true;
        }
        #endregion
    }
}
