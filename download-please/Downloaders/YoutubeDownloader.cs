
using download_please.Utils;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace download_please.Downloaders
{
    public class YoutubeDownloader : IDownloader

    {
        public DownloadReply CurrentStatus => new();

        private readonly YoutubeClient youtube;
        private readonly IFileUtils fileUtils;

        public YoutubeDownloader(YoutubeClient youtube, IFileUtils fileUtils)
        {
            this.youtube = youtube;
            this.fileUtils = fileUtils;

        }

        public async Task<DownloadReply> Download(DownloadRequest request, string fileUri, CancellationToken token)
        {
            var videoInfo = await youtube.Videos.GetAsync(request.Url, token);
            var manifestInfo = await youtube.Videos.Streams.GetManifestAsync(request.Url, token);
            var videoManifest = manifestInfo.GetAudioOnlyStreams().GetWithHighestBitrate();
            var videoStream = await youtube.Videos.Streams.GetAsync(videoManifest, token);
            var fileStream = fileUtils.CreateFile(videoInfo.Title + "." + videoManifest.Container.Name);
            await videoStream.CopyToAsync(fileStream, token);
            return new DownloadReply();
        }

        public Task<DownloadReply> Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }

    }
}
