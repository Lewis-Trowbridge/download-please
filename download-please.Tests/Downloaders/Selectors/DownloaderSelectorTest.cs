using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

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
            FakeServiceCollection.AddSingleton<IFileSystem, MockFileSystem>();
            FakeServiceCollection.AddSingleton<IFileUtils,  FileUtils>();
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
    }
}
