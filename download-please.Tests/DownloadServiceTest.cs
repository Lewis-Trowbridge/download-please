using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Services;
using Downloaders.Runners;
using Grpc.Core.Testing;
using Microsoft.Extensions.Hosting;

namespace download_please.Tests
{
    public class DownloadServiceTest
    {
        private Mock<IDownloaderSelector> MockDownloaderSelector;
        private Mock<IDownloadBackgroundRunnerFactory> MockDownloaderBackgroundRunnerFactory;
        private DownloadService TestService { get; }

        public DownloadServiceTest() 
        {
            MockDownloaderSelector = new Mock<IDownloaderSelector>();
            MockDownloaderBackgroundRunnerFactory = new Mock<IDownloadBackgroundRunnerFactory>();
            TestService = new DownloadService(MockDownloaderSelector.Object, MockDownloaderBackgroundRunnerFactory.Object);
        }

        [Fact]
        public async Task DownloadService_WhenGivenRequest_CallsDownloaderFactoryWithRequest()
        {
            var fakeFileName = "fakefile";
            var mockDownloader = new Mock<IDownloader>();
            MockDownloaderSelector.Setup(x => x.Select(It.IsAny<DownloadRequest>())).Returns(mockDownloader.Object);
            var expectedCancellationToken = new CancellationTokenSource().Token;
            var mockContext = TestServerCallContext.Create(null, null, DateTime.MaxValue, null, expectedCancellationToken, null, null, null, null, null, null);
            var fakeRequest = new DownloadRequest()
            {
                Url = $"http://fake-url.com/{fakeFileName}"
            };
            var mockBackgroundService = new Mock<DownloadBackgroundRunner>(mockDownloader.Object, fakeRequest, fakeFileName);
            MockDownloaderBackgroundRunnerFactory.Setup(x => x.CreateRunner(mockDownloader.Object, It.IsAny<DownloadRequest>(), It.IsAny<string>()))
                .Returns(mockBackgroundService.Object);


            await TestService.Download(fakeRequest, mockContext);

            MockDownloaderBackgroundRunnerFactory.Verify(x => x.CreateRunner(mockDownloader.Object, fakeRequest, fakeFileName), Times.Once());
        }

        [Fact]
        public async Task DownloadService_WhenGivenRequest_CallsDownloaderBackgroundRunnerWithRequest()
        {
            var fakeFileName = "fakefile";
            var mockDownloader = new Mock<IDownloader>();
            MockDownloaderSelector.Setup(x => x.Select(It.IsAny<DownloadRequest>())).Returns(mockDownloader.Object);
            var expectedCancellationToken = new CancellationTokenSource().Token;
            var mockContext = TestServerCallContext.Create(null, null, DateTime.MaxValue, null, expectedCancellationToken, null, null, null, null, null, null);
            var fakeRequest = new DownloadRequest()
            {
                Url = $"http://fake-url.com/{fakeFileName}"
            };
            var mockBackgroundService = new Mock<DownloadBackgroundRunner>(mockDownloader.Object, fakeRequest, fakeFileName);
            MockDownloaderBackgroundRunnerFactory.Setup(x => x.CreateRunner(mockDownloader.Object, It.IsAny<DownloadRequest>(), It.IsAny<string>()))
                .Returns(mockBackgroundService.Object);


            await TestService.Download(fakeRequest, mockContext);

            mockBackgroundService.Verify(downloader => downloader.StartAsync(expectedCancellationToken));
        }
    }
}