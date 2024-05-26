using download_please.Downloaders.Selectors;
using Grpc.Core;

namespace download_please.Services
{
    public class DownloadService : Download.DownloadBase
    {
        private readonly HttpClient _httpClient;
        private readonly IDownloaderSelector _downloaderSelector;
        public DownloadService(HttpClient httpClient, IDownloaderSelector downloaderSelector)
        {
            _httpClient = httpClient;
            _downloaderSelector = downloaderSelector;
        }

        public async override Task<DownloadReply> Download(DownloadRequest request, ServerCallContext context)
        {
            var fileUrl = new Uri(request.Url, UriKind.Absolute);
            var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var downloader = _downloaderSelector.Select(request);
            using var localFileStream = File.Create($"{homeDirectory}{Path.DirectorySeparatorChar}{fileUrl.Segments.Last()}");

            return await downloader.Download(request, localFileStream);
        }
    }
}
