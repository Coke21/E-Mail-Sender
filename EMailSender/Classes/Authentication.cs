using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace EMailSender.Classes
{
    class Authentication
    {
        public static DriveService DService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer =
                GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(new FileStream("credentials.json", FileMode.Open, FileAccess.Read)).Secrets,
                    new [] {DriveService.Scope.Drive},
                    "user",
                    CancellationToken.None,
                    new FileDataStore("token.json", true)).Result,
            ApplicationName = "xd"
        });

        public static GmailService GService = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer =
                GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(new FileStream("credentials1.json", FileMode.Open, FileAccess.Read)).Secrets,
                    new[] { GmailService.Scope.MailGoogleCom },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("token1.json", true)).Result,
            ApplicationName = "xd"
        });
    }
}
