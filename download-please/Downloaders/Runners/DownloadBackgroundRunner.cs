
using download_please;
using download_please.Downloaders;

namespace Downloaders.Runners
{
    public class DownloadBackgroundRunner : BackgroundService
    {
        public IDownloader Downloader { get; }
        public DownloadRequest Request { get; }
        public string FileUri { get; }


        public DownloadBackgroundRunner(IDownloader downloader, DownloadRequest request, string fileUri)
        {
            Downloader = downloader;
            Request = request;
            FileUri = fileUri;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Downloader.Download(Request, FileUri, stoppingToken);
        }
    }
}
