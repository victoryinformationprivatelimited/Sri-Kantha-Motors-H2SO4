/************************************************************************************************************
 *   Author     :  Heshantha Lakshitha                                                                       *                    
 *   Date       :  06-05-2013                                                                                *             
 *   Purpose    :  Keep the list of Master Company                                                            *                                    
 *   Company    :  Victory Information                                                                       *     
 *   Module     :  ERP System => Masters => Payroll                                                          * 
 *                                                                                                           *     
 ************************************************************************************************************/
using ERP.ERPService;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;


namespace ERP
{
    class CompanyViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        //bool nultipalecompany = false;
        #endregion

        string path = "";

        #region Constructor
        public CompanyViewModel()
        {
            scale();

            this.refrishCompany();
            //this.refreshCompanies();
            this.refreshCities();
            this.refreshTowns();
            this.New();
            path = "C:\\H2SO4\\LocalCompany\\";
        }
        #endregion

        #region Properties

        private IEnumerable<z_Company> _Companies;
        public IEnumerable<z_Company> Companies
        {
            get
            {
                return this._Companies;
            }
            set
            {
                this._Companies = value;
                this.OnPropertyChanged("Companies");
            }
        }

        private z_Company _CurrentCompany;
        public z_Company CurrentCompany
        {
            get
            {
                return this._CurrentCompany;
            }
            set
            {
                this._CurrentCompany = value;
                this.OnPropertyChanged("CurrentCompany");
                if (_CurrentCompany != null)
                {
                    ImagePath = CurrentCompany.image;
                }
            }
        }

        private IEnumerable<z_City> _Cities;
        public IEnumerable<z_City> Cities
        {
            get
            {
                return this._Cities;
            }
            set
            {
                this._Cities = value;
                this.OnPropertyChanged("Cities");
            }
        }

        private z_City _CurrentCity;
        public z_City CurrentCity
        {
            get
            {
                return this._CurrentCity;
            }
            set
            {
                this._CurrentCity = value;
                this.OnPropertyChanged("Currentcity");
                RichTextArea();
            }
        }

        private IEnumerable<z_Town> _Towns;
        public IEnumerable<z_Town> Towns
        {
            get
            {
                return this._Towns;
            }
            set
            {
                this._Towns = value;
                this.OnPropertyChanged("Towns");
            }
        }

        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get
            {
                return this._CurrentTown;
            }
            set
            {
                this._CurrentTown = value;
                this.OnPropertyChanged("CurrentTown");
            }
        }



        private string _TextArea;
        public string TextArea
        {
            get
            {
                return this._TextArea;
            }
            set
            {
                this._TextArea = value;
                this.OnPropertyChanged("TextArea");

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
                return new RelayCommand(save, saveCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(delete, deleteCanExecute);
            }
        }

        private void Clear()
        {
            Guid id = CurrentCompany.company_id;
            CurrentCompany = new z_Company();
            CurrentCompany.company_id = id;
        }

        #endregion

        #region UserDefined Method

        #region New Method

        private void New()
        {
                CurrentCompany = new z_Company();
                CurrentCompany.company_id = Guid.NewGuid();
        }

        bool newCanExecute()
        {
            return true;
        }
        #endregion

        #region Save Method

        private void save()
        {

            bool isUpdate = false;

            z_Company newCompany = new z_Company();
            newCompany.company_id = CurrentCompany.company_id;
            newCompany.company_name_01 = CurrentCompany.company_name_01;
            newCompany.comapany_name_02 = CurrentCompany.comapany_name_02;
            newCompany.address_01 = CurrentCompany.address_01;
            newCompany.address_02 = CurrentCompany.address_02;
            newCompany.address_03 = CurrentCompany.address_03;
            newCompany.city_id = CurrentCompany.city_id;
            newCompany.town_id = CurrentCompany.town_id;
            newCompany.email = CurrentCompany.email;
            newCompany.fax = CurrentCompany.fax;
            newCompany.image = CompanyImagePath;
            newCompany.telephone_01 = CurrentCompany.telephone_01;
            newCompany.telephone_02 = CurrentCompany.telephone_02;
            newCompany.web = CurrentCompany.web;
            newCompany.web_2 = CurrentCompany.web_2;
            newCompany.email_2 = CurrentCompany.email_2;
            newCompany.Description = CurrentCompany.Description;
            newCompany.company_capacity = CurrentCompany.company_capacity;

            IEnumerable<z_Company> allcompanies = this.serviceClient.GetCompanies();

            if (allcompanies != null)
            {
                foreach (z_Company company in allcompanies)
                {
                    if (company.company_id == CurrentCompany.company_id)
                    {
                        isUpdate = true;
                        break;
                    }
                }
            }
            if (newCompany != null && newCompany.company_name_01 != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(203))
                    {
                        if (this.serviceClient.UpdateCompanies(newCompany))
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        SaveImage();
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }

                }
                else
                {
                    int companyno = Companies.Count();
                    if (companyno <= 1)
                    {
                        if (clsSecurity.GetSavePermission(203))
                        {
                            if (this.serviceClient.SaveCompanies(newCompany))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                SaveImage();
                            }
                        }
                        else
                        {
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You Don't have Permission to Add Multiple Companies");
                        SetCompany();
                    }
                }



            }
            else
            {
                clsMessages.setMessage("Please mension company Name  !");
            }
        }

        bool saveCanExecute()
        {
            if (CurrentCompany != null)
            {
                if (CurrentCompany.company_id == null || CurrentCompany.company_id == Guid.Empty)
                    return false;

                if (CurrentCompany.company_name_01 == null || CurrentCompany.company_name_01 == string.Empty)
                    return false;

                if (CurrentCompany.address_01 == null || CurrentCompany.address_01 == string.Empty)
                    return false;

                if (CurrentCompany.address_02 == null || CurrentCompany.address_02 == string.Empty)
                    return false;

                if (CurrentCompany.address_03 == null || CurrentCompany.address_03 == string.Empty)
                    return false;

                if (CurrentCompany.city_id == null || CurrentCompany.city_id == Guid.Empty)
                    return false;

                if (CurrentCompany.town_id == null || CurrentCompany.town_id == Guid.Empty)
                    return false;

                if (CurrentCompany.telephone_01 == null || CurrentCompany.telephone_01 == string.Empty)
                    return false;

                if (CurrentCompany.email == null || CurrentCompany.email == string.Empty)
                    return false;

            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Delete Method

        private void delete()
        {
            if (clsSecurity.GetDeletePermission(203))
            {
                DialogResult Result = new DialogResult();
                Result = MessageBox.Show("Are you sure delete this record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.Yes)
                {
                    if (serviceClient.DeleteCompanies(CurrentCompany))
                    {
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

        bool deleteCanExecute()
        {
            if (CurrentCompany == null)
            {
                return false;
            }
            return true;
        }

        #endregion


        #region Image
        public ICommand ImageDelete
        {
            get { return new RelayCommand(deleteImage); }
        }
        void deleteImage()
        {
            CompanyImagePath = null;
            ImagePath = null;
            clsMessages.setMessage("Now Press Save button to Complete the Action");
        }

        private string _employeeImagePath;
        public string CompanyImagePath
        {
            get { return _employeeImagePath; }
            set { _employeeImagePath = value; OnPropertyChanged("EmployeeImagePath"); }
        }


        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; OnPropertyChanged("ImagePath"); }
        }

        private Image _Image;
        public Image Image
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
                string DirectoryPath = path + "\\" + CurrentCompany.company_id + "\\";
                if (Directory.Exists(DirectoryPath) == false)
                {
                    Directory.CreateDirectory(DirectoryPath);
                    Image.Save(CompanyImagePath, ImageFormat.Jpeg);
                }
                else
                    if (File.Exists(CompanyImagePath))
                    {
                        try
                        {
                            Image.Save(CompanyImagePath, ImageFormat.Jpeg);
                        }
                        catch (Exception)
                        {

                            clsMessages.setMessage("Please Close the Application and Restart to Set the Image");
                        }
                    }
                    else
                        Image.Save(CompanyImagePath, ImageFormat.Jpeg);

                ImagePath = CompanyImagePath;
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

        void ImageNameandPath()
        {
            Guid FileName = new Guid();
            FileName = Guid.NewGuid();
            CompanyImagePath = path + CurrentCompany.company_id + "\\" + FileName + ".Jpeg";

        }
        #endregion

        #region TextArea Method
        private void RichTextArea()
        {
            if (CurrentCompany.address_01 != null && CurrentCompany.address_02 != null && CurrentCompany.address_03 != null &&
                CurrentCity.city != null && CurrentTown.town_name != null
                )


                TextArea += CurrentCompany.address_01 + ".\n";
            TextArea += CurrentCompany.address_02 + ".\n";
            TextArea += CurrentCompany.address_03 + ".\n";
            TextArea += CurrentCity.city + ".\n";
            TextArea += CurrentTown.town_name + ".\n";


        }
        #endregion

        #region Refresh Method

        private void refrishCompany()
        {
            this.serviceClient.GetCompaniesCompleted += (s, e) =>
                {
                    this.Companies = e.Result;
                    SetCompany();
                };
            this.serviceClient.GetCompaniesAsync();
        }

        private void refreshCities()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
                {
                    this.Cities = e.Result;

                };
            this.serviceClient.GetCitiesAsync();
        }

        private void refreshTowns()
        {
            this.serviceClient.GetTownsCompleted += (s, e) =>
                {
                    this.Towns = e.Result;

                };
            this.serviceClient.GetTownsAsync();
        }

        #endregion

        private void SetCompany()
        {

            if (_Companies != null)
            {
                CurrentCompany = Companies.Where(c => c.company_id != Guid.Empty).FirstOrDefault();
                if (_CurrentCompany == null)
                    this.New();
            }


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
                ScaleSize = 1.2;
        }


    }
}
