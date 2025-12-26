using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace download_please.Utils
{
    public interface IYoutubeDownloadUtils
    {
        ValueTask<Stream> GetAudioStream(IStreamInfo streamInfo, CancellationToken token);
        Task<IStreamInfo> GetAudioStreamInfo(string uri, CancellationToken token);
        ValueTask<Video> GetVideoInfo(string uri, CancellationToken token);
    }
}