
using download_please;
using download_please.Downloaders;
using download_please.Utils;

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
            Status = DownloadStatusUtils.GetStatusFromProgress(Downloader.Progress),
            Uuid = Guid.ToString(),
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
