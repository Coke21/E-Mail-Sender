using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMailSender.Classes;
using HeyRed.Mime;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using Screen = Caliburn.Micro.Screen;

namespace EMailSender.ViewModels
{
    class MainViewModel : Screen
    {
        public string Title => "File Sender | 0.1 by Coke";

        public void Choose()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
                SelectedFileTextBox = openFileDialog.FileName;
        }

        private string _selectedFileTextBox;
        public string SelectedFileTextBox
        {
            get { return _selectedFileTextBox; }
            set
            {
                _selectedFileTextBox = value;

                NotifyOfPropertyChange(() => SelectedFileTextBox);
            }
        }

        public bool CanUpload(string selectedFileTextBox) => !string.IsNullOrWhiteSpace(selectedFileTextBox);

        public async void Upload(string selectedFileTextBox) => await UploadFileAsync(Authentication.DService, selectedFileTextBox);

        public async Task UploadFileAsync(Google.Apis.Drive.v3.DriveService service, string fileName)
        {
            IsProgressBarEnabled = true;

            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = Path.GetFileName(fileName);
            file.Parents = new List<string> { "root" };

            byte[] byteArray = File.ReadAllBytes(fileName);

            Google.Apis.Drive.v3.Data.File fileUploadedBody;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                var fileToUpload = service.Files.Create(file, stream, MimeTypesMap.GetMimeType(fileName));
                await fileToUpload.UploadAsync();
                fileUploadedBody = fileToUpload.ResponseBody;
            }
                
            //Put the newly uploaded file to public so anyone can download the file without permission
            var permission = new Google.Apis.Drive.v3.Data.Permission
            {
                Role = "reader",
                Type = "anyone"
            };
            await service.Permissions.Create(permission, fileUploadedBody.Id).ExecuteAsync();

            //Get public link to download the file
            var request = service.Files.Get(fileUploadedBody.Id);
            request.Fields = "webContentLink";

            var returnedFile = await request.ExecuteAsync();
            DownloadableLinkTextBox = returnedFile.WebContentLink;

            IsProgressBarEnabled = false;
        }

        private bool _isProgressBarEnabled;
        public bool IsProgressBarEnabled
        {
            get { return _isProgressBarEnabled; }
            set
            {
                _isProgressBarEnabled = value;

                NotifyOfPropertyChange(() => IsProgressBarEnabled);
            }
        }

        private string _downloadableLinkTextBox;
        public string DownloadableLinkTextBox
        {
            get { return _downloadableLinkTextBox; }
            set
            {
                _downloadableLinkTextBox = value; 

                NotifyOfPropertyChange(() => DownloadableLinkTextBox);
            }
        }

        private string _emailAddressTextBox;
        public string EmailAddressTextBox
        {
            get { return _emailAddressTextBox; }
            set
            {
                _emailAddressTextBox = value;

                NotifyOfPropertyChange(() => EmailAddressTextBox);
            }
        }

        private string _subjectTextBox;
        public string SubjectTextBox
        {
            get { return _subjectTextBox; }
            set
            {
                _subjectTextBox = value; 

                NotifyOfPropertyChange(() => SubjectTextBox);
            }
        }

        private string _textTextBox;
        public string TextTextBox
        {
            get { return _textTextBox; }
            set
            {
                _textTextBox = value;

                NotifyOfPropertyChange(() => TextTextBox);
            }
        }

        public bool CanSendEMail(string emailAddressTextBox) => emailAddressTextBox.Contains('@');
        public async Task SendEMailAsync(string emailAddressTextBox)
        {
            try
            {
                string plainText = $"To:{emailAddressTextBox}\r\n" +
                                   $"Subject: {SubjectTextBox}\r\n" +
                                   "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                                   $"<p>{TextTextBox}<p>";

                //"<p><a href=\"https://www.youtube.com/\">test</a><p>"

                var newMsg = new Google.Apis.Gmail.v1.Data.Message();
                newMsg.Raw = Base64UrlEncode(plainText);
                await Authentication.GService.Users.Messages.Send(newMsg, "me").ExecuteAsync();

                MessageBox.Show("An E-Mail has been sent.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error:\n{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string Base64UrlEncode(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}