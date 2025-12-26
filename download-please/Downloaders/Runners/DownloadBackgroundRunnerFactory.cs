using download_please;
using download_please.Downloaders;

namespace Downloaders.Runners
{
    public class DownloadBackgroundRunnerFactory : IDownloadBackgroundRunnerFactory
    {
        public Dictionary<Guid, DownloadBackgroundRunner> Runners { get; } = [];
        public DownloadBackgroundRunner CreateRunner(IDownloader downloader, DownloadRequest request, string fileUrl)
        {
            var guid = Guid.NewGuid();
            var newRunner = new DownloadBackgroundRunner(downloader, request, fileUrl, guid);
            Runners.Add(guid, newRunner);
            return newRunner;
        }
    }
}
