namespace Terrasoft.DevEnvironment.Commands {
	using Managers;

	public class PrepareDevDatabaseCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Prepare database");
			var tsManager = new TerrasoftManager();
			var prepareDbScript = tsManager.CreatePrepareDevDatabaseScript(context.Settings.PackageStorePath);
			var dbManager = new DbManager();
			dbManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
			dbManager.DataBase = Context.DatabaseName;
			dbManager.ExecSqlScript(prepareDbScript);
			Logger.WriteCommandSuccess();
		}

	}

}
