using download_please;

namespace download_please.Downloaders
{
    public interface IDownloader
    {
        public Task<DownloadReply> Download(DownloadRequest request, string fileUri, CancellationToken token);
        public Task<DownloadReply> Download(DownloadRequest request, string fileUri);
        public DownloadReply CurrentStatus { get; }
    }
}
