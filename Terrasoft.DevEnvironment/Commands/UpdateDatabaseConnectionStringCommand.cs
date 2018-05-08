namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class UpdateDatabaseConnectionStringCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Update database connection string");
			var tsManager = new TerrasoftManager();
			var connectionStringPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName,
				BpmonlineConstants.WebAppLoaderConnectionStringsRelativePath);
			var connectionString = Context.Settings.MSSSQLConnectionString.Replace("##dbname##", Context.DatabaseName);
			tsManager.UpdateDbConnectionString(connectionStringPath, connectionString);
			Logger.WriteCommandSuccess();
		}

	}

}
