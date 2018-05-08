namespace Terrasoft.DevEnvironment.Commands {
	using Managers;

	public class UnzipBuildCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Unzip build");
			var fileManager = new FileManager();
			var tempFolder = fileManager.CreateTempFolder();
			Logger.WriteCommandAddition($"Destination path: {tempFolder}");
			fileManager.Unzip(Context.LocalCopyBuildPath, tempFolder);
			Context.TempUnzippedBuildDirectory = tempFolder;
			var cleanManager = new CleanManager(context, Logger);
			cleanManager.CleanLocalCopyBuild();
			Logger.WriteCommandSuccess();
		}

	}

}
