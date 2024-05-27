using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Services;
using Grpc.Core;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace download_please.Tests
{
    public class DownloadServiceTest
    {
        private Mock<IDownloaderSelector> MockDownloaderSelector;
        private IFileSystem FakeFileSystem { get; }
        private DownloadService TestService;

        public DownloadServiceTest() 
        {
            MockDownloaderSelector = new Mock<IDownloaderSelector>();
            FakeFileSystem = new MockFileSystem();
            FakeFileSystem.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            TestService = new DownloadService(MockDownloaderSelector.Object, FakeFileSystem);
        }

        [Fact]
        public async Task DownloadService_WhenGivenRequest_CallsDownloaderServiceWithRequest()
        {
            var mockDownloader = new Mock<IDownloader>();
            MockDownloaderSelector.Setup(x => x.Select(It.IsAny<DownloadRequest>())).Returns(mockDownloader.Object);
            var mockContext = Mock.Of<ServerCallContext>();
            var fakeRequest = new DownloadRequest()
            {
                Url = "http://fake-url.com/fakefile"
            };

            await TestService.Download(fakeRequest, mockContext);

            mockDownloader.Verify(downloader => downloader.Download(fakeRequest, It.IsAny<MockFileStream>()));
        }
    }
}