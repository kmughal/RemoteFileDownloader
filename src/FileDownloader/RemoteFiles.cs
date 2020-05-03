namespace FileDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    public class RemoteFiles
    {
        private static RemoteFiles _instance;
        private static WebClient _webClient;

        public static RemoteFiles Instance => _instance ??= new RemoteFiles();

        public void Download(DownloadArgs downloadArgs)
        {
            ValidateDownloadArgs(downloadArgs);

            var files = GetSourceAndDestination(downloadArgs);
            var postFetchContentOverride = downloadArgs.PostFetchContentOverride;

            foreach (var file in files)
            {
                var (source, destination) = (file.source, file.destination);

                if (downloadArgs.PostFetchContentOverride is null) DownloadFile(source, destination);
                else
                {
                    var contents = ReadFileAsText(source.ToString());
                    ApplyTextOverrideAndSaveFile(destination, contents, postFetchContentOverride);
                }

                if (downloadArgs.ProgressTracker != null) downloadArgs.ProgressTracker((source.ToString(), destination));
            }

            List<(Uri source, string destination)> GetSourceAndDestination(DownloadArgs _args)
            {
                List<(Uri _source, string _destination)> result = _args.MaintainSameDirectoryStructure switch
                {
                    true => _args.Files.Select(x => (x.Source, GetDestinationFilename(x.Source, _args.RootFolderForDownload))).ToList(),
                    false => _args.Files
                };

                return result;

                string GetDestinationFilename(Uri _source, string _rootFoldername)
                {
                    var fullPathExcludingHostname = _source.PathAndQuery;
                    var result = $@"{_rootFoldername}\{fullPathExcludingHostname}";
                    return result;
                }
            }

            void DownloadFile(Uri _source, string _destination)
            {
                var dir = Path.GetDirectoryName(_destination);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                _webClientInstance.DownloadFile(_source, _destination);
            }

            void ApplyTextOverrideAndSaveFile(string _destination, string _contents, Func<string, string> func)
            {
                var overrideContents = func(_contents);
                var dir = Path.GetDirectoryName(_destination);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(_destination, overrideContents);
            }
        }

        private void ValidateDownloadArgs(DownloadArgs downloadArgs)
        {
            Contracts.NullCheck(downloadArgs.Files);

            if (downloadArgs.MaintainSameDirectoryStructure)
            {
                Contracts.NullCheck(downloadArgs.RootFolderForDownload);
            }
            else
            {
                foreach (var file in downloadArgs.Files)
                {
                    Contracts.NullCheck(file.Source);
                    Contracts.NullCheck(file.Destination);
                }
            }
        }

        public string ReadFileAsText(string address)
        {
            var contents = _webClientInstance.DownloadString(address);
            return contents;
        }

        private static WebClient _webClientInstance => _webClient ??= new WebClient();
    }
}
