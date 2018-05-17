namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System;
	using System.IO;

	public class ValidationCommand : BaseCommand {

		private void ValidateRootProjectPath() {
			if (string.IsNullOrEmpty(Context.Settings.ProjectsPath)) {
				throw new SoftCommonException("Please, set 'ProjectsPath' setting in config file");
			}
			if (!Directory.Exists(Context.Settings.ProjectsPath)) {
				Directory.CreateDirectory(Context.Settings.ProjectsPath);
				Logger.WriteCommandAddition($"Directory '{Context.Settings.ProjectsPath}' has been created");
			}
		}

		private void ValidateMsBuild() {
			if (!File.Exists(CommonConstants.MsBuildPath)) {
				throw new SoftCommonException($"MsBuild wasn't fined by path {CommonConstants.MsBuildPath}." +
					" Please install MsBuild and try again.");
			}
		}

		private void ValidateInfrastructureConsole() {
			var infrastructureConsoleDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, InfrastructureConsoleConstants.DirectoryName);
			if (Directory.Exists(infrastructureConsoleDirectoryPath)) {
				var consoleFilePath = Path.Combine(infrastructureConsoleDirectoryPath, InfrastructureConsoleConstants.ConsoleRelativePath);
				if (!File.Exists(consoleFilePath)) {
					Directory.Delete(infrastructureConsoleDirectoryPath, true);
				}
			}
			if (!Directory.Exists(infrastructureConsoleDirectoryPath)) {
				Directory.CreateDirectory(infrastructureConsoleDirectoryPath);
				Logger.WriteCommandAddition($"Directory '{infrastructureConsoleDirectoryPath}' has been created");
				var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
				svnManager.Checkout(Context.Settings.InfrastructureConsolePath, infrastructureConsoleDirectoryPath);
			}
			

		}

		private void ValidateDatabaseServer() {
			try {
				var dbManager = new DbManager();
				dbManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
				dbManager.Ping();
			} catch {
				throw new SoftCommonException("Can'not ping DB server");
			}
		}

		private void ValidateDfsServer() {
			try {
				Directory.GetFiles(Context.Settings.DfsBuildsDirectoryPath);
			} catch (Exception ex) {
				throw new SoftCommonException("Can'not ping DFS server. Check access to " +
					$"'{Context.Settings.DfsBuildsDirectoryPath} ({ex.Message})");
			}
		}
		private void ValidateShare() {
			try {
				var tempFileName = Path.GetRandomFileName();
				var path = Path.Combine(Context.Settings.SharedDirectoryPath, tempFileName);
				File.AppendAllText(path, "Ping");
				File.Delete(path);
			} catch (Exception ex) {
				throw new SoftCommonException($@"Can'not ping shared server. Check access to '{Context.Settings.SharedDirectoryPath} ({ex.Message})");
			}
		}

		private void ValidateSvn() {
			try {
				var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
				svnManager.Ping(Context.Settings.CorePath);
			} catch (Exception ex) {
				throw new SoftCommonException($@"Can'not ping svn server. Check your credentials. '{ex.Message}'");
			}
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Start validation");
			ValidateRootProjectPath();
			ValidateInfrastructureConsole();
			ValidateDatabaseServer();
			ValidateDfsServer();
			ValidateShare();
			ValidateSvn();
			Logger.WriteCommandSuccess();
		}

	}

}
