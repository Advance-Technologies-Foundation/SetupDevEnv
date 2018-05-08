namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class SetWorkspaceConsolePrefer32BitCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Set WorkspaceConsole prefer 32 bit");
			var tsManager = new TerrasoftManager();
			var workspaceConsoleCsprojPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, BpmonlineConstants.WorkspaceConsoleCsprojRelativePath);
			Logger.WriteCommandAddition($"File path: {workspaceConsoleCsprojPath}");
			tsManager.SetWorkspaceConsolePrefer32Bit(workspaceConsoleCsprojPath, true);
			Logger.WriteCommandSuccess();
		}

	}

}
