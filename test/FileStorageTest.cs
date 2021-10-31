using System.IO;
using Xunit;
using Tusba.Components.FileSystem;

namespace test
{
	public class FileStorageTest : BaseTest
	{
		[Fact]
		public void ProvideNonExistingDirectoryTest()
		{
			string dirName = @"dir-test";
			string dirPath = Path.Combine(baseDir, dirName);

			Assert.False(Directory.Exists(dirPath));

			for (int i = 0; i < 2; i++) {
				Assert.True(FileStorage.ProvideDirectory(dirName));
				Assert.True(Directory.Exists(dirPath));
			}

			Directory.Delete(dirPath);
		}

		[Fact]
		public void ProvideExistingDirectoryTest()
		{
			string dirName = @"test";
			string dirPath = Path.Combine(baseDir, dirName);

			Assert.True(Directory.Exists(dirPath));

			for (int i = 0; i < 2; i++) {
				Assert.True(FileStorage.ProvideDirectory(dirName));
				Assert.True(Directory.Exists(dirPath));
			}
		}
	}
}
