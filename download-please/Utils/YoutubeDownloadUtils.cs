using AngleSharp.Io;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace download_please.Utils
{
    public class YoutubeDownloadUtils : IYoutubeDownloadUtils
    {
        private YoutubeClient YoutubeClient { get; }
        public YoutubeDownloadUtils(YoutubeClient youtube)
        {
            YoutubeClient = youtube;
        }

        public async Task<IStreamInfo> GetAudioStreamInfo(string uri, CancellationToken token)
        {
            var manifestInfo = await YoutubeClient.Videos.Streams.GetManifestAsync(uri, token);
            return manifestInfo.GetAudioOnlyStreams().GetWithHighestBitrate();
        }

        public ValueTask<Stream> GetAudioStream(IStreamInfo streamInfo, CancellationToken token)
        {
            return YoutubeClient.Videos.Streams.GetAsync(streamInfo, token);
        }

        public ValueTask<Video> GetVideoInfo(string uri, CancellationToken token)
        {
            return YoutubeClient.Videos.GetAsync(uri, token);
        }
    }
}
