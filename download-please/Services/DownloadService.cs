using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using Grpc.Core;

namespace download_please.Services
{
    public class DownloadService : Download.DownloadBase
    {
        private readonly IDownloaderSelector _downloaderSelector;
        private readonly DownloadBackgroundRunnerFactory _downloadBackgroundRunnerFactory;

        public DownloadService(
            IDownloaderSelector downloaderSelector,
            DownloadBackgroundRunnerFactory downloadBackgroundRunnerFactory
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
            var a = _downloadBackgroundRunnerFactory.Runners.GetValueOrDefault(Guid.Parse(request.Uuid));
            return a.Downloader.CurrentStatus;
        }
    }
}
