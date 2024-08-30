using download_please.Utils;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace download_please.Tests.Utils
{
    public class FileUtilsTest : IDisposable
    {
        private FileUtils FileUtils { get; }
        private IFileSystem FakeFileSystem { get; }
        private string HomeDirectory { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private string FakeDirectory { get; } = "/fake/folder";

        public FileUtilsTest() {
            FakeFileSystem = new MockFileSystem();
            FakeFileSystem.Directory.CreateDirectory(HomeDirectory);
            FakeFileSystem.Directory.CreateDirectory(FakeDirectory);
            FileUtils = new FileUtils(FakeFileSystem);
            Environment.SetEnvironmentVariable(FileUtils.DOWNLOAD_PLEASE_DIR, FakeDirectory);
        }

        [Fact]
        public void FileUtils_WhenEnvironmentVariableIsFound_CreatesFileInGivenDirectory()
        {
            var fakeFileName = "fakefile";

            FileUtils.CreateFile(fakeFileName);

            FakeFileSystem.File.Exists($"{FakeDirectory}{Path.DirectorySeparatorChar}{fakeFileName}").Should().BeTrue();
        }
        
        [Fact]
        public void FileUtils_WhenEnvironmentVariableIsNotFound_CreatesFileInHomeDirectory()
        {
            Environment.SetEnvironmentVariable(FileUtils.DOWNLOAD_PLEASE_DIR, null);
            var fakeFileName = "fakefile";

            FileUtils.CreateFile(fakeFileName);

            FakeFileSystem.File.Exists($"{HomeDirectory}{Path.DirectorySeparatorChar}{fakeFileName}").Should().BeTrue();
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(FileUtils.DOWNLOAD_PLEASE_DIR, null);
        }
    }
}
