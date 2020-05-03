using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FileDownloader;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(Example1, Console.Out);
            Run(Example2, Console.Out);
            Console.WriteLine("Hello World!");
        }

        static void Run(Action action, TextWriter tw)
        {
            NullCheck(action);
            NullCheck(tw);

            var sp = new Stopwatch();
            sp.Start();
            action();
            var ts = sp.Elapsed;
            var ellapseTime = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            tw.WriteLine($"Completed action in : {ellapseTime}");

            void NullCheck<T>(T _value)
            {
                if (_value == null) throw new ArgumentNullException();
            }
        }

        static void Example2()
        {
            Console.WriteLine("Change all h1 to h2 before saving the contents!");
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
        }

        static void Example1()
        {
            var files = new List<(Uri, string)> {
                (new Uri("https://www.html-5-tutorial.com/table-tag.htm"),null),
                 (new Uri("https://www.html-5-tutorial.com/html-tag.htm") , null),
                 (new Uri("https://dotnet.microsoft.com/learn/csharp") , null)
            };

            var downloadArgs = new DownloadArgs
            {
                MaintainSameDirectoryStructure = true,
                RootFolderForDownload = "Test",
                Files = files,
                ProgressTracker = t => Console.WriteLine($"Download complete for {t.sourceFileName}")
            };

            RemoteFiles.Instance.Download(downloadArgs);
        }
    }
}
