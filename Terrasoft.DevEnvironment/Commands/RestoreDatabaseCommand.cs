namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System;
	using System.IO;
	using System.Linq;

	public class RestoreDatabaseCommand : BaseCommand {

		private string CreateSharedBakFilePath(string sharedDirectory) {
			var randomFileName = Path.GetRandomFileName().Replace(".", string.Empty) + ".bak";
			var tempbakfile = Path.Combine(sharedDirectory, randomFileName);
			return tempbakfile;
		}

		private string ShareDatabaseBakFile(string bakFilePath) {
			var tempSharedBakFilePath = CreateSharedBakFilePath(Context.Settings.SharedDirectoryPath);
			File.Copy(bakFilePath, tempSharedBakFilePath);
			return tempSharedBakFilePath;
		}

		private void DeleteFile(string path) {
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}

		private string GetUserName() {
			string userName = string.Empty;
			string fullName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			var nameBox = fullName.Replace(@"\\", @"\").Split('\\');
			if (nameBox.Count() == 2) {
				userName = nameBox[1];
			}
			if (nameBox.Count() == 1) {
				userName = nameBox[0];
			}
			return userName;
		}

		private string CreateDatabaseName() {
			var userName = GetUserName();
			var databaseName = Context.Settings.DatabaseNamePattern;
			databaseName = databaseName.Replace("##username##", userName);
			databaseName = databaseName.Replace("##projectname##", Context.ProjectDirectoryName);
			if (string.IsNullOrEmpty(databaseName)) {
				throw new InvalidOperationException("Can't create database name");
			}
			return databaseName;
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Restore database");
			var tsManager = new TerrasoftManager();
			var originalBakFilePath = tsManager.GetDatabaseBackupFilePath(Context.TempUnzippedBuildDirectory);
			var tempSharedBakFilePath = ShareDatabaseBakFile(originalBakFilePath);
			var dbManager = new DbManager();
			dbManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
			var databaseName = CreateDatabaseName();
			var restoredDatabaseName = dbManager.RestoreBpmonlineDbWithUniqueName(tempSharedBakFilePath, databaseName);
			Context.DatabaseName = restoredDatabaseName;
			DeleteFile(tempSharedBakFilePath);
			Logger.WriteCommandAddition($"Database name: {restoredDatabaseName}");
			var cleanManager = new CleanManager(context, Logger);
			cleanManager.CleanTempUnzippedBuildDirectory();
			Logger.WriteCommandSuccess();
		}

	}

}
