using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using ERP.BasicSearch;
using ERP.Base;

namespace ERP
{
    public class UserMasterViewModel : INotifyPropertyChanged
    {
        private ERPServiceClient serviceClinet = new ERPServiceClient();

        #region Constructor
        string path = "C:\\H2SO4\\LocalUserImages\\";

        public UserMasterViewModel()
        {
            this.refreshUsers();
            this.refreshUserLevel();
            this._NewRecordButton = new NewButton(this);
            this._SaveRecordButton = new SaveButton(this);
            this._DeleteRecordButton = new DeleteButton(this);
            this._ConfirmPasswordButton = new ConfirmButton(this);
            this._ChoosePhotoButton = new ChoiceButton(this);
            // this._DeleteImage = new DeleteImageButton(this);
            this.newRecord();
        }

        #endregion

        #region Event

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Properties
        private IEnumerable<EmployeeSearchView> _employeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _employeeSearch; }
            set { _employeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        private IEnumerable<usr_User> _UserDerails;
        public IEnumerable<usr_User> UserDerails
        {
            get
            {
                return this._UserDerails;
            }
            set
            {
                this._UserDerails = value;
                OnPropertyChanged("UserDerails");
            }
        }

        private usr_User _CurrentUser;
        public usr_User CurrentUser
        {
            get
            {
                return this._CurrentUser;
            }
            set
            {
                this._CurrentUser = value;
                OnPropertyChanged("CurrentUser");
                ImagePath = null;
                if (CurrentUser != null) 
                {
                    if(CurrentUser.image != null)
                    ImagePath = CurrentUser.image;
                    CurrentUser.user_password = StringCipher.Decrypt(CurrentUser.user_password, "12");

                    
                }

            }
        }

        private IEnumerable<usr_UserLevel> _UserLevel;
        public IEnumerable<usr_UserLevel> UserLevel
        {
            get
            {
                return this._UserLevel;
            }
            set
            {
                this._UserLevel = value;
                this.OnPropertyChanged("UserLevel");
            }
        }

        private usr_UserLevel _CurrentUserLevel;
        public usr_UserLevel CurrentUserLevel
        {
            get
            {
                return this._CurrentUserLevel;
            }
            set
            {
                this._CurrentUserLevel = value;
                this.OnPropertyChanged("CurrentUserLevel");
            }
        }

        private string _PasswordConfirm;
        public string PasswordConfirm
        {
            get
            {
                return this._PasswordConfirm;
            }
            set
            {
                this._PasswordConfirm = value;
            }
        }

        #endregion

        #region Button Classes

        #region Button Properties

        private NewButton _NewRecordButton;
        public NewButton NewRecordButton
        {
            get { return this._NewRecordButton; }
        }

        private SaveButton _SaveRecordButton;
        public SaveButton SaveRecordButtton
        {
            get { return this._SaveRecordButton; }
        }


        private DeleteButton _DeleteRecordButton;
        public DeleteButton DeleteRecordButton
        {
            get { return this._DeleteRecordButton; }
        }

        private ConfirmButton _ConfirmPasswordButton;
        public ConfirmButton ConfirmPasswordButton
        {
            get
            {
                return this._ConfirmPasswordButton;
            }
        }

        //private Choice _ConfirmPasswordButton;
        //public ConfirmButton ConfirmPasswordButton
        //{
        //    get
        //    {
        //        return this._ConfirmPasswordButton;
        //    }
        //}

        private ChoiceButton _ChoosePhotoButton;
        public ChoiceButton ChoosePhotoButton
        {
            get { return this._ChoosePhotoButton; }
        }

        //private DeleteImageButton _DeleteImage;
        //public DeleteImageButton DeleteImage
        //{
        //    get { return this._DeleteImage; }
        //}

        #endregion

        public class NewButton : ICommand
        {
            private UserMasterViewModel ViewModel;

            public NewButton(UserMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
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
                this.ViewModel.newRecord();
            }

        }
        public class ChoiceButton : ICommand
        {
            private UserMasterViewModel ViewModel;

            public ChoiceButton(UserMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
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
                this.ViewModel.choosePhoto();
            }

        }

        public class SaveButton : ICommand
        {
            private UserMasterViewModel ViewModel;

            public SaveButton(UserMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return this.ViewModel.CurrentUser != null && this.ViewModel.CurrentUserLevel != null;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                var password = parameter as PasswordBox;
                this.ViewModel.saveRecord();

            }
        }

        public class DeleteButton : ICommand
        {
            private UserMasterViewModel ViewModel;

            public DeleteButton(UserMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return this.ViewModel.CurrentUser != null;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.deleteRecord();
            }
        }

        public class ConfirmButton : ICommand
        {
            private UserMasterViewModel ViewModel;

            public ConfirmButton(UserMasterViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
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
                if (parameter != null)
                {
                    var password = parameter as PasswordBox;
                    this.ViewModel.PasswordConfirm = password.Password;
                }
            }
        }

        //public class ChooseButton : ICommand
        //{
        //    private UserMasterViewModel ViewModel;

        //    public ChooseButton(UserMasterViewModel ViewModel)
        //    {
        //        this.ViewModel = ViewModel;
        //        this.ViewModel.PropertyChanged += (s, e) =>
        //        {
        //            if (this.CanExecuteChanged != null)
        //            {
        //                this.CanExecuteChanged(this, new EventArgs());
        //            }
        //        };
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return true;
        //        // return this.ViewModel.CurrentUser != null;
        //    }

        //    public event EventHandler CanExecuteChanged;

        //    public void Execute(object parameter)
        //    {
        //        this.ViewModel.choosePhoto();

        //    }
        //}

        //public class DeleteImageButton : ICommand
        //{

        //    private UserMasterViewModel ViewModel;

        //    public DeleteImageButton(UserMasterViewModel ViweModel)
        //    {
        //        this.ViewModel = ViweModel;
        //        this.ViewModel.PropertyChanged += (s, e) =>
        //        {
        //            if (this.CanExecuteChanged != null)
        //            {
        //                this.CanExecuteChanged(this, new EventArgs());
        //            }
        //        };
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return true;
        //    }

        //    public event EventHandler CanExecuteChanged;

        //    public void Execute(object parameter)
        //    {
        //        this.ViewModel.CurrentUser.image = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "person.gif");
        //    }
        //}

        #endregion

        #region User defind Method

        private void refreshUsers()
        {
            this.serviceClinet.GetUsersCompleted += (s, e) =>
                {
                    this.UserDerails = e.Result.OrderBy(c => c.user_name);
                };
            this.serviceClinet.GetUsersAsync();

            // CurrentUser.image = "Images/person.jpg";
        }

        private void refreshUserLevel()
        {

            this.UserLevel = this.serviceClinet.GetUserLevel().OrderBy(c => c.user_level);
        }

        private void newRecord()
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.UserMaster), clsSecurity.loggedUser.user_id))
            //{
            UserDerails = null;
            CurrentUser = new usr_User();
            CurrentUser.user_id = Guid.NewGuid();
            refreshUsers();
            CurrentUser.isAcvtive = true;
            CurrentUser.is_App_User = true;
            CurrentUser.is_Web_Portal_User = false;
            //}
            //else
            //{
            //    clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            //}
        }


        private void saveRecord()
        {
            if (validation())
            {
                bool isUpdate = false;
                bool canContinue = true;
                usr_User newUser = new usr_User();
                newUser.user_id = CurrentUser.user_id;
                newUser.user_level_id = CurrentUserLevel.user_level_id;
                newUser.user_name = CurrentUser.user_name;
                newUser.name = CurrentUser.name;
                newUser.image = ImagePath == null ? string.Empty : ImagePath;
                newUser.user_password = CurrentUser.user_password == null ? string.Empty : CurrentUser.user_password;
                newUser.isAcvtive = CurrentUser.isAcvtive;
                newUser.is_App_User = CurrentUser.is_App_User;
                newUser.is_Web_Portal_User = CurrentUser.is_Web_Portal_User;
                newUser.is_mobile_app_user = CurrentUser.is_mobile_app_user;

                if (newUser.user_password == CurrentUser.user_password && CurrentUser.user_password.Trim().Length > 0)
                {
                    PasswordConfirmBox PCB = new PasswordConfirmBox(this);
                    do
                    {
                        clsMessages.setMessage("Please Confirm Password");
                        PCB.ShowDialog();

                    } while (PasswordConfirm == null);

                    //PCB.Visibility = Visibility.Hidden;

                    if (CurrentUser.user_password != PasswordConfirm)
                    {
                        canContinue = false;
                        clsMessages.setMessage("Password Confirmation Dosen't match please try again..!");
                    }
                    else
                    {

                        newUser.user_password = PasswordConfirm;
                    }
                }

                if (canContinue)
                {

                    IEnumerable<usr_User> allUsers = this.serviceClinet.GetUsers();

                    foreach (usr_User user in allUsers)
                    {
                        if (user.user_id == CurrentUser.user_id)
                        {
                            isUpdate = true;
                        }
                    }

                    if (newUser != null && newUser.user_name != null && newUser.name != null)
                    {

                        if (isUpdate)
                        {
                            if (clsSecurity.GetUpdatePermission(102))
                            {
                                newUser.user_password = StringCipher.Encrypt(newUser.user_password, "12");
                                this.serviceClinet.updateUser(newUser);
                                clsMessages.setMessage("Record modify Successfully !");
                                #region Image Save
                                if (Image == null)
                                    ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "person.gif");
                                if (Image != null)
                                    SaveImage();

                                refreshUsers();
                                refreshUserLevel();
                                newRecord();
                                #endregion
                            }
                            else
                            {
                                clsMessages.setMessage("You don't have permission to Update in this form...");
                            }
                        }
                        else
                        {
                            if (clsSecurity.GetSavePermission(102))
                            {
                                var result = UserDerails.FirstOrDefault(c => c.user_name == CurrentUser.user_name);
                                if (result == null)
                                {
                                    newUser.user_password = StringCipher.Encrypt(newUser.user_password, "12");
                                    this.serviceClinet.saveUsers(newUser);
                                    clsMessages.setMessage("Record Saved Successfully !");

                                    #region Image Save
                                    if (Image != null)
                                        SaveImage();
                                    #endregion
                                    refreshUsers();
                                    refreshUserLevel();
                                    newRecord();
                                }
                                else
                                {
                                    clsMessages.setMessage("You Cannot Enter Already Exist Username...");
                                }
                            }
                            else
                            {
                                clsMessages.setMessage("You don't have permission to Save in this form...");
                            }

                        }
                        //  refreshUsers();
                        // refreshUserLevel();
                        //newRecord();
                    }
                    else
                    {
                        MessageBox.Show("Please mention employee Name fields and employee salary fields !");
                    }
                }
            }
        }

        private void choosePhoto()
        {
            if (DialogResult.Yes == MessageBox.Show("Do you want change profile picture ?", "ERP", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.UserMaster), clsSecurity.loggedUser.user_id))
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
                    fileDialog.FilterIndex = 3;
                    fileDialog.RestoreDirectory = true;
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string staeredpat = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                        string destinationPath = staeredpat + "\\Img\\";
                        string files = fileDialog.InitialDirectory + fileDialog.FileName;

                        try
                        {
                            System.IO.File.Copy(files, System.IO.Path.Combine(destinationPath, System.IO.Path.GetFileName(files)));
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        CurrentUser.image = destinationPath + fileDialog.FileName;
                    }
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForDelete);
            }
        }

        private bool validation()
        {
            if (CurrentUser.user_name == null)
            {
                clsMessages.setMessage("Please Enter Username...");
                return false;
            }
            else if (CurrentUser.name == null)
            {
                clsMessages.setMessage("Please Enter Name...");
                return false;
            }
            else if (CurrentUser.user_level_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A User Level...");
                return false;
            }
            else
                return true;
            //string Message = "ERP System says..! please mention \n";
            //bool isValidate = true;           

            //if (CurrentUser.user_name == null || CurrentUser.user_name.Trim().Length <= 0 )            
            //{
            //    Message += "User name\n";
            //    isValidate = false;
            //}
            //if (CurrentUser.name == null || CurrentUser.name.Trim().Length <= 0)
            //{
            //    Message += "Name\n";
            //    isValidate = false;
            //}
            //Message += " fileds";
            //if (!isValidate)
            //{
            //    MessageBox.Show(Message,"ERP",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //}
            //var result = UserDerails.FirstOrDefault(c=> c.user_name == CurrentUser.user_name);
            //if (result != null)
            //{
            //    clsMessages.setMessage("You Cannot Enter Already Existing Username...");
            //}
            //return isValidate;
        }

        #endregion

        #region Image

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; OnPropertyChanged("ImagePath"); }
        }

        private System.Drawing.Image _Image;
        public System.Drawing.Image Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged("Image"); }
        }

        public ICommand ImageButton
        {
            get { return new RelayCommand(browseImage); }
        }

        void SaveImage()
        {
            try
            {
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                    Image.Save(path + CurrentUser.user_id + ".Jpeg", ImageFormat.Jpeg);
                }
                else
                    if (File.Exists(path + CurrentUser.user_id + ".Jpeg"))
                    {
                        File.Delete(path + CurrentUser.user_id + ".Jpeg");
                        Image.Save(path + CurrentUser.user_id + ".Jpeg", ImageFormat.Jpeg);
                    }
                    else
                        Image.Save(path + CurrentUser.user_id + ".Jpeg", ImageFormat.Jpeg);
            }
            catch (Exception)
            {
            }
        }

        void browseImage()
        {
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Image = new Bitmap(open.FileName);
                    ImagePath = open.FileName;
                    //string imagename = open.SafeFileName;
                    ImageNameandPath();
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        public ICommand SelectButton
        {
            get { return new RelayCommand(Select); }
        }

        void Select()
        {
            List<EmployeeSearchView> tempEmployeeSearch = new List<EmployeeSearchView>();
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            EmployeeSearch = null;
            if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                EmployeeSearch = window.viewModel.selectEmployeeList;
            window.Close();
            window = null;
            newRecord();
            if (EmployeeSearch != null)
            {
                EmployeeSearchView temp_emp = new EmployeeSearchView();
                temp_emp = EmployeeSearch.First();
                if (temp_emp != null)
                {
                    CurrentUser.user_name = temp_emp.first_name;
                    CurrentUser.name = temp_emp.first_name;
                }
            }
        }

        private void deleteRecord()
        {
            if (clsSecurity.GetDeletePermission(102))
            {
                try
                {
                    if (CurrentUser.user_id != clsSecurity.loggedUser.user_id)
                    {
                        usr_User DeleteUser = new usr_User();
                        DeleteUser.user_id = CurrentUser.user_id;
                        DeleteUser.user_level_id = CurrentUserLevel.user_level_id;
                        DeleteUser.user_name = CurrentUser.user_name;
                        DeleteUser.name = CurrentUser.name;
                        DeleteUser.image = path + CurrentUser.user_id + ".Jpeg";
                        DeleteUser.user_password = CurrentUser.user_password;
                        DeleteUser.isAcvtive = CurrentUser.isAcvtive;
                        // DeleteUser.isAcvtive = false;

                        if (DeleteUser != null && DeleteUser.user_name != null && DeleteUser.name != null)
                        {
                            this.serviceClinet.deleteUser(DeleteUser);
                            clsMessages.setMessage("Record Deleted Successfully !");
                            refreshUsers();
                            refreshUserLevel();
                            newRecord();
                        }
                    }
                    else
                        clsMessages.setMessage("Please login from another account and delete.");
                }
                catch (Exception)
                {

                }
            }
            else
                clsMessages.setMessage("You don't have permission to Delete in this form...");
        }



        #region NewCodes 15-8-2016

        private string _employeeImagePath;
        public string EmployeeImagePath
        {
            get { return _employeeImagePath; }
            set { _employeeImagePath = value; OnPropertyChanged("EmployeeImagePath"); }
        }
        void ImageNameandPath()
        {
            Guid FileName = new Guid();
            FileName = Guid.NewGuid();
            EmployeeImagePath = path + CurrentUser.user_id + "\\" + FileName + ".Jpeg";
        }
        public ICommand DeleteImage
        {
            get { return new RelayCommand(deleteImage); }
        }
        void deleteImage()
        {
            if (ImagePath != null)
            {
                ImagePath = null;
                CurrentUser.image = null;
            }
            else
                clsMessages.setMessage("There Is No Image To Delete...");
            // clsMessages.setMessage("Now Press Save button to Complete the Action");
        }
        #endregion


    }
}
