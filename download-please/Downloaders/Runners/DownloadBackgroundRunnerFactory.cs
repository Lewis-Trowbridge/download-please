using download_please;
using download_please.Downloaders;
using System.Runtime.CompilerServices;

namespace Downloaders.Runners
{
    public class DownloadBackgroundRunnerFactory(ILogger<DownloadBackgroundRunner> logger) : IDownloadBackgroundRunnerFactory
    {
        public Dictionary<Guid, DownloadBackgroundRunner> Runners { get; } = [];
        public DownloadBackgroundRunner CreateRunner(IDownloader downloader, DownloadRequest request, string fileUrl)
        {
            logger.LogInformation("Creating background process runner with downloader {}", downloader);
            var newRunner = new DownloadBackgroundRunner(downloader, request, fileUrl);
            var guid = Guid.NewGuid();
            Runners.Add(guid, newRunner);
            newRunner.Downloader.CurrentStatus.Uuid = guid.ToString();
            logger.LogInformation("Runner created: {}\nRunner UUID is {}", newRunner, guid);
            return newRunner;
        }
    }
}
