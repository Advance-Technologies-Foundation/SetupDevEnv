namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class EnableFileDesignModeWorkspaceConsoleCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Enable file design mode");
			var tsManager = new TerrasoftManager();
			var appConfigPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, BpmonlineConstants.WorkspaceConsoleAppConfigRelativePath);
			tsManager.UpdateFileDesignModeWorkspaceConsole(appConfigPath, true);
			Logger.WriteCommandSuccess();
		}

	}

}
