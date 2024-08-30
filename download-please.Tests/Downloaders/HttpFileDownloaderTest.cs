using download_please.Downloaders;
using download_please.Utils;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace download_please.Tests.Downloaders
{
    public class HttpFileDownloaderTest
    {

        private Mock<HttpMessageHandler> MockHandler { get; }
        private Mock<IFileUtils> MockFileUtils { get; }
        private MemoryStream FakeStream { get; }
        private HttpFileDownloader TestService { get; }

        public HttpFileDownloaderTest()
        {
            MockHandler = new Mock<HttpMessageHandler>();
            MockHandler.SetupAnyRequest().ReturnsResponse(System.Net.HttpStatusCode.OK);
            FakeStream = new MemoryStream();
            MockFileUtils = new Mock<IFileUtils>();
            MockFileUtils.Setup(x => x.CreateFile(It.IsAny<string>())).Returns(FakeStream);
            TestService = new HttpFileDownloader(MockHandler.CreateClient(), MockFileUtils.Object);
        }

        [Fact]
        public async Task HttpFileDownloader_WhenGivenRequest_SendsRequestToUrl()
        {
            var fakeUrl = "http://fake.url";
            var fakeRequest = new DownloadRequest()
            {
                Url = fakeUrl
            };

            await TestService.Download(fakeRequest, "");

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

            await TestService.Download(fakeRequest, "");

            var actual = Encoding.UTF8.GetString(FakeStream.ToArray());

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

            var expected = new DownloadReply()
            {
                Status = "Downloaded"
            };

            var actual = await TestService.Download(fakeRequest, "");

            actual.Should().BeEquivalentTo(expected);

        }
    }
}
