using download_please;

namespace download_please.Downloaders
{
    public interface IDownloader
    {
        public Task<DownloadReply> Download(DownloadRequest request, Stream localFileStream, CancellationToken token);
        public Task<DownloadReply> Download(DownloadRequest request, Stream localFileStream);
        public DownloadReply CurrentStatus { get; }
    }
}
