using download_please.Downloaders;
using download_please.Utils;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Channels;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using System.Text;

namespace download_please.Tests.Downloaders
{
    public class YoutubeAudioDownloaderTest
    {

        private Mock<IFileUtils> MockFileUtils { get; }
        private Mock<IYoutubeDownloadUtils> MockYoutubeDownloadUtils { get; }
        private MemoryStream FakeFileStream { get; }
        private MemoryStream FakeVideoStream { get; }
        private byte[] FakeStreamContent { get; }
        private string FakeTitle { get; }
        private Video FakeVideoInfo { get; }
        private Mock<IStreamInfo> MockStreamInfo { get; }

        public YoutubeAudioDownloaderTest() {
            FakeFileStream = new MemoryStream();
            FakeVideoStream = new MemoryStream();
            FakeStreamContent = Encoding.UTF8.GetBytes("Hello");
            FakeVideoStream.Write(FakeStreamContent, 0, FakeStreamContent.Length);
            FakeVideoStream.Position = 0;

            MockFileUtils = new Mock<IFileUtils>();
            MockFileUtils.Setup(x => x.CreateFile(It.IsAny<string>())).Returns(FakeFileStream);
            FakeTitle = "Fake title";
            FakeVideoInfo = new Video(VideoId.Parse("https://www.youtube.com/watch?v=oA0CpI0vCK4"), FakeTitle, new Author(ChannelId.Parse("https://www.youtube.com/channel/UC3n5uGu18FoCy23ggWWp8tA"), ""), DateTimeOffset.Now, "", null, [], [], new Engagement(0, 0, 0));
            MockYoutubeDownloadUtils = new Mock<IYoutubeDownloadUtils>();
            MockYoutubeDownloadUtils.Setup(x => x.GetVideoInfo(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(FakeVideoInfo);

            MockStreamInfo = new Mock<IStreamInfo>();
            var fakeContainer = new Container("mp4");
            MockStreamInfo.SetupGet(x => x.Container).Returns(fakeContainer);

            MockYoutubeDownloadUtils.Setup(x => x.GetAudioStreamInfo(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockStreamInfo.Object);
            MockYoutubeDownloadUtils.Setup(x => x.GetAudioStream(MockStreamInfo.Object, It.IsAny<CancellationToken>())).ReturnsAsync(FakeVideoStream);
        }

        [Fact]
        public async Task YoutubeAudioDownloader_WhenGivenRequest_SavesToFileWithVideoName()
        {
            var fakeUrl = "fakeUrl";
            var mockClient = new Mock<YoutubeClient>();
            
            var testDownloader = new YoutubeAudioDownloader(mockClient.Object, MockFileUtils.Object, MockYoutubeDownloadUtils.Object);
            await testDownloader.Download(new DownloadRequest() { Url = fakeUrl }, "");

            MockFileUtils.Verify(x => x.CreateFile(FakeTitle + ".mp4"));

            var actualContent = FakeFileStream.ToArray();
            actualContent.Should().Equal(FakeStreamContent);

        }
    }
}
