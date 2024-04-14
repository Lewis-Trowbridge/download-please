using download_please;
using Grpc.Core;

namespace download_please.Services
{
    public class DownloadService : Download.DownloadBase
    {
        private readonly ILogger<DownloadService> _logger;
        public DownloadService(ILogger<DownloadService> logger)
        {
            _logger = logger;
        }

        public async override Task<DownloadReply> Download(DownloadRequest request, ServerCallContext context)
        {
            var fileUrl = new Uri(request.Url, UriKind.Absolute);
            var localFileStream = File.Create(fileUrl.Segments.Last());
            using (var client = new HttpClient())
            {
                var internetStream = await client.GetStreamAsync(fileUrl);
                internetStream.CopyToAsync(localFileStream);
            }
            return new DownloadReply
            {
                Status = "Downloading"
            };
        }
    }
}
