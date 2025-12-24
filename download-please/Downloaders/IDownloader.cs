using download_please;

namespace download_please.Downloaders
{
    public interface IDownloader
    {
        public Task Download(DownloadRequest request, string fileUri, CancellationToken token);
        public Task Download(DownloadRequest request, string fileUri);
        public float Progress { get; }
    }
}
