using download_please.Utils;
using HttpProgress;
using NaiveProgress;
using System.Diagnostics;

namespace download_please.Downloaders
{
    public class HttpFileDownloader : IDownloader
    {
        private HttpClient _httpClient;
        private readonly FileUtils _fileUtils;

        public DownloadReply CurrentStatus { get; private set; } = new()
        {
            Status = "Not started",
            Progress = 0,
        };

        private IProgress<ICopyProgress> progress;
        public HttpFileDownloader(HttpClient httpClient,
            FileUtils fileUtils
            )
        {
            _httpClient = httpClient;
            _fileUtils = fileUtils;
            progress = new NaiveProgress<ICopyProgress>(x => {
                Console.WriteLine(x.PercentComplete);
                CurrentStatus.Progress = x.PercentComplete;
            });
        }


        public Task<DownloadReply> Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }

        public async Task<DownloadReply> Download(DownloadRequest request, string fileUri, CancellationToken token) {
            CurrentStatus.Status = "Downloading";
            var localFileStream = _fileUtils.CreateFile(fileUri);
            await _httpClient.GetAsync(request.Url, localFileStream, progress, token);
            CurrentStatus.Status = "Downloaded";
            return CurrentStatus;
        }
    }
}
