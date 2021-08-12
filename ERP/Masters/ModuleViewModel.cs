/************************************************************************************************************
 *   Author     :  Heshantha Lakshitha                                                                       *                    
 *   Date       :  23-04-2013                                                                                *             
 *   Purpose    :  Keep the list of Master Modules                                                            *                                    
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
    class ModuleViewModel : ViewModelBase
    {
        #region Services Object
        private ERPServiceClient serviceClient = new ERPServiceClient(); 
        #endregion

        #region Constructor
        public ModuleViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.Module),clsSecurity.loggedUser.user_id))
            {
                this.RefreshModules();
                New(); 
            }
        }
        
        #endregion     

        #region Properties

        private IEnumerable<z_module> _Modules;

        public IEnumerable<z_module> Modules
        {
            get
            {
                return this._Modules;
            }
            set
            {
                this._Modules = value;
                this.OnPropertyChanged("Modules");
            }
        }

        private z_module _CurrentModule;

        public z_module CurrentModule
        {
            get
            {
                return this._CurrentModule;
            }
            set
            {
                this._CurrentModule = value;
                this.OnPropertyChanged("CurrentModule");
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
                    RefreshModules();
                }
                else
                {
                    searchTextChanged();
                }
               
            }
        }
        #endregion

        #region Button Command
        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }
       

  
        #endregion

        #region UserDefined Method
        #region Save Method
        private void Save()
        {
            if (validation())
            {
                bool isUpadte = false;

                z_module newModule = new z_module();
                newModule.module_id = CurrentModule.module_id;
                newModule.module_name = CurrentModule.module_name;
                newModule.isActive = CurrentModule.isActive;

                IEnumerable<z_module> allmodules = this.serviceClient.GetModules();

                if (allmodules != null)
                {
                    foreach (z_module module in allmodules)
                    {
                        if (module.module_id == CurrentModule.module_id)
                        {
                            isUpadte = true;
                        }
                    }
                }

                if (newModule != null && newModule.module_name != null)
                {
                    if (isUpadte)
                    {
                        if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.Module), clsSecurity.loggedUser.user_id))
                        {
                            if (this.serviceClient.UpdateModules(newModule))

                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            }
                        }
                    }
                    else
                    {
                        if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.Module),clsSecurity.loggedUser.user_id))
                        {
                            if (this.serviceClient.SaveModules(newModule))
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            } 
                        }
                    }
                }
                else
                {
                    clsMessages.setMessage("Please Mension Module Name  !");
                }

            }
            RefreshModules();            
        }

        bool saveCanExecute()
        {
            if (CurrentModule != null)
            {
                if (CurrentModule.module_id == null || CurrentModule.module_id == Guid.Empty)
                    return false;

                if (CurrentModule.module_name == null || CurrentModule.module_name == string.Empty)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }
        #endregion

        #region New Method
        private void New()
        {
            this.CurrentModule = null;
            CurrentModule = new z_module();
            CurrentModule.module_id = Guid.NewGuid();           
            RefreshModules();
        }

        bool newCanExecute()
        {
            return true;
        }
        #endregion

        #region DeleteMethod
        private void Delete()
        {
            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.Module),clsSecurity.loggedUser.user_id))
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteModules(CurrentModule))
                    {
                        RefreshModules();
                        MessageBox.Show("Record Deleted");
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        MessageBox.Show("You Cannot Delete This Module,Because This Module Related to another Operations...");
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                    RefreshModules();
                } 
            }
        }

        bool deleteCanExecute()
        {
            if (CurrentModule == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Refresh Method
        private void RefreshModules()
        {
            this.serviceClient.GetModulesCompleted += (s, e) =>
            {
                this.Modules = e.Result;
            };
            this.serviceClient.GetModulesAsync();
        } 
        #endregion

        #endregion

        #region Grid Data Refresh
        private void searchTextChanged()
        {
            Modules = Modules.Where(e => e.module_name.ToUpper().Contains(Search.ToUpper())).ToList();

        }
        #endregion

        #region Search Key
		public class relayCommand : ICommand
        {
            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {

            }
            public relayCommand(Action<object> execute, Predicate<object> canExecute)
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
                add
                {
                    CommandManager.RequerySuggested += value;
                }
                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }



            public void Execute(object parameter)
            {
                _execute(parameter);
            }
        }   
	#endregion

        #region Search Property
        relayCommand _operationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_operationCommand == null)
                {
                    _operationCommand = new relayCommand(param => this.ExecuteCommand(),
                        Param => this.CanExecuteCommand);
                }
                return _operationCommand;
            }
        }
        bool CanExecuteCommand
        {
            get
            {
                return true;
            }
        }  
        #endregion 
        
        #region search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }  
        #endregion

        #region Is Active Validation Method
        private bool validation()
        {
            string Message = "ERP System says..! please mention \n";
            bool isValidate = true;

            if (CurrentModule.isActive == null)
            {
                Message += "Method\n";
                isValidate = false;
            }
            if (!isValidate)
            {
                MessageBox.Show(Message, "ERP", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return isValidate;

        } 
        #endregion
    }
}

