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
        public GoogleDrive()
        {
            Console.WriteLine("Loading credentials...");
            credential = GetUserCredential();
            Console.WriteLine("Credentials loaded. Loading DriveService...");
            service = GetDriveService();
            Console.WriteLine("DriveService loaded. Loading list of items...");
            IList<Google.Apis.Drive.v3.Data.File> items = service.Files.List().Execute().Files;
            Console.WriteLine("List of items loaded. Sorting...");
            foreach (var file in items)
            {
                if (file.MimeType == "application/vnd.google-apps.folder") folders.Add(file);
                else if (file.MimeType == "application/vnd.google-apps.file") files.Add(file);
            }
            Console.WriteLine("Sorted.");       
        }
        public void Sync(string workpath)
        {
            bool contains = false;
            foreach (var file in folders) { if (file.Name == "Passwords") { contains = true; } break; }
            if (!contains)
            {
                Console.WriteLine("Creating folder \"Passwords\"...");
                if (!string.IsNullOrEmpty(CreateFolder("Passwords"))) Console.WriteLine("Folder created.");
                else Console.WriteLine("Error. Folder not created!");
            }
        }
        private string CreateFolder(string folderName)
        {
            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = "Passwords";
            file.MimeType = "application/vnd.google-apps.folder";

            var request = service.Files.Create(file);
            request.Fields = "id";

            var result = request.Execute();
            return result.Id;
        }
        private UserCredential GetUserCredential()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                creadPath = Path.Combine(creadPath, "driveCredentials", "drive-credentials.json");
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
         
    }
}
