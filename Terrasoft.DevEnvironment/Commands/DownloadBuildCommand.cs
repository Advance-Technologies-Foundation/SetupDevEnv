namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class DownloadBuildCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Download build");
			var fileManager = new FileManager();
			var localCopyBuildPath = Path.Combine(fileManager.CreateTempFolder(), Path.GetFileName(Context.DfsBuildPath));
			Logger.WriteCommandAddition($"Destination path: {localCopyBuildPath}");
			File.Copy(Context.DfsBuildPath, localCopyBuildPath);
			Context.LocalCopyBuildPath = localCopyBuildPath;
			Logger.WriteCommandSuccess();
		}

	}

}
