namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class UpdateIntegrationConnectionStringCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Update connection string for Integration Unit Tests");
			var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
			var tsManager = new TerrasoftManager();
			var configPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.ConfigurationIntegrationConnectionStringRelativePath);
			var connectionString = DbManager.CreateConnectionString(Context.Settings.MSSSQLConnectionString, Context.DatabaseName);
			tsManager.UpdateIntegrationConnectionString(configPath, connectionString);
			Logger.WriteCommandSuccess();
		}

	}

}
