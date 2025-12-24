using download_please.Utils;
using HttpProgress;
using NaiveProgress;
using System.Diagnostics;

namespace download_please.Downloaders
{
    public class HttpFileDownloader : IDownloader
    {
        private readonly HttpClient _httpClient;
        private readonly IFileUtils _fileUtils;

        public float Progress { get; private set; } = 0f;

        private IProgress<ICopyProgress> progress;
        public HttpFileDownloader(HttpClient httpClient, IFileUtils fileUtils)
        {
            _httpClient = httpClient;
            _fileUtils = fileUtils;

            progress = new NaiveProgress<ICopyProgress>(x => {
                Progress = Convert.ToSingle(x.PercentComplete);
            });
        }


        public Task Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }

        public async Task Download(DownloadRequest request, string fileUri, CancellationToken token) {
            var localFileStream = _fileUtils.CreateFile(fileUri);
            await _httpClient.GetAsync(request.Url, localFileStream, progress, token);
        }
    }
}
