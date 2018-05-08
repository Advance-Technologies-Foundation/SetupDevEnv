namespace Terrasoft.DevEnvironment.Managers {
	using System.IO;

	public class FileManager
	{
		public void Unzip(string zipPath, string extractPath) {
			System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
		}

		public string CreateTempFolder() {
			var rootTempPath = Path.GetTempPath();
			var thisTempPath = Path.GetRandomFileName();
			var fullTempPath = Path.Combine(rootTempPath, $"mng_{thisTempPath}");
			Directory.CreateDirectory(fullTempPath);
			return fullTempPath;
		}

	}

}
