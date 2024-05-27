using download_please.Downloaders.Selectors;
using Grpc.Core;
using System.IO.Abstractions;

namespace download_please.Services
{
    public class DownloadService : Download.DownloadBase
    {
        private readonly IDownloaderSelector _downloaderSelector;
        private readonly IFileSystem _fileSystem;
        public DownloadService(IDownloaderSelector downloaderSelector, IFileSystem fileSystem)
        {
            _downloaderSelector = downloaderSelector;
            _fileSystem = fileSystem;
        }

        public async override Task<DownloadReply> Download(DownloadRequest request, ServerCallContext context)
        {
            var downloader = _downloaderSelector.Select(request);

            var fileUrl = new Uri(request.Url, UriKind.Absolute);
            var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            using var localFileStream = _fileSystem.File.Create($"{homeDirectory}{Path.DirectorySeparatorChar}{fileUrl.Segments.Last()}");

            return await downloader.Download(request, localFileStream);
        }
    }
}
