# Chrome Native Messaging Host Example

This code should provide a basic understanding of how to handle messages sent and received by a Google Chrome extension through a native messaging host.

It's a Visual Studio Code C# (Windows) implementation of Googles echo-example found under:

https://github.com/GoogleChrome/chrome-extensions-samples/tree/main/api-samples/nativeMessaging
https://developer.chrome.com/docs/extensions/develop/concepts/native-messaging

## Guide 
* Edit the Path in com.google.chrome.example.echo-win.json to represent your current install
* Run the install-host.bat in NativeMessagingHostsConfig
* Install the unpacked extension in Google Chrome (using dev mode)
* Click on the extension to run the example

For debugging purposes the host writes a log file of the sent and received messages (using log4net)

## Dev-Environment Setup in Visual Studio Code
* Install VSCode (from Microsoft Store)
* Install C# Dev Tools
* In VSCode Terminal run the following commands
```
dotnet add package log4net
dotnet add package Newtonsoft.Json
```
* Press CRTL+Shift+P -> Build ;) 
