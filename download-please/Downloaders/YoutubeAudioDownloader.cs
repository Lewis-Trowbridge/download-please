
using download_please.Utils;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace download_please.Downloaders
{
    public class YoutubeAudioDownloader : IDownloader

    {
        public double Progress => fileStreamMonitor != null ? (Convert.ToDouble(fileStreamMonitor.Position) / Convert.ToDouble(fileStreamMonitor.Length)) * 100 : 0d;

        private readonly YoutubeClient youtube;
        private readonly IFileUtils fileUtils;
        private Stream? fileStreamMonitor;
        private IYoutubeDownloadUtils downloadUtils;

        public YoutubeAudioDownloader(YoutubeClient youtube, IFileUtils fileUtils, IYoutubeDownloadUtils downloadUtils)
        {
            this.youtube = youtube;
            this.fileUtils = fileUtils;
            this.downloadUtils = downloadUtils;

        }

        public async Task Download(DownloadRequest request, string fileUri, CancellationToken token)
        {
            

            var videoInfo = downloadUtils.GetVideoInfo(request.Url, token);
            var streamInfo = downloadUtils.GetAudioStreamInfo(request.Url, token);
            
            var fileStream = fileUtils.CreateFile((await videoInfo).Title + "." + (await streamInfo).Container.Name);
            var videoStream = await downloadUtils.GetAudioStream(await streamInfo, token);
            fileStreamMonitor = videoStream;
            await videoStream.CopyToAsync(fileStream, token);
        }

        public Task Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }

    }
}
