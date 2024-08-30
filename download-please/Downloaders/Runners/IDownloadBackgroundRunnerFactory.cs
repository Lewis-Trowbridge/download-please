
using download_please;
using download_please.Downloaders;

namespace Downloaders.Runners
{
    public interface IDownloadBackgroundRunnerFactory
    {
        Dictionary<Guid, DownloadBackgroundRunner> Runners { get; }

        DownloadBackgroundRunner CreateRunner(IDownloader downloader, DownloadRequest request, string fileUrl);
    }
}