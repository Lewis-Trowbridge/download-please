using HttpProgress;
using NaiveProgress;

namespace download_please.Downloaders
{
    public class HttpFileDownloader : IDownloader
    {
        private HttpClient _httpClient;
        private DownloadReply _currentStatus = new()
        {
            Status = "Not started",
            Progress = 0,
        };

        public DownloadReply CurrentStatus => _currentStatus;

        private IProgress<ICopyProgress> progress;
        public HttpFileDownloader(HttpClient httpClient)
        {
            _httpClient = httpClient;
            progress = new NaiveProgress<ICopyProgress>(x => _currentStatus.Progress = x.PercentComplete);
        }


        public Task<DownloadReply> Download(DownloadRequest request, Stream localFileStream)
        {
            return Download(request, localFileStream, CancellationToken.None);
        }

        public async Task<DownloadReply> Download(DownloadRequest request, Stream localFileStream, CancellationToken token) {
            _currentStatus.Status = "Downloading";
            await _httpClient.GetAsync(request.Url, localFileStream, progress, token);
            _currentStatus.Status = "Downloaded";
            return _currentStatus;
        }
    }
}
