namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class EnableFileDesignModeCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Enable file design mode");
			var tsManager = new TerrasoftManager();
			var weebConfigFilePath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName,
				BpmonlineConstants.WebAppLoaderWebConfigRelativePath);
			tsManager.UpadteFileDesignMode(weebConfigFilePath, true);
			Logger.WriteCommandSuccess();
		}

	}

}
