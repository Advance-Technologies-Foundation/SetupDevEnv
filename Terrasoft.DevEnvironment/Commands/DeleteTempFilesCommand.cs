namespace Terrasoft.DevEnvironment.Commands {
	using System.IO;

	public class DeleteTempFilesCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Clean temp data");
			if (File.Exists(Context.LocalCopyBuildPath)) {
				Logger.WriteCommandAddition($"Remove: {Context.LocalCopyBuildPath} ");
				File.Delete(Context.LocalCopyBuildPath);
			}
			if (Directory.Exists(Context.TempUnzippedBuildDirectory)) {
				Logger.WriteCommandAddition($"Remove: {Context.TempUnzippedBuildDirectory} ");
				Directory.Delete(Context.TempUnzippedBuildDirectory, true);
			}
			Logger.WriteCommandSuccess();
		}

	}

}
