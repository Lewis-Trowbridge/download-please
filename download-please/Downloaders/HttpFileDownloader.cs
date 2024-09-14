using download_please.Utils;
using HttpProgress;
using NaiveProgress;
using System.Diagnostics;

namespace download_please.Downloaders
{
    public class HttpFileDownloader : IDownloader
    {
        private readonly HttpClient _httpClient;
        private readonly IFileUtils _fileUtils;
        private readonly ILogger<HttpFileDownloader> _logger;

        public DownloadReply CurrentStatus { get; private set; } = new()
        {
            Status = "Not started",
            Progress = 0,
        };

        private IProgress<ICopyProgress> progress;
        public HttpFileDownloader(
            HttpClient httpClient,
            IFileUtils fileUtils,
            ILogger<HttpFileDownloader> logger
            )
        {
            _httpClient = httpClient;
            _fileUtils = fileUtils;
            _logger = logger;

            progress = new NaiveProgress<ICopyProgress>(x => {
                CurrentStatus.Progress = x.PercentComplete;
            });
        }


        public Task<DownloadReply> Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }

        public async Task<DownloadReply> Download(DownloadRequest request, string fileUri, CancellationToken token) {
            _logger.LogInformation("Beginning download of {} to {}", request, fileUri);
            CurrentStatus.Status = "Downloading";
            try
            {

                var localFileStream = _fileUtils.CreateFile(fileUri);
                await _httpClient.GetAsync(request.Url, localFileStream, progress, token);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception thrown while downloading: {}", ex);
            }
            CurrentStatus.Status = "Downloaded";
            _logger.LogInformation("Download {} complete!", request);
            return CurrentStatus;
        }
    }
}
