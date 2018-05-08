namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System;
	using System.Collections.Generic;
	using System.Linq;


	public class UnlockDevPackagesInDatabaseCommand : BaseCommand {

		private List<string> SplitPackegesSetting(string value) {
			if (!string.IsNullOrEmpty(value)) {
				var separators = new char[] { ',', ' ', ';' };
				var packages = value.Split(separators, StringSplitOptions.RemoveEmptyEntries)
					.Distinct().ToList();
				return packages;
			}
			return new List<string>();
		}

		private List<string> GetDevPackages() {
			if (string.Equals(Context.Settings.Packages.Trim(), "*")) {
				var databaseManager = new DbManager();
				databaseManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
				databaseManager.DataBase = Context.DatabaseName;
				var tsManager = new TerrasoftManager();
				var pkg = tsManager.GetPackeges(databaseManager);
				return pkg;
			}
			return SplitPackegesSetting(Context.Settings.Packages);
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Prepare packages");
			var tsManager = new TerrasoftManager();
			var devPackages = GetDevPackages();
			var dbManager = new DbManager();
			dbManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
			dbManager.DataBase = Context.DatabaseName;
			foreach (var package in devPackages) {
				var preparePackageScript = tsManager.CreatePrepareDevPackageScript(package);
				dbManager.ExecSqlScript(preparePackageScript);
				Logger.WriteCommandAddition($"{package} ");
			}
			Logger.WriteCommandSuccess();
		}

	}

}
