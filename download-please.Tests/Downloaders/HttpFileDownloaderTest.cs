using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Services;
using System.Text;

namespace download_please.Tests.Downloaders
{
    public class HttpFileDownloaderTest
    {

        private Mock<HttpMessageHandler> MockHandler { get; }
        private HttpFileDownloader TestService { get; }

        public HttpFileDownloaderTest()
        {
            MockHandler = new Mock<HttpMessageHandler>();
            MockHandler.SetupAnyRequest().ReturnsResponse(System.Net.HttpStatusCode.OK);
            TestService = new HttpFileDownloader(MockHandler.CreateClient());

        }

        [Fact]
        public async Task HttpFileDownloader_WhenGivenRequest_SendsRequestToUrl()
        {
            var fakeUrl = "http://fake.url";
            var fakeRequest = new DownloadRequest()
            {
                Url = fakeUrl
            };
            var fakeStream = new MemoryStream();

            await TestService.Download(fakeRequest, fakeStream);

            MockHandler.VerifyRequest(fakeUrl);
        }

        [Fact]
        public async Task HttpFileDownloader_WhenGivenRequest_CopiesHttpContentToStream()
        {
            var fakeUrl = "http://fake.url";
            var fakeContent = "fake content";
            MockHandler.SetupAnyRequest().ReturnsResponse(System.Net.HttpStatusCode.OK, fakeContent);
            var fakeRequest = new DownloadRequest()
            {
                Url = fakeUrl
            };
            var fakeStream = new MemoryStream();

            await TestService.Download(fakeRequest, fakeStream);

            var actual = Encoding.UTF8.GetString(fakeStream.ToArray());

            actual.Should().BeEquivalentTo(fakeContent);
        }

        [Fact]
        public async Task HttpFileDownloader_WhenGivenRequest_ReturnsDownloadReply()
        {
            var fakeUrl = "http://fake.url";
            var fakeRequest = new DownloadRequest()
            {
                Url = fakeUrl
            };
            var fakeStream = new MemoryStream();

            var expected = new DownloadReply()
            {
                Status = "Downloading",
            };

            var actual = await TestService.Download(fakeRequest, fakeStream);

            actual.Should().BeEquivalentTo(expected);

        }
    }
}
