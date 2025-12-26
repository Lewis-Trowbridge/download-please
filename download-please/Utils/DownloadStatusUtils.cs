namespace download_please.Utils
{
    public static class DownloadStatusUtils
    {
        public static DownloadStatus GetStatusFromProgress(double progress) =>
            progress switch
            {
                var p when p == 0d => DownloadStatus.NotStarted,
                var p when p < 100d && p > 0 => DownloadStatus.Downloading,
                var p when p == 100d => DownloadStatus.FinishedDownloading,
                _ => DownloadStatus.Error,
            };
    }
}
