namespace download_please.Utils
{
    public static class StatusReplyExtensions
    {
        public static StatusReply AddDownloadReplies(this StatusReply statusReply, IEnumerable<DownloadReply> downloadReplies) {
            foreach (var downloadReply in downloadReplies)
            {
                statusReply.Statuses.Add(downloadReply);
            }
            return statusReply;
        }
    }
}
