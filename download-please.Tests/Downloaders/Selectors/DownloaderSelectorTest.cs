using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using YoutubeExplode;

namespace download_please.Tests.Downloaders.Selectors
{
    public class DownloaderSelectorTest
    {

        private IServiceCollection FakeServiceCollection { get; }
        private IServiceProvider FakeServiceProvider { get; }
        private DownloaderSelector TestService { get; }

        public DownloaderSelectorTest() 
        { 
            FakeServiceCollection = new ServiceCollection();
            FakeServiceCollection.AddHttpClient();
            FakeServiceCollection.AddSingleton<HttpFileDownloader>();
            FakeServiceCollection.AddSingleton<YoutubeAudioDownloader>();
            FakeServiceCollection.AddSingleton<IFileSystem, MockFileSystem>();
            FakeServiceCollection.AddSingleton<IFileUtils,  FileUtils>();
            FakeServiceCollection.AddSingleton<IYoutubeDownloadUtils, YoutubeDownloadUtils>();
            FakeServiceCollection.AddSingleton<YoutubeClient, YoutubeClient>();
            FakeServiceProvider = FakeServiceCollection.BuildServiceProvider();
            TestService = new DownloaderSelector(FakeServiceProvider);
        }

        [Fact]
        public void DownloaderSelector_WhenGivenRequest_ReturnsHttpFileDownloaderFromServiceProvider()
        {
            var testRequest = new DownloadRequest()
            {
                Url = "https://unrecognised-url.com"
            };

            var actual = TestService.Select(testRequest);

            actual.Should().BeOfType<HttpFileDownloader>();
        }

        [Fact]
        public void DownloaderSelector_WhenGivenRequestWithYoutubeDomain_ReturnsYoutubeDownloaderFromServiceProvider()
        {
            var testRequest = new DownloadRequest()
            {
                Url = "https://www.youtube.com/watch?v=oA0CpI0vCK4"
            };

            var actual = TestService.Select(testRequest);

            actual.Should().BeOfType<YoutubeAudioDownloader>();
        }
    }
}
