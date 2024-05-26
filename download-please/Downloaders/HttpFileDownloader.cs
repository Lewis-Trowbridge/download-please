namespace download_please.Downloaders
{
    public class HttpFileDownloader : IDownloader
    {
        private HttpClient _httpClient;
        public HttpFileDownloader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DownloadReply> Download(DownloadRequest request, Stream localFileStream) { 
            var internetStream = await _httpClient.GetStreamAsync(request.Url);
            internetStream.CopyToAsync(localFileStream);
            return new DownloadReply
            {
                Status = "Downloading"
            };
        }
    }
}
