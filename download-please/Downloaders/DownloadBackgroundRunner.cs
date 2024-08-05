
namespace download_please.Downloaders
{
    public class DownloadBackgroundRunner : BackgroundService
    {
        public IDownloader Downloader { get; }
        public DownloadRequest Request { get; }
        public Stream Stream { get; }
        

        public DownloadBackgroundRunner(IDownloader downloader, DownloadRequest request, Stream fileStream) {
            Downloader = downloader;
            Request = request;
            Stream = fileStream;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Downloader.Download(Request, Stream, stoppingToken);
        }
    }
}
