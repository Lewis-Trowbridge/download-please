
using download_please.Utils;
using VideoLibrary;

namespace download_please.Downloaders
{
    public class YoutubeDownloader : IDownloader

    {
        public DownloadReply CurrentStatus => throw new NotImplementedException();

        private readonly YouTube youtube;
        private readonly IFileUtils fileUtils;

        public YoutubeDownloader(YouTube youtube, IFileUtils fileUtils)
        {
            this.youtube = youtube;
            this.fileUtils = fileUtils;

        }

        public async Task<DownloadReply> Download(DownloadRequest request, string fileUri, CancellationToken token)
        {
            var video = await youtube.GetVideoAsync(request.Url);
            var fileStream = fileUtils.CreateFile(fileUri);
            var videoStream = await video.StreamAsync();
            await videoStream.CopyToAsync(fileStream, token);
            return new DownloadReply();
        }

        public Task<DownloadReply> Download(DownloadRequest request, string fileUri)
        {
            return Download(request, fileUri, CancellationToken.None);
        }
    }
}
