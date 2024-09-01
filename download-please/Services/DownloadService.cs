using download_please.Downloaders.Selectors;
using download_please.Utils;
using Downloaders.Runners;
using Google.Protobuf.Collections;
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

        public async override Task<StatusReply> Status(StatusRequest request, ServerCallContext context)
        {
            if (request.HasUuid)
            {
                if (_downloadBackgroundRunnerFactory.Runners.TryGetValue(Guid.Parse(request.Uuid), out DownloadBackgroundRunner? downloadRunner))
                {
                    return new StatusReply().AddDownloadReplies([downloadRunner.Downloader.CurrentStatus]);
                }
                else
                {
                    return new StatusReply().AddDownloadReplies([]);
                }
            }
            else
            {
                return new StatusReply().AddDownloadReplies(_downloadBackgroundRunnerFactory.Runners.Select(runner => runner.Value.Downloader.CurrentStatus));
            }
        }
    }
}
