using download_please.Utils;

namespace download_please.Tests.Utils
{
    public class DownloadStatusUtilsTest
    {
        [Fact]
        public void GetStatusFromProgress_WhenGivenZero_ReturnsNotStarted()
        {
            DownloadStatusUtils.GetStatusFromProgress(0).Should().Be(DownloadStatus.NotStarted);
        }

        [Fact]
        public void GetStatusFromProgress_WhenGivenNumberBetween0And100_ReturnsDownloading()
        {
            DownloadStatusUtils.GetStatusFromProgress(50).Should().Be(DownloadStatus.Downloading);
        }

        [Fact]
        public void GetStatusFromProgress_WhenGiven100_ReturnsFinishedDownloading()
        {
            DownloadStatusUtils.GetStatusFromProgress(100).Should().Be(DownloadStatus.FinishedDownloading);
        }

        [Fact]
        public void GetStatusFromProgress_WhenGivenNegativeNumber_ReturnsError()
        {
            DownloadStatusUtils.GetStatusFromProgress(-1).Should().Be(DownloadStatus.Error);
        }

    }
}
