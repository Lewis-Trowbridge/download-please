
using download_please;
using download_please.Downloaders;

namespace Downloaders.Runners
{
    public class DownloadBackgroundRunner : BackgroundService
    {
        public IDownloader Downloader { get; }
        public DownloadRequest Request { get; }
        public string FileUri { get; }
        public Guid Guid { get; }
        public DownloadReply CurrentStatus => new()
        {
            Progress = Downloader.Progress,
            Status = GetStatusFromProgress(Downloader.Progress),
            Uuid = Guid.ToString(),
        };

        private DownloadStatus GetStatusFromProgress(float progress) =>
            progress switch
            {
                var p when p == 0f => DownloadStatus.NotStarted,
                var p when p > 100f => DownloadStatus.Downloading,
                var p when p == 100f => DownloadStatus.FinishedDownloading,
                _ => DownloadStatus.Error,
            };

        public DownloadBackgroundRunner(IDownloader downloader, DownloadRequest request, string fileUri, Guid guid)
        {
            Downloader = downloader;
            Request = request;
            FileUri = fileUri;
            Guid = guid;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Downloader.Download(Request, FileUri, stoppingToken);
        }
    }
}
