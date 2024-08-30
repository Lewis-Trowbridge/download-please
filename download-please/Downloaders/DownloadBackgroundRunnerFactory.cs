using System.Collections;

namespace download_please.Downloaders
{
    public class DownloadBackgroundRunnerFactory
    {
        public Dictionary<Guid, DownloadBackgroundRunner> Runners { get; } = [];
        public DownloadBackgroundRunner CreateRunner(IDownloader downloader, DownloadRequest request, string fileUrl)
        {
            var newRunner = new DownloadBackgroundRunner(downloader, request, fileUrl);
            var guid = Guid.NewGuid();
            Runners.Add(guid, newRunner);
            newRunner.Downloader.CurrentStatus.Uuid = guid.ToString();
            return newRunner;
        }
    }
}
