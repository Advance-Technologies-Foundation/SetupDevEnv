namespace Terrasoft.DevEnvironment.Commands {
	using System.Collections.Generic;
	using System.IO;
	using Terrasoft.DevEnvironment.Managers;

	public class InstallTsqltCommand : BaseCommand {

		private List<string> GetFilesContent(string directoryPath) {
			var result = new List<string>();
			var files = Directory.GetFiles(directoryPath);
			foreach (var file in files) {
				result.Add(File.ReadAllText(file));
			}
			return result;
		}

		private List<string> GetInstallScripts() {
			var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
			var fileManager = new FileManager();
			var tempDirectoryPath = fileManager.CreateTempFolder();
			svnManager.Checkout(Context.Settings.TSqltPath, tempDirectoryPath);
			List<string> contents = GetFilesContent(tempDirectoryPath);
			Directory.Delete(tempDirectoryPath, true);
			return contents;
		}

		private void ExecuteInstallScripts(List<string> scripts) {
			var databaseManager = new DbManager();
			databaseManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
			databaseManager.DataBase = Context.DatabaseName;
			foreach (var item in scripts) {
				databaseManager.ExecSqlScript2(item);
			}
		}

		protected override void InternalExecute(Context context) {
			List<string> scripts = GetInstallScripts();
			ExecuteInstallScripts(scripts);
		}

	}

}
