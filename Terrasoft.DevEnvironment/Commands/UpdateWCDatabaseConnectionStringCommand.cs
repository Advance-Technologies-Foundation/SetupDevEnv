namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class UpdateWCDatabaseConnectionStringCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Update WorkspaceConsole connection string");
			var tsManager = new TerrasoftManager();
			var workspaceConsoleAppConfigPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, BpmonlineConstants.WorkspaceConsoleAppConfigRelativePath);
			var connectionString = DbManager.CreateConnectionString(Context.Settings.MSSSQLConnectionString, Context.DatabaseName);
			tsManager.UpdateWCConnectionString(workspaceConsoleAppConfigPath, connectionString);
			Logger.WriteCommandSuccess();
		}

	}

}
