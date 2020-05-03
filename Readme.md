# Introduction

A simple client to download remote files.
In order to use this package:

- Add reference for this package from nuget : https://www.nuget.org/packages/SimpleRemoteFilesDownloader/
- Below code will download the files for you in your local :

```

            var files = new List<(Uri, string)> {
                (new Uri("https://www.html-5-tutorial.com/table-tag.htm"),"Downloads/table-tag.html"),
                 (new Uri("https://www.html-5-tutorial.com/html-tag.htm") , "Downloads/html-tag.html"),
                 (new Uri("https://dotnet.microsoft.com/learn/csharp") , "Downloads/learn-csharp.html")
            };

            var downloadArgs = new DownloadArgs
            {
                Files = files,
                ProgressTracker = t => Console.WriteLine($"Download complete for {t.sourceFileName}"),
                PostFetchContentOverride = contents => contents.Replace("h1", "h2")
            };

            RemoteFiles.Instance.Download(downloadArgs);

```

- Also navigate to the Example folder which is a simple console app!

Feel free to fork this.
