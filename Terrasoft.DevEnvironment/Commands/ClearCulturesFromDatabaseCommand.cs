using Terrasoft.DevEnvironment.Managers;

namespace Terrasoft.DevEnvironment.Commands
{
	public class ClearCulturesFromDatabaseCommand : BaseCommand
	{
		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Delete all cultures except ru-RU and en-US");
			var tsManager = new TerrasoftManager();
			var script = tsManager.GetClearCultureScript();
			var dbManager = new DbManager();
			dbManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
			dbManager.DataBase = Context.DatabaseName;
			dbManager.ExecSqlScript(script);
			Logger.WriteCommandSuccess();
		}

	}
}