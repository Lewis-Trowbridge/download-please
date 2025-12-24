
using download_please.Utils;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace download_please.Downloaders
{
    public class YoutubeDownloader : IDownloader

    {
        public double Progress => fileStreamMonitor != null ? (Convert.ToDouble(fileStreamMonitor.Position) / Convert.ToDouble(fileStreamMonitor.Length)) * 100 : 0d;

        private readonly YoutubeClient youtube;
        private readonly IFileUtils fileUtils;
        private Stream? fileStreamMonitor;

        public YoutubeDownloader(YoutubeClient youtube, IFileUtils fileUtils)
        {
            this.youtube = youtube;
            this.fileUtils = fileUtils;

        }

        public async Task Download(DownloadRequest request, string fileUri, CancellationToken token)
        {
            var videoInfo = await youtube.Videos.GetAsync(request.Url, token);
            var manifestInfo = await youtube.Videos.Streams.GetManifestAsync(request.Url, token);
            var streamInfo = manifestInfo.GetAudioOnlyStreams().GetWithHighestBitrate();
            var videoStream = await youtube.Videos.Streams.GetAsync(streamInfo, token);
            var fileStream = fileUtils.CreateFile(videoInfo.Title + "." + streamInfo.Container.Name);
            fileStreamMonitor = videoStream;
            await videoStream.CopyToAsync(fileStream, token);
        }

        public Task Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }

    }
}
