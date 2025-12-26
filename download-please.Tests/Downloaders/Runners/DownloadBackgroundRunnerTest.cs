using download_please.Downloaders;
using Downloaders.Runners;

namespace download_please.Tests.Downloaders.Runners
{
    public class DownloadBackgroundRunnerTest
    {
        public Mock<IDownloader> MockDownloader { get; }
        public DownloadRequest FakeDownloadRequest { get; } = new DownloadRequest();
        public string FakeFileUri { get; } = "mockFileUri";
        public Guid FakeGuid { get; } = Guid.NewGuid();
        public DownloadBackgroundRunner TestRunner { get; }

        public DownloadBackgroundRunnerTest()
        {
            MockDownloader = new Mock<IDownloader>();
            TestRunner = new DownloadBackgroundRunner(MockDownloader.Object, FakeDownloadRequest, FakeFileUri, FakeGuid);
        }

        [Fact]
        public async Task DownloadBackgroundRunner_WhenCalled_CallsDownloaderWithGivenArguments()
        {
            await TestRunner.StartAsync(CancellationToken.None);

            MockDownloader.Verify(x => x.Download(FakeDownloadRequest, FakeFileUri, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DownloadBackgroundRunner_GetsCurrentStatus_BasedOnDownloaderState()
        {
            var expectedCurrentStatus = new DownloadReply()
            {
                Progress = 0,
                Status = DownloadStatus.NotStarted,
                Uuid = FakeGuid.ToString(),
            };

            MockDownloader.Setup(x => x.Progress).Returns(expectedCurrentStatus.Progress);
            TestRunner.CurrentStatus.Should().Be(expectedCurrentStatus);
        }
    }
}
