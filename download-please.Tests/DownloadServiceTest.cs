using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Services;
using Grpc.Core;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace download_please.Tests
{
    public class DownloadServiceTest : IDisposable
    {
        private Mock<IDownloaderSelector> MockDownloaderSelector;
        private IFileSystem FakeFileSystem { get; }
        private string HomeDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private string FakeDirectory { get; } = "/fake/folder";
        private DownloadService TestService { get; }

        public DownloadServiceTest() 
        {
            MockDownloaderSelector = new Mock<IDownloaderSelector>();
            FakeFileSystem = new MockFileSystem();
            FakeFileSystem.Directory.CreateDirectory(HomeDirectory);
            FakeFileSystem.Directory.CreateDirectory(FakeDirectory);
            Environment.SetEnvironmentVariable(DownloadService.DOWNLOAD_PLEASE_DIR, FakeDirectory);
            TestService = new DownloadService(MockDownloaderSelector.Object, FakeFileSystem);
        }

        [Fact]
        public async Task DownloadService_WhenEnvironmentVariableIsFound_CreatesFileInGivenDirectory()
        {
            var fakeFileName = "fakefile";
            var mockDownloader = new Mock<IDownloader>();
            MockDownloaderSelector.Setup(x => x.Select(It.IsAny<DownloadRequest>())).Returns(mockDownloader.Object);
            var mockContext = Mock.Of<ServerCallContext>();
            var fakeRequest = new DownloadRequest()
            {
                Url = $"http://fake-url.com/{fakeFileName}"
            };

            await TestService.Download(fakeRequest, mockContext);

            FakeFileSystem.File.Exists($"{FakeDirectory}{Path.DirectorySeparatorChar}{fakeFileName}").Should().BeTrue();
        }

        [Fact]
        public async Task DownloadService_WhenEnvironmentVariableIsNotFound_CreatesFileInHomeDirectory()
        {
            Environment.SetEnvironmentVariable(DownloadService.DOWNLOAD_PLEASE_DIR, null);
            var fakeFileName = "fakefile";
            var mockDownloader = new Mock<IDownloader>();
            MockDownloaderSelector.Setup(x => x.Select(It.IsAny<DownloadRequest>())).Returns(mockDownloader.Object);
            var mockContext = Mock.Of<ServerCallContext>();
            var fakeRequest = new DownloadRequest()
            {
                Url = $"http://fake-url.com/{fakeFileName}"
            };

            await TestService.Download(fakeRequest, mockContext);

            FakeFileSystem.File.Exists($"{HomeDirectory}{Path.DirectorySeparatorChar}{fakeFileName}").Should().BeTrue();
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

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(DownloadService.DOWNLOAD_PLEASE_DIR, null);
        }
    }
}