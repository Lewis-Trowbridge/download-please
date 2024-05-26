namespace download_please.Downloaders.Selectors
{
    public interface IDownloaderSelector
    {
        IDownloader Select(DownloadRequest request);
    }
}