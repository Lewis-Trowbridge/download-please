using download_please;

namespace download_please.Downloaders
{
    public interface IDownloader
    {
        public Task<DownloadReply> Download(DownloadRequest request, FileStream localFileStream);
    }
}
