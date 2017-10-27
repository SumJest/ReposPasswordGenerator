using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Download;

namespace PasswordGenerator
{
    class GoogleDrive
    {
        private string[] Scopes = { DriveService.Scope.Drive };
        private string ApplicationName = "PasswordGenerator";
        List<Google.Apis.Drive.v3.Data.File> files = new List<Google.Apis.Drive.v3.Data.File>();
        List<Google.Apis.Drive.v3.Data.File> folders = new List<Google.Apis.Drive.v3.Data.File>();
        private UserCredential credential;
        private DriveService service;
        string profile = "";
        string folderId = "";
        private Form1 form;

        public GoogleDrive(string profile, Form1 form)
        {
            this.form = form;
            this.profile = profile;
            string msg = string.Format("Loading credentials of profile: {0}...", profile);
            Console.WriteLine(msg);
            form.toolStripStatusLabel1.Text = msg;
            credential = GetUserCredential();
            msg = "Credentials loaded. Loading DriveService...";
            Console.WriteLine(msg);
            form.toolStripStatusLabel1.Text = msg;
            service = GetDriveService();
            msg = "DriveService loaded.";
            Console.WriteLine(msg);
            form.toolStripStatusLabel1.Text = msg;
            msg = "Loading list of items...";
            Console.WriteLine(msg);
            form.toolStripStatusLabel1.Text = msg;
            IList<Google.Apis.Drive.v3.Data.File> items = service.Files.List().Execute().Files;
            msg = "List of items loaded. Sorting...";
            Console.WriteLine(msg);
            form.toolStripStatusLabel1.Text = msg;
            foreach (var file in items)
            {
                if (file.MimeType == "application/vnd.google-apps.folder") folders.Add(file);
                files.Add(file);
            }
            msg = "Complete.";
            Console.WriteLine(msg);
            form.toolStripStatusLabel1.Text = msg;
            bool contains = false;
            foreach (var file in folders) { if (file.Name == "Passwords") { contains = true; folderId = file.Id; } break; }
            if (!contains)
            {
                Console.WriteLine("Creating folder \"Passwords\"...");
                form.toolStripStatusLabel1.Text = "Creating folder \"Passwords\"...";
                if (!string.IsNullOrEmpty(CreateFolder("Passwords"))) { Console.WriteLine("Folder created."); form.toolStripStatusLabel1.Text = "Folder created."; }
            else Console.WriteLine("[Error] Folder not created!"); 
            }
            Console.WriteLine("Folder id: " + folderId);
        }
        public void Update()
        {
            Console.WriteLine("Loading list of items...");
            form.toolStripStatusLabel1.Text = "Loading list of items...";
            IList<Google.Apis.Drive.v3.Data.File> items = service.Files.List().Execute().Files;
            files.Clear();
            Console.WriteLine("List of items loaded. Sorting...");
            form.toolStripStatusLabel1.Text = "List of items loaded. Sorting...";
            foreach (var file in items)
            {
                if (file.MimeType == "application/vnd.google-apps.folder") folders.Add(file);
                files.Add(file);
            }
            Console.WriteLine("Sorted " + files.Count + " files");
            form.toolStripStatusLabel1.Text = "Sorted " + files.Count + " files";
        }
        private string CreateFolder(string folderName)
        {
            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = "Passwords";
            file.MimeType = "application/vnd.google-apps.folder";

            var request = service.Files.Create(file);
            request.Fields = "id";

            var result = request.Execute();
            folderId = result.Id;
            return result.Id;
        }
        private UserCredential GetUserCredential()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                creadPath = Path.Combine(creadPath, "driveCredentials", profile, "drive-credentials.json");
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "User",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true) ).Result;
            }
        }
        private DriveService GetDriveService()
        {
            return new DriveService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
        }

        private string UploadFileToDrive(string path, string contentType)
        {
            var filedata = new Google.Apis.Drive.v3.Data.File();
            filedata.Name = Path.GetFileName(path);
            filedata.Parents = new List<string> { folderId };

            FilesResource.CreateMediaUpload request;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                request = service.Files.Create(filedata, stream, contentType);
                request.Upload();
            }
            var file = request.ResponseBody;

            return file.Id;
        }
        private void DownloadFileFromDrive(string fileid, string path)
        {
            var request = service.Files.Get(fileid);

            using (var memoryStream = new MemoryStream())
            {
                request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                {
                };
                request.Download(memoryStream);

                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
                }
            }

        }

        public void Download(string workpath)
        {
            Console.WriteLine("Download started!");
            Update();
            List<Google.Apis.Drive.v3.Data.File> list = new List<Google.Apis.Drive.v3.Data.File>();
            for ( int i = 0; i < files.Count; i ++)
            {
                try
                {
                    if (Path.GetExtension(files[i].Name).Equals(".password")/*file.Parents[0] == folderId*/)
                    {
                        //var req = service.Files.Get(files[i].Id);
                        //req.Fields = "parents";
                        //var file = req.Execute();
                        //if (file.Parents[0].Equals(folderId)) 
                        list.Add(files[i]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[Error] " + ex.Message);
                }
               
            }
            for (int i = 0; i < list.Count; i++)
            {
                DownloadFileFromDrive(list[i].Id, workpath + "\\" + list[i].Name);
                Console.WriteLine("[Info] Downloading is " + (i + 1) + "/" + list.Count);
                form.toolStripStatusLabel1.Text = "Downloading is " + (i + 1) + "/" + list.Count;
            }
            Console.WriteLine("[Info] Download complete!");
            form.toolStripStatusLabel1.Text = "Download complete!";
            }
        public void Upload(string workpath)
        {
            Console.WriteLine("Upload started!");
            Update();
            string[] filess = Directory.GetFiles(workpath);
            
            for (int i = 0; i < filess.Length; i++)
            {
                //Google.Apis.Drive.v3.Data.File file = files[i];
                //if (file.Parents == new List<string> { folderId })
                //{
                //    DownloadFileFromDrive(file.Id, workpath + "\\" + file.Name);
                //}
                foreach (var file in files)
                {
                    try
                    {
                        if (Path.GetFileName(filess[i]).Equals(file.Name))
                        {
                            service.Files.Delete(file.Id).Execute(); Console.WriteLine("Delete file with name: " + file.Name);
                        }
                    }
                    catch (Exception) { }
                }
            }
            for (int i = 0; i < filess.Length; i ++)
            {
                UploadFileToDrive(filess[i], "");
                Console.Write("[Info] Uploading {0}/{1}...", i+1, filess.Length);
                Console.WriteLine("");
                form.toolStripStatusLabel1.Text = string.Format("Uploading {0}/{1}...", i + 1, filess.Length);
            }
            Console.WriteLine("[Info] Uploading complete!");
            form.toolStripStatusLabel1.Text = "Uploading complete!";
        }

    }
}
