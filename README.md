# E-Mail Sender

<p align="center">
  <img src="https://user-images.githubusercontent.com/44268275/71026336-4e483a00-2109-11ea-9e21-b870af5ec019.png">
</p>

## Features
* Upload a file to your Google Drive account.
* The app sets the uploaded file to public and provides a link so users can download it.
* Send an E-Mail though the app.

## Instaling Guide
1. Have a Google account.

2. Login into https://console.developers.google.com/ (It is a platform where you handle all your tokens etc.)

3. Create a new project - top left, click on Select a project, new window should appear, click on New Project, name it whatever you want, when that's done, click on Navigation menu (top left), APIs & Services -> Library, Search for Google Drive and Gmail API, enable them both. Then go to Credentials (In APIs & Services), click Create credentials, choose OAuth client ID, you might need to configure consent screen (but I think it's pretty easy, you have to choose either Internal or External and provide the Application Name), in Create OAuth client ID, choose Other, name it whatever you want, Click Ok on the new window, then Download JSON on Credentials Page (on the right), place the file in the same dictionary where the .exe app is. Then create another credential for Gmail token (the same way as above). Place it again in the same dictionary where the .exe app is.

4. To run the app, you will need .NET Core 3.1 https://dotnet.microsoft.com/download (or just newer version of Visual Studio)

5. Build the app, Click the Choose button to select a file, then Click Upload button.

6. If the tokens are in the same dictionary, a new website should launch and ask you to log in to an appropriate account (I would choose the same Gmail account where you created these tokens) then if you give access to this app, agree to that. When that's done, a token folder should appear in the same dictionary as the .exe file. The reason why the token folder is created is so you won't have to repeat this step again.

7. A downloadable link should appear, copy it and paste in the Text field in the E-Mail tab.

8. Type the E-mail address.

9. The subject field can be empty.

10. Click Send-Email (now just redo the 6th step)
