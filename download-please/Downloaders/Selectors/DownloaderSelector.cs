namespace download_please.Downloaders.Selectors
{
    public class DownloaderSelector : IDownloaderSelector
    {
        private readonly IServiceProvider _serviceProvider;

        public DownloaderSelector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDownloader Select(DownloadRequest request) =>
            request.Url switch
            {
                _ => _serviceProvider.GetRequiredService<HttpFileDownloader>(),
            };
    }
}
