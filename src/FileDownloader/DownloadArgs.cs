namespace FileDownloader
{
    using System;
    using System.Collections.Generic;

    public class DownloadArgs
    {

        public bool MaintainSameDirectoryStructure { get; set; }

        public string RootFolderForDownload { get; set; }

        public List<(Uri Source, string Destination)> Files { get; set; }

        public Func<string, string> PostFetchContentOverride { get; set; }

        public Action<(string sourceFileName, string destinationFileName)> ProgressTracker { get; set; }

    }
}
