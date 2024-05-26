using download_please.Downloaders;
using download_please.Downloaders.Selectors;
using download_please.Services;

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
        public async void HttpFileDownloader_WhenGivenRequest_SendsRequestToUrl()
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
    }
}
