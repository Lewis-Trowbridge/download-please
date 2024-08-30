using download_please.Downloaders.Selectors;
using Downloaders.Runners;
using Grpc.Core;

namespace download_please.Services
{
    public class DownloadService : Download.DownloadBase
    {
        private readonly IDownloaderSelector _downloaderSelector;
        private readonly IDownloadBackgroundRunnerFactory _downloadBackgroundRunnerFactory;

        public DownloadService(
            IDownloaderSelector downloaderSelector,
            IDownloadBackgroundRunnerFactory downloadBackgroundRunnerFactory
            )
        {
            _downloaderSelector = downloaderSelector;
            _downloadBackgroundRunnerFactory = downloadBackgroundRunnerFactory;
        }

        public async override Task<DownloadReply> Download(DownloadRequest request, ServerCallContext context)
        {
            var downloader = _downloaderSelector.Select(request);

            var fileUrl = new Uri(request.Url, UriKind.Absolute);

            var background = _downloadBackgroundRunnerFactory.CreateRunner(downloader, request, fileUrl.Segments.Last());

            await background.StartAsync(context.CancellationToken);

            return background.Downloader.CurrentStatus;
        }

        public async override Task<DownloadReply> Status(StatusRequest request, ServerCallContext context)
        {
            if (_downloadBackgroundRunnerFactory.Runners.TryGetValue(Guid.Parse(request.Uuid), out DownloadBackgroundRunner? downloadRunner))
            {
                return downloadRunner.Downloader.CurrentStatus;
            }
            else
            {
                return new DownloadReply()
                {
                    Status = "Not found"
                };
            }
        }
    }
}
