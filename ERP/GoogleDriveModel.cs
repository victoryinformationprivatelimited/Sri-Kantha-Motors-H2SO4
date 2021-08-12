using CustomBusyBox;
using ERP.ERPService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ERP
{
    class GoogleDriveModel
    {
        private string[] Scopes = { DriveService.Scope.Drive };
        private string ApplicationName = "IMAGE-UPLOAD-API";
        string path = @"folderId.txt";
        string folderId = "";
        private FilesResource.ListRequest listRequest;
        private static readonly object Instancelock = new object();
        private static int counter = 0;
        private static GoogleDriveModel instance = null;
        DriveService service;

        #region Service Client

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion Service Client

        private GoogleDriveModel()
        {
            counter++;
        }

        public static GoogleDriveModel GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (Instancelock)
                    {
                        if (instance == null)
                        {
                            instance = new GoogleDriveModel();
                        }
                    }
                }
                return instance;
            }
        }

        public void getAuth()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            IList<Google.Apis.Drive.v3.Data.File> fileList = listFiles();
            if (fileList.Count != 0)
            {
                bool empty = true;
                foreach (var x in fileList)
                {
                    if (x.Name == "Employee_IMG")
                    {
                        empty = false;
                        storeFolderId(x.Id);
                        readFolderId();
                    }
                }
                if (empty)
                {
                    Google.Apis.Drive.v3.Data.File file = CreateFolder("Employee_IMG", null);
                    storeFolderId(file.Id);
                    readFolderId();
                }
            }
            else
            {
                Google.Apis.Drive.v3.Data.File file = CreateFolder("Employee_IMG", null);
                storeFolderId(file.Id);
                readFolderId();
            }

        }

        public IList<Google.Apis.Drive.v3.Data.File> listFiles()
        {
            List<Google.Apis.Drive.v3.Data.File> allFiles = new List<Google.Apis.Drive.v3.Data.File>();

            Google.Apis.Drive.v3.Data.FileList result = null;
            while (true)
            {
                if (result != null && string.IsNullOrWhiteSpace(result.NextPageToken))
                    break;

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(id, name)";
                if (result != null)
                    listRequest.PageToken = result.NextPageToken;

                result = listRequest.Execute();
                allFiles.AddRange(result.Files);
            }

            return allFiles;
        }

        public void storeFolderId(string folderId)
        {
            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(folderId);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

            }
            catch
            {
            }
        }

        public void readFolderId()
        {
            // Open the stream and read it back.
            string result = "";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    result = s;
                }
            }
            folderId = result;
        }

        public string getFolderId()
        {
            return folderId;
        }

        public Google.Apis.Drive.v3.Data.File CreateFolder(string name, string id)
        {

            if (id == null)
            {
                id = "root";
            }

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new[] { id }
            };

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType";

            return request.Execute();
        }

        public string upload(string imagePath, string employeeId, string id = "root")
        {
            Google.Apis.Drive.v3.Data.File fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = employeeId,
                Parents = new[] { id }
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(imagePath,
                                    System.IO.FileMode.Open))
            {
                request = service.Files.Create(
                    fileMetadata, stream, "image/jpg");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            return file.Id;
        }

        public MemoryStream Download(string fileId)
        {
            MemoryStream outputstream = new MemoryStream();
            var request = service.Files.Get(fileId);

            request.Download(outputstream);

            outputstream.Position = 0;

            return outputstream;
        }

        public void delete(string fileId)
        {
            service.Files.Delete(fileId).Execute();
        }

        public bool portalImageSync()
        {
            bool allSynced = false;
            bool syncSuccess = false;
            IEnumerable<mas_Employee> empList = serviceClient.GetEmployees();
            path = ConfigurationManager.AppSettings["ImagePath"].ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var x in empList)
            {
                string PortalImageId;
                if (x.image != null && x.portalImage == null)
                {
                    string image = x.image.ToString().Split('\\')[2];
                    PortalImageId = upload(Directory.GetCurrentDirectory() + "\\EmployeeImages\\" + x.employee_id + ".jpeg", x.employee_id.ToString() + ".jpeg", folderId);
                    x.portalImage = PortalImageId;
                    x.dtl_Employee = serviceClient.GetDetailEmployees(x.employee_id)[0];
                    if (serviceClient.UpdateEmployee(x, null, null))
                    {
                        syncSuccess = true;
                    }
                    else
                    {
                        clsMessages.setMessage("Portal Image Sync failed");
                        GoogleDriveModel.GetInstance.delete(PortalImageId);
                    }

                }
                else if (x.image == null && x.portalImage != null)
                {
                    if (!Directory.Exists(x.employee_id.ToString()))
                    {
                        MemoryStream stream = Download(x.portalImage.ToString());
                        SaveStream(stream, "EmployeeImages\\" + x.employee_id + ".jpeg");

                        Directory.CreateDirectory("EmployeeImages\\" + x.employee_id.ToString());
                        SaveStream(stream, "EmployeeImages\\" + x.employee_id.ToString() + "\\" + x.employee_id + ".jpeg");
                        x.image = "EmployeeImages\\" + x.employee_id.ToString() + "\\" + x.employee_id + ".jpeg";
                        x.dtl_Employee = serviceClient.GetDetailEmployees(x.employee_id)[0];
                        if (serviceClient.UpdateEmployee(x, null, null))
                        {
                            syncSuccess = true;
                        }
                    }
                }
                else if (x.image != null && x.portalImage != null)
                {
                    if (!Directory.Exists("EmployeeImages\\" + x.employee_id.ToString()))
                    {
                        MemoryStream stream = Download(x.portalImage.ToString());
                        SaveStream(stream, "EmployeeImages\\" + x.employee_id + ".jpeg");

                        string image = x.image.ToString().Split('\\')[2];

                        Directory.CreateDirectory("EmployeeImages\\" + x.employee_id.ToString());
                        SaveStream(stream, "EmployeeImages\\" + x.employee_id.ToString() + "\\" + image);
                        syncSuccess = true;
                    }
                }
                else if (x.image == null && x.portalImage == null)
                {
                    IList<Google.Apis.Drive.v3.Data.File> fileList = listFiles();
                    bool found = false;
                    BusyBox.ShowBusy("Sync Process Started...");
                    foreach (var y in fileList)
                    {
                        if (y.Name != "Employee_IMG")
                        {
                            if (x.employee_id.ToString() == y.Name.Split('.')[0])
                            {
                                if (!Directory.Exists(x.employee_id.ToString()))
                                {
                                    MemoryStream stream = Download(y.Id);
                                    SaveStream(stream, "EmployeeImages\\" + x.employee_id + ".jpeg");

                                    Directory.CreateDirectory("EmployeeImages\\" + x.employee_id.ToString());
                                    SaveStream(stream, "EmployeeImages\\" + x.employee_id.ToString() + "\\" + x.employee_id + ".jpeg");
                                    x.image = "EmployeeImages\\" + x.employee_id.ToString() + "\\" + x.employee_id + ".jpeg";
                                    x.portalImage = y.Id;
                                    x.dtl_Employee = serviceClient.GetDetailEmployees(x.employee_id)[0];
                                    if (serviceClient.UpdateEmployee(x, null, null))
                                    {
                                        syncSuccess = true;
                                        found = true;
                                    }
                                }
                            }
                        }
                        if (found)
                            break;
                    }
                }
                else
                {
                    allSynced = true;
                }
            }
            /*if (allSynced && syncSuccess == false)
                clsMessages.setMessage("All Synced");
            if (syncSuccess)
                clsMessages.setMessage("Synced successfully!");*/
            if (allSynced || syncSuccess)
                return true;
            else
                return false;
        }

        private void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }

    }
}
