using System.IO.Abstractions;

namespace download_please.Utils
{
    public class FileUtils : IFileUtils
    {
        public static readonly string DOWNLOAD_PLEASE_DIR = "DOWNLOAD_PLEASE_DIR";
        private readonly IFileSystem _fileSystem;


        public FileUtils(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Stream CreateFile(string filename)
        {
            var homeDirectory = Environment.GetEnvironmentVariable(DOWNLOAD_PLEASE_DIR) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return _fileSystem.File.Create($"{homeDirectory}{Path.DirectorySeparatorChar}{filename}");
        }
    }
}
